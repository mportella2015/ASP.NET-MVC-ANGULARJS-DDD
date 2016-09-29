using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class FornecedorContatoMap : EntityTypeConfiguration<FornecedorContato>
    {
        public FornecedorContatoMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdFornecedorContato);

            // Properties
            this.Property(t => t.NumIdFornecedorContato)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.NomeContato)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Setor)
                .HasMaxLength(50);

            this.Property(t => t.Cargo)
                .HasMaxLength(50);

            this.Property(t => t.TxtTelefone)
                .HasMaxLength(21);

            this.Property(t => t.TxtCelularCorporativo)
                .HasMaxLength(21);

            this.Property(t => t.Email)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("FornecedorContato");
            this.Property(t => t.NumIdFornecedorContato).HasColumnName("NumIdFornecedorContato");
            this.Property(t => t.NumIdFornecedor).HasColumnName("NumIdFornecedor");
            this.Property(t => t.NomeContato).HasColumnName("NomeContato");
            this.Property(t => t.IndResponsavelCadastro).HasColumnName("IndResponsavelCadastro");
            this.Property(t => t.Setor).HasColumnName("Setor");
            this.Property(t => t.Cargo).HasColumnName("Cargo");
            this.Property(t => t.TxtTelefone).HasColumnName("TxtTelefone");
            this.Property(t => t.Ramal).HasColumnName("Ramal");
            this.Property(t => t.TxtCelularCorporativo).HasColumnName("TxtCelularCorporativo");
            this.Property(t => t.Email).HasColumnName("Email");

            // Relationships
            this.HasRequired(t => t.Fornecedor)
                .WithMany(t => t.FornecedorContatoes)
                .HasForeignKey(d => d.NumIdFornecedor);

        }
    }
}
