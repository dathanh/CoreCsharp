using System;

namespace Framework.Exceptions
{
    [Serializable]
    public class BusinessLogicException : StarBerryException
    {
        public BusinessLogicException(string message, Exception rootCause)
            : base(message, rootCause)
        {
        }
    }
}
