using System;

namespace Framework.Exceptions.DataAccess.Meta
{
    /// <summary>
    ///     The generic database meta info.
    /// </summary>
    public class GenericDbMetaInfo : IDbMetaInfo
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericDbMetaInfo" /> class.
        /// </summary>
        public GenericDbMetaInfo()
        {
            ErrorCodes = new DbErrorCodes();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets error codes category.
        /// </summary>
        public DbErrorCodes ErrorCodes { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Parse exception for constraint id.
        /// </summary>
        /// <param name="ex">
        ///     The exception to parse.
        /// </param>
        /// <returns>
        ///     The parsed constraint id.
        /// </returns>
        public string ParseConstraintId(Exception ex)
        {
            return string.Empty;
        }

        /// <summary>
        ///     Parse exception for error code.
        /// </summary>
        /// <param name="ex">
        ///     The exception to parse.
        /// </param>
        /// <returns>
        ///     The parsed error code.
        /// </returns>
        public string ParseErrorCode(Exception ex)
        {
            return string.Empty;
        }

        #endregion
    }
}