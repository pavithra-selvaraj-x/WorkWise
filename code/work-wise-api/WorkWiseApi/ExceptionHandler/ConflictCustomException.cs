using System.Net;
using System.Runtime.Serialization;

namespace ExceptionHandler
{
    /// <summary>
    /// Class <c>ConflictCustomException</c>.
    /// </summary>
    [Serializable]
    public sealed class ConflictCustomException : BaseCustomException
    {
        /// <summary>
        /// ConflictCustomException class base constructor.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private ConflictCustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            // ...
        }
        /// <summary>
        /// ConflictCustomException class parameterised constructor.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="description"></param>
        public ConflictCustomException(string message, string description) : base(message, description, (int)HttpStatusCode.Conflict)
        {
        }
    }
}