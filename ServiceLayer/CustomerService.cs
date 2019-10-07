using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Exceptions;
using Framework.Utility;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using ServiceLayer.Translate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace ServiceLayer
{
    public class CustomerService : MasterFileService<Customer>, ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IFrontEndMessageLookup _frontEndMessageLookup;
        public CustomerService(ITenantPersistenceService tenantPersistenceService, ICustomerRepository customerRepository,
            IFrontEndMessageLookup frontEndMessageLookup,
            IBusinessRuleSet<Customer> businessRuleSet = null)
            : base(customerRepository, customerRepository, tenantPersistenceService, businessRuleSet)
        {
            _customerRepository = customerRepository;
            _frontEndMessageLookup = frontEndMessageLookup;
        }

        private Customer RegisterWithOauth(SigninWithFbDto fbInfo, bool loginWithFaceBook)
        {
            byte[] avatar = null;
            if (!string.IsNullOrWhiteSpace(fbInfo.AvatarUrl))
            {
                try
                {
                    using (var webClient = new WebClient())
                    {
                        avatar = webClient.DownloadData(fbInfo.AvatarUrl);
                    }
                }
                catch
                {
                    avatar = null;
                }
            }

            var customer = _customerRepository.FirstOrDefault(o => o.UserName.ToLower() == fbInfo.FbId.ToLower());
            if (customer != null)
            {
                if ((customer.Avatar == null || customer.Avatar.Length == 0) && avatar != null)
                {
                    customer.Avatar = avatar;
                }
                if (!string.IsNullOrEmpty(customer.FullName) && (!string.IsNullOrWhiteSpace(fbInfo.FirstName) || !string.IsNullOrWhiteSpace(fbInfo.LastName)))
                {
                    customer.FullName = fbInfo.FirstName + " " + fbInfo.LastName;
                }
                _customerRepository.Commit();
            }
            else
            {
                var password = PasswordHelper.HashString(fbInfo.FbId, fbInfo.FbId);
                if (fbInfo.LanguageId.GetValueOrDefault() == 0)
                {
                    fbInfo.LanguageId = (int)LanguageKey.English;
                }
                customer = new Customer
                {
                    UserName = fbInfo.FbId.ToLower(),
                    Password = password,
                    IsActive = true,
                    FullName = fbInfo.FirstName + " " + fbInfo.LastName,
                    Email = fbInfo.UserName,
                    IsAccountFacebook = loginWithFaceBook,
                    IsAccountGoogle = !loginWithFaceBook,
                    Avatar = avatar,
                    LanguageId = fbInfo.LanguageId
                };
                _customerRepository.Add(customer);
                _customerRepository.Commit();
            }
            return customer;
        }

        public Customer RegisterByFacebook(SigninWithFbDto fbInfo)
        {
            return RegisterWithOauth(fbInfo, true);
        }

        public Customer RegisterByGoogle(SigninWithFbDto ggInfo)
        {
            return RegisterWithOauth(ggInfo, false);
        }

        public Customer GetCustomerByUserNameAndPass(string userName, string password)
        {
            var hashedPassword = PasswordHelper.HashString(password, userName);
            return _customerRepository.FirstOrDefault(o => o.UserName == userName && o.Password == hashedPassword);
        }


        public bool Register(SignupDto signUp)
        {
            var failed = false;
            var validationResult = new List<ValidationResult>();
            if (!signUp.IsCheckCondition)
            {
                var mess = _frontEndMessageLookup.GetMessage("CheckTermWhenSignUp", signUp.LanguageId.GetValueOrDefault());
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            if (string.IsNullOrWhiteSpace(signUp.UserName) || string.IsNullOrWhiteSpace(signUp.Password))
            {
                var mess = _frontEndMessageLookup.GetMessage("UserNameAndPasswordRequired", signUp.LanguageId.GetValueOrDefault());
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            else
            {
                if (signUp.Password != signUp.ConfirmPass)
                {
                    var mess = _frontEndMessageLookup.GetMessage("PasswordAndConfNotMatch", signUp.LanguageId.GetValueOrDefault());
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                if (!signUp.UserName.IsFormatEmail())
                {
                    var mess = _frontEndMessageLookup.GetMessage("UserNameIsNotAnValidEmail", signUp.LanguageId.GetValueOrDefault());
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
            }
            if (!failed)
            {
                if (!CaculatorHelper.IsValidPasswordCustomer(signUp.Password))
                {
                    var mess = _frontEndMessageLookup.GetMessage("PasswordMustBeComplex", signUp.LanguageId.GetValueOrDefault());
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
            }

            if (!failed)
            {
                if (_customerRepository.CheckExist(o => o.UserName.ToLower() == signUp.UserName.ToLower()))
                {
                    var mess = _frontEndMessageLookup.GetMessage("UserNameIsExists", signUp.LanguageId.GetValueOrDefault());
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
            }
            if (failed)
            {
                var result = new BusinessRuleResult(true, "", "RegisterCustomer", 0, null, "RegisterCustomerRule") { ValidationResults = validationResult };
                throw new BusinessRuleException("BussinessGenericErrorMessageKey", new[] { result });
            }

            var password = PasswordHelper.HashString(signUp.Password, signUp.UserName.ToLower().Trim());
            if (signUp.LanguageId.GetValueOrDefault() == 0)
            {
                signUp.LanguageId = (int)LanguageKey.English;
            }
            var customer = new Customer
            {
                UserName = signUp.UserName.ToLower(),
                Password = password,
                Email = signUp.UserName.ToLower(),
                LanguageId = signUp.LanguageId,
                ExpiredTime = DateTime.Now.AddDays(5),
                ActiveCode = Guid.NewGuid().ToString(),
                IsActive = false,
                UnsubscribeCode = Guid.NewGuid().ToString(),
                IsUnsubscribe = false,
            };
            _customerRepository.Add(customer);
            _customerRepository.Commit();
            return true;
        }

        public bool UpdateProfile(CustomerEditProfile data)
        {
            var failed = false;
            var validationResult = new List<ValidationResult>();
            if (string.IsNullOrWhiteSpace(data.FullName))
            {
                var mess = _frontEndMessageLookup.GetMessage("FillInFullName", data.LanguageId.GetValueOrDefault());
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            if (data.Gender.GetValueOrDefault() == 0)
            {
                var mess = _frontEndMessageLookup.GetMessage("FillInGender", data.LanguageId.GetValueOrDefault());
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            if (data.BirthDateValue == DateTime.MinValue)
            {
                var mess = _frontEndMessageLookup.GetMessage("BirthDateIsInValid", data.LanguageId.GetValueOrDefault());
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            if (failed)
            {
                var result = new BusinessRuleResult(true, "", "UpdateCustomerInfo", 0, null, "UpdateCustomerInfoRule") { ValidationResults = validationResult };
                throw new BusinessRuleException("BussinessGenericErrorMessageKey", new[] { result });
            }

            var customer = _customerRepository.GetById(data.Id);
            if (!string.IsNullOrWhiteSpace(data.Avatar) && data.Avatar.Contains("data:image"))
            {
                var countDataAvatar = data.Avatar.Split(",");
                if (countDataAvatar.Length > 1)
                {
                    data.Avatar = countDataAvatar[1];
                    customer.Avatar = Convert.FromBase64String(data.Avatar);
                }
            }
            customer.FullName = data.FullName;
            customer.Description = data.Description;
            customer.Gender = data.Gender;
            customer.Dob = data.BirthDateValue;
            _customerRepository.Commit();
            return true;
        }

        public bool ChangePassword(CustomerChangePasswordDto data)
        {
            var failed = false;
            var validationResult = new List<ValidationResult>();
            var customer = _customerRepository.GetById(data.Id);
            if (customer == null)
            {
                throw new FrontEndUserVisibleException("InvalidData");
            }
            if (string.IsNullOrWhiteSpace(data.CurrentPassword))
            {
                var mess = _frontEndMessageLookup.GetMessage("FillInCurrentPassword", data.LanguageId.GetValueOrDefault());
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            if (string.IsNullOrWhiteSpace(data.NewPassword))
            {
                var mess = _frontEndMessageLookup.GetMessage("FillInNewPassword", data.LanguageId.GetValueOrDefault());
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            else if (data.NewPassword != data.RetypePassword)
            {
                var mess = _frontEndMessageLookup.GetMessage("NewPassAndRetypePassNotMatch", data.LanguageId.GetValueOrDefault());
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            else if (!CaculatorHelper.IsValidPasswordCustomer(data.NewPassword))
            {
                var mess = _frontEndMessageLookup.GetMessage("PasswordMustBeComplex", data.LanguageId.GetValueOrDefault());
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            var hashedPassword = PasswordHelper.HashString(data.CurrentPassword, customer.UserName);
            if (hashedPassword != customer.Password)
            {
                var mess = _frontEndMessageLookup.GetMessage("CurrentPasswordInvalid", data.LanguageId.GetValueOrDefault());
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            if (failed)
            {
                var result = new BusinessRuleResult(true, "", "ChangeCustomerPass", 0, null, "ChangeCustomerPassRule") { ValidationResults = validationResult };
                throw new BusinessRuleException("BussinessGenericErrorMessageKey", new[] { result });
            }
            var newPass = PasswordHelper.HashString(data.NewPassword, customer.UserName);
            customer.Password = newPass;
            _customerRepository.Commit();
            return true;
        }
        public bool SendFeedback(Feedback data)
        {
            var failed = false;
            var validationResult = new List<ValidationResult>();
            if (string.IsNullOrWhiteSpace(data.Username))
            {
                var mess = _frontEndMessageLookup.GetMessage("FillInUsername", data.LanguageId.GetValueOrDefault());
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            if (string.IsNullOrWhiteSpace(data.Email))
            {
                var mess = _frontEndMessageLookup.GetMessage("FillInEmail", data.LanguageId.GetValueOrDefault());
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            else if (!data.Email.IsFormatEmail())
            {
                var mess = _frontEndMessageLookup.GetMessage("UserNameIsNotAnValidEmail", data.LanguageId.GetValueOrDefault());
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }

            if (string.IsNullOrWhiteSpace(data.Message))
            {
                var mess = _frontEndMessageLookup.GetMessage("FillInMessage", data.LanguageId.GetValueOrDefault());
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            if (failed)
            {
                var result = new BusinessRuleResult(true, "", "SendFeedbackInfo", 0, null, "SendFeedbackInfoRule") { ValidationResults = validationResult };
                throw new BusinessRuleException("BussinessGenericErrorMessageKey", new[] { result });
            }

            return true;
        }

        public bool SetActiveNewAccount(ActiveCustomer activeCustomerInfo)
        {
            var failed = false;
            var customer = _customerRepository.FirstOrDefault(o => o.ActiveCode == activeCustomerInfo.ActiveCode);
            var validationResult = new List<ValidationResult>();
            if (customer == null)
            {
                var mess = _frontEndMessageLookup.GetMessage("InvalidData", activeCustomerInfo.LanguageId.GetValueOrDefault());
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            else
            if (customer.ExpiredTime.GetValueOrDefault() < DateTime.Now)
            {
                var mess = _frontEndMessageLookup.GetMessage("ExpiredTime", activeCustomerInfo.LanguageId.GetValueOrDefault());
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }

            if (failed)
            {
                var result = new BusinessRuleResult(true, "", "SetActiveNewAccount", 0, null, "SetActiveNewAccount") { ValidationResults = validationResult };
                throw new BusinessRuleException("BussinessGenericErrorMessageKey", new[] { result });
            }

            customer.IsActive = true;
            customer.ExpiredTime = DateTime.Now.AddSeconds(-1);
            _customerRepository.Commit();
            return true;
        }

        public string ForgotPassword(EmailForgotPassword emailInfo)
        {
            var failed = false;
            var validationResult = new List<ValidationResult>();
            Customer customer = null;
            if (string.IsNullOrWhiteSpace(emailInfo.Email))
            {
                var mess = _frontEndMessageLookup.GetMessage("FillInEmail", emailInfo.LanguageId.GetValueOrDefault());
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            else if (!emailInfo.Email.IsFormatEmail())
            {
                var mess = _frontEndMessageLookup.GetMessage("UserNameIsNotAnValidEmail", emailInfo.LanguageId.GetValueOrDefault());
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            else
            {
                customer = _customerRepository.FirstOrDefault(o => o.UserName.ToLower() == emailInfo.Email.ToLower());
                if (customer == null)
                {
                    var mess = _frontEndMessageLookup.GetMessage("EmailNotExists", emailInfo.LanguageId.GetValueOrDefault());
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
            }

            if (failed)
            {
                var result = new BusinessRuleResult(true, "", "ForgotPassword", 0, null, "ForgotPassword") { ValidationResults = validationResult };
                throw new BusinessRuleException("BussinessGenericErrorMessageKey", new[] { result });
            }

            if (customer != null)
            {
                customer.ForgotPassword = Guid.NewGuid().ToString();
                _customerRepository.Commit();
                return customer.ForgotPassword;
            }

            return "";
        }

        public bool SetNewPassByForgotPassCode(SetNewPassWordModel setNewPassInfo)
        {
            var failed = false;
            var validationResult = new List<ValidationResult>();

            var customer = _customerRepository.FirstOrDefault(o => o.ForgotPassword == setNewPassInfo.ForgotPasswordCode);

            if (customer == null)
            {
                var mess = _frontEndMessageLookup.GetMessage("EmailNotExists", setNewPassInfo.LanguageId.GetValueOrDefault());
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            if (string.IsNullOrWhiteSpace(setNewPassInfo.NewPassword))
            {
                var mess = _frontEndMessageLookup.GetMessage("FillInNewPassword", setNewPassInfo.LanguageId.GetValueOrDefault());
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            else if (setNewPassInfo.NewPassword != setNewPassInfo.ConfirmNewPassword)
            {
                var mess = _frontEndMessageLookup.GetMessage("NewPassAndRetypePassNotMatch", setNewPassInfo.LanguageId.GetValueOrDefault());
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            else if (!CaculatorHelper.IsValidPasswordCustomer(setNewPassInfo.NewPassword))
            {
                var mess = _frontEndMessageLookup.GetMessage("PasswordMustBeComplex", setNewPassInfo.LanguageId.GetValueOrDefault());
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            if (failed)
            {
                var result = new BusinessRuleResult(true, "", "ChangeCustomerPass", 0, null, "ChangeCustomerPassRule") { ValidationResults = validationResult };
                throw new BusinessRuleException("BussinessGenericErrorMessageKey", new[] { result });
            }

            var newPass = PasswordHelper.HashString(setNewPassInfo.NewPassword, customer.UserName);
            customer.Password = newPass;
            customer.ForgotPassword = "";
            _customerRepository.Commit();
            return true;
        }

        public bool SetUnsubscribe(string unsubscribeCode, int languageId)
        {
            var failed = false;
            var validationResult = new List<ValidationResult>();
            var customer = _customerRepository.FirstOrDefault(o => o.UnsubscribeCode == unsubscribeCode);

            if (customer == null)
            {
                var mess = _frontEndMessageLookup.GetMessage("InvalidData", languageId);
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            else if (customer.IsUnsubscribe.GetValueOrDefault())
            {
                var mess = _frontEndMessageLookup.GetMessage("EmailUnsubscribe", languageId);
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }

            if (failed)
            {
                var result = new BusinessRuleResult(true, "", "SetUnsubscribe", 0, null, "SetUnsubscribe") { ValidationResults = validationResult };
                throw new BusinessRuleException("BussinessGenericErrorMessageKey", new[] { result });
            }

            customer.IsUnsubscribe = true;
            _customerRepository.Commit();
            return true;
        }
    }
}