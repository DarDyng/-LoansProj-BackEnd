using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LoansAppWebApi.Core.Interfaces;
using LoansAppWebApi.Models.DbModels;
using LoansAppWebApi.Models.DTO_s;
using Microsoft.EntityFrameworkCore;

namespace LoansAppWebApi.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CategoryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategories() =>
            _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(await _context.Categories.ToListAsync());
    }
}
