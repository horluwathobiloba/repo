using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Infrastructure.Identity;
using RubyReloaded.AuthService.Infrastructure.Persistence;
using RubyReloaded.AuthService.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RubyReloaded.AuthService.Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RubyReloaded.AuthService.Infrastructure.Auth;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using RubyReloaded.AuthService.Infrastructure.Authorization;
using RubyReloaded.AuthService.Infrastructure.Utility;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace RubyReloaded.AuthService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("RubyReloaded.AuthServiceDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            //services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            // services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }) .AddJwtBearer(options =>
                     {
                         options.TokenValidationParameters = new TokenValidationParameters
                         {
                             ValidateAudience = false,
                             ValidateIssuer = false,
                             ValidateLifetime = true,
                             IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["TokenConstants:key"])),
                             ValidIssuer = configuration["Token:issuer"],
                             ValidAudience = configuration["Token:aud"]
                         };
                         options.Authority = configuration["Token:issuer"];
                         options.SaveToken = true;
                         options.Audience = configuration["Token:aud"];
                         options.RequireHttpsMetadata = false;
                         options.Configuration = new OpenIdConnectConfiguration();
                     });
            services.AddDefaultIdentity<ApplicationUser>()
                   .AddEntityFrameworkStores<ApplicationDbContext>();
            //services.AddIdentityServer()
            //    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IBase64ToFileConverter, Base64ToFileConverter>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IAuthenticateService, AuthenticateService>();
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IAPIClient, APIClient>();
            //services.AddTransient<ISqlService, SqlService>();
            services.AddTransient<IUtilityService, UtilityService>();
            services.AddTransient<IBlobStorageService, BlobStorageService>();
            services.AddTransient<IStringHashingService, StringHashingService>();
            services.AddTransient<ISqlService, SqlService>();

            return services;
        }


    }
    public static class ServiceExtensions
    {
        public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services)
        {
            var builder = services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            builder.AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(TokenConstants.key)),
                    ValidIssuer = TokenConstants.Issuer,
                    ValidAudience = TokenConstants.Audience
                };
            });

            return services;
        }

        public static IServiceCollection AddRolesAndPolicyAuthorization(this IServiceCollection services)
        {
            //services.AddAuthorization(
            //    config =>
            //    {
            //        config.AddPolicy("ShouldBeAReader", options =>
            //        {
            //            options.RequireAuthenticatedUser();
            //            options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            //            options.Requirements.Add(new ShouldBeAReaderRequirement());
            //        });

            //        // Add a new Policy with requirement to check for Admin
            //        config.AddPolicy("ShouldBeAnAdmin", options =>
            //        {
            //            options.RequireAuthenticatedUser();
            //            options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            //            options.Requirements.Add(new ShouldBeAnAdminRequirement());
            //        });

            //        config.AddPolicy("ShouldContainRole", options => options.RequireClaim(ClaimTypes.Role));
            //    });

            return services;
        }
    }
}

