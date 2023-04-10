using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.JSInterop.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LoansAppWebApi.Core.Filters
{
    public class HttpResponseFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            HttpStatusCode statusCode;
            var stackTrace = string.Empty;
            var messages = new List<string>();

            switch (context.Result)
            {
                case BadRequestObjectResult badRequestObject:
                    {
                        context.HttpContext.Response.ContentType = "application/json";
                        if (badRequestObject.Value is IEnumerable<IdentityError> errors)
                        {
                            context.Result = new JsonResult(errors.Select(x => $"{x.Description}"));
                            return;
                        }
                        else if (badRequestObject.Value is IEnumerable<string> errors2)
                        {
                            context.Result = new BadRequestObjectResult(errors2);
                            return;
                        }
                        else
                        {
                            context.Result = new BadRequestObjectResult(new string[] { badRequestObject.Value?.ToString() ?? string.Empty });
                            return;
                        }
                    };
                case UnauthorizedObjectResult unauthorizedObject:
                    {
                        context.HttpContext.Response.ContentType = "application/json";
                        if (unauthorizedObject.Value is IEnumerable<string> errors)
                        {
                            context.Result = new UnauthorizedObjectResult(errors);
                            return;
                        }
                        //else
                        // it`s just string
                        context.Result = new UnauthorizedObjectResult(new string[] { unauthorizedObject.Value?.ToString() ?? string.Empty });

                        break;
                    }
                default:
                    break;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
