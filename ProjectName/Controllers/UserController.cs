using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Exceptions;
using Framework.Mapping;
using Framework.Service.Translation;
using Framework.Utility;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Authorization;
using ServiceLayer.Interfaces;
using ProjectName.Attributes;
using ProjectName.Controllers.Base;
using ProjectName.Models.Base;
using ProjectName.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DashboardUserDataViewModel = ProjectName.Models.User.DashboardUserDataViewModel;
using DashboardUserIndexViewModel = ProjectName.Models.User.DashboardUserIndexViewModel;
using UserParameter = ProjectName.Models.User.UserParameter;

namespace ProjectName.Controllers
{
    public class UserController : ApplicationControllerGeneric<User, DashboardUserDataViewModel>
    {
        private readonly IUserService _userService;
        private readonly IGridConfigService _gridConfigService;

        public UserController(IUserService userService, IGridConfigService gridConfigService, IOperationAuthorization operationAuthorization)
            : base(userService, operationAuthorization)
        {
            _userService = userService;
            _gridConfigService = gridConfigService;
        }

        // GET: User
        [Authorize(DocumentTypeKey.User, OperationAction.View)]
        public async Task<ActionResult> Index()
        {
            var canAddItem = OperationAuthorization.VerifyAccess(DocumentTypeKey.User, OperationAction.Add, out _);

            var viewModel = new DashboardUserIndexViewModel
            {
                CanExportExcel = true,
                CanAddNewRecord = canAddItem,
            };

            Func<GridViewModel> gridViewModel = () => new GridViewModel
            {
                GridId = "UserGrid",
                ModelName = "User",
                DocumentTypeId = (int)DocumentTypeKey.User,
                GridInternalName = "User",
                ActionDefaultWidthColumn = 150,
                UseActionDefaultColumn = true,
                AddFunction = "vm.add",
                UpdateFunction = "vm.edit",
            };

            viewModel.GridViewModel = await BuildGridViewModel(_gridConfigService, gridViewModel);
            return View(viewModel);
        }

        [Authorize(DocumentTypeKey.User, OperationAction.View)]
        public MasterfileGridDataVo GetDataForGrid(QueryInfo queryInfo)
        {
            return _userService.GetDataForGridMasterFile(queryInfo);
        }

        protected override IList<ViewColumnViewModel> GetViewColumns()
        {
            return new List<ViewColumnViewModel>

            {
                new ViewColumnViewModel
                {
                    ColumnOrder = 1,
                    ColumnWidth = 120,
                    Name = "UserName",
                    Text = "User name",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 2,
                    ColumnWidth = 200,
                    Name = "FullName",
                    Text = "Full name",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 3,
                    ColumnWidth = 125,
                    Name = "Role",
                    Text = "Role",
                    Sortable = false,
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 4,
                    ColumnWidth = 200,
                    Name = "Email",
                    Text = "Email",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 5,
                    ColumnWidth = 125,
                    Name = "PhoneInFormat",
                    Text = "Phone",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 6,
                    ColumnWidth = 60,
                    Name = "IsActive",
                    Text = "Active",
                    Sortable = false,
                    ColumnJustification = GridColumnJustification.Center,
                    CustomTemplate = "isActiveTemplate"
                }
            };
        }

        [Authorize(DocumentTypeKey.User, OperationAction.View)]
        public ActionResult Shared()
        {
            return View();
        }

