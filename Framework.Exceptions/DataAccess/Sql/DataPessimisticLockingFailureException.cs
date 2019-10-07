using System;

namespace Framework.Exceptions.DataAccess.Sql
{
    /// <summary>
    ///     The data pessimistic locking failure exception.
    /// </summary>
    [Serializable]
    public class DataPessimisticLockingFailureException : DataLockingFailureException
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataPessimisticLockingFailureException" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="rootCause">
        ///     The root cause.
        /// </param>
        public DataPessimisticLockingFailureException(string message, Exception rootCause)
            : base(message, rootCause)
        {
        }

        #endregion
    }
}