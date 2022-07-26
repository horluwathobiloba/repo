using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Infrastructure.Identity;
using OnyxDoc.AuthService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnyxDoc.AuthService.Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OnyxDoc.AuthService.Infrastructure.Auth;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using OnyxDoc.AuthService.Infrastructure.Authorization;
using OnyxDoc.AuthService.Infrastructure.Utility;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using OnyxDoc.AuthService.Infrastructure.Services;

namespace OnyxDoc.AuthService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("OnyxDoc.AuthServiceDb"));
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
            services.AddDefaultIdentity<ApplicationUser>().AddRoles<ApplicationRole>()
                   .AddEntityFrameworkStores<ApplicationDbContext>();
            //services.AddIdentityServer()
            //    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IBase64ToFileConverter, Base64ToFileConverter>();
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IAuthenticateService, AuthenticateService>();
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IGenerateUserInviteLinkService, GenerateUserInviteLinkService>();
            services.AddTransient<IAPIClient, APIClient>();
            services.AddTransient<ISqlService, SqlService>();
            services.AddTransient<IUtilityService, UtilityService>();
            services.AddTransient<IBlobStorageService, BlobStorageService>();
            services.AddTransient<IStringHashingService, StringHashingService>();
            services.AddTransient<IDomainCheckService, DomainCheckService>();
            services.AddTransient<IVerificationEmailService, VerificationEmailService>();

            return services;
        }


    }
  
}

