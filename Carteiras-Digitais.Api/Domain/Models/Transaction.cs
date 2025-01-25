namespace Carteiras_Digitais.Core.Domain.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public Guid SenderWalletId { get; set; }
        public Guid ReceiverWalletId { get; set; }
        public required Wallet Wallet { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
