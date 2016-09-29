using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Agnus.Framework;

namespace Agnus.Interface.Web.Models.ViewModels
{
    public abstract class BaseViewModel<TView>
    {
        public virtual long Id { get; set; }

        public string NomeExibicao { get; set; } 

        public string CustomActionColumn { get; set; }

        public string Search { get; set; }

        public Expression<Func<dynamic,object>> AdictionalExpression { get; set; }
        
        public static TView GetEntityMap<T>(T entity) where T : BaseEntity
        {                        
            return Mapper.Map<TView>(entity);
        }

        private static TView GetEntityMap(object entity)
        {                        
            var destination = Activator.CreateInstance<TView>();
            return Mapper.Map(entity,destination);
        }

        public static IList<TView> GetAllMap<T>(IList<T> entityList) where T : BaseEntity
        {
            return entityList.Select(x => GetEntityMap<T>(x)).ToList();
        }

        public static IList<TView> GetAllMap(dynamic entityList)
        {            
            return ((IEnumerable<object>)entityList).Select(x => GetEntityMap(x)).ToList();
        }

        public static TModel Map<TModel>(TView entity) 
            where TModel : BaseEntity
        {
            return Mapper.Map<TView, TModel>(entity);            
        }
        
        public virtual List<string> ExclusionCloumns()
        {
            return null;
        }

        public virtual string ActionColumnClientScriptOnClick()
        {
            return "actionExecute";
        }

        public static void MapToDataBase<TModel>(TView entity, TModel model)
        {
            Mapper.Map(entity, model);            
        }

        public  static IList<TModel> MapAll<TModel>(ICollection<TView> collection) where TModel : BaseEntity
        {
            return collection.Select(x => Map<TModel>(x)).ToList();            
        }

        public static TView MapToSameClass(TView source)
        {
            var destination = Activator.CreateInstance<TView>();
            return MapToSameClass(source, destination);
        }

        public static TView MapToSameClass(TView source, TView destination)
        {            
            return Mapper.Map(source, destination);
        }

        
    }
}
