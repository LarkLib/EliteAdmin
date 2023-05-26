using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    [Serializable]
    public class ConditionItem
    {
        public string FieldName { get; set; }
        public ConditionalType ConditionalType { get; set; }
        public string FieldValue { get; set; }
    }
}
