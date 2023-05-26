using Infrastructure.Attribute;
using JinianNet.JNTemplate;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Elite;
using ZR.Service;

namespace ZR.AdminService
{
    public partial class DynamicService
    {
        private Dictionary<string, string> Sqls = new()
        {
            ["T_Base_Employee_With_Group"] = "select e.*,g.F_TeamGroupName from T_Base_Employee e join T_Base_TeamGroup g on e.F_TeamGroupID=g.F_ID",
        }; 
    }
}