using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansAppWebApi.Models.DTO_s.Resposnes
{
    public class AuthenticatedUserResposne
    {
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
    }
}
