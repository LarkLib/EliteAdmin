using System;
using SqlSugar;
using Infrastructure.Attribute;
using System.Linq;
using ZR.Service;
using System.Collections.Generic;
using System.Data;
using ZR.Model.Dto;
using ZR.Model.Elite;
using ZR.Repository;
using ZR.Model;
using MiniExcelLibs;
using Infrastructure.Extensions;
using Aliyun.OSS;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZR.AdminService
{
    /// <summary>
    /// Dynamic Service业务层处理
    ///
    /// @author admin
    /// @date 2023-05-08
    /// </summary>
    [AppService(ServiceType = typeof(IDynamicService), ServiceLifetime = LifeTime.Transient)]
    public partial class DynamicService : BaseService<BaseFieldConfig>, IDynamicService
    {
        #region 业务逻辑代码
        public List<BaseFieldConfig> GetFiledConfigInfo(string tableName, long userId = 1)
        {
            //var conModels = new List<IConditionalModel>();
            //conModels.Add(new ConditionalModel { FieldName = "tableName", ConditionalType = ConditionalType.Equal, FieldValue = tableName });

            //搜索条件查询语法参考Sqlsugar
            var response = Queryable()
                .Where("tableName", "=", tableName)
                .OrderBy("gridIndex")
                .ToList();
            return response;
        }

        public Dictionary<string, Dictionary<dynamic, dynamic>> GetkeyvalueGroupByTableName(string tableName)
        {
            var dict = new Dictionary<string, Dictionary<dynamic, dynamic>>();
            //搜索条件查询语法参考Sqlsugar
            var fields = Queryable()
                .Where(info => info.TableName.Equals(tableName, StringComparison.InvariantCultureIgnoreCase) && info.Control.Equals("LookUpEdit", StringComparison.InvariantCultureIgnoreCase))?
                .ToList();
            foreach (var field in fields)
            {
                var sql = field.DataSource;
                if (sql == null) continue;
                if (!sql.Contains("select", StringComparison.InvariantCultureIgnoreCase))
                {
                    sql = $"select dictValue VALUE,dictLabel NAME from sys_dict_data where dictType='{sql}' order by dictSort";
                }
                var kv = base.Context.Ado.SqlQuery<dynamic>(sql).ToDictionary(k => k.VALUE, k => k.NAME);
                dict[field.Name] = kv;
            }
            //return response;
            return dict;
        }

        public PagedInfo<dynamic> GetDataTable(DynamicQueryDto parm)
        {
            var conModels = new List<IConditionalModel>();
            var conditions = parm.Conditions;
            foreach (var condition in conditions)
            {
                if (condition!.FieldValue.IsNotEmpty()) conModels.Add(new ConditionalModel { FieldName = condition.FieldName, ConditionalType = condition.ConditionalType, FieldValue = condition.FieldValue });
            }
            var source = base.Context.Queryable<dynamic>().AS(parm.QueryCode)
                .Where(conModels);
            return source.ToPage<dynamic>(parm);
            //return base.Context.Queryable<dynamic>().AS(parm.TableName)
            //    .Where(conModels)
            //    .ToPageList(parm.PageNum, parm.PageSize, ref total);
        }

        public PagedInfo<dynamic> GetDataTableBySqlCode(DynamicQueryDto parm)
        {
            var conModels = new List<IConditionalModel>();
            var conditions = parm.Conditions;
            foreach (var condition in conditions)
            {
                conModels.Add(new ConditionalModel { FieldName = condition.FieldName, ConditionalType = ConditionalType.Equal, FieldValue = condition.FieldValue });
            }
            var sql = Sqls[parm.QueryCode];
            var source = base.Context.SqlQueryable<dynamic>(sql).Where(conModels);
            return source?.ToPage<dynamic>(parm);
        }

        public int AddDynamicObject(string tableName, Dictionary<string, object> parm)
        {
            return base.Context.InsertableByDynamic(parm)
                .AS(tableName)
                .ExecuteCommand(); ;
        }

        public dynamic GetDynamicObjectById(string tableName, string idFieldName, string id)
        {
            return base.Context.Queryable<dynamic>().AS(tableName)
                .Where(idFieldName, "=", id)
                .First();
        }

        public int UpdateDynamicObject(string tableName, string idFieldName, Dictionary<string, object> data)
        {
            return base.Context.UpdateableByDynamic(data)
                .AS(tableName)
                .WhereColumns(idFieldName)
                .ExecuteCommand();
        }

        public int DeleteDynamicObjec(string tableName, string idFieldName, int[] idsArr)
        {
            return base.Context.Deleteable<object>()
                .AS(tableName)
                .Where($"{idFieldName} in (@ids) ", new { ids = idsArr })
                .ExecuteCommand();
        }
        #endregion
    }
}