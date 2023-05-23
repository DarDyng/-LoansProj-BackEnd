using AutoMapper;
using LoansAppWebApi.Core;
using LoansAppWebApi.Core.Constants;
using LoansAppWebApi.Models.DbModels;
using LoansAppWebApi.Models.DTO_s;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoansAppWebApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public CategoriesController(ApplicationDbContext context, UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();

            var res = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(categories);

            return Ok(res);
        }
    }
}
