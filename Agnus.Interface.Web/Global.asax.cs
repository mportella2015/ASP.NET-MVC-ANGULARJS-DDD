using Agnus.Interface.Web.App_Start;
using Agnus.Interface.Web.AutoMapper;
using Agnus.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Agnus.Interface.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DependencyResolver.SetResolver(new NinjectDependencyResolverService());

            ModelBinders.Binders.Add(typeof(long?), new LongNullBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalBinder());
            ModelBinders.Binders.Add(typeof(decimal), new DecimalBinder());

            AutoMapperConfig.RegisterMappings();
        }
    }
}
