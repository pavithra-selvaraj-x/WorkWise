using Entities.Models;

namespace Contracts.IRepository
{
    /// <summary>
    /// Interface <c>IUserRepository</c> used to define the methods related for user.
    /// </summary>
    public interface IUserRepository : IRepositoryBase<User>
    {
    }
}