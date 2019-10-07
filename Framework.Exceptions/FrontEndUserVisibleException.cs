using System;
using System.Runtime.Serialization;

namespace Framework.Exceptions
{
    [Serializable]
    public class FrontEndUserVisibleException : DataAccessException
    {
        public FrontEndUserVisibleException()
        {
        }

        public FrontEndUserVisibleException(string messageResourceKey)
            : base(messageResourceKey)
        {
            MessageResourceKey = messageResourceKey;
        }

        public FrontEndUserVisibleException(string messageResourceKey, Exception inner)
            : base(messageResourceKey, inner)
        {
        }

        protected FrontEndUserVisibleException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        public string MessageResourceKey { get; set; }
    }
}
