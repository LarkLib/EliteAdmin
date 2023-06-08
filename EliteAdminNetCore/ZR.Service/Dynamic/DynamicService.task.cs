using Microsoft.Data.SqlClient;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.AdminService;
using DbType = System.Data.DbType;

namespace ZR.AdminService
{
    public partial class DynamicService
    {
        public string ExecutePalletDown(long storeCell, int downFlag, ref string message)
        {
            var name = new SugarParameter("@name", "haha") { DbType = DbType.AnsiString, Size = 255 };

            SugarParameter[] parameters =
            {
                new SugarParameter("@F_StoreCell",storeCell, DbType.Int64),
                new SugarParameter("@downFlag",downFlag, DbType.Int32),
                new SugarParameter("@operator","admin", DbType.AnsiString){Size=255},
                new SugarParameter("@message",null, DbType.AnsiString,ParameterDirection.Output, 500),
                new SugarParameter("@return",null, DbType.AnsiString,ParameterDirection.ReturnValue,50)
            };
            var result = base.Context.Ado.UseStoredProcedure().ExecuteCommand("Elite_P_Project_ExecutePalletDown", parameters);
            message = parameters[3].Value?.ToString();
            return parameters[4].Value?.ToString();// smethod_2("Elite_P_Project_ExecutePalletDown", ref message, parameters);
        }
    }
}
