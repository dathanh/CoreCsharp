using ClosedXML.Excel;
using Framework.DomainModel;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Exceptions;
using Framework.Exceptions.DataAccess.Sql;
using Framework.Mapping;
using Framework.Service.Diagnostics;
using Framework.Service.Translation;
using Framework.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServiceLayer.Authorization;
using ServiceLayer.Interfaces;
using ProjectName.Models.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectName.Controllers.Base
{
    /// <summary>
    /// This is the base controller of system
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public abstract class ApplicationControllerGeneric<TEntity, TViewModel> : Controller
        where TEntity : Entity
        where TViewModel : MasterfileViewModelBase<TEntity>, new()
    {
        protected readonly IDiagnosticService _diagnosticService;
        protected readonly IMasterFileService<TEntity> _masterfileService;
        protected readonly IOperationAuthorization _operationAuthorization;
        protected readonly IConfiguration _configuration;
        protected readonly IAuthenticationService _authenticationService;

        protected ApplicationControllerGeneric(IMasterFileService<TEntity> masterfileService,
                                          IOperationAuthorization operationAuthorization = null)
        {
            _diagnosticService = AppDependencyResolver.Current.GetService<IDiagnosticService>();
            _masterfileService = masterfileService;
            _operationAuthorization = operationAuthorization;
            _configuration = AppDependencyResolver.Current.GetService<IConfiguration>();
            _authenticationService = AppDependencyResolver.Current.GetService<IAuthenticationService>();
        }

        protected IMasterFileService<TEntity> MasterFileService => _masterfileService;
        protected IAuthenticationService AuthenticationService => _authenticationService;
        protected IDiagnosticService DiagnosticService => _diagnosticService;
        protected IOperationAuthorization OperationAuthorization => _operationAuthorization;

        public async Task<int> GetCurrentUserId()
        {
            var getCurrentUser = await AuthenticationService.GetCurrentUser();
            return getCurrentUser != null ? getCurrentUser.Id
               : -1;
        }

        public async Task<UserDto> GetCurrentUser()
        {
            return await AuthenticationService.GetCurrentUser();
        }
        /// <summary>
        /// Get data for lookup
        /// </summary>
        /// <param name="query"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public virtual List<LookupItemVo> GetLookupForEntity(LookupQuery query, Func<TEntity, LookupItemVo> selector)
        {
            return _masterfileService.GetLookup(query, selector);
        }

        /// <summary>
        /// Build grid
        /// </summary>
        /// <param name="gridConfigService"></param>
        /// <param name="initGridViewModel"></param>
        /// <returns></returns>
        public virtual async Task<GridViewModel> BuildGridViewModel(IGridConfigService gridConfigService, Func<GridViewModel> initGridViewModel = null, Func<List<ViewColumnViewModel>> getViewColumns = null)
        {
            var modelName = typeof(TEntity).Name;
            //my be this is a value object, not view model
            var gridViewModel = initGridViewModel != null ? initGridViewModel() : new GridViewModel
            {
                GridId = string.Format("{0}Grid", modelName),
                ModelName = modelName,
                //AdvancedSearchUrl = "~/Views/Shared/AdvancedSearch.cshtml"
            };
            Func<GridConfig, GridConfig> selector = g => g;

            var objGridConfigOriginal = gridConfigService.GetGridConfig(selector,
                await GetCurrentUserId(),
                gridViewModel.DocumentTypeId,
                gridViewModel.GridInternalName);
            var objGridConfig = objGridConfigOriginal.MapTo<GridConfigViewModel>();
            var objListColumnInGridConfig = new List<ViewColumnViewModel>();
            if (objGridConfig != null && objGridConfig.ViewColumns != null && objGridConfig.ViewColumns.Count != 0)
            {
                gridViewModel.Id = objGridConfig.Id;
                objListColumnInGridConfig = objGridConfig.ViewColumns.OrderBy(o => o.ColumnOrder).ToList();
            }
            IList<ViewColumnViewModel> defaultColumns;
            if (getViewColumns != null)
            {
                defaultColumns = getViewColumns();
            }
            else
            {
                defaultColumns = GetViewColumns();
            }
            foreach (var column in defaultColumns)
            {
                var configColumn = objListColumnInGridConfig.FirstOrDefault(o => o.Name == column.Name);
                if (configColumn != null)
                {
                    column.HideColumn = configColumn.HideColumn;
                    column.ColumnOrder = configColumn.ColumnOrder;
                    column.ColumnWidth = configColumn.ColumnWidth;
                }
            }

            if (string.IsNullOrWhiteSpace(gridViewModel.SearchPlaceholder))
            {
                gridViewModel.SearchPlaceholder = "Keyword.....";
            }
            gridViewModel.ViewColumns = defaultColumns.OrderBy(o => o.ColumnOrder).ToList();


            if (_operationAuthorization != null)
            {
                // Set property for CanUpdate, CanDelete and CanAdd
                // Get document type key from TEntity
                DocumentTypeKey documentTypeKey;
                try
                {
                    documentTypeKey = (DocumentTypeKey)Enum.Parse(typeof(DocumentTypeKey), modelName);
                }
                catch
                {
                    documentTypeKey = DocumentTypeKey.None;
                }


                if (documentTypeKey != DocumentTypeKey.None)
                {
                    var canAddItem = _operationAuthorization.VerifyAccess(documentTypeKey, OperationAction.Add, out _);
                    var canUpdateItem = _operationAuthorization.VerifyAccess(documentTypeKey, OperationAction.Update, out _);
                    var canDeleteItem = _operationAuthorization.VerifyAccess(documentTypeKey, OperationAction.Delete, out _);
                    gridViewModel.CanAddNewRecord = canAddItem;
                    gridViewModel.CanUpdateRecord = canUpdateItem;
                    gridViewModel.CanDeleteRecord = canDeleteItem;
                }
            }



            return gridViewModel;
        }

        /// <summary>
        /// Create list column in the grid
        /// </summary>
        /// <returns></returns>
        protected virtual IList<ViewColumnViewModel> GetViewColumns()
        {
            return null;
        }


        protected JsonSerializerSettings JsonSerializerSetting
        {
            get
            {
                var jSettings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                };
                jSettings.Converters.Add(new DefaultWrongFormatDeserialize());
                jSettings.Converters.Add(new IntergerWrongFormatDeserialize());
                return jSettings;
            }
        }

        protected virtual TViewModel MapFromClientParameters(MasterfileParameter parameters, Action<TViewModel> advanceMapping = null)
        {
            return MapFromClientParameters<TViewModel>(parameters, advanceMapping);
        }

        protected virtual TVModel MapFromClientParameters<TVModel>(MasterfileParameter parameters, Action<TVModel> advanceMapping = null)
            where TVModel : MasterfileViewModelBase<TEntity>, new()
        {
            var viewModel = new TVModel();

            viewModel.ProcessFromClientParameters(parameters);
            if (advanceMapping != null)
            {
                advanceMapping(viewModel);
            }

            return viewModel;
        }

        public virtual TViewModel GetMasterFileViewModel(int id)
        {
            var entity = MasterFileService.GetById(id);
            var viewModel = entity.MapTo<TViewModel>();
            return viewModel;
        }

        public virtual TEntity CreateMasterFile(MasterfileParameter parameters, Action<TViewModel> advanceMapping = null)
        {
            var viewModel = MapFromClientParameters(parameters);
            var entity = viewModel.MapTo<TEntity>();
            var savedEntity = MasterFileService.Add(entity);

            return savedEntity;
        }

        public virtual dynamic DeleteMultiMasterfile(string selectedRowIdArray, string isDeleteAll)
        {
            if (isDeleteAll == "1")
            {
                MasterFileService.DeleteAll(o => o.Id > 0);
            }
            else
            {
                var liststrId = selectedRowIdArray.Split(',');
                var listId = new List<int>();
                foreach (var item in liststrId)
                {
                    int.TryParse(item, out int id);
                    if (id != 0)
                    {
                        listId.Add(id);
                    }
                }
                MasterFileService.DeleteAll(o => listId.Contains(o.Id));
            }
            return new { Error = string.Empty };
        }

        public virtual MasterfileGridDataVo GetDataForGridMasterFile(QueryInfo queryInfo)
        {
            return MasterFileService.GetDataForGridMasterFile(queryInfo);
        }

        public virtual dynamic ExportExcelMasterfile(List<ColumnModel> gridConfig, QueryInfo queryInfo, string sheetName = "")
        {
            var data = new ExportExcel();
            var tempPath = Path.GetTempPath();
            var dataBind = MasterFileService.GetDataForGridMasterFile(queryInfo);

            string jsonTemp = JsonConvert.SerializeObject(dataBind);
            var dynamicTemp = JsonConvert.DeserializeObject<dynamic>(jsonTemp);
            string dataTemp = JsonConvert.SerializeObject(dynamicTemp.Data);
            var dataItem = JsonConvert.DeserializeObject<List<dynamic>>(dataTemp);

            data.GridConfigViewModel = gridConfig;
            data.ListDataSource = dataItem;

            using (var wb = new XLWorkbook())
            {
                string guid = Guid.NewGuid().ToString();
                string filePath = Path.Combine(tempPath, guid + ".xlsx");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                if (string.IsNullOrWhiteSpace(sheetName))
                {
                    sheetName = typeof(TEntity).Name;
                }
                wb.Worksheets.Add(GenDataTableFromExportExcelType(data), sheetName).ColumnsUsed().AdjustToContents();
                wb.SaveAs(filePath);
                return new { FileNameResult = guid + ".xlsx", Error = string.Empty };
            }

            //var content = RenderRazorViewToString("~/Views/Shared/Export/_BusinessReportExportContent.cshtml", data);
            //return Json(new { Item = content, Error = string.Empty }, JsonRequestBehavior.AllowGet);
        }

        public virtual FileResult DownloadExcelMasterFile(string fileName, string lastFileName)
        {
            var tempPath = Path.GetTempPath();
            string filePath = Path.Combine(tempPath, fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, lastFileName + ".xlsx");
        }


        protected DataTable GenDataTableFromExportExcelType(ExportExcel data)
        {
            var dt = new DataTable();

            var listColumn = new List<string>();
            foreach (var column in data.GridConfigViewModel)
            {
                if (!string.IsNullOrWhiteSpace(column.Field.Trim()) && !string.IsNullOrWhiteSpace(column.Title.Trim()))
                {
                    listColumn.Add(column.Field);
                }
                dt.Columns.Add(column.Title);
            }

            foreach (var item in data.ListDataSource)
            {
                var array = new object[listColumn.Count];
                for (int i = 0; i < listColumn.Count; i++)
                {
                    array[i] = item[listColumn[i]];
                }
                dt.Rows.Add(array);
            }
            return dt;
        }

        public virtual dynamic UpdateMasterFile(MasterfileParameter parameters, Action<TViewModel> advanceMapping = null)
        {
            var viewModel = MapFromClientParameters(parameters);

            if (advanceMapping != null)
            {
                advanceMapping.Invoke(viewModel);
            }

            byte[] lastModified = null;

            if (ModelState.IsValid)
            {
                var entity = MasterFileService.GetById(viewModel.SharedViewModel.Id);
                var mappedEntity = viewModel.MapPropertiesToInstance(entity);
                lastModified = MasterFileService.Update(mappedEntity).LastModified;
            }

            return new { Error = string.Empty, Data = new { LastModified = lastModified } };
        }

        public virtual dynamic DeleteMasterFile(TViewModel viewModel)
        {
            var entity = MasterFileService.GetById(viewModel.Id);
            MasterFileService.Delete(entity);

            return new { Error = string.Empty };
        }

        public virtual dynamic CheckExists(int id)
        {
            var entity = MasterFileService.GetById(id);
            return new { IsExists = entity != null };
        }
        protected dynamic HandleAjaxRequestException(Exception ex)
        {
            //get current error from view model state
            var errors = ViewData
                .ModelState
                .Values
                .SelectMany(v => v.Errors)
                .Select(v => v.ErrorMessage)
                .Distinct();

            var feedbackViewModel = BuildFeedBackViewModel(ex, errors);
            return feedbackViewModel;
        }

        protected FeedbackViewModel BuildFeedBackViewModel(Exception ex, IEnumerable<string> modelStateErrors)
        {
            var feedback = new FeedbackViewModel();

            var shouldRethrow = HandleException(ex, out ExceptionHandlingResult exceptionHandlingResult);

            feedback.Status = shouldRethrow ? FeedbackStatus.Critical : FeedbackStatus.Error;

            feedback.Error = exceptionHandlingResult.ErrorMessage;
            feedback.AddModelStateErrors(modelStateErrors.ToArray());

            //add more exception from exception stack trace
            feedback.AddModelStateErrors(exceptionHandlingResult.ModelStateErrors.ToArray());
            feedback.InnerException = exceptionHandlingResult.InnerException;
            return feedback;
        }
        /// <summary>
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="exceptionHandlingResult"></param>
        /// <returns></returns>
        protected bool HandleException(Exception ex, out ExceptionHandlingResult exceptionHandlingResult)
        {
            exceptionHandlingResult = new ExceptionHandlingResult();
            bool shouldRethrow;

            _diagnosticService.Error(ex);
            _diagnosticService.Error(ex.StackTrace);
            var isProductionMode = IsProductionMode;

            var commonErrorMessage = SystemMessageLookup.GetMessage("GeneralExceptionMessageText");

            //  if production mode then show generic error
            if (isProductionMode)
            {
                exceptionHandlingResult.ErrorMessage = commonErrorMessage;
                exceptionHandlingResult.StackTrace = string.Empty;
            }
            else //  else: show all exception
            {
                exceptionHandlingResult.ErrorMessage = ex.Message;
                exceptionHandlingResult.StackTrace = ex.StackTrace;
            }
            var innerError = ex.InnerException;
            if (innerError != null && !isProductionMode)
            {
                // Check if the error message not is the message from error in the entity framework( this is the error that we can handle, not show to user)
                if (innerError.Message !=
                    "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    exceptionHandlingResult.AddModelStateErrors(innerError.Message);
                    //get innerException
                    if (innerError.InnerException != null)
                    {
                        exceptionHandlingResult.AddInnerException(innerError.InnerException.Message);
                        if (innerError.InnerException.InnerException != null)
                        {
                            exceptionHandlingResult.AddInnerException(innerError.InnerException.InnerException.Message);
                        }
                    }
                }
            }
            if (ex is BusinessLogicException)
            {
                //all business exception be showed to client
                exceptionHandlingResult.ErrorMessage = ex.Message;
                shouldRethrow = false;
            }
            else if (ex is UserVisibleException)
            {
                //exception has been transformed
                exceptionHandlingResult.ErrorMessage = ex.Message;

                shouldRethrow = true;
            }
            else if (ex is DataBadSqlException)
            {
                shouldRethrow = false;
            }
            else if (ex is DataCannotSerializeTransactionException)
            {
                shouldRethrow = false;
            }
            else if (ex is DataDeadlockException)
            {
                shouldRethrow = false;
            }
            else if (ex is DataIntegrityViolationException)
            {
                shouldRethrow = false;
            }
            else if (ex is DataLockingFailureException)
            {
                //exceptionHandlingResult.ErrorMessage =
                //    TranslationService.TranslateString("ConcurrencyExceptionMessageText");

                shouldRethrow = false;
            }

            else if (ex is DataObjectRetrievalFailureException)
            {
                shouldRethrow = false;
            }
            else if (ex is DataPermissionDeniedException)
            {
                shouldRethrow = true;
            }
            else if (ex is DataAccessException)
            {
                shouldRethrow = true;
            }
            else //all other exception
            {
                shouldRethrow = true;
            }

            return shouldRethrow;
        }
        /// <summary>
        ///     Get deployment mode of application, it can  be developement or production
        /// </summary>
        public virtual bool IsProductionMode
        {
            get
            {
                var applicationMode = _configuration["ApplicationMode"];
                return !string.IsNullOrWhiteSpace(applicationMode) &&
                       String.CompareOrdinal("production", applicationMode.ToLower().Trim()) == 0;
            }
        }
    }
}
