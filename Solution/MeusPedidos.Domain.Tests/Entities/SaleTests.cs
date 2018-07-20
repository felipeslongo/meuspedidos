using FizzWare.NBuilder;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MeusPedidos.Domain.Tests.Entities
{
    public class SaleTests
    {
        [Fact]
        public void CalculateDiscount_MultiplePolicies_DiscountCalculated()
        {
            //Arrange
            var category = Builder<CategoryMock>.CreateNew().Build();
            var policies = Builder<SalePolicy>.CreateListOfSize(10)
                .All()
                .WithFactory(index => new SalePolicy(++index, 10 * index))
                .Build();
            var sale = new Sale(category, "Sale", policies);
            var product = Builder<Product>.CreateNew()
                .Do(p =>
                {
                    p.Category = category;
                    p.Price = 10;
                })
                .Build();

            //Act
            var actual = sale.CalculateDiscount(product, 7);

            //Assert
            Assert.Equal(0.7m, actual.Value);
        }

        [Fact]
        public void CalculateDiscount_ProductEligible_DiscountCalculated()
        {
            //Arrange
            var category = Builder<CategoryMock>.CreateNew().Build();
            var policies = new SalePolicy(10);
            var sale = new Sale(category, "Sale", new SalePolicy[] { policies });
            var product = Builder<Product>.CreateNew()
                .Do(p =>
                {
                    p.Category = category;
                    p.Price = 10;
                })
                .Build();

            //Act
            var actual = sale.CalculateDiscount(product, 1);

            //Assert
            Assert.Equal(0.1m, actual.Value);
        }

        [Fact]
        public void CalculateDiscount_ProductIneligible_NoDiscount()
        {
            //Arrange
            var category = Builder<CategoryMock>.CreateNew().Build();
            var policies = new SalePolicy(10, 10);
            var sale = new Sale(category, "Sale", new SalePolicy[] { policies });
            var product = Builder<Product>.CreateNew()
                .Do(p =>
                {
                    p.Category = category;
                    p.Price = 10;
                })
                .Build();

            //Act
            var actual = sale.CalculateDiscount(product, 9);

            //Assert
            Assert.Equal(0m, actual.Value);
        }

        [Fact]
        public void IsOnSale_ProductDifferentCategory_False()
        {
            //Arrange
            var category = Builder<CategoryMock>.CreateNew().Build();
            var sale = new Sale(category, "Sale");
            var product = Builder<Product>.CreateNew().Do(p => p.Category = category).Build();

            //Act
            var actual = sale.IsOnSale(product);

            //Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsOnSale_ProductSameCategory_True()
        {
            //Arrange
            var category = Builder<CategoryMock>.CreateNew().Build();
            var sale = new Sale(category, "Sale");
            var product = Builder<Product>.CreateNew()
                .Do(p => p.Category = Builder<CategoryMock>.CreateNew().Build())
                .Build();

            //Act
            var actual = sale.IsOnSale(product);

            //Assert
            Assert.False(actual);
        }
    }
}