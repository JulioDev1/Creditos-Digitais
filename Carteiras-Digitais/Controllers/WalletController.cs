using Carteiras_Digitais.Core.Domain.Interfaces;
using Carteiras_Digitais.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Carteiras_Digitais.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService walletService;

        public WalletController(IWalletService walletService)
        {
            this.walletService = walletService;
        }
        [HttpPatch("/deposit-balance")]
        [Authorize]
        public async Task<ActionResult> DepositBalanceInUserAccount([FromBody] decimal balance)
        {
            try
            {
                var Id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(Id))
                {
                    return Unauthorized("user not logged");
                }

                var userId = Guid.Parse(Id);

                var deposit = new BalanceDto(balance, userId);

                var wallet = await walletService.DepositBalanceToWallet(deposit);

                return Ok(wallet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("/get-user-balance")]
        [Authorize]
        public async Task<ActionResult> GetUserWalletById()
        {
            try
            {
                var Id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);

                if (Id == null)
                {
                    return Unauthorized("user not logged");
                }

                var userId = Guid.Parse(Id.Value);


                var wallet = await walletService.GetUserBalanceWallet(userId);

                return Ok(wallet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
