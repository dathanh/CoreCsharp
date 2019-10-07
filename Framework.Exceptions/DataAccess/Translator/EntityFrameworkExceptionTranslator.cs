using Framework.Exceptions.DataAccess.Meta;
using System;

namespace Framework.Exceptions.DataAccess.Translator
{
    public class EntityFrameworkExceptionTranslator : ExceptionTranslator
    {
        public EntityFrameworkExceptionTranslator(IDbMetaInfo meta)
            : base(meta)
        {
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
        public override Exception TranslateException(Exception ex)
        {
            /*
            //could not connect to database
            if (ex is ArgumentException) 
            {
            }*/

            return base.TranslateException(ex);
        }
    }
}