using Contracts.IRepository;
using Entities;
using Entities.Models;

namespace Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class UserSessionRepository : RepositoryBase<UserSession>, IUserSessionRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repositoryContext"></param>
        public UserSessionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

    }
}