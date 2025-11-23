using Contracts.IRepository;
using Entities;
using Entities.Models;

namespace Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class UserSecretRepository : RepositoryBase<UserSecret>, IUserSecretRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repositoryContext"></param>
        public UserSecretRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}