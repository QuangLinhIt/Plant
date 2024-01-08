using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Plant.Areas.Identity.Data;
using Plant.Data;
using Plant.Models;
using Plant.Services;
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
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
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

            services.AddDbContext<PlantContext>(options => options.UseSqlServer(Configuration.GetConnectionString("PlantContextConnection")));
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<PlantContext>();
            // Truy cập IdentityOptions
            services.Configure<IdentityOptions>(options =>
            {
                /// Thiết lập về Password
                options.Password.RequireDigit = true;// Yêu cầu ít nhất một chữ số
                options.Password.RequireLowercase = true;// Không bắt phải có chữ thường
                options.Password.RequireNonAlphanumeric = true;// Yêu cầu ít nhất một ký tự đặc biệt không phải chữ cái hoặc số
                options.Password.RequireUppercase = true;// Yêu cầu ít nhất một chữ cái viết hoa
                options.Password.RequireLowercase = true; // Yêu cầu ít nhất một chữ cái viết thường
                options.Password.RequiredLength = 8;// Độ dài tối thiểu của mật khẩu
                options.Password.RequiredUniqueChars = 1;// Số ký tự riêng biệt

                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
                options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
                options.Lockout.AllowedForNewUsers = true;

                // Cấu hình về User.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;

                //confirm email
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddAuthentication()
                .AddFacebook(facebookOptions =>
                    {
                        facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                        facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                        facebookOptions.CallbackPath = new PathString("/signin-facebook");
                        facebookOptions.AccessDeniedPath = "/AccessDeniedPathInfo";
                    })
                .AddGoogle(googleOptions =>
                 {
                     googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                     googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                     googleOptions.CallbackPath = new PathString("/signin-google");
                     googleOptions.AccessDeniedPath = "/AccessDeniedPathInfo";
                 });
            services.AddRazorPages();

            //VNPAY
            services.AddSingleton<IVnPayService, VnPayService>();

            //cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.WithOrigins("https://localhost:44349/")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
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
            app.UseCors("AllowSpecificOrigin");
            //var cultures = new[] { "vi", "en" }; 
            //var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(cultures[0])
            //    .AddSupportedCultures(cultures)
            //    .AddSupportedUICultures(cultures);
            //app.UseRequestLocalization(localizationOptions);

            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);


            app.UseAuthentication();
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
                endpoints.MapRazorPages();
            });
        }

    }
}
