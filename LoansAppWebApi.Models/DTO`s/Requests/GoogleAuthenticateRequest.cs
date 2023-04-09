using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansAppWebApi.Models.DTO_s.Requests
{
    public class GoogleAuthenticateRequest
    {
        [Required]
        public string IdToken { get; set; }
    }
}
