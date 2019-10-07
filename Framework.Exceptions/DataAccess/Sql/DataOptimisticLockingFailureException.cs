using System;

namespace Framework.Exceptions.DataAccess.Sql
{
    /// <summary>
    ///     The data optimistic locking failure exception.
    /// </summary>
    [Serializable]
    public class DataOptimisticLockingFailureException : DataLockingFailureException
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataOptimisticLockingFailureException" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="rootCause">
        ///     The root cause.
        /// </param>
        public DataOptimisticLockingFailureException(string message, Exception rootCause)
            : base(message, rootCause)
        {
        }

        #endregion
    }
}