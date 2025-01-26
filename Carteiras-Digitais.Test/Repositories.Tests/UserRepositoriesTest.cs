using AutoFixture;
using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Repositories;
using Carteiras_Digitais.Test.Repositories.Tests.Database;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;


namespace Carteiras_Digitais.Test.Repositories.Tests
{
    public class UserRepositoriesTest
    {
        private readonly Fixture fixture;

        public UserRepositoriesTest()
        {
            fixture = new Fixture();
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

            var findingUser = await context.users.FirstOrDefaultAsync(u => u.Email == newUser.Email);

            var userFounded = await repository.FindUserByEmail(findingUser!.Email);

            findingUser.Should().NotBeNull();

            userFounded.Should().Be(newUser);
        }
        [Fact]
        public async Task ShouldBeReturnUserDataWhenFoundIdUser()
        {
            var context = AppDbContextFactory.CreateInMemoryDbContext();

            var repository = new UserRepository(context);

            var newUser = fixture.Create<User>();

            context.users.Add(newUser);

            await context.SaveChangesAsync();

            var findingUser = await context.users.FirstAsync(u => u.Email == newUser.Email);

            var userFounded = await repository.GetUserById(findingUser.Id);

            findingUser.Should().NotBeNull();

            userFounded.Should().Be(newUser);
        }
    }
}
