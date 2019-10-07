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
    public class PlayListRule<TEntity> : IBusinessRule<TEntity> where TEntity : Entity
    {
        private readonly IPlayListRepository _playListRepository;

        public PlayListRule(IPlayListRepository playListRepository)
        {
            _playListRepository = playListRepository;
        }

        public BusinessRuleResult Execute(IEntity instance)
        {
            var failed = false;
            var playList = instance as PlayList;
            var validationResult = new List<ValidationResult>();
            if (playList != null)
            {
                if (!string.IsNullOrWhiteSpace(playList.Name) &&
                    _playListRepository.CheckExist(o => o.Name == playList.Name && o.Id != playList.Id))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("ExistsTextResourceKey"), "Name");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (playList.PlayListLanguages == null || playList.PlayListLanguages.Count == 0)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"),
                        "Other language");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else
                {
                    foreach (var playListLanguage in playList.PlayListLanguages)
                    {
                        if (string.IsNullOrWhiteSpace(playListLanguage.Name))
                        {
                            var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"),
                                "Name in other language");
                            validationResult.Add(new ValidationResult(mess));
                            failed = true;
                        }
                        else if (_playListRepository.CheckNameLanguageIsExists(playListLanguage.Id, playListLanguage.Name,
                            playListLanguage.LanguageId))
                        {
                            var mess = string.Format(SystemMessageLookup.GetMessage("ExistsTextResourceKey"),
                                "Name in other language");
                            validationResult.Add(new ValidationResult(mess));
                            failed = true;
                        }
                    }
                }
                if (playList.VideoPlayLists.Count == 0)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"),
                        "Videos");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                var result = new BusinessRuleResult(failed, "", instance.GetType().Name, instance.Id, PropertyNames, Name) { ValidationResults = validationResult };
                return result;
            }

            return new BusinessRuleResult();

        }

        public string Name => "PlayListRule";

        public string[] PropertyNames { get; set; }
    }
}
