using System.Net;
using System.Runtime.Serialization;

namespace ExceptionHandler
{
    /// <summary>
    /// Class <c>NotFoundCustomException</c>.
    /// </summary>
    [Serializable]
    public sealed class NotFoundCustomException : BaseCustomException
    {
        /// <summary>
        /// NotFoundCustomException class base constructor.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private NotFoundCustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            // ...
        }

        /// <summary>
        /// NotFoundCustomException class parameterised constructor.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="description"></param>
        public NotFoundCustomException(string message, string description) : base(message, description, (int)HttpStatusCode.NotFound)
        {
        }
    }
}