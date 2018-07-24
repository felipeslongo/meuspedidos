using MeusPedidos.Application.Apis;
using MeusPedidos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeusPedidos.Application.Services
{
    public class CartService
    {
        private static Cart cart = new Cart();
        private ProductService productService = new ProductService();
        private SaleService saleService = new SaleService();

        public void AddProduct(ProductViewModel productVM, SaleViewModel saleVM)
        {
            var product = productService.GetEntity(productVM);
            var sale = saleService.GetEntity(saleVM);
            cart.AddProduct(sale, product);
        }

        public IEnumerable<CartItemViewModel> GetCartItems()
        {
            //CartItem
            //var cart = new Cart();
            //cart.AddProduct(new Product()
            //{
            //    Id = 1,
            //    Name = "Game Horizon Zero Dawn - PS4",
            //    Price = 2025.25m,
            //}, 2);

            //var category = new Category(1, "Esporte");
            //cart.AddProduct(
            //    new Sale(category, "Loucos Por Esportes", new SalePolicy(5, 50)),
            //    new Product()
            //    {
            //        Id = 1,
            //        Name = "Fifa",
            //        Price = 10.10m,
            //        Category = category
            //    }, 4);

            return cart.Itens.Select(item => new CartItemViewModel(item));
        }

        public decimal GetDiscountPercent(ProductViewModel product) => cart.GetDiscount(productService.GetEntity(product)).ValuePercent;

        /// <summary>
        /// Preço total a pagar pelo cliente.
        /// Leva-se em conta o preço total dos produtos e os descontos aplicados.
        /// </summary>
        /// <returns></returns>
        public decimal GetPriceToPay() => cart.PriceToPay;

        /// <summary>
        /// Retorna a quantidade/unidade de um dado tipo de produto está no carrinho.
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Int representando a quantidade ou zero se não existir</returns>
        public int GetUnits(ProductViewModel productVM) => GetUnits(productService.GetEntity(productVM));

        /// <summary>
        /// Retorna a quantidade/unidade total de produtos no carrinho
        /// </summary>
        /// <returns></returns>
        public int GetUnits() => cart.GetUnits();

        public void RemoveProduct(ProductViewModel productVM)
        {
            var product = productService.GetEntity(productVM);
            cart.RemoveProductUnit(product);
        }

        /// <summary>
        /// Retorna a quantidade/unidade de um dado tipo de produto está no carrinho.
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Int representando a quantidade ou zero se não existir</returns>
        internal int GetUnits(Product product) => cart.GetUnits(product);
    }
}