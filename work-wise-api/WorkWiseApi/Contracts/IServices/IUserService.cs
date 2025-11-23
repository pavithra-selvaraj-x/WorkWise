using Entities.Models;
using Entities.Dtos;

namespace Contracts.IServices;

/// <summary>
/// Interface for user service.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Authenticate the user with credentials and generate the JWT token.
    /// </summary>
    /// <param name="loginDto">The LoginDto object contains the user credentials.</param>
    TokenDto GetAuthToken(LoginDto loginDto);

    /// <summary>
    /// Get the user info based on email address.
    /// </summary>
    /// <param name="email"></param>
    /// <returns>User</returns>
    User GetUserByEmail(string email);

    /// <summary>
    /// Validate the password with the secret value stored in database.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="secret"></param>
    /// <param name="iterations"></param>
    /// <returns></returns>
    bool ValidateSecret(Guid id, string secret, int iterations);

    /// <summary>
    /// Method to create a user.
    /// </summary>
    /// <param name="userDto">The user details dto which contains information of user</param>
    /// <returns name= "userId">Id of created user</returns>
    ResponseDto CreateUser(UserDto userDto);

    /// <summary>
    /// Check if user already exists for given email
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <returns>
    /// True or false
    /// </returns>
    bool IsUserExists(string email);

    /// <summary>
    /// Method to get user details based on Id
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <returns>
    /// UserDto object containing the user details.
    /// </returns>
    UserDto GetUser(Guid userId);

    /// <summary>
    /// Method to reset the user password.
    /// </summary>
    /// <param name="resetPasswordDto">The resetPasswordDto object with the user reset details</param>
    /// <param name="loggedInUserId">The logged in user Id</param>
    void ResetUserPassword(ResetPasswordDto resetPasswordDto, Guid loggedInUserId);

    /// <summary>
    /// Method to update user details.
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <param name="userDto">The user details dto which contains information of user</param>
    void UpdateUserDetails(Guid userId, UserDto userDto);

    /// <summary>
    /// Method to delete user details
    /// </summary>
    /// <param name="userId">The id of the user</param>
    void DeleteUser(Guid userId);
}