using GoogleAuthentication.Models.Helper;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace GoogleAuthentication.Models.UserMaster
{
    public class UserMaster
    {
        public int Id { get; set; }
        public int OpCode { get; set; }
        public String UserName { get; set; }
        public String ConnectionId { get; set; }
        
    }
    public class DapperUserMaster : IUserMaster
    {
        private readonly IHelper _dbHelper;

        public DapperUserMaster(IHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public string AddUpdateConnection(UserMaster user)
        {
            try
            {
                SqlParameter[] param =
               {
                    new SqlParameter("opCode" ,user.OpCode),
                    new SqlParameter("connectionId", user.ConnectionId),
                    new SqlParameter("userName",user.UserName)
                };
                var dt = _dbHelper.ExcuteByProcedure("sp_SaveUpadateConnection", param);
                return JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string DisConnect(UserMaster user)
        {
            throw new NotImplementedException();
        }

        public string GellAllChat(string userName ,string selectUser)
        {
            try
            {
                SqlParameter[] param =
               {
                    new SqlParameter("opCode" ,1),
                    new SqlParameter("UserName",userName),
                    new SqlParameter("SelectUser",selectUser)
                };
                var dt = _dbHelper.ExcuteByProcedure("sp_GetUserChat", param);
                return JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string GetAllUser(string UserName)
        {
            try
            {
                SqlParameter[] param =
               {
                    new SqlParameter("opCode" ,3),
                   
                    new SqlParameter("userName",UserName)
                };
                var dt = _dbHelper.ExcuteByProcedure("sp_SaveUpadateConnection", param);
                return JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }

}
