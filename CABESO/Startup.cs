using CABESO.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Linq;

namespace CABESO
{
    /// <summary>
    /// Initialisierungsklasse zur Konfiguration der Projektkontexte
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Der Datenbankkontext
        /// </summary>
        public static ApplicationDbContext Context;

        /// <summary>
        /// Erzeugt ein neues <see cref="Startup"/> mit gegebener Konfiguration.
        /// </summary>
        /// <param name="configuration">
        /// Die Initialkonfiguration
        /// </param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Erzeugt ein neues <see cref="Startup"/> in einer gegebenen Umgebung.
        /// </summary>
        /// <param name="env">
        /// Die Konfigurationsumgebung
        /// </param>
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

        /// <summary>
        /// Die Konfiguration des Projekts
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Die statische E-Mail-Adresse als Absender
        /// </summary>
        public static string MailAddress { get; private set; }

        /// <summary>
        /// Das Passwort des Sender-E-Mail-Kontos
        /// </summary>
        public static string MailPassword { get; private set; }

        /// <summary>
        /// Der SMTP-Server des Sender-E-Mail-Kontos
        /// </summary>
        public static string MailSmtp { get; private set; }

        /// <summary>
        /// Der POP3-Server des Sender-E-Mail-Kontos
        /// </summary>
        public static string MailPop3 { get; private set; }

        /// <summary>
        /// Die E-Mail-Adresse des Absenders bei Rücksendung von empfängerlosen E-Mails
        /// </summary>
        public static string MailReturn { get; private set; }

        /// <summary>
        /// Der Verbindungsstring zum Datenbankserver
        /// </summary>
        public static string DefaultConnection { get; private set; }

        /// <summary>
        /// Die Anzeige der Passwortanforderungen in Tooltips
        /// </summary>
        public static string PasswordRequirements { get; private set; }

        /// <summary>
        /// Konfiguriert alle nötigen Projektdienste.
        /// </summary>
        /// <param name="services">
        /// Angeforderte Dienste
        /// </param>
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

        /// <summary>
        /// Konfiguriert die Infrastruktur der Navigation innerhalb des Projekts
        /// </summary>
        /// <param name="app">
        /// Erzeugerinstanz für das Programm
        /// </param>
        /// <param name="env">
        /// Laufumgebung des Projekts
        /// </param>
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