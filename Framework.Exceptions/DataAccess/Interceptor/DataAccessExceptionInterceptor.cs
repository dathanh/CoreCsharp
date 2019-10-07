using Castle.DynamicProxy;
using Framework.Service.Diagnostics;
using System;

namespace Framework.Exceptions.DataAccess.Interceptor
{
    public class DataAccessExceptionInterceptor : IDataAccessExceptionInterceptor
    {
        private readonly IDiagnosticService _diagnosticService;

        public DataAccessExceptionInterceptor(IDiagnosticService diagnosticService)
        {
            _diagnosticService = diagnosticService;
        }

        /// <summary>
        ///     Execute and translate exception to database exception
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                _diagnosticService.Error(ex);
                throw;
            }
        }
    }
}
