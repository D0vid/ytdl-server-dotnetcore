﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace dds_ytdl_server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("System.Net.Http.UseSocketsHttpHandler", false);
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
