using System;

namespace MeusPedidos.Domain
{
    public class SalePolicy
    {
        public SalePolicy(decimal discount) : this(1, discount)
        {
        }

        public SalePolicy(int minimum, decimal discount)
        {
            Discount = discount;
            Minimum = minimum;
        }

        public decimal Discount { get; protected set; }
        public int Minimum { get; protected set; }

        public bool CanApply(int units) => units >= Minimum;
    }
}