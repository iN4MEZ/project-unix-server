using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Project_UNIX.AuthServer.Network.RoutesController;
using Project_UNIX.Common.Database;
using Project_UNIX.Common.Server;
using Project_UNIX.Common.Validator.Token;

namespace Project_UNIX.AuthServer.Network.Https
{
    internal class HttpsStartUp : IStartup
    {
        public void Configure(IApplicationBuilder app)
        {

            //app.UseMiddleware<AuthVerify>();

            app.UseRouter(routes =>
            {

                routes.MapPost("/login", context => app.ApplicationServices.GetRequiredService<LoginController>().Handle(context));

                routes.MapPost("/register", context => app.ApplicationServices.GetRequiredService<RegisterController>().Handle(context));

                routes.MapGet("/query_gateway", context => app.ApplicationServices.GetRequiredService<QueryGatewayController>().Handle(context)
                );
                routes.MapGet("/query_dispatch", context => app.ApplicationServices.GetRequiredService<QueryDispatchController>().Handle(context));

                routes.MapGet("/patch/datav", context => app.ApplicationServices.GetRequiredService<GamePatcherController>().GameVersion(context));

                routes.MapGet("/patch/dev2", context => app.ApplicationServices.GetRequiredService<GamePatcherController>().UpdateVersion(context));
            });
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            services.AddLogging(builder =>
            {
                builder.AddConsole();
            });
            services.AddSingleton<DatabaseHandler>();
            services.AddSingleton<TokenValidation>();
            services.AddSingleton<Account>();
            services.AddSingleton<QueryGatewayController>();
            services.AddSingleton<QueryDispatchController>();
            services.AddSingleton<LoginController>();
            services.AddSingleton<RegisterController>();
            services.AddSingleton<GamePatcherController>();

            return services.BuildServiceProvider();
        }
    }
}
