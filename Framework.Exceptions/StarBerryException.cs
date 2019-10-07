using System;
using System.Runtime.Serialization;

namespace Framework.Exceptions
{
    public class StarBerryException : Exception
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="StarBerryException" /> class.
        /// </summary>
        public StarBerryException()
        {

        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StarBerryException" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        public StarBerryException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StarBerryException" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="inner">
        ///     The root cause.
        /// </param>
        public StarBerryException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StarBerryException" /> class.
        /// </summary>
        /// <param name="info">
        ///     The serialization information.
        /// </param>
        /// <param name="context">
        ///     The context.
        /// </param>
        public StarBerryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
