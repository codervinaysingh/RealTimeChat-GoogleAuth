using System.Data;
using System.Data.SqlClient;

namespace GoogleAuthentication.Models.Helper
{
    public class Helper : IHelper
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public Helper(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("sqlcon");
        }

        public void Excute(string commandName, SqlParameter[] param)
        {
            throw new NotImplementedException();
        }

        public DataTable ExcuteByProcedure(string CommandName, SqlParameter[] param)
        {
            if (string.IsNullOrWhiteSpace(CommandName))
            {
                throw new ArgumentNullException("Command can't Empty", nameof(CommandName));
            }
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(CommandName, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                try
                {
                    for (int i = 0; i < param.Length; i++)
                    {
                        cmd.Parameters.Add(param[i]);

                    }
                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                    }
                    return dt;
                }
                catch (Exception ex)
                {

                    throw new Exception("Expeption :" + ex.Message);
                }
                finally
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                    conn.Close();
                }
            }

        }
    }
}
