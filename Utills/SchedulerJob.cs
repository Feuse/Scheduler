using Microsoft.Extensions.Logging;
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
        private readonly ILogger<SchedulerJob> _logger;
        private IQueue queue;
        public SchedulerJob(IQueue _queue, ILogger<SchedulerJob> logger)
        {
            queue = _queue;
            _logger = logger;
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
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogTrace(e.StackTrace);
                throw;
            }
        }
    }
}
