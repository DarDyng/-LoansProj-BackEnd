using System.Security.Claims;
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
using AutoMapper;
using LoansAppWebApi.Models.DTO_s;
using LoansAppWebApi.Models.DTO_s.Resposnes;

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
        private readonly IMapper _mapper;


        public LoansController(ApplicationDbContext context,
            ILogger<LoansController> logger,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
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

                var loanToAdd = new Loans
                {
                    Id = Guid.NewGuid(),
                    StartDate = loanRequest.StartDate,
                    EndDate = loanRequest.EndDate,
                    Name = loanRequest.Name,
                    SumOfLoan = loanRequest.SumOfLoan,
                    PercentsInYear = loanRequest.PercentsInYear,
                    User = user,
                    IsPaid = true
                };

                await _context.AddAsync(loanToAdd);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<Loans, LoanDTO>(loanToAdd));
            }

            return BadRequest("Something went wrong.");
        }

        [HttpDelete("{loanId}")]
        public async Task<IActionResult> DeleteLoan([FromRoute] string loanId)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == AuthConstants.ClaimNames.Id)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("You are not correct user!");
            }

            var id = Guid.Parse(loanId);
            var loan = await _context.Loans.FirstOrDefaultAsync(x => x.Id == id);

            if (loan == null)
                return BadRequest("Loan not found!");

            if (loan.UserId != Guid.Parse(userId))
                return BadRequest("This is not your loan!");

            _context.Loans.Remove(loan);
            
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<Loans, LoanDTO>(loan));
        }

        [HttpPut("{loanId}")]
        public async Task<IActionResult> EditLoan([FromRoute] string loanId, [FromBody] LoanDTO editLoanDto)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == AuthConstants.ClaimNames.Id)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("You are not correct user!");
            }

            var id = Guid.Parse(loanId);
            var loan = await _context.Loans.FirstOrDefaultAsync(x => x.Id == id);

            if (loan == null)
                return BadRequest("Loan not found!");
            
            if (loan.UserId != Guid.Parse(userId))
                return BadRequest("This is not your loan!");

            loan.StartDate = editLoanDto.StartDate;
            loan.EndDate = editLoanDto.EndDate;
            loan.Name = editLoanDto.Name;
            loan.PercentsInYear = editLoanDto.PercentsInYear;
            loan.SumOfLoan = editLoanDto.SumOfLoan;

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<Loans, LoanDTO>(loan));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanDTO>>> GetAll()
        {

            var id = HttpContext.User.Claims.FirstOrDefault(x => x.Type == AuthConstants.ClaimNames.Id)?.Value;
            if (id != null)
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                    return BadRequest("User not exists.");
                else
                {
                    var res = await _context.Loans.Where(x => x.UserId == user.Id).ToListAsync();
                    var mappedRes = _mapper.Map<IEnumerable<Loans>, IEnumerable<LoanDTO>>(res);
                    return Ok(mappedRes);
                }
            }

            return BadRequest("Error");

        }
    }
}
