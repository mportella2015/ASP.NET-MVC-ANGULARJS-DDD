using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class FornecedorEmailMap : EntityTypeConfiguration<FornecedorEmail>
    {
        public FornecedorEmailMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdFornecedorEmail);

            // Properties
            this.Property(t => t.NumIdFornecedorEmail)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TxtEmail)
                .IsRequired()
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("FornecedorEmail");
            this.Property(t => t.NumIdFornecedorEmail).HasColumnName("NumIdFornecedorEmail");
            this.Property(t => t.NumIdFornecedor).HasColumnName("NumIdFornecedor");
            this.Property(t => t.TxtEmail).HasColumnName("TxtEmail");
            this.Property(t => t.CodTipoEmail).HasColumnName("CodTipoEmail");

            // Relationships
            this.HasRequired(t => t.Fornecedor)
                .WithMany(t => t.FornecedorEmails)
                .HasForeignKey(d => d.NumIdFornecedor);

        }
    }
}
