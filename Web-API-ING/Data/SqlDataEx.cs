using System.Data;
using Microsoft.VisualBasic;
using MySqlConnector;
using Oracle.ManagedDataAccess.Client;

namespace Web_API_ING.Data
{
	public class SqlDataEx
    {
        DataSet DS = new DataSet();
        public async Task<bool> ExecuteNonQueryAsyncData(string Cmd)
		{
			bool Result = false;
			try
			{
				OracleConnection conn = new OracleConnection();
				conn = SqlHelper.GetConnectionEX();
					var commandText = "";
					conn.Open();
					commandText = Cmd;
					OracleCommand cmd = new OracleCommand(commandText, conn);
			
				return Result = true;
			}
			catch { return Result; throw; }
		}

        public async Task<DataTable> QueryDataTable(string SqlCmdIn)
        {

                OracleConnection conn = SqlHelper.GetConnectionEX();
                conn.Open();
                OracleCommand odbcc = new OracleCommand();
                odbcc.CommandText = SqlCmdIn;
                odbcc.Connection = conn;
                OracleDataAdapter odbca = new OracleDataAdapter(odbcc);
                DS.Clear();

                odbca.Fill(DS);

                return DS.Tables[0];
        
        }
		public async Task<DataTable> QueryDataTableDX26(string SqlCmdIn)
		{

			OracleConnection conn = SqlHelper.GetConnectionDX26();
			conn.Open();
			OracleCommand odbcc = new OracleCommand();
			odbcc.CommandText = SqlCmdIn;
			odbcc.Connection = conn;
			OracleDataAdapter odbca = new OracleDataAdapter(odbcc);
			DS.Clear();

			odbca.Fill(DS);

			return DS.Tables[0];

		}

	}
}

