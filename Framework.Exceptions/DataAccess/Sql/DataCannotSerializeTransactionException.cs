using System;

namespace Framework.Exceptions.DataAccess.Sql
{
    [Serializable]
    public class DataCannotSerializeTransactionException : DataAccessException
    {
        public DataCannotSerializeTransactionException(string message, Exception rootCause)
            : base(message, rootCause)
        {
        }
    }
}