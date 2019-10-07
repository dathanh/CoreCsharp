using Framework.Service.Translation;
using System;
using System.Runtime.Serialization;

namespace Framework.Exceptions
{
    [Serializable]
    public class UnAuthorizedAccessException : ProjectNameException
    {
        public UnAuthorizedAccessException()
        {
        }

        public UnAuthorizedAccessException(string messageResourceKey)
            : base(SystemMessageLookup.GetMessage(messageResourceKey))
        {
        }

        public UnAuthorizedAccessException(string messageResourceKey, Exception inner)
            : base(SystemMessageLookup.GetMessage(messageResourceKey), inner)
        {
        }

        protected UnAuthorizedAccessException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
