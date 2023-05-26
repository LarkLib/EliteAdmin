using System;
using ZR.Model;
using ZR.Model.Dto;
using ZR.Model.Elite;
using System.Collections.Generic;

namespace ZR.Service.Elite.IEliteService
{
    /// <summary>
    /// 字段信息配置service接口
    /// </summary>
    public interface IBaseFieldConfigService : IBaseService<BaseFieldConfig>
    {
        PagedInfo<BaseFieldConfigDto> GetList(BaseFieldConfigQueryDto parm);

        int AddBaseFieldConfig(BaseFieldConfig parm);

        int UpdateBaseFieldConfig(BaseFieldConfig parm);
    }
}
