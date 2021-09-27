using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Utills
{
    public class QuartzSettings
    {
        public string Quartz = "Quartz";
        [JsonProperty("quartz.scheduler.instanceName")]
        public string InstanceName { get; set; }

    }
}
