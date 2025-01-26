using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Core.Domain.Interfaces;
using Carteiras_Digitais.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Carteiras_Digitais.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("/create-user")]
        public async Task<ActionResult<Guid?>> CreateUserAccount(UserDto userDto)
        {
            try
            {
                var AddUserData = new UserDto
                {
                    Email = userDto.Email,
                    Name = userDto.Name,
                    Password = userDto.Password,
                };

                var userAccountCreated = await userService.CreateUserAndWallet(AddUserData);

                return userAccountCreated;
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An unexpected error occurred.", Details = ex.Message });
            }
        }
    }
}
