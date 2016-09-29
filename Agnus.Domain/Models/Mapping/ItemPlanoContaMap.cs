using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class ItemPlanoContaMap : EntityTypeConfiguration<ItemPlanoConta>
    {
        public ItemPlanoContaMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdItemPlanoConta);

            // Properties
            this.Property(t => t.NumIdItemPlanoConta)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TxtItemPlanoConta)
                .IsRequired()
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("ItemPlanoContas");
            this.Property(t => t.NumIdItemPlanoConta).HasColumnName("NumIdItemPlanoConta");
            this.Property(t => t.TxtItemPlanoConta).HasColumnName("TxtItemPlanoConta");
        }
    }
}
