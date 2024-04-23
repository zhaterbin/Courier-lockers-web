using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

namespace Courier_lockers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   //生产环境
                   webBuilder.UseUrls("http://*:9501");//监控端口9501下所有IP

                   webBuilder.UseStartup<Startup>();
               })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}