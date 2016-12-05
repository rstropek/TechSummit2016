namespace Shop.WebApi.Model
{
    public class OrderInput
    {
        public int ProductId { get; set; }

        public decimal Amount { get; set; }
    }
}