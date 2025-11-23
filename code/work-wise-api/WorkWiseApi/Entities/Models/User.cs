using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Enums;

namespace Entities.Models
{
    /// <summary>
    /// Class <c>User</c> contains the user details.
    /// </summary>
    [Table("users")]
    public class User : BaseModel
    {
        /// <summary>
        /// The first name of the user.
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user.
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// The email of the user.
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// The phone number of the user.
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The role of the user.
        /// </summary>
        [EnumDataType(typeof(Role))]
        public Role Role { get; set; }
    }
}
