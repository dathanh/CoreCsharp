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
    public class BannerRule<TEntity> : IBusinessRule<TEntity> where TEntity : Entity
    {
        private readonly IBannerRepository _bannerRepository;

        public BannerRule(IBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        public BusinessRuleResult Execute(IEntity instance)
        {
            var failed = false;
            var banner = instance as Banner;
            var validationResult = new List<ValidationResult>();

            if (banner != null)
            {

                if (!string.IsNullOrWhiteSpace(banner.Name) && _bannerRepository.CheckExist(o => o.Name == banner.Name && o.Id != banner.Id))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("ExistsTextResourceKey"), "Name");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                if (banner.BannerLanguages == null || banner.BannerLanguages.Count == 0)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Other language");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else
                {
                    foreach (var catLanguage in banner.BannerLanguages)
                    {
                        if (string.IsNullOrWhiteSpace(catLanguage.Name))
                        {
                            var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Name in other language");
                            validationResult.Add(new ValidationResult(mess));
                            failed = true;
                        }
                        else if (_bannerRepository.CheckNameLanguageIsExists(catLanguage.Id, catLanguage.Name, catLanguage.LanguageId))
                        {
                            var mess = string.Format(SystemMessageLookup.GetMessage("ExistsTextResourceKey"), "Name in other language");
                            validationResult.Add(new ValidationResult(mess));
                            failed = true;
                        }
                    }
                }
                if (banner.Type == (int)BannerTypeEnum.BannerTop)
                {
                    if (string.IsNullOrEmpty(banner.VideoFile))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Video File");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                    if (banner.VideoId.GetValueOrDefault() == 0)
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Video Detail");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                }
                else if (banner.Type == (int)BannerTypeEnum.ComingSoon)
                {
                    if (string.IsNullOrEmpty(banner.UrlLink))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Youtube link");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                }
                else if (banner.Type == (int)BannerTypeEnum.AdvertisementBanner)
                {
                    if (string.IsNullOrEmpty(banner.VideoFile))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Video File");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                }

                var result = new BusinessRuleResult(failed, "", instance.GetType().Name, instance.Id, PropertyNames, Name) { ValidationResults = validationResult };
                return result;
            }

            return new BusinessRuleResult();
        }

        public string Name => "BannerRule";

        public string[] PropertyNames { get; set; }
    }
}