using System;
using FormatMessageCallback = System.Action<Framework.Service.Diagnostics.FormatMessageHandler>;
namespace Framework.Service.Diagnostics
{
    public class FormatMessageCallbackFormattedMessage
    {
        private readonly FormatMessageCallback _formatMessageCallback;
        private readonly IFormatProvider _formatProvider;
        private volatile string _cachedMessage;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FormatMessageCallbackFormattedMessage" /> class.
        /// </summary>
        /// <param name="formatMessageCallback">The format message callback.</param>
        public FormatMessageCallbackFormattedMessage(FormatMessageCallback formatMessageCallback)
        {
            _formatMessageCallback = formatMessageCallback;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FormatMessageCallbackFormattedMessage" /> class.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="formatMessageCallback">The format message callback.</param>
        public FormatMessageCallbackFormattedMessage(IFormatProvider formatProvider,
                                                     FormatMessageCallback formatMessageCallback)
        {
            _formatProvider = formatProvider;
            _formatMessageCallback = formatMessageCallback;
        }

        /// <summary>
        ///     Calls <see cref="_formatMessageCallback" /> and returns result.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_cachedMessage == null && _formatMessageCallback != null)
            {
                _formatMessageCallback(FormatMessage);
            }
            return _cachedMessage ?? "";
        }

        private string FormatMessage(string format, params object[] args)
        {
            _cachedMessage = string.Format(_formatProvider, format, args);
            return _cachedMessage;
        }
    }
}
