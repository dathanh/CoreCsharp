using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Mapping;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Authorization;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Common;
using ProjectName.Attributes;
using ProjectName.Controllers.Base;
using ProjectName.Models.Base;
using ProjectName.Models.Config;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ProjectName.Controllers
{
    public class ConfigController : ApplicationControllerGeneric<Config, DashboardConfigDataViewModel>
    {
        private readonly IConfigService _configService;
        private readonly IConfigSystem _configSystem;
        private readonly IGridConfigService _gridConfigService;

        public ConfigController(IConfigService configService, IGridConfigService gridConfigService,
                                IOperationAuthorization operationAuthorization, IConfigSystem configSystem)
            : base(configService, operationAuthorization)
        {
            _configService = configService;
            _gridConfigService = gridConfigService;
            _configSystem = configSystem;
        }

        // GET: Config
        [Authorize(DocumentTypeKey.Config, OperationAction.View)]
        public async Task<ActionResult> Index()
        {
            var viewModel = new DashboardConfigIndexViewModel();

            Func<GridViewModel> gridViewModel = () => new GridViewModel
            {
                GridId = "ConfigGrid",
                ModelName = "Config",
                DocumentTypeId = (int)DocumentTypeKey.Config,
                GridInternalName = "Config",
                ActionDefaultWidthColumn = 150,
                UseActionDefaultColumn = false,
                CustomHeaderTemplate = "configFilter",
                AddFunction = "vm.add",
                UpdateFunction = "vm.edit",
            };

            viewModel.GridViewModel = await BuildGridViewModel(_gridConfigService, gridViewModel);
            return View(viewModel);
        }

        [Authorize(DocumentTypeKey.Config, OperationAction.View)]
        public MasterfileGridDataVo GetDataForGrid(QueryInfo queryInfo)
        {
            return _configService.GetDataForGridMasterFile(queryInfo);
        }

        protected override IList<ViewColumnViewModel> GetViewColumns()
        {
            return new List<ViewColumnViewModel>
            {
                new ViewColumnViewModel
                {
                    ColumnOrder = 1,
                    Name = "Name",
                    Text = "Name",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 2,
                    Name = "VideoName",
                    Text = "Video Name",
                    ColumnWidth = 200,
                    Mandatory = true,
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 3,
                    Name = "Description",
                    Text = "Description",
                    ColumnWidth = 450,
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 4,
                    Name = "Command",
                    Text = " ",
                    ColumnWidth = 100,
                    Sortable = false,
                    ColumnJustification = GridColumnJustification.Left,
                    CustomTemplate = "commandConfigTemplate"
                }
            };
        }

        [Authorize(DocumentTypeKey.Config, OperationAction.View)]
        public ActionResult Shared()
        {
            return View();
        }

        [Authorize(DocumentTypeKey.Config, OperationAction.View)]
        public ActionResult Update(int id)
        {
            var viewModel = GetMasterFileViewModel(id);
            return Json(viewModel.SharedViewModel);
        }

        [HttpPost]
        [Authorize(DocumentTypeKey.Config, OperationAction.Update)]
        public ActionResult Update([FromBody]ConfigParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);

            byte[] lastModified = null;

            if (ModelState.IsValid)
            {
                var entity = MasterFileService.GetById(viewModel.SharedViewModel.Id);
                var mappedEntity = viewModel.MapPropertiesToInstance(entity);
                lastModified = MasterFileService.Update(mappedEntity).LastModified;
                _configSystem.RefershListData();
                try
                {
                    using (var webClient = new WebClient())
                    {
                        var refreshLink = _configuration["ApiFrontEntUrl"] + "api/auth/refresh-config-system";
                        var data = webClient.DownloadString(refreshLink);
                    }
                }
                catch (Exception ex)
                {
                    _diagnosticService.Error(ex);
                }
            }

            return Json(new { Error = string.Empty, Data = new { LastModified = lastModified } });
        }
    }
}