using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class TabelaGenericaDominioMap : EntityTypeConfiguration<TabelaGenericaDominio>
    {
        public TabelaGenericaDominioMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdTabelaGenericaDominio);

            // Properties
            this.Property(t => t.NumIdTabelaGenericaDominio)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.NomeTabelaGenericaDominio)
                .IsRequired()
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("TabelaGenericaDominio");
            this.Property(t => t.NumIdTabelaGenericaDominio).HasColumnName("NumIdTabelaGenericaDominio");
            this.Property(t => t.NomeTabelaGenericaDominio).HasColumnName("NomeTabelaGenericaDominio");
            this.Property(t => t.IndApenasVisualizacao).HasColumnName("IndApenasVisualizacao");
        }
    }
}
