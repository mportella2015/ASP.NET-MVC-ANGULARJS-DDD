using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Agnus.Interface.Web.Utilitarios
{
    public static class AjaxMessageResponse
    {        
        public static JsonResult EnviarMensagemSucessoPadrao() 
        {
            return ContruirCorpoMensagem();
        }

        public static JsonResult EnviarMensagemErro(string mensagem) 
        {
            return ContruirCorpoMensagem(mensagem: mensagem, statuscode: HttpStatusCode.BadRequest);
        }

        public static JsonResult EnviarSucessoComConteudo(object conteudo, string mensagem = null)
        {
            return ContruirCorpoMensagem(conteudo: conteudo, mensagem: mensagem);
        }

        private static JsonResult ContruirCorpoMensagem(HttpStatusCode statuscode = HttpStatusCode.OK, string mensagem = null, object conteudo = null)
        {
            var result = new JsonResult();
            result.Data = ConstruirObjetoEnvio(statuscode, mensagem,conteudo);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        private static object ConstruirObjetoEnvio(HttpStatusCode statuscode, string mensagem, object conteudo)
        {
            return new { status = (int)statuscode, mensagem = mensagem, conteudo = conteudo };
        }
    }    
}