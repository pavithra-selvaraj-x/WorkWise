using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Entities.Dtos
{
    /// <summary>
    /// The ResponseDto object used to bind the response details for API.
    /// </summary>
    [DataContract]
    public partial class ResponseDto
    {
        /// <summary>
        /// The unique identifier of the entity.
        /// </summary>
        /// <value>The unique identifier of the entity.</value>
        [DataMember(Name = "id")]
        public Guid? Id { get; set; }

    }
}
