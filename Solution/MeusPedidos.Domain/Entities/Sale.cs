using System;
using System.Collections.Generic;
using System.Linq;

namespace MeusPedidos.Domain
{
    public class Sale : IEquatable<Sale>
    {
        public Sale(Category category, string name, IEnumerable<SalePolicy> policies)
        {
            Category = category;
            Name = name;
            if (policies != null)
                Policies = policies.OrderByDescending(p => p.Discount).ToList().AsReadOnly();
        }

        public Sale(Category category, string name, SalePolicy policy) : this(category, name, new SalePolicy[] { policy })
        {
        }

        public Sale(Category category, string name) : this(category, name, new SalePolicy[0])
        {
        }

        public Category Category { get; protected set; }
        public string Name { get; protected set; }
        public IReadOnlyCollection<SalePolicy> Policies { get; protected set; } = new List<SalePolicy>();

        // this is second one '!='
        public static bool operator !=(Sale obj1, Sale obj2)
        {
            return !(obj1 == obj2);
        }

        public static bool operator ==(Sale obj1, Sale obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }

            if (ReferenceEquals(obj1, null))
            {
                return false;
            }
            if (ReferenceEquals(obj2, null))
            {
                return false;
            }

            return obj1.Category == obj2.Category && obj1.Name == obj2.Name;
        }

        public Percent CalculateDiscount(Product product, int units)
        {
            var policy = Policies.FirstOrDefault(p => p.CanApply(units));
            if (policy == null)
                return new Percent();

            return new Percent(policy.Discount);
        }

        public bool Equals(Sale other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Category == other.Category && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((Sale)obj);
        }

        public override int GetHashCode() => (Category.Id + Name).GetHashCode();

        public bool IsOnSale(Product product) => product.Category == Category;
    }
}