using System;
using System.Collections.Generic;
using System.Text;

namespace MeusPedidos.Domain
{
    /// <summary>
    /// Representa um catálogo de produtos para o cliente.
    /// </summary>
    public class Catalog
    {
        private List<CatalogSection> _sections = new List<CatalogSection>();

        /// <summary>
        /// Constroi Catalogo vazio
        /// </summary>
        public Catalog()
        {
        }

        /// <summary>
        /// Constroi um catalogo com produtos, sem promoções.
        /// </summary>
        /// <param name="products"></param>
        public Catalog(IEnumerable<Product> products)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constroi um catalogo com produtos e promoções.
        /// </summary>
        /// <param name="products"></param>
        /// <param name="sales"></param>
        public Catalog(IEnumerable<Product> products, IEnumerable<Sale> sales) : this(products)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Divisões promocionais do catálogo.
        /// </summary>
        public IReadOnlyList<CatalogSection> Sections => _sections;

        /// <summary>
        /// Filtra os produtos do catálogo por categoria.
        /// Um produto vai permanecer se pertencer a alguma categoria passada.
        /// </summary>
        /// <param name="categories">Categorias a serem usadas no filtro.</param>
        /// <returns>Uma nova instância de <see cref="Catalog"/> com somente os produtos pertencentes as categorias.</returns>
        public Catalog ApplyFilters(params Category[] categories)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Remove todos os filtros aplicados
        /// </summary>
        /// <returns>Instância de <see cref="Catalog"/> sem nenhum filtro.</returns>
        public Catalog RemoveFilters()
        {
            throw new NotImplementedException();
        }
    }
}