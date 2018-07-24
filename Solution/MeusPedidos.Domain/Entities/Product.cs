using System;

namespace MeusPedidos.Domain
{
    /// <summary>
    /// Entidade produto.
    /// </summary>
    public class Product : EntityBase
    {
        public Product() : base(0)
        {
        }

        /// <summary>
        /// Construct a product entity with minimal contract requirements
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        public Product(int id, Category category, string name, decimal price) : base(id)
        {
            Category = category;
            Name = name;
            Price = price;
        }

        public Category Category { get; set; }
        public string Description { get; set; }
        public bool IsFavorite { get; set; }
        public string Name { get; set; }
        public Uri Photo { get; set; }
        public decimal Price { get; set; }
    }
}