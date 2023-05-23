using LoansAppWebApi.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansAppWebApi.Models.DTO_s
{
    public class LoanDTO
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public float SumOfLoan { get; set; }
        public float SumOfPaidLoan { get; set; }
        public float PercentsInYear { get; set; }
        public string Name { get; set; }
        public string CategoryId { get; set; }
        public bool IsPaid { get; set; }
    }
}
