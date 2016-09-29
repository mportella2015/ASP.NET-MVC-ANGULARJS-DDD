using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class FornecedorContratoMap : EntityTypeConfiguration<FornecedorContrato>
    {
        public FornecedorContratoMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdFornecedorContrato);

            // Properties
            this.Property(t => t.NumIdFornecedorContrato)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("FornecedorContrato");
            this.Property(t => t.NumIdFornecedorContrato).HasColumnName("NumIdFornecedorContrato");
            this.Property(t => t.NumIdFornecedor).HasColumnName("NumIdFornecedor");
            this.Property(t => t.DataInicioContrato).HasColumnName("DataInicioContrato");
            this.Property(t => t.DataFimContrato).HasColumnName("DataFimContrato");
            this.Property(t => t.CodIndiceReajuste).HasColumnName("CodIndiceReajuste");
            this.Property(t => t.CodStatusContrato).HasColumnName("CodStatusContrato");
            this.Property(t => t.DataCancelamento).HasColumnName("DataCancelamento");

            // Relationships
            this.HasOptional(t => t.ConteudoTabelaDominio)
                .WithMany(t => t.FornecedorContratoes)
                .HasForeignKey(d => d.CodIndiceReajuste);
            this.HasOptional(t => t.ConteudoTabelaDominio1)
                .WithMany(t => t.FornecedorContratoes1)
                .HasForeignKey(d => d.CodStatusContrato);
            this.HasRequired(t => t.Fornecedor)
                .WithMany(t => t.FornecedorContratoes)
                .HasForeignKey(d => d.NumIdFornecedor);

        }
    }
}
