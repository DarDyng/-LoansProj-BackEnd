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


        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Loans)obj;
            return Id == other.Id &&
                   StartDate == other.StartDate &&
                   NullableEquals(EndDate, other.EndDate) &&
                   Math.Abs(SumOfLoan - other.SumOfLoan) < float.Epsilon &&
                   Math.Abs(SumOfPaidLoan - other.SumOfPaidLoan) < float.Epsilon &&
                   Math.Abs(PercentsInYear - other.PercentsInYear) < float.Epsilon &&
                   Name == other.Name &&
                   IsPaid == other.IsPaid &&
                   CategoryId == other.CategoryId &&
                   UserId == other.UserId;
        }

        private bool NullableEquals<T>(T? obj1, T? obj2) where T : struct
        {
            if (obj1.HasValue && obj2.HasValue)
            {
                return obj1.Value.Equals(obj2.Value);
            }

            return obj1.HasValue == obj2.HasValue;
        }
    }
}