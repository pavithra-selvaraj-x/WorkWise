using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Entities.Dtos
{
    /// <summary>
    /// The TokenDto used to bind the authenticated user&#x27;s JWT and Refresh token.
    /// </summary>
    [DataContract]
    public partial class TokenDto
    {
        /// <summary>
        /// The token is a JWT token contains the authenticated user info.
        /// </summary>
        /// <value>The token is a JWT token contains the authenticated user info.</value>
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// The type of the token to authenticate the user.
        /// </summary>
        /// <value>The type of the token to authenticate the user.</value>
        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }

    }
}
