using System;
using Xunit;
using MeusPedidos.Domain;

namespace MeusPedidos.Domain.Tests
{
    public class CatalogTests
    {
        [Fact]
        public void Constructor_Empty_SuccessWithZeroSection()
        {
            // Arrange
            // Act
            var catalog = new Catalog();

            // Assert
            Assert.NotNull(catalog.Sections);
            Assert.Empty(catalog.Sections);
        }

        [Fact]
        public void Constructor_WithFiveProducts_SucessWithSingleSection()
        {
            var catalog = new Catalog();
        }
    }
}