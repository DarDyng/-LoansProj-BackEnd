using LoansAppWebApi.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansAppWebApi.Models.DTO_s.Requests
{
    public class CreateLoanRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "Sum is required")]
        public float SumOfLoan { get; set; }
        [Required(ErrorMessage = "Percents is required")]
        public float PercentsInYear { get; set; }
        [Required(ErrorMessage = "CategoryName is required")]
        public string Name { get; set; } = null!;

        public Guid CategoryId { get; set; }
    }
}