using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class TipoServicoMap : EntityTypeConfiguration<TipoServico>
    {
        public TipoServicoMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdTipoServico);

            // Properties
            this.Property(t => t.NumIdTipoServico)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TxtTipoServico)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.TxtTipoServicoIngles)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TipoServico");
            this.Property(t => t.NumIdTipoServico).HasColumnName("NumIdTipoServico");
            this.Property(t => t.TxtTipoServico).HasColumnName("TxtTipoServico");
            this.Property(t => t.TxtTipoServicoIngles).HasColumnName("TxtTipoServicoIngles");
            this.Property(t => t.NumIdItemPlanoConta).HasColumnName("NumIdItemPlanoConta");

            // Relationships
            this.HasRequired(t => t.ItemPlanoConta)
                .WithMany(t => t.TipoServicoes)
                .HasForeignKey(d => d.NumIdItemPlanoConta);

        }
    }
}
