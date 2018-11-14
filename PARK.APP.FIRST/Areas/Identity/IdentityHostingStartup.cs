using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PARK.APP.FIRST.Data;
using PARK.APP.FIRST.Models.ApplicationModel;

[assembly: HostingStartup(typeof(PARK.APP.FIRST.Areas.Identity.IdentityHostingStartup))]
namespace PARK.APP.FIRST.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}