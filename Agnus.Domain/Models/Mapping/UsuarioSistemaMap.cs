using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class UsuarioSistemaMap : EntityTypeConfiguration<UsuarioSistema>
    {
        public UsuarioSistemaMap()
        {
            // Primary Key
            this.HasKey(t => t.Login);

            // Properties
            this.Property(t => t.Login)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.NomeUsuario)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("UsuarioSistema");
            this.Property(t => t.Login).HasColumnName("Login");
            this.Property(t => t.NomeUsuario).HasColumnName("NomeUsuario");
        }
    }
}
