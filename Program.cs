using Microsoft.AspNetCore.Authentication.JwtBearer;
using FactoriesGateSystem.Helpers;
using FactoriesGateSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FactoriesGateSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options => options
            .UseMySql(builder.Configuration.GetConnectionString("Connection"),
            new MySqlServerVersion(new Version(8, 2, 12))));


            builder.Services.AddScoped<CustomerRepo>();
            builder.Services.AddScoped<OrderRepo>();
            builder.Services.AddScoped<ProductRepo>();
            builder.Services.AddScoped<MaterialRepo>();
            builder.Services.AddScoped<EmployeeRepo>();
            builder.Services.AddScoped<SupplierRepo>();
            builder.Services.AddScoped<AuthRepo>();
            builder.Services.AddScoped<PasswordHasher>();
            builder.Services.AddScoped<JwtHelper>();


            // JWT settings
            var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
            });


            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Seed();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
