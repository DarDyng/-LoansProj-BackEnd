using Autofac.Extras.Moq;
using AutoMapper;
using FakeItEasy;
using LoansAppWebApi.Core.Services;
using LoansAppWebApi.Core;
using LoansAppWebApi.Core.Interfaces;
using LoansAppWebApi.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Moq;

public class LoansTests
{
    [Fact]
    public async Task GetLoanByCorrectLoanId_ValidCall()
    {
        var loanId = Guid.NewGuid();

        var mockLoanService = new Mock<ILaonsService>();
        mockLoanService.Setup(x => x.GetLoanById(loanId))
            .Returns(Task.FromResult(new Loans { Id = loanId }));

        var loan = await mockLoanService.Object.GetLoanById(loanId);

        Assert.True(loan != null);
        Assert.Equal(loan.Id, loanId);
    }


    [Fact]
    public async Task GetLoanByCorrectLoanId_ShouldFail()
    {
        using (var mock = AutoMock.GetLoose())
        {
            var expectedGuid = Guid.NewGuid();

            mock.Mock<ILaonsService>().Setup(
                    x => x.GetLoanById(expectedGuid))
                .Returns(Task.FromResult(new Loans() { Id = Guid.NewGuid() }));

            var loansService = mock.Create<ILaonsService>();

            var actual = await loansService.GetLoanById(expectedGuid);

            Assert.True(expectedGuid != actual.Id);
        }
    }


    [Fact]
    public async Task CreateLoan_ShouldPass()
    {
        using (var mock = AutoMock.GetLoose())
        {
            var loan = new Loans()
            {
                CategoryId = Guid.NewGuid(),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Id = Guid.NewGuid(),
                IsPaid = false
            };

            mock.Mock<ILaonsService>().Setup(
                    x => x.CreateLoan(loan))
                .Returns(Task.FromResult(loan));

            var loansService = mock.Create<ILaonsService>();

            var actual = await loansService.CreateLoan(loan);

            Assert.True(loan.Id == actual.Id);
        }
    }

    [Fact]
    public async Task UpdateLoan_ShouldReturnUpdatedLoan()
    {
        using (var mock = AutoMock.GetLoose())
        {
            var existingLoan = LoansMockItemsProvider.GetRandomLoan();

            var updateLoan = new Loans()
            {
                Id = existingLoan.Id,
                IsPaid = !existingLoan.IsPaid,
                Name = existingLoan.Name
            };

            mock.Mock<ILaonsService>().Setup(x => x.UpdateLoan(updateLoan))
                .Returns(Task.FromResult(new Loans() { Id = updateLoan.Id, IsPaid = updateLoan.IsPaid, Name = updateLoan.Name }));

            var loansService = mock.Create<ILaonsService>();

            var actual = await loansService.UpdateLoan(updateLoan);

            Assert.Equal(actual, updateLoan);
        }
    }

    [Fact]
    public async Task GetLoans_ShouldReturnAllLoansForUser()
    {
        using var mock = AutoMock.GetLoose();

        var userId = Guid.NewGuid();

        var loansForUser = LoansMockItemsProvider.GetLoans(10);

        //mock.Mock<ILaonsService>().Setup(x => x.GetUserLoans(userId))
        //    .Returns(Task.FromResult(loansForUser));
    }


    public static class LoansMockItemsProvider
    {
        public static IEnumerable<Loans> GetLoans(int loansToGenerate)
        {
            var guids = new List<Guid>();
            var random = new Random();

            for (int i = 0; i < loansToGenerate; i++)
            {
                guids.Add(Guid.NewGuid()); ;
            }

            for (int i = 0; i < loansToGenerate; i++)
            {
                yield return new Loans()
                {
                    Id = guids[i],
                    StartDate = DateTime.Now,
                    IsPaid = false,
                    Name = $"Loan - {i}",
                    PercentsInYear = new Random().Next(1, 10),
                    SumOfLoan = random.Next(1000, 10000)
                };
            }
        }

        public static Loans GetRandomLoan()
        {
            return new Loans()
            {
                Id = Guid.NewGuid(),
                IsPaid = false,
                Name = $"Loan 1"
            };
        }
    }
}
