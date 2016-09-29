using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class TipoMaterialMap : EntityTypeConfiguration<TipoMaterial>
    {
        public TipoMaterialMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdTipoMaterial);

            // Properties
            this.Property(t => t.NumIdTipoMaterial)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TxtTipoMaterial)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.TxtTipoMaterialIngles)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TipoMaterial");
            this.Property(t => t.NumIdTipoMaterial).HasColumnName("NumIdTipoMaterial");
            this.Property(t => t.TxtTipoMaterial).HasColumnName("TxtTipoMaterial");
            this.Property(t => t.TxtTipoMaterialIngles).HasColumnName("TxtTipoMaterialIngles");
            this.Property(t => t.NumIdItemPlanoConta).HasColumnName("NumIdItemPlanoConta");

            // Relationships
            this.HasOptional(t => t.ItemPlanoConta)
                .WithMany(t => t.TipoMaterials)
                .HasForeignKey(d => d.NumIdItemPlanoConta);

        }
    }
}
