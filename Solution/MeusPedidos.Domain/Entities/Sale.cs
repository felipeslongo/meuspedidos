using System;
using System.Collections.Generic;
using System.Linq;

namespace MeusPedidos.Domain
{
    public class Sale
    {
        public Sale(Category category, string name, IEnumerable<SalePolicy> policies)
        {
            Category = category;
            Name = name;
            if (policies != null)
                Policies = policies.OrderByDescending(p => p.Discount).ToList().AsReadOnly();
        }

        public Sale(Category category, string name) : this(category, name, null)
        {
        }

        public Category Category { get; protected set; }
        public string Name { get; protected set; }
        public IReadOnlyCollection<SalePolicy> Policies { get; protected set; } = new List<SalePolicy>();

        public Percent CalculateDiscount(Product product, int units)
        {
            var policy = Policies.FirstOrDefault(p => p.CanApply(units));
            if (policy == null)
                return new Percent();

            return new Percent(policy.Discount);
        }

        public bool IsOnSale(Product product) => product.Category == Category;
    }
}