using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoansAppWebApi.Core.Constants;
using Microsoft.AspNetCore.Http;

namespace LoansAppWebApi.Core.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserClaimId(this HttpContext context) =>
            context.User.Claims.FirstOrDefault(x => x.Type == AuthConstants.ClaimNames.Id)?.Value;
    }
}
