using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace HttpServerDemo
{
    class WebAPIStart
    {
        public static void StartWebAPI(int port)
        {
            string url = string.Format("http://*:{0}", port);
            var host = new WebHostBuilder()
               .UseKestrel()
               .UseContentRoot(Directory.GetCurrentDirectory())
               .UseStartup<Startup>()
               .UseUrls(url)
               .Build();
            host.RunAsync();
        }

        public static void StartWebAPI2(int port)
        {
            string url = string.Format("http://*:{0}", port);
            Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseKestrel()
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseStartup<Startup>()
                        .UseUrls(url)
                        .ConfigureLogging(logging =>
                        {
                            logging.SetMinimumLevel(LogLevel.Information);
                        });
                }).Build().RunAsync();
        }
    }
}
