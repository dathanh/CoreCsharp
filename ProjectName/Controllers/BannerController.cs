using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Mapping;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Authorization;
using ServiceLayer.Interfaces;
using ProjectName.Attributes;
using ProjectName.Controllers.Base;
using ProjectName.Models.Banner;
using ProjectName.Models.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectName.Controllers
{
    public class BannerController : ApplicationControllerGeneric<Banner, DashboardBannerDataViewModel>
    {
        private readonly IBannerService _bannerService;
        private readonly IGridConfigService _gridConfigService;

        public BannerController(IBannerService bannerService, IGridConfigService gridConfigService, IOperationAuthorization operationAuthorization)
            : base(bannerService, operationAuthorization)
        {
            _bannerService = bannerService;
            _gridConfigService = gridConfigService;
        }

        // GET: Banner
        [Authorize(DocumentTypeKey.Banner, OperationAction.View)]
        public async Task<ActionResult> Index()
        {
            var canAddRecord = OperationAuthorization.VerifyAccess(DocumentTypeKey.Banner, OperationAction.Add, out List<UserRoleFunction> userRoleFunctions);
            var viewModel = new DashboardBannerIndexViewModel
            {
                CanAddNewRecord = canAddRecord
            };
            Func<GridViewModel> gridViewModel = () => new GridViewModel
            {
                GridId = "BannerGrid",
                ModelName = "Banner",
                DocumentTypeId = (int)DocumentTypeKey.Banner,
                GridInternalName = "Banner",
                ActionDefaultWidthColumn = 150,
                UseActionDefaultColumn = true,
                AddFunction = "vm.add",
                UpdateFunction = "vm.edit",
                CustomParameters = new List<string> { "Type" },
                CustomHeaderTemplate = "bannerFilter",
            };

            viewModel.GridViewModel = await BuildGridViewModel(_gridConfigService, gridViewModel);
            return View(viewModel);
        }

        [Authorize(DocumentTypeKey.Banner, OperationAction.View)]
        public MasterfileGridDataVo GetDataForGrid(BannerQueryInfo queryInfo)
        {
            return _bannerService.GetDataForGridMasterFile(queryInfo);
        }

        protected override IList<ViewColumnViewModel> GetViewColumns()
        {
            return new List<ViewColumnViewModel>

            {
                new ViewColumnViewModel
                {
                    ColumnOrder = 1,
                    ColumnWidth = 120,
                    Name = "Name",
                    Text = "Name",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 2,
                    ColumnWidth = 125,
                    Name = "UrlLink",
                    Text = "Url Link",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 3,
                    ColumnWidth = 125,
                    Name = "TypeFormat",
                    Text = "Type",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 4,
                    ColumnWidth = 125,
                    Name = "OrderNumber",
                    Text = "Order Number",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 5,
                    ColumnWidth = 60,
                    Name = "IsActive",
                    Text = "Active",
                    Sortable = false,
                    ColumnJustification = GridColumnJustification.Center,
                    CustomTemplate = "isActiveTemplate"
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 6,
                    ColumnWidth = 60,
                    Name = "IsHideDescription",
                    Text = "Hide Description",
                    Sortable = false,
                    ColumnJustification = GridColumnJustification.Center,
                    CustomTemplate = "isHideDescriptionTemplate"
                }
            };
        }

        [Authorize(DocumentTypeKey.Banner, OperationAction.View)]
        public ActionResult Shared()
        {
            return View();
        }

        [HttpPost]
        [Authorize(DocumentTypeKey.Banner, OperationAction.Add)]
        public int Create([FromBody]BannerParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);
            var entity = viewModel.MapTo<Banner>();
            return _bannerService.Add(entity).Id;
        }

        [Authorize(DocumentTypeKey.Banner, OperationAction.View)]
        public ActionResult Update(int id)
        {
            var viewModel = GetMasterFileViewModel(id);
            return Json(viewModel.SharedViewModel);
        }

        [HttpPost]
        [Authorize(DocumentTypeKey.Banner, OperationAction.Update)]
        public ActionResult Update([FromBody]BannerParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);

            byte[] lastModified = null;

            if (ModelState.IsValid)
            {
                var entity = MasterFileService.GetById(viewModel.SharedViewModel.Id);
                var mappedEntity = viewModel.MapPropertiesToInstance(entity);
                lastModified = MasterFileService.Update(mappedEntity).LastModified;
            }

            return Json(new { Error = string.Empty, Data = new { LastModified = lastModified } });
        }

        [Authorize(DocumentTypeKey.Banner, OperationAction.Delete)]
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            MasterFileService.DeleteById(id);
            return Json(true);
        }
    }
}