using System;
using System.Collections.Generic;
using System.Text;

namespace MeusPedidos.Domain
{
    public class Percent
    {
        public Percent() : this(0)
        {
        }

        public Percent(int value)
        {
            Value = (decimal)value / 100;
        }

        public Percent(decimal value)
        {
            Value = (decimal)value / 100;
        }

        public decimal Value { get; }
    }
}