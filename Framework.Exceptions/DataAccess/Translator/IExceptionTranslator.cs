using System;

namespace Framework.Exceptions.DataAccess.Translator
{
    /// <summary>
    ///     Exception translator for all Entity Framework exception
    /// </summary>
    public interface IExceptionTranslator
    {
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
        bool ShouldHandleException(Exception ex);

        /// <summary>
        ///     Translate an exception.
        /// </summary>
        /// <param name="ex">
        ///     The exception to translate.
        /// </param>
        /// <returns>
        ///     The translated exception.
        /// </returns>
        Exception TranslateException(Exception ex);

        #endregion
    }
}
