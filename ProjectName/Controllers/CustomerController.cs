using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Authorization;
using ServiceLayer.Interfaces;
using ProjectName.Attributes;
using ProjectName.Controllers.Base;
using ProjectName.Models.Base;
using ProjectName.Models.Customer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectName.Controllers
{
    public class CustomerController : ApplicationControllerGeneric<Customer, DashboardCustomerDataViewModel>
    {
        private readonly ICustomerService _customerService;
        private readonly IGridConfigService _gridConfigService;

        public CustomerController(ICustomerService customerService, IGridConfigService gridConfigService,
                                IOperationAuthorization operationAuthorization)
            : base(customerService, operationAuthorization)
        {
            _customerService = customerService;
            _gridConfigService = gridConfigService;
        }

        // GET: Customer
        [Authorize(DocumentTypeKey.Customer, OperationAction.View)]
        public async Task<ActionResult> Index()
        {
            var viewModel = new DashboardCustomerIndexViewModel();

            Func<GridViewModel> gridViewModel = () => new GridViewModel
            {
                GridId = "CustomerGrid",
                ModelName = "Customer",
                DocumentTypeId = (int)DocumentTypeKey.Customer,
                GridInternalName = "Customer",
                ActionDefaultWidthColumn = 150,
                UseActionDefaultColumn = false,
                CustomHeaderTemplate = "customerFilter",
                CustomParameters = new List<string> { "Type", "RegisterStartDate", "RegisterEndDate" }
            };

            viewModel.GridViewModel = await BuildGridViewModel(_gridConfigService, gridViewModel);
            return View(viewModel);
        }

        [Authorize(DocumentTypeKey.Customer, OperationAction.View)]
        public MasterfileGridDataVo GetDataForGrid(CustomerQueryInfo queryInfo)
        {
            return _customerService.GetDataForGridMasterFile(queryInfo);
        }

        protected override IList<ViewColumnViewModel> GetViewColumns()
        {
            return new List<ViewColumnViewModel>
            {
                new ViewColumnViewModel
                {
                    ColumnOrder = 1,
                    Name = "UserName",
                    Text = "UserName",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 2,
                    Name = "Email",
                    Text = "Email",
                    ColumnWidth = 200,
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 3,
                    Name = "FullName",
                    Text = "FullName",
                    ColumnWidth = 150,
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 3,
                    Name = "Type",
                    Text = "Register from",
                    ColumnWidth = 150,
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 4,
                    Name = "CreatedDateStr",
                    Text = "Register date",
                    ColumnWidth = 150,
                    ColumnJustification = GridColumnJustification.Left
                },
            };
        }
        [Authorize(DocumentTypeKey.Customer, OperationAction.View)]
        public JsonResult ExportExcel(List<ColumnModel> gridColumns, CustomerQueryInfo queryInfo)
        {
            return ExportExcelMasterfile(gridColumns, queryInfo, "registers");
        }

        [Authorize(DocumentTypeKey.Customer, OperationAction.View)]
        public FileResult DownloadExcelFile(string fileName)
        {
            return DownloadExcelMasterFile(fileName, "registers");
        }
    }
}