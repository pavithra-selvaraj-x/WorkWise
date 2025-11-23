using Contracts.IRepository;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        protected RepositoryContext RepositoryContext { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repositoryContext"></param>
        protected RepositoryBase(RepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return RepositoryContext.Set<T>().Where(expression).AsNoTracking();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public int FindCountByCondition(Expression<Func<T, bool>> expression)
        {
            return RepositoryContext.Set<T>().Where(expression).Count();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public T FindFirstByCondition(Expression<Func<T, bool>> expression)
        {
            return RepositoryContext.Set<T>().Where(expression).FirstOrDefault();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void Create(T entity)
        {
            _ = RepositoryContext.Set<T>().Add(entity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        public void CreateBatch(List<T> entities)
        {
            RepositoryContext.Set<T>().AddRange(entities);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            _ = RepositoryContext.Set<T>().Update(entity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        public void UpdateBatch(List<T> entities)
        {
            RepositoryContext.Set<T>().UpdateRange(entities);
        }

    }
}