using LoansAppWebApi.Core;
using LoansAppWebApi.Core.Constants;
using LoansAppWebApi.Models.DbModels;
using LoansAppWebApi.Models.DTO_s.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Permissions;

namespace LoansAppWebApi.Controllers
{
    [Route("api/loans")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LoansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LoansController> _logger;
        private readonly UserManager<User> _userManager;

        public LoansController(ApplicationDbContext context,
            ILogger<LoansController> logger,
            UserManager<User> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoan(CreateLoanRequest loanRequest)
        {
            var id = HttpContext.User.Claims.FirstOrDefault(x => x.Type == AuthConstants.ClaimNames.Id)?.Value;

            if (id != null)
            {
                _logger.LogInformation($"User id found: {id}");

                var user = await _userManager.FindByIdAsync(id);
                
                if (user == null)
                    return BadRequest("User not exists.");

                var loadToAdd = new Loans
                {
                    Id = Guid.NewGuid(),
                    StartDate = DateTime.Now,
                    Name = loanRequest.Name,
                    SumOfLoan = loanRequest.SumOfLoan,
                    PercentsInYear = loanRequest.PercentsInYear,
                    User = user,
                };

                await _context.AddAsync(loadToAdd);
                await _context.SaveChangesAsync();

                return Ok();
            }

            return BadRequest("Something went wrong.");
        }

        [HttpGet]
        public async Task<ActionResult<Loans>> GetAll()
        {


            return Ok(await _context.Loans.ToListAsync());
        }
    }
}
