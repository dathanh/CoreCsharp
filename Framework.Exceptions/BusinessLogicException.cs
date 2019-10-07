using System;

namespace Framework.Exceptions
{
    [Serializable]
    public class BusinessLogicException : ProjectNameException
    {
        public BusinessLogicException(string message, Exception rootCause)
            : base(message, rootCause)
        {
        }
    }
}
