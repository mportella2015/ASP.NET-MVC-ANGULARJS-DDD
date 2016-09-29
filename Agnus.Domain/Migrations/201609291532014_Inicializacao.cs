namespace Agnus.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicializacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TipoQuarto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(maxLength: 100),
                        PossuiCamaCasal = c.String(maxLength: 1),
                        PossuiAR = c.String(maxLength: 1),
                        PossuiFrigoBar = c.String(maxLength: 1),
                        PossuiInternet = c.String(maxLength: 1),
                        PossuiHidroMassagem = c.String(maxLength: 1),
                        QuantidadePermitidaPessoas = c.Int(nullable: false),
                        Observacao = c.String(maxLength: 500),
                        LoginCadastro = c.String(nullable: false, maxLength: 60),
                        DataCadastro = c.DateTime(nullable: false),
                        LoginUltimaAtualizacao = c.String(maxLength: 60),
                        DataUltimaAtualizacao = c.DateTime(),
                        LoginExclusao = c.String(maxLength: 60),
                        DataExclusao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UsuarioSite",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 150),
                        Nome = c.String(nullable: false, maxLength: 100, unicode: false),
                        Senha = c.String(nullable: false, maxLength: 100),
                        IdStatusUsuarioSite = c.Long(nullable: false),
                        IdTipoPessoa = c.Long(nullable: false),
                        PrimeiroAcesso = c.Boolean(nullable: false),
                        LoginCadastro = c.String(nullable: false, maxLength: 60),
                        DataCadastro = c.DateTime(nullable: false),
                        LoginUltimaAtualizacao = c.String(maxLength: 60),
                        DataUltimaAtualizacao = c.DateTime(),
                        LoginExclusao = c.String(maxLength: 60),
                        DataExclusao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true, name: "IX_Email_IX");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.UsuarioSite", "IX_Email_IX");
            DropTable("dbo.UsuarioSite");
            DropTable("dbo.TipoQuarto");
        }
    }
}
