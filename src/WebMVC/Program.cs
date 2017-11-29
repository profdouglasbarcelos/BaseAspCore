namespace WebMVC
{
    using Infrastructure.Data;
    using Infrastructure.Identity;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var escopo = host.Services.CreateScope())
            {
                IServiceProvider services = escopo.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var baseAspCoreContext = services.GetRequiredService<BaseAspCoreContext>();
                    //BaseAspCoreContextSeed.SeedAsync(baseAspCoreContext, loggerFactory).Wait();

                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    AppIdentityDbContextSeed.SeedAsync(userManager).Wait();
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
