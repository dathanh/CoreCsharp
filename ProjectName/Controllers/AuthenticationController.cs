using Framework.DomainModel.ValueObject;
using Framework.Service.Translation;
using Framework.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceLayer.Interfaces;
using ProjectName.Controllers.Base;
using ProjectName.Models.Authentication;
using ProjectName.Models.User;
using System.Threading.Tasks;

namespace ProjectName.Controllers
{
    public class AuthenticationController : ApplicationControllerBase
    {
        private readonly IUserService _userService;
        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IActionResult> SignOut()
        {
            //Sign out from authentication
            await HttpContext.SignOutAsync();
            return Json(new { IsSignOut = true });
        }

        [AllowAnonymous]
        public ActionResult SignIn(string returnUrl)
        {
            var viewModel = new DashboardAuthenticationSignInViewModel();
            return View(viewModel);
        }


        [HttpPost]
        public async Task<JsonResult> SignIn([FromBody]SigninViewModel loginModel)
        {
            var password = PasswordHelper.HashString(loginModel.Password, loginModel.UserName);
            await AuthenticationService.SignIn(loginModel.UserName, password, loginModel.RememberMe);
            return Json(new { isLogin = true });
        }

        [AllowAnonymous]
        public JsonResult SaveChangePassword(string param)
        {
            var data = JsonConvert.DeserializeObject<ChangePasswordViewModel>(param);
            if (data != null)
            {
                _userService.SaveChangePassword(data.Id, data.NewPassword, data.ConfirmNewPassword);
                return Json(true);
            }
            return Json(new { Error = SystemMessageLookup.GetMessage("ChangePasswordError") });
        }

        [AllowAnonymous]
        public ActionResult NoPermission()
        {
            return View();
        }
    }
}