using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PARK.APP.FIRST.Models.ApplicationModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PARK.APP.FIRST.Data.SeedData
{
    public static class Identity_SeedData
    {
        #region+ InitializeRoleAsync : https://romansimuta.com/blogs/blog/authorization-with-roles-in-asp.net-core-mvc-web-application
        //public static async Task InitializeRoleAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        //{
        //    context.Database.EnsureCreated();

        //    var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        //    string[] roleNames = { "Admin", "Manager", "Member" };

        //    IdentityResult roleResult;
        //    foreach (var roleName in roleNames)
        //    {
        //        var roleExist = await RoleManager.RoleExistsAsync(roleName);
        //        if (!roleExist)
        //        {
        //            roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
        //        }
        //    }
        //    var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

        //    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        //    var _user = await userManager.FindByEmailAsync(config.GetSection("UserSettings")["UserEmail"]);

        //    if (_user == null)
        //    {
        //        var poweruser = new ApplicationUser
        //        {
        //            UserName = config.GetSection("UserSettings")["UserEmail"],
        //            Email = config.GetSection("UserSettings")["UserEmail"]
        //        };
        //        string UserPassword = config.GetSection("UserSettings")["UserPassword"];
        //        var createPowerUser = await userManager.CreateAsync(poweruser, UserPassword);
        //        if (createPowerUser.Succeeded)
        //        {
        //            await userManager.AddToRoleAsync(poweruser, "Admin");
        //        }
        //    }
        //}
        #endregion

        #region+ CreateRoles : https://gooroo.io/GoorooTHINK/Article/17333/Custom-user-roles-and-rolebased-authorization-in-ASPNET-core/32835#.W-zpcegzaUk
        //public static async Task CreateRoles(IServiceProvider serviceProvider, IConfiguration Configuration)
        //{
        //    //adding customs roles
        //    var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //    var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        //    string[] roleNames = { "Admin", "Manager", "Member" };
        //    IdentityResult roleResult;

        //    foreach (var roleName in roleNames)
        //    {
        //        //creating the roles and seeding them to the database
        //        var roleExist = await RoleManager.RoleExistsAsync(roleName);
        //        if (!roleExist)
        //        {
        //            roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
        //        }
        //    }

        //    //creating a super user who could maintain the web app
        //    var poweruser = new ApplicationUser
        //    {
        //        UserName = Configuration.GetSection("UserSettings")["UserEmail"],
        //        Email = Configuration.GetSection("UserSettings")["UserEmail"]
        //    };

        //    // default Admin User Seed Data Create
        //    string userPassword = Configuration.GetSection("UserSettings")["UserPassword"];
        //    var user = await UserManager.FindByEmailAsync(Configuration.GetSection("UserSettings")["UserEmail"]);

        //    if (user == null)
        //    {
        //        var createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);
        //        if (createPowerUser.Succeeded)
        //        {
        //            //here we tie the new user to the "Admin" role 
        //            await UserManager.AddToRoleAsync(poweruser, "Admin");

        //        }
        //    }
        //}
        #endregion

        #region+ InitializeAsync : https://nbarbettini.gitbooks.io/little-asp-net-core-book/content/chapters/security-and-identity/authorization-with-roles.html
        //public static async Task InitializeAsync(IServiceProvider services)
        //{
        //    var roleManager = services
        //        .GetRequiredService<RoleManager<IdentityRole>>();
        //    await EnsureRolesAsync(roleManager);

        //    var userManager = services
        //        .GetRequiredService<UserManager<ApplicationUser>>();
        //    await EnsureTestAdminAsync(userManager);
        //}

        //private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
        //{
        //    var alreadyExists = await roleManager
        //        .RoleExistsAsync(Constants.AdministratorRole);

        //    if (alreadyExists) return;

        //    await roleManager.CreateAsync(
        //        new IdentityRole(Constants.AdministratorRole));
        //}

        //private static async Task EnsureTestAdminAsync(UserManager<ApplicationUser> userManager)
        //{
        //    var testAdmin = await userManager.Users
        //        .Where(x => x.UserName == "admin@todo.local")
        //        .SingleOrDefaultAsync();

        //    if (testAdmin != null) return;

        //    testAdmin = new ApplicationUser
        //    {
        //        UserName = "admin@todo.local",
        //        Email = "admin@todo.local"
        //    };
        //    await userManager.CreateAsync(
        //        testAdmin, "NotSecure123!!");
        //    await userManager.AddToRoleAsync(
        //        testAdmin, Constants.AdministratorRole);
        //}
        #endregion

        #region+ InitializeRoleAsyncYoutube : https://www.youtube.com/watch?v=yydVHt2OKHk
        //public static async Task InitializeRoleAsyncYoutube(ApplicationDbContext context, IServiceProvider serviceProvider)
        //{
        //    RoleManager<IdentityRole> RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        //    string[] roleNames = { "Admin", "Manager", "Member" };

        //    IdentityResult roleResult;

        //    foreach(var roleName in roleNames)
        //    {
        //        bool roleExist = await RoleManager.RoleExistsAsync(roleName);

        //        if(!roleExist)
        //        {
        //            roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
        //        }
        //    }
        //}
        #endregion
    }
}
