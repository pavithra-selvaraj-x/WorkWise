using System.Net;
using System.Runtime.Serialization;

namespace ExceptionHandler
{
    /// <summary>
    /// Class <c>UnAuthorizedCustomException</c>.
    /// </summary>
    [Serializable]
    public sealed class UnAuthorizedCustomException : BaseCustomException
    {
        /// <summary>
        /// UnAuthorizedCustomException class base constructor.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private UnAuthorizedCustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            // ...
        }

        /// <summary>
        /// UnAuthorizedCustomException class parameterised constructor.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="description"></param>
        public UnAuthorizedCustomException(string message, string description) : base(message, description, (int)HttpStatusCode.Unauthorized)
        {
        }
    }
}