using Contracts.IRepository;
using Entities;
using Services;

namespace Repository
{
    /// <summary>
    /// Configure repository
    /// </summary>
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly RepositoryContext _repoContext;
        private readonly UserIdentityService _userIdentityService;
        private IUserRepository _user;
        private IUserSecretRepository _userSecret;
        private IUserSessionRepository _userSession;
        private IGoalRepository _goal;
        private ITaskRepository _task;

        /// <summary>
        /// Configure repository for user
        /// </summary>
        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_repoContext);
                }
                return _user;
            }
        }

        /// <summary>
        /// Configure repository for user secret
        /// </summary>
        public IUserSecretRepository UserSecret
        {
            get
            {
                if (_userSecret == null)
                {

                    _userSecret = new UserSecretRepository(_repoContext);
                }
                return _userSecret;
            }
        }

        /// <summary>
        /// Configure repository for user session
        /// </summary>
        public IUserSessionRepository UserSession
        {
            get
            {
                if (_userSession == null)
                {
                    _userSession = new UserSessionRepository(_repoContext);
                }
                return _userSession;
            }
        }

        /// <summary>
        /// Configure repository for goal
        /// </summary>
        public IGoalRepository Goal
        {
            get
            {
                if (_goal == null)
                {
                    _goal = new GoalRepository(_repoContext);
                }
                return _goal;
            }
        }

        /// <summary>
        /// Configure repository for task
        /// </summary>
        public ITaskRepository Task
        {
            get
            {
                if (_task == null)
                {
                    _task = new TaskRepository(_repoContext);
                }
                return _task;
            }
        }

        /// <summary>
        /// Constructor for injecting repository context
        /// </summary>
        /// <param name="repositoryContext"></param>
        /// <param name="userIdentityService"></param>
        public RepositoryWrapper(RepositoryContext repositoryContext, UserIdentityService userIdentityService)
        {
            _repoContext = repositoryContext;
            _userIdentityService = userIdentityService;
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <returns></returns>
        public void Save()
        {
            _repoContext.OnBeforeSaving(GetCurrentUser());
            _repoContext.SaveChanges();
        }

        /// <summary>
        /// Get current user
        /// </summary>
        /// <returns></returns>
        private Guid GetCurrentUser()
        {
            return _userIdentityService.GetUserIdentity();
        }
    }
}
