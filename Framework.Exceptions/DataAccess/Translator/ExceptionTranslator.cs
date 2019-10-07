using Framework.Exceptions.DataAccess.Meta;
using Framework.Exceptions.DataAccess.Sql;
using System;
using System.Data.Common;

namespace Framework.Exceptions.DataAccess.Translator
{
    /// <summary>
    ///     The exception translator.
    /// </summary>
    public class ExceptionTranslator : IExceptionTranslator
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExceptionTranslator" /> class.
        /// </summary>
        /// <param name="meta">
        ///     The database metadata information
        /// </param>
        public ExceptionTranslator(IDbMetaInfo meta)
        {
            Meta = meta;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets database metadata.
        /// </summary>
        protected IDbMetaInfo Meta { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Check whether should handle exception.
        /// </summary>
        /// <param name="ex">
        ///     The exception to translate.
        /// </param>
        /// <returns>
        ///     The should handle exception.
        /// </returns>
        public virtual bool ShouldHandleException(Exception ex)
        {
            return true;
        }

        /// <summary>
        ///     Translate an exception.
        /// </summary>
        /// <param name="ex">
        ///     The exception to translate.
        /// </param>
        /// <returns>
        ///     The translated exception.
        /// </returns>
        public virtual Exception TranslateException(Exception ex)
        {
            var databaseException = ex as DbException;

            if (databaseException == null)
            {
                return ex;
                //return new DataUncategorizedException(ex.Message, ex);
            }

            var errorCodes = Meta.ErrorCodes;
            if (errorCodes == null)
            {
                return new DataUncategorizedException(ex.Message, ex);
            }

            var errorCode = Meta.ParseErrorCode(ex);

            if (Array.IndexOf(errorCodes.BadSqlGrammarCodes, errorCode) >= 0)
            {
                return new DataBadSqlException(ex.Message, ex);
            }

            if (Array.IndexOf(errorCodes.DuplicateKeyCodes, errorCode) >= 0)
            {
                return new DataDupplicateKeyException(ex.Message, ex) { ConstraintId = Meta.ParseConstraintId(ex) };
            }

            if (Array.IndexOf(errorCodes.PermissionDeniedCodes, errorCode) >= 0)
            {
                return new DataPermissionDeniedException(ex.Message, ex);
            }

            if (Array.IndexOf(errorCodes.DataIntegrityViolationCodes, errorCode) >= 0)
            {
                return new DataDupplicateKeyException(ex.Message, ex) { ConstraintId = Meta.ParseConstraintId(ex) };
            }

            if (Array.IndexOf(errorCodes.CannotAcquireLockCodes, errorCode) >= 0)
            {
                return new DataPermissionDeniedException(ex.Message, ex);
            }

            if (Array.IndexOf(errorCodes.DeadlockLoserCodes, errorCode) >= 0)
            {
                return new DataDeadlockException(ex.Message, ex);
            }

            if (Array.IndexOf(errorCodes.CannotSerializeTransactionCodes, errorCode) >= 0)
            {
                return new DataCannotSerializeTransactionException(ex.Message, ex);
            }

            return new DataUncategorizedException(ex.Message, ex);
        }

        #endregion
    }
}