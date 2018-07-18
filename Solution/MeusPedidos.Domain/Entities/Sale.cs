using System.Collections.Generic;

namespace MeusPedidos.Domain
{
    public class Sale
    {
        public Category Category { get; protected set; }
        public string Name { get; protected set; }
        public IReadOnlyCollection<SalePolicy> Policies { get; protected set; }
    }
}