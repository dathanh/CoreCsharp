using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Mapping;
using Framework.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interfaces;
using ProjectName.Attributes;
using ProjectName.Controllers.Base;
using ProjectName.Models.Base;
using ProjectName.Models.MyProfile;
using ProjectName.Models.User;
using System;
using System.Threading.Tasks;

namespace ProjectName.Controllers
{
    public class MyProfileController : ApplicationControllerGeneric<User, DashboardMyProfileDataViewModel>
    {
        private readonly IUserService _userService;
        private readonly IResizeImage _resizeImage;
        private readonly IHostingEnvironment _env;

        public MyProfileController(IResizeImage resizeImage,
            IHostingEnvironment env, IUserService userService)
            : base(userService)
        {
            _userService = userService;
            _resizeImage = resizeImage;
            _env = env;
        }


        [Authorize(DocumentTypeKey.None, OperationAction.View)]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(DocumentTypeKey.None, OperationAction.View)]
        public async Task<ActionResult> GetProfileInfo()
        {
            var currentUserId = await GetCurrentUserId();
            User user = _userService.GetById(currentUserId);
            var viewModel = user.MapTo<DashboardMyProfileDataViewModel>();
            var shareViewModel = viewModel.SharedViewModel as DashboardMyProfileShareViewModel;
            if (shareViewModel != null)
            {
                if (user.Avatar != null)
                {
                    shareViewModel.Avatar = "data:image/jpg;base64," + Convert.ToBase64String(user.Avatar);
                }
                else
                {
                    shareViewModel.Avatar = "/css/images/avatar.jpg";
                }
            }

            return Json(shareViewModel);
        }


        [HttpPost]
        [Authorize(DocumentTypeKey.None, OperationAction.View)]
        public async Task<ActionResult> SaveMyProfile([FromBody]UserParameter parameters)
        {
            var logoFilePath = "";
            var currentUserId = await GetCurrentUserId();
            var viewModel = MapFromClientParameters(parameters);
            var user = _userService.GetById(currentUserId);
            if (user != null)
            {
                var mappedUser = viewModel.MapPropertiesToInstance(user);
                if (viewModel.SharedViewModel is DashboardMyProfileShareViewModel shareViewModel)
                {
                    if (string.IsNullOrWhiteSpace(shareViewModel.Avatar) || shareViewModel.Avatar == "/css/images/avatar.jpg")
                    {
                        mappedUser.Avatar = null;
                    }
                    else if (!shareViewModel.Avatar.Contains("data:image/jpg;base64"))
                    {
                        logoFilePath = _env.WebRootPath + shareViewModel.Avatar;
                        mappedUser.Avatar = _resizeImage.ResizeImageByWidth(logoFilePath, AppSettingValue.DefaultResize);
                    }
                }

                byte[] lastUserModified = _userService.Update(mappedUser).LastModified;

                //Delete image after save success
                if (System.IO.File.Exists(logoFilePath))
                {
                    System.IO.File.Delete(logoFilePath);
                }

                return Json(new { Error = string.Empty, Data = new { LastModified = lastUserModified } });

            }

            return Json(new { Error = "UserNonExist" });
        }
    }
}