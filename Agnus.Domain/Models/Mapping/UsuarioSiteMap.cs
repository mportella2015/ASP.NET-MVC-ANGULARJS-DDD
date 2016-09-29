using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class UsuarioSiteMap : EntityTypeConfiguration<UsuarioSite>
    {
        public UsuarioSiteMap()
        {
            // Primary Key
            this.HasKey(t => t.Login);

            // Properties
            this.Property(t => t.Login)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.TxtEmail)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.NomeUsuario)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Senha)
                .IsRequired()
                .HasMaxLength(8);

            this.Property(t => t.CodStatusUsuarioSite)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.CodTipoPessoa)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.IndTrocaSenha)
                .IsRequired()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("UsuarioSite");
            this.Property(t => t.Login).HasColumnName("Login");
            this.Property(t => t.TxtEmail).HasColumnName("TxtEmail");
            this.Property(t => t.NomeUsuario).HasColumnName("NomeUsuario");
            this.Property(t => t.Senha).HasColumnName("Senha");
            this.Property(t => t.NumIdFornecedor).HasColumnName("NumIdFornecedor");
            this.Property(t => t.CodStatusUsuarioSite).HasColumnName("CodStatusUsuarioSite");
            this.Property(t => t.CodTipoPessoa).HasColumnName("CodTipoPessoa");
            this.Property(t => t.IndTrocaSenha).HasColumnName("IndTrocaSenha");

            // Relationships
            this.HasOptional(t => t.Fornecedor)
                .WithMany(t => t.UsuarioSites)
                .HasForeignKey(d => d.NumIdFornecedor);

        }
    }
}
