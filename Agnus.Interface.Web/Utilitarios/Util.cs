using Agnus.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Agnus.Interface.Web.Utilitarios
{
    public static class Util
    {
        public static IList<SelectListItem> CastToListItem<T>(IEnumerable<T> dataSource, string propertyText, string propertyValue) 
        {
            var listItens = new List<SelectListItem>();
            if (dataSource != null && dataSource.Count() > 0)
                dataSource.ToList().ForEach(
                    delegate(T dataItem) 
                    {
                        listItens.Add(
                            new SelectListItem() 
                            {
                                Text = Util.GetPropertyValueByName(dataItem, propertyText),
                                Value = Util.GetPropertyValueByName(dataItem,propertyValue)                             
                            });
                    });              
            return listItens;

        }


        public static string GetPropertyValueByName(object data, string propertyName)
        {
            if (data == null)
                return null;
            var propertyInfo = data.GetType().GetProperty(propertyName);
            if (propertyInfo == null)
                return null;
            var value = propertyInfo.GetValue(data, null);
            return value == null ? string.Empty : value.ToString();
        }

        public static IEnumerable<dynamic> BuildSourceCombo<T>(IEnumerable<T> list, long? id, string propNameText, string propNameValue = "Id",string textFormat = "{0}") 
            where T : BaseEntity
        {
            var dataSource = new List<dynamic>();

            if (propNameText.Split('|').ToList().Count > 1)
            {
                var textProp = propNameText.Split('|').ToList();

                foreach (var entity in list)
                {
                    dataSource.Add(
                                   new
                                   {
                                       Text = string.Format("{0} - {1}", entity.GetType().GetProperty(textProp[0]).GetValue(entity), entity.GetType().GetProperty(textProp[1]).GetValue(entity)),
                                       Value = entity.GetType().GetProperty(propNameValue).GetValue(entity),
                                       Selected = (id.HasValue && id != 0 && entity.Id == id.Value)
                                   }
                                   );
                }
                return dataSource;
            }
            else
            {
                foreach (var entity in list)
                {
                    dataSource.Add(
                                   new
                                   {
                                       Text = string.Format(textFormat, entity.GetType().GetProperty(propNameText).GetValue(entity)),
                                       Value = entity.GetType().GetProperty(propNameValue).GetValue(entity),
                                       Selected = (id.HasValue && id != 0 && entity.Id == id.Value)
                                   }
                                   );
                }
                return dataSource;
            }
        }

        //public static IEnumerable<dynamic> BuildSourceComboCodigo<T>(IEnumerable<T> list, long? id, List<string> propNameText, string propNameValue = "Id", string textFormat = "{0}")
        //    where T : BaseEntity
        //{
        //    var dataSource = new List<dynamic>();
        //        foreach (var entity in list)
        //        {
        //            dataSource.Add(
        //                           new
        //                           {
        //                               Text = string.Format("{0} - {1}", entity.GetType().GetProperty(propNameText[0]).GetValue(entity), entity.GetType().GetProperty(propNameText[1]).GetValue(entity)),
        //                               Value = entity.GetType().GetProperty(propNameValue).GetValue(entity),
        //                               Selected = (id.HasValue && id != 0 && entity.Id == id.Value)
        //                           }
        //                           );
        //        }
        //        return dataSource;
        //}

        public static string ConverterValorExibicaoMonetaria(decimal valor) 
        {
            return string.Format(new System.Globalization.CultureInfo("pt-BR", false), "R$ {0:n}", valor);
        }

        public static string ConverterValorExibicaoMonetaria(decimal? valor)
        {
            if (valor.HasValue)
                return ConverterValorExibicaoMonetaria(valor.Value);
            return string.Empty;
        }
    }
}