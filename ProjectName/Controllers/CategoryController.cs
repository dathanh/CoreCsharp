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
using ProjectName.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectName.Controllers
{
    public class CategoryController : ApplicationControllerGeneric<Category, DashboardCategoryDataViewModel>
    {
        private readonly ICategoryService _categoryService;
        private readonly IGridConfigService _gridConfigService;

        public CategoryController(ICategoryService categoryService, IGridConfigService gridConfigService, IOperationAuthorization operationAuthorization)
            : base(categoryService, operationAuthorization)
        {
            _categoryService = categoryService;
            _gridConfigService = gridConfigService;
        }

        // GET: Category
        [Authorize(DocumentTypeKey.Category, OperationAction.View)]
        public async Task<ActionResult> Index()
        {
            var viewModel = new DashboardCategoryIndexViewModel();

            Func<GridViewModel> gridViewModel = () => new GridViewModel
            {
                GridId = "CategoryGrid",
                ModelName = "Category",
                DocumentTypeId = (int)DocumentTypeKey.Category,
                GridInternalName = "Category",
                ActionDefaultWidthColumn = 150,
                UseActionDefaultColumn = true,
                AddFunction = "vm.add",
                UpdateFunction = "vm.edit",
            };

            viewModel.GridViewModel = await BuildGridViewModel(_gridConfigService, gridViewModel);
            return View(viewModel);
        }

        [Authorize(DocumentTypeKey.Category, OperationAction.View)]
        public MasterfileGridDataVo GetDataForGrid(QueryInfo queryInfo)
        {
            return _categoryService.GetDataForGridMasterFile(queryInfo);
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
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 3,
                    ColumnWidth = 125,
                    Name = "ParentName",
                    Text = "Category Parent",
                    Sortable = false,
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

        [Authorize(DocumentTypeKey.Category, OperationAction.View)]
        public ActionResult Shared()
        {
            return View();
        }

        [HttpPost]
        [Authorize(DocumentTypeKey.Category, OperationAction.Add)]
        public int Create([FromBody]CategoryParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);
            var entity = viewModel.MapTo<Category>();
            return _categoryService.Add(entity).Id;
        }

        [Authorize(DocumentTypeKey.Category, OperationAction.View)]
        public ActionResult Update(int id)
        {
            var viewModel = GetMasterFileViewModel(id);
            return Json(viewModel.SharedViewModel);
        }

        [HttpPost]
        [Authorize(DocumentTypeKey.Category, OperationAction.Update)]
        public ActionResult Update([FromBody]CategoryParameter parameters)
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

        [Authorize(DocumentTypeKey.Category, OperationAction.Delete)]
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            MasterFileService.DeleteById(id);
            return Json(true);
        }

        [Authorize(DocumentTypeKey.Category, OperationAction.View)]
        public JsonResult GetParentLookup(LookupQuery queryInfo)
        {
            var listData = _categoryService.Get(o => o.ParentId == null).OrderBy(o => o.Name);
            var result = listData.Select(o => new { DisplayName = o.Name, KeyId = o.Id });
            return Json(result);
        }

        [Authorize(DocumentTypeKey.Category, OperationAction.View)]
        public JsonResult GetListSubCategory(QueryInfoWithParams queryInfo)
        {
            List<Category> data;
            if (!string.IsNullOrWhiteSpace(queryInfo.ParameterDependencies))
            {
                int.TryParse(queryInfo.ParameterDependencies, out int parentCategoryId);
                if (parentCategoryId == 0)
                {
                    data = new List<Category>();
                }
                else
                {
                    data = _categoryService.Get(o => o.ParentId == parentCategoryId).OrderBy(o => o.Name).ToList();
                }
            }
            else
            {
                data = new List<Category>();
            }
            var result = data.Select(o => new { DisplayName = o.Name, KeyId = o.Id });
            return Json(result);
        }

        [Authorize(DocumentTypeKey.Category, OperationAction.View)]
        public JsonResult GetListCategories()
        {
            var data = _categoryService.ListAll();
            var result = data.Select(o => new LookupItemVo { DisplayName = o.Name, KeyId = o.Id }).ToList();
            return Json(result);
        }
    }
}