using System;

namespace Framework.Exceptions.DataAccess.Sql
{
    /// <summary>
    ///     The data bad sql exception.
    /// </summary>
    [Serializable]
    public class DataBadSqlException : DataAccessException
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataBadSqlException" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="rootCause">
        ///     The root cause.
        /// </param>
        public DataBadSqlException(string message, Exception rootCause)
            : base(message, rootCause)
        {
        }

        #endregion
    }
}
