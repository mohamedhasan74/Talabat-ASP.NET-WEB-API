using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Data.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task UserDataSeedAsync(UserManager<AppUser> userManager)
        {
            if(userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    DisplayName = "Mo7422",
                    UserName = "Mo7422",
                    Email = "mo7422@gmail.com",
                    PhoneNumber = "01004647312"
                };
                await userManager.CreateAsync(user, "Mo@7422");
            }
        }
    }
}