        private void ValidatePasswordForUser(DashboardUserShareViewModel shareVm)
        {
            if (shareVm != null)
            {
                var validationResult = new List<ValidationResult>();
                var failed = false;
                if (string.IsNullOrWhiteSpace(shareVm.Password))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Password");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else
                {
                    if (!CaculatorHelper.IsValidPassword(shareVm.Password))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("PasswordIsInvalid"));
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                }

                if (string.IsNullOrWhiteSpace(shareVm.ConfirmPassword))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"),
                        "Confirm password");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (!failed && !shareVm.Password.Equals(shareVm.ConfirmPassword))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("NewPasswordNotMatch"));
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (failed)
                {
                    var result = new BusinessRuleResult(true, "", "SaveChangePassword", 0, null, "SaveChangePasswordRule") { ValidationResults = validationResult };
                    throw new BusinessRuleException("BussinessGenericErrorMessageKey", new[] { result });
                }
            }
        }

        [HttpPost]
        [Authorize(DocumentTypeKey.User, OperationAction.Add)]
        public int Create([FromBody]UserParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);
            var shareVm = viewModel.SharedViewModel as DashboardUserShareViewModel;
            ValidatePasswordForUser(shareVm);

            var entity = viewModel.MapTo<User>();
            if (!string.IsNullOrWhiteSpace(entity.Password))
            {
                entity.Password = PasswordHelper.HashString(entity.Password, entity.UserName);
            }
            return _userService.Add(entity).Id;
        }

        [Authorize(DocumentTypeKey.User, OperationAction.View)]
        public ActionResult Update(int id)
        {
            var viewModel = GetMasterFileViewModel(id);
            var shareVm = viewModel.SharedViewModel as DashboardUserShareViewModel;
            if (shareVm != null)
            {
                shareVm.ConfirmPassword = "";
                shareVm.Password = "";
            }
            return Json(viewModel.SharedViewModel);
        }

        [HttpPost]
        [Authorize(DocumentTypeKey.User, OperationAction.Update)]
        public ActionResult Update([FromBody]UserParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);
            var objShareVm = viewModel.SharedViewModel as DashboardUserShareViewModel;

            if (objShareVm != null)
            {
                if (!string.IsNullOrWhiteSpace(objShareVm.Password) && objShareVm.Password != objShareVm.ConfirmPassword)
                {
                    throw new UserVisibleException("PasswordNotMatch");
                }
            }
            byte[] lastModified = null;

            if (ModelState.IsValid)
            {
                var entity = MasterFileService.GetById(viewModel.SharedViewModel.Id);
                var oldPass = entity.Password;
                entity.OldRoleId = entity.UserRoleId;
                var mappedEntity = viewModel.MapPropertiesToInstance(entity);
                if (objShareVm != null)
                {
                    if (!string.IsNullOrWhiteSpace(objShareVm.Password))
                    {
                        ValidatePasswordForUser(objShareVm);
                        mappedEntity.Password = PasswordHelper.HashString(objShareVm.Password, mappedEntity.UserName);
                    }
                    else
                    {
                        mappedEntity.Password = oldPass;
                    }
                }
                lastModified = MasterFileService.Update(mappedEntity).LastModified;
            }

            return Json(new { Error = string.Empty, Data = new { LastModified = lastModified } });
        }

        [Authorize(DocumentTypeKey.User, OperationAction.Delete)]
        [HttpDelete]
        public async Task<JsonResult> Delete(int id)
        {
            var isDelete = await GetCurrentUserId();

            if (id == isDelete)
            {
                return Json(new { Error = SystemMessageLookup.GetMessage("CannotDeleteYourself") });
            }

            MasterFileService.DeleteById(id);
            return Json(true);
        }

        //Change password
        [Authorize(DocumentTypeKey.None, OperationAction.View)]
        public ActionResult ChangePassword()
        {
            return PartialView("ChangePassword");
        }

        [HttpPost]
        [Authorize(DocumentTypeKey.None, OperationAction.View)]
        public async Task<JsonResult> SaveChangePasswordProfile(SaveChangePasswordViewModel data)
        {
            var currentUserId = await GetCurrentUserId();
            var user = _userService.GetById(currentUserId);

            if (user != null)
            {
                var isSuccessful = _userService.SaveChangePassword(user.Id, data.Password, data.ConfirmPassword, data.OldPassword);
                if (isSuccessful)
                {
                    return Json(new { Status = true });
                }
            }

            return Json(new { Status = false });
        }

        [Authorize(DocumentTypeKey.User, OperationAction.View)]
        public JsonResult GetLookup()
        {
            var data = _userService.Get(o => o.IsActive);
            var result = data.Select(o => new LookupItemVo { DisplayName = o.FullName, KeyId = o.Id }).ToList();
            return Json(result);
        }
    }
}