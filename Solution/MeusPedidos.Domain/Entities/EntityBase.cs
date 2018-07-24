using System;
using System.Collections.Generic;
using System.Text;

namespace MeusPedidos.Domain
{
    /// <summary>
    /// Representa uma entidade cuja identidade é representável por um Id.
    /// </summary>
    public abstract class EntityBase : IEquatable<EntityBase>
    {
        public EntityBase(int id)
        {
            Id = id;
        }

        /// <summary>
        /// Identificador único da entidade.
        /// Diferentes instancias do objeto são iguais quando o Id é igual;
        /// </summary>
        public int Id { get; protected set; }

        // this is second one '!='
        public static bool operator !=(EntityBase obj1, EntityBase obj2)
        {
            return !(obj1 == obj2);
        }

        public static bool operator ==(EntityBase obj1, EntityBase obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }

            if (ReferenceEquals(obj1, null))
            {
                return false;
            }
            if (ReferenceEquals(obj2, null))
            {
                return false;
            }

            return (obj1.Id == obj2.Id);
        }

        public bool Equals(EntityBase other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((EntityBase)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}