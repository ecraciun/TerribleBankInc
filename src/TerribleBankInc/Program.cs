using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace TerribleBankInc;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            //.ConfigureWebHost(c => c.UseKestrel(k => k.AddServerHeader = false))
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
