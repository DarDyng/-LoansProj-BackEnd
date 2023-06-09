﻿using System.Security.Claims;
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
using LoansAppWebApi.Core.Extensions;
using LoansAppWebApi.Core.Interfaces;
using LoansAppWebApi.Models.DTO_s;
using LoansAppWebApi.Models.DTO_s.Resposnes;
using LoansAppWebApi.Models.DTO_s.Statistic;

namespace LoansAppWebApi.Controllers
{
    [Route("api/loans")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LoansController : ControllerBase
    {
        private readonly ILogger<LoansController> _logger;
        private readonly IUserService _userService;
        private readonly ILaonsService _loansService;
        private readonly IMapper _mapper;

        public LoansController(
            ILogger<LoansController> logger,
            IUserService userService,
            ILaonsService loansService,
            IMapper mapper)
        {
            _loansService = loansService;
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoan(CreateLoanRequest loanRequest)
        {

            // you can add this as extension for HttpContext
            var id = HttpContext.User.Claims.FirstOrDefault(x => x.Type == AuthConstants.ClaimNames.Id)?.Value;

            if (id != null)
            {
                _logger.LogInformation($"User id found: {id}");

                var user = await _userService.GetUserById(Guid.Parse(id));

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
                    CategoryId = loanRequest.CategoryId,
                    IsPaid = false
                };

                await _loansService.CreateLoan(loanToAdd);

                return Ok(_mapper.Map<Loans, LoanDTO>(loanToAdd));
            }

            return BadRequest("Something went wrong.");
        }

        [HttpGet("getLoanStatistic")]
        [AllowAnonymous]
        public async Task<ActionResult<StatisticDTO>> GetStatistic()
        {
            var id = HttpContext.User.Claims.FirstOrDefault(x => x.Type == AuthConstants.ClaimNames.Id)?.Value;

            if (id != null)
            {
                _logger.LogInformation($"User id found: {id}");

                var user = await _userService.GetUserById(Guid.Parse(id));

                if (user == null)
                    return BadRequest("User not exists.");

                var statistic = new StatisticDTO();

                statistic.CategoriesContainer = await ConstructContainer(statistic, user.Id);

                statistic.TotalDebt = statistic.CategoriesContainer.CategoryModels.Sum(x => x.TotalDebt);
                statistic.TotalLoans = statistic.CategoriesContainer.CategoryModels.Sum(x => x.TotalCount);

                return Ok(statistic);
            }

            return BadRequest("User statistic not found");
        }

        private async Task<CategoriesContainer> ConstructContainer(StatisticDTO statisticDto, Guid userId)
        {
            var container = new CategoriesContainer();
            container.CategoryModels = new List<CategoryModel>(); // Initialize the list

            var loans = _loansService.GetUserLoansQueriable(userId);

            var groups = loans.GroupBy(x => x.Category);

            await groups.ForEachAsync(group =>
            {
                var categoryModel = new CategoryModel();

                categoryModel.CategoryName = group.Key.CategoryName.ToString();
                categoryModel.PaidLoansCount = group.Count(x => x.IsPaid);
                categoryModel.TotalDebt = group.Sum(x => !x.IsPaid ? x.SumOfLoan : 0);
                categoryModel.TotalCount = group.Count();
                categoryModel.UnPaidLoansCount = group.Count(x => !x.IsPaid);
                categoryModel.Loans = _mapper.Map<List<Loans>, List<LoanStatisticModel>>(group.ToList());

                container.CategoryModels.Add(categoryModel);
            });

            return container;
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
            var loan = await _loansService.GetLoanById(id);

            if (loan == null)
                return BadRequest("Loan not found!");

            if (loan.UserId != Guid.Parse(userId))
                return BadRequest("This is not your loan!");

            await _loansService.DeleteLoan(loan);

            return Ok(_mapper.Map<Loans, LoanDTO>(loan));
        }

        [HttpPut("{loanId}")]
        public async Task<IActionResult> EditLoan([FromRoute] string loanId, [FromBody] LoanDTO editLoanDto)
        {
            var userId = HttpContext.GetUserClaimId();

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("You are not correct user!");
            }

            var loan = await _loansService.GetLoanById(Guid.Parse(loanId));

            if (loan == null)
                return BadRequest("Loan not found!");

            if (loan.UserId != Guid.Parse(userId))
                return BadRequest("This is not your loan!");

            loan.StartDate = editLoanDto.StartDate;
            loan.EndDate = editLoanDto.EndDate;
            loan.Name = editLoanDto.Name;
            loan.PercentsInYear = editLoanDto.PercentsInYear;
            loan.SumOfLoan = editLoanDto.SumOfLoan;
            loan.SumOfPaidLoan = editLoanDto.SumOfPaidLoan;
            if (loan.SumOfLoan == loan.SumOfPaidLoan)
                loan.IsPaid = true;

            await _loansService.UpdateLoan(loan);

            var loanMapped = _mapper.Map<Loans, LoanDTO>(loan);

            return Ok(loanMapped);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanDTO>>> GetAll()
        {
            var id = HttpContext.GetUserClaimId();
            if (id != null)
            {
                var user = await _userService.GetUserById(Guid.Parse(id));

                if (user == null)
                    return BadRequest("User not exists.");
                else
                {
                    return Ok(await _loansService.GetUserLoans(Guid.Parse(id)));
                }
            }

            return BadRequest("Error");

        }
    }
}
