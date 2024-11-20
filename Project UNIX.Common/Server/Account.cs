using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Project_UNIX.Common.Database;
using Project_UNIX.Common.Database.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Project_UNIX.Common.Server
{
    public class Account
    {

        private readonly DatabaseHandler _databaseHandler;
        private readonly ILogger<Account> _logger;
        private readonly JwtSecurityTokenHandler tokenHandler;

        private IMongoCollection<BsonDocument> accountTable;

        private IMongoDatabase mongoDatabase;
        public Account(ILogger<Account> logger,DatabaseHandler databaseHandler)
        {
            _logger = logger;
            tokenHandler = new JwtSecurityTokenHandler();
            _databaseHandler = databaseHandler;
            mongoDatabase = _databaseHandler.mongoDb;
            accountTable = mongoDatabase.GetCollection<BsonDocument>("accounts");
        }

        #region Register
        public async Task CreateAccount(string username, string password)
        {
            try
            {

                var usedUsername = IsUsernameInDatabase(username);

                if (usedUsername) { _logger.LogInformation("Username Already Use!"); return; }

                // ## Generate to prepare Data

                string userSecret = Create256BitKey();

                var uidCounter = accountTable.EstimatedDocumentCount() + 1000;

                var uid = Interlocked.Increment(ref uidCounter);

                var token = GenerateJwtToken(userSecret, new { userId = uid, userName = username });    // GenerateEncryptedToken(username!, uid, userSecret);

                AccountModel accountObject = new()
                { username = username, password = password , createDate = DateTime.UtcNow, uid = uid, secret = userSecret,  token = token};

                await accountTable.InsertOneAsync(accountObject.ToBsonDocument());

                _logger.LogInformation("Create Account Success!");


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private string GenerateEncryptedToken(string ua,long uid, string secretKey)
        {
            string token = GenerateJwtToken(secretKey, new { userId = uid, userName = ua}) ?? throw new NullReferenceException();

            Project_UNIX.Common.Crypto.RSA.InitializeEncryption();

            var encryptedTokenByte = Project_UNIX.Common.Crypto.RSA.EncryptWithPublicKey(token, Project_UNIX.Common.Crypto.RSA.publicKey);

            var encryptedTokenB64 = Convert.ToBase64String(encryptedTokenByte);

            var jsonObject = new
            {
                Token = encryptedTokenB64,
            };

            var jsonDocument = JsonSerializer.Serialize(jsonObject);

            var b64json = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonDocument));

            return b64json;
        }
        private string GenerateJwtToken(string secretKey, object payload)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

                var claims = new List<Claim>();

                foreach(var prop in payload.GetType().GetProperties()) {
                    claims.Add(new Claim(prop.Name,prop.GetValue(payload)!.ToString()!));
                }

                var token = new JwtSecurityToken(
                    issuer: "Unix",
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1), // Token expiration time
                    signingCredentials: credentials
                ); ;

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }

        }
        private string Create256BitKey()
        {
            // สร้าง byte array ขนาด 32 bytes (256 bits)
            byte[] keyBytes = new byte[32];

            // สร้าง random bytes ให้กับ keyBytes
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(keyBytes);
            }

            // แปลง byte array เป็น hex string
            string keyHex = BitConverter.ToString(keyBytes).Replace("-", "");

            return keyHex;
        }
        #endregion


        #region Authentication

        public async ValueTask<bool> Authentication(string username,string password)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("username", username) & Builders<BsonDocument>.Filter.Eq("password", password);
                var user = accountTable.Find(filter).FirstOrDefault();

                if (user != null)
                {
                    var tokenFilter = Builders<BsonDocument>.Filter.Eq("_id", user["_id"].AsObjectId!);

                    var newToken = GenerateJwtToken(user?["secret"].AsString!, new { userId = user?["uid"].ToInt64(), userName = username });

                    var update = Builders<BsonDocument>.Update.Set("token", newToken);

                    var result = await accountTable.UpdateOneAsync(tokenFilter, update);

                    return ValueTask.FromResult(result).IsCompletedSuccessfully;
                }
                else
                {
                    return false;
                }
            } catch
            {
                throw new Exception("Auth Error");
            }
        }

        #endregion

        #region Reusble Private Method

        private bool IsUsernameInDatabase(string username)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("username", username);
            var existingUser = accountTable.Find(filter).FirstOrDefault();

            return existingUser == null ? false : true;
        }
        #endregion

        #region Reusble Public Method


        public string GetTokenByUid(int id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("uid", id);
            var uid = accountTable.Find(filter).FirstOrDefault();

            return uid?["token"].AsString!;
        }

        public AccountModel GetAccountByUid(int uid)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("uid", uid);
            var account = accountTable.Find(filter).FirstOrDefault();

            return BsonSerializer.Deserialize<AccountModel>(account) ?? throw new ArgumentNullException();
        }

        public AccountModel GetAccountByUsername(string un)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("username", un);
            var account = accountTable.Find(filter).FirstOrDefault();

            return BsonSerializer.Deserialize<AccountModel>(account) ?? throw new ArgumentNullException();
        }

        #endregion

    }

}
