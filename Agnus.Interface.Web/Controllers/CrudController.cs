using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Agnus.Domain;
using Agnus.Framework;
using Agnus.Service;
using System.Linq.Expressions;
using Agnus.Framework.Helper;
using System.Text.RegularExpressions;
using System.Data.Entity.Infrastructure;
using Agnus.Interface.Web.Models.ViewModels;

namespace Agnus.Interface.Web.Controllers
{
    public class CrudController<TView, TEntity> : BaseController
        where TView : BaseViewModel<TView>
        where TEntity : AuditableEntity
    {
        protected IEntityService<TEntity> service = DependencyResolver.Current.GetService<IEntityService<TEntity>>();

        public virtual ActionResult Index(long? Id = null)
        {
            ViewBag.Controller = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View("Index");
        }

        public virtual ActionResult IndexWithAlert(string alertMessage)
        {
            ViewBag.AlertMessage = alertMessage;
            return Index();
        }

        public virtual ActionResult Incluir()
        {
            return View("Incluir");
        }
        
        public virtual ActionResult Visualizar(long id)
        {
            var result = Editar(id);
            ViewBag.Editar = false;
            return result;
        }

        public virtual ActionResult Editar(long id)
        {
            var entity = service.GetBy(id);
            var modelEntity = BaseViewModel<TView>.GetEntityMap(entity);
            return View("Incluir", modelEntity);
        }

        [HttpPost]
        public virtual ActionResult Incluir(TView entity)
        {
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                if (entity.Id != 0)
                {
                    var dbEntity = MapModelContext(entity);
                    return Save(dbEntity, entity);
                }
                else
                {
                    var dbEntity = BaseViewModel<TView>.Map<TEntity>(entity);
                    var result = Save(dbEntity, entity);
                    entity.Id = dbEntity.Id;
                    return result;
                }

            }
            return new HttpStatusCodeResult(400, MensagemErro(ModelState));
        }

        public virtual JsonResult Excluir(long id)
        {
            var entity = Find(id);
            service.Delete(entity);
            return null;
        }

        [HttpPost]
        public virtual ActionResult Editar(TView entity)
        {
            if (ModelState.IsValid)
            {
                var dbEntity = Find(entity.Id);
                BaseViewModel<TView>.MapToDataBase<TEntity>(entity, dbEntity);
                return Save(dbEntity, entity);
            }
            return new HttpStatusCodeResult(400, MensagemErro(ModelState));
        }

        protected TEntity Find(long id)
        {
            return ((EntityService<TEntity>)service).GetBy(id);
        }

        public virtual void SaveEntity(TEntity entity, TView entityView)
        {
            service.Save(entity);
        }

        public virtual ActionResult Save(TEntity entity, TView entityView)
        {
            try
            {
                SaveEntity(entity, entityView);
                ViewData["Mensagem"] = "Registro salvo com sucesso!";
            }
            catch (DbUpdateException e)
            {
                var errorMsg = e.InnerException.InnerException.Message;
                var msg = string.Empty;
                if (errorMsg.Contains("unique index"))
                {
                    Match match = Regex.Match(errorMsg, @"IX_([A-Za-z0-9\-]+)_IX");
                    if (match != null)
                    {
                        //msg = string.Format("O campo {0} deve ser único.", match.Value.Replace("IX_", string.Empty).Replace("_IX", string.Empty));
                        msg = string.Format("Campo '{0}': Esta numeração já existe.", match.Value.Replace("IX_", string.Empty).Replace("_IX", string.Empty));
                    }
                }

                ViewData["Mensagem"] = msg;
                return View();
            }
            catch (Exception ex)
            {
                ViewData["Mensagem"] = "Ocorreu um erro: " + ex.Message;
                return View();
            }
            return Redirect("Index");
        }

        protected virtual ActionResult Redirect(string action, string controller = null, object routeValues = null)
        {
            if (controller != null && routeValues != null)
                return RedirectToAction(action, controller, routeValues);
            if (controller != null)
                return RedirectToAction(action, controller);
            return RedirectToAction(action);
        }

        protected string MensagemErro(ModelStateDictionary modelState, IList<string> errosAdicionais = null)
        {
            var errors = ViewData.ModelState.Where(n => n.Value.Errors.Count > 0).ToList();
            var strErros = "<ul class='model-error'><li>";
            var lE = new List<string>();
            errors.ForEach(x => lE.AddRange(x.Value.Errors.Select(z => z.ErrorMessage)));
            if (errosAdicionais != null && errosAdicionais.Count > 0)
                lE.AddRange(errosAdicionais);
            strErros += String.Join("</li><li>", lE);
            strErros += "</li></ul>";
            return new MvcHtmlString(strErros).ToHtmlString();
        }

        protected string MensagemErro(Exception ex)
        {
            var strErros = "<ul class='model-error'><li>";
            strErros += ex.Message;
            strErros += "</li></ul>";
            return new MvcHtmlString(strErros).ToHtmlString();
        }

        protected IEnumerable<dynamic> GetDataSourceCombo<T>(long? id, string propNameText, Expression<Func<T, bool>> predicate = null, bool novoRegistro = false, List<string> textos = null, string propNameValue = "Id") where T : BaseEntity
        {
            var service = DependencyResolver.Current.GetService<IEntityService<T>>();
            var query = new List<T>();

            if (predicate != null)
                query = service.GetAllByFilter(predicate).ToList();
            else
                query = service.GetAll().ToList();

            return Utilitarios.Util.BuildSourceCombo(query, id, propNameText, propNameValue);
        }

        [HttpPost]
        public virtual JsonResult Pesquisar(string sidx, string sord, int page, int rows, string filters, bool loadonce)
        {
            try
            {
                var predicado = Filtrar();
                var dataSource = service.GetAllQuery(predicado);
                return Json(GetDataSource(dataSource));
            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        public virtual Expression<Func<TEntity, bool>> Filtrar()
        {
            return PredicateBuilder.True<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetDataSource(IEnumerable<TEntity> dataSource)
        {
            return dataSource;
        }


        public virtual JsonResult GetDropDownActionDataSource(int? id)
        {
            return Json(new List<dynamic>(), JsonRequestBehavior.AllowGet);
        }

        public virtual TEntity MapModelContext(TView entity)
        {
            var dbEntity = Find(entity.Id);
            BaseViewModel<TView>.MapToDataBase<TEntity>(entity, dbEntity);
            return dbEntity;
        }     
    }
}
