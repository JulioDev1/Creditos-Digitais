using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Shared.Dtos
{
    public class TransactionDto
    {
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime ? CreatedAt { get; set; }
        public Guid SenderWalletId { get; set; }
        public Guid ReceiverWalletId { get; set; }
    }
}
