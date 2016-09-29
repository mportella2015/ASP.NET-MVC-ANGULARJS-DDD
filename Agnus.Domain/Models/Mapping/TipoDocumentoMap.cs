using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class TipoDocumentoMap : EntityTypeConfiguration<TipoDocumento>
    {
        public TipoDocumentoMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdTipoDocumento);

            // Properties
            this.Property(t => t.NumIdTipoDocumento)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TxtTipoDocumento)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.TxtTipoDocumentoIngles)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TipoDocumento");
            this.Property(t => t.NumIdTipoDocumento).HasColumnName("NumIdTipoDocumento");
            this.Property(t => t.PeriodoValidade).HasColumnName("PeriodoValidade");
            this.Property(t => t.TxtTipoDocumento).HasColumnName("TxtTipoDocumento");
            this.Property(t => t.TxtTipoDocumentoIngles).HasColumnName("TxtTipoDocumentoIngles");
            this.Property(t => t.IndObrigatorioPFEstrangeiro).HasColumnName("IndObrigatorioPFEstrangeiro");
            this.Property(t => t.IndObrigatorioPF).HasColumnName("IndObrigatorioPF");
            this.Property(t => t.IndObrigatorioPJEstarngeiro).HasColumnName("IndObrigatorioPJEstarngeiro");
            this.Property(t => t.IndObrigatorioPJ).HasColumnName("IndObrigatorioPJ");
        }
    }
}
