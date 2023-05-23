using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansAppWebApi.Models.DTO_s.Statistic
{
    public class CategoryModel
    {
        public string CategoryName { get; set; }
        public int TotalCount { get; set; }
        public float TotalDebt { get; set; }
        public int PaidLoansCount { get; set; }
        public int UnPaidLoansCount { get; set; }
        public List<LoanStatisticModel> Loans { get; set; }
    }
}
