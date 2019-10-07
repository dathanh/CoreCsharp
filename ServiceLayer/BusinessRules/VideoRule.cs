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
    public class VideoRule<TEntity> : IBusinessRule<TEntity> where TEntity : Entity
    {
        private readonly IVideoRepository _videoRepository;

        public VideoRule(IVideoRepository videoRepository)
        {
            _videoRepository = videoRepository;
        }

        public BusinessRuleResult Execute(IEntity instance)
        {
            var failed = false;
            var video = instance as Video;
            var validationResult = new List<ValidationResult>();
            if (video != null)
            {
                if (!string.IsNullOrWhiteSpace(video.Name) && _videoRepository.CheckExist(o => o.Name == video.Name && o.Id != video.Id))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("ExistsTextResourceKey"), "Name");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else if (video.Name.Length > 65)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredLengthOfName"), "Name");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                if (string.IsNullOrWhiteSpace(video.UrlLink))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "UrlLink");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                if (video.CategoryId.GetValueOrDefault() == 0)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Sub Category");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                if (video.VideoLanguages == null || video.VideoLanguages.Count == 0)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Other language");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else
                {
                    foreach (var videoLanguage in video.VideoLanguages)
                    {
                        if (string.IsNullOrWhiteSpace(videoLanguage.Name))
                        {
                            var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Name in other language");
                            validationResult.Add(new ValidationResult(mess));
                            failed = true;
                        }
                        else if (videoLanguage.Name.Length > 65)
                        {
                            var mess = string.Format(SystemMessageLookup.GetMessage("RequiredLengthOfName"), "Name in other language");
                            validationResult.Add(new ValidationResult(mess));
                            failed = true;
                        }
                        else if (_videoRepository.CheckNameLanguageIsExists(videoLanguage.Id, videoLanguage.Name, videoLanguage.LanguageId))
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

        public string Name => "VideoRule";

        public string[] PropertyNames { get; set; }
    }
}