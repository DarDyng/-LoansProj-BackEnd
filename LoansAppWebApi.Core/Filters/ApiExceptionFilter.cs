using LoansAppWebApi.Models.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LoansAppWebApi.Core.Filters
{
    public class ApiExceptionFilterAttribute : Attribute, IActionFilter
    {
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.ModelState.IsValid) return;

            Console.WriteLine(context.ToString() + " Executing");
            context.Result = new BadRequestObjectResult(context.ModelState.Select(x => x.Value.Errors.Select(x => x.ErrorMessage)).ToArray());
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                Console.WriteLine(context.ToString() + " Executed");
            }
        }
    }

}