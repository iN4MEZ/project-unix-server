using Microsoft.AspNetCore.Http;
using Project_UNIX.Common.Utils;
using System.Text.Json;

namespace Project_UNIX.AuthServer.Network.RoutesController
{
    internal class QueryDispatchController
    {

        public async Task<Task> Handle(HttpContext httpContext)
        {
            string gameVersion = httpContext.Request.Query["v"].ToString();

            var RegionName = new[]
            {
                new { ServerName = "DEV", Address = "127.0.0.1", Port = "5001" },

            };

            var regionJson = JsonSerializer.Serialize(RegionName);

            await httpContext.Response.WriteAsync(regionJson);

            return Task.CompletedTask;
        }
    }
}
