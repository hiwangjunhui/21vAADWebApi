using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Test.WebApi.Jobs
{
    public class Job2 : IJob
    {
        public Task Execute()
        {
            Console.WriteLine($"[{DateTime.Now}]: {nameof(Job2)} executed..");
            return Task.CompletedTask;
        }
    }
}
