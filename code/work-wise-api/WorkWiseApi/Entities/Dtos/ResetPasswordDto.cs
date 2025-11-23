using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Entities.Dtos
{
    /// <summary>
    /// The ResetPasswordDto used to bind the reset password data.
    /// </summary>
    [DataContract]
    public partial class ResetPasswordDto
    {
        /// <summary>
        /// The email of the user.
        /// </summary>
        /// <value>The email of the user.</value>
        [Required]
        [DataMember(Name = "email")]
        public string Email { get; set; }

        /// <summary>
        /// The current password of the user.
        /// </summary>
        /// <value>The current password of the user.</value>
        [Required]
        [DataMember(Name = "current_password")]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// The new password of the user.
        /// </summary>
        /// <value>The new password of the user.</value>
        [Required]
        [DataMember(Name = "new_password")]
        public string NewPassword { get; set; }

    }
}
