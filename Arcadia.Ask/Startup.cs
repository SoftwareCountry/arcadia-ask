namespace Arcadia.Ask
{
    using Arcadia.Ask.Auth;
    using Arcadia.Ask.Hubs;
    using Arcadia.Ask.Storage;
    using Arcadia.Ask.Storage.Questions;

    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SpaServices.AngularCli;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.LoadUserCookieNameFromConfiguration();
        }

        private string userCookieName;

        private void LoadUserCookieNameFromConfiguration()
        {
            const string defaultValue = "User";
            const string sectionName = "UserCookieName";

            var configUserCookieNameValue = this.Configuration.GetSection(sectionName).Value;

            this.userCookieName = string.IsNullOrEmpty(configUserCookieNameValue) ? defaultValue : configUserCookieNameValue;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkInMemoryDatabase();

            services.AddDbContext<DatabaseContext>((sp, options) =>
            {
                options
                    .UseInMemoryDatabase("InMemoryDatabase")
                    .UseInternalServiceProvider(sp);
            });
            services.AddTransient<IQuestionStorage, QuestionStorage>();

            services.AddSignalR();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = this.userCookieName;
                    options.Cookie.IsEssential = true;
                });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseAuthentication();

            app.UseGuidIdentification(this.userCookieName);

            app.UseSignalR(routes =>
            {
                routes.MapHub<QuestionsHub>("/questions");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer("start");
                }
            });
        }
    }
}