//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using YemekSepeti.Models;

//namespace YemekSepeti.Services
//{
//    public class AccountService
//    {
//        public static async Task SeedIdentityData(IServiceProvider serviceProvider)
//        {
//            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
//            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser<int>>>();
//            var context = serviceProvider.GetRequiredService<YemekSepetContext>();

//            var roles = new List<string> { "Customer", "Restaurant", "DeliveryPersonnel", "SuperAdmin" };
//            foreach (var role in roles)
//            {
//                if (!await roleManager.RoleExistsAsync(role))
//                {
//                    await roleManager.CreateAsync(new IdentityRole<int>(role));
//                }
//            }
//        }
//    }
//    //       var users = new List<IdentityUser<int>>
//    //       {
//    //            new IdentityUser<int> { Id = -1, UserName = "Maydonoz Döner", Email = "maydanozdoner@gmail.com", PhoneNumber = "(0442) 238 23 22",SecurityStamp = Guid.NewGuid().ToString(),PasswordHash=new PasswordHasher<IdentityUser<int>>().HashPassword(null,"Maydanozdoner1234!") },
//    //            new IdentityUser<int> { Id = -2, UserName = "Musqa Burger", Email = "musqaburger@gmail.com", PhoneNumber = "0545 442 85 25",SecurityStamp = Guid.NewGuid().ToString(),PasswordHash=new PasswordHasher<IdentityUser<int>>().HashPassword(null,"Musqaburger1234!") },
//    //            new IdentityUser<int> { Id = -3, UserName = "Burger King", Email = "burgerking@gmail.com", PhoneNumber = "444 5 464",SecurityStamp = Guid.NewGuid().ToString(),PasswordHash=new PasswordHasher<IdentityUser<int>>().HashPassword(null,"Burgerking1234!") },
//    //            new IdentityUser<int> { Id = -4, UserName = "Cheff Pizza", Email = "cheffpizza@gmail.com", PhoneNumber = "0553 697 79 42",SecurityStamp = Guid.NewGuid().ToString(),PasswordHash=new PasswordHasher<IdentityUser<int>>().HashPassword(null,"Cheffpizza1234!") },
//    //            new IdentityUser<int> { Id = -5, UserName = "Popeyes", Email = "popeyes@gmail.com", PhoneNumber = "444 7 677",SecurityStamp = Guid.NewGuid().ToString(),PasswordHash=new PasswordHasher<IdentityUser<int>>().HashPassword(null,"Popeyes1234!") },
//    //            new IdentityUser<int> { Id = -6, UserName = "Öncü Döner", Email = "oncudoner@gmail.com", PhoneNumber = "(0442) 233 05 05",SecurityStamp = Guid.NewGuid().ToString(),PasswordHash=new PasswordHasher<IdentityUser<int>>().HashPassword(null,"Oncudoner1234!") },
//    //            new IdentityUser<int> { Id = -7, UserName = "Lapidis Pide", Email = "lapidispide@gmail.com", PhoneNumber = "(0442) 215 48 25",SecurityStamp = Guid.NewGuid().ToString(),PasswordHash=new PasswordHasher<IdentityUser<int>>().HashPassword(null,"Lapidispide1234!") },
//    //            new IdentityUser<int> { Id = -8, UserName = "Usta Dönerci", Email = "ustadonerci@gmail.com", PhoneNumber = "(0442) 343 99 99",SecurityStamp = Guid.NewGuid().ToString(),PasswordHash=new PasswordHasher<IdentityUser<int>>().HashPassword(null,"Ustadonerci1234!") },
//    //            new IdentityUser<int> { Id = -9, UserName = "Fatime Murtuzova", Email = "fatimemurtuzova@gmail.com", PhoneNumber = "05053661826",SecurityStamp = Guid.NewGuid().ToString(),PasswordHash=new PasswordHasher<IdentityUser<int>>().HashPassword(null,"Fatime1234!") },
//    //            new IdentityUser<int> { Id = -10, UserName = "Hakan Yıldız", Email = "hakanyildiz@gmail.com", PhoneNumber = "0538 613 36 08",SecurityStamp = Guid.NewGuid().ToString(),PasswordHash=new PasswordHasher<IdentityUser<int>>().HashPassword(null,"Hakan1234!") }
//    //       };

//    //        //using (var transaction = await context.Database.BeginTransactionAsync())
//    //        //{

//    //        //    await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT AspNetUsers ON");

//    //        //    foreach (var user in users)
//    //        //    {
//    //        //        if (await userManager.FindByEmailAsync(user.Email) == null)
//    //        //        {
//    //        //            context.Users.Add(user);
//    //        //        }
//    //        //    }

//    //        //    await context.SaveChangesAsync();

//    //        //    await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT AspNetUsers OFF");

//    //        //    await transaction.CommitAsync();
//    //        //}

//    //        foreach (var user in users)
//    //        {
//    //            if (user.UserName.Contains("Döner") || user.UserName.Contains("Burger") || user.UserName.Contains("Pizza") || user.UserName.Contains("Pide"))
//    //            {
//    //                await userManager.AddToRoleAsync(user, "Restaurant");
//    //            }
//    //            else if (user.UserName == "Hakan Yıldız")
//    //            {
//    //                await userManager.AddToRoleAsync(user, "DeliveryPersonnel");
//    //            }
//    //            else if (user.UserName == "Fatime Murtuzova")
//    //            {
//    //                await userManager.AddToRoleAsync(user, "Customer");
//    //            }
//    //        }
//    //        await context.SaveChangesAsync();
//    //    }
//    //}
//}
