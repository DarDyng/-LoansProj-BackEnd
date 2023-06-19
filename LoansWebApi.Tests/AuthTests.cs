using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoansAppWebApi.Core.Interfaces;
using LoansAppWebApi.Core.Services;
using LoansAppWebApi.Models.DbModels;
using LoansAppWebApi.Models.DTO_s.Resposnes;
using LoansAppWebApi.Models.Exceptions;
using LoansWebApi.Tests.Models;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace LoansWebApi.Tests
{
    public class AuthTests
    {

        [Fact]
        public async Task CreateUser_ShouldPass()
        {
            // Arrange
            var userToCreate = new TestUser()
            {
                AuthType = AuthType.Google,
                Email = "mishkafreddy123@gmail.com",
                EmailConfirmed = false,
                UserName = "Amigo"
            };
            var userPassword = "SuperPassword!";

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(x => x
                .CreateUser(userToCreate, userPassword))
                .Returns(Task.FromResult(new CreateUserResponse() { Succeeded = true, User = userToCreate }));


            // Act
            var actual = await mockUserService.Object.CreateUser(userToCreate, userPassword);
            
            // Assert

            Assert.True(actual != null);
            Assert.True(actual.Succeeded);
            Assert.Equal((TestUser)actual.User, userToCreate);
        }
    }
}
