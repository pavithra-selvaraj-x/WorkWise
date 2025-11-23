using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Entities.Enums;
using Newtonsoft.Json;

namespace Entities.Dtos
{
    /// <summary>
    /// The UserDto used to bind details for the user to be created.
    /// </summary>
    [DataContract]
    public partial class UserDto
    {
        /// <summary>
        /// The id of the user.
        /// </summary>
        /// <value>The id of the user.</value>
        [DataMember(Name = "id")]
        public Guid? Id { get; set; }

        /// <summary>
        /// The first name of the user.
        /// </summary>
        /// <value>The first name of the user.</value>
        [Required]
        [DataMember(Name = "first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user.
        /// </summary>
        /// <value>The last name of the user.</value>
        [Required]
        [DataMember(Name = "last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// The email of the user.
        /// </summary>
        /// <value>The email of the user.</value>
        [Required]
        [DataMember(Name = "email")]
        public string Email { get; set; }

        /// <summary>
        /// The phone number of the user.
        /// </summary>
        /// <value>The phone number of the user.</value>
        [DataMember(Name = "phone_number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or Sets Role
        /// </summary>
        [Required]
        [DataMember(Name = "role")]
        public Role Role { get; set; }
    }
}
