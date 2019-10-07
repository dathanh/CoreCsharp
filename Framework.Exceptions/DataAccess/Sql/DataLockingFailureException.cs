using System;

namespace Framework.Exceptions.DataAccess.Sql
{
    /// <summary>
    ///     The data locking failure exception.
    /// </summary>
    [Serializable]
    public class DataLockingFailureException : DataAccessException
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataLockingFailureException" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="rootCause">
        ///     The root cause.
        /// </param>
        public DataLockingFailureException(string message, Exception rootCause)
            : base(message, rootCause)
        {
        }

        #endregion
    }
}