using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansAppWebApi.Core.Constants
{
    public class AuthConstants
    {
        public static class UserRoles
        {
            public const string Admin = "Admin";
            public const string User = "User";
            public const string Artist = "Artist";
        }

        public static class ClaimNames
        {
            public const string Id = "Id";
        }
    }
}
