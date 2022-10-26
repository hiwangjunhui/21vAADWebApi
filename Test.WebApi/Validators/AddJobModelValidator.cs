using System;
using FluentValidation;
using Test.WebApi.Models;

namespace Test.WebApi.Validators
{
    public class AddJobModelValidator : AbstractValidator<AddJobModel>
    {
        public AddJobModelValidator()
        {
            RuleFor(x => x.TypeStr).NotNull().Custom((p, context) =>
            {
                var type = Type.GetType(p);
                if (type == null)
                {
                    context.AddFailure("don't use the type any more, aiden said.");
                }
            });
            RuleFor(x => x.Cron).NotNull().Custom((p, context) =>
            {
                try
                {
                    Cronos.CronExpression.Parse(p);
                }
                catch (Exception)
                {
                    context.AddFailure("the format of cron expression is invalid.");
                }
            });
        }
    }
}
