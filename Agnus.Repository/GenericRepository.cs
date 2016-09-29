using Agnus.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;

namespace Agnus.Repository
{
    public class GenericRepository<T> : IGenericRepository<T>
      where T : BaseEntity
    {
        protected DbContext _entities;
        protected IDbSet<T> _dbset;

        public GenericRepository(DbContext context)
        {
            _entities = context;
            _dbset = context.Set<T>();
        }

        public void ChangeContext(DbContext context)
        {
            _entities = context;
            _dbset = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbset.AsEnumerable<T>();
        }

        public virtual IQueryable<T> GetAllQuery()
        {
            return _dbset.AsQueryable<T>();
        }

        public IEnumerable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> query = _dbset.Where(predicate).AsEnumerable();
            return query;
        }

        public IEnumerable<T> FindBy(string predicate)
        {
            IEnumerable<T> query = _dbset.Where(predicate).AsEnumerable();
            return query;
        }

        public T FindById(long id)
        {
            return _dbset.Find(id);
        }

        public virtual T Delete(T entity)
        {
            return _dbset.Remove(entity);
        }

        public virtual T Save(T entity)
        {            
            if (_entities.Entry(entity).State != System.Data.Entity.EntityState.Modified)
                _dbset.Add(entity);
            return entity;
        }


        public virtual T Edit(T entity)
        {            
            _entities.Entry(entity).State = System.Data.Entity.EntityState.Modified;   
            return entity;            
        }

        public void AttachEntity(T entity)
        {
            _dbset.Attach(entity);
        }

        public void DetachEntity(T entity)
        {
            _entities.Entry(entity).State = System.Data.Entity.EntityState.Detached;      
        }

        private T GetEntityInDataBase(T entity)
        {
            T entityToUpdate = FindById(entity.Id);

            return null;
        }

        public bool ObjetoPossuiRelacionamento()
        {
            var isObjetoComRelacionamento = typeof(T).GetProperties().Any(x => x.PropertyType.IsAssignableFrom(typeof(BaseEntity)) ||(x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>)));
            return isObjetoComRelacionamento;
        }

        public IEnumerable<PropertyInfo> ObterPropriedadesComRelacionamento()
        {
            return typeof(T).GetProperties().Where(x => x.PropertyType.IsAssignableFrom(typeof(BaseEntity)) || (x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>)));
        }


        public void VincularEntidade(T entity)
        {
            if (ObjetoPossuiRelacionamento())
            {
                var props = ObterPropriedadesComRelacionamento();
                foreach (var prop in props)
                {
                    VincularPropriedade(entity,prop);
                }
            }
        }        

        public void VincularPropriedade(T entity,PropertyInfo propertyInfo)
        {
            var propVal = propertyInfo.GetValue(entity, null);
            if (propVal == null)
                return;
        }

        public int ExecuteSqlCommand(string sqlCommand)
        {
            return _entities.Database.ExecuteSqlCommand(sqlCommand);
        }


        public void Refresh(object entity)
        {
            _entities.Entry(entity).Reload();
        }

        public void AttachAny<TAny>(ICollection<TAny> entityList) where TAny : Entity
        {
            foreach (var entity in entityList)            
                _entities.Set<TAny>().Attach(entity);
        }        


        public IGenericRepository<TAny> GetRepositoryInstance<TAny>() where TAny : Entity
        {
            return new GenericRepository<TAny>(this._entities); 
        }

        public void Detach(object entity)
        {
            _entities.Entry(entity).State = EntityState.Detached;
        }
    }
}
