using Agnus.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agnus.Service;
using System.Reflection;
using System.Linq.Expressions;
using LinqKit;

namespace Agnus.Framework
{
    public class EntityService<T> : IEntityService<T> where T : BaseEntity
    {
        protected IUnitOfWork _unitOfWork;
        protected IGenericRepository<T> _repository;                

        public EntityService(IUnitOfWork unitOfWork, IGenericRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }


        public virtual T Save(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            if (((BaseEntity)entity).Id == 0)
                SaveEntity(entity);
            else
                EditEntity(entity);
            _unitOfWork.Commit();
            return entity;
        }
        
        public virtual void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repository.Delete(entity);
            _unitOfWork.Commit();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public virtual IQueryable<T> GetAllQuery()
        {
            return _repository.GetAllQuery();
        }

        public virtual T GetBy(long id)
        {
            return _repository.FindById(id);
        }

        public virtual IEnumerable<T> GetAllByFilter(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _repository.FindBy(predicate);
        }

        public IEnumerable<T> GetAllByFilter(string predicate)
        {
            return _repository.FindBy(predicate);
        }

        public virtual bool CheckIfExists(string property,object value)
        {
            var objects = _repository.GetAll();
            var exist = objects.Any(x => x.GetType().GetProperty(property).GetValue(x).ToString() == ((IEnumerable<string>)value).ToArray()[0]);
            return exist;
        }

        public void AttachEntity(T entity)
        {
            _repository.AttachEntity(entity);
        }


        public IEnumerable<T> GetAllQuery(Expression<Func<T, bool>> predicate = null)
        {
            var query = _repository.GetAllQuery();

            if (predicate != null)
                query = query.AsExpandable().Where(predicate);
            
            return query.OrderBy(x => x.Id);
        }


        public virtual void SaveEntity(T entity)
        {
            _repository.Save(entity);
        }

        public virtual void EditEntity(T entity)
        {
            _repository.Edit(entity); 
        }


        public void AttachAny<TAny>(ICollection<TAny> entityList) 
            where TAny : Entity
        {
            _repository.AttachAny(entityList);
        }

        public IEntityService<TAny> GetEnityServiceInstance<TAny>() 
            where TAny : Entity
        {
            return new EntityService<TAny>(this._unitOfWork, this._repository.GetRepositoryInstance<TAny>());
        }

        public void Detach<TAny>(TAny entity)
            where TAny : Entity
        {
            _repository.Detach(entity);
        }

        public void DetachList<TAny>(ICollection<TAny> entitys)
            where TAny : Entity
        {
            foreach (var entity in entitys.ToList())
                this.Detach(entity);
        }
    }    
}
