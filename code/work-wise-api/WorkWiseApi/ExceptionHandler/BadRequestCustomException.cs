using System.Net;
using System.Runtime.Serialization;

namespace ExceptionHandler
{
    /// <summary>
    /// Class <c>BadRequestCustomException</c>.
    /// </summary>
    [Serializable]
    public sealed class BadRequestCustomException : BaseCustomException
    {
        /// <summary>
        /// BadRequestCustomException class base constructor.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private BadRequestCustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            // ...
        }

        /// <summary>
        /// BadRequestCustomException class parameterised constructor.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="description"></param>
        public BadRequestCustomException(string message, string description) : base(message, description, (int)HttpStatusCode.BadRequest)
        {

        }
    }
}