using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoansAppWebApi.Models.Errors;

namespace LoansAppWebApi.Core.Filters
{
    public class ValidateModelFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Select(b => new ValidationError { Name = b.Key, Errors = b.Value?.Errors.Select(x => x.ErrorMessage) }).ToArray();
                context.Result = new BadRequestObjectResult(errors);
            }
        }
    }
}
