using System;
using System.Runtime.Serialization;

namespace Framework.Exceptions.DataAccess.Sql
{
    /// <summary>
    ///     The data integrity violation exception.
    /// </summary>
    [Serializable]
    public class DataIntegrityViolationException : DataAccessException
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataIntegrityViolationException" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="rootCause">
        ///     The root cause.
        /// </param>
        public DataIntegrityViolationException(string message, Exception rootCause)
            : base(message, rootCause)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets ConstraintId.
        /// </summary>
        public string ConstraintId { get; set; }


        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        #endregion
    }
}