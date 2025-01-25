using AutoFixture;
using Carteiras_Digitais.Application.Helpers;
using Carteiras_Digitais.Application.Services;
using Carteiras_Digitais.Core.Domain.Interfaces;
using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Repositories;
using Carteiras_Digitais.Infrasctruture.Repositories.@interface;
using Carteiras_Digitais.Shared.Dtos;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Test.Services.Tests
{
    public class AuthencationServiceTests
    {
        private readonly Mock<IUserRepositories> userRepository;
        private readonly Mock<IPasswordService> passwordService;
        private readonly Fixture fixture;

        public AuthencationServiceTests()
        {
            this.userRepository = new Mock<IUserRepositories>();
            this.passwordService = new Mock<IPasswordService>();
            this.fixture = new Fixture();
            
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task ShouldBeReturnErrorNotFoundedEmail()
        {
            var InputEmailNotFound = fixture.Create<LoginDto>();

            userRepository.Setup(r => r.FindUserByEmail(InputEmailNotFound.Email))
                .ReturnsAsync((User?)null);
            
            var authenticationService = new AuthService(userRepository.Object, passwordService.Object);

            Func<Task> action = async () => await authenticationService.AuthenticateUser(InputEmailNotFound);

            await action.Should().ThrowAsync<Exception>();
        }
    }
}
