using Framework.Service.Translation;
using System;
using System.Runtime.Serialization;

namespace Framework.Exceptions
{

    [Serializable]
    public class UserVisibleException : DataAccessException
    {
        public UserVisibleException()
        {
        }

        public UserVisibleException(string messageResourceKey)
            : base(SystemMessageLookup.GetMessage(messageResourceKey))
        {
            MessageResourceKey = messageResourceKey;
        }

        public UserVisibleException(string messageResourceKey, Exception inner)
            : base(SystemMessageLookup.GetMessage(messageResourceKey), inner)
        {
        }

        protected UserVisibleException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        public string MessageResourceKey { get; set; }
    }
}
