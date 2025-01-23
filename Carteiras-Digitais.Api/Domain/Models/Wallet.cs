using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Api.Domain.Models
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public decimal Balance { get; set; } = 0;
        public Guid UserId { get; set; }
        public required User user { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public DateTime CreatedAt { get; } = DateTime.Now;
        
    }
}
