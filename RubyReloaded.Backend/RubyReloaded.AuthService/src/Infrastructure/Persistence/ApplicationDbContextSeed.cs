using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using System;
using RubyReloaded.AuthService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace RubyReloaded.AuthService.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {

          
            //var defaultOrg = new Organization
            //{
            //    Name = "Test Org",
            //    CreatedDate = DateTime.Now,
            //    CreatedBy = "SYSTEM",
            //    CreatedById = "SYSTEM",
            //    RCNumber = "12345678"
            //};
            //var defaultRole = new Role
            //{
            //    Name = "Super Admin",
            //    CreatedDate = DateTime.Now,
            //    CreatedBy = "SYSTEM",
            //    CreatedById = "SYSTEM",
            //};
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
          
            //if (context.Roles.All(u => u.Name != defaultRole.Name))
            //{
            //    var result =  await context.AddAsync(defaultRole);
            //    await context.SaveChangesAsync();
            //}
            if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
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
