using System;
using SqlSugar;
using Infrastructure.Attribute;
using ZR.Model;
using ZR.Model.Dto;
using ZR.Model.Elite;
using ZR.Repository;
using ZR.Service.Elite.IEliteService;
using System.Linq;

namespace ZR.Service.Elite
{
    /// <summary>
    /// 字段信息配置Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IBaseFieldConfigService), ServiceLifetime = LifeTime.Transient)]
    public class BaseFieldConfigService : BaseService<BaseFieldConfig>, IBaseFieldConfigService
    {
        /// <summary>
        /// 查询字段信息配置列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<BaseFieldConfigDto> GetList(BaseFieldConfigQueryDto parm)
        {
            var predicate = Expressionable.Create<BaseFieldConfig>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.TableName), it => it.TableName.Contains(parm.TableName));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.Name), it => it.Name.Contains(parm.Name));
            var response = Queryable()
                .Where(predicate.ToExpression())
                .ToPage<BaseFieldConfig, BaseFieldConfigDto>(parm);

            return response;
        }


        /// <summary>
        /// 添加字段信息配置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddBaseFieldConfig(BaseFieldConfig model)
        {
            return Add(model, true);
        }

        /// <summary>
        /// 修改字段信息配置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateBaseFieldConfig(BaseFieldConfig model)
        {
            //var response = Update(w => w.Id == model.Id, it => new BaseFieldConfig()
            //{
            //    TableName = model.TableName,
            //    Name = model.Name,
            //    Caption = model.Caption,
            //    Control = model.Control,
            //    DataType = model.DataType,
            //    MaxLenght = model.MaxLenght,
            //    NumPrecisionRadix = model.NumPrecisionRadix,
            //    FormatString = model.FormatString,
            //    DataSource = model.DataSource,
            //    WithBlank = model.WithBlank,
            //    BlankValue = model.BlankValue,
            //    BlankName = model.BlankName,
            //    ErrorMessage = model.ErrorMessage,
            //});
            //return response;
            return Update(model, true);
        }
    }
}