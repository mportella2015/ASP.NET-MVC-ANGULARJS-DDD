using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class FornecedorDocumentoMap : EntityTypeConfiguration<FornecedorDocumento>
    {
        public FornecedorDocumentoMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdFornecedorDocumento);

            // Properties
            this.Property(t => t.NumIdFornecedorDocumento)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Documento)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.NomeArquivo)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("FornecedorDocumento");
            this.Property(t => t.NumIdFornecedorDocumento).HasColumnName("NumIdFornecedorDocumento");
            this.Property(t => t.NumIdTipoDocumento).HasColumnName("NumIdTipoDocumento");
            this.Property(t => t.NumIdFornecedor).HasColumnName("NumIdFornecedor");
            this.Property(t => t.Documento).HasColumnName("Documento");
            this.Property(t => t.DataVencimento).HasColumnName("DataVencimento");
            this.Property(t => t.NomeArquivo).HasColumnName("NomeArquivo");

            // Relationships
            this.HasRequired(t => t.Fornecedor)
                .WithMany(t => t.FornecedorDocumentoes)
                .HasForeignKey(d => d.NumIdFornecedor);
            this.HasRequired(t => t.TipoDocumento)
                .WithMany(t => t.FornecedorDocumentoes)
                .HasForeignKey(d => d.NumIdTipoDocumento);

        }
    }
}
