namespace Arcadia.Ask
{
    using System;
    using System.Threading.Tasks;

    using Arcadia.Ask.Auth;
    using Arcadia.Ask.Auth.Permissions;
    using Arcadia.Ask.Configuration;
    using Arcadia.Ask.Hubs;
    using Arcadia.Ask.Models.Entities;
    using Arcadia.Ask.Questions;
    using Arcadia.Ask.Storage;
    using Arcadia.Ask.Storage.Questions;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SpaServices.AngularCli;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        private readonly IConfiguration configuration;
        private bool useInMemoryDatabase = false;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.ApplicationSettings = configuration.Get<ApplicationSettings>();
        }

        public ApplicationSettings ApplicationSettings { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>((sp, options) =>
            {
                if (this.useInMemoryDatabase)
                {
                    options
                        .UseInMemoryDatabase("InMemoryDatabase");
                }
                else
                {
                    options
                        .UseSqlServer(this.configuration.GetConnectionString("DefaultConnection"));
                }
            });
            services.AddTransient<IQuestionStorage, QuestionStorage>();
            services.AddTransient<IPermissionsByRoleLoader, PermissionsByRoleLoader>();
            services.AddTransient<IPasswordHasher<ModeratorEntity>, PasswordHasher<ModeratorEntity>>();
            services.AddTransient<ISignInService, SignInService>();
            services.AddSingleton<IDisplayedQuestion, DisplayedQuestion>();

            services.AddSignalR();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/api/auth");
                    options.Cookie.Name = this.ApplicationSettings.AuthSettings.UserCookieName;
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
                this.useInMemoryDatabase = true;
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

            app.UseSignalR(routes =>
            {
                routes.MapHub<QuestionsHub>("/questions");
                routes.MapHub<DisplayQuestionHub>("/displayed-question");
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
                spa.ApplicationBuilder.Use(RequireAuthentication);

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer("start");
                }
            });
        }

        private static async Task RequireAuthentication(HttpContext context, Func<Task> next)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                await context.ChallengeAsync();
            }
            else
            {
                await next();
            }
        }
    }
}