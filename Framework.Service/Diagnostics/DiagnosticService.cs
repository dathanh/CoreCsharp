using NLog;
using System;

namespace Framework.Service.Diagnostics
{
    public class DiagnosticService : IDiagnosticService
    {
        private readonly Logger _logger = LogManager.GetLogger("IDiagnosticService");

        #region Trace

        /// <summary>
        ///     Log a message object with the Trace level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        public void Trace(object message)
        {
            if (IsTraceEnabled)
            {
                WriteInternal(LogLevel.Trace, message, null);
            }
        }

        /// <summary>
        ///     Log a message object with the Trace level including
        ///     the stack trace of the <see cref="Exception" /> passed
        ///     as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        public void Trace(object message, Exception exception)
        {
            if (IsTraceEnabled)
            {
                WriteInternal(LogLevel.Trace, message, exception);
            }
        }

        /// <summary>
        ///     Log a message with the Trace level.
        /// </summary>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="args"></param>
        public void TraceFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsTraceEnabled)
            {
                WriteInternal(LogLevel.Trace, new StringFormatFormattedMessage(formatProvider, format, args), null);
            }
        }

        /// <summary>
        ///     Log a message with the Trace level.
        /// </summary>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        public void TraceFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsTraceEnabled)
            {
                WriteInternal(LogLevel.Trace, new StringFormatFormattedMessage(formatProvider, format, args), exception);
            }
        }

        /// <summary>
        ///     Log a message with the Trace level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="args">the list of format arguments</param>
        public void TraceFormat(string format, params object[] args)
        {
            if (IsTraceEnabled)
            {
                WriteInternal(LogLevel.Trace, new StringFormatFormattedMessage(null, format, args), null);
            }
        }

        /// <summary>
        ///     Log a message with the Trace level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        public virtual void TraceFormat(string format, Exception exception, params object[] args)
        {
            if (IsTraceEnabled)
            {
                WriteInternal(LogLevel.Trace, new StringFormatFormattedMessage(null, format, args), exception);
            }
        }

        /// <summary>
        ///     Log a message with the Trace level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Trace(Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsTraceEnabled)
            {
                WriteInternal(LogLevel.Trace, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
            }
        }

        /// <summary>
        ///     Log a message with the Trace level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        public void Trace(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            if (IsTraceEnabled)
            {
                WriteInternal(LogLevel.Trace, new FormatMessageCallbackFormattedMessage(formatMessageCallback),
                    exception);
            }
        }

        /// <summary>
        ///     Log a message with the Trace level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Trace(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsTraceEnabled)
            {
                WriteInternal(LogLevel.Trace,
                    new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
            }
        }

        /// <summary>
        ///     Log a message with the Trace level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        public void Trace(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback,
            Exception exception)
        {
            if (IsTraceEnabled)
            {
                WriteInternal(LogLevel.Trace,
                    new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback),
                    exception);
            }
        }

        #endregion

        #region Debug

        /// <summary>
        ///     Log a message object with the Debug level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        public void Debug(object message)
        {
            if (IsDebugEnabled)
            {
                WriteInternal(LogLevel.Debug, message, null);
            }
        }

        /// <summary>
        ///     Log a message object with the Debug level including
        ///     the stack Debug of the <see cref="Exception" /> passed
        ///     as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack Debug.</param>
        public void Debug(object message, Exception exception)
        {
            if (IsDebugEnabled)
            {
                WriteInternal(LogLevel.Debug, message, exception);
            }
        }

        /// <summary>
        ///     Log a message with the Debug level.
        /// </summary>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="args"></param>
        public void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsDebugEnabled)
            {
                WriteInternal(LogLevel.Debug, new StringFormatFormattedMessage(formatProvider, format, args), null);
            }
        }

        /// <summary>
        ///     Log a message with the Debug level.
        /// </summary>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        public void DebugFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsDebugEnabled)
            {
                WriteInternal(LogLevel.Debug, new StringFormatFormattedMessage(formatProvider, format, args), exception);
            }
        }

        /// <summary>
        ///     Log a message with the Debug level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="args">the list of format arguments</param>
        public void DebugFormat(string format, params object[] args)
        {
            if (IsDebugEnabled)
            {
                WriteInternal(LogLevel.Debug, new StringFormatFormattedMessage(null, format, args), null);
            }
        }

        /// <summary>
        ///     Log a message with the Debug level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        public void DebugFormat(string format, Exception exception, params object[] args)
        {
            if (IsDebugEnabled)
            {
                WriteInternal(LogLevel.Debug, new StringFormatFormattedMessage(null, format, args), exception);
            }
        }

        /// <summary>
        ///     Log a message with the Debug level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Debug(Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsDebugEnabled)
            {
                WriteInternal(LogLevel.Debug, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
            }
        }

        /// <summary>
        ///     Log a message with the Debug level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Debug.</param>
        public void Debug(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            if (IsDebugEnabled)
            {
                WriteInternal(LogLevel.Debug, new FormatMessageCallbackFormattedMessage(formatMessageCallback),
                    exception);
            }
        }

        /// <summary>
        ///     Log a message with the Debug level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Debug(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsDebugEnabled)
            {
                WriteInternal(LogLevel.Debug,
                    new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
            }
        }

        /// <summary>
        ///     Log a message with the Debug level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Debug.</param>
        public void Debug(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback,
            Exception exception)
        {
            if (IsDebugEnabled)
            {
                WriteInternal(LogLevel.Debug,
                    new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback),
                    exception);
            }
        }

        #endregion

        #region Info

        /// <summary>
        ///     Log a message object with the Info level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        public void Info(object message)
        {
            if (IsInfoEnabled)
            {
                WriteInternal(LogLevel.Info, message, null);
            }
        }

        /// <summary>
        ///     Log a message object with the Info level including
        ///     the stack Info of the <see cref="Exception" /> passed
        ///     as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack Info.</param>
        public void Info(object message, Exception exception)
        {
            if (IsInfoEnabled)
            {
                WriteInternal(LogLevel.Info, message, exception);
            }
        }

        /// <summary>
        ///     Log a message with the Info level.
        /// </summary>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="args"></param>
        public void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsInfoEnabled)
            {
                WriteInternal(LogLevel.Info, new StringFormatFormattedMessage(formatProvider, format, args), null);
            }
        }

        /// <summary>
        ///     Log a message with the Info level.
        /// </summary>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        public void InfoFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsInfoEnabled)
            {
                WriteInternal(LogLevel.Info, new StringFormatFormattedMessage(formatProvider, format, args), exception);
            }
        }

        /// <summary>
        ///     Log a message with the Info level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="args">the list of format arguments</param>
        public void InfoFormat(string format, params object[] args)
        {
            if (IsInfoEnabled)
            {
                WriteInternal(LogLevel.Info, new StringFormatFormattedMessage(null, format, args), null);
            }
        }

        /// <summary>
        ///     Log a message with the Info level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        public void InfoFormat(string format, Exception exception, params object[] args)
        {
            if (IsInfoEnabled)
            {
                WriteInternal(LogLevel.Info, new StringFormatFormattedMessage(null, format, args), exception);
            }
        }

        /// <summary>
        ///     Log a message with the Info level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Info(Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsInfoEnabled)
            {
                WriteInternal(LogLevel.Info, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
            }
        }

        /// <summary>
        ///     Log a message with the Info level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Info.</param>
        public void Info(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            if (IsInfoEnabled)
            {
                WriteInternal(LogLevel.Info, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
            }
        }

        /// <summary>
        ///     Log a message with the Info level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Info(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsInfoEnabled)
            {
                WriteInternal(LogLevel.Info,
                    new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
            }
        }

        /// <summary>
        ///     Log a message with the Info level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Info.</param>
        public void Info(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback,
            Exception exception)
        {
            if (IsInfoEnabled)
            {
                WriteInternal(LogLevel.Info,
                    new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback),
                    exception);
            }
        }

        #endregion

        #region Warn

        /// <summary>
        ///     Log a message object with the Warn level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        public void Warn(object message)
        {
            if (IsWarnEnabled)
            {
                WriteInternal(LogLevel.Warn, message, null);
            }
        }

        /// <summary>
        ///     Log a message object with the Warn level including
        ///     the stack Warn of the <see cref="Exception" /> passed
        ///     as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack Warn.</param>
        public void Warn(object message, Exception exception)
        {
            if (IsWarnEnabled)
            {
                WriteInternal(LogLevel.Warn, message, exception);
            }
        }

        /// <summary>
        ///     Log a message with the Warn level.
        /// </summary>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting Warn information.
        /// </param>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="args"></param>
        public void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsWarnEnabled)
            {
                WriteInternal(LogLevel.Warn, new StringFormatFormattedMessage(formatProvider, format, args), null);
            }
        }

        /// <summary>
        ///     Log a message with the Warn level.
        /// </summary>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting Warn information.
        /// </param>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        public void WarnFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsWarnEnabled)
            {
                WriteInternal(LogLevel.Warn, new StringFormatFormattedMessage(formatProvider, format, args), exception);
            }
        }

        /// <summary>
        ///     Log a message with the Warn level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="args">the list of format arguments</param>
        public void WarnFormat(string format, params object[] args)
        {
            if (IsWarnEnabled)
            {
                WriteInternal(LogLevel.Warn, new StringFormatFormattedMessage(null, format, args), null);
            }
        }

        /// <summary>
        ///     Log a message with the Warn level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        public void WarnFormat(string format, Exception exception, params object[] args)
        {
            if (IsWarnEnabled)
            {
                WriteInternal(LogLevel.Warn, new StringFormatFormattedMessage(null, format, args), exception);
            }
        }

        /// <summary>
        ///     Log a message with the Warn level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Warn(Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsWarnEnabled)
            {
                WriteInternal(LogLevel.Warn, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
            }
        }

        /// <summary>
        ///     Log a message with the Warn level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Warn.</param>
        public void Warn(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            if (IsWarnEnabled)
            {
                WriteInternal(LogLevel.Warn, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
            }
        }

        /// <summary>
        ///     Log a message with the Warn level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Warn(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsWarnEnabled)
            {
                WriteInternal(LogLevel.Warn,
                    new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
            }
        }

        /// <summary>
        ///     Log a message with the Warn level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Warn.</param>
        public void Warn(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback,
            Exception exception)
        {
            if (IsWarnEnabled)
            {
                WriteInternal(LogLevel.Warn,
                    new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback),
                    exception);
            }
        }

        #endregion

        #region Error

        /// <summary>
        ///     Log a message object with the Error level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        public void Error(object message)
        {
            if (IsErrorEnabled)
            {
                WriteInternal(LogLevel.Error, message, null);
            }
        }

        /// <summary>
        ///     Log a message object with the Error level including
        ///     the stack Error of the <see cref="Exception" /> passed
        ///     as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack Error.</param>
        public void Error(object message, Exception exception)
        {
            if (IsErrorEnabled)
            {
                WriteInternal(LogLevel.Error, message, exception);
            }
        }

        /// <summary>
        ///     Log a message with the Error level.
        /// </summary>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting Error information.
        /// </param>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="args"></param>
        public void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsErrorEnabled)
            {
                WriteInternal(LogLevel.Error, new StringFormatFormattedMessage(formatProvider, format, args), null);
            }
        }

        /// <summary>
        ///     Log a message with the Error level.
        /// </summary>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting Error information.
        /// </param>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        public void ErrorFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsErrorEnabled)
            {
                WriteInternal(LogLevel.Error, new StringFormatFormattedMessage(formatProvider, format, args), exception);
            }
        }

        /// <summary>
        ///     Log a message with the Error level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="args">the list of format arguments</param>
        public void ErrorFormat(string format, params object[] args)
        {
            if (IsErrorEnabled)
            {
                WriteInternal(LogLevel.Error, new StringFormatFormattedMessage(null, format, args), null);
            }
        }

        /// <summary>
        ///     Log a message with the Error level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        public void ErrorFormat(string format, Exception exception, params object[] args)
        {
            if (IsErrorEnabled)
            {
                WriteInternal(LogLevel.Error, new StringFormatFormattedMessage(null, format, args), exception);
            }
        }

        /// <summary>
        ///     Log a message with the Error level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Error(Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsErrorEnabled)
            {
                WriteInternal(LogLevel.Error, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
            }
        }

        /// <summary>
        ///     Log a message with the Error level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Error.</param>
        public void Error(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            if (IsErrorEnabled)
            {
                WriteInternal(LogLevel.Error, new FormatMessageCallbackFormattedMessage(formatMessageCallback),
                    exception);
            }
        }

        /// <summary>
        ///     Log a message with the Error level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Error(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsErrorEnabled)
            {
                WriteInternal(LogLevel.Error,
                    new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
            }
        }

        /// <summary>
        ///     Log a message with the Error level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Error.</param>
        public void Error(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback,
            Exception exception)
        {
            if (IsErrorEnabled)
            {
                WriteInternal(LogLevel.Error,
                    new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback),
                    exception);
            }
        }

        #endregion

        #region Fatal

        /// <summary>
        ///     Log a message object with the Fatal level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        public void Fatal(object message)
        {
            if (IsFatalEnabled)
            {
                WriteInternal(LogLevel.Fatal, message, null);
            }
        }

        /// <summary>
        ///     Log a message object with the Fatal level including
        ///     the stack Fatal of the <see cref="Exception" /> passed
        ///     as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack Fatal.</param>
        public void Fatal(object message, Exception exception)
        {
            if (IsFatalEnabled)
            {
                WriteInternal(LogLevel.Fatal, message, exception);
            }
        }

        /// <summary>
        ///     Log a message with the Fatal level.
        /// </summary>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting Fatal information.
        /// </param>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="args"></param>
        public void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsFatalEnabled)
            {
                WriteInternal(LogLevel.Fatal, new StringFormatFormattedMessage(formatProvider, format, args), null);
            }
        }

        /// <summary>
        ///     Log a message with the Fatal level.
        /// </summary>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting Fatal information.
        /// </param>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        public void FatalFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsFatalEnabled)
            {
                WriteInternal(LogLevel.Fatal, new StringFormatFormattedMessage(formatProvider, format, args), exception);
            }
        }

        /// <summary>
        ///     Log a message with the Fatal level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="args">the list of format arguments</param>
        public void FatalFormat(string format, params object[] args)
        {
            if (IsFatalEnabled)
            {
                WriteInternal(LogLevel.Fatal, new StringFormatFormattedMessage(null, format, args), null);
            }
        }

        /// <summary>
        ///     Log a message with the Fatal level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        public void FatalFormat(string format, Exception exception, params object[] args)
        {
            if (IsFatalEnabled)
            {
                WriteInternal(LogLevel.Fatal, new StringFormatFormattedMessage(null, format, args), exception);
            }
        }

        /// <summary>
        ///     Log a message with the Fatal level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Fatal(Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsFatalEnabled)
            {
                WriteInternal(LogLevel.Fatal, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
            }
        }

        /// <summary>
        ///     Log a message with the Fatal level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Fatal.</param>
        public void Fatal(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            if (IsFatalEnabled)
            {
                WriteInternal(LogLevel.Fatal,
                    new FormatMessageCallbackFormattedMessage(formatMessageCallback),
                    exception);
            }
        }

        /// <summary>
        ///     Log a message with the Fatal level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Fatal(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsFatalEnabled)
            {
                WriteInternal(LogLevel.Fatal,
                    new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback),
                    null);
            }
        }

        /// <summary>
        ///     Log a message with the Fatal level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Fatal.</param>
        public void Fatal(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback,
            Exception exception)
        {
            if (IsFatalEnabled)
            {
                WriteInternal(LogLevel.Fatal,
                    new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback),
                    exception);
            }
        }

        #endregion

        /// <summary>
        ///     Gets a value indicating whether this instance is trace enabled.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is trace enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsTraceEnabled => _logger.IsTraceEnabled;

        /// <summary>
        ///     Gets a value indicating whether this instance is debug enabled.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is debug enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsDebugEnabled => _logger.IsDebugEnabled;

        /// <summary>
        ///     Gets a value indicating whether this instance is info enabled.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is info enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsInfoEnabled => _logger.IsInfoEnabled;


        /// <summary>
        ///     Gets a value indicating whether this instance is warn enabled.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is warn enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsWarnEnabled => _logger.IsWarnEnabled;

        /// <summary>
        ///     Gets a value indicating whether this instance is error enabled.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is error enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsErrorEnabled => _logger.IsErrorEnabled;

        /// <summary>
        ///     Gets a value indicating whether this instance is fatal enabled.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is fatal enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsFatalEnabled => _logger.IsFatalEnabled;

        /// <summary>
        ///     Actually sends the message to the underlying log system.
        /// </summary>
        /// <param name="logLevel">the level of this log event.</param>
        /// <param name="message">the message to log</param>
        /// <param name="exception">the exception to log (may be null)</param>
        private void WriteInternal(LogLevel logLevel, object message, Exception exception)
        {
            var logEvent = new LogEventInfo(logLevel, _logger.Name, null, "{0}", new[] { message }, exception);

            _logger.Log(logEvent);
        }
    }
}
