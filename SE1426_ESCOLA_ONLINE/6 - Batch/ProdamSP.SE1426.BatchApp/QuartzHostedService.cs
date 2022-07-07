using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using Quartz.Impl.Matchers;
using Serilog;
using Quartz.Impl;

namespace ProdamSP.SE1426.BatchApp
{
    public class QuartzHostedService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<JobSchedule> _jobSchedules;
        private IScheduler _scheduler;

        public QuartzHostedService(
            ISchedulerFactory schedulerFactory,
            IEnumerable<JobSchedule> jobSchedules,
            IJobFactory jobFactory)
        {
            _schedulerFactory = schedulerFactory;
            _jobSchedules = jobSchedules;
            _jobFactory = jobFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            _scheduler.JobFactory = _jobFactory;

            foreach (var jobSchedule in _jobSchedules)
            {
                var job = CreateJob(jobSchedule);
                var trigger = CreateTrigger(jobSchedule);

                await _scheduler.ScheduleJob(job, trigger, cancellationToken);
            }

            await _scheduler.Start(cancellationToken);

            var jobGroupNames = await _scheduler.GetJobGroupNames();


            if ((_scheduler.IsStarted))
            {
                Log.Information("*************************--- Verificando jobs do Quartz ---");
                foreach (string jobGroup in jobGroupNames)
                {
                    var groupMatcher = GroupMatcher<JobKey>.GroupContains(jobGroup);
                    var jobKeys = _scheduler.GetJobKeys(groupMatcher);



                    foreach (var jobKey in jobKeys.Result)
                    {
                            var detail = _scheduler.GetJobDetail(jobKey);
                            var triggers = _scheduler.GetTriggersOfJob(jobKey);
                            foreach (ITrigger trigger in triggers.Result)
                        {
                            Log.Information("*" + jobKey.Name);
                            //log.Warn("*Job: " + jobGroup);
                            //log.Warn("*" + detail.Description);
                            //log.Warn("*" + trigger.Key.Name);
                            //log.Warn("*" + trigger.Key.Group);
                            //log.Warn("*" + trigger.GetType().Name);
                            //log.Warn("*" + _scheduler.GetTriggerState(trigger.Key));
                            DateTimeOffset? nextFireTime = trigger.GetNextFireTimeUtc();

                            if (nextFireTime.HasValue)
                            {
                                Log.Information("* Próxima Execução ---" + nextFireTime.Value.LocalDateTime.ToString());
                            }

                            DateTimeOffset? previousFireTime = trigger.GetPreviousFireTimeUtc();
                            if (previousFireTime.HasValue)
                            {
                                Log.Information("* Execução Anterior ---" + previousFireTime.Value.LocalDateTime.ToString());
                            }
                        }
                    }
                }
                
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _scheduler.Shutdown(cancellationToken);
        }

        private static ITrigger CreateTrigger(JobSchedule schedule)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{schedule.JobType.FullName}.trigger")
                .WithCronSchedule(schedule.CronExpression)
                .WithDescription(schedule.CronExpression)
                .Build();
        }

        private static IJobDetail CreateJob(JobSchedule schedule)
        {
            var jobType = schedule.JobType;
            return JobBuilder
                .Create(jobType)
                .WithIdentity(jobType.FullName)
                .WithDescription(jobType.Name)
                .Build();
        }
    }
}
