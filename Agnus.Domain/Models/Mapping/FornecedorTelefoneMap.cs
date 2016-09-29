using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class FornecedorTelefoneMap : EntityTypeConfiguration<FornecedorTelefone>
    {
        public FornecedorTelefoneMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdFornecedorTelefone);

            // Properties
            this.Property(t => t.NumIdFornecedorTelefone)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TxtTelefone)
                .IsRequired()
                .HasMaxLength(21);

            // Table & Column Mappings
            this.ToTable("FornecedorTelefone");
            this.Property(t => t.NumIdFornecedorTelefone).HasColumnName("NumIdFornecedorTelefone");
            this.Property(t => t.NumIdFornecedor).HasColumnName("NumIdFornecedor");
            this.Property(t => t.TxtTelefone).HasColumnName("TxtTelefone");
            this.Property(t => t.CodTipoTelefone).HasColumnName("CodTipoTelefone");
            this.Property(t => t.CodTipoUsoTelefone).HasColumnName("CodTipoUsoTelefone");

            // Relationships
            this.HasRequired(t => t.ConteudoTabelaDominio)
                .WithMany(t => t.FornecedorTelefones)
                .HasForeignKey(d => d.CodTipoTelefone);
            this.HasRequired(t => t.ConteudoTabelaDominio1)
                .WithMany(t => t.FornecedorTelefones1)
                .HasForeignKey(d => d.CodTipoUsoTelefone);
            this.HasRequired(t => t.Fornecedor)
                .WithMany(t => t.FornecedorTelefones)
                .HasForeignKey(d => d.NumIdFornecedor);

        }
    }
}
