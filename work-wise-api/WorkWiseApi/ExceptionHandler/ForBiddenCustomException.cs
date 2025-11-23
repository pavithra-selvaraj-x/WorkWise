using System.Net;
using System.Runtime.Serialization;

namespace ExceptionHandler
{
    /// <summary>
    /// Class <c>ForBiddenCustomException</c>.
    /// </summary>
    [Serializable]
    public sealed class ForBiddenCustomException : BaseCustomException
    {
        /// <summary>
        /// ForBiddenCustomException class base constructor.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private ForBiddenCustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            // ...
        }

        /// <summary>
        /// ForBiddenCustomException class parameterised constructor.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="description"></param>
        public ForBiddenCustomException(string message, string description) : base(message, description, (int)HttpStatusCode.Forbidden)
        {
        }

    }
}