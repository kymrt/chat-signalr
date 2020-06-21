using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chat_signalr.Handlers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils.Messaging;

namespace chat_signalr
{
    public class Program
    {
        //We define thats here because we dont want to lose our client until server restart.
        public static UserHandler UserHandler = new UserHandler();
        public static MessageHandler MessageHandler = new MessageHandler();

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
