using System;

namespace MeusPedidos.Domain
{
    /// <summary>
    /// Entidade produto.
    /// </summary>
    public class Product
    {
        public Category Category { get; set; }
        public string Description { get; set; }
        public int Id { get; protected set; }
        public bool IsFavorite { get; }
        public string Name { get; protected set; }
        public Uri Photo { get; set; }
        public decimal Price { get; set; }
    }
}