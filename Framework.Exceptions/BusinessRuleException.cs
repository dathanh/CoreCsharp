using Framework.DomainModel.Entities.Common;
using System;
using System.Collections.Generic;

namespace Framework.Exceptions
{
    [Serializable]
    public class BusinessRuleException : UserVisibleException
    {
        private readonly ICollection<BusinessRuleResult> _mFailedRules;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessRuleException"/> class.
        /// </summary>
        /// <param name="messageResourceKey"></param>
        /// <param name="failedRules">A set of <see cref="BusinessRuleResult"/>
        /// to transport information what has been evaluated as a business rule
        /// violation, and on which object.</param>
        public BusinessRuleException(string messageResourceKey, BusinessRuleResult[] failedRules)
            : base(messageResourceKey)
        {
            if (failedRules == null)
            {
                _mFailedRules = new BusinessRuleResult[0];
            }
            else
            {
                _mFailedRules = failedRules;
            }
        }

        /// <summary>
        /// Gets a collection of business rule result that have causes the exception to be 
        /// thrown. 
        /// </summary>       
        /// <value>A set of <see cref="BusinessRuleResult"/>
        /// to transport information what has been evaluated as a business rule
        /// violation, and on which object.</value>
        public ICollection<BusinessRuleResult> FailedRules => _mFailedRules;
    }
}
