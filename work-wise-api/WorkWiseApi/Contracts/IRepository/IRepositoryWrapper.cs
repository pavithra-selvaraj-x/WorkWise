﻿namespace Contracts.IRepository
{
    /// <summary>
    /// Interface <c>IRepositoryWrapper</c> used to define the methods related for product.
    /// </summary>
    public interface IRepositoryWrapper
    {
        /// <summary>
        /// Interface <c>IUserRepository</c> used to define the methods related for User.
        /// </summary>
        IUserRepository User { get; }

        /// <summary>
        /// Interface <c>IUserSecretRepository</c> used to define the methods related for UserSecret.
        /// </summary>
        IUserSecretRepository UserSecret { get; }

        /// <summary>
        /// Interface <c>IUserSessionRepository</c> used to define the methods related for UserSession.
        /// </summary>
        IUserSessionRepository UserSession { get; }

        /// <summary>
        /// Interface <c>IGoalRepository</c> used to define the methods related to goal.
        /// </summary>
        IGoalRepository Goal { get; }

        /// <summary>
        /// Interface <c>IUserSessionRepository</c> used to define the methods related for UserSession.
        /// </summary>
        ITaskRepository Task { get; }

        /// <summary>
        /// Save
        /// </summary>
        void Save();
    }
}
