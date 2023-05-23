using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LoansAppWebApi.Models.DTO_s.Statistic;

namespace LoansAppWebApi.Models.DTO_s
{
    public class StatisticDTO
    {
        public float TotalDebt { get; set; }
        public int TotalLoans { get; set; }
        public CategoriesContainer CategoriesContainer { get; set; }
    }
}
