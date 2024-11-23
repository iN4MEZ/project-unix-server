using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Project_UNIX.Common.Database.Models;
using Project_UNIX.Common.Server;
using Project_UNIX.Common.Validator.Token;

namespace Project_UNIX.SDKServer.Network.RoutesController
{
    internal class LoginController : IRoutesController
    {
        private readonly Account _account;

        private readonly TokenValidation _tokenValidation;

        private readonly ILogger _logger;

        public LoginController(ILogger<LoginController> logger, Account account, TokenValidation tokenValidation) { 
            _logger = logger;
            _account = account;
            _tokenValidation = tokenValidation;
        }

        public async Task<Task> Handle(HttpContext httpContext)
        {

            using (StreamReader reader = new StreamReader(httpContext.Request.Body))
            {
                var reqBody = await reader.ReadToEndAsync();

                AccountModel accountModelJson = JsonConvert.DeserializeObject<AccountModel>(reqBody);

                bool passAuthentication = await _account.Authentication(accountModelJson.username!, accountModelJson.password!);


                if (passAuthentication)
                {
                    AccountModel target = _account.GetAccountByUsername(accountModelJson.username);

                    httpContext.Response.Headers.Add("Token",target.token);

                    var rsp = new { msg = "OK" };

                    var jsonDoc = System.Text.Json.JsonSerializer.Serialize(rsp);

                    await httpContext.Response.WriteAsync(jsonDoc);
                } else
                {
                    var rsp = new { msg = "Error Username or Password Incorrect!" };

                    var jsonDoc = System.Text.Json.JsonSerializer.Serialize(rsp);

                    await httpContext.Response.WriteAsync(jsonDoc);
                }
            }

            return Task.CompletedTask;
        }
    }
}
