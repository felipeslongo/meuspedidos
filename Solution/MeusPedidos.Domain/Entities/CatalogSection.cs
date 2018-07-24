using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeusPedidos.Domain
{
    /// <summary>
    /// Representa um agrupamento de produtos sob a mesma promoção em um catálogo
    /// </summary>
    public class CatalogSection
    {
        /// <summary>
        /// Produtos da seção.
        /// </summary>
        private readonly List<Product> _products = new List<Product>();

        /// <summary>
        /// Construtor de um agrupamento de produtos sob a mesma promoção.
        /// </summary>
        /// <param name="sale"></param>
        /// <param name="products"></param>
        public CatalogSection(Sale sale, IEnumerable<Product> products)
        {
            Sale = sale;
            if (products != null)
                _products = products.ToList();
        }

        /// <summary>
        /// Construtor de um agruapmento de produtos sem promoção.
        /// </summary>
        /// <param name="products"></param>
        public CatalogSection(IEnumerable<Product> products) : this(null, products)
        {
        }

        public CatalogSection(Sale sale) : this(sale, null)
        {
        }

        public CatalogSection() : this(null, null)
        {
        }

        public bool HasSale => Sale != null;
        public bool IsEmpty => !Products.Any();
        public IEnumerable<Product> Products => _products;
        public Sale Sale { get; }

        public void AddProduct(Product product) => _products.Add(product);

        public void AddProducts(IEnumerable<Product> products) => _products.AddRange(products);

        public bool CanAdd(Product product) => Sale?.IsOnSale(product) ?? true;

        public void RemoveProducts(IEnumerable<Product> products) => _products.RemoveAll(p => products.Contains(p));
    }
}