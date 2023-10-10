using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Plant.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Plant
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
            services.AddNotyf(config => { config.DurationInSeconds = 3; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });
            services.AddDbContextPool<plantContext>(options => options.UseSqlServer(Configuration.GetConnectionString("plantConnectString")));
            services.AddSession();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            #region snippet_LocalizationConfigurationServices
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
            #endregion
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var cultures = new List<CultureInfo>
                {
                    new CultureInfo("vi"),
                    new CultureInfo("en")
                };
                options.DefaultRequestCulture = new RequestCulture("vi");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseSession();
            app.UseStaticFiles();
            app.UseNotyf();
            app.UseRouting();

            //var cultures = new[] { "vi", "en" }; 
            //var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(cultures[0])
            //    .AddSupportedCultures(cultures)
            //    .AddSupportedUICultures(cultures);
            //app.UseRequestLocalization(localizationOptions);

            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=AdminHome}/{action=Dashboard}/{id?}"
                );
                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}"
                   );
            });
        }

    }
}
