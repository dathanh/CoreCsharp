using System;

namespace Framework.Exceptions.DataAccess.Meta
{
    /// <summary>
    ///     Metadata for database
    /// </summary>
    public interface IDbMetaInfo
    {
        /// <summary>
        ///     Gets error codes category.
        /// </summary>
        DbErrorCodes ErrorCodes { get; }

        /// <summary>
        ///     Parse exception for constraint id.
        /// </summary>
        /// <param name="ex">
        ///     The exception to parse.
        /// </param>
        /// <returns>
        ///     The parsed constraint id.
        /// </returns>
        string ParseConstraintId(Exception ex);

        /// <summary>
        ///     Parse exception for error code.
        /// </summary>
        /// <param name="ex">
        ///     The exception to parse.
        /// </param>
        /// <returns>
        ///     The parsed error code.
        /// </returns>
        string ParseErrorCode(Exception ex);
    }
}
