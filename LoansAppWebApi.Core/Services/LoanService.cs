using LoansAppWebApi.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LoansAppWebApi.Models.DbModels;
using LoansAppWebApi.Models.DTO_s;
using Microsoft.EntityFrameworkCore;

namespace LoansAppWebApi.Core.Services
{
    public class LoanService : ILaonsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public LoanService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateLoan(Loans loanToAdd)
        {
            await _context.AddAsync(loanToAdd);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LoanDTO>> GetUserLoans(Guid userId) 
            => _mapper.Map<IEnumerable<Loans>, IEnumerable<LoanDTO>>(await _context.Loans.Include(x => x.Category).Where(x => x.UserId == userId).ToListAsync());

        public IQueryable<Loans> GetUserLoansQueriable(Guid userId)
        {
            return _context.Loans.Include(x => x.Category)
                .Include(x => x.User).Where(x => x.User.Id == userId);
        }

        public async Task<Loans> GetLoanById(Guid id)
            => await _context.Loans.FirstOrDefaultAsync(x => x.Id == id);

        public async Task UpdateLoan(Loans loans)
        {
            _context.Loans.Update(loans);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteLoan(Loans loans)
        {
            _context.Remove(loans);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
