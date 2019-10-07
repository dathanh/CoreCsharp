using Framework.BusinessRule;
using Framework.DomainModel;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.Service.Translation;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.BusinessRules
{
    public class CategoryRule<TEntity> : IBusinessRule<TEntity> where TEntity : Entity
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryRule(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public BusinessRuleResult Execute(IEntity instance)
        {
            var failed = false;
            var category = instance as Category;
            var validationResult = new List<ValidationResult>();
            if (category != null)
            {
                if (!string.IsNullOrWhiteSpace(category.Name) && _categoryRepository.CheckExist(o => o.Name == category.Name && o.Id != category.Id))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("ExistsTextResourceKey"), "Name");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (category.ParentId.GetValueOrDefault() != 0 && category.Id == category.ParentId)
                {
                    var mess = SystemMessageLookup.GetMessage("CannotSelectParentWithSameCategory");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                if (category.CategoryLanguages == null || category.CategoryLanguages.Count == 0)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Other language");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else
                {
                    foreach (var catLanguage in category.CategoryLanguages)
                    {
                        if (string.IsNullOrWhiteSpace(catLanguage.Name))
                        {
                            var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Name in other language");
                            validationResult.Add(new ValidationResult(mess));
                            failed = true;
                        }
                        else if (_categoryRepository.CheckNameLanguageIsExists(catLanguage.Id, catLanguage.Name, catLanguage.LanguageId))
                        {
                            var mess = string.Format(SystemMessageLookup.GetMessage("ExistsTextResourceKey"), "Name in other language");
                            validationResult.Add(new ValidationResult(mess));
                            failed = true;
                        }
                    }
                }
                var result = new BusinessRuleResult(failed, "", instance.GetType().Name, instance.Id, PropertyNames, Name) { ValidationResults = validationResult };
                return result;
            }

            return new BusinessRuleResult();
        }

        public string Name => "CategoryRule";

        public string[] PropertyNames { get; set; }
    }
}