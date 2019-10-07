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
using ProjectName.Models.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ProjectName.Controllers
{
    public class SeriesController : ApplicationControllerGeneric<Series, DashboardSeriesDataViewModel>
    {
        private readonly ISeriesService _seriesService;
        private readonly IGridConfigService _gridConfigService;

        public SeriesController(ISeriesService seriesService, IGridConfigService gridConfigService, IOperationAuthorization operationAuthorization)
            : base(seriesService, operationAuthorization)
        {
            _seriesService = seriesService;
            _gridConfigService = gridConfigService;
        }

        // GET: Series
        [Authorize(DocumentTypeKey.Series, OperationAction.View)]
        public async Task<ActionResult> Index()
        {
            var viewModel = new DashboardSeriesIndexViewModel();

            Func<GridViewModel> gridViewModel = () => new GridViewModel
            {
                GridId = "SeriesGrid",
                ModelName = "Series",
                DocumentTypeId = (int)DocumentTypeKey.Series,
                GridInternalName = "Series",
                ActionDefaultWidthColumn = 150,
                UseActionDefaultColumn = true,
                AddFunction = "vm.add",
                UpdateFunction = "vm.edit",
            };

            viewModel.GridViewModel = await BuildGridViewModel(_gridConfigService, gridViewModel);
            return View(viewModel);
        }

        [Authorize(DocumentTypeKey.Series, OperationAction.View)]
        public MasterfileGridDataVo GetDataForGrid(QueryInfo queryInfo)
        {
            return _seriesService.GetDataForGridMasterFile(queryInfo);
        }

        protected override IList<ViewColumnViewModel> GetViewColumns()
        {
            return new List<ViewColumnViewModel>

            {
                new ViewColumnViewModel
                {
                    ColumnOrder = 1,
                    ColumnWidth = 120,
                    Name = "BackgroundFormat",
                    Text = "Background",
                    Sortable = false,
                    ColumnJustification = GridColumnJustification.Left,
                    CustomTemplate="backgroundTemplate"
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
                    ColumnWidth = 200,
                    Name = "Description",
                    Text = "Description",
                    ColumnJustification = GridColumnJustification.Left,
                    CustomTemplate="descriptionTemplate"
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 4,
                    ColumnWidth = 50,
                    Name = "IsActive",
                    Text = "Active",
                    Sortable = false,
                    ColumnJustification = GridColumnJustification.Center,
                    CustomTemplate = "isActiveTemplate"
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 5,
                    ColumnWidth = 50,
                    Name = "OrderNumber",
                    Text = "OrderNumber",
                    ColumnJustification = GridColumnJustification.Center,
                },

            };
        }

        [Authorize(DocumentTypeKey.Series, OperationAction.View)]
        public ActionResult Shared()
        {
            return View();
        }

        [HttpPost]
        [Authorize(DocumentTypeKey.Series, OperationAction.Add)]
        public int Create([FromBody]SeriesParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);
            var entity = viewModel.MapTo<Series>();
            var shareViewModel = viewModel.SharedViewModel as DashboardSeriesShareViewModel;
            var listVideoId = shareViewModel == null ? new List<int>() : shareViewModel.SelectedItemIds;
            return _seriesService.AddSeries(entity, listVideoId);
        }

        [Authorize(DocumentTypeKey.Series, OperationAction.View)]
        public ActionResult Update(int id)
        {
            var viewModel = GetMasterFileViewModel(id);
            return Json(viewModel.SharedViewModel);
        }

        [HttpPost]
        [Authorize(DocumentTypeKey.Series, OperationAction.Update)]
        public ActionResult Update([FromBody]SeriesParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);

            byte[] lastModified = null;

            if (ModelState.IsValid)
            {
                var entity = MasterFileService.GetById(viewModel.SharedViewModel.Id);
                var mappedEntity = viewModel.MapPropertiesToInstance(entity);
                var shareViewModel = viewModel.SharedViewModel as DashboardSeriesShareViewModel;
                var listVideoId = shareViewModel == null ? new List<int>() : shareViewModel.SelectedItemIds;
                lastModified = _seriesService.UpdateSeries(entity, listVideoId);
            }

            return Json(new { Error = string.Empty, Data = new { LastModified = lastModified } });
        }

        [Authorize(DocumentTypeKey.Series, OperationAction.Delete)]
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            MasterFileService.DeleteById(id);
            return Json(true);
        }
        [Authorize(DocumentTypeKey.Series, OperationAction.View)]
        public JsonResult GetListSeries()
        {
            var data = _seriesService.ListAll();
            var result = data.Select(o => new LookupItemVo { DisplayName = o.Name, KeyId = o.Id }).ToList();
            return Json(result);
        }
    }
}