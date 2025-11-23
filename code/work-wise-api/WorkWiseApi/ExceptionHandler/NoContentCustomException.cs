using System.Net;
using System.Runtime.Serialization;

namespace ExceptionHandler
{
    /// <summary>
    /// Class <c>NoContentCustomException</c>.
    /// </summary>
    [Serializable]
    public sealed class NoContentCustomException : BaseCustomException
    {
        /// <summary>
        /// NoContentCustomException class base constructor.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private NoContentCustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            // ...
        }

        /// <summary>
        /// NoContentCustomException class parameterised constructor.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="description"></param>
        public NoContentCustomException(string message, string description) : base(message, description, (int)HttpStatusCode.NoContent)
        {

        }
    }
}