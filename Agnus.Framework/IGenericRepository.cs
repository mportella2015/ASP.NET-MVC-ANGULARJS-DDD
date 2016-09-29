using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Framework
{
    public interface IGenericRepository<T> where T : BaseEntity
    {        
        IEnumerable<T> GetAll();
        IQueryable<T> GetAllQuery();
        T FindById(long id);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        IEnumerable<T> FindBy(string predicate);
        T Delete(T entity);
        T Save(T entity);
        T Edit(T entity);
        void AttachEntity(T entity);
        int ExecuteSqlCommand(string sqlCommand);
        void ChangeContext(DbContext context);

        void Refresh(object entity);
        void AttachAny<TAny>(ICollection<TAny> entityList) where TAny : Entity;        

        IGenericRepository<TAny> GetRepositoryInstance<TAny>() where TAny : Entity;
        void Detach(object entity);
    }
}
