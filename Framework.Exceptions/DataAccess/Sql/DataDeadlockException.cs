using System;

namespace Framework.Exceptions.DataAccess.Sql
{
    [Serializable]
    public class DataDeadlockException : DataAccessException
    {
        public DataDeadlockException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}