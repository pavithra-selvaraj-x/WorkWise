using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    /// <summary>
    /// Class <c>UserSession</c> contains the user session details.
    /// </summary>
    [Table("user_session")]
    public class UserSession : BaseModel
    {
        /// <summary>
        /// The unique id of the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The login time of the user.
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// The logout time of the user.
        /// </summary>
        public DateTime LogoutTime { get; set; }
    }
}