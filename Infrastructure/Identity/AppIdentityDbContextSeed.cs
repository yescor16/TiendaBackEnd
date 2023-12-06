using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Bob",
                    Email = "Bob@ec",
                    UserName = "Bobuser",
                    Address = new Address {
                          FirstName="as",
                          LastName="er",
                          Street="sd2",
                          City="345",
                          State="ert",
                          ZipCode="345"
                    }
                };
                await userManager.CreateAsync(user,"Pa$$w0rd");
            }
        

        }

    }
}
