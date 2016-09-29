using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Agnus.Domain.Models;
using Agnus.Framework;
using Agnus.Repository;
using Agnus.Service.Implementations;
using Agnus.Service.Interfaces;
using Ninject;

namespace Agnus.Service
{
    public class NinjectDependencyResolverService : IDependencyResolver
    {
        public readonly IKernel _kernel;

        public NinjectDependencyResolverService()
        {
            _kernel = new StandardKernel();
            AddBindings();
        }

        private void AddBindings()
        {
            _kernel.Bind<DbContext>()
                        .To<AgnusContext>()
                        .InScope(x => HttpContext.Current);

            _kernel.Bind<IUnitOfWork>()
                .To<UnitOfWork>().InScope(x => HttpContext.Current);

            _kernel.Bind(typeof(IGenericRepository<>))
                .To(typeof(GenericRepository<>));

            _kernel.Bind(typeof(IEntityService<>))
                .To(typeof(EntityService<>));
                      
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }
    }
}