using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json;

namespace Project_UNIX.AuthServer.Network.RoutesController
{
    internal class GamePatcherController
    {
        public async Task<Task> GameVersion(HttpContext httpContext)
        {

            await httpContext.Response.WriteAsync("dev2");

            return Task.CompletedTask;
        }

        public async Task<Task> UpdateVersion(HttpContext httpContext)
        {
            var newVer = Encoding.UTF8.GetBytes("Congret you're new version Lol");

            await httpContext.Response.Body.WriteAsync(newVer, 0, newVer.Length);

            return Task.CompletedTask;
        }


    }
}
