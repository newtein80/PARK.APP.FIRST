using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PARK.APP.FIRST.Data;
using PARK.APP.FIRST.Data.SeedData;

namespace PARK.APP.FIRST
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //// default generate
            //CreateWebHostBuilder(args).Build().Run();

            var host = CreateWebHostBuilder(args).Build();

            //// https://nbarbettini.gitbooks.io/little-asp-net-core-book/content/chapters/security-and-identity/authorization-with-roles.html
            //InitializeDatabase(host);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    context.Database.Migrate();
                    //UserInfo_SeedData.Initialize(services);

                    //// https://www.youtube.com/watch?v=yydVHt2OKHk
                    //Identity_SeedData.InitializeRoleAsyncYoutube(context, services).Wait();

                    //// https://romansimuta.com/blogs/blog/authorization-with-roles-in-asp.net-core-mvc-web-application
                    //Identity_SeedData.InitializeRoleAsync(context, services).Wait();

                    //// https://gooroo.io/GoorooTHINK/Article/17333/Custom-user-roles-and-rolebased-authorization-in-ASPNET-core/32835#.W-zpcegzaUk
                    //// Account, Role Seed Data Create
                    //var serviceProvider = services.GetRequiredService<IServiceProvider>();
                    //var configuration = services.GetRequiredService<IConfiguration>();
                    //Identity_SeedData.CreateRoles(serviceProvider, configuration).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        //private static void InitializeDatabase(IWebHost host)
        //{
        //    using (var scope = host.Services.CreateScope())
        //    {
        //        var services = scope.ServiceProvider;

        //        try
        //        {
        //            Identity_SeedData.InitializeAsync(services).Wait();
        //        }
        //        catch (Exception ex)
        //        {
        //            var logger = services
        //                .GetRequiredService<ILogger<Program>>();
        //            logger.LogError(ex, "Error occurred seeding the DB.");
        //        }
        //    }
        //}
    }
}
