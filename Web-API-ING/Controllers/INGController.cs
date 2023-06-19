using System.Data;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_API_ING.Data;
using Web_API_ING.Model;

namespace Web_API_ING.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class INGController : ControllerBase
	{


		[HttpPost]
		[Route("DQC342")]
		public async Task<IActionResult> DQC342Get([FromBody] ObMode obMode)
		{
			try
			{
				List<ObDqc342> obDqcs = new List<ObDqc342>();
				SqlDataEx ex = new SqlDataEx();
				string SqlCmd = "SELECT SER_SN, MO_SN, JO_NO, " +
				"MODEL, M_CUST, LINE_NO, STATION, " +
				"DEF_ITEM, TEST_UID, TEST_DTTM, " +
				"INSP_DT, LAST_UPD, " +
				"UID1, CUST_ECO_NO, SN_TYPE, LOCATION, " +
				"PROD_TYPE, SUMMONS_NUMBER " +
				"FROM DQC342  " +
				"WHERE MO_SN = '" + obMode.SerId + "'";
				DataTable dt = new DataTable();
				if (obMode.SerId.Length == 9)
				{
					dt = await ex.QueryDataTable(SqlCmd);
				}
				else if (obMode.SerId.Length > 9)
				{
					dt =await ex.QueryDataTableDX26(SqlCmd);
				}

				if (dt != null)
				{
					foreach (DataRow dr in dt.Rows)
					{
						obDqcs.Add(new ObDqc342
						{
							SerSn = dr.ItemArray[0].ToString(),
							MoSn = dr.ItemArray[1].ToString(),
							JoNo = dr.ItemArray[2].ToString(),
							Model = dr.ItemArray[3].ToString(),
							MCust = dr.ItemArray[4].ToString(),
							LineNo = dr.ItemArray[5].ToString(),
							Station = dr.ItemArray[6].ToString(),
							DefItem = dr.ItemArray[7].ToString(),
							TestUid = dr.ItemArray[8].ToString(),
							TestDttm = DateTime.Parse(dr.ItemArray[9].ToString()),
							InspDt = DateTime.Parse(dr.ItemArray[10].ToString()),
							LastUpd = DateTime.Parse(dr.ItemArray[11].ToString()),
							Uid1 = dr.ItemArray[12].ToString(),
							CustEcoNo = dr.ItemArray[13].ToString(),
							SnType = dr.ItemArray[14].ToString(),
							Location = dr.ItemArray[15].ToString(),
							ProdType = dr.ItemArray[16].ToString(),
							SummonsNumber = dr.ItemArray[17].ToString()
						});
					}
				}

				if (obDqcs.Count > 0)
				{
					return Ok(obDqcs);
				}
				else
				{
					return BadRequest();
				}


			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpPost]
		[Route("PCBALOG")]
		public async Task<IActionResult> PCBALOG([FromBody] ObMode obMode)
		{
			try
			{
				List<ObPCBALOG> obDqcs = new List<ObPCBALOG>();
				SqlDataEx ex = new SqlDataEx();
				string SqlCmd = "SELECT SERIAL_NUMBER,SUMMONS_NUMBER,STATUS,MACHINEID,TEST_AP,LAST_UPD,DETAIL_VALUES,ATTRIBUTE_NAME " +
				   "FROM PCBALOG_GPES " +
				   "WHERE ATTRIBUTE_NAME = 'item' AND SERIAL_NUMBER = '" + obMode.SerId + "'";
				DataTable dt = new DataTable();
				if (obMode.SerId.Length == 9)
				{
					dt = await ex.QueryDataTable(SqlCmd);
				}
				else if (obMode.SerId.Length > 9)
				{
					dt =await ex.QueryDataTableDX26(SqlCmd);
				}

				if (dt != null)
				{
					foreach (DataRow dr in dt.Rows)
					{
						obDqcs.Add(new ObPCBALOG
						{
							SerialNumber = dr.ItemArray[0].ToString(),
							SummonsNumber = dr.ItemArray[1].ToString(),
							Status = dr.ItemArray[2].ToString(),
							MachineID = dr.ItemArray[3].ToString(),
							FullMode = dr.ItemArray[4].ToString(),
							LastUpd = DateTime.Parse(dr.ItemArray[5].ToString()),
							DetailValues = dr.ItemArray[6].ToString(),

						});
					}
				}

				if (obDqcs.Count > 0)
				{
					return Ok(obDqcs);
				}
				else
				{
					return BadRequest();
				}


			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}
		public class ApiDqc31
		{
			public bool Status { get; set; }
			public List<ObDqc31> Data { get; set; }
			public string Note { get; set; }

		}
		[HttpPost("DQC31")]
		public async Task<ApiDqc31> DQC31([FromBody] string SN)
		{
			ApiDqc31 ob = new ApiDqc31();
			try
			{
				SN = SN.ToUpper();
				string SqlCMD = "SELECT MO_SN,WO_NO,MO_SR,MODEL,SUMMONS_NUMBER FROM DQC31 WHERE MO_SN = '"+SN+"'";
				DataTable dt = new DataTable();
				List<ObDqc31> ls = new List<ObDqc31>();
				SqlDataEx db = new SqlDataEx();
				if(SN.Length == 9) // INGDB
				{
					dt = await db.QueryDataTable(SqlCMD);
					if(dt != null)
					{
					   foreach(DataRow dr in dt.Rows)
						{
							ls.Add(new ObDqc31
							{
								MO_SN = dr.ItemArray[0].ToString(),
								WO_NO = dr.ItemArray[1].ToString(),
								MO_SR = dr.ItemArray[2].ToString(),
								MODEL = dr.ItemArray[3].ToString(),
								SUMMONS = dr.ItemArray[4].ToString()
							});
						}
					}

				}
				else if (SN.Length > 9)  // INGTB
				{
					dt = await db.QueryDataTableDX26(SqlCMD);
					if (dt != null)
					{
						foreach (DataRow dr in dt.Rows)
						{
							ls.Add(new ObDqc31
							{
								MO_SN = dr.ItemArray[0].ToString(),
								WO_NO = dr.ItemArray[1].ToString(),
								MO_SR = dr.ItemArray[2].ToString(),
								MODEL = dr.ItemArray[3].ToString(),
								SUMMONS = dr.ItemArray[4].ToString()
							});
						}
					}
				}

				ob = new ApiDqc31
				{
					Data = ls,
					Note = "OK",
					Status = true
				};
				return ob;
			}
			catch
			{
				ob = new ApiDqc31
				{
					Note = "Query Error DQC31",
					Status = false
				};
				return ob;
			}
		}
		public class ApiDqc021Sum
		{
			public bool Status { get; set; }
			public List<ObDql021SUM> Data { get; set; }
			public string Note { get; set; }

		}
		[HttpPost("DQC021SUM")]
		public async Task<ApiDqc021Sum> DQC021SUM([FromBody] string SN)
		{
			ApiDqc021Sum ob = new ApiDqc021Sum();
			try
			{
				SN = SN.ToUpper();
				string SqlCMD = "SELECT BARCODE,STATION,DEF_ITEM,ROOT_CAUSE,NOTE,RESULT,LAST_UPD FROM DQC021SUM WHERE BARCODE = '" + SN + "'";
				DataTable dt = new DataTable();
				List<ObDql021SUM> ls = new List<ObDql021SUM>();
				SqlDataEx db = new SqlDataEx();
				if (SN.Length == 9) // INGDB
				{
					dt = await db.QueryDataTable(SqlCMD);
					if (dt != null)
					{
						foreach (DataRow dr in dt.Rows)
						{
							ls.Add(new ObDql021SUM
							{
								BARCODE = dr.ItemArray[0].ToString(),
								STATION = dr.ItemArray[1].ToString(),
								DEF_ITEM = dr.ItemArray[2].ToString(),
								ROOT_CAUSE = dr.ItemArray[3].ToString(),
								NOTE = dr.ItemArray[4].ToString(),
								RESULT = dr.ItemArray[5].ToString(),
								LAST_UPD = dr.ItemArray[6].ToString()
							});
						}
					}

				}
				else if (SN.Length > 9)  // INGTB
				{
					dt = await db.QueryDataTableDX26(SqlCMD);
					if (dt != null)
					{
						foreach (DataRow dr in dt.Rows)
						{
							ls.Add(new ObDql021SUM
							{
								BARCODE = dr.ItemArray[0].ToString(),
								STATION = dr.ItemArray[1].ToString(),
								DEF_ITEM = dr.ItemArray[2].ToString(),
								ROOT_CAUSE = dr.ItemArray[3].ToString(),
								NOTE = dr.ItemArray[4].ToString(),
								RESULT = dr.ItemArray[5].ToString(),
								LAST_UPD = dr.ItemArray[6].ToString()
							});
						}
					}
				}

				ob = new ApiDqc021Sum
				{
					Data = ls,
					Note = "OK",
					Status = true
				};
				return ob;
			}
			catch
			{
				ob = new ApiDqc021Sum
				{
					Note = "Query Error DQC31",
					Status = false
				};
				return ob;
			}
		}
	}
}
