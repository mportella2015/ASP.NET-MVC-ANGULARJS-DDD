namespace Agnus.Domain.Models
{
    using Agnus.Framework.Helper;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;
    using System.Security.Cryptography;

    [Table("UsuarioSite")]
    [DataContract]
    public partial class UsuarioSite : KeyAuditableEntity , ILogin
    {
        [Required]
        [StringLength(150)]
        [Index("IX_Email_IX", IsUnique = true)]
        [DataMember]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        [DataMember]
        public string Nome { get; set; }

        [Required]
        [StringLength(100)]
        public string Senha { get; set; }

        [DataMember]
        public long IdStatusUsuarioSite { get; set; }

        [DataMember]
        public long IdTipoPessoa { get; set; }

        public bool PrimeiroAcesso { get; set; }

        public string Username
        {
            get { return this.Email; }
        }

        public object Identifier
        {
            get { return this.Id; }
        }


        public bool FullAccess
        {
            get { return false; }
        }

        public string DisplayName
        {
            get { return this.Nome; }
        }

        
        public void AtualizarSenha(string senha)
        {
            Senha = CriptografarSenha(senha);
        }

        public string CriptografarSenha(string senha)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                return Util.GetMd5Hash(md5Hash, senha);
            }
        }
    }
}
