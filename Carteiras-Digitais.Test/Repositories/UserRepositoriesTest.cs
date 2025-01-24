using AutoFixture;
using Carteiras_Digitais.Api.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Database;
using Carteiras_Digitais.Infrasctruture.Repositories;
using Carteiras_Digitais.Test.Repositories.Database;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Test.Repositories
{
    public class UserRepositoriesTest
    {
        private readonly Fixture fixture;
        
        public UserRepositoriesTest() 
        {
            this.fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
 
        [Fact]
        
        public async Task ShouldBeCreateUserInDatabase() 
        { 
            var context = AppDbContextFactory.CreateInMemoryDbContext();
            
            var repository = new UserRepository(context);

            var newUser = fixture.Create<User>();

            var createUserWithSuccess = await repository.CreateUserDatabase(newUser);

            var AddedUser = await context.users.FindAsync(newUser.Id);

            AddedUser.Should().NotBeNull();

            AddedUser.Should().Be(newUser);
        }
        [Fact]
        public async Task ShouldBeReturnNullWhenEmailIsNotFound() 
        {
            var context = AppDbContextFactory.CreateInMemoryDbContext();

            var repository = new UserRepository(context);

            var userNotFounded = await repository.FindUserByEmail("fakeEmail@mail.com");

            userNotFounded.Should().BeNull();
        }
        [Fact]
        public async Task ShouldBeReturnUserDataWhenFoundUser()
        {
            var context = AppDbContextFactory.CreateInMemoryDbContext();

            var repository = new UserRepository(context);

            var newUser = fixture.Create<User>();

            context.users.Add(newUser);

            await context.SaveChangesAsync();

            var findingUser = await context.users.FirstOrDefaultAsync(u=> u.Email == newUser.Email);

            var userFounded = await repository.FindUserByEmail(findingUser!.Email);

            findingUser.Should().NotBeNull();
            userFounded.Should().Be(newUser);
            
        }
    }
}
