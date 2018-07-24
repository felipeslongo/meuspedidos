using System.Collections.Generic;

namespace MeusPedidos.Application
{
    public class SaleViewModel
    {
        public SaleViewModel()
        {
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public IEnumerable<SalePolicyViewModel> Policies { get; set; }
    }
}