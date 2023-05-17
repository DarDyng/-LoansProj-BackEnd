using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace LoansAppWebApi.Models.DbModels
{
    public class Loans
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal SumOfLoan { get; set; }
        public decimal PercentsInYear { get; set; }
        public string Name { get; set; }
        public bool IsPaid { get; set; }

        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}