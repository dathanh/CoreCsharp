using Framework.DomainModel;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using System;
using System.Collections.Generic;

namespace Framework.BusinessRule
{
    /// <summary>
    /// Interface for objects that organize a set of business rules.
    /// </summary>
    public interface IBusinessRuleSet<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// List of rules that are invoked before a domain object is persisited to 
        /// the database. If one of the business rules evaluates with an "Error" result,
        /// the transaction cannot be completed.
        /// </summary>
        IList<IBusinessRule<TEntity>> Rules
        {
            get;
        }

        /// <summary>
        /// Executes the rules defined in a on a domain object.
        /// </summary>
        /// <param name="instance">The domain object instance.</param>
        /// <param name="filter">A filter for rules that are run. The filter is applied 
        /// before the rules are run.</param>
        /// <returns>A list of <see cref="BusinessRuleResult"/> objects; one for each business 
        /// rule that has been run on the object.</returns>
        IEnumerable<BusinessRuleResult> ExecuteRules(
            IEntity instance,
            Func<IBusinessRule<TEntity>, bool> filter);
    }
}
