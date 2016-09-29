using System.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;



public class Utilitario
{
    internal static JsonResult SerializeObject(object obj)
    {
        var jsonData = DataToStringJson(obj);
        return GetJson(new { jsonData });
    }

    public static string DataToStringJson(object obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
    }

    public static JsonResult GetJson(object objJson)
    {
        var json = new JsonResult();
        json.Data = objJson;
        return json;
    }

    internal static PropertyInfo[] ConvertStringPropertyInfo(IEnumerable<string> strProps, Type type)
    {
        return type.GetProperties().Where(x => strProps.Contains(x.Name)).ToArray();
    }

    public static IList<PropertyInfo> GetPropertyByAttribute<T>(Type attributeType)
    {
        return typeof(T).GetProperties().Where(x => x.GetCustomAttribute(attributeType, true) != null).ToList();
    }
}
