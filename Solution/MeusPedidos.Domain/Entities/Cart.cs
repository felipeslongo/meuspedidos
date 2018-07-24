using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeusPedidos.Domain
{
    /// <summary>
    /// Carrinho de Compra
    /// </summary>
    public class Cart
    {
        private List<CartItem> _itens = new List<CartItem>();

        public IEnumerable<CartItem> Itens => _itens;

        /// <summary>
        /// Preço total a pagar pelo cliente.
        /// Leva-se em conta o preço total dos produtos e os descontos aplicados.
        /// </summary>
        public decimal PriceToPay => Itens.Sum(item => item.PriceToPay);

        public void AddProduct(Sale sale, Product product, int units = 1)
        {
            if (units < 1)
                throw new ArgumentOutOfRangeException("Quantidade menor que uma unidade adicionada.");

            if (product == null)
                throw new ArgumentNullException("Produto nulo/vazio adicionado.");

            var item = GetProductItem(product);
            if (item == null)
            {
                item = new CartItem(sale, product, units);
                _itens.Add(item);
            }
            else
            {
                if (item.Sale != sale)
                    throw new InvalidOperationException($"Uma promoção já está aplicada foi aplicada ao produto {item.Product.Name}.");
                item.AddUnits(units);
            }
        }

        public void AddProduct(Product product, int units = 1) => AddProduct(null, product, units);

        public Percent GetDiscount(Product product)
        {
            if (product == null)
                return Percent.Zero;

            var cartItem = GetProductItem(product);
            if (cartItem == null)
                return Percent.Zero;

            return cartItem.DiscountPercent;
        }

        /// <summary>
        /// Retorna a quantidade/unidade de um dado tipo de produto está no carrinho.
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Int representando a quantidade ou zero se não existir</returns>
        public int GetUnits(Product product)
        {
            if (product == null)
                return 0;

            var cartItem = GetProductItem(product);
            if (cartItem == null)
                return 0;

            return cartItem.Units;
        }

        /// <summary>
        /// Retorna a quantidade/unidade total de produtos no carrinho
        /// </summary>
        /// <returns></returns>
        public int GetUnits() => Itens.Sum(cartItem => cartItem.Units);

        public bool IsProductInCart(Product product) => GetProductItem(product) != null;

        public void RemoveProductUnit(Product product)
        {
            var item = GetProductItem(product);
            if (item == null)
                return;

            if (item.Units == 1)
                _itens.Remove(item);
            else
                item.AddUnits(-1);
        }

        private CartItem GetProductItem(Product product)
        {
            if (product == null)
                return null;
            return _itens.FirstOrDefault(i => i.Product == product);
        }
    }
}