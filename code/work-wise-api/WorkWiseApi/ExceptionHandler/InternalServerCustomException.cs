using System.Net;
using System.Runtime.Serialization;

namespace ExceptionHandler
{
    /// <summary>
    /// Class <c>InternalServerCustomException</c>.
    /// </summary>
    [Serializable]
    public sealed class InternalServerCustomException : BaseCustomException
    {
        /// <summary>
        /// InternalServerCustomException class base constructor.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private InternalServerCustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            // ...
        }

        /// <summary>
        /// InternalServerCustomException class parameterised constructor.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="description"></param>
        public InternalServerCustomException(string message, string description) : base(message, description, (int)HttpStatusCode.InternalServerError)
        {

        }
    }
}