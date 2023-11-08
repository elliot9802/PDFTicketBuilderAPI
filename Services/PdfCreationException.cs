namespace Services
{
    /// <summary>
    /// Represents errors that occur during PDF conversion.
    /// </summary>
    public class PdfCreationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PdfCreationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that caused the current exception.</param>
        public PdfCreationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

}
