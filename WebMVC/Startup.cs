namespace WebMVC
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Infrastructure.Identity;
    using Microsoft.AspNetCore.Identity;
    using ApplicationCore.Interfaces;
    using Infrastructure.Data;
    using Infrastructure.Logging;
    using Microsoft.EntityFrameworkCore;
    public class Startup
    {
        private IServiceCollection _servicos;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            // use real database
            // ConfigureProductionServices(services);

            // use in-memory database
            ConfigureTestingServices(services);
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            // use real database
            services.AddDbContext<BaseAspCoreContext>(c =>
            {
                try
                {
                    // Requires LocalDB which can be installed with SQL Server Express 2016
                    // https://www.microsoft.com/en-us/download/details.aspx?id=54284
                    c.UseSqlServer(Configuration.GetConnectionString("BaseAspCoreConnection"));
                }
                catch (System.Exception ex)
                {
                    var message = ex.Message;
                }
            });

            // Add Identity DbContext
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IdentityBaseAspCoreConnection")));

            ConfigureServices(services);
        }

        private void ConfigureTestingServices(IServiceCollection services)
        {
            // use in-memory database
            services.AddDbContext<BaseAspCoreContext>(c =>
                c.UseInMemoryDatabase("BaseAspCoreDB"));

            // Add Identity DbContext
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseInMemoryDatabase("IdentityBaseAspCoreDB"));

            ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configurando o Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                           .AddEntityFrameworkStores<AppIdentityDbContext>()
                           .AddDefaultTokenProviders();

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            AuthAppBuilderExtensions.UseAuthentication(app);
            //app.UseIdentity();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
