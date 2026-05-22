using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Oil_System.Repository.Data;
using Oil_System.Service;
using System.Text;

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
            services.AddIdentity<IdentityUser, IdentityRole>()
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

            #region Custom Services registeration
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            #endregion

            return services;
        }
    }
}
