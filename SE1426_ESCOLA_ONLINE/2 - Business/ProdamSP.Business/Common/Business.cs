using ProdamSP.Domain.Interfaces.Business;
using ProdamSP.Domain.Interfaces.Business.Common;
using ProdamSP.Domain.Entities;
using ProdamSP.Domain.Interfaces.Repository;
using ProdamSP.Domain.Interfaces.Repository.Common;
using ProdamSP.Domain.Interfaces.Validation;
using ProdamSP.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ProdamSP.Business.Common
{
    public class Business<TEntity, TKey> : IBusiness<TEntity, TKey> where TEntity : class
    {
        #region Constructor

        public IRepository<TEntity, TKey> _repository;
        private readonly ValidationResult _validationResult;

        public Business(IRepository<TEntity, TKey> repository)
        {
            _repository = repository;
            _validationResult = new ValidationResult();
        }


        #endregion

        #region Properties

        protected IRepository<TEntity, TKey> Repository
        {
            get { return _repository; }
        }

        protected ValidationResult ValidationResult
        {
            get { return _validationResult; }
        }

        #endregion

        #region Read Methods

        public virtual TEntity Get(TKey id)
        {
            return _repository.Get(id);
        }

        public virtual IEnumerable<TEntity> All()
        {
            return _repository.All();
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _repository.Find(predicate);
        }

        #endregion

        #region CRUD Methods

        public virtual ValidationResult Add(TEntity entity)
        {
            if (!ValidationResult.IsValid)
                return ValidationResult;

            var selfValidationEntity = entity as ISelfValidation;
            if (selfValidationEntity != null && !selfValidationEntity.IsValid)
                return selfValidationEntity.ValidationResult;


            _repository.Add(entity);
            return _validationResult;
        }

        public virtual ValidationResult Update(TEntity entity)
        {
            if (!ValidationResult.IsValid)
                return ValidationResult;

            var selfValidationEntity = entity as ISelfValidation;
            if (selfValidationEntity != null && !selfValidationEntity.IsValid)
                return selfValidationEntity.ValidationResult;

            _repository.Update(entity);
            return _validationResult;
        }

        public virtual ValidationResult Delete(TEntity entity)
        {
            if (!ValidationResult.IsValid)
                return ValidationResult;

            _repository.Delete(entity);
            return _validationResult;
        }

        public void DeleteAll(IEnumerable<TEntity> obj)
        {
            _repository.DeleteAll(obj);
        }
        public void AddAll(IEnumerable<TEntity> obj)
        {
            _repository.AddAll(obj);
        }
        public void SaveAll()
        {
            _repository.SaveAll();
        }
        #endregion
    }
}
