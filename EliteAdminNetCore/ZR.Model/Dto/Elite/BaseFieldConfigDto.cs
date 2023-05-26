using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
//using ZR.Model.Elite;
using MiniExcelLibs.Attributes;

namespace ZR.Model.Dto
{
    /// <summary>
    /// 字段信息配置查询对象
    /// </summary>
    public class BaseFieldConfigQueryDto : PagerInfo 
    {
        public string TableName { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// 字段信息配置输入输出对象
    /// </summary>
    public class BaseFieldConfigDto
    {
        [Required(ErrorMessage = "编号不能为空")]
        [ExcelColumn(Name = "编号")]
        public int Id { get; set; }

        [Required(ErrorMessage = "表名不能为空")]
        [ExcelColumn(Name = "表名")]
        public string TableName { get; set; }

        [Required(ErrorMessage = "名称不能为空")]
        [ExcelColumn(Name = "名称")]
        public string Name { get; set; }

        [ExcelColumn(Name = "标题")]
        public string Caption { get; set; }

        [ExcelColumn(Name = "序号")]
        public int? GridIndex { get; set; }

        [ExcelColumn(Name = "是否可编辑")]
        public bool GridEditable { get; set; }

        [ExcelColumn(Name = "宽度")]
        public int? GridWidth { get; set; }

        [Required(ErrorMessage = "控件不能为空")]
        [ExcelColumn(Name = "控件")]
        public string Control { get; set; }

        [ExcelColumn(Name = "数据类型")]
        public string DataType { get; set; }

        [ExcelColumn(Name = "最大长度")]
        public int? MaxLenght { get; set; }

        [ExcelColumn(Name = "小数点位数")]
        public int? NumPrecisionRadix { get; set; }

        [ExcelColumn(Name = "格式化")]
        public string FormatString { get; set; }

        [ExcelColumn(Name = "验证操作")]
        public string ValidationOperator { get; set; }

        [ExcelColumn(Name = "验证值1")]
        public string ValidationValue1 { get; set; }

        [ExcelColumn(Name = "验证值2")]
        public string ValidationValue2 { get; set; }

        [ExcelColumn(Name = "数据源")]
        public string DataSource { get; set; }

        [ExcelColumn(Name = "是否有空选项")]
        public string WithBlank { get; set; }

        [ExcelColumn(Name = "空选项默认值")]
        public string BlankValue { get; set; }

        [ExcelColumn(Name = "空选项名称")]
        public string BlankName { get; set; }

        [ExcelColumn(Name = "错误消息")]
        public string ErrorMessage { get; set; }



    }
}