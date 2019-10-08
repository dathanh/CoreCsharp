using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Mapping;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Authorization;
using ServiceLayer.Interfaces;
using VingID.Attributes;
using VingID.Controllers.Base;
using VingID.Models.Base;
using VingID.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VingID.Controllers
{
    public class CustomerController : ApplicationControllerGeneric<Customer, DashboardCustomerDataViewModel>
    {
        private readonly ICustomerService _Service;
        private readonly IGridConfigService _gridConfigService;

        public CustomerController(ICustomerService CustomerService, IGridConfigService gridConfigService, IOperationAuthorization operationAuthorization)
            : base(CustomerService, operationAuthorization)
        {
            _Service = CustomerService;
            _gridConfigService = gridConfigService;
        }

        // GET: Customer
        [Authorize(DocumentTypeKey.Customer, OperationAction.View)]
        public async Task<ActionResult> Index()
        {
            var canAddRecord = OperationAuthorization.VerifyAccess(DocumentTypeKey.Customer, OperationAction.Add, out List<UserRoleFunction> userRoleFunctions);
            var viewModel = new DashboardCustomerIndexViewModel
            {
                CanAddNewRecord = canAddRecord
            };

            Func<GridViewModel> gridViewModel = () => new GridViewModel
            {
                GridId = "CustomerGrid",
                ModelName = "Customer",
                DocumentTypeId = (int)DocumentTypeKey.Customer,
                GridInternalName = "Customer",
                ActionDefaultWidthColumn = 150,
                UseActionDefaultColumn = true,
                AddFunction = "vm.add",
                UpdateFunction = "vm.edit",
            };

            viewModel.GridViewModel = await BuildGridViewModel(_gridConfigService, gridViewModel);
            return View(viewModel);
        }

        [Authorize(DocumentTypeKey.Customer, OperationAction.View)]
        public MasterfileGridDataVo GetDataForGrid(QueryInfo queryInfo)
        {
            return _Service.GetDataForGridMasterFile(queryInfo);
        }

        protected override IList<ViewColumnViewModel> GetViewColumns()
        {
            return new List<ViewColumnViewModel>

            {
                new ViewColumnViewModel
                {
                    ColumnOrder = 1,
                    ColumnWidth = 100,
                    Name = "FullName",
                    Text = "FullName",
                    ColumnJustification = GridColumnJustification.Left,
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 1,
                    ColumnWidth = 100,
                    Name = "isActive",
                    Text = "isActive",
                    ColumnJustification = GridColumnJustification.Left,
                    Sortable = false,
                    CustomTemplate = "isActiveTemplate",
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 1,
                    ColumnWidth = 100,
                    Name = "Phone",
                    Text = "Phone",
                    ColumnJustification = GridColumnJustification.Left,
                },
            };
        }

        [Authorize(DocumentTypeKey.Customer, OperationAction.View)]
        public ActionResult Shared()
        {
            return View();
        }

        [HttpPost]
        [Authorize(DocumentTypeKey.Customer, OperationAction.Add)]
        public int Create([FromBody]CustomerParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);
            var entity = viewModel.MapTo<Customer>();
            return _Service.Add(entity).Id;
        }

        [Authorize(DocumentTypeKey.Customer, OperationAction.View)]
        public ActionResult Update(int id)
        {
            var viewModel = GetMasterFileViewModel(id);
            return Json(viewModel.SharedViewModel);
        }

        [HttpPost]
        [Authorize(DocumentTypeKey.Customer, OperationAction.Update)]
        public ActionResult Update([FromBody]CustomerParameter parameters)
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

        [Authorize(DocumentTypeKey.Customer, OperationAction.Delete)]
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            MasterFileService.DeleteById(id);
            return Json(true);
        }

        public List<LookupItemVo> GetLookup(LookupQuery queryInfo)
        {
            var selector = new Func<Customer, LookupItemVo>(o => new LookupItemVo
            {
                KeyId = o.Id,
                DisplayName = o.Name
            });
            return GetLookupForEntity(queryInfo, selector);
        }

    }
}