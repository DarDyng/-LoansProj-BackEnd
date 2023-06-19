using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansAppWebApi.Models.Exceptions
{
    public class UserUsernameAlreadyExistsException : Exception
    {
        public UserUsernameAlreadyExistsException(string message) : base(message) { }
    }
}
