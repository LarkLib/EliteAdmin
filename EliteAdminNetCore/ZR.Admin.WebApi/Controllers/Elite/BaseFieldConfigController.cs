using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Enums;
using Infrastructure.Model;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using ZR.Model.Dto;
using ZR.Model.Elite;
using ZR.Service.Elite.IEliteService;
using ZR.Admin.WebApi.Extensions;
using ZR.Admin.WebApi.Filters;
using ZR.Common;

//创建时间：2023-05-23
namespace ZR.Admin.WebApi.Controllers
{
    /// <summary>
    /// 字段信息配置
    /// </summary>
    [Verify]
    [Route("elite/BaseFieldConfig")]
    public class BaseFieldConfigController : BaseController
    {
        /// <summary>
        /// 字段信息配置接口
        /// </summary>
        private readonly IBaseFieldConfigService _BaseFieldConfigService;

        public BaseFieldConfigController(IBaseFieldConfigService BaseFieldConfigService)
        {
            _BaseFieldConfigService = BaseFieldConfigService;
        }

        /// <summary>
        /// 查询字段信息配置列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "task:basefieldconfig:list")]
        public IActionResult QueryBaseFieldConfig([FromQuery] BaseFieldConfigQueryDto parm)
        {
            var response = _BaseFieldConfigService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询字段信息配置详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "task:basefieldconfig:query")]
        public IActionResult GetBaseFieldConfig(int Id)
        {
            var response = _BaseFieldConfigService.GetFirst(x => x.Id == Id);
            
            return SUCCESS(response);
        }

        /// <summary>
        /// 添加字段信息配置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "task:basefieldconfig:add")]
        [Log(Title = "字段信息配置", BusinessType = BusinessType.INSERT)]
        public IActionResult AddBaseFieldConfig([FromBody] BaseFieldConfigDto parm)
        {
            if (parm == null)
            {
                throw new CustomException("请求参数错误");
            }
            var modal = parm.Adapt<BaseFieldConfig>().ToCreate(HttpContext);

            var response = _BaseFieldConfigService.AddBaseFieldConfig(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 更新字段信息配置
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "task:basefieldconfig:edit")]
        [Log(Title = "字段信息配置", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateBaseFieldConfig([FromBody] BaseFieldConfigDto parm)
        {
            if (parm == null)
            {
                throw new CustomException("请求实体不能为空");
            }
            var modal = parm.Adapt<BaseFieldConfig>().ToUpdate(HttpContext);

            var response = _BaseFieldConfigService.UpdateBaseFieldConfig(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除字段信息配置
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "task:basefieldconfig:delete")]
        [Log(Title = "字段信息配置", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteBaseFieldConfig(string ids)
        {
            int[] idsArr = Tools.SpitIntArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }

            var response = _BaseFieldConfigService.Delete(idsArr);

            return ToResponse(response);
        }

        /// <summary>
        /// 导出字段信息配置
        /// </summary>
        /// <returns></returns>
        [Log(Title = "字段信息配置", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "task:basefieldconfig:export")]
        public IActionResult Export([FromQuery] BaseFieldConfigQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _BaseFieldConfigService.GetList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "字段信息配置", "字段信息配置");
            return ExportExcel(result.Item2, result.Item1);
        }


    }
}