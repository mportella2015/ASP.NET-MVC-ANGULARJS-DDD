using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class FornecedorHistoricoAlteracaoMap : EntityTypeConfiguration<FornecedorHistoricoAlteracao>
    {
        public FornecedorHistoricoAlteracaoMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdFornecedorAlteracao);

            // Properties
            this.Property(t => t.NumIdFornecedorAlteracao)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.LoginUsuarioResponsavel)
                .IsRequired()
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("FornecedorHistoricoAlteracao");
            this.Property(t => t.NumIdFornecedorAlteracao).HasColumnName("NumIdFornecedorAlteracao");
            this.Property(t => t.NumIdFornecedor).HasColumnName("NumIdFornecedor");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");
            this.Property(t => t.XML).HasColumnName("XML");
            this.Property(t => t.LoginUsuarioResponsavel).HasColumnName("LoginUsuarioResponsavel");
            this.Property(t => t.CodOperacaoCadastral).HasColumnName("CodOperacaoCadastral");

            // Relationships
            this.HasRequired(t => t.Fornecedor)
                .WithMany(t => t.FornecedorHistoricoAlteracaos)
                .HasForeignKey(d => d.NumIdFornecedor);

        }
    }
}
