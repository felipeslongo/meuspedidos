using System;
using System.Collections.Generic;
using System.Text;

namespace MeusPedidos.Application
{
    public class CatalogItemViewModel
    {
        public decimal DiscountPercent { get; set; }
        public string Header { get; internal set; } = "";
        public bool IsHeader { get; internal set; }
        public ProductViewModel Product { get; set; }
        public SaleViewModel Sale { get; set; }
        public int Units { get; set; }
    }
}