using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Upskillz_invoice_mgt_Application.Common.Interfaces;
using Upskillz_invoice_mgt_Domain.Common;
using Upskillz_invoice_mgt_Domain.Entities;
using Upskillz_invoice_mgt_Domain.Interfaces;
using Upskillz_invoice_mgt_Infrastructure.ContextClass;
using Upskillz_invoice_mgt_Infrastructure.Identity;
using Upskillz_invoice_mgt_Infrastructure.Implementation;
using Upskillz_invoice_mgt_Infrastructure.Policy;
using Upskillz_invoice_mgt_Infrastructure.Utility;

namespace Upskillz_invoice_mgt_Infrastructure
{
    public static class InfrastructureInjection
    {
        public static void InjectInfrastructure(this IServiceCollection services, IConfiguration Configuration)
        {
            //setup database connection
            services.AddDbContextPool<UpskillDbContext>(option =>
            {
                option.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
            });

            services.AddAuthorization(config =>
            {
                config.AddPolicy(Policies.SuperAdmin, Policies.SuperAdminPolicy());
                config.AddPolicy(Policies.Admin, Policies.AdminPolicy());
                config.AddPolicy(Policies.User, Policies.UserPolicy());
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])),
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ITokenConverter, TokenConverter>();
            services.AddTransient<ITokenGeneratorService, TokenGeneratorService>();

            services.AddTransient<IMailService, MailService>();

            services.AddIdentity<AppUser, IdentityRole>()
                 .AddEntityFrameworkStores<UpskillDbContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
            });

            //services.AddAutoMapper(typeof(MapInitializer));
            var emailConfiguration = new MailSettings
            {
                Mail = Configuration["MailSettings:Mail"],
                DisplayName = Configuration["MailSettings:DisplayName"],
                Password = Configuration["MailSettings:Password"],
                Host = Configuration["MailSettings:Host"],
                Port = Convert.ToInt32(Configuration["MailSettings:Port"])
            };

            services.AddSingleton(emailConfiguration);
        }
    }
}
