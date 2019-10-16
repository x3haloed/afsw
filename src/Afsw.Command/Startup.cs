using Afsw.Command.IdentityProvider;
using Afsw.Command.Middleware;
using Afsw.Command.Services;
using Afsw.Types;
using Hangfire;
using Hangfire.LiteDB;
using LiteDB;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Afsw.Command
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // add LiteDB
            services.AddSingleton(new LiteDatabase(@"store.db"));

            // Add identity types
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddDefaultTokenProviders();

            // Identity Services
            services.AddTransient<IUserStore<ApplicationUser>, LiteDbUserStore>();
            //services.AddTransient<IUserRoleStore<ApplicationUser>, LiteDbUserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, StaticRoleStore>();
            services.AddTransient<LiteDbUsersTable>();

            // configure basic authentication 
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", o => { });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 0;
            });

            

            services.AddMvc();

            services.AddSingleton<ViewRenderService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<HangfireTasks>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // this should be moved somewhere else
            GlobalConfiguration.Configuration.UseLiteDbStorage();
            new BackgroundJobServer();
            RecurringJob.AddOrUpdate(() => app.ApplicationServices.GetService<HangfireTasks>().CompilePosts(), Cron.Minutely);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
