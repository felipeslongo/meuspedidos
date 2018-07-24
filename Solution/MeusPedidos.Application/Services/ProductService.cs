using MeusPedidos.AndroidApp.Repositories;
using MeusPedidos.Application.Apis;
using MeusPedidos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeusPedidos.Application.Services
{
    public class ProductService
    {
        //private static HashSet<int> favoritesHashset = new HashSet<int>();
        private CategoryService categoryService = new CategoryService();

        public void AddToFavorites(ProductViewModel vm)
        {
            //favoritesHashset.Add(vm.Id);
            RepositoryFactory.CreateProductRepository().AddToFavorites(vm);
        }

        public void RemoveFromFavorites(ProductViewModel vm)
        {
            //favoritesHashset.Remove(vm.Id);
            RepositoryFactory.CreateProductRepository().RemoveFromFavorites(vm);
        }

        internal Product GetEntity(int? id)
        {
            var vm = GetProducts().FirstOrDefault(item => item.Id == id);
            if (vm == null)
                return null;

            return GetEntity(vm);
        }

        internal Product GetEntity(ProductViewModel vm)
        {
            var category = categoryService.GetEntity(vm.CategoryId);
            return new Product(vm.Id, category, vm.Name, vm.Price)
            {
                Description = vm.Description,
                Photo = string.IsNullOrWhiteSpace(vm.PhotoUrl) ? null : new Uri(vm.PhotoUrl),
                IsFavorite = IsFavorite(vm)
            };
        }

        internal IEnumerable<ProductViewModel> GetProducts() => new ProductClient().Get();

        internal IEnumerable<Product> GetProductsEntities() => GetProducts().Select(vm => GetEntity(vm));

        private bool IsFavorite(ProductViewModel vm)
        {
            //return favoritesHashset.Contains(vm.Id);
            return RepositoryFactory.CreateProductRepository().IsFavorite(vm);
        }
    }
}