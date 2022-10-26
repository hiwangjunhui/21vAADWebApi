using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Test.WebApi.Jobs
{
    public class Job1 : IJob
    {
        private readonly ILogger<Job1> _logger;
        public Job1(ILogger<Job1> logger)
        {
            _logger = logger;
        }

        public Task Execute()
        {
            _logger.LogInformation($"[{DateTime.Now}]: {nameof(Job1)} executed..");
            return Task.CompletedTask;
        }
    }
}
