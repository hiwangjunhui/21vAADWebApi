using System;
using System.Threading.Tasks;

namespace Test.WebApi.Jobs
{
    public interface IJob
    {
        public Task Execute();
    }
}
