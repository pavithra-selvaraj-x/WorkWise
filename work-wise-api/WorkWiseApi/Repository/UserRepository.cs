using Contracts.IRepository;
using Entities;
using Entities.Models;

namespace Repository
{
    /// <summary>
    /// Class <c>UserRepository</c> used to implement the methods related for user.
    /// </summary>
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        /// <summary>
        /// Constructor for injecting
        /// </summary>
        /// <param name="repositoryContext"></param>
        /// <returns></returns>
        public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}