﻿using System;
using System.Collections.Generic;

namespace MeusPedidos.Domain
{
    public class CartItem
    {
        public CartItem(Sale sale, Product product, int units)
        {
            Sale = sale;
            Product = product;
            Units = units;
        }

        public decimal Discount => Sale.CalculateDiscount(Product, Units).Value * Price;
        public decimal Price => Product.Price * Units;

        public decimal PriceToPay => Price - Discount;

        public Product Product { get; protected set; }
        public Sale Sale { get; protected set; }
        public int Units { get; protected set; }

        internal void AddUnits(int units)
        {
            Units += units;
        }
    }
}