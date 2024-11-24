using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Project_UNIX.Common.Database.Models;
using Project_UNIX.Common.Server;

namespace Project_UNIX.SDKServer.Network.RoutesController
{
    internal class RegisterController : IRoutesController
    {
        private readonly Account _account;

        public RegisterController(Account account)
        {
            _account = account;
        }

        public async Task<Task> Handle(HttpContext httpContext)
        {
            using (StreamReader reader = new StreamReader(httpContext.Request.Body))
            {
                var reqBody = await reader.ReadToEndAsync();

                AccountModel accountModelJson = JsonConvert.DeserializeObject<AccountModel>(reqBody);

                if (accountModelJson.username.IsNullOrEmpty() || accountModelJson.password.IsNullOrEmpty()) {

                    await httpContext.Response.WriteAsync("Please Enter Username or password");

                    return Task.CompletedTask;

                }

                await _account.CreateAccount(accountModelJson.username!, accountModelJson.password!);
            }

            await httpContext.Response.WriteAsync("Register!");

            return Task.CompletedTask;
        }
    }
}
