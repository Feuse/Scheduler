using Microsoft.Extensions.Logging;
using Quartz;
using ServicesModels;
using System;
using System.Threading.Tasks;

namespace Scheduler
{
    public class Scheduler : ServicesInterfaces.Scheduler.IScheduler
    {
        private readonly IScheduler _scheduler;
        private readonly ILogger<Scheduler> _logger;
        private string UniqueId { get; set; }

        public Scheduler(IScheduler scheduler, ILogger<Scheduler> logger)
        {
            _scheduler = scheduler;
            _logger = logger;
        }
        public async Task Schedule(Message message)
        {
            try
            {
                CheckTime(message);

                IJobDetail job = CreateJob(message);

                ITrigger likeTrigger = CreateTrigger(message, job);

                await _scheduler.ScheduleJob(job, likeTrigger);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogTrace(e.StackTrace);
            }
        }

        private static void CheckTime(Message message)
        {
            if (message.Time == DateTime.MinValue || message.Time < DateTime.MinValue)
            {
                message.Time = DateTime.UtcNow;
            }
        }

        private IJobDetail CreateJob(Message message)
        {
            try
            {
                UniqueId = Guid.NewGuid().ToString();

                IJobDetail job = JobBuilder.Create<SchedulerJob>().WithIdentity(UniqueId, "bots").Build();

                job.JobDataMap.Put("message", message);
                return job;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private ITrigger CreateTrigger(Message message, IJobDetail job)
        {
            try
            {
                return TriggerBuilder.Create()
                        .WithIdentity(UniqueId, "bots")
                        .StartAt(message.Time).WithSimpleSchedule(x => x
                        .WithIntervalInHours(24) // tested with interval seconds, working
                        .WithRepeatCount(message.Repeat- 1)) // -1 because the trigger it self is counting as 1 repeat.
                        .ForJob(job)
                        .Build();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
