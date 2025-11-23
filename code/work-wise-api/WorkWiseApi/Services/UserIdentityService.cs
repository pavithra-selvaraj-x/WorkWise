using Contracts;
using Entities.Common;
using ExceptionHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace Services
{
    /// <summary>
    /// 
    /// </summary>
    public class UserIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ILoggerManager _logger;
        /// <summary>
        /// Constructor for injecting the logger, httpContextAccessor and configuration
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public UserIdentityService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ILoggerManager logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// The method <c>GetUserIdentity</c> to get user Identity from the jwt token.
        /// </summary>      
        /// <returns>Unique Id of the user</returns>
        public Guid GetUserIdentity()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                return GetUserIdFromJwtToken(_httpContextAccessor.HttpContext.Request.Headers["Authorization"]);
            }
            return Guid.Empty;
        }

        /// <summary>
        /// The method <c>GetUserIdFromJwtToken</c> to get User ID from the jwt token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public System.Guid GetUserIdFromJwtToken(string token)
        {
            if (token != null)
            {
                token = token.Replace("Bearer ", string.Empty);
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[Common.TOKEN_KEY])),
                    ValidateLifetime = false
                };
                var tokenHandler = new JwtSecurityTokenHandler();

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new UnAuthorizedCustomException("Invalid Token", "The token is either not valid or expired");
                var identity = principal.Identities.FirstOrDefault();
                var _personId = System.Guid.Parse(identity.FindFirst("UserId").Value);
                return _personId;
            }
            else
            {
                return Guid.Parse("d0e96ca8-7a2f-41d0-84b8-0c0298208def");
            }
        }
    }
}