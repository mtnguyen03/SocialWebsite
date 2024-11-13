
using BookApi.Repositories;
using BusinessObject;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using System.Text;

namespace SocialApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //Add odata
            var modelBuilder = new ODataConventionModelBuilder();
            //modelBuilder.EntitySet<Category>("Categories");
            modelBuilder.EntitySet<User>("Users");


            builder.Services.AddHttpContextAccessor();

            builder.Services.AddControllers()
                .AddOData(o => o.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null)
                .AddRouteComponents("odata", modelBuilder.GetEdmModel()));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            // life cycle DI: Addsington, addTransisent, addScope
            // add more scope here
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped(typeof(SocialDbContext));
            builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
            builder.Services.AddDbContext<SocialDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DB")
                  );
            });

            builder.Services.AddIdentity<User, IdentityRole>()
                            .AddEntityFrameworkStores<SocialDbContext>().AddDefaultTokenProviders();
            // Add services to the container.


            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.RequireHttpsMetadata = false;
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
                };

            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseODataBatching();
            app.UseRouting();
            app.UseCors("AllowAllOrigins"); // Enable CORS policy https;;lohos888 --> 999
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
