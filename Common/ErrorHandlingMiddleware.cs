using Framework.DomainModel.ValueObject;
using Framework.Exceptions;
using Framework.Exceptions.DataAccess.Sql;
using Framework.Service.Diagnostics;
using Framework.Service.Translation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Common
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private static IHostingEnvironment _hostingEnvironment;
        private static IDiagnosticService _diagnosticService;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IHostingEnvironment hostingEnvironment, IDiagnosticService diagnosticService)
        {

            _hostingEnvironment = hostingEnvironment;
            _diagnosticService = diagnosticService;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode code;
            var errors = new List<string>();

            if (exception is BusinessRuleException)
            {
                var failResult = ((BusinessRuleException)exception).FailedRules.FirstOrDefault();
                if (failResult != null)
                {
                    errors = failResult.ValidationResults.Select(o => o.ErrorMessage).ToList();
                }
                code = HttpStatusCode.OK;
            }
            else if (exception is UnAuthorizedAccessException)
            {
                code = HttpStatusCode.Unauthorized;
            }
            else
            {
                errors.Add(exception.Message);
                code = HttpStatusCode.OK;
            }

            var jsonResult = JsonConvert.SerializeObject(GetJsonError(exception, errors));
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(jsonResult);
        }

        private static FeedbackViewModel GetJsonError(Exception ex, IEnumerable<string> errors)
        {
            var modelStateErrors = errors as IList<string> ?? errors.ToList();
            return BuildFeedBackViewModel(ex, modelStateErrors);
        }

        private static FeedbackViewModel BuildFeedBackViewModel(Exception ex, IEnumerable<string> modelStateErrors)
        {
            var feedback = new FeedbackViewModel();
            var shouldRethrow = HandleException(ex, out var exceptionHandlingResult);
            feedback.Status = shouldRethrow ? FeedbackStatus.Critical : FeedbackStatus.Error;
            feedback.Error = exceptionHandlingResult.ErrorMessage;

            if (ex is BusinessRuleException)
            {
                feedback.AddModelStateErrors(modelStateErrors.ToArray());
                //add more exception from exception stack trace
                feedback.AddModelStateErrors(exceptionHandlingResult.ModelStateErrors.ToArray());
            }

            feedback.InnerException = exceptionHandlingResult.InnerException;
            return feedback;
        }

        private static bool HandleException(Exception ex, out ExceptionHandlingResult exceptionHandlingResult)
        {
            exceptionHandlingResult = new ExceptionHandlingResult();
            bool shouldRethrow;
            _diagnosticService.Error(ex);
            _diagnosticService.Error(ex.StackTrace);
            var isProductionMode = _hostingEnvironment.IsProduction();
            var commonErrorMessage = SystemMessageLookup.GetMessage("GeneralExceptionMessageText");

            switch (ex)
            {
                //  if production mode then show generic error
                case BusinessRuleException _:
                    //all business exception be showed to client
                    exceptionHandlingResult.ErrorMessage = ex.Message;
                    exceptionHandlingResult.StackTrace = ex.StackTrace;
                    break;
                case InvalidClaimsException _:
                case UserVisibleException _:
                    exceptionHandlingResult.ErrorMessage = ex.Message;
                    exceptionHandlingResult.StackTrace = string.Empty;
                    break;
                default:
                    {
                        if (isProductionMode)
                        {
                            exceptionHandlingResult.ErrorMessage = commonErrorMessage;
                            exceptionHandlingResult.StackTrace = string.Empty;
                        }
                        else //  else: show all exception
                        {
                            exceptionHandlingResult.ErrorMessage = ex.Message;
                            exceptionHandlingResult.StackTrace = ex.StackTrace;
                        }

                        break;
                    }
            }

            var innerError = ex.InnerException;

            if (innerError != null && !isProductionMode)
            {
                if (ex is BusinessRuleException)
                {
                    exceptionHandlingResult.AddModelStateErrors(innerError.Message);
                }

                //get innerException
                if (innerError.InnerException != null)
                {
                    exceptionHandlingResult.AddInnerException(innerError.InnerException.Message);
                    if (innerError.InnerException.InnerException != null)
                    {
                        exceptionHandlingResult.AddInnerException(innerError.InnerException.InnerException.Message);
                    }
                }
            }

            switch (ex)
            {
                case UserVisibleException _:
                    //exception has been transformed
                    exceptionHandlingResult.ErrorMessage = ex.Message;

                    shouldRethrow = true;
                    break;
                case DataPermissionDeniedException _:
                case DataAccessException _:
                    shouldRethrow = true;
                    break;
                default:
                    shouldRethrow = true;
                    break;
            }

            return shouldRethrow;
        }
    }
}
