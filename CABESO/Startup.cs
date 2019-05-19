using CABESO.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace CABESO
{
    public class Startup
    {
        public static ApplicationDbContext Context;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Startup(IHostingEnvironment env)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json",
                             optional: false,
                             reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
                builder.AddUserSecrets<Startup>();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public static string MailAddress { get; private set; }

        public static string MailPassword { get; private set; }

        public static string MailSmtp { get; private set; }

        public static string MailPop3 { get; private set; }

        public static string MailReturn { get; private set; }

        public static string DefaultConnection { get; private set; }

        public static string PasswordRequirements { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Identity/Account/AccessDenied");
                options.LoginPath = new PathString("/Identity/Account/Login");
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(DefaultConnection = Configuration.GetConnectionString("DefaultConnection")), contextLifetime: ServiceLifetime.Transient);

            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
                config.User.RequireUniqueEmail = true;

                PasswordOptions pwdOptions = config.Password;
                ErrorDescriber describer = new ErrorDescriber();
                IdentityError[] errors =
                {
                    pwdOptions.RequireDigit ? describer.PasswordRequiresDigit() : null,
                    pwdOptions.RequireLowercase ? describer.PasswordRequiresLower() : null,
                    pwdOptions.RequireNonAlphanumeric ? describer.PasswordRequiresNonAlphanumeric() : null,
                    pwdOptions.RequireUppercase ? describer.PasswordRequiresUpper() : null,
                    pwdOptions.RequiredUniqueChars > 1 ? describer.PasswordRequiresUniqueChars(pwdOptions.RequiredUniqueChars) : null,
                    describer.PasswordTooShort(pwdOptions.RequiredLength)
                };
                PasswordRequirements = string.Join('\n', Array.ConvertAll(errors, error => error?.Description).Where(requirement => !string.IsNullOrEmpty(requirement)));
            })
                .AddErrorDescriber<ErrorDescriber>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            MailAddress = Configuration["Mail:Address"];
            MailPassword = Configuration["Mail:Password"];
            MailSmtp = Configuration["Mail:Smtp"];
            MailPop3 = Configuration["Mail:Pop3"];
            MailReturn = Configuration["Mail:Return"];
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}