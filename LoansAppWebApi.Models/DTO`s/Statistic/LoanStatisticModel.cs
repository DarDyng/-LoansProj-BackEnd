using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansAppWebApi.Models.DTO_s.Statistic
{
    public class LoanStatisticModel
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public float SumOfLoan { get; set; }
        public float SumOfPaidLoan { get; set; }
        public decimal PercentsInYear { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public bool IsPaid { get; set; }

        public float PercentagePaid
        {
            get
            {
                if (SumOfLoan == 0)
                {
                    return 0; // Handle the case when there is no loan amount
                }
                else
                {
                    return (SumOfPaidLoan / SumOfLoan) * 100;
                }
            }
        }
    }
}
