using System.Runtime.Serialization;

namespace ExceptionHandler
{
    /// <summary>
    /// Class <c>BaseCustomException</c>.
    /// </summary>
    [Serializable]
    public class BaseCustomException : Exception
    {
        /// <summary>
        /// The status code of the error.
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// The description of the error.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// BaseCustomException class base constructor.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected BaseCustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        /// <summary>
        /// BaseCustomException class parameterised constructor.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="description"></param>
        /// <param name="code"></param>
        public BaseCustomException(string message, string description, int code) : base(message)
        {
            Code = code;
            Description = description;
        }
    }
}