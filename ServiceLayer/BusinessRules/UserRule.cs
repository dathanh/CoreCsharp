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
    public class UserRule<TEntity> : IBusinessRule<TEntity> where TEntity : Entity
    {
        private readonly IUserRepository _userRepository;

        public UserRule(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public BusinessRuleResult Execute(IEntity instance)
        {
            var failed = false;
            var user = instance as User;
            var validationResult = new List<ValidationResult>();
            if (user != null)
            {
                if (user.UserRoleId == 0 || user.UserRoleId == null)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Role");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (!string.IsNullOrWhiteSpace(user.UserName) && _userRepository.CheckExist(o => o.UserName == user.UserName && o.UserRoleId == user.UserRoleId && o.Id != user.Id))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("ExistsTextResourceKey"), "User name");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (!string.IsNullOrWhiteSpace(user.Email) && _userRepository.CheckExist(o => o.Email.Trim().ToLower() == user.Email.Trim().ToLower() && o.UserRoleId == user.UserRoleId && o.Id != user.Id))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("ExistsTextResourceKey"), "Email");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                var result = new BusinessRuleResult(failed, "", instance.GetType().Name, instance.Id, PropertyNames, Name) { ValidationResults = validationResult };
                return result;
            }

            return new BusinessRuleResult();
        }

        public string Name => "UserRule";

        public string[] PropertyNames { get; set; }
    }
}