using System;
using System.Data.Common;

namespace Framework.Exceptions.DataAccess.Meta
{
    /// <summary>
    ///     Database metadata for SQLServer.
    /// </summary>
    public class SqlServerDbMetaInfo : IDbMetaInfo
    {
        #region Constants and Fields

        /// <summary>
        ///     The exception type name.
        /// </summary>
        private const string ExceptionTypeName = "System.Data.SqlClient.SqlException";

        /// <summary>
        ///     The _error codes.
        /// </summary>
        private readonly DbErrorCodes _errorCodes;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlServerDbMetaInfo" /> class.
        /// </summary>
        public SqlServerDbMetaInfo()
        {
            _errorCodes = new DbErrorCodes
            {
                BadSqlGrammarCodes = new[] { "156", "170", "207", "208" },
                PermissionDeniedCodes = new[] { "229" },
                DataIntegrityViolationCodes = new[] { "544", "2627", "8114", "8115" },
                DeadlockLoserCodes = new[] { "1205" }
            };
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets ErrorCodes.
        /// </summary>
        public DbErrorCodes ErrorCodes => _errorCodes;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Parse constraint id from exception.
        /// </summary>
        /// <param name="ex">
        ///     The exception to parse.
        /// </param>
        /// <returns>
        ///     The parsed constraint id.
        /// </returns>
        public string ParseConstraintId(Exception ex)
        {
            var databaseException = ex as DbException;
            if (databaseException == null)
            {
                return string.Empty;
            }

            var msg = ex.Message;
            if (string.IsNullOrWhiteSpace(msg) || !msg.StartsWith("The"))
            {
                return string.Empty;
            }

            var startIdx = msg.IndexOf('"');
            if (startIdx < 0)
            {
                return string.Empty;
            }

            var endIdx = msg.IndexOf('"', startIdx + 1);
            if (endIdx < 0)
            {
                return string.Empty;
            }

            return msg.Substring(startIdx + 1, endIdx - startIdx);
        }

        /// <summary>
        ///     Parse error code from exception
        /// </summary>
        /// <param name="ex">
        ///     The exception to parse error code from.
        /// </param>
        /// <returns>
        ///     The parsed error code.
        /// </returns>
        public string ParseErrorCode(Exception ex)
        {
            if (IsSqlException(ex))
            {
                dynamic sqlException = ex;
                return sqlException.Errors[0].Number.ToString();
            }

            return string.Empty;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Check whether the exception is SQL exception
        /// </summary>
        /// <param name="ex">
        ///     The exception to check.
        /// </param>
        /// <returns>
        ///     <value>true</value>
        ///     if it is Sql Exception.
        /// </returns>
        private bool IsSqlException(Exception ex)
        {
            var databaseException = ex as DbException;
            if (databaseException == null)
            {
                return false;
            }

            var fullName = ex.GetType().FullName;
            return fullName.Equals(ExceptionTypeName);
        }

        #endregion
    }
}