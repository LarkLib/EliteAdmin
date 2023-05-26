using Infrastructure.Attribute;
using Infrastructure.Enums;
using Infrastructure.Model;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using ZR.Admin.WebApi.Filters;
using ZR.Common;
using Mapster;
using ZR.Admin.WebApi.Extensions;
using SqlSugar;
using ZR.Model.Dto;
using ZR.AdminService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ZR.Admin.WebApi.Controllers
{
    [ApiController]
    [Verify]
    [Route("dynamic/data")]
    public class DynamicController : BaseController
    {
        /// <summary>
        /// 管理接口
        /// </summary>
        private readonly IDynamicService _DynamicService;

        public DynamicController(IDynamicService dynamicService)
        {
            _DynamicService = dynamicService;
        }

        [HttpGet("getFiledConfigInfo")]
        [ActionPermissionFilter(Permission = "dynamic:fieldconfiginfo:list")]
        public IActionResult GetFiledConfigInfo([FromQuery] string tableName)
        {
            var response = _DynamicService.GetFiledConfigInfo(tableName);
            return SUCCESS(response);
        }

        [HttpGet("getkeyvalueGroup")]
        [ActionPermissionFilter(Permission = "dynamic:fieldconfiginfo:keyvaluegroup")]
        public IActionResult GetkeyvalueGroupByTableName([FromQuery] string tableName)
        {
            var response = _DynamicService.GetkeyvalueGroupByTableName(tableName);
            return SUCCESS(response);
        }

        [HttpPost("list")]
        [ActionPermissionFilter(Permission = "dynamic:datatable:list")]
        public IActionResult GetDataTable([FromBody] DynamicQueryDto parm)
        {
            var response = _DynamicService.GetDataTable(parm);
            //return SUCCESS(response, setCamelCaseProperty: false);
            return SUCCESS(response);
        }

        [HttpPost("listBySqlCode")]
        [ActionPermissionFilter(Permission = "dynamic:datatable:sqlcode")]
        public IActionResult GetDataTableBySqlCode([FromBody] DynamicQueryDto parm)
        {
            var response = _DynamicService.GetDataTableBySqlCode(parm);
            return SUCCESS(response);
        }

        [HttpPost("insert")]
        [ActionPermissionFilter(Permission = "dynamic:datatable:insert")]
        public IActionResult AddDynamicObject([FromBody] JObject parm)
        {
            //var response = _DynamicService.GetDataTableBySqlCode(parm);
            return SUCCESS(parm);
        }
    }
}
