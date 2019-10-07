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
    public class SeriesRule<TEntity> : IBusinessRule<TEntity> where TEntity : Entity
    {
        private readonly ISeriesRepository _seriesRepository;

        public SeriesRule(ISeriesRepository seriesRepository)
        {
            _seriesRepository = seriesRepository;
        }

        public BusinessRuleResult Execute(IEntity instance)
        {
            var failed = false;
            var series = instance as Series;
            var validationResult = new List<ValidationResult>();
            if (series != null)
            {
                if (!string.IsNullOrWhiteSpace(series.Name) &&
                    _seriesRepository.CheckExist(o => o.Name == series.Name && o.Id != series.Id))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("ExistsTextResourceKey"), "Name");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (series.SeriesLanguages == null || series.SeriesLanguages.Count == 0)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"),
                        "Other language");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else
                {
                    foreach (var seriesLanguage in series.SeriesLanguages)
                    {
                        if (string.IsNullOrWhiteSpace(seriesLanguage.Name))
                        {
                            var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"),
                                "Name in other language");
                            validationResult.Add(new ValidationResult(mess));
                            failed = true;
                        }
                        else if (_seriesRepository.CheckNameLanguageIsExists(seriesLanguage.Id, seriesLanguage.Name,
                            seriesLanguage.LanguageId))
                        {
                            var mess = string.Format(SystemMessageLookup.GetMessage("ExistsTextResourceKey"),
                                "Name in other language");
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

        public string Name => "SeriesRule";

        public string[] PropertyNames { get; set; }
    }
}
