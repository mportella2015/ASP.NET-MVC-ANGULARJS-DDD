namespace Agnus.Domain.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Data.Entity.Infrastructure;
    using System.Threading;

    public partial class AgnusContext : DbContext
    {
        public AgnusContext()
            : base("name=AgnusContext")
        {            
            this.Configuration.AutoDetectChangesEnabled = true;
        }
                
        //public virtual DbSet<UsuarioSistema> UsuarioSistemas { get; set; }
        public virtual DbSet<UsuarioSite> UsuarioSites { get; set; }
        public virtual DbSet<TipoQuarto> TipoQuarto { get; set; }
     
        public override int SaveChanges()
        {
            var manager = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager;
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity
                    && (x.State == System.Data.Entity.EntityState.Added || x.State == System.Data.Entity.EntityState.Modified || x.State == EntityState.Deleted));

            foreach (var entry in modifiedEntries)
            {
                var entity = entry.Entity as IAuditableEntity;
                if (entity != null)
                {
                    string identityName = Thread.CurrentThread.Name;
                    DateTime now = DateTime.UtcNow;

                    if (entry.State == System.Data.Entity.EntityState.Added)
                    {
                        if (identityName != null)
                        {
                            entity.LoginCadastro = identityName;
                        }
                        else
                        {
                            entity.LoginCadastro = "Sistema";
                        }
                        entity.DataCadastro = now;
                    }
                    else if (entry.State == System.Data.Entity.EntityState.Modified)
                    {
                        base.Entry(entity).Property(x => x.LoginCadastro).IsModified = false;
                        base.Entry(entity).Property(x => x.DataCadastro).IsModified = false;
                    }

                    entity.LoginUltimaAtualizacao = identityName;
                    entity.DataUltimaAtualizacao = now;
                }
            }

            var deletedEntries = ChangeTracker.Entries().Where(x => x.Entity is IAuditableEntity);
            
            try
            {                
                return base.SaveChanges();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<TipoServico>()
            //    .HasMany(e => e.FornecedorContratoServicoes)
            //    .WithRequired(e => e.TipoServico)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<UsuarioSite>()
               .Property(e => e.Nome)
               .IsUnicode(false);

            //modelBuilder.Entity<TipoQuarto>()
            //    .Property(e => e.Descricao)
            //  .IsUnicode(false);

            //modelBuilder.Entity<UsuarioSistema>()
            //    .Property(e => e.Login)
            //    .IsUnicode(false);

            //modelBuilder.Entity<UsuarioSistema>()
            //    .Property(e => e.NomeUsuario)
            //    .IsUnicode(false);

            //modelBuilder.Entity<UsuarioSite>()
            //    .Property(e => e.Email)
            //    .IsUnicode(false);

            //modelBuilder.Entity<UsuarioSite>()
            //    .Property(e => e.Senha)
            //    .IsUnicode(false);

            //modelBuilder.Entity<ObjetoAcao>()
            //    .Property(e => e.Nome)
            //    .IsUnicode(false);

            //modelBuilder.Entity<ObjetoSistema>()
            //    .Property(e => e.Nome)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Perfil>()
            //    .Property(e => e.Nome)
            //    .IsUnicode(false);
        }
    }
}
