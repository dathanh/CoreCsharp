using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;

namespace Framework.BusinessRule
{
    /// <summary>
    /// Interface for business rules that are attached to domain objects.
    /// </summary>
    /// <remarks>
    /// Business rules are evaluated before a domain object is saved to the database.
    /// Think of them as a replacement for triggers: you can prevent operations
    /// from happened by throwing an exception, or you can modify the internal 
    /// state of the object.        
    /// <br/>        
    /// </remarks>
    public interface IBusinessRule<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Evaluates the state of a domain object.
        /// </summary>
        /// <param name="instance">Instance of a domain object</param>
        /// <returns>The result of the business rule.</returns>
        BusinessRuleResult Execute(IEntity instance);

        /// <summary>
        /// An identifier for the business rule.
        /// </summary>
        /// <remarks>
        /// The name is passed to <see cref="BusinessRuleResult"/> so that you can 
        /// infer what business rule has been executed by examining the result.
        /// </remarks>
        string Name
        {
            get;
        }


        /// <summary>
        /// Defines the property names that the business rule evaluates on.
        /// </summary>
        /// <remarks>
        /// In case that at least one of the properties is not available to the form,
        /// the business rule cannot be evaluated and will be ignored.
        /// The business rule -> property mapping is also required to visualize
        /// failed business rules correctly.
        /// </remarks>
        string[] PropertyNames
        {
            get;
            set;

        }

    }
}
