using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Exceptions;
using Framework.Mapping;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceLayer.Authorization;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Common;
using ProjectName.Attributes;
using ProjectName.Controllers.Base;
using ProjectName.Models.Base;
using ProjectName.Models.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectName.Controllers
{
    public class UserRoleController : ApplicationControllerGeneric<UserRole, DashboardRoleDataViewModel>
    {
        //
        // GET: /UserRole/
        private readonly IUserRoleService _userRoleService;
        private readonly IGridConfigService _gridConfigService;
        private readonly IMenuExtractData _menuExtractData;
        //private readonly IRenderViewToString _renderViewToString;

        public UserRoleController(IUserRoleService userRoleService, IGridConfigService gridConfigService,
            IOperationAuthorization operationAuthorization, IMenuExtractData menuExtractData)
            : base(userRoleService, operationAuthorization)
        {
            _userRoleService = userRoleService;
            _gridConfigService = gridConfigService;
            _menuExtractData = menuExtractData;
        }

        [Authorize(DocumentTypeKey.UserRole, OperationAction.View)]
        public async Task<ActionResult> Index()
        {
            var canUpdateItem = OperationAuthorization.VerifyAccess(DocumentTypeKey.UserRole, OperationAction.Update, out _);
            var canDeleteItem = OperationAuthorization.VerifyAccess(DocumentTypeKey.UserRole, OperationAction.Delete, out _);
            var viewModel = new DashboardRoleIndexViewModel
            {
                CanDeleteRecord = canDeleteItem,
                CanEditRecord = canUpdateItem
            };

            Func<GridViewModel> gridViewModel = () => new GridViewModel
            {
                GridId = "RoleGrid",
                ModelName = "UserRole",
                DocumentTypeId = (int)DocumentTypeKey.UserRole,
                GridInternalName = "UserRole",
                //UseDeleteColumn = true,
                UseActionDefaultColumn = false,
                AddFunction = "vm.add",
                UpdateFunction = "vm.edit",
                ExtFunc1 = "vm.detail",
                PopupWidth = 600,
                PopupHeight = 460
            };

            viewModel.GridViewModel = await BuildGridViewModel(_gridConfigService, gridViewModel);
            return View(viewModel);
        }

        [Authorize(DocumentTypeKey.UserRole, OperationAction.View)]
        public MasterfileGridDataVo GetDataForGrid(QueryInfo queryInfo)
        {
            return GetDataForGridMasterFile(queryInfo);
        }

        protected override IList<ViewColumnViewModel> GetViewColumns()
        {
            var objViewColumn = new List<ViewColumnViewModel>
            {
                new ViewColumnViewModel
                {
                    ColumnOrder = 1,
                    Name = "Name",
                    Text = "Name",
                    ColumnWidth = 600,
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 2,
                    Name = "Command",
                    Text = " ",
                    ColumnWidth = 200,
                    Sortable = false,
                    ColumnJustification = GridColumnJustification.Left,
                    CustomTemplate = "commandRoleTemplate"
                }
            };
            return objViewColumn;
        }

        [Authorize(DocumentTypeKey.UserRole, OperationAction.View)]
        public ActionResult Update(int id)
        {
            var viewModel = GetMasterFileViewModel(id);

            return Json(viewModel.SharedViewModel);
        }

        [Authorize(DocumentTypeKey.UserRole, OperationAction.View)]
        public JsonResult GetRoleFunction(int id)
        {
            var queryData = _userRoleService.GetRoleFunction(id);
            var clientsJson = Json(queryData);

            return clientsJson;
        }

        private List<UserRoleFunction> GetAllRoleFunction()
        {
            var objResult = new List<UserRoleFunction>();
            var objListDocumentType = _userRoleService.GetAllDocumentTypeId();
            foreach (var id in objListDocumentType)
            {
                var objView = new UserRoleFunction
                {
                    DocumentTypeId = id,
                    SecurityOperationId = (int)OperationAction.View
                };

                objResult.Add(objView);
                var objInsert = new UserRoleFunction
                {
                    DocumentTypeId = id,
                    SecurityOperationId = (int)OperationAction.Add
                };

                objResult.Add(objInsert);
                var objUpdate = new UserRoleFunction
                {
                    DocumentTypeId = id,
                    SecurityOperationId = (int)OperationAction.Update
                };

                objResult.Add(objUpdate);
                var objDelete = new UserRoleFunction
                {
                    DocumentTypeId = id,
                    SecurityOperationId = (int)OperationAction.Delete
                };

                objResult.Add(objDelete);

                var objShowMenu = new UserRoleFunction
                {
                    DocumentTypeId = id,
                    SecurityOperationId = (int)OperationAction.ShowMenu
                };
                objResult.Add(objShowMenu);
            }
            return objResult;
        }

        private List<UserRoleFunction> ProcessMappingFromUserRoleGrid(string userRoleFunctionDataParam)
        {
            var userRoleFunctionData = JsonConvert.DeserializeObject<List<UserRoleFunctionGridVo>>(userRoleFunctionDataParam);
            var objResult = new List<UserRoleFunction>();
            var listUpdate = userRoleFunctionData;
            if (listUpdate != null && listUpdate.Count != 0)
            {
                foreach (var userRoleFunctionGridVo in listUpdate)
                {
                    var objView = new UserRoleFunction
                    {
                        DocumentTypeId = userRoleFunctionGridVo.Id,
                        SecurityOperationId = (int)OperationAction.View,
                        IsDeleted = !userRoleFunctionGridVo.IsView
                    };
                    objResult.Add(objView);

                    //Implement View insert
                    var objInsert = new UserRoleFunction
                    {
                        DocumentTypeId = userRoleFunctionGridVo.Id,
                        SecurityOperationId = (int)OperationAction.Add,
                        IsDeleted = !userRoleFunctionGridVo.IsInsert
                    };
                    objResult.Add(objInsert);

                    //Implement View update
                    var objUpdate = new UserRoleFunction
                    {
                        DocumentTypeId = userRoleFunctionGridVo.Id,
                        SecurityOperationId = (int)OperationAction.Update,
                        IsDeleted = !userRoleFunctionGridVo.IsUpdate
                    };
                    objResult.Add(objUpdate);
                    //Implement View delete
                    var objDelete = new UserRoleFunction
                    {
                        DocumentTypeId = userRoleFunctionGridVo.Id,
                        SecurityOperationId = (int)OperationAction.Delete,
                        IsDeleted = !userRoleFunctionGridVo.IsDelete
                    };
                    objResult.Add(objDelete);

                    //objResult.Add(objProcess);

                    var objShowMenu = new UserRoleFunction
                    {
                        DocumentTypeId = userRoleFunctionGridVo.Id,
                        SecurityOperationId = (int)OperationAction.ShowMenu,
                        IsDeleted = !userRoleFunctionGridVo.IsShowMenu
                    };
                    objResult.Add(objShowMenu);
                }
            }
            return objResult;
        }

        [HttpPost]
        [Authorize(DocumentTypeKey.UserRole, OperationAction.Update)]
        public ActionResult Update([FromBody]RoleParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);
            byte[] lastModified = null;
            var shareViewModel = viewModel.SharedViewModel as DashboardRoleShareViewModel;
            if (shareViewModel != null)
            {
                var entity = MasterFileService.GetById(shareViewModel.Id);
                //if (entity.AppRoleName == AppRole.GlobalAdmin.ToString())
                //{
                //    throw new Exception("Not allow to change global admin role function");
                //}
                var listRoleFunctionUpdate = shareViewModel.CheckAll ? GetAllRoleFunction() : ProcessMappingFromUserRoleGrid(shareViewModel.RoleFunctionData);
                var mappedEntity = shareViewModel.MapPropertiesToInstance(entity);
                var listRoleFunctionOld = mappedEntity.UserRoleFunctions;
                // Check user have edit some value in list role old => delete role old and add role new
                foreach (var oldItem in listRoleFunctionOld)
                {
                    if (listRoleFunctionUpdate.Any(o => o.DocumentTypeId == oldItem.DocumentTypeId))
                    {
                        oldItem.IsDeleted = true;
                    }
                }
                //after check user removed, remove item of the list new with conditions has property IsDelete equal true;

                //Copy listRoleFunctionUpdate
                var listRoleFunctionUpdateRecheck = listRoleFunctionUpdate.ToList();
                foreach (var item in listRoleFunctionUpdateRecheck.Where(item => item.IsDeleted))
                {
                    listRoleFunctionUpdate.Remove(item);
                }
                // Add listUpdate
                foreach (var item in listRoleFunctionUpdate)
                {
                    mappedEntity.UserRoleFunctions.Add(item);
                }
                if (ModelState.IsValid)
                {
                    mappedEntity = MasterFileService.Update(mappedEntity);
                    lastModified = mappedEntity.LastModified;
                    _menuExtractData.RefreshListData();
                }
            }


            return Json(new { Error = string.Empty, Data = new { LastModified = lastModified } });
        }

        [HttpPost]
        [Authorize(DocumentTypeKey.UserRole, OperationAction.View)]
        public ActionResult ViewDetail([FromBody]RoleParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);
            byte[] lastModified = null;
            var shareViewModel = viewModel.SharedViewModel as DashboardRoleShareViewModel;
            if (shareViewModel != null)
            {
                var entity = MasterFileService.GetById(shareViewModel.Id);
                //if (entity.AppRoleName == AppRole.GlobalAdmin.ToString())
                //{
                //    throw new Exception("Not allow to change global admin role function");
                //}
                var listRoleFunctionUpdate = shareViewModel.CheckAll ? GetAllRoleFunction() : ProcessMappingFromUserRoleGrid(shareViewModel.RoleFunctionData);
                var mappedEntity = shareViewModel.MapPropertiesToInstance(entity);
                var listRoleFunctionOld = mappedEntity.UserRoleFunctions;
                // Check user have edit some value in list role old => delete role old and add role new
                foreach (var oldItem in listRoleFunctionOld)
                {
                    if (listRoleFunctionUpdate.Any(o => o.DocumentTypeId == oldItem.DocumentTypeId))
                    {
                        oldItem.IsDeleted = true;
                    }
                }
                //after check user removed, remove item of the list new with conditions has property IsDelete equal true;

                //Copy listRoleFunctionUpdate
                var listRoleFunctionUpdateRecheck = listRoleFunctionUpdate.ToList();
                foreach (var item in listRoleFunctionUpdateRecheck.Where(item => item.IsDeleted))
                {
                    listRoleFunctionUpdate.Remove(item);
                }
                // Add listUpdate
                foreach (var item in listRoleFunctionUpdate)
                {
                    mappedEntity.UserRoleFunctions.Add(item);
                }
                if (ModelState.IsValid)
                {
                    mappedEntity = MasterFileService.Update(mappedEntity);
                    lastModified = mappedEntity.LastModified;
                    _menuExtractData.RefreshListData();
                }
            }


            return Json(new { Error = string.Empty, Data = new { LastModified = lastModified } });
        }


        [Authorize(DocumentTypeKey.UserRole, OperationAction.Add)]
        public ActionResult Shared()
        {
            return View();
        }

        [Authorize(DocumentTypeKey.UserRole, OperationAction.View)]
        public ActionResult ViewDetailRole(int id)
        {
            return View(id);
        }
        [HttpPost]
        [Authorize(DocumentTypeKey.UserRole, OperationAction.Add)]
        public ActionResult Create([FromBody]RoleParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);
            if (viewModel.SharedViewModel is DashboardRoleShareViewModel shareViewModel)
            {
                var listRoleFunctionUpdate = shareViewModel.CheckAll ? GetAllRoleFunction() : ProcessMappingFromUserRoleGrid(shareViewModel.RoleFunctionData);
                var listRoleFunctionOld = new List<UserRoleFunction>();
                // Check user have edit some value in list role old => delete role old and add role new
                foreach (var oldItem in listRoleFunctionOld)
                {
                    if (listRoleFunctionUpdate.Any(o => o.DocumentTypeId == oldItem.DocumentTypeId))
                    {
                        oldItem.IsDeleted = true;
                    }
                }
                //after check user removed, remove item of the list new with conditions has property IsDelete equal true;

                //Copy listRoleFunctionUpdate
                var listRoleFunctionUpdateRecheck = listRoleFunctionUpdate.ToList();
                foreach (var item in listRoleFunctionUpdateRecheck.Where(item => item.IsDeleted))
                {
                    listRoleFunctionUpdate.Remove(item);
                }
                var objListFunctionForEntity = new List<UserRoleFunction>();
                // Add listUpdate
                objListFunctionForEntity.AddRange(listRoleFunctionUpdate);
                // Add list data in old
                foreach (var itemOld in listRoleFunctionOld.Where(o => !o.IsDeleted))
                {
                    var objAdd = new UserRoleFunction
                    {
                        DocumentTypeId = itemOld.DocumentTypeId,
                        SecurityOperationId = itemOld.SecurityOperationId
                    };
                    objListFunctionForEntity.Add(objAdd);
                }
                var entity = shareViewModel.MapTo<UserRole>();
                entity.UserRoleFunctions = objListFunctionForEntity;
                var savedEntity = MasterFileService.Add(entity);
                _menuExtractData.RefreshListData();
                return Json(new { savedEntity.Id });
            }
            return Json(new { Id = 0 });
        }

        [HttpPost]
        [Authorize(DocumentTypeKey.UserRole, OperationAction.Delete)]
        public ActionResult Delete(int id)
        {
            var item = MasterFileService.GetById(id);
            if (item != null && (item.AppRoleName == AppRole.GlobalAdmin.ToString()))
            {
                throw new UserVisibleException("CanDeleteAppRole");
            }
            MasterFileService.DeleteById(id);
            _menuExtractData.RefreshListData();
            return Json(new { isSuccess = true });
        }


        [Authorize(DocumentTypeKey.UserRole, OperationAction.View)]
        public List<LookupItemVo> GetLookup(LookupQuery queryInfo)
        {
            var selector = new Func<UserRole, LookupItemVo>(o => new LookupItemVo
            {
                KeyId = o.Id,
                DisplayName = o.Name
            });
            return GetLookupForEntity(queryInfo, selector);
        }

        [Authorize(DocumentTypeKey.UserRole, OperationAction.View)]
        public JsonResult GetListRoles()
        {
            var data = _userRoleService.ListAll();
            var result = data.Select(o => new LookupItemVo { DisplayName = o.Name, KeyId = o.Id }).ToList();
            return Json(result);
        }

        [Authorize(DocumentTypeKey.UserRole, OperationAction.View)]
        public ActionResult ViewDetailId(int id)
        {
            var viewModel = GetMasterFileViewModel(id);

            return Json(viewModel.SharedViewModel);
        }
    }
}