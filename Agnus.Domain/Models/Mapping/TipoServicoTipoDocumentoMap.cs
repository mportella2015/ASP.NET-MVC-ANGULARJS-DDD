using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class TipoServicoTipoDocumentoMap : EntityTypeConfiguration<TipoServicoTipoDocumento>
    {
        public TipoServicoTipoDocumentoMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdTipoServicoTipoDocumento);

            // Properties
            this.Property(t => t.NumIdTipoServicoTipoDocumento)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TipoServicoTipoDocumento");
            this.Property(t => t.NumIdTipoServicoTipoDocumento).HasColumnName("NumIdTipoServicoTipoDocumento");
            this.Property(t => t.NumIdTipoDocumento).HasColumnName("NumIdTipoDocumento");
            this.Property(t => t.NumIdTipoServico).HasColumnName("NumIdTipoServico");

            // Relationships
            this.HasOptional(t => t.TipoDocumento)
                .WithMany(t => t.TipoServicoTipoDocumentoes)
                .HasForeignKey(d => d.NumIdTipoDocumento);
            this.HasOptional(t => t.TipoServico)
                .WithMany(t => t.TipoServicoTipoDocumentoes)
                .HasForeignKey(d => d.NumIdTipoServico);

        }
    }
}
