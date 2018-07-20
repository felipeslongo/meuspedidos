using FizzWare.NBuilder;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace MeusPedidos.Domain.Tests
{
    public class CartTests
    {
        [Fact]
        public void AddProduct_LessThanOneUnit_ArgumentOutOfRangeException()
        {
            // Arrange
            var product = Builder<Product>.CreateNew().Build();
            var cart = new Cart();

            // Act
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => cart.AddProduct(product, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => cart.AddProduct(product, -0));
        }

        [Fact]
        public void AddProduct_NullProduct_ArgumentNullException()
        {
            // Arrange
            var cart = new Cart();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => cart.AddProduct(null, 10));
        }

        [Fact]
        public void AddProduct_OverrideSaleSameProduct_Exception()
        {
            // Arrange
            var product = Builder<Product>.CreateNew().Build();
            var sale = Builder<SaleMock>.CreateNew().Build();
            var saleOverride = Builder<SaleMock>.CreateNew().Build();
            var cart = new Cart();
            cart.AddProduct(sale, product);

            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(() => cart.AddProduct(saleOverride, product, 1));
        }

        [Fact]
        public void AddProduct_WithSale_CreateCardItemWithSale()
        {
            // Arrange
            var product = Builder<Product>.CreateNew().Build();
            var sale = Builder<SaleMock>.CreateNew().Build();
            var cart = new Cart();

            // Act
            cart.AddProduct(sale, product);

            // Assert
            Assert.Equal(product, cart.Itens.First().Product);
            Assert.Equal(sale, cart.Itens.First().Sale);
        }

        [Fact]
        public void RemoveProductUnit_NullProduct_Nothing()
        {
            // Arrange
            var cart = new Cart();

            // Act
            // Assert
            cart.RemoveProductUnit(null);
        }

        [Fact]
        public void RemoveProductUnit_SingleProduct_ProductRemoved()
        {
            // Arrange
            var product = Builder<Product>.CreateNew().Build();
            var sale = Builder<SaleMock>.CreateNew().Build();
            var cart = new Cart();
            cart.AddProduct(product);

            // Act
            cart.RemoveProductUnit(product);

            // Assert
            Assert.Empty(cart.Itens);
        }

        [Fact]
        public void RemoveProductUnit_SingleUnitFromTenUnitsCartItem_RemainsNineUnitsCartItem()
        {
            // Arrange
            var product = Builder<Product>.CreateNew().Build();
            var sale = Builder<SaleMock>.CreateNew().Build();
            var cart = new Cart();
            cart.AddProduct(product, 10);

            // Act
            cart.RemoveProductUnit(product);

            // Assert
            Assert.Single(cart.Itens);
            Assert.Equal(9, cart.Itens.First().Units);
        }
    }
}