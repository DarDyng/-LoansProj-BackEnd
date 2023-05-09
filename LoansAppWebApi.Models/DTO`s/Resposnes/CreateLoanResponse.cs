using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansAppWebApi.Models.DTO_s.Resposnes
{
    public class CreateLoanResponse
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required(ErrorMessage = "Sum is required")]
        public decimal SumOfLoan { get; set; }
        [Required(ErrorMessage = "Percents is required")]
        public decimal PercentsInYear { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;
        public bool IsPaid { get; set; }
    }
}
