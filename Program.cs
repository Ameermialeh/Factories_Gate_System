using Microsoft.AspNetCore.Authentication.JwtBearer;
using FactoriesGateSystem.Helpers;
using FactoriesGateSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using FactoriesGateSystem.Middleware;
using FactoriesGateSystem.Services;
using FactoriesGateSystem.Repositories.Interfaces;
using FactoriesGateSystem.Services.ServiceInterfaces;

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

            builder.Services.AddHttpContextAccessor(); // For Cookies




            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<ICookieService, CookieService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IExpenseService, ExpenseService>();  
            builder.Services.AddScoped<IFactoryService, FactoryService>();
            builder.Services.AddScoped<IInvoiceService, InvoiceService>();
            builder.Services.AddScoped<IManagerService, ManagerService>();
            builder.Services.AddScoped<IMaterialService, MaterialService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ISalaryService, SalaryService>();
            builder.Services.AddScoped<ISupplierService, SupplierService>();
            builder.Services.AddScoped<IVacationService, VacationService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped<IAuthRepo, AuthRepo>();
            builder.Services.AddScoped<IFactoryRepo, FactoryRepo>();
            builder.Services.AddScoped<IAdminRepo, AdminRepo>();
            builder.Services.AddScoped<IAuthRepo, AuthRepo>();
            builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
            builder.Services.AddScoped<IEmployeeRepo, EmployeeRepo>();
            builder.Services.AddScoped<IExpenseRepo, ExpenseRepo>();
            builder.Services.AddScoped<IInvoiceRepo, InvoiceRepo>();
            builder.Services.AddScoped<IManagerRepo, ManagerRepo>();
            builder.Services.AddScoped<IMaterialRepo, MaterialRepo>();
            builder.Services.AddScoped<IOrderRepo, OrderRepo>();
            builder.Services.AddScoped<IProductRepo, ProductRepo>();
            builder.Services.AddScoped<ISalaryRepo, SalaryRepo>();
            builder.Services.AddScoped<ISupplierRepo, SupplierRepo>();
            builder.Services.AddScoped<IVacationRepo, VacationRepo>();

            builder.Services.AddScoped<IJwtHelper, JwtHelper>();
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            // JWT settings
            var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!);

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
                    ClockSkew = TimeSpan.Zero,
                };
            });

            // React connection
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReact",
                    policy => policy
                        .WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            var app = builder.Build();
            // Seed
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

            app.UseCors("AllowReact");

            // Global exception handler
            app.UseMiddleware<ExceptionMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
