using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Project_UNIX.AuthServer.Network.Model;
using Newtonsoft.Json;

namespace Project_UNIX.AuthServer.Network.RoutesController
{
    internal class QueryGatewayController : IRoutesController
    {


        private readonly ILogger _logger;
        public QueryGatewayController(ILogger<QueryGatewayController> logger)
        {
            _logger = logger;
        }

        public async Task<Task> Handle(HttpContext httpContext)
        {
            string _userId = httpContext.Request.Query["userId"].ToString();

            GatewayModel[] activeGateways =
            {
                new GatewayModel
                {
                    RegionName = "DevServer",
                    Address = "49.228.131.138",
                    Port = 7719
                },
                new GatewayModel
                {
                    RegionName = "LocalHost",
                    Address = "127.0.0.1",
                    Port = 13720
                }
            };

            var json = JsonConvert.SerializeObject(activeGateways,Formatting.Indented);

            await httpContext.Response.WriteAsync(json);

            return Task.CompletedTask;

        }
    }
}
