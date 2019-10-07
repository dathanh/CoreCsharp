using System;

namespace Framework.Service.Diagnostics
{
    public class StringFormatFormattedMessage
    {
        private readonly object[] _args;
        private readonly IFormatProvider _formatProvider;
        private readonly string _message;
        private volatile string _cachedMessage;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StringFormatFormattedMessage" /> class.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The args.</param>
        public StringFormatFormattedMessage(IFormatProvider formatProvider, string message, params object[] args)
        {
            _formatProvider = formatProvider;
            _message = message;
            _args = args;
        }

        /// <summary>
        ///     Runs <see cref="string.Format(System.IFormatProvider,string,object[])" /> on supplied arguemnts.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (_cachedMessage == null && _message != null)
            {
                _cachedMessage = string.Format(_formatProvider, _message, _args);
            }
            return _cachedMessage ?? "";
        }
    }
}
