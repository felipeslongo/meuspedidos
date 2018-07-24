using MeusPedidos.Application.Apis;
using MeusPedidos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeusPedidos.Application.Services
{
    public class CategoryService
    {
        public IEnumerable<CategoryViewModel> GetCategories() => new CategoryClient().Get();

        internal IEnumerable<Category> GetEntities(IEnumerable<CategoryViewModel> categories) => categories.Select(vm => GetEntity(vm));

        internal Category GetEntity(int? id)
        {
            if (id == null)
                return null;

            var vm = GetCategories().FirstOrDefault(item => item.Id == id);
            if (vm == null)
                return null;

            return new Category(vm.Id, vm.Name);
        }

        internal Category GetEntity(CategoryViewModel categoryVM) => GetEntity(categoryVM.Id);
    }
}