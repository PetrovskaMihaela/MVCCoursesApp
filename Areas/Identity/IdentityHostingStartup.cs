using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniCoursesApp.Areas.Identity.Data;
using UniCoursesApp.Models;

[assembly: HostingStartup(typeof(UniCoursesApp.Areas.Identity.IdentityHostingStartup))]
namespace UniCoursesApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<UniCoursesAppContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("UniCoursesAppContext")));

               });
        }
    }
}