using System;

namespace Framework.Exceptions.DataAccess.Sql
{
    /// <summary>
    ///     The data uncategorized exception.
    /// </summary>
    [Serializable]
    public class DataUncategorizedException : DataAccessException
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataUncategorizedException" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="rootCause">
        ///     The root cause.
        /// </param>
        public DataUncategorizedException(string message, Exception rootCause)
            : base(message, rootCause)
        {
        }

        #endregion
    }
}