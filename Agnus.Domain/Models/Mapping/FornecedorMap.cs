using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class FornecedorMap : EntityTypeConfiguration<Fornecedor>
    {
        public FornecedorMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdFornecedor);

            // Properties
            this.Property(t => t.NumIdFornecedor)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.NomeFornecedor)
                .IsRequired()
                .HasMaxLength(120);

            this.Property(t => t.NomeFantasia)
                .HasMaxLength(100);

            this.Property(t => t.CPFCNPJ)
                .IsRequired()
                .HasMaxLength(18);

            this.Property(t => t.NumRG)
                .HasMaxLength(20);

            this.Property(t => t.NomeOrgaoRG)
                .HasMaxLength(20);

            this.Property(t => t.Naturalidade)
                .HasMaxLength(50);

            this.Property(t => t.NumPISPASEP)
                .HasMaxLength(20);

            this.Property(t => t.NumInscricaoINSS)
                .HasMaxLength(20);

            this.Property(t => t.Site)
                .HasMaxLength(150);

            this.Property(t => t.CodAgencia)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.NumContaBancaria)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.NumInscricaoEstadual)
                .HasMaxLength(20);

            this.Property(t => t.NumInscricaoMunicipal)
                .HasMaxLength(10);

            this.Property(t => t.IndCEPOM_RJ)
                .HasMaxLength(1);

            this.Property(t => t.indCEPOM_SP)
                .HasMaxLength(1);

            this.Property(t => t.indCEPOM_MG)
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("Fornecedor");
            this.Property(t => t.NumIdFornecedor).HasColumnName("NumIdFornecedor");
            this.Property(t => t.CodTipoPessoa).HasColumnName("CodTipoPessoa");
            this.Property(t => t.CodNacionalidade).HasColumnName("CodNacionalidade");
            this.Property(t => t.NomeFornecedor).HasColumnName("NomeFornecedor");
            this.Property(t => t.NomeFantasia).HasColumnName("NomeFantasia");
            this.Property(t => t.CPFCNPJ).HasColumnName("CPFCNPJ");
            this.Property(t => t.NumRG).HasColumnName("NumRG");
            this.Property(t => t.DataEmissaoRG).HasColumnName("DataEmissaoRG");
            this.Property(t => t.NomeOrgaoRG).HasColumnName("NomeOrgaoRG");
            this.Property(t => t.Naturalidade).HasColumnName("Naturalidade");
            this.Property(t => t.DataNascimento).HasColumnName("DataNascimento");
            this.Property(t => t.NumPISPASEP).HasColumnName("NumPISPASEP");
            this.Property(t => t.NumInscricaoINSS).HasColumnName("NumInscricaoINSS");
            this.Property(t => t.Site).HasColumnName("Site");
            this.Property(t => t.CodFormaPagamento).HasColumnName("CodFormaPagamento");
            this.Property(t => t.CodClasseContaBancaria).HasColumnName("CodClasseContaBancaria");
            this.Property(t => t.CodContaBancaria).HasColumnName("CodContaBancaria");
            this.Property(t => t.NumBanco).HasColumnName("NumBanco");
            this.Property(t => t.CodAgencia).HasColumnName("CodAgencia");
            this.Property(t => t.NumContaBancaria).HasColumnName("NumContaBancaria");
            this.Property(t => t.CodNaturezaJuridica).HasColumnName("CodNaturezaJuridica");
            this.Property(t => t.NumInscricaoEstadual).HasColumnName("NumInscricaoEstadual");
            this.Property(t => t.NumInscricaoMunicipal).HasColumnName("NumInscricaoMunicipal");
            this.Property(t => t.DataVencimentoCadastro).HasColumnName("DataVencimentoCadastro");
            this.Property(t => t.CodStatusFornecedor).HasColumnName("CodStatusFornecedor");
            this.Property(t => t.IndCEPOM_RJ).HasColumnName("IndCEPOM_RJ");
            this.Property(t => t.indCEPOM_SP).HasColumnName("indCEPOM_SP");
            this.Property(t => t.indCEPOM_MG).HasColumnName("indCEPOM_MG");
            this.Property(t => t.CodRegimeEmpresa).HasColumnName("CodRegimeEmpresa");
            this.Property(t => t.CodSexo).HasColumnName("CodSexo");

            // Relationships
            this.HasOptional(t => t.ConteudoTabelaDominio)
                .WithMany(t => t.Fornecedors)
                .HasForeignKey(d => d.CodNaturezaJuridica);
            this.HasRequired(t => t.ConteudoTabelaDominio1)
                .WithMany(t => t.Fornecedors1)
                .HasForeignKey(d => d.CodStatusFornecedor);

        }
    }
}
