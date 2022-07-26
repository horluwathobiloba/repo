using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Infrastructure.Identity;
using OnyxDoc.DocumentService.Infrastructure.Persistence;
using OnyxDoc.DocumentService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OnyxDoc.DocumentService.Infrastructure.Contract;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using RestSharp;
using OnyxDoc.DocumentService.Infrastructure.Utility;

namespace OnyxDoc.DocumentService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("OnyxDoc.DocumentServiceDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            //services.AddSingleton<IApiAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            // services.AddScoped<IApiAuthorizationHandler, PermissionApiAuthorizationHandler>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                    .AddJwtBearer(options =>
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
            services.AddAuthorization();
            // services.AddDefaultIdentity<ApplicationUser>().AddRoles<ApplicationRole>()
            //   .AddEntityFrameworkStores<ApplicationDbContext>();
            // services.AddIdentityServer()
            //  .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
            services.AddTransient<IDateTime, DateTimeService>();
          // services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IBase64ToFileConverter, Base64ToFileConverter>();
            services.AddTransient<IBearerTokenService, BearerTokenService>();
            services.AddTransient<IAPIClientService, APIClientService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IRestClient, RestClient>();
            services.AddTransient<IBlobStorageService, BlobStorageService>();
            services.AddTransient<IStringHashingService, StringHashingService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<INotificationService, NotificationService>();

            return services;
        }


    }
    public static class ServiceExtensions
    {
        public static IServiceCollection AddJwtBearerContractentication(this IServiceCollection services)
        {
            //var builder = services.AddAuthentication(o =>
            //{
            //    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //});

            //builder.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateAudience = true,
            //        ValidateIssuer = true,
            //        ValidateLifetime = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(
            //            Encoding.UTF8.GetBytes(TokenConstants.key)),
            //        ValidIssuer = TokenConstants.Issuer,
            //        ValidAudience = TokenConstants.Audience
            //    };
            //});

            return services;
        }

        public static IServiceCollection AddRolesAndPolicyApiAuthorization(this IServiceCollection services)
        {
            //services.AddApiAuthorization(
            //    config =>
            //    {
            //        config.AddPolicy("ShouldBeAReader", options =>
            //        {
            //            options.RequireContractenticatedUser();
            //            options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            //            options.Requirements.Add(new ShouldBeAReaderRequirement());
            //        });

            //        // Add a new Policy with requirement to check for Admin
            //        config.AddPolicy("ShouldBeAnAdmin", options =>
            //        {
            //            options.RequireContractenticatedUser();
            //            options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            //            options.Requirements.Add(new ShouldBeAnAdminRequirement());
            //        });

            //        config.AddPolicy("ShouldContainRole", options => options.RequireClaim(ClaimTypes.Role));
            //    });

            return services;
        }
    }
}

