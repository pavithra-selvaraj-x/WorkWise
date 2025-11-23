using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Entities.Dtos
{
    /// <summary>
    /// LoginDto model used to bind the login credentials for API request.
    /// </summary>
    [DataContract]
    public partial class LoginDto
    {
        /// <summary>
        /// The username of the user to authenticate.
        /// </summary>
        /// <value>The username of the user to authenticate.</value>
        [Required]
        [DataMember(Name = "user_name")]
        public string UserName { get; set; }

        /// <summary>
        /// The password of the user to authenticate.
        /// </summary>
        /// <value>The password of the user to authenticate.</value>
        [Required]
        [DataMember(Name = "password")]
        public string Password { get; set; }

    }
}
