using GoogleAuthentication.Models.Helper;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace GoogleAuthentication.Models.ChatHistory
{
    public class ChatHistory
    {
        public int opCode { get; set; }
        public String message { get; set; }
        public String userName { get; set; }
        public String sendTo { get; set; }
    }
    public class DapperChatHistory:IChatHistory
    {
        private readonly IHelper _dbHelper;

        public DapperChatHistory(IHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public string sendChatHistory(ChatHistory obj)
        {
            try
            {
                SqlParameter[] param =
                {
                    new SqlParameter("opCode",obj.opCode),
                    new SqlParameter("userName",obj.userName),
                    new SqlParameter("sendTo",obj.sendTo),
                    new SqlParameter("message",obj.message),
                };
                var dt = _dbHelper.ExcuteByProcedure("sp_SaveChatHistory", param);
                string jsonString = JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented);           
                return jsonString;
                  
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
