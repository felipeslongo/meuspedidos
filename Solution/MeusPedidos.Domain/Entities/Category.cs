using System;
using System.Collections.Generic;
using System.Text;

namespace MeusPedidos.Domain
{
    public class Category : EntityBase
    {
        public Category(int id, string name) : base(id)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}