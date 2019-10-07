using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace Framework.DomainModel.ValueObject
{
    public class FeedbackViewModel
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        public FeedbackViewModel()
        {
            Status = FeedbackStatus.Success; //default value
            Error = string.Empty;
            StackTrace = string.Empty;
            ModelStateErrors = new List<string>();
        }

        /// <summary>
        ///     This is values such as "success", error", "critical"
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public FeedbackStatus Status { get; set; }

        /// <summary>
        ///     Primary Message that gets displayed
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        ///     Secondary message which gets displayed, should contain the debug info.
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        ///InnerException string.
        /// </summary>
        public string InnerException { get; set; }

        /// <summary>
        ///     Contains an array of validation errors on the current form.
        /// </summary>
        public IList<string> ModelStateErrors { get; set; }

        /// <summary>
        ///     Contains return data from server.
        /// </summary>
        public Object Data { get; set; }

        /// <summary>
        ///     Add a list of errors to model state
        /// </summary>
        /// <param name="error"></param>
        public void AddModelStateError(string error)
        {
            ModelStateErrors.Add(error);
        }

        /// <summary>
        ///     Add errors to model state
        /// </summary>
        /// <param name="errors"></param>
        public void AddModelStateErrors(params string[] errors)
        {
            foreach (var error in errors)
            {
                ModelStateErrors.Add(error);
            }
        }

        internal void AddModelStateErrors(IEnumerable<string> modelStateErrors)
        {
            foreach (var error in modelStateErrors)
            {
                ModelStateErrors.Add(error);
            }
        }
    }
}