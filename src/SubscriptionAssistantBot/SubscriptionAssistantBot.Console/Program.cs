using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel;
using System.IO;

namespace SubscriptionAssistantBot.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            var token = configuration["BotApiToken"];
            var bot = new Bot.Bot(token);
            System.Console.ReadLine();
        }

        
    }
}
