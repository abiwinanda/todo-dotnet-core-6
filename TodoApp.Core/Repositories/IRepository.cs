using System;
namespace TodoApp.Core.Repositories
{
	public interface IRepository<TEntity> where TEntity : class
	{
        IQueryable<TEntity> GetAll();
        Task<TEntity?> GetById(int id);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}

