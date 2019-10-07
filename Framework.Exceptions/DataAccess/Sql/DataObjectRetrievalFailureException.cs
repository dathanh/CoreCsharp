using System;

namespace Framework.Exceptions.DataAccess.Sql
{
    /// <summary>
    ///     The data object retrieval failure exception.
    /// </summary>
    [Serializable]
    public class DataObjectRetrievalFailureException : DataAccessException
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataObjectRetrievalFailureException" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="rootCause">
        ///     The root cause.
        /// </param>
        public DataObjectRetrievalFailureException(string message, Exception rootCause)
            : base(message, rootCause)
        {
        }

        #endregion
    }
}