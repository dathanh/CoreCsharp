using System;

namespace Framework.Exceptions.DataAccess.Sql
{
    /// <summary>
    ///     The data permission denied exception.
    /// </summary>
    [Serializable]
    public class DataPermissionDeniedException : DataAccessException
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataPermissionDeniedException" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="rootCause">
        ///     The root cause.
        /// </param>
        public DataPermissionDeniedException(string message, Exception rootCause)
            : base(message, rootCause)
        {
        }

        #endregion
    }
}