using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LoansAppWebApi.Models.DbModels
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        public Categories CategoryName { get; set; }

        public ICollection<Loans> Loans { get; set; }

    }


    public enum Categories
    {
        Auto,
        Personal,
        Mortgage,
        Student,
        Payday,
        Pawn,
        Business
    }
}
