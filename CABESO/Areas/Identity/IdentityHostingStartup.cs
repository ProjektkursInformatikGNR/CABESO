using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(CABESO.Areas.Identity.IdentityHostingStartup))]
namespace CABESO.Areas.Identity
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