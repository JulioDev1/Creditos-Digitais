using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Shared.Dtos
{
    public class BalanceDto
    {
        public Guid UserId { get; set; }
        public decimal Balance { get; set; } = 0;
        public BalanceDto(decimal balance, Guid userId)
        {
            UserId = userId;
            Balance = balance;
        }
    }
}
