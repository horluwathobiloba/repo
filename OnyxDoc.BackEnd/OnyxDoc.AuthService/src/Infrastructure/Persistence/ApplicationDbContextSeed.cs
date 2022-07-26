using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using System;
using OnyxDoc.AuthService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace OnyxDoc.AuthService.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            var defaultOrg = new Subscriber
            {
                Name = "Revent Technologies",
                CreatedDate = DateTime.Now,
                CreatedByEmail = "SYSTEM",
                CreatedById = "SYSTEM",
                SubscriberAccessLevel = Domain.Enums.SubscriberAccessLevel.System,
                ContactEmail = "info@reventtechnologies.com",
                Country = "Nigeria",
                City = "Lagos",
                Industry = "Technology Services",
                State = "Lagos",
                SubscriberType = Domain.Enums.SubscriberType.Corporate,
                Status = Domain.Enums.Status.Active,
                StatusDesc = Domain.Enums.Status.Active.ToString(),
                PhoneNumber = "08055195377",
            };
            var defaultRole = new Role
            {
                Name = "Super Admin",
                CreatedDate = DateTime.Now,
                CreatedByEmail = "SYSTEM",
                CreatedById = "SYSTEM",
            };
            var defaultUser = new ApplicationUser
            {
                FirstName = "admin",
                LastName = "admin",
                Email = "onyxadmin@reventtechnologies.com",
                EmailConfirmed = true,
                CreatedByEmail = "SYSTEM",
                CreatedById = "SYSTEM",
                CreatedDate = DateTime.Now,

            };
            if (context.Subscribers.All(u => u.Name != defaultOrg.Name))
            {
                var result = await context.AddAsync(defaultOrg);
                await context.SaveChangesAsync();
            }
            if (context.Roles.All(u => u.Name != defaultRole.Name))
            {
                defaultRole.SubscriberId = context.Subscribers.FirstOrDefaultAsync().Result.Id;
                var result =  await context.AddAsync(defaultRole);
                await context.SaveChangesAsync();
            }
            
            
                defaultUser.SubscriberId = context.Subscribers.FirstOrDefaultAsync().Result.Id;
                defaultUser.RoleId = context.Roles.FirstOrDefaultAsync().Result.Id;
                defaultUser.Id = Guid.NewGuid().ToString();
                defaultUser.UserId = Guid.NewGuid().ToString();
                defaultUser.UserName = defaultUser.Email;
                var userResult = await userManager.CreateAsync(defaultUser, "Administrator1!");
                await context.SaveChangesAsync();
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
