using LoansAppWebApi.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansAppWebApi.Models.DTO_s.Resposnes
{
    public class CreateUserResponse
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public User? User { get; set; }
    }
}
