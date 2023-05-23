using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace LoansAppWebApi.Models.DbModels
{
    public class Loans
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public float SumOfLoan { get; set; }
        public float SumOfPaidLoan { get; set; }
        public float PercentsInYear { get; set; }
        public string Name { get; set; }
        public bool IsPaid { get; set; }
        
        [ForeignKey("CategoryId")]
        public Guid CategoryId { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        
        [IgnoreDataMember]
        public Category Category { get; set; }
        [IgnoreDataMember]
        public User User { get; set; }

    }
}