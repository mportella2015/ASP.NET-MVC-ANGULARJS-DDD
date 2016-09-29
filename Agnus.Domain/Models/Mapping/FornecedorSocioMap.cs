using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class FornecedorSocioMap : EntityTypeConfiguration<FornecedorSocio>
    {
        public FornecedorSocioMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdFornecedorSocio);

            // Properties
            this.Property(t => t.NumIdFornecedorSocio)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("FornecedorSocio");
            this.Property(t => t.NumIdFornecedorSocio).HasColumnName("NumIdFornecedorSocio");
            this.Property(t => t.NumIdPessoaJuridica).HasColumnName("NumIdPessoaJuridica");
            this.Property(t => t.NumIdPessoaFisica).HasColumnName("NumIdPessoaFisica");
            this.Property(t => t.CodTipoRelacao).HasColumnName("CodTipoRelacao");

            // Relationships
            this.HasOptional(t => t.ConteudoTabelaDominio)
                .WithMany(t => t.FornecedorSocios)
                .HasForeignKey(d => d.CodTipoRelacao);
            this.HasOptional(t => t.Fornecedor)
                .WithMany(t => t.FornecedorSocios)
                .HasForeignKey(d => d.NumIdPessoaJuridica);
            this.HasOptional(t => t.Fornecedor1)
                .WithMany(t => t.FornecedorSocios1)
                .HasForeignKey(d => d.NumIdPessoaFisica);

        }
    }
}
