using System.Data;
using System.Data.SqlClient;

namespace GoogleAuthentication.Models.Helper
{
    public interface IHelper
    {
     void Excute (string commandName, SqlParameter[] param);
       DataTable ExcuteByProcedure(string CommandName, SqlParameter[] param);
    }
}
