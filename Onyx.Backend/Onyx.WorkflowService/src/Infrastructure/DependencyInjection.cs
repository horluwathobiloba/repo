using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Infrastructure.Files;
using Onyx.WorkFlowService.Infrastructure.Identity;
using Onyx.WorkFlowService.Infrastructure.Persistence;
using Onyx.WorkFlowService.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onyx.WorkFlowService.Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Onyx.WorkFlowService.Infrastructure.Auth;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Onyx.WorkFlowService.Infrastructure.Authorization;

namespace Onyx.WorkFlowService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("Onyx.AuthServiceDb"));
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
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                         .GetBytes(TokenConstants.key)),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    });
            services.AddDefaultIdentity<ApplicationUser>().AddRoles<ApplicationRole>()
                   .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
            services.AddTransient<IBase64ToFileConverter, Base64ToFileConverter>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IAuthenticateService, AuthenticateService>();
            //services.AddAuthentication()
            //    .AddIdentityServerJwt();
            //services.AddJwtBearerAuthentication();

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

