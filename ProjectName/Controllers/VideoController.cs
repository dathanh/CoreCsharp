using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Mapping;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Authorization;
using ServiceLayer.Interfaces;
using ProjectName.Attributes;
using ProjectName.Controllers.Base;
using ProjectName.Models.Base;
using ProjectName.Models.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectName.Controllers
{
    public class VideoController : ApplicationControllerGeneric<Video, DashboardVideoDataViewModel>
    {
        private readonly IVideoService _videoService;
        private readonly IGridConfigService _gridConfigService;

        public VideoController(IVideoService videoService, IGridConfigService gridConfigService, IOperationAuthorization operationAuthorization)
            : base(videoService, operationAuthorization)
        {
            _videoService = videoService;
            _gridConfigService = gridConfigService;
        }

        // GET: Video
        [Authorize(DocumentTypeKey.Video, OperationAction.View)]
        public async Task<ActionResult> Index()
        {
            var canAddRecord = OperationAuthorization.VerifyAccess(DocumentTypeKey.Video, OperationAction.Add, out List<UserRoleFunction> userRoleFunctions);
            var viewModel = new DashboardVideoIndexViewModel
            {
                CanAddNewRecord = canAddRecord
            };

            Func<GridViewModel> gridViewModel = () => new GridViewModel
            {
                GridId = "VideoGrid",
                ModelName = "Video",
                DocumentTypeId = (int)DocumentTypeKey.Video,
                GridInternalName = "Video",
                ActionDefaultWidthColumn = 150,
                UseActionDefaultColumn = true,
                AddFunction = "vm.add",
                UpdateFunction = "vm.edit",
                CustomParameters = new List<string> { "ParentCategoryId", "CategoryId" },
                CustomHeaderTemplate = "videoFilter",
            };

            viewModel.GridViewModel = await BuildGridViewModel(_gridConfigService, gridViewModel);
            return View(viewModel);
        }

        [Authorize(DocumentTypeKey.Video, OperationAction.View)]
        public MasterfileGridDataVo GetDataForGrid(VideoQuery queryInfo)
        {
            return _videoService.GetDataForGridMasterFile(queryInfo);
        }

        protected override IList<ViewColumnViewModel> GetViewColumns()
        {
            return new List<ViewColumnViewModel>

            {
                 new ViewColumnViewModel
                {
                    ColumnOrder = 1,
                    ColumnWidth = 100,
                    Name = "FormatAvatar",
                    Text = "Avatar",
                    ColumnJustification = GridColumnJustification.Left,
                    CustomTemplate="avatarTemplate"
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 2,
                    ColumnWidth = 120,
                    Name = "Name",
                    Text = "Name",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 3,
                    ColumnWidth = 130,
                    Name = "ParentCategory",
                    Text = "Parent Category",
                    Sortable = false,
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 3,
                    ColumnWidth = 130,
                    Name = "Category",
                    Text = "Sub Category",
                    Sortable = false,
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 4,
                    ColumnWidth = 130,
                    Name = "Series",
                    Text = "Series",
                    Sortable = false,
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 4,
                    ColumnWidth = 50,
                    Name = "IsTrending",
                    Text = "Trending",
                    Sortable = false,
                    ColumnJustification = GridColumnJustification.Center,
                    CustomTemplate = "isTrendingTemplate"
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 5,
                    ColumnWidth = 50,
                    Name = "IsPopular",
                    Text = "Popular",
                    Sortable = false,
                    ColumnJustification = GridColumnJustification.Center,
                    CustomTemplate = "isPopularTemplate",
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 6,
                    ColumnWidth = 50,
                    Name = "IsActive",
                    Text = "Active",
                    Sortable = false,
                    ColumnJustification = GridColumnJustification.Center,
                    CustomTemplate = "isActiveTemplate",
                 }

            };
        }

        [Authorize(DocumentTypeKey.Video, OperationAction.View)]
        public ActionResult Shared()
        {
            return View();
        }

        [HttpPost]
        [Authorize(DocumentTypeKey.Video, OperationAction.Add)]
        public int Create([FromBody]VideoParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);
            var entity = viewModel.MapTo<Video>();
            return _videoService.Add(entity).Id;
        }

        [Authorize(DocumentTypeKey.Video, OperationAction.View)]
        public ActionResult Update(int id)
        {
            var viewModel = GetMasterFileViewModel(id);
            return Json(viewModel.SharedViewModel);
        }

        [HttpPost]
        [Authorize(DocumentTypeKey.Video, OperationAction.Update)]
        public ActionResult Update([FromBody]VideoParameter parameters)
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

        [Authorize(DocumentTypeKey.Video, OperationAction.Delete)]
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            MasterFileService.DeleteById(id);
            return Json(true);
        }

        [Authorize(DocumentTypeKey.Video, OperationAction.View)]
        public JsonResult GetCategory(LookupQuery queryInfo)
        {
            var listData = _videoService.Get(o => o.CategoryId == null).OrderBy(o => o.Name);
            var result = listData.Select(o => new { DisplayName = o.Name, KeyId = o.Id });
            return Json(result);
        }

        public List<LookupItemVo> GetLookup(LookupQuery queryInfo)
        {
            var selector = new Func<Video, LookupItemVo>(o => new LookupItemVo
            {
                KeyId = o.Id,
                DisplayName = o.Name
            });
            return GetLookupForEntity(queryInfo, selector);
        }

    }
}