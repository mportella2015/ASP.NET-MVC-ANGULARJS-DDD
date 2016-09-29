using Agnus.Interface.Web.Models;
using Agnus.Domain.Models;
using Agnus.Domain.Models.Enum;
using Agnus.Framework;
using Agnus.Framework.Helper;
using Agnus.Service;
using System.Collections.Generic;
using System.Web.Mvc;
using Agnus.Service.Implementations;
using System.Linq;
using System.Globalization;
using System;

namespace Agnus.Interface.Web.Controllers
{
    public class TipoQuartoController : CrudController<TipoQuartoViewModel, Domain.Models.TipoQuarto>
    {
        private IEntityService<Domain.Models.TipoQuarto> _tipoQuartoService = DependencyResolver.Current.GetService<IEntityService<Domain.Models.TipoQuarto>>();

        public TipoQuartoController()
        {
            ViewBag.Descricao = "Tipo de Quarto";
        }

        #region Processamentos

        /// <summary>
        /// Lista os dados em tela.
        /// </summary>
        /// <returns></returns>
        public JsonResult Listar()
        {
            try
            {
              return Json(PreparaListaRetornoJson(), JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Salva os dados em tela
        /// </summary>
        /// <param name="atipoQuarto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Salvar(TipoQuartoViewModel atipoQuarto)
        {
            Domain.Models.TipoQuarto tipoQuarto = null;

            try
            {
                if (atipoQuarto.Id != 0)
                {
                    tipoQuarto = _tipoQuartoService.GetBy(atipoQuarto.Id);
                    tipoQuarto = MapperTipoQuarto(tipoQuarto, atipoQuarto);
                    _tipoQuartoService.AttachEntity(tipoQuarto);
                }
                else
                {
                    tipoQuarto = new Domain.Models.TipoQuarto();
                    tipoQuarto = MapperTipoQuarto(tipoQuarto, atipoQuarto);
                }

                _tipoQuartoService.Save(tipoQuarto);

                return Json(PreparaListaRetornoJson(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public override JsonResult Excluir(long Id)
        {
            Domain.Models.TipoQuarto tipoQuarto = null;

            try
            {
                if (Id != 0)
                {
                    tipoQuarto = _tipoQuartoService.GetBy(Id);
                    _tipoQuartoService.AttachEntity(tipoQuarto);
                }

                _tipoQuartoService.Delete(tipoQuarto);

                return Json(PreparaListaRetornoJson(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message + ". " + ex.Source }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Metodos Privados

        /// <summary>
        /// Executa o DE-PARA entre a view e o dominio.
        /// </summary>
        /// <param name="TipoQuarto"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Domain.Models.TipoQuarto MapperTipoQuarto(Domain.Models.TipoQuarto TipoQuarto, TipoQuartoViewModel entity)
        {
            TipoQuarto.Id = entity.Id;
            TipoQuarto.Descricao = entity.Descricao.Trim();
            TipoQuarto.QuantidadePermitidaPessoas = entity.QuantidadePermitidaPessoas;
            TipoQuarto.PossuiCamaCasal = (entity.PossuiCamaCasal == null ? "N" : "S");
            TipoQuarto.PossuiAR = (entity.PossuiAR == null ? "N" : "S"); 
            TipoQuarto.PossuiFrigoBar = (entity.PossuiFrigoBar == null ? "N" : "S");
            TipoQuarto.PossuiInternet = (entity.PossuiInternet == null ? "N" : "S");
            TipoQuarto.PossuiHidroMassagem = (entity.PossuiHidroMassagem == null ? "N" : "S");
            TipoQuarto.Observacao = entity.Observacao ;

            return TipoQuarto;
        }

        private dynamic PreparaListaRetornoJson() 
        {
            var data = new List<dynamic>();

            var listaTipoQuarto = _tipoQuartoService.GetAll().ToList();
            foreach (var entity in listaTipoQuarto)
            {
                data.Add(
                    new
                    {
                        Id = entity.Id,
                        Descricao = entity.Descricao,
                        QuantidadePermitidaPessoas = entity.QuantidadePermitidaPessoas,
                        PossuiCamaCasal = (entity.PossuiCamaCasal == "S" ? "Sim" : "Não"),
                        PossuiFrigoBar = (entity.PossuiFrigoBar == "S" ? "Sim" : "Não"),
                        PossuiHidroMassagem = (entity.PossuiHidroMassagem == "S" ? "Sim" : "Não"),
                        PossuiInternet = (entity.PossuiInternet == "S" ? "Sim" : "Não"),
                        DataCadastro = entity.DataCadastro.ToString("dd/M/yyyy hh:mm:ss", CultureInfo.InvariantCulture),
                        LoginCadastro = entity.LoginCadastro
                    });
            }

            return data;

        }

        #endregion
    }
}