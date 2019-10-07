namespace Framework.Exceptions
{
    /// <summary>
    ///     The database error codes.
    /// </summary>
    public class DbErrorCodes
    {
        #region Constants and Fields

        /// <summary>
        ///     The empty array.
        /// </summary>
        private static readonly string[] EmptyArray = new string[] { };

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DbErrorCodes" /> class.
        /// </summary>
        public DbErrorCodes()
        {
            BadSqlGrammarCodes = EmptyArray;
            DataIntegrityViolationCodes = EmptyArray;
            CannotAcquireLockCodes = EmptyArray;
            DeadlockLoserCodes = EmptyArray;
            DuplicateKeyCodes = EmptyArray;
            CannotSerializeTransactionCodes = EmptyArray;
            PermissionDeniedCodes = EmptyArray;
            DataAccessFailureCodes = EmptyArray;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets BadSqlGrammarCodes.
        /// </summary>
        public string[] BadSqlGrammarCodes { get; set; }

        /// <summary>
        ///     Gets or sets CannotAcquireLockCodes.
        /// </summary>
        public string[] CannotAcquireLockCodes { get; set; }

        /// <summary>
        ///     Gets or sets CannotSerializeTransactionCodes.
        /// </summary>
        public string[] CannotSerializeTransactionCodes { get; set; }

        /// <summary>
        ///     Gets or sets DataAccessFailureCodes.
        /// </summary>
        public string[] DataAccessFailureCodes { get; set; }

        /// <summary>
        ///     Gets or sets DataIntegrityViolationCodes.
        /// </summary>
        public string[] DataIntegrityViolationCodes { get; set; }

        /// <summary>
        ///     Gets or sets DeadlockLoserCodes.
        /// </summary>
        public string[] DeadlockLoserCodes { get; set; }

        /// <summary>
        ///     Gets or sets DuplicateKeyCodes.
        /// </summary>
        public string[] DuplicateKeyCodes { get; set; }

        /// <summary>
        ///     Gets or sets PermissionDeniedCodes.
        /// </summary>
        public string[] PermissionDeniedCodes { get; set; }

        #endregion
    }
}