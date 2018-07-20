﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MeusPedidos.Domain
{
    public class Category
    {
        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; protected set; }
        public string Name { get; set; }
    }
}