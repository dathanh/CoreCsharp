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
using ProjectName.Models.PlayList;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ProjectName.Controllers
{
    public class PlayListController : ApplicationControllerGeneric<PlayList, DashboardPlayListDataViewModel>
    {
        private readonly IPlayListService _playListService;
        private readonly IGridConfigService _gridConfigService;

        public PlayListController(IPlayListService playListService, IGridConfigService gridConfigService, IOperationAuthorization operationAuthorization)
            : base(playListService, operationAuthorization)
        {
            _playListService = playListService;
            _gridConfigService = gridConfigService;
        }

        // GET: PlayList
        [Authorize(DocumentTypeKey.PlayList, OperationAction.View)]
        public async Task<ActionResult> Index()
        {
            var viewModel = new DashboardPlayListIndexViewModel();

            Func<GridViewModel> gridViewModel = () => new GridViewModel
            {
                GridId = "PlayListGrid",
                ModelName = "PlayList",
                DocumentTypeId = (int)DocumentTypeKey.PlayList,
                GridInternalName = "PlayList",
                ActionDefaultWidthColumn = 150,
                UseActionDefaultColumn = true,
                AddFunction = "vm.add",
                UpdateFunction = "vm.edit",
            };

            viewModel.GridViewModel = await BuildGridViewModel(_gridConfigService, gridViewModel);
            return View(viewModel);
        }

        [Authorize(DocumentTypeKey.PlayList, OperationAction.View)]
        public MasterfileGridDataVo GetDataForGrid(QueryInfo queryInfo)
        {
            return _playListService.GetDataForGridMasterFile(queryInfo);
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
                    ColumnWidth = 200,
                    Name = "Description",
                    Text = "Description",
                    ColumnJustification = GridColumnJustification.Left,
                    CustomTemplate="descriptionTemplate"
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 3,
                    ColumnWidth = 50,
                    Name = "IsActive",
                    Text = "Active",
                    Sortable = false,
                    ColumnJustification = GridColumnJustification.Center,
                    CustomTemplate = "isActiveTemplate"
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 3,
                    ColumnWidth = 50,
                    Name = "OrderNumber",
                    Text = "OrderNumber",
                    ColumnJustification = GridColumnJustification.Center,
                },

            };
        }

        [Authorize(DocumentTypeKey.PlayList, OperationAction.View)]
        public ActionResult Shared()
        {
            return View();
        }

        [HttpPost]
        [Authorize(DocumentTypeKey.PlayList, OperationAction.Add)]
        public int Create([FromBody]PlayListParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);
            var entity = viewModel.MapTo<PlayList>();
            var shareViewModel = viewModel.SharedViewModel as DashboardPlayListShareViewModel;
            if (shareViewModel != null)
            {
                var listVideoId = shareViewModel.SelectedItemIds;
                foreach (var id in listVideoId)
                {
                    entity.VideoPlayLists.Add(new VideoPlayList
                    {
                        VideoId = id
                    });
                }
            }
            return _playListService.Add(entity).Id;
        }

        [Authorize(DocumentTypeKey.PlayList, OperationAction.View)]
        public ActionResult Update(int id)
        {
            var viewModel = GetMasterFileViewModel(id);
            return Json(viewModel.SharedViewModel);
        }

        [HttpPost]
        [Authorize(DocumentTypeKey.PlayList, OperationAction.Update)]
        public ActionResult Update([FromBody]PlayListParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);

            byte[] lastModified = null;

            if (ModelState.IsValid)
            {
                var entity = MasterFileService.GetById(viewModel.SharedViewModel.Id);
                var mappedEntity = viewModel.MapPropertiesToInstance(entity);
                foreach (var item in mappedEntity.VideoPlayLists)
                {
                    item.IsDeleted = true;
                }
                var shareViewModel = viewModel.SharedViewModel as DashboardPlayListShareViewModel;
                if (shareViewModel != null)
                {
                    var listVideoId = shareViewModel.SelectedItemIds;
                    foreach (var id in listVideoId)
                    {
                        entity.VideoPlayLists.Add(new VideoPlayList
                        {
                            VideoId = id
                        });
                    }
                }
                lastModified = MasterFileService.Update(mappedEntity).LastModified;
            }

            return Json(new { Error = string.Empty, Data = new { LastModified = lastModified } });
        }

        [Authorize(DocumentTypeKey.PlayList, OperationAction.Delete)]
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            MasterFileService.DeleteById(id);
            return Json(true);
        }

    }
}