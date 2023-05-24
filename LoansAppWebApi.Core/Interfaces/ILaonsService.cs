using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoansAppWebApi.Models.DbModels;
using LoansAppWebApi.Models.DTO_s;

namespace LoansAppWebApi.Core.Interfaces
{
    public interface ILaonsService
    {
        Task CreateLoan(Loans loanToAdd);
        Task<IEnumerable<LoanDTO>> GetUserLoans(Guid userId);
        IQueryable<Loans> GetUserLoansQueriable(Guid userId);
        Task<Loans> GetLoanById(Guid id);

        Task UpdateLoan(Loans loans);

        Task<bool> DeleteLoan(Loans loans);
    }
}
