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
            if (!context.ModelState.IsValid)
            {
                Console.WriteLine(context.ToString() + " Executing");
                context.Result = new BadRequestObjectResult(context.ModelState.Select(x => x.Value.Errors.Select(x => x.ErrorMessage)).ToArray());
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                Console.WriteLine(context.ToString() + " Executed");
            }
        }

        //public override void OnException(ExceptionContext context)
        //{
        //    HttpStatusCode statusCode;
        //    var stackTrace = string.Empty;
        //    var messages = new List<string>();

        //    switch (context.Exception)
        //    {
        //        case BadRequestException argumentException:
        //            {
        //                messages.Add(argumentException.Message);
        //                statusCode = HttpStatusCode.BadRequest;
        //                stackTrace = argumentException.StackTrace;
        //                break;
        //            }
        //        case UnauthorizedException:
        //            {
        //                if (!string.IsNullOrEmpty(notFoundException.Message) && notFoundException.Errors.Count < 1)
        //                {
        //                    messages.Add(notFoundException.Message);
        //                }
        //                else if (notFoundException.Errors.Count > 1)
        //                {
        //                    messages.AddRange(notFoundException.Errors);
        //                }
        //                else
        //                {
        //                    messages.AddRange(notFoundException.Errors);
        //                }
        //                statusCode = HttpStatusCode.NotFound;
        //                stackTrace = notFoundException.StackTrace;
        //                break;
        //            }
        //        default:
        //            messages.Add(ex.Message);
        //            statusCode = HttpStatusCode.InternalServerError;
        //            stackTrace = ex.StackTrace;
        //            break;
        //        default:
        //            break;
        //    }
        //    base.OnException(context);
        //}


    }

}
public class BadRequestException : Exception
{
    public IEnumerable<ValidationError> Errors { get; }

    public BadRequestException(string message) : base(message)
    {
    }

    public BadRequestException(IEnumerable<ValidationError> errors)
    {
        Errors = errors;
    }
}

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message)
    {
    }
}
