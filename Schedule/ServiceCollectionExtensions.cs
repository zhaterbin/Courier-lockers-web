using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSchedule(this IServiceCollection services)
        {
            services.AddSingleton<ScheduleEngine>();
            services.AddSingleton<IScheduleEngine>(p => p.GetService<ScheduleEngine>()!);
            services.AddHostedService(p => p.GetService<ScheduleEngine>()!);
            return services;
        }
    }
}
