using System.Collections.Generic;

namespace MeusPedidos.Domain.Tests
{
    internal class SaleMock : Sale
    {
        public SaleMock(Category category, string name) : base(category, name)
        {
        }

        public SaleMock(Category category, string name, IEnumerable<SalePolicy> policies) : base(category, name, policies)
        {
        }

        public SaleMock() : this(null, null)
        {
        }
    }
}