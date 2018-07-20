using FizzWare.NBuilder;
using System.Linq;
using Xunit;

namespace MeusPedidos.Domain.Tests.Entities
{
    public class CatalogSectionTests
    {
        [Fact]
        public void Constructor_WithFiveProducts_SucessWithFiveProductsOrderedByFifo()
        {
            // Arrange
            var productsMock = Builder<Product>.CreateListOfSize(5).Build();

            // Act
            var section = new CatalogSection(productsMock);

            // Assert
            Assert.NotNull(section.Products);
            Assert.Equal(productsMock.Count, section.Products.Count());
            Assert.Equal(productsMock, section.Products);
        }

        [Fact]
        public void HasSale_WithoutSale_False()
        {
            // Arrange
            var productsMock = Builder<Product>.CreateListOfSize(5).Build();

            // Act
            var section = new CatalogSection(productsMock);
            var actual = section.HasSale;

            // Assert
            Assert.False(actual, "Deveria detectar que não tem promoção");
            Assert.Null(section.Sale);
        }

        [Fact]
        public void HasSale_WithSale_True()
        {
            // Arrange
            var productsMock = Builder<Product>.CreateListOfSize(5).Build();
            var sale = Builder<SaleMock>.CreateNew().Build();

            // Act
            var section = new CatalogSection(sale, productsMock);
            var actual = section.HasSale;

            // Assert
            Assert.True(actual, "Deveria que tem promoção");
            Assert.NotNull(section.Sale);
        }
    }
}