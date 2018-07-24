using MeusPedidos.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeusPedidos.Application
{
    public static class ServiceFactory
    {
        public static CartService CreateCartService() => new CartService();

        public static CatalogService CreateCatalogService() => new CatalogService();

        public static CategoryService CreateCategoryService() => new CategoryService();

        public static ProductService CreateProductService() => new ProductService();
    }
}