using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    /// <summary>
    /// Class <c>UserSecret</c> contains the user secret details.
    /// </summary>
    [Table("user_secret")]
    public class UserSecret : BaseModel
    {
        /// <summary>
        /// The unique id of the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The secret of the user.
        /// </summary>
        public required string Secret { get; set; }
    }
}