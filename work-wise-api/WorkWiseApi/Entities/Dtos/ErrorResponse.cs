using System.Runtime.Serialization;

namespace Entities.Dtos
{
    /// <summary>
    /// ErrorResponse model used to bind the error message for API response.
    /// </summary>
    [DataContract]
    public partial class ErrorResponse
    {
        /// <summary>
        /// An error response status code for an operation.
        /// </summary>
        /// <value>An error response status code for an operation.</value>
        [DataMember(Name = "status_code")]
        public int? StatusCode { get; set; }

        /// <summary>
        /// An error response message for an operation.
        /// </summary>
        /// <value>An error response message for an operation.</value>
        [DataMember(Name = "message")]
        public string? Message { get; set; }

        /// <summary>
        /// The detailed error response for an operation.
        /// </summary>
        /// <value>The detailed error response for an operation.</value>
        [DataMember(Name = "description")]
        public string? Description { get; set; }

    }
}
