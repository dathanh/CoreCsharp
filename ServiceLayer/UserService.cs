using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Exceptions;
using Framework.Service.Translation;
using Framework.Utility;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;

namespace ServiceLayer
{
    public class UserService : MasterFileService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public UserService(ITenantPersistenceService tenantPersistenceService, IUserRoleRepository userRoleRepository, IUserRepository userRepository,
            IBusinessRuleSet<User> businessRuleSet = null)
            : base(userRepository, userRepository, tenantPersistenceService, businessRuleSet)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
        }

        public byte[] GetAvatarContent(int userId)
        {
            var objUser = _userRepository.FirstOrDefault(o => o.Id == userId);
            return objUser?.Avatar;
        }

        public bool HasPermission(int userId, int documentTypeId, int operationAction)
        {
            return _userRepository.HasPermission(userId, documentTypeId, operationAction);
        }

        public bool SaveChangePassword(int id, string newPassword, string confirmPassword, string oldPassword)
        {
            var failed = false;
            var validationResult = new List<ValidationResult>();
            var user = _userRepository.GetById(id);

            if (user != null)
            {
                if (user.Password != PasswordHelper.HashString(oldPassword, user.UserName))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("CurrentPasswordInvalid"));
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else if (oldPassword == newPassword)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("NewPasswordEqualCurrentPassword"));
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (string.IsNullOrWhiteSpace(newPassword) || !CaculatorHelper.IsValidPassword(newPassword))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("PasswordIsInvalid"));
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (failed)
                {
                    var result = new BusinessRuleResult(true, "", "SaveChangePassword", 0, null, "SaveChangePasswordRule") { ValidationResults = validationResult };
                    throw new BusinessRuleException("BussinessGenericErrorMessageKey", new[] { result });

                }

                return SaveChangePassword(id, newPassword, confirmPassword);
            }
            return false;
        }

        public bool SaveChangePassword(int id, string newPassword, string confirmNewPassword)
        {
            var failed = false;
            var validationResult = new List<ValidationResult>();

            User user = _userRepository.GetById(id);
            if (user == null)
            {
                var mess = string.Format(SystemMessageLookup.GetMessage("UserWasDeleted"));
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(newPassword))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "New Password");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else if (!CaculatorHelper.IsValidPassword(newPassword))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("PasswordIsInvalid"));
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (string.IsNullOrWhiteSpace(confirmNewPassword))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"),
                        "Mật khẩu nhập lại");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (!failed && !newPassword.Equals(confirmNewPassword))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("NewPasswordNotMatch"));
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
            }

            var result = new BusinessRuleResult(failed, "", "SaveChangePassword", 0, null, "SaveChangePasswordRule") { ValidationResults = validationResult };

            if (failed)
            {// Give messages on every rule that failed
                throw new BusinessRuleException("BussinessGenericErrorMessageKey", new[] { result });
            }

            _userRepository.UpdatePassword(user.Id, newPassword);
            return true;
        }

        public User RegisterByFacebook(SigninWithFbDto hostInfo, string hostName)
        {
            byte[] avatar = null;

            if (!string.IsNullOrWhiteSpace(hostInfo.AvatarUrl))
            {
                try
                {
                    using (var webClient = new WebClient())
                    {
                        avatar = webClient.DownloadData(hostInfo.AvatarUrl);
                    }
                }
                catch
                {
                    avatar = null;
                }
            }

            var hostRole = _userRoleRepository.FirstOrDefault(o => o.Name == hostName);

            if (hostRole == null)
            {
                throw new UserVisibleException("InvalidData");
            }

            var host = _userRepository.Get(o => o.Email == hostInfo.UserName && o.UserRoleId == hostRole.Id).FirstOrDefault();

            if (host != null)
            {
                if (avatar != null)
                {
                    host.Avatar = avatar;
                }
                if (!string.IsNullOrWhiteSpace(hostInfo.FirstName) || !string.IsNullOrWhiteSpace(hostInfo.LastName))
                {
                    host.FullName = hostInfo.FirstName + " " + hostInfo.LastName;
                }
                _userRepository.Update(host);
                _userRepository.Commit();
            }
            else
            {
                var password = PasswordHelper.HashString(hostInfo.Password, hostInfo.UserName);
                host = new User
                {
                    UserName = hostInfo.UserName,
                    Password = password,
                    UserRoleId = hostRole.Id,
                    IsActive = true,
                    FullName = hostInfo.FirstName + " " + hostInfo.LastName,
                    Email = hostInfo.UserName,
                    IsAccountFacebook = true,
                    Avatar = avatar
                };
                _userRepository.Add(host);
                _userRepository.Commit();
            }

            return host;
        }

        public UserDto GetUserByUserNameAndPass(string username, string password)
        {
            var hashedPassword = PasswordHelper.HashStringNetCore(password, username);
            var user = _userRepository.GetUserByUserNameAndPass(username, hashedPassword);
            if (user == null)
            {
                return null;
            }
            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName
            };
            return userDto;
        }

    }
}
