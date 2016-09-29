using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class FornecedorFaleConoscoMap : EntityTypeConfiguration<FornecedorFaleConosco>
    {
        public FornecedorFaleConoscoMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdFornecedorFaleConosco);

            // Properties
            this.Property(t => t.NumIdFornecedorFaleConosco)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TxtFornecedorFaleConosco)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("FornecedorFaleConosco");
            this.Property(t => t.NumIdFornecedorFaleConosco).HasColumnName("NumIdFornecedorFaleConosco");
            this.Property(t => t.DataFornecedorFaleConosco).HasColumnName("DataFornecedorFaleConosco");
            this.Property(t => t.TxtFornecedorFaleConosco).HasColumnName("TxtFornecedorFaleConosco");
            this.Property(t => t.NumIdFornecedor).HasColumnName("NumIdFornecedor");

            // Relationships
            this.HasOptional(t => t.Fornecedor)
                .WithMany(t => t.FornecedorFaleConoscoes)
                .HasForeignKey(d => d.NumIdFornecedor);

        }
    }
}
