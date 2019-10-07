using Framework.BusinessRule;
using Framework.DomainModel;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.BusinessRules.Common
{
    /// <summary>
    /// This data-annotation rule will loop through all data annotation rules of domain model and validate
    /// before EF validation. We want to have central place of displaying/catching data validation rules 
    /// as well as custom business rules.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class DataAnnotationBusinessRule<TEntity> : IBusinessRule<TEntity> where TEntity : Entity
    {
        public BusinessRuleResult Execute(IEntity instance)
        {
            var validationResults = new List<ValidationResult>();

            var vc = new ValidationContext(instance);
            bool bCanCreate = Validator.TryValidateObject(instance, vc, validationResults, true);

            var businessRuleResult = new BusinessRuleResult(!bCanCreate, "", instance.GetType().Name, instance.Id,
                PropertyNames, Name);
            //Validation result contains field names and message
            businessRuleResult.ValidationResults = validationResults;
            return businessRuleResult;

        }

        public string Name => "DataAnnotationBusinessRule";

        public string[] PropertyNames { get; set; }
    }
}
