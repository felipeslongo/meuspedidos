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