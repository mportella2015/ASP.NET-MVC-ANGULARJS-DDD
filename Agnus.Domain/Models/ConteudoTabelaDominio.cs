namespace Agnus.Domain.Models
{
    using Agnus.Domain.Models.Enum;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Table("ConteudoTabelaDominio")]
    [DataContract]
    public partial class ConteudoTabelaDominio : KeyAuditableEntity
    {
        public ConteudoTabelaDominio()
        {
            FornecedorCNAEs = new HashSet<FornecedorCNAE>();
            FornecedorContratoes = new HashSet<FornecedorContrato>();
            StatusFornecedors = new HashSet<Fornecedor>();
            FornecedorAvaliacaos = new HashSet<FornecedorAvaliacao>();
            StatusContratos = new HashSet<FornecedorContrato>();
            FornecedorPendencias = new HashSet<FornecedorPendencia>();
            FornecedorSocios = new HashSet<FornecedorSocio>();
            FornecedorTelefoneTipoTelefone = new HashSet<FornecedorTelefone>();
            FornecedorTelefonesTipoUsoTelefone = new HashSet<FornecedorTelefone>();
            FornecedorEnderecoes = new HashSet<FornecedorEndereco>();
            FornecedorServicoes = new HashSet<FornecedorServico>();
            FornecedorContratoServicoes = new HashSet<FornecedorContratoServico>();
            FornecedorMaterials = new HashSet<FornecedorMaterial>();
            UsuarioSiteTipoPessoas = new HashSet<UsuarioSite>();
            StatusUsuarioSites = new HashSet<UsuarioSite>();
            FornecedorEmailTipoEmails = new HashSet<FornecedorEmail>();
          
        }
        [DataMember]
        public int Codigo { get; set; }

        [StringLength(300)]
        [DataMember]
        public string Texto { get; set; }

        [StringLength(300)]
        [DataMember]
        public string TextoIngles { get; set; }

        [DataMember]
        public bool? Indicador { get; set; }
        
        [DataMember]
        public bool IndAtivo { get; set; }

        public virtual ICollection<UsuarioSite> UsuarioSiteTipoPessoas { get; set; }

        public virtual ICollection<FornecedorCNAE> FornecedorCNAEs { get; set; }

        public virtual ICollection<FornecedorContrato> FornecedorContratoes { get; set; }

        public virtual ICollection<Fornecedor> StatusFornecedors { get; set; }

        public virtual ICollection<FornecedorAvaliacao> FornecedorAvaliacaos { get; set; }

        public virtual ICollection<FornecedorContrato> StatusContratos { get; set; }

        public virtual ICollection<FornecedorPendencia> FornecedorPendencias { get; set; }

        public virtual ICollection<FornecedorSocio> FornecedorSocios { get; set; }

        public virtual ICollection<FornecedorTelefone> FornecedorTelefoneTipoTelefone { get; set; }

        public virtual ICollection<FornecedorTelefone> FornecedorTelefonesTipoUsoTelefone { get; set; }

        public virtual ICollection<FornecedorEndereco> FornecedorEnderecoes { get; set; }

        public virtual ICollection<FornecedorServico> FornecedorServicoes { get; set; }

        public virtual ICollection<FornecedorContratoServico> FornecedorContratoServicoes { get; set; }

        public virtual ICollection<FornecedorMaterial> FornecedorMaterials { get; set; }

        public virtual ICollection<UsuarioSite> StatusUsuarioSites { get; set; }

        public virtual ICollection<FornecedorEmail> FornecedorEmailTipoEmails { get; set; }

        [ForeignKey("TabelaGenericaDominio")]
        public long IdTabelaGenericaDominio { get; set; }

        public virtual TabelaGenericaDominio TabelaGenericaDominio { get; set; }
        
        [NotMapped]
        public string txtCodigo
        {
            get { return IdTabelaGenericaDominio == (long)DominioGenericoEnum.OperacaoBancaria || IdTabelaGenericaDominio == (long)DominioGenericoEnum.Banco && Codigo.ToString().Length < 3 ? Codigo.ToString().PadLeft(3, '0') : Codigo.ToString(); }
        }
    }
}
