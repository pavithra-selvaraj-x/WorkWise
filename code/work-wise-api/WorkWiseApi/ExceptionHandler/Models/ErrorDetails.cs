namespace ExceptionHandler.Models
{
    /// <summary>
    /// Class <c>ErrorDetails</c> contains the details of any error.
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// The status code of the error.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// The error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The description of the error message.
        /// </summary>
        public string Description { get; set; }

    }
}