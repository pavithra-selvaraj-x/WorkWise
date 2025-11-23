using Contracts;
using Contracts.IRepository;
using Contracts.IServices;
using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using ExceptionHandler;
using Microsoft.Extensions.Configuration;
using Services.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Entities.Common;
using Entities.Enums;

namespace Services;

public class UserService : IUserService
{
    private readonly ILoggerManager _logger;
    private readonly IRepositoryWrapper _repoWrapper;

    private readonly IMapper _mapper;

    private readonly IConfiguration _configuration;

    /// <summary>
    /// Constructor for injecting mapper, repoWrapper, logger
    /// </summary>
    /// <param name="mapper">mapper object</param>
    /// <param name="repoWrapper">The repoWrapper object</param>
    /// <param name="logger">The logger object</param>
    /// <param name="configuration"></param>
    public UserService(IMapper mapper, IRepositoryWrapper repoWrapper, ILoggerManager logger, IConfiguration configuration)
    {
        _mapper = mapper;
        _repoWrapper = repoWrapper;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// The method <c>GetAuthToken</c> to get authentication token.
    /// </summary>
    /// <param name="loginDto"></param>
    /// <returns></returns>
    public TokenDto GetAuthToken(LoginDto loginDto)
    {
        _logger.LogInfo("Get authentication token by validating the user credentials");
        User _userInfo = GetUserByEmail(loginDto.UserName);
        if (_userInfo == null)
        {
            _logger.LogWarn("User not found");
            throw new NotFoundCustomException("User not found", "The username or the password you entered is incorrect");
        }
        if (_configuration["Authentication:Iterations"] != null)
        {
            if (ValidateSecret(_userInfo.Id, loginDto.Password, Convert.ToInt32(_configuration["Authentication:Iterations"])))
            {
                var claims = new[]
                {
                        new Claim(JwtRegisteredClaimNames.GivenName, _userInfo.FirstName + ' ' + _userInfo.LastName),
                        new Claim(JwtRegisteredClaimNames.Sub, loginDto.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, _userInfo.Email),
                        new Claim(type: "UserId",_userInfo.Id.ToString()),
                        new Claim(type: "UserName",loginDto.UserName),
                        new Claim(type: "FirstName",_userInfo.FirstName.ToString()),
                        new Claim(type: "LastName",_userInfo.LastName.ToString()),
                        new Claim(type: "Role",_userInfo.Role.ToString()),
                    };

                TokenDto tokenDto = new TokenDto
                {
                    AccessToken = GenerateAccessToken(claims),
                    TokenType = Common.BEARER_TOKEN
                };

                var _userSessionId = CreateUserSession(_userInfo.Id);
                _logger.LogDebug($"User logged in information stored, {_userSessionId} ");
                return tokenDto;
            }
            else
            {
                throw new ForBiddenCustomException("The Username or the password you entered is incorrect", "The Username or the password you entered is incorrect");
            }
        }
        else
        {
            throw new InternalServerCustomException("Internal Server Error - Key Not Found", "Unable to fetch the value for the key Authentication:Iterations");
        }
    }

    /// <summary>
    /// The method <c>GetUserByEmail</c> to get user details by user email.
    /// </summary>
    /// <param name="email"></param>
    /// <returns>User</returns>
    public User GetUserByEmail(string email)
    {
        _logger.LogInfo("Get user information from User table");
        User user = _repoWrapper.User.FindFirstByCondition(user => user.Email.Equals(email) && user.IsActive == true);
        return user;
    }

    /// <summary>
    /// The method <c>ValidateSecret</c> to validate secret of the user.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="secret"></param>
    /// <param name="iterations"></param>
    /// <returns></returns>
    public bool ValidateSecret(Guid id, string secret, int iterations)
    {
        _logger.LogInfo("Validating the user secret");
        UserSecret user = _repoWrapper.UserSecret.FindByCondition(x => x.IsActive && x.UserId.Equals(id)).FirstOrDefault();
        if (user != null)
        {
            if (PasswordHash.VerifyPasswordHash(secret, user.Secret, iterations))
            {
                return true;
            }
            else
            {
                _logger.LogWarn("Invalid credentials");
                return false;
            }
        }
        else
        {
            _logger.LogWarn("Password mismatch");
            return false;
        }
    }

    /// <summary>
    /// The method <c>GenerateAccessToken</c> to generate access token.
    /// </summary>
    /// <param name="claims"></param>
    /// <returns></returns>
    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        _logger.LogDebug("Generate Access token");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[Common.TOKEN_KEY]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: _configuration[Common.TOKEN_ISSUER],
            audience: _configuration[Common.TOKEN_ISSUER],
            claims: claims,
            notBefore: DateTime.UtcNow,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt); //the method is called WriteToken but returns a string
    }

    /// <summary>
    /// The method <c>CreateUserSession</c> to create session for logged  user.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>The id of the session</returns>
    public Guid CreateUserSession(Guid userId)
    {
        _logger.LogInfo($"Creating UserSession for the User Id: {userId}");

        var _userSession = new UserSession
        {
            UserId = userId,
            LoginTime = DateTime.UtcNow
        };

        _repoWrapper.UserSession.Create(_userSession);
        _repoWrapper.Save();

        return _userSession.Id;
    }

    /// <summary>
    /// Method to create a user.
    /// </summary>
    /// <param name="userDto">The user details dto which contains information of user</param>
    /// <returns name= "userId">Id of created user</returns>
    public ResponseDto CreateUser(UserDto userDto)
    {
        _logger.LogDebug("Create user for given details");
        if (IsUserExists(userDto.Email))
        {
            _logger.LogError("User for the email already exists");
            throw new ConflictCustomException("User already exists", "user already exists");
        }

        // Create the user entry
        User user = new User()
        {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email,
            PhoneNumber = userDto.PhoneNumber,
            Role = (Entities.Enums.Role)userDto.Role
        };
        _repoWrapper.User.Create(user);

        // Create the default user secret
        var iterations = Convert.ToInt32(_configuration["Authentication:Iterations"]);
        var passwordhash = PasswordHash.GetPasswordHash(Common.DEFAULT_PASSWORD, iterations);
        UserSecret userSecret = new UserSecret()
        {
            UserId = user.Id,
            Secret = passwordhash
        };
        _repoWrapper.UserSecret.Create(userSecret);

        _repoWrapper.Save();

        ResponseDto response = new ResponseDto();
        response.Id = user.Id;
        return response;
    }

    /// <summary>
    /// Check if user already exists for given email
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <returns>
    /// True or false
    /// </returns>
    public bool IsUserExists(string email)
    {
        _logger.LogInfo("Checking if user already exists");
        User user = _repoWrapper.User.FindFirstByCondition(user => user.Email.Equals(email) && user.IsActive);
        if (user != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Method to get user details based on Id
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <returns>
    /// UserDto object containing the user details.
    /// </returns>
    public UserDto GetUser(Guid userId)
    {
        User user = _repoWrapper.User.FindFirstByCondition((user) => user.Id.Equals(userId) && user.IsActive);
        if (user == null)
        {
            _logger.LogWarn($"User for id {userId} not found");
            throw new NotFoundCustomException("User not found", "User not found for given Id");
        }
        return new UserDto()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = (Role)user.Role
        };
    }

    /// <summary>
    /// Method to reset the user password.
    /// </summary>
    /// <param name="resetPasswordDto">The resetPasswordDto object with the user reset details</param>
    /// <param name="loggedInUserId">The logged in user Id</param>
    public void ResetUserPassword(ResetPasswordDto resetPasswordDto, Guid loggedInUserId)
    {
        User user = _repoWrapper.User.FindFirstByCondition((user) => user.Id.Equals(loggedInUserId) && user.IsActive);
        if (user == null || resetPasswordDto.Email != user.Email)
        {
            _logger.LogWarn("User not found");
            throw new NotFoundCustomException("User not found", "The email you entered doesn't exist");
        }
        UserSecret userSecret = _repoWrapper.UserSecret.FindFirstByCondition((us) => us.UserId.Equals(user.Id));
        var iterations = Convert.ToInt32(_configuration["Authentication:Iterations"]);
        if (ValidateSecret(user.Id, resetPasswordDto.CurrentPassword, iterations))
        {
            var newPasswordHash = PasswordHash.GetPasswordHash(resetPasswordDto.NewPassword, iterations);
            userSecret.Secret = newPasswordHash;
            _repoWrapper.UserSecret.Update(userSecret);
            _repoWrapper.Save();
        }
        else
        {
            throw new ForBiddenCustomException("The current password is incorrect", "The current password you entered is incorrect");
        }
    }

    /// <summary>
    /// Method to update user details.
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <param name="userDto">The user details dto which contains information of user</param>
    public void UpdateUserDetails(Guid userId, UserDto userDto)
    {
        User user = _repoWrapper.User.FindFirstByCondition((user) => user.IsActive && user.Id.Equals(userId));
        if (user == null)
        {
            throw new NotFoundCustomException("The user not found.", "The user not found for given id");
        }

        var emailExistsInDb = _repoWrapper.User.FindFirstByCondition((user) => user.IsActive && user.Email.Equals(userDto.Email) && user.Id != userId);
        if (emailExistsInDb != null)
        {
            throw new ConflictCustomException("The email id already exists", "The email id already exists. Please provide a different id");
        }

        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.Email = userDto.Email;
        user.PhoneNumber = userDto.PhoneNumber;
        user.Role = (Entities.Enums.Role)userDto.Role;
        _repoWrapper.User.Update(user);
        _repoWrapper.Save();
    }

    /// <summary>
    /// Method to delete user details
    /// </summary>
    /// <param name="userId">The id of the user</param>
    public void DeleteUser(Guid userId)
    {
        User user = _repoWrapper.User.FindFirstByCondition(user => user.IsActive && user.Id.Equals(userId));
        if (user == null)
        {
            _logger.LogWarn($"User for id {userId} not found");
            throw new NotFoundCustomException("User not found", "User not found for given Id");
        }

        // Remove user
        user.IsActive = false;
        _repoWrapper.User.Update(user);

        // Remove user sessions
        List<UserSession> userSessions = _repoWrapper.UserSession.FindByCondition((session) => session.UserId.Equals(userId) && session.IsActive).ToList();
        if (userSessions.Count > 0)
        {
            userSessions.ForEach(session =>
            {
                session.LogoutTime = DateTime.UtcNow;
                session.IsActive = false;
            });
            _repoWrapper.UserSession.UpdateBatch(userSessions);
        }

        // Remove user secret
        UserSecret userSecret = _repoWrapper.UserSecret.FindFirstByCondition((s) => s.UserId.Equals(userId) && s.IsActive);
        if (userSecret != null)
        {
            userSecret.IsActive = false;
            _repoWrapper.UserSecret.Update(userSecret);
        }

        _repoWrapper.Save();
    }
}