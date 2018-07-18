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
        private readonly List<Product> _products;

        /// <summary>
        /// Construtor de um agrupamento de produtos sob a mesma promoção.
        /// </summary>
        /// <param name="sale"></param>
        /// <param name="products"></param>
        public CatalogSection(Sale sale, IEnumerable<Product> products)
        {
            if (!products.Any())
                throw new ArgumentOutOfRangeException("Não é possível criar uma Seção de Catálogo de Produto vazia.");

            Sale = sale;
            _products = products.ToList();
        }

        /// <summary>
        /// Construtor de um agruapmento de produtos sem promoção.
        /// </summary>
        /// <param name="products"></param>
        public CatalogSection(IEnumerable<Product> products) : this(null, products)
        {
        }

        public bool HasSale => Sale != null;
        public IReadOnlyList<Product> Products => _products;
        public Sale Sale { get; }
    }
}