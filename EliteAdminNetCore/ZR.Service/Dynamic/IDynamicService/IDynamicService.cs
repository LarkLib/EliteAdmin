using System.Collections.Generic;
using ZR.Service;
using System.Data;
using ZR.Model.Dto;
using ZR.Model.Elite;
using ZR.Model;
using SqlSugar;

namespace ZR.AdminService
{
    /// <summary>
    ///Dynamic管理service接口
    ///
    /// @author admin
    /// @date 2023-05-08
    /// </summary>
    public interface IDynamicService : IBaseService<BaseFieldConfig>
    {
        List<BaseFieldConfig> GetFiledConfigInfo(string tableName, long userId = 1);
        Dictionary<string, Dictionary<dynamic, dynamic>> GetkeyvalueGroupByTableName(string tableName);
        PagedInfo<dynamic> GetDataTable(DynamicQueryDto parm);
        DataTable GetDataTableData(DynamicQueryDto parm);
        PagedInfo<dynamic> GetDataTableBySqlCode(DynamicQueryDto parm);
        int AddDynamicObject(string tableName, Dictionary<string, object> parm);
        dynamic GetDynamicObjectById(string tableName, string idFieldName, string id);
        int UpdateDynamicObject(string tableName, string idFieldName, Dictionary<string, object> data);
        int DeleteDynamicObjec(string tableName, string idFieldName, int[] idsArr);
        string ExecutePalletDown(long storeCell, int downFlag, ref string message);
    }
}
