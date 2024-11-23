using Microsoft.AspNetCore.Http;
using System.Text;

namespace Project_UNIX.SDKServer.Network.RoutesController
{
    internal class GamePatcherController
    {
        public async Task<Task> GameVersion(HttpContext httpContext)
        {

            await httpContext.Response.WriteAsync("monsterdatav1");

            return Task.CompletedTask;
        }

        public async Task<Task> UpdateVersion(HttpContext httpContext)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "resources", "data.db");


            if (!System.IO.File.Exists(filePath))
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                var errorMessage = Encoding.UTF8.GetBytes("File not found.");
                await httpContext.Response.Body.WriteAsync(errorMessage, 0, errorMessage.Length);
                return Task.CompletedTask;
            }

            // กำหนด Header สำหรับการดาวน์โหลดไฟล์
            httpContext.Response.ContentType = "application/octet-stream";
            httpContext.Response.Headers.Add("Content-Disposition", $"attachment; filename=\"data.db\"");

            // อ่านไฟล์และเขียนลงไปใน Response Body
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                await stream.CopyToAsync(httpContext.Response.Body);
            }

            return Task.CompletedTask;
        }


    }
}
