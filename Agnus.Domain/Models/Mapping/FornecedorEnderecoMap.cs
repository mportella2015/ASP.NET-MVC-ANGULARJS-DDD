using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class FornecedorEnderecoMap : EntityTypeConfiguration<FornecedorEndereco>
    {
        public FornecedorEnderecoMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdFornecedorEndereco);

            // Properties
            this.Property(t => t.NumIdFornecedorEndereco)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TxtEndereco)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.TxtComplemento)
                .HasMaxLength(50);

            this.Property(t => t.NumEndereco)
                .IsRequired()
                .HasMaxLength(8);

            this.Property(t => t.NomeBairro)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.NomeCidade)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CodUF)
                .HasMaxLength(2);

            this.Property(t => t.CodCEP)
                .IsRequired()
                .HasMaxLength(9);

            // Table & Column Mappings
            this.ToTable("FornecedorEndereco");
            this.Property(t => t.NumIdFornecedorEndereco).HasColumnName("NumIdFornecedorEndereco");
            this.Property(t => t.CodTipoEndereco).HasColumnName("CodTipoEndereco");
            this.Property(t => t.NumIdFornecedor).HasColumnName("NumIdFornecedor");
            this.Property(t => t.TxtEndereco).HasColumnName("TxtEndereco");
            this.Property(t => t.TxtComplemento).HasColumnName("TxtComplemento");
            this.Property(t => t.NumEndereco).HasColumnName("NumEndereco");
            this.Property(t => t.NomeBairro).HasColumnName("NomeBairro");
            this.Property(t => t.NomeCidade).HasColumnName("NomeCidade");
            this.Property(t => t.CodUF).HasColumnName("CodUF");
            this.Property(t => t.CodCEP).HasColumnName("CodCEP");
            this.Property(t => t.CodPais).HasColumnName("CodPais");

            // Relationships
            this.HasRequired(t => t.ConteudoTabelaDominio)
                .WithMany(t => t.FornecedorEnderecoes)
                .HasForeignKey(d => d.CodTipoEndereco);
            this.HasRequired(t => t.Fornecedor)
                .WithMany(t => t.FornecedorEnderecoes)
                .HasForeignKey(d => d.NumIdFornecedor);

        }
    }
}
