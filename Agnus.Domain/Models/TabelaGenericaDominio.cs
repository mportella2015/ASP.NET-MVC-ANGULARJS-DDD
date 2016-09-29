using System.Linq;
using System.Net.Mime;
using Agnus.Domain.Models.Enum;
using Agnus.Framework.Helper;

namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TabelaGenericaDominio")]
    public partial class TabelaGenericaDominio : AuditableEntity
    {
        public TabelaGenericaDominio()
        {
            ConteudoTabelaDominios = new HashSet<ConteudoTabelaDominio>();
        }

        public TabelaGenericaDominio(DominioGenericoEnum dominio)
        {
            ConteudoTabelaDominios = new HashSet<ConteudoTabelaDominio>();
            Nome = Util.GetEnumDescription(dominio);
            Id = (int)dominio;
        }

        [Required]
        [StringLength(150)]
        public string Nome { get; set; }

        public bool IndApenasVisualizacao { get; set; }

        public virtual ICollection<ConteudoTabelaDominio> ConteudoTabelaDominios { get; set; }

        public ConteudoTabelaDominio CriarItem(int codigo, string texto, string textoIngles)
        {
            if (this.ConteudoTabelaDominios != null && this.ConteudoTabelaDominios.Any(x => x.Codigo == codigo))
                throw new Exception("Domínio " + this.Nome + " Código já existente: " + codigo);

            var conteudo = new ConteudoTabelaDominio();
            conteudo.Codigo = codigo;
            conteudo.Texto = texto;
            conteudo.TextoIngles = textoIngles;
            conteudo.IndAtivo = true;

            this.ConteudoTabelaDominios.Add(conteudo);
            return conteudo;
        }
    }
}
