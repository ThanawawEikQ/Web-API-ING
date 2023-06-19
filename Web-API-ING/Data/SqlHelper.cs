using Oracle.ManagedDataAccess.Client;

namespace Web_API_ING.Data
{
	public class SqlHelper
	{
		public static string conStrEXDX;
		public static string conStrDX26;
		public static OracleConnection GetConnectionEX()
		{
			try
			{
				OracleConnection connection = new OracleConnection(conStrEXDX);
				return connection;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		public static OracleConnection GetConnectionDX26()
		{
			try
			{
				OracleConnection connection = new OracleConnection(conStrDX26);
				return connection;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
	}
}
