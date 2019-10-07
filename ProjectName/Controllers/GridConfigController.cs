using Framework.DomainModel.Entities;
using Framework.Mapping;
using Framework.Service.Translation;
using Framework.Utility;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interfaces;
using ProjectName.Controllers.Base;
using ProjectName.Models.Base;
using System;
using System.Linq;

namespace ProjectName.Controllers
{
    public class GridConfigController : ApplicationControllerBase
    {
        //
        // GET: /GridConfig/
        private readonly IGridConfigService _gridConfigService;
        public GridConfigController(IGridConfigService gridConfigService)
        {
            _gridConfigService = gridConfigService;
        }

        [HttpPost]
        public ActionResult Save([FromBody]GridConfigViewModel viewModel)
        {
            if (viewModel == null)
            {
                return
                    Json(
                        new
                        {
                            Error = SystemMessageLookup.GetMessage("InvalidData"),
                        });
            }

            //check if gird not have Mandatory columns are selected anymore
            if (viewModel.ViewColumns.Where(o => !o.Mandatory).Count(o => (!o.HideColumn)) <= 0)
            {
                return
                Json(
                    new
                    {
                        Error = SystemMessageLookup.GetMessage("HideAllColumn"),
                    });
            }

            var gridConfig = _gridConfigService.FirstOrDefault(x => x.Id == viewModel.Id);

            if (gridConfig != null && gridConfig.UserId == 0)
            {
                gridConfig = null;
                viewModel.Id = 0;
            }

            gridConfig = viewModel.MapPropertiesToInstance(gridConfig);
            _gridConfigService.InsertOrUpdate(gridConfig);

            return
               Json(
                   new
                   {
                       Error = string.Empty,
                       Data = new { gridConfig.Id }
                   });
        }

        [HttpPost]
        public JsonResult Get([FromBody]GridConfigViewModel viewModel)
        {
            if (viewModel == null)
            {
                return Json(new GridConfigViewModel());
            }

            Func<GridConfig, GridConfigViewModel> selector = g => g.MapTo<GridConfigViewModel>();

            var gridConfig = _gridConfigService.GetGridConfig(selector,
                                                            viewModel.UserId,
                                                            viewModel.DocumentTypeId,
                                                            viewModel.GridInternalName);
            var xml = SerializationHelper.SerializeToXml(gridConfig);
            var gridConfigJson = Json(gridConfig);

            return gridConfigJson;
        }

    }
}
