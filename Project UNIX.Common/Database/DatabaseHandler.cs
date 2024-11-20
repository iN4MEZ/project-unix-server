using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Project_UNIX.Common.Database
{
    public class DatabaseHandler
    {
        private readonly ILogger _logger;
        public IMongoDatabase mongoDb { get; private set; }

        public DatabaseHandler(ILogger<DatabaseHandler> logger) {

            try
            {
                _logger = logger;
                MongoClient client;
                client = new MongoClient("mongodb://localhost:27017");
                if(client != null)
                {

                    _logger.LogInformation($"Database Connected!");

                    mongoDb = client.GetDatabase("ProjectUNIX");

                    mongoDb.CreateCollection("accounts");

                    mongoDb.CreateCollection("avatars");
                } else
                {
                    throw new Exception();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Unable To Connect database {ex}");
            }
        }    

    }
}
