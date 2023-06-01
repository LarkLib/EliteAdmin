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
using Org.BouncyCastle.Crypto;

namespace ZR.Admin.WebApi.Controllers
{
    [ApiController]
    [Verify]
    [Route("dynamic/data")]
    public class DynamicController : BaseController
    {
        /// <summary>
        /// 通用管理接口
        /// </summary>
        private readonly IDynamicService _DynamicService;

        public DynamicController(IDynamicService dynamicService)
        {
            _DynamicService = dynamicService;
        }

        /// <summary>
        /// 按表名查询字段配置信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        [HttpGet("getFiledConfigInfo")]
        [ActionPermissionFilter(Permission = "dynamic:fieldconfiginfo:list")]
        public IActionResult GetFiledConfigInfo([FromQuery] string tableName)
        {
            var response = _DynamicService.GetFiledConfigInfo(tableName);
            return SUCCESS(response);
        }

        /// <summary>
        /// 查询字段信息配置的sql语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        [HttpGet("getkeyvalueGroup")]
        [ActionPermissionFilter(Permission = "dynamic:fieldconfiginfo:keyvaluegroup")]
        public IActionResult GetkeyvalueGroupByTableName([FromQuery] string tableName)
        {
            var response = _DynamicService.GetkeyvalueGroupByTableName(tableName);
            return SUCCESS(response);
        }

        /// <summary>
        /// 按表名查询数据
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost("list")]
        [ActionPermissionFilter(Permission = "dynamic:datatable:list")]
        public IActionResult GetDataTable([FromBody] DynamicQueryDto parm)
        {
            var response = _DynamicService.GetDataTable(parm);
            //return SUCCESS(response, setCamelCaseProperty: false);
            return SUCCESS(response);
        }

        /// <summary>
        /// 按sql语句查询数据
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost("listBySqlCode")]
        [ActionPermissionFilter(Permission = "dynamic:datatable:sqlcode")]
        public IActionResult GetDataTableBySqlCode([FromBody] DynamicQueryDto parm)
        {
            var response = _DynamicService.GetDataTableBySqlCode(parm);
            return SUCCESS(response);
        }

        /// <summary>
        /// 按钟表插入数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost("insert")]
        [ActionPermissionFilter(Permission = "dynamic:datatable:insert")]
        public int AddDynamicObject([FromQuery] string tableName, [FromBody] Dictionary<string, object> parm)
        {
            return _DynamicService.AddDynamicObject(tableName, parm);
        }

        /// <summary>
        /// 按Id查询单条记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="idFieldName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getDynamicObjectById")]
        [ActionPermissionFilter(Permission = "dynamic:datatable:geyById")]
        public IActionResult GetDynamicObjectById([FromQuery] string tableName, string idFieldName, string id)
        {
            var response = _DynamicService.GetDynamicObjectById(tableName, idFieldName, id);
            return SUCCESS(response);
        }

        /// <summary>
        /// 按表名表更新数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="idFieldName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("updateDynamicObject")]
        [ActionPermissionFilter(Permission = "dynamic:datatable:update")]
        public IActionResult UpdateDynamicObject([FromQuery] string tableName, [FromQuery] string idFieldName, [FromBody] Dictionary<string, object> data)
        {
            var response = _DynamicService.UpdateDynamicObject(tableName, idFieldName, data);
            return SUCCESS(response);
        }

        /// <summary>
        /// 按Id删除指定表中的数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="idFieldName"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete("deleteDynamicObjec")]
        [ActionPermissionFilter(Permission = "dynamic:datatable:delete")]
        public IActionResult DeleteDynamicObjec([FromQuery] string tableName, [FromQuery] string idFieldName, string ids)
        {
            int[] idsArr = Tools.SpitIntArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }
            var response = _DynamicService.DeleteDynamicObjec(tableName, idFieldName, idsArr);
            return SUCCESS(response);
        }
    }
}
