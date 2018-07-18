namespace MeusPedidos.Domain
{
    public class SalePolicy
    {
        public decimal Discount { get; protected set; }
        public int Minimum { get; protected set; }
    }
}