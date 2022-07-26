using Onyx.AuthService.Domain.Entities;
using Onyx.AuthService.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using System;
using Onyx.AuthService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Onyx.AuthService.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {

          
            var defaultOrg = new Organization
            {
                Name = "Test Org",
                CreatedDate = DateTime.Now,
                CreatedBy = "SYSTEM",
                CreatedById = "SYSTEM",
                RCNumber = "12345678"
            };
            var defaultRole = new Role
            {
                Name = "Super Admin",
                CreatedDate = DateTime.Now,
                CreatedBy = "SYSTEM",
                CreatedById = "SYSTEM",
            };
            var defaultUser = new ApplicationUser
            {
                UserName = "administrator@localhost",
                FirstName = "admin",
                LastName = "admin",
                Email = "administrator@localhost",
                CreatedBy = "SYSTEM",
                CreatedById = "SYSTEM",
                CreatedDate = DateTime.Now,
            };
            if (context.Organizations.All(u => u.Name != defaultOrg.Name))
            {
                var result = await context.AddAsync(defaultOrg);
                await context.SaveChangesAsync();
            }
            if (context.Roles.All(u => u.Name != defaultRole.Name))
            {
                defaultRole.OrganizationId = context.Organizations.FirstOrDefaultAsync().Result.Id;
                var result =  await context.AddAsync(defaultRole);
                await context.SaveChangesAsync();
            }
            if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
                defaultUser.OrganizationId = context.Organizations.FirstOrDefaultAsync().Result.Id;
                defaultUser.RoleId = context.Roles.FirstOrDefaultAsync().Result.Id;
                defaultUser.Id = Guid.NewGuid().ToString();
                defaultUser.UserId = Guid.NewGuid().ToString();
                await userManager.CreateAsync(defaultUser, "Administrator1!");
            }


        }

        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            // Seed, if necessary
            //if (!context.TodoLists.Any())
            //{
            //    context.TodoLists.Add(new TodoList
            //    {
            //        Title = "Shopping",
            //        Items =
            //        {
            //            new TodoItem { Title = "Apples", Done = true },
            //            new TodoItem { Title = "Milk", Done = true },
            //            new TodoItem { Title = "Bread", Done = true },
            //            new TodoItem { Title = "Toilet paper" },
            //            new TodoItem { Title = "Pasta" },
            //            new TodoItem { Title = "Tissues" },
            //            new TodoItem { Title = "Tuna" },
            //            new TodoItem { Title = "Water" }
            //        }
            //    });

                await context.SaveChangesAsync();
           // }
        }
    }
}
