using System;
using System.Runtime.Serialization;

namespace Framework.Exceptions
{
    public class ProjectNameException : Exception
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProjectNameException" /> class.
        /// </summary>
        public ProjectNameException()
        {

        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProjectNameException" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        public ProjectNameException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProjectNameException" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="inner">
        ///     The root cause.
        /// </param>
        public ProjectNameException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProjectNameException" /> class.
        /// </summary>
        /// <param name="info">
        ///     The serialization information.
        /// </param>
        /// <param name="context">
        ///     The context.
        /// </param>
        public ProjectNameException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
