using System;
using FormatMessageCallback = System.Action<Framework.Service.Diagnostics.FormatMessageHandler>;

namespace Framework.Service.Diagnostics
{
    public interface IDiagnosticService
    {
        /// <summary>
        ///     Checks if this logger is enabled for the Trace level.
        /// </summary>
        bool IsTraceEnabled { get; }

        /// <summary>
        ///     Checks if this logger is enabled for the Debug level.
        /// </summary>
        bool IsDebugEnabled { get; }

        /// <summary>
        ///     Checks if this logger is enabled for the Error level.
        /// </summary>
        bool IsErrorEnabled { get; }

        /// <summary>
        ///     Checks if this logger is enabled for the Fatal level.
        /// </summary>
        bool IsFatalEnabled { get; }

        /// <summary>
        ///     Checks if this logger is enabled for the Info level.
        /// </summary>
        bool IsInfoEnabled { get; }

        /// <summary>
        ///     Checks if this logger is enabled for the Warn level.
        /// </summary>
        bool IsWarnEnabled { get; }

        /// <summary>
        ///     Log a message object with the Trace level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Trace(object message);

        /// <summary>
        ///     Log a message object with the Trace level including
        ///     the stack trace of the <see cref="Exception" /> passed
        ///     as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Trace(object message, Exception exception);

        /// <summary>
        ///     Log a message with the Trace level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="args">the list of format arguments</param>
        void TraceFormat(string format, params object[] args);

        /// <summary>
        ///     Log a message with the Trace level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        void TraceFormat(string format, Exception exception, params object[] args);

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
        void TraceFormat(IFormatProvider formatProvider, string format, params object[] args);

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
        void TraceFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args);

        /// <summary>
        ///     Log a message with the Trace level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        void Trace(FormatMessageCallback formatMessageCallback);

        /// <summary>
        ///     Log a message with the Trace level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Trace(FormatMessageCallback formatMessageCallback, Exception exception);

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
        void Trace(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback);

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
        void Trace(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception);

        /// <summary>
        ///     Log a message object with the Debug level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Debug(object message);

        /// <summary>
        ///     Log a message object with the Debug level including
        ///     the stack trace of the <see cref="Exception" /> passed
        ///     as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Debug(object message, Exception exception);

        /// <summary>
        ///     Log a message with the Debug level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="args">the list of format arguments</param>
        void DebugFormat(string format, params object[] args);

        /// <summary>
        ///     Log a message with the Debug level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        void DebugFormat(string format, Exception exception, params object[] args);

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
        void DebugFormat(IFormatProvider formatProvider, string format, params object[] args);

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
        void DebugFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args);

        /// <summary>
        ///     Log a message with the Debug level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        void Debug(FormatMessageCallback formatMessageCallback);

        /// <summary>
        ///     Log a message with the Debug level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Debug(FormatMessageCallback formatMessageCallback, Exception exception);

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
        void Debug(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback);

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
        void Debug(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception);

        /// <summary>
        ///     Log a message object with the Info level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Info(object message);

        /// <summary>
        ///     Log a message object with the Info level including
        ///     the stack trace of the <see cref="Exception" /> passed
        ///     as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Info(object message, Exception exception);

        /// <summary>
        ///     Log a message with the Info level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="args">the list of format arguments</param>
        void InfoFormat(string format, params object[] args);

        /// <summary>
        ///     Log a message with the Info level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        void InfoFormat(string format, Exception exception, params object[] args);

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
        void InfoFormat(IFormatProvider formatProvider, string format, params object[] args);

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
        void InfoFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args);

        /// <summary>
        ///     Log a message with the Info level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        void Info(FormatMessageCallback formatMessageCallback);

        /// <summary>
        ///     Log a message with the Info level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Info(FormatMessageCallback formatMessageCallback, Exception exception);

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
        void Info(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback);

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
        void Info(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception);

        /// <summary>
        ///     Log a message object with the Warn level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Warn(object message);

        /// <summary>
        ///     Log a message object with the Warn level including
        ///     the stack trace of the <see cref="Exception" /> passed
        ///     as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Warn(object message, Exception exception);

        /// <summary>
        ///     Log a message with the Warn level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="args">the list of format arguments</param>
        void WarnFormat(string format, params object[] args);

        /// <summary>
        ///     Log a message with the Warn level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        void WarnFormat(string format, Exception exception, params object[] args);

        /// <summary>
        ///     Log a message with the Warn level.
        /// </summary>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="args"></param>
        void WarnFormat(IFormatProvider formatProvider, string format, params object[] args);

        /// <summary>
        ///     Log a message with the Warn level.
        /// </summary>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        void WarnFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args);

        /// <summary>
        ///     Log a message with the Warn level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        void Warn(FormatMessageCallback formatMessageCallback);

        /// <summary>
        ///     Log a message with the Warn level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Warn(FormatMessageCallback formatMessageCallback, Exception exception);

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
        void Warn(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback);

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
        void Warn(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception);

        /// <summary>
        ///     Log a message object with the Error level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Error(object message);

        /// <summary>
        ///     Log a message object with the Error level including
        ///     the stack trace of the <see cref="Exception" /> passed
        ///     as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Error(object message, Exception exception);

        /// <summary>
        ///     Log a message with the Error level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="args">the list of format arguments</param>
        void ErrorFormat(string format, params object[] args);

        /// <summary>
        ///     Log a message with the Error level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        void ErrorFormat(string format, Exception exception, params object[] args);

        /// <summary>
        ///     Log a message with the Error level.
        /// </summary>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="args"></param>
        void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args);

        /// <summary>
        ///     Log a message with the Error level.
        /// </summary>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        void ErrorFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args);

        /// <summary>
        ///     Log a message with the Error level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        void Error(FormatMessageCallback formatMessageCallback);

        /// <summary>
        ///     Log a message with the Error level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Error(FormatMessageCallback formatMessageCallback, Exception exception);

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
        void Error(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback);

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
        void Error(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception);

        /// <summary>
        ///     Log a message object with the Fatal level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Fatal(object message);

        /// <summary>
        ///     Log a message object with the Fatal level including
        ///     the stack trace of the <see cref="Exception" /> passed
        ///     as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Fatal(object message, Exception exception);

        /// <summary>
        ///     Log a message with the Fatal level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="args">the list of format arguments</param>
        void FatalFormat(string format, params object[] args);

        /// <summary>
        ///     Log a message with the Fatal level.
        /// </summary>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        void FatalFormat(string format, Exception exception, params object[] args);

        /// <summary>
        ///     Log a message with the Fatal level.
        /// </summary>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="args"></param>
        void FatalFormat(IFormatProvider formatProvider, string format, params object[] args);

        /// <summary>
        ///     Log a message with the Fatal level.
        /// </summary>
        /// <param name="formatProvider">
        ///     An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.
        /// </param>
        /// <param name="format">
        ///     The format of the message object to log.<see cref="string.Format(string,object[])" />
        /// </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        void FatalFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args);

        /// <summary>
        ///     Log a message with the Fatal level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        void Fatal(FormatMessageCallback formatMessageCallback);

        /// <summary>
        ///     Log a message with the Fatal level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        ///     Using this method avoids the cost of creating a message and evaluating message arguments
        ///     that probably won't be logged due to log level settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Fatal(FormatMessageCallback formatMessageCallback, Exception exception);

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
        void Fatal(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback);

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
        void Fatal(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception);
    }
}
