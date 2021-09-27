using Quartz;
using Quartz.Spi;
using System;
using Microsoft.Extensions.DependencyInjection;


namespace Scheduler
{
    public class AspnetCoreJobFactory : IJobFactory
    {
        protected readonly IServiceProvider _serviceProvider;

        public AspnetCoreJobFactory(IServiceProvider factory)
        {
            _serviceProvider = factory;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                return _serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void ReturnJob(IJob job)
        {

        }
    }
}
