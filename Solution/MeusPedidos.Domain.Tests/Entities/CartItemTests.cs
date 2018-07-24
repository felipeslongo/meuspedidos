using FizzWare.NBuilder;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MeusPedidos.Domain.Tests
{
    public class CartItemTests
    {
        [Fact]
        public void PriceCalculation_WithSale_CalculateDiscountInProperties()
        {
            //Arrange
            var category = Builder<CategoryMock>.CreateNew().Build();
            var policy = new SalePolicy(10, 10);
            var sale = new Sale(category, "name", new SalePolicy[] { policy });
            var product = Builder<Product>.CreateNew().Build();
            product.Price = 10;

            //Act
            var cartItem = new CartItem(sale, product, 10);

            //Assert
            Assert.Equal(90m, cartItem.PriceToPay);
            Assert.Equal(100m, cartItem.Price);
            Assert.Equal(10m, cartItem.Discount);
        }
    }
}