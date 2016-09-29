namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UsuarioSistema")]
    public partial class UsuarioSistema : KeyAuditableEntity , ILogin
    {
        public UsuarioSistema()
        {
           
        }

        [Required]
        [StringLength(150)]
        [Index("IX_Login_IX", IsUnique = true)]
        public string Login { get; set; }

        [Required]
        [StringLength(100)]
        public string NomeUsuario { get; set; }

        public string TxtEmail { get; set; }

        public DateTime? DataInicioAcesso { get; set; }
        public DateTime? DataFimAcesso { get; set; }
        
        public string Username
        {
            get { return Login; }
        }

        public object Identifier
        {
            get { return Id; }
        }


        public bool FullAccess
        {
            get { return true; }
        }

        public string DisplayName
        {
            get { return this.NomeUsuario; }
        }
    }
}
