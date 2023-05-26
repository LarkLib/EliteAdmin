using System;
using SqlSugar;
using System.Collections.Generic;

namespace ZR.Model.Elite
{
    /// <summary>
    /// 字段信息配置
    /// </summary>
    [SugarTable("T_Base_FieldConfig")]
    public class BaseFieldConfig
    {
        /// <summary>
        /// 编号 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 表名 
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 名称 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标题 
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// 序号 
        /// </summary>
        public int? GridIndex { get; set; }

        /// <summary>
        /// 是否可编辑 
        /// </summary>
        public bool GridEditable { get; set; }

        /// <summary>
        /// 宽度 
        /// </summary>
        public int? GridWidth { get; set; }

        /// <summary>
        /// 控件 
        /// </summary>
        public string Control { get; set; }

        /// <summary>
        /// 数据类型 
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 最大长度 
        /// </summary>
        public int? MaxLenght { get; set; }

        /// <summary>
        /// 小数点位数 
        /// </summary>
        public int? NumPrecisionRadix { get; set; }

        /// <summary>
        /// 格式化 
        /// </summary>
        public string FormatString { get; set; }

        /// <summary>
        /// 验证操作 
        /// </summary>
        public string ValidationOperator { get; set; }

        /// <summary>
        /// 验证值1 
        /// </summary>
        public string ValidationValue1 { get; set; }

        /// <summary>
        /// 验证值2 
        /// </summary>
        public string ValidationValue2 { get; set; }

        /// <summary>
        /// 数据源 
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// 是否有空选项 
        /// </summary>
        public string WithBlank { get; set; }

        /// <summary>
        /// 空选项默认值 
        /// </summary>
        public string BlankValue { get; set; }

        /// <summary>
        /// 空选项名称 
        /// </summary>
        public string BlankName { get; set; }

        /// <summary>
        /// 错误消息 
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 是否显示 
        /// </summary>
        public bool Visible { get; set; }
    }
} 