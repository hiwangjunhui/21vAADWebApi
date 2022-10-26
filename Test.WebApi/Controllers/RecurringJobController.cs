using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Test.WebApi.Jobs;
using Test.WebApi.Models;

namespace Test.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecurringJobController : ControllerBase
    {
        private readonly ILogger<RecurringJobController> _logger;

        public RecurringJobController(ILogger<RecurringJobController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> GetJobs()
        {
            var jobs = GetType().Assembly.GetTypes().Where(t => typeof(IJob).IsAssignableFrom(t) && !t.IsAbstract);
            return jobs.Select(t => t.FullName);
        }

        [HttpPost]
        public bool AddJob(AddJobModel model)
        {
            var type = Type.GetType(model.TypeStr);
            if (type == null || !typeof(IJob).IsAssignableFrom(type)) return false;

            var constructor = type.GetConstructors().FirstOrDefault();
            Expression body = null;
            if (constructor == null)
            {
                body = Expression.Call(null, type.GetMethod(nameof(IJob.Execute)));
            }
            else
            {
                var constructorInfos = constructor.GetParameters();
                var parameters = constructorInfos.Select(t => HttpContext.RequestServices.GetService(t.ParameterType));
                
                var instance = Expression.Constant(Activator.CreateInstance(type, parameters.ToArray()));
                body = Expression.Call(instance, type.GetMethod("Execute"));
            }
            
            Expression<Action> expression = Expression.Lambda<Action>(body);
            RecurringJob.AddOrUpdate(model.TypeStr, expression, model.Cron);
            return true;
        }
    }
}
