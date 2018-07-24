using MeusPedidos.AndroidApp.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeusPedidos.Application
{
    public static class RepositoryFactory
    {
        private static IProductRepository productRepository;

        public static IProductRepository CreateProductRepository() => productRepository ?? throw new Exception($"{nameof(RepositoryFactory)} was not set with a Factory for {nameof(IProductRepository)}");

        public static void InjectProductRepositoryFactory(IProductRepository repository)
        {
            productRepository = repository;
        }
    }
}