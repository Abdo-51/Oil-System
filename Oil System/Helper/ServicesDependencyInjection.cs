using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Oil_System.BaseInterface;
using Oil_System.BaseInterface.Implementation;
using Oil_System.Contract.Request.Authentcation;
using Oil_System.Helper.Mapping;
using Oil_System.Models;
using Oil_System.Repository.Data;
using Oil_System.Service;
using System.Text;
using System.Threading.RateLimiting;

namespace Oil_System.Helper
{
    public static class ServicesDependencyInjection
    {
        public static IServiceCollection injectservices(this IServiceCollection services, IConfiguration configuration)
        {
            #region DatabaseContext registeration

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("YusufConnection")) // AbdoConnection Replace with your actual connection string name
                .UseLazyLoadingProxies());

            #endregion

            #region Identity registeration
            services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();
            #endregion

            #region JWT Authentication registeration
            var jwtKey = configuration["Jwt:Key"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
                };
            });
            #endregion

            #region Swagger registeration
            services.AddSwaggerGen(opt =>
            {
                // 1. Define the security scheme
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter JWT Bearer token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // Must be lowercase
                    BearerFormat = "JWT"
                };

                opt.AddSecurityDefinition("Bearer", securityScheme);

                // 2. Apply it globally using the new .NET 10 pattern
                opt.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    // Use the new OpenApiSecuritySchemeReference class here
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
                });
            });
            #endregion

            #region RateLimiting registeration
            services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {
                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 10, // Maximum number of requests
                            Window = TimeSpan.FromMinutes(1), // Time window
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0 // No queuing
                        });
                });
                options.RejectionStatusCode = 429; // Too Many Requests
            });
            #endregion

            //Register FluentValidation
            services.AddValidatorsFromAssemblies(new[] { typeof(RegisterRequestValidator).Assembly });

            //Register Automapper
            services.AddAutoMapper(cfg => cfg.AddProfile<Profiling>());

            #region Add CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy("OilSystem", builder =>
                {
                    builder.WithOrigins("http://oilsystem.runasp.net/") // Replace with your actual frontend URLs
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            #endregion

            #region Custom Services registeration
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IOilPacketService, OilPacketService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBrandService, BrandService>();
            #endregion

            return services;
        }
    }
}
