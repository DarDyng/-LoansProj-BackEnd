using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoansAppWebApi.Models.DbModels;

namespace LoansWebApi.Tests.Models
{
    public class TestUser : User
    {
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otherUser = (User)obj;
            return AuthType == otherUser.AuthType &&
                   Email == otherUser.Email &&
                   EmailConfirmed == otherUser.EmailConfirmed &&
                   UserName == otherUser.UserName;
        }

        public override int GetHashCode()
        {
            // Implement GetHashCode if needed
            return base.GetHashCode();
        }
    }
}
