using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agnus.Framework;
using System.Linq.Expressions;

namespace Agnus.Service
{
    public interface IService
    {
    }

    public interface IEntityService<T> : IService
    where T : BaseEntity
    {
        T Save(T entity);
        void Delete(T entity);
        IEnumerable<T> GetAll();
        IQueryable<T> GetAllQuery();
        void AttachEntity(T entity);
        IEnumerable<T> GetAllByFilter(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAllByFilter(string predicate);
        bool CheckIfExists(string property, object value);
        T GetBy(long id);
        IEnumerable<T> GetAllQuery(Expression<Func<T, bool>> predicate = null);

        void AttachAny<TAny>(ICollection<TAny> entityList) where TAny : Entity;

        IEntityService<TAny> GetEnityServiceInstance<TAny>() where TAny : Entity;
        void SaveEntity(T entity);

        void Detach<TAny>(TAny entity) where TAny : Entity;
    }
}
