using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Shared.Dtos
{
    public class FilterTransactionDto
    {
        public Guid userId { get; set; }

        public Guid SenderWalletId { get; set; }
        public DateTime ? InitialDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
