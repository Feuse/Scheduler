using Quartz;
using ServicesInterfaces.Scheduler;
using ServicesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
    public class SchedulerJob : IJob
    {
        private IQueue queue;
        public SchedulerJob(IQueue _queue)
        {
            queue = _queue;
        }
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                JobDataMap jobMap = context.JobDetail.JobDataMap;
                Message message = (Message)jobMap.Get("message");

                queue.QueueMessage(message);
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
