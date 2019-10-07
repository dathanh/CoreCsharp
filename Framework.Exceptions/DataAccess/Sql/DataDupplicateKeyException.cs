using System;

namespace Framework.Exceptions.DataAccess.Sql
{
    /// <summary>
    ///     The data duplicate key exception.
    /// </summary>
    [Serializable]
    public class DataDupplicateKeyException : DataIntegrityViolationException
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataDupplicateKeyException" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="rootCause">
        ///     The root cause.
        /// </param>
        public DataDupplicateKeyException(string message, Exception rootCause)
            : base(message, rootCause)
        {
        }

        #endregion
    }
}