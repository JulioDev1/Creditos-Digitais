using Carteiras_Digitais.Application.Services;
using Carteiras_Digitais.Core.Domain.Interfaces;
using Carteiras_Digitais.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Carteiras_Digitais.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }
        [HttpPatch("/transaction-between-user")]
        [Authorize]
        public async Task<ActionResult> TransactionBetweenUsers([FromBody] TransactionDto transactionDto)
        {
            try
            {
                var Id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(Id))
                {
                    return Unauthorized("user not logged");
                }
                var userId = Guid.Parse(Id);

                var transaction = await transactionService.TransactionToBalanceToReceiver(transactionDto);

                return Ok(transaction);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        [HttpGet("/list-user-transcation")]
        [Authorize]
        public async Task<ActionResult> TransactionWithFilterOptionalFilter([FromBody] FilterTransactionDto transactionDto)
        {
            try
            {
                var Id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(Id))
                {
                    return Unauthorized("user not logged");
                }
                var userId = Guid.Parse(Id);

                var transaction = await transactionService.GetUserAllUserTransactionsWithFilter(transactionDto);

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An unexpected error occurred.", Details = ex.Message });
            }
        }
    }
}
