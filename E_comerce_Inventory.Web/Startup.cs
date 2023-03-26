using E_comerce_Inventory.DataAccess.Data;
using E_comerce_Inventory.DataAccess.Repository;
using E_comerce_Inventory.DataAccess.Repository.Interface;
using E_comerce_Inventory.Models.DataModels;
using E_comerce_Inventory.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_comerce_Inventory.Web
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
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"));

            });

            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddIdentity<IdentityUser,IdentityRole>((options) =>
            {
                options.User.RequireUniqueEmail = true;//activo la validacion por email              
                options.User.AllowedUserNameCharacters = null;//Desactivo la validacion por UserName

            }
            )
            .AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();// services.AddDefaultIdentity NO ACEPTA ROLES
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            services.AddScoped<IWorkUnit,WorkUnit>();
            services.AddSingleton<IEmailSender,EmailSender>(sp => new EmailSender(sp.GetService<IConfiguration>()));

            //al controlador de de vistas razor , depeus de haber instalado el packete le a�ado .AddRazorRuntimeCompilation() , para poder modificar las vistas en tiempo de ejecucion
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true; //js no puede acceder a la cookie
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);//Establece el tiempo de caducidad de la cookie 
                options.LoginPath = "/Identity/Account/Login";//Establece la ruta a la p�gina de acceso denegado a la que se redirigir� al usuario si est� autenticado pero no tiene acceso a un recurso en particular.
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(5);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            } else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["SecretKey"];

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=Inventory}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
