using MeusPedidos.Application.Apis;
using MeusPedidos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeusPedidos.Application.Services
{
    public class CatalogService
    {
        private static Catalog catalogCurrent;
        private CartService cartService = new CartService();
        private SaleService saleService = new SaleService();

        public IEnumerable<CatalogItemViewModel> FilterCatalogItems(IEnumerable<CategoryViewModel> categoriesVM)
        {
            var unfilteredItems = GetCatalogItems();
            if (categoriesVM == null || !categoriesVM.Any())
                return unfilteredItems;

            var categories = new CategoryService().GetEntities(categoriesVM);
            catalogCurrent = catalogCurrent.ApplyFilters(categories.ToArray());
            return CreateCatalogItemsVM(catalogCurrent);
        }

        public IEnumerable<CatalogItemViewModel> GetCatalogItems()
        {
            var productsEntities = new ProductService().GetProductsEntities();
            var salesEntities = new SaleService().GetSalesEntities();

            var catalog = new Catalog(productsEntities, salesEntities);
            catalogCurrent = catalog;
            return CreateCatalogItemsVM(catalog);
        }

        private IEnumerable<CatalogItemViewModel> CreateCatalogItemsVM(Catalog catalog)
        {
            var vms = catalog.Sections.SelectMany(section => section.Products.Select(sectionProduct =>
            {
                int units = cartService.GetUnits(sectionProduct);
                var isHeader = section.Products.First() == sectionProduct;
                return new CatalogItemViewModel()
                {
                    IsHeader = isHeader,
                    Header = section.HasSale ? section.Sale.Name : "Veja Tambem",
                    Units = units,
                    DiscountPercent = saleService.CalcularteDiscountPercent(section.Sale, sectionProduct, units),
                    Product = new ProductViewModel()
                    {
                        Id = sectionProduct.Id,
                        Name = sectionProduct.Name,
                        Price = sectionProduct.Price,
                        PhotoUrl = sectionProduct.Photo.AbsoluteUri,
                        CategoryId = sectionProduct.Category?.Id,
                        IsFavorite = sectionProduct.IsFavorite
                    },
                    Sale = section.HasSale ? new SaleViewModel()
                    {
                        Name = section.Sale.Name,
                        CategoryId = section.Sale.Category.Id,
                        Policies = section.Sale.Policies.Select(policy => new SalePolicyViewModel() { Discount = policy.Discount, Minimum = policy.Minimum })
                    } : null
                };
            }));

            return vms;
        }
    }
}