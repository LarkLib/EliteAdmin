using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto;

namespace ZR.Model.Dto
{
    [Serializable]
    public class DynamicQueryDto : PagerInfo
    {
        public string QueryCode { get; set; }
        public List<ConditionItem> Conditions { get; set; }
    }
}
