using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Framework.DomainModel.Entities.Common
{
    // Summary:
    //     Transports the result of a business rule execution. It provides information
    //     about the business rule in a form that the user can understand, and the domain
    //     object that has been evaluated. In order to keep the serialization footprint
    //     low, only the type and Id values are stored.
    //  
    public class BusinessRuleResult
    {
        public BusinessRuleResult()
        {
            ValidationResults = new List<ValidationResult>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessRuleResult"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public BusinessRuleResult(
          string message)
        {
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessRuleResult"/> class.
        /// </summary>
        /// <param name="isFailed">if set to <c>true</c>, the business rule has failed.</param>
        /// <param name="message">The message.</param>
        /// <param name="instanceTypeName">Type of the business object that has been evaluated.</param>
        /// <param name="instanceId">Id of the business object that has been evaluated.</param>
        /// <param name="propertyNames">Names of the properties that has been evaluated by the business rule.</param>
        /// <param name="businessRuleName">Name of the business rule.</param>
        public BusinessRuleResult(
          bool isFailed,
          string message,
          string instanceTypeName,
          int instanceId,
          string[] propertyNames,
          string businessRuleName
          )
        {
            IsFailed = isFailed;
            Message = message;
            InstanceTypeName = instanceTypeName;
            InstanceId = instanceId;
            PropertyNames = propertyNames;
            BusinessRuleName = businessRuleName;
        }


        /// <summary>
        /// Gets or sets the name of the business rule.
        /// </summary>
        /// <value>The name of the business rule.</value>        
        public string BusinessRuleName
        {
            get;
            set;
        }


        /// <summary>
        /// Gets a translated message for the user.
        /// </summary>
        /// <value>The message.</value>        
        public string Message
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the business rule has failed.
        /// </summary>
        /// <value><c>true</c> if this instance is failed; otherwise, <c>false</c>.</value>        
        public bool IsFailed
        {
            get;
            set;
        }

        /// <summary>
        /// Type of the domain object that the business rule belongs to.
        /// </summary>
        /// <value>Name of a type.</value>        
        public string InstanceTypeName
        {
            get;
            set;
        }

        public int InstanceId
        {
            get;
            set;
        }

        public string[] PropertyNames
        {
            get;
            set;
        }

        public List<ValidationResult> ValidationResults { get; set; }

    }
}
