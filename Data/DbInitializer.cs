using Fall_2022_Lab_1_000818994.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Fall_2022_Lab_1_000818994.Data
{
    public static class DbInitializer
    {

        public static string uri = @"https://lab2-000818994.vault.azure.net/";

        public static async Task<int> SeedUsersAndRoles(IServiceProvider serviceProvider)
        {
            // create the database if it doesn't exist
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Check if roles already exist and exit if there are
            if (roleManager.Roles.Count() > 0)
                return 1;  // should log an error message here

            // Seed roles
            int result = await SeedRoles(roleManager);
            if (result != 0)
                return 2;  // should log an error message here

            // Check if users already exist and exit if there are
            if (userManager.Users.Count() > 0)
                return 3;  // should log an error message here

            // Seed users
            result = await SeedUsers(userManager);
            if (result != 0)
                return 4;  // should log an error message here

            return 0;
        }

        private static async Task<int> SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            // Create Manager Role
            var result = await roleManager.CreateAsync(new IdentityRole("Manager"));
            if (!result.Succeeded)
                return 1;  // should log an error message here

            // Create Member Role
            result = await roleManager.CreateAsync(new IdentityRole("Player"));
            if (!result.Succeeded)
                return 2;  // should log an error message here

            return 0;
        }

        private static async Task<int> SeedUsers(UserManager<ApplicationUser> userManager)
        {

            var client = new SecretClient(new Uri(uri), new DefaultAzureCredential());

            // Create Manager User
            var managerUser = new ApplicationUser
            {
                UserName = "the.manager@gmail.ca",
                Email = "the.manager@gmail.ca",
                FirstName = "The",
                LastName = "Manager",
                BirthDate = "05/22/2000",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(managerUser, client.GetSecret("adminPass").Value.Value);
            if (!result.Succeeded)
                return 1;  // should log an error message here

            // Assign user to Manager role
            result = await userManager.AddToRoleAsync(managerUser, "Manager");
            if (!result.Succeeded)
                return 2;  // should log an error message here

            // Create Player User
            var playerUser = new ApplicationUser
            {
                UserName = "the.player@hotmail.ca",
                Email = "the.player@hotmail.ca",
                FirstName = "The",
                LastName = "Player",
                BirthDate = "01/12/2001",
                EmailConfirmed = true
            };
            result = await userManager.CreateAsync(playerUser, client.GetSecret("userPass").Value.Value);
            if (!result.Succeeded)
                return 3;  // should log an error message here

            // Assign user to Player role
            result = await userManager.AddToRoleAsync(playerUser, "Player");
            if (!result.Succeeded)
                return 4;  // should log an error message here

            return 0;
        }
    }
}
