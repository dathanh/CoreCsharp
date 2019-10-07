using Framework.BusinessRule;
using Framework.DomainModel;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.Service.Translation;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.BusinessRules
{
    public class ConfigRule<TEntity> : IBusinessRule<TEntity> where TEntity : Entity
    {
        private readonly IConfigRepository _configRepository;

        public ConfigRule(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        public BusinessRuleResult Execute(IEntity instance)
        {
            var failed = false;
            var config = instance as Framework.DomainModel.Entities.Config;
            var validationResult = new List<ValidationResult>();
            if (config != null)
            {
                if (string.IsNullOrWhiteSpace(config.Background))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Background");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                if (string.IsNullOrWhiteSpace(config.VideoFile))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Background");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                var result = new BusinessRuleResult(failed, "", instance.GetType().Name, instance.Id, PropertyNames, Name) { ValidationResults = validationResult };
                return result;
            }

            return new BusinessRuleResult();
        }

        public string Name => "ConfigRule";

        public string[] PropertyNames { get; set; }
    }
}