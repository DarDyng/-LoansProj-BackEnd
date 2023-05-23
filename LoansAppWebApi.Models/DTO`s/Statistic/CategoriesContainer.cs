using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LoansAppWebApi.Models.DTO_s.Statistic
{
    public class CategoriesContainer
    {

        public List<CategoryModel> CategoryModels { get; set; } = new List<CategoryModel>();
    }
}
