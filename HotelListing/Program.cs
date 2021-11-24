using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(
                path: "c:\\Hotellist\\logs\\log-.txt",
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}]  {Message:lj}{NewLine}{Exception}",
                //paterne zakhire kardan
                rollingInterval:RollingInterval.Day,
                //harfile ro bar asase roz create mikone  mesle log-20211123
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information
                //minimom level baryae log kardan 
                ).CreateLogger();
            try
            {
                Log.Information("Application Is Starting");// avalin log ro neveshtim harbar run mishe inp minevise to file  
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {

                Log.Fatal(ex,"application Failed to start ");//agar khate beshe ino mizane 
            }
           finally
            {
                Log.CloseAndFlush();// az aval shoro mikone ejra kardane barname 
            }
           
        }
        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
