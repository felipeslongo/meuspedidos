using MeusPedidos.Application.Apis;
using MeusPedidos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeusPedidos.Application.Services
{
    public class SaleService
    {
        private CategoryService categoryService = new CategoryService();

        public IEnumerable<SaleViewModel> GetSales() => new SaleClient().Get();

        internal decimal CalcularteDiscountPercent(Sale sale, Product product, int units) => sale?.CalculateDiscount(product, units).ValuePercent ?? 0;

        internal Sale GetEntity(SaleViewModel saleVM)
        {
            if (saleVM == null)
                return null;

            var category = categoryService.GetEntity(saleVM.CategoryId);
            return new Sale(
                category,
                saleVM.Name,
                saleVM.Policies.Select(policy => new SalePolicy(policy.Minimum, policy.Discount)));
        }

        internal IEnumerable<Sale> GetSalesEntities() => GetSales().Select(vm => GetEntity(vm));
    }
}