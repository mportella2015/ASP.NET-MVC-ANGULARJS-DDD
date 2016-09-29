using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class FornecedorAvaliacaoMap : EntityTypeConfiguration<FornecedorAvaliacao>
    {
        public FornecedorAvaliacaoMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdFornecedorAvaliacao);

            // Properties
            this.Property(t => t.NumIdFornecedorAvaliacao)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.LoginResponsavelAvaliacao)
                .IsRequired()
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("FornecedorAvaliacao");
            this.Property(t => t.NumIdFornecedorAvaliacao).HasColumnName("NumIdFornecedorAvaliacao");
            this.Property(t => t.NumIdFornecedor).HasColumnName("NumIdFornecedor");
            this.Property(t => t.CodResultadoAvaliacao).HasColumnName("CodResultadoAvaliacao");
            this.Property(t => t.DataAvaliacao).HasColumnName("DataAvaliacao");
            this.Property(t => t.LoginResponsavelAvaliacao).HasColumnName("LoginResponsavelAvaliacao");

            // Relationships
            this.HasRequired(t => t.ConteudoTabelaDominio)
                .WithMany(t => t.FornecedorAvaliacaos)
                .HasForeignKey(d => d.CodResultadoAvaliacao);
            this.HasRequired(t => t.Fornecedor)
                .WithMany(t => t.FornecedorAvaliacaos)
                .HasForeignKey(d => d.NumIdFornecedor);

        }
    }
}
