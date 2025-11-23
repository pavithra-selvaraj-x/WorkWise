using System.Linq.Expressions;

namespace Contracts.IRepository
{
    /// <summary>
    /// Interface <c>IRepositoryBase</c> used to define the methods for the repository base.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepositoryBase<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        int FindCountByCondition(Expression<Func<T, bool>> expression);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        T FindFirstByCondition(Expression<Func<T, bool>> expression);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        void Create(T entity);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        void CreateBatch(List<T> entities);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        void UpdateBatch(List<T> entities);

    }
}