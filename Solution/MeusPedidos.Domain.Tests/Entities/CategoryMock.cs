using FizzWare.NBuilder;
using System;
using System.Collections.Generic;

namespace MeusPedidos.Domain.Tests
{
    public class CategoryMock : Category
    {
        public CategoryMock() : base(1, "name")
        {
        }

        internal static IList<Category> CreateListOfSize(int size)
        {
            return Builder<Category>.CreateListOfSize(size).All()
                 .WithFactory(index => new Category(++index, nameof(Category.Name) + index)).Build();
        }
    }
}