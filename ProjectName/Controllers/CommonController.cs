using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interfaces;
using ProjectName.Attributes;
using ProjectName.Controllers.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectName.Controllers
{
    public class CommonController : ApplicationControllerBase
    {
        private readonly ITempUploadFileService _tempUploadFileService;
        private readonly IXmlDataHelpper _xmlDataHelper;
        public CommonController(ITempUploadFileService tempUploadFileService,
            IXmlDataHelpper xmlDataHelper)
        {
            _tempUploadFileService = tempUploadFileService;
            _tempUploadFileService.FilePath = "/UploadTemp/";
            _xmlDataHelper = xmlDataHelper;
        }

        [HttpPost]
        public async Task<ActionResult> SaveFileUpload([FromForm(Name = "files")]IFormFile file)
        {
            var fileName = new List<string>();
            var saveFile = await _tempUploadFileService.SaveFile(file);
            fileName.Add(saveFile);
            return Json(new { file = fileName, _tempUploadFileService.FilePath });
        }
        public async Task<ActionResult> SaveFileUploadWithType([FromForm(Name = "files")]IFormFile file, [FromQuery]int type)
        {
            var fileName = new List<string>();
            var saveFile = await _tempUploadFileService.SaveFile(file, type);
            fileName.Add(saveFile);
            var url = _configuration["ImageCdnUrl"];

            return Json(new { file = fileName, OriginalUrl = url, _tempUploadFileService.FilePath });
        }

        [HttpPost]
        public ActionResult RemoveFileUpload([FromBody]string fileNames)
        {
            _tempUploadFileService.RemoveFile(fileNames);

            return Ok();
        }

        [HttpGet]
        [Authorize(DocumentTypeKey.None, OperationAction.View)]
        public JsonResult BannerTypeLookup()
        {
            var data = _xmlDataHelper.GetData(XmlDataTypeEnum.BannerType.ToString()).Select(o => new { DisplayName = o.Value, KeyId = int.Parse(o.Key) });
            return Json(data);
        }

        [HttpGet]
        [Authorize(DocumentTypeKey.None, OperationAction.View)]
        public JsonResult CustomerRegisterTypeLookup()
        {
            var data = _xmlDataHelper.GetData(XmlDataTypeEnum.CustomerRegister.ToString()).Select(o => new { DisplayName = o.Value, KeyId = int.Parse(o.Key) });
            return Json(data);
        }
        [HttpPost]
        public ActionResult RemoveFileUpload([FromBody]FileNameWrap data)
        {
            _tempUploadFileService.RemoveFile(data.fileNames);

            return Ok();
        }
        public ActionResult RemoveFileUploadWithType([FromBody]FileNameWrap data, [FromQuery]int type)
        {
            _tempUploadFileService.RemoveFile(data.fileNames, type);

            return Ok();
        }
        [HttpPost]
        public ActionResult RemoveFileFake([FromBody]FileNameWrap data)
        {
            return Ok();
        }
    }
    public class FileNameWrap
    {
        public string fileNames { get; set; }
    }
}