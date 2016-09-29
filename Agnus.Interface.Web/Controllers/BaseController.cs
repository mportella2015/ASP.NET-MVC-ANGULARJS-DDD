using System;
using System.Threading;
using System.Web.Mvc;
using Agnus.Domain.Models;
using System.Web.Security;
using System.Web;
using Agnus.Service;
using System.Globalization;
using System.IO;

namespace Agnus.Interface.Web.Controllers
{
    public abstract class BaseController : Controller//, IAuthorizationFilter
    {
        public const string SESSION_USUARIO_ID = "SHHRTGS%&%GQAH87492HF8J%&H329U40284DPJS";
        const string SESSION_USUARIO_SITE_ID = "SHHRTGS%&%GQAH87492HF8J%&ASDASKQ##)#";

        public const string OPERACAO_EDITAR = "Editar";
        public const string OPERACAO_INCLUIR = "Incluir";
        public const string OPERACAO_VISUALIZAR = "Visualizar";

        public long IdFornecedor
        {
            get
            {
                if (Session["IdFornecedor"] != null)
                    return (long)Session["IdFornecedor"];
                return 0;
            }
            set
            {
                Session.Add("IdFornecedor", value);
            }
        }

        public string Checks
        {
            get
            {
                if (Session["Checks"] != null)
                    return Session["Checks"].ToString();
                return string.Empty;
            }
            set
            {
                Session.Add("Checks", value);
            }
        }

        public CultureInfo Culture
        {
            get
            {
                if (Session["Culture"] == null)
                    Session["Culture"] = new CultureInfo("pt-br");
                return (CultureInfo)Session["Culture"];
            }
            set
            {
                Session.Add("Culture", value);
            }
        }

        public string ActionName
        {
            get
            {
                if (Session["ActionName"] != null)
                    return Session["ActionName"].ToString();
                return string.Empty;
            }
            set
            {
                Session.Add("ActionName", value);
            }
        }

        public UsuarioSite UsuarioSite { get { return this.GetUserSiteSession(); } private set { } }

        public UsuarioSistema UsuarioLogado { get { return this.GetUserSession(); } private set { } }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding)
        {
            return Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
        }

        //protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        //{
        //    return new JsonNetResult
        //    {
        //        Data = data,
        //        ContentType = contentType,
        //        ContentEncoding = contentEncoding,
        //        JsonRequestBehavior = behavior
        //    };
        //}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (this.UsuarioLogado == null && this.UsuarioSite == null)
            {
                HttpCookie cookie = filterContext.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null)
                {
                    var id = Convert.ToInt64(FormsAuthentication.Decrypt(cookie.Value).UserData);
                    var usuario = DependencyResolver.Current.GetService<IEntityService<UsuarioSistema>>().GetBy(id);

                    if (usuario == null)
                        Logout();
                    else
                        PutUserSession(usuario);
                }
            }

            if (this.UsuarioLogado != null)
            {
                Thread.CurrentThread.Name = this.UsuarioLogado.Login;
                ViewBag.UsuarioLogado = this.UsuarioLogado;
            }
            if (this.UsuarioSite != null)
            {
                Thread.CurrentThread.Name = this.UsuarioSite.Email;
                if (!filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Contains("CadastroFornecedor") && !filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Contains("AcessoSite"))
                    Logout();
                else
                    ViewBag.UsuarioLogado = this.UsuarioSite;
            }

            Thread.CurrentThread.CurrentCulture = this.Culture;
            Thread.CurrentThread.CurrentUICulture = this.Culture;
            base.OnActionExecuting(filterContext);
        }

        protected void PutUserSession(UsuarioSistema usuario)
        {
            if (usuario != null)
            {
               
            }
            else
            {
                LimparSessao();
            }
            Session[SESSION_USUARIO_ID] = usuario;
        }

        protected void PutUserSiteSession(UsuarioSite usuario)
        {
            if (usuario == null)
            {
                LimparSessao();
            }

            Session[SESSION_USUARIO_SITE_ID] = usuario;
        }

        private UsuarioSistema GetUserSession()
        {
            return Session[SESSION_USUARIO_ID] as UsuarioSistema;
        }

        private UsuarioSite GetUserSiteSession()
        {
            return Session[SESSION_USUARIO_SITE_ID] as UsuarioSite;
        }

        protected void FlushSession()
        {
            Session.Clear();
            Session.Abandon();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            this.FlushSession();
            FormsAuthentication.RedirectToLoginPage();
            return null;
        }

        public string ViewToHtml(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        protected void LimparSessao()
        {
            this.IdFornecedor = default(long);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
    }
}
