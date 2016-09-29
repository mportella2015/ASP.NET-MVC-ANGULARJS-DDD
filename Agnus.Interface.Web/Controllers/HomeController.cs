using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using Agnus.Domain.Models;
using Agnus.Domain.Models.Enum;
using Agnus.Service;
using System.Web.Security;
using Agnus.Domain;
using Agnus.Service.Authentication;

namespace Agnus.Interface.Web.Controllers.TipoQuarto
{
    public class LoginController : BaseController
    {
        public IEntityService<UsuarioSistema> _usuarioService = DependencyResolver.Current.GetService<IEntityService<UsuarioSistema>>();
        private IEntityService<UsuarioSite> _usuarioSiteService = DependencyResolver.Current.GetService<IEntityService<UsuarioSite>>();

        public LoginController()
        {
            ViewBag.Descricao = "Página Login";
        }

        public ActionResult Index()
        {
            return View();
        }
        
        [AllowAnonymous]
        public ActionResult Login()
        {
            ClearUser();
            return View();
        }

        private void ClearUser()
        {
            this.PutUserSession(null);
            this.PutUserSiteSession(null);
            ViewBag.UsuarioLogado = null;
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Login(string usuario, string senha, string returnUrl)
        {
            ClearUser();
            JsonResult jsonResult = null;
            if (AutenticarUsuario(usuario, senha, Response, out jsonResult))
                return jsonResult;
            return null;
        }

        private bool AutenticarUsuario(string usuario, string senha, HttpResponseBase response, out JsonResult jsonResult)
        {
            this.Culture = new CultureInfo("pt-br");
            jsonResult = null;
            var _authenticationService = new AuthenticationService(_usuarioService, _usuarioSiteService);

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(senha))
                return false;

            ILogin usuarioLogin = _authenticationService.GetUsuario(usuario);
            if (usuarioLogin == null)
            {
                ViewBag.MensagemErro = "Usuário ou Senha inválidos";
                //jsonResult = View("Login");
                // return true;
            }
            if (!_authenticationService.SetAuthentication(usuarioLogin, response))
                return false;

            var usuarioLoginBaseType = usuarioLogin.GetType().BaseType;
            if (usuarioLoginBaseType == typeof(UsuarioSistema))
            {
                this.PutUserSession(usuarioLogin as UsuarioSistema);
            }
            else if (usuarioLoginBaseType == typeof(UsuarioSite))
            {
                var usuarioSite = (usuarioLogin as UsuarioSite);

                if (usuarioSite.Senha != usuarioSite.CriptografarSenha(senha))
                {
                    ViewBag.MensagemErro = "Usuário ou Senha inválidos";
                    //jsonResult = View("Login");
                    //return true;
                }

                if (usuarioSite.IdStatusUsuarioSite != (int)StatusUsuarioSiteEnum.Ativo && usuarioSite.IdStatusUsuarioSite != (int)StatusUsuarioSiteEnum.Potencial)
                {
                    ViewBag.MensagemErro = "Por favor, entre em contato com SUPORTE";
                    //actionResult = View("Login");
                    // return true;
                }

                this.PutUserSiteSession(usuarioSite);

                //actionResult = RedirectToAction("Index", "Home", new { Id = usuarioSite.Id });
            }
            return true;
        }

        private void SetAuthentication(HttpResponseBase response, string userName, int idUsuario)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                userName,
                DateTime.Now,
                DateTime.Now.AddDays(30),
                true,
                idUsuario.ToString(),
                FormsAuthentication.FormsCookiePath);

            string encTicket = FormsAuthentication.Encrypt(ticket);
            response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
        }
        

    }
}