using Agnus.Domain;
using Agnus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Agnus.Service.Authentication
{
    public class AuthenticationService
    {            
        IEntityService<UsuarioSistema> _usuarioSistemaService;
        IEntityService<UsuarioSite> _usuarioSiteService;

        public AuthenticationService(IEntityService<UsuarioSistema> usuarioSistemaService, IEntityService<UsuarioSite> usuarioSiteService)
        {
            this._usuarioSistemaService = usuarioSistemaService;
            this._usuarioSiteService = usuarioSiteService;
        }


        public bool SetAuthentication(ILogin usuarioLogin, HttpResponseBase response)
        {
            if (usuarioLogin.Identifier == null)
                throw new Exception("O identificador do usuario deve ser informado");
            var isAuthenticated = false;
            try
            {
                FormsAuthenticationTicket ticket = this.CreateTicket(usuarioLogin);
                if (ticket == null)
                    throw new Exception("Não foi possivel criar o cookie para o usuário");
                string encTicket = FormsAuthentication.Encrypt(ticket);
                response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                isAuthenticated = true;
            }
            catch (Exception)
            {
                //TODO: Logar exception                
            }

            return isAuthenticated;
        }

        private FormsAuthenticationTicket CreateTicket(ILogin usuarioLogin)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                usuarioLogin.Username,
                DateTime.Now,
                DateTime.Now.AddDays(30),
                true,
                usuarioLogin.Identifier.ToString(),
                FormsAuthentication.FormsCookiePath);
            return ticket;
        }

        public ILogin GetUsuario(string usuario)
        {
            ILogin usuarioLogin;
            usuarioLogin = _usuarioSiteService.GetAllByFilter(x => x.Email.ToUpper() == usuario.ToUpper()).FirstOrDefault();
            if (usuarioLogin == null)            
                usuarioLogin = _usuarioSistemaService.GetAllByFilter(x => x.Login.ToUpper() == usuario.ToUpper()).FirstOrDefault();
            return usuarioLogin;            
        }        
    }
}
