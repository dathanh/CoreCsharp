using Framework.DomainModel;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.Service.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.BusinessRule
{
    /// <summary>
    /// Organizes a set of business rules that apply to a one type of 
    /// domain object.
    /// </summary>
    public class BusinessRuleSet<TEntity> : IBusinessRuleSet<TEntity> where TEntity : Entity
    {
        private readonly IList<IBusinessRule<TEntity>> _mRules = new List<IBusinessRule<TEntity>>();


        /// <summary>
        /// Log message that indicates that a business rule has been successfully executed.
        /// Placeholders are for the type of business rule, the instance the rule is executed against, 
        /// the result, and the result message.
        /// </summary>
        /// <value>BusinessRules: Executed '{0}' for '{1}'. Result: {2} (\"{3}\")</value>

        public const string LogBusinessRuleSuccess = "##AL_BusinessRules001##: Successfully evaluated '{0}' for '{1}' for fields '{4}'. Result: {2} (\"{3}\") ";

        /// <summary>
        /// Log message that indicates that a business rule has falied.
        /// Placeholders are for the type of business rule, the instance the rule is executed against, 
        /// the result, and the result message.
        /// </summary>
        /// <value>BusinessRules: Executed '{0}' for '{1}'. Result: {2} (\"{3}\")</value>

        public const string LogBusinessRuleFailed = "##AL_BusinessRules002##: Failed '{0}' for '{1}' for fields '{4}'. Result: {2} (\"{3}\") ";

        protected IDiagnosticService DiagnosticService { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BusinessRuleSet(IDiagnosticService diagnosticService)
        {
            DiagnosticService = diagnosticService;
        }

        /// <summary>
        /// Initializes a new instance of the class with a specified set of rules.
        /// </summary>
        /// <param name="diagnosticService"></param>
        /// <param name="rules">The rules that should be contained by this set.</param>
        public BusinessRuleSet(IDiagnosticService diagnosticService, IEnumerable<IBusinessRule<TEntity>> rules)
            : this(diagnosticService)
        {
            _mRules = rules.ToList();
        }



        /// <summary>
        /// List of business rules. 
        /// </summary>
        /// <remarks>
        /// The setter of this property is a little unusual.
        /// In this implementation, Rules are set via DI. This is why
        /// the setter is protected (and internal for unit tests); 
        /// in addition, rules are added to the internal collection
        /// instead of replacing it. This behavior is for deriving classes
        /// that add rules from other sources than the DI configuration (e.g.
        /// configuration data from the database). Deriving classes that whish the 
        /// Rules setter to be public should also remove this behavior by replacing the
        /// getter/setter with a more standard implementation.
        /// </remarks>
        public virtual IList<IBusinessRule<TEntity>> Rules
        {
            get => _mRules;
            protected internal set
            {
                if (value != null)
                {
                    foreach (IBusinessRule<TEntity> newRule in value)
                    {
                        _mRules.Add(newRule);
                    }
                }
            }
        }

        /// <summary>
        /// Executes the rules defined in a on a domain object.
        /// </summary>
        /// <param name="instance">The domain object instance.</param>
        /// <param name="filter">A filter for rules that are run. The filter is applied 
        /// before the rules are run.</param>
        /// <returns>A list of objects; one for each business 
        /// rule that has been run on the object.</returns>
        public virtual IEnumerable<BusinessRuleResult> ExecuteRules(
          IEntity instance,
          Func<IBusinessRule<TEntity>, bool> filter)
        {
            // Execute business rules that match the property name (if defined).
            // As the result, create an anonymous type that includes the 
            // business rule itself, and the result of its execution.
            List<dynamic> ruleExecution;
            if (filter != null)
            {
                ruleExecution = Rules
                  .ToArray()
                  .Where(filter)
                  .Select(rule => new
                  {
                      Result = rule.Execute(instance),
                      Rule = rule
                  })
                  .ToList<dynamic>();
            }
            else
            {
                ruleExecution = Rules
                 .ToArray()
                 .Select(rule => new
                 {
                     Result = rule.Execute(instance),
                     Rule = rule
                 })
                 .ToList<dynamic>();
            }

            // Log the execution results.
            foreach (var execution in ruleExecution)
            {
                string message = execution.Result.IsFailed ? LogBusinessRuleFailed : LogBusinessRuleSuccess;
                //Join the porpertyNames to one string seperated by ','
                string fields = "";
                if (execution.Rule.PropertyNames != null && execution.Rule.PropertyNames.Length != 0)
                {
                    fields = string.Join(",", execution.Rule.PropertyNames);
                }

                DiagnosticService.Debug(string.Format("Execute Rule: Message {0} - Rule Type {1} - Instance {2} - Result Message {3} - Field {4} ", message,
                  execution.Rule.GetType(), instance, execution.Result.Message, fields));
            }

            // Get the results of all business rule executions, and return them as
            // an array.
            IEnumerable<BusinessRuleResult> results = ruleExecution
              .Select(execution => (BusinessRuleResult)execution.Result);

            return results.ToArray();
        }
    }
}
