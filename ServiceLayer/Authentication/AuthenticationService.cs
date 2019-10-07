using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Exceptions;
using Framework.Service.Diagnostics;
using Framework.Service.Translation;
using Framework.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServiceLayer.Authentication
{

    public class AuthenticationService : Interfaces.IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthenticationService(IHttpContextAccessor httpContextAccessor,
                                     IClaimsManager claimsManager,
                                     IDiagnosticService diagnosticService, IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            ClaimsManager = claimsManager;
            _diagnosticService = diagnosticService;
            _userRepository = userRepository;
        }

        private readonly IDiagnosticService _diagnosticService;
        public IClaimsManager ClaimsManager { get; set; }
        private readonly IUserRepository _userRepository;

        public async Task<UserDto> GetCurrentUser()
        {
            var userDataClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(o => o.Type == ClaimsDeclaration.UserDataClaim);
            if (userDataClaim != null)
            {
                var userDataJson = userDataClaim.Value;
                return JsonConvert.DeserializeObject<UserDto>(userDataJson);
            }
            await _httpContextAccessor.HttpContext.SignOutAsync();
            return null;
        }

        public async Task SignOut()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync();

        }

        public async Task<UserDto> SignIn(string userName, string password, bool rememberMe, string appRole = "")
        {
            // encrypt password

            var claims = ClaimsManager.CreateClaims(userName, password, appRole).ToList();
            var user = ClaimsManager.ValidateExpressProjectLogin(claims);

            if (user == null || !user.IsWebProjectUser)
            {

                var claimException = new InvalidClaimsException("InvalidUserAndPasswordText");
                _diagnosticService.Error(SystemMessageLookup.GetMessage("InvalidUserAndPasswordText"));
                _diagnosticService.Error("UserName:" + userName);
                throw claimException;
            }

            if (!user.IsActive)
            {
                var claimException = new UserVisibleException("LoginWithInacticeUser");
                _diagnosticService.Error(SystemMessageLookup.GetMessage("LoginWithInacticeUser"));
                _diagnosticService.Error("UserName:" + userName);
                throw claimException;
            }

            var userDto = new UserDto
            {
                Email = user.Email,
                Id = user.Id,
                UserName = user.UserName,
                UserRoleId = user.UserRoleId,
                OldUserRoleId = user.OldRoleId,
                UserRoleName = user.AppRole.ToString(),
            };

            var userData = JsonConvert.SerializeObject(userDto);
            claims.Add(new Claim(ClaimsDeclaration.UserDataClaim, userData));
            var userIdentity = new ClaimsIdentity(claims, "UserIdentity");
            var userPrincipal = new ClaimsPrincipal(userIdentity);
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties { ExpiresUtc = new DateTimeOffset(DateTime.UtcNow.AddMonths(1)), IsPersistent = true });

            return userDto;
        }

        public User RestorePassword(string email, out string passwordRandom)
        {
            var failed = false;
            var validationResult = new List<ValidationResult>();
            User objUser = null;

            // Check email
            if (string.IsNullOrWhiteSpace(email))
            {
                var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Email");
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            else if (
                !Regex.IsMatch(email,
                    @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z"))
            {
                var mess = string.Format(SystemMessageLookup.GetMessage("FieldInvalidText"), "Email");
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            else
            {
                objUser = _userRepository.FirstOrDefault(o => o.Email == email);
                if (objUser == null)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("FieldInvalidText"), "Email");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
            }

            var result = new BusinessRuleResult(failed, "", "RestorePassword", 0, null, "RestorePasswordRule") { ValidationResults = validationResult };

            if (failed)
            {
                // Give messages on every rule that failed
                throw new BusinessRuleException("BussinessGenericErrorMessageKey", new[] { result });
            }

            // Create password
            passwordRandom = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            //var password = PasswordHelper.HashString(passwordRandom, objUser.UserName);
            // Update password to database
            //objUser.Password = password;
            _userRepository.Update(objUser);
            _userRepository.Commit();
            return objUser;
        }
    }
}
