using System.Runtime.Serialization;
using Agnus.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Domain
{
    [DataContract]
    public abstract class KeyAuditableEntity : AuditableEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [DataMember]
        public override long Id { get; set; }
    }
}
