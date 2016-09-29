using System.Runtime.Serialization;
using Agnus.Domain.Models.Enum;
using System.Linq;

namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Fornecedor")]
    [DataContract]
    public partial class Fornecedor : KeyAuditableEntity, ILogFornecedor
    {
        public Fornecedor()
        {
            FornecedorTelefones = new HashSet<FornecedorTelefone>();
            FornecedorAvaliacaos = new HashSet<FornecedorAvaliacao>();
            FornecedorCNAEs = new HashSet<FornecedorCNAE>();
            FornecedorContatoes = new HashSet<FornecedorContato>();
            FornecedorContratoes = new HashSet<FornecedorContrato>();
            FornecedorDocumentoes = new HashSet<FornecedorDocumento>();
            FornecedorEmails = new HashSet<FornecedorEmail>();
            FornecedorEnderecoes = new HashSet<FornecedorEndereco>();
            FornecedorFaleConoscoes = new HashSet<FornecedorFaleConosco>();
            FornecedorHistoricoAlteracaos = new HashSet<FornecedorHistoricoAlteracao>();
            FornecedorMaterials = new HashSet<FornecedorMaterial>();
            FornecedorSocios = new HashSet<FornecedorSocio>();
            FornecedorPendencias = new HashSet<FornecedorPendencia>();
            FornecedorServicoes = new HashSet<FornecedorServico>();
            FornecedorSocios1 = new HashSet<FornecedorSocio>();
            UsuarioSites = new HashSet<UsuarioSite>();
        }

        [Required]
        [StringLength(120)]
        [DataMember]
        public string NomeFornecedor { get; set; }

        [StringLength(100)]
        [DataMember]
        public string NomeFantasia { get; set; }

        [StringLength(100)]
        [DataMember]
        public string NomeArtistico { get; set; }

        [StringLength(18)]
        [DataMember]
        public string CPFCNPJ { get; set; }

        [StringLength(20)]
        [DataMember]
        public string NumRG { get; set; }

        [DataMember]
        public DateTime? DataEmissaoRG { get; set; }

        [StringLength(20)]
        [DataMember]
        public string NomeOrgaoRG { get; set; }

        [StringLength(50)]
        [DataMember]
        public string Naturalidade { get; set; }

        [DataMember]
        public DateTime? DataNascimento { get; set; }

        [StringLength(20)]
        [DataMember]
        public string NumPISPASEP { get; set; }

        [StringLength(20)]
        [DataMember]
        public string NumInscricaoINSS { get; set; }

        [StringLength(150)]
        [DataMember]
        public string Site { get; set; }

        [DataMember]
        public int? CodFormaPagamento { get; set; }

        [DataMember]
        public int? CodClasseContaBancaria { get; set; }

        [DataMember]
        public int? CodContaBancaria { get; set; }

        [DataMember]
        public int? NumBanco { get; set; }
                
        [StringLength(10)]
        [DataMember]
        public string CodAgencia { get; set; }
               
        [StringLength(10)]
        [DataMember]
        public string NumContaBancaria { get; set; }

        [DataMember]
        public int? DigitoVerificador { get; set; }

        [StringLength(20)]
        [DataMember]
        public string NumInscricaoEstadual { get; set; }

        [StringLength(10)]
        [DataMember]
        public string NumInscricaoMunicipal { get; set; }

        [DataMember]
        public DateTime? DataVencimentoCadastro { get; set; }

        [DataMember]
        public bool? IndCEPOM_RJ { get; set; }

        [DataMember]
        public bool? indCEPOM_SP { get; set; }

        [DataMember]
        public bool? indCEPOM_MG { get; set; }

        [DataMember]
        public int? CodRegimeEmpresa { get; set; }

        [DataMember]
        public int? CodSexo { get; set; }

        [StringLength(50)]
        [DataMember]
        public string AccountNumber { get; set; }

        [StringLength(50)]
        [DataMember]
        public string SwiftCode { get; set; }

        [StringLength(100)]
        [DataMember]
        public string Bank { get; set; }

        [ForeignKey("NaturezaJuridica")]
        [DataMember]
        public long? IdNaturezaJuridica { get; set; }
                
        public virtual ConteudoTabelaDominio NaturezaJuridica { get; set; }

        [ForeignKey("TipoPessoa")]
        [DataMember]
        public long? IdTipoPessoa { get; set; }
                
        public virtual ConteudoTabelaDominio TipoPessoa { get; set; }

        [ForeignKey("Nacionalidade")]
        [DataMember]
        public long? IdNacionalidade { get; set; }
                
        public virtual ConteudoTabelaDominio Nacionalidade { get; set; }

        [ForeignKey("StatusFornecedor")]
        [DataMember]
        public long? IdStatusFornecedor { get; set; }
        
        public int AreaAtual { get; set; }

        public int? CodFornecedorConspiraware { get; set; }

        [ForeignKey("Operacao")]
        [DataMember]
        public long? IdOperacao { get; set; }

        public virtual ConteudoTabelaDominio Operacao { get; set; }
                
        public virtual ConteudoTabelaDominio StatusFornecedor { get; set; }
                
        public virtual ICollection<FornecedorTelefone> FornecedorTelefones { get; set; }
                
        public virtual ICollection<FornecedorAvaliacao> FornecedorAvaliacaos { get; set; }
                
        public virtual ICollection<FornecedorCNAE> FornecedorCNAEs { get; set; }
                
        public virtual ICollection<FornecedorContato> FornecedorContatoes { get; set; }
                
        public virtual ICollection<FornecedorContrato> FornecedorContratoes { get; set; }

        public virtual ICollection<FornecedorDocumento> FornecedorDocumentoes { get; set; }
                
        public virtual ICollection<FornecedorEmail> FornecedorEmails { get; set; }
                
        public virtual ICollection<FornecedorEndereco> FornecedorEnderecoes { get; set; }

        public virtual ICollection<FornecedorFaleConosco> FornecedorFaleConoscoes { get; set; }

        public virtual ICollection<FornecedorHistoricoAlteracao> FornecedorHistoricoAlteracaos { get; set; }
                
        public virtual ICollection<FornecedorMaterial> FornecedorMaterials { get; set; }
                
        public virtual ICollection<FornecedorSocio> FornecedorSocios { get; set; }

        public virtual ICollection<FornecedorPendencia> FornecedorPendencias { get; set; }
                
        public virtual ICollection<FornecedorServico> FornecedorServicoes { get; set; }
                
        public virtual ICollection<FornecedorSocio> FornecedorSocios1 { get; set; }
                
        public virtual ICollection<UsuarioSite> UsuarioSites { get; set; }

        [NotMapped]
        long ILogFornecedor.IdFornecedor
        {
            get { return this.Id; }
        }

        [NotMapped]
        public string NomeEntidade
        {
            get { return "Fornecedor"; }
        }

        [NotMapped]
        public bool EhPessoaFisica 
        {
            get { return this.TipoPessoa.Codigo == (int)TipoPessoaEnum.PessoaFisicaEstabelecidaForaPais || this.TipoPessoa.Codigo == (int)TipoPessoaEnum.PessoaFisicaEstabelecidaPais; } 
        }

        public void AtualizarModuloAtual(ModuloFornecedorEnum novoModuloAtual)
        {
            var idModuloAtual = (int) novoModuloAtual;
            AreaAtual = idModuloAtual > AreaAtual ? idModuloAtual : AreaAtual;
        }

        public bool PossuiPendenciaEmAberto(TipoPendenciaEnum tipoPendenciaEnum, ConteudoTabelaDominio tipoDocumento)
        {
            var query = this.FornecedorPendencias.Where(x => !x.DataResolucao.HasValue && x.TipoPendencia != null && x.TipoPendencia.Codigo == (int)tipoPendenciaEnum);

            if (tipoDocumento != null)
                query = query.Where(x => x.TipoDocumento != null && x.TipoDocumento.Id == tipoDocumento.Id);

            return query.Any();
        }

        public IEnumerable<FornecedorPendencia> PendenciasEmAberto(IEnumerable<TipoPendenciaEnum> tiposPendencia, ConteudoTabelaDominio tipoDocumento)
        {
            var idsTipos = tiposPendencia.Select(x=> (int)x);
            var query = this.FornecedorPendencias.Where(x => !x.DataResolucao.HasValue && x.TipoPendencia != null && idsTipos.Contains(x.TipoPendencia.Codigo));

            if (tipoDocumento != null)
                query = query.Where(x => x.TipoDocumento != null && x.TipoDocumento.Id == tipoDocumento.Id);

            return query;
        }
    }
}
