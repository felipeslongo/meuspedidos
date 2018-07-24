using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MeusPedidos.Domain
{
    /// <summary>
    /// Representa um catálogo de produtos para o cliente.
    /// </summary>
    public class Catalog
    {
        //private readonly Dictionary<Product, CatalogSection> _productsSections = new Dictionary<Product, CatalogSection>();
        //private readonly Dictionary<Sale, CatalogSection> _salesSections = new Dictionary<Sale, CatalogSection>();
        private readonly List<CatalogSection> _sections = new List<CatalogSection>();

        private Catalog CatalogUnfiltered;

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
        public Catalog(IEnumerable<Product> products) : this(products, null)
        {
        }

        /// <summary>
        /// Constroi um catalogo com produtos e promoções.
        /// </summary>
        /// <param name="products"></param>
        /// <param name="sales"></param>
        public Catalog(IEnumerable<Product> products, IEnumerable<Sale> sales)
        {
            AddSales(sales);
            AddProducts(products);
        }

        public IEnumerable<Product> Products => SectionsWithProducts.SelectMany(section => section.Products);

        public IEnumerable<Sale> Sales => SectionsOnSale.Select(s => s.Sale);

        /// <summary>
        /// Divisões do catálogo.
        /// </summary>
        public IEnumerable<CatalogSection> Sections => _sections.Union(new CatalogSection[] { SectionWithoutSale });

        /// <summary>
        /// Divisões com promoção do catálogo ou vazio se não tiver produtos com promoção.
        /// </summary>
        public IEnumerable<CatalogSection> SectionsOnSale => Sections.Where(section => section.HasSale);

        public IEnumerable<CatalogSection> SectionsWithProducts => Sections.Where(s => !s.IsEmpty);

        /// <summary>
        /// Divisão sem promoção do catalogo
        /// </summary>
        public CatalogSection SectionWithoutSale { get; } = new CatalogSection();

        /// <summary>
        /// Filtra os produtos do catálogo por categoria.
        /// Um produto vai permanecer se pertencer a alguma categoria passada.
        /// </summary>
        /// <param name="categories">Categorias a serem usadas no filtro.</param>
        /// <returns>Uma nova instância de <see cref="Catalog"/> com somente os produtos pertencentes as categorias.</returns>
        public Catalog ApplyFilters(params Category[] categories)
        {
            var catalogFiltered = new Catalog(Products.Where(p => categories.Contains(p.Category)), Sales);
            catalogFiltered.CatalogUnfiltered = CatalogUnfiltered ?? this;
            return catalogFiltered;
        }

        /// <summary>
        /// Remove todos os filtros aplicados
        /// </summary>
        /// <returns>Instância de <see cref="Catalog"/> sem nenhum filtro.</returns>
        public Catalog RemoveFilters() => CatalogUnfiltered ?? this;

        private void AddProducts(IEnumerable<Product> products)
        {
            if (products == null)
                return;
            foreach (var product in products)
                Sections.First(section => section.CanAdd(product)).AddProduct(product);
        }

        private void AddSales(IEnumerable<Sale> sales)
        {
            if (sales == null)
                return;
            foreach (var sale in sales)
                if (!_sections.Any(section => section.Sale == sale))
                    AddSection(new CatalogSection(sale));
        }

        private void AddSection(CatalogSection section)
        {
            if (section == null)
                return;

            section.AddProducts(SectionWithoutSale.Products.Where(product => section.Sale.IsOnSale(product)));
            SectionWithoutSale.RemoveProducts(section.Products);
            _sections.Add(section);
        }
    }
}