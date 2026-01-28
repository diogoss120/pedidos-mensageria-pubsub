namespace Contracts.Messages
{
    public class OrderCreatedMessage
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;

        public List<OrderItemMessage> Items { get; set; } = new();

        public string PaymentMethod { get; set; } = string.Empty;
    }

    public class OrderItemMessage
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
