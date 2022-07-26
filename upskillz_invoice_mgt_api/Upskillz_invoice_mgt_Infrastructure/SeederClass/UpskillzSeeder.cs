using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Upskillz_invoice_mgt_Domain.Entities;
using Upskillz_invoice_mgt_Infrastructure.ContextClass;
using Upskillz_invoice_mgt_Infrastructure.Policy;

namespace Upskillz_invoice_mgt_Infrastructure.SeederClass
{
    public class UpskillzSeeder
    {
        public static async Task SeedData(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            await Seeder(
                (UserManager<AppUser>)serviceScope.ServiceProvider.GetService(typeof(UserManager<AppUser>)),
                serviceScope.ServiceProvider.GetService<UpskillDbContext>(),
                (RoleManager<IdentityRole>)serviceScope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>))
                );
        }
        private async static Task Seeder(UserManager<AppUser> userManager, UpskillDbContext context, RoleManager<IdentityRole> roleManager)
        {
            try
            {
                var baseDir = Directory.GetCurrentDirectory();

                await context.Database.EnsureCreatedAsync();
                
                if (!context.Companies.Any())
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = Policies.SuperAdmin });
                    await roleManager.CreateAsync(new IdentityRole { Name = Policies.Admin });
                    await roleManager.CreateAsync(new IdentityRole { Name = Policies.User });

                    var path = File.ReadAllText(FilePath(baseDir, "StaticsFiles/Json/Company.json"));

                    var meal = JsonConvert.DeserializeObject<Company>(path);

                    foreach (var user in meal.AppUsers)
                    {
                        await userManager.CreateAsync(user, user.PasswordHash);
                        user.CreatedAt = DateTime.UtcNow;
                        user.UpdatedAt = DateTime.UtcNow;

                        if (user.Email.Contains("super", StringComparison.OrdinalIgnoreCase))
                        {
                            await userManager.AddToRoleAsync(user, Policies.SuperAdmin);
                        }
                        else if (user.Email.Contains("admin", StringComparison.OrdinalIgnoreCase))
                        {
                            await userManager.AddToRoleAsync(user, Policies.Admin);
                        }
                        else
                            await userManager.AddToRoleAsync(user, Policies.User);
                    }

                    await context.Companies.AddRangeAsync(meal);
                }

                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        static string FilePath(string folderName, string fileName)
        {
            return Path.Combine(folderName, fileName);
        }
    }
}
