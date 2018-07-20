using FizzWare.NBuilder;
using System.Linq;
using Xunit;

namespace MeusPedidos.Domain.Tests
{
    public class CatalogTests
    {
        [Fact]
        public void ApplyFilters_TwoCategories_CreateCatalogWithFilteredProducts()
        {
            // Arrange
            var categories = Builder<CategoryMock>.CreateListOfSize(3).Build();
            var products = Builder<Product>
                .CreateListOfSize(9)
                .All()
                .Do((p, i) => p.Category = categories[(i + 1) % 3])
                .Build();

            var catalog = new Catalog(products);
            var filters = categories.Where(c => c.Id == 0 || c.Id == 2);

            // Act
            var catalogFiltered = catalog.ApplyFilters(filters.ToArray());

            // Assert
            System.Collections.Generic.IEnumerable<Product> expected = products.Where(p => filters.Contains(p.Category));
            Assert.Equal(expected, catalogFiltered.Products);
        }

        [Fact]
        public void Constructor_Empty_SuccessWithZeroSection()
        {
            // Arrange
            // Act
            var catalog = new Catalog();

            // Assert
            Assert.NotNull(catalog.Sections);
            Assert.Empty(catalog.SectionsWithProducts);
        }

        [Fact]
        public void Constructor_WithFiveProducts_SucessWithSingleSectionWithFiveProducts()
        {
            // Arrange
            var products = Builder<Product>
                .CreateListOfSize(5)
                .All()
                .Do(p => p.Category = Builder<CategoryMock>.CreateNew().Build())
                .Build();

            // Act
            var catalog = new Catalog(products);

            // Assert
            Assert.NotNull(catalog.Sections);
            Assert.Single(catalog.Sections);
            Assert.Equal(products, catalog.SectionsWithProducts.First().Products);
        }

        [Fact]
        public void RemoveFilters_FromFilteredCatalog_UnfilteredOriginalCatalog()
        {
            // Arrange
            var categories = CategoryMock.CreateListOfSize(2);
            var products = Builder<Product>
                .CreateListOfSize(10)
                .All()
                .Do((p, i) => p.Category = categories[(i + 1) % 2])
                .Build();

            var unfilteredCatalog = new Catalog(products);
            var filteredCatalog = unfilteredCatalog.ApplyFilters(categories.First());

            // Act
            var actual = filteredCatalog.RemoveFilters();

            // Assert
            Assert.Equal(unfilteredCatalog, actual);
        }
    }
}