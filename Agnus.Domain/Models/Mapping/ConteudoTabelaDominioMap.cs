using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class ConteudoTabelaDominioMap : EntityTypeConfiguration<ConteudoTabelaDominio>
    {
        public ConteudoTabelaDominioMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdConteudoTabelaDominio);

            // Properties
            this.Property(t => t.NumIdConteudoTabelaDominio)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Texto)
                .HasMaxLength(300);

            this.Property(t => t.TextoIngles)
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("ConteudoTabelaDominio");
            this.Property(t => t.NumIdConteudoTabelaDominio).HasColumnName("NumIdConteudoTabelaDominio");
            this.Property(t => t.NumIdTabelaGenericaDominio).HasColumnName("NumIdTabelaGenericaDominio");
            this.Property(t => t.Codigo).HasColumnName("Codigo");
            this.Property(t => t.Texto).HasColumnName("Texto");
            this.Property(t => t.TextoIngles).HasColumnName("TextoIngles");
            this.Property(t => t.Indicador).HasColumnName("Indicador");
            this.Property(t => t.CodigoConspiraware).HasColumnName("CodigoConspiraware");
            this.Property(t => t.IndAtivo).HasColumnName("IndAtivo");
        }
    }
}
