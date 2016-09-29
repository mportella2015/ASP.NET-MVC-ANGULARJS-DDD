using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Framework
{
    [DataContract]
    public abstract class BaseEntity
    {
        public virtual long Id { get; set; }
    }

    public abstract class Entity : BaseEntity, IEntity
    {
    }
}
