using BusinessObject;
using DataAccess.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialFrontEnd.Helper;

namespace SocialFrontEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpClient("default", client =>
            {
                client.Timeout = TimeSpan.FromSeconds(10);
            });
            // Add services to the container.
     
            builder.Services.AddTransient<JwtHelper>();
            builder.Services.AddDbContext<SocialDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DB")
                  );
            });
            builder.Services.AddSignalR();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/account/login"; 
                });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy =>
                    policy.RequireRole(AppRole.User)); // Require the user to have the "User" role
            });

            builder.Services.AddSession((options) =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
      
            builder.Services.AddIdentity<User, IdentityRole>()
                            .AddEntityFrameworkStores<SocialDbContext>().AddDefaultTokenProviders();

            builder.Services.AddAuthorization();
            builder.Services.AddControllersWithViews(); 
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapHub<ChatHub>("/chathub");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        
            app.Run();
        }
    }
}
