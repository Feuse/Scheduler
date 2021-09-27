using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using ServicesInterfaces.Global;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;

namespace Scheduler
{
    public class QuartzInstance
    {
        private static IConfigurationRoot Configuration;
        private static IScheduler _instace = null;
        private static readonly object _padlock = new object();

        public static IScheduler Instance
        {
            get
            {

                lock (_padlock)
                {
                    if (_instace == null)
                    {
                        NameValueCollection settings = GetSchedulerSettings();

                        StdSchedulerFactory sche = new StdSchedulerFactory(settings);
                        _instace = sche.GetScheduler().GetAwaiter().GetResult();
                        _instace.Start();
                    }
                }
                return _instace;
            }
        }

        private static NameValueCollection GetSchedulerSettings()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            var values = Configuration.GetSection("QuartzSettings").GetChildren();
            NameValueCollection collection = new NameValueCollection();
            foreach (var item in values)
            {
                collection.Add(item.Key, item.Value);
            }

            return collection;
        }
    }
}
