using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LoansAppWebApi.Models.DbModels;
using LoansAppWebApi.Models.DTO_s;

namespace LoansAppWebApi.Core.Configuration.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDTO>()
                .ForMember(x => x.Name, opt => opt.MapFrom(y => y.CategoryName.ToString()));
        }
    }
}
