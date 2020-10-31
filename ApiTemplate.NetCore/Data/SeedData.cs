using ApiTemplate.NetCore.Static;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApi.Data
{
    public static class SeedData
    {
        //private readonly static string _usersAdminName = "admin";
        private readonly static string _adminEmail = "admin@api.com";
        private readonly static string _adminPassword = "P@ssword1";
        //private readonly static string _usersCustomer1Name = "JoeJackson";
        private readonly static string _user1Email = "joeJackson@gmail.com";
        //private readonly static string _usersCustomer2Name = "JillJohnson";
        private readonly static string _user2Email = "jillJohnson@yahoo.com";
        private readonly static string _userPassword = "P@ssword1";


        public async static Task Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
        }

        private async static Task SeedUsers(UserManager<IdentityUser> userManager)
        {
            if(await userManager.FindByEmailAsync(_adminEmail) == null)
            {
                var user = new IdentityUser { UserName = _adminEmail, Email = _adminEmail };
                var result = await userManager.CreateAsync(user, _adminPassword);
                if(result.Succeeded)
                {
                    await userManager.AddToRolesAsync(user, new List<string>() { UserRoles.Administrator, UserRoles.User });
                }
            }

            if (await userManager.FindByEmailAsync(_user1Email) == null)
            {
                var user = new IdentityUser { UserName = _user1Email, Email = _user1Email };
                var result = await userManager.CreateAsync(user, _userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, UserRoles.User);
                }
            }

            if (await userManager.FindByEmailAsync(_user2Email) == null)
            {
                var user = new IdentityUser { UserName = _user2Email, Email = _user2Email };
                var result = await userManager.CreateAsync(user, _userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, UserRoles.User);
                }
            }

        }

        private async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if(!await roleManager.RoleExistsAsync(UserRoles.Administrator))
            {
                var role = new IdentityRole { Name = UserRoles.Administrator };
                await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync(UserRoles.User))
            {
                var role = new IdentityRole { Name = UserRoles.User };
                await roleManager.CreateAsync(role);
            }
        }
    }
}
