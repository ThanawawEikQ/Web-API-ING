using System;
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
        SqlDataEx db = new SqlDataEx();

        public class ApiDqc342
        {
            public bool Status { get; set; }
            public List<ObDqc342> Data { get; set; }
            public string Note { get; set; }

        }

        [HttpPost]
        [Route("DQC342")]
        public async Task<IActionResult> DQC342Get([FromBody] ObMode obMode)
        {
            try
            {
                QueryFn fn = new QueryFn();
                List<ObDqc342> obDqcs = new List<ObDqc342>();
                obDqcs = await fn.QueryDqc342(obMode.SerId);
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
                    if (dt.Rows.Count < 2)
                    {
                        SqlCmd = "SELECT SERIAL_NUMBER,SUMMONS_NUMBER,STATUS,MACHINEID,TEST_AP,LAST_UPD,DETAIL_VALUES,ATTRIBUTE_NAME,ROW_GROUP " +
                        "FROM PCBALOG_GPES  WHERE ATTRIBUTE_NAME = 'part_number' AND ROW_GROUP ='0' AND SERIAL_NUMBER = '" + obMode.SerId + "'";
                        dt = await ex.QueryDataTable(SqlCmd);
                    }
                }
                else if (obMode.SerId.Length > 9)
                {
                    dt = await ex.QueryDataTableDX26(SqlCmd);
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
        public class SerialNo
        {
            public string SN { get; set; }
        }
        [HttpPost]
        [Route("DTBR_WINTEST")]
        public async Task<IActionResult> DTBR_WINTEST([FromBody] SerialNo obMode)
        {
            try
            {
                List<DTBL_IFTPOb> obDqcs = new List<DTBL_IFTPOb>();
                SqlDataEx ex = new SqlDataEx();
                string SqlCmd = @"SELECT DTBR_TABLEWINTESTTETRA.SERIAL_NUMBER_C, DTBR_TABLEWINTESTTETRA.STATUS_I, DTBR_TABLEWINTESTTETRA.C_CODEPRODUIT, DTBR_TABLEWINTESTTETRA.C_DEFAUT, DTBR_TABLEWINTESTTETRA.C_POSTE, DTBR_TABLEWINTESTTETRA.LAST_UPD, DTBR_TABLEWINTESTTETRA.UID1
                            FROM TMCP.DTBR_TABLEWINTESTTETRA DTBR_TABLEWINTESTTETRA
                            WHERE(DTBR_TABLEWINTESTTETRA.SERIAL_NUMBER_C = '" + obMode.SN + "')";
                DataTable dt = new DataTable();
                if (obMode.SN.Length > 9)
                {
                    dt = await ex.QueryDataTableDX26(SqlCmd);
                }

                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string GetStatus = dr.ItemArray[1].ToString();
                        if (GetStatus == "0")
                        {
                            GetStatus = "Pass";

                        }
                        else
                        {
                            GetStatus = "Fail";

                        }
                        obDqcs.Add(new DTBL_IFTPOb
                        {
                            SERIAL_NUMBER_C = dr.ItemArray[0].ToString(),
                            STATUS_I = GetStatus,
                            C_CODEPRODUIT = dr.ItemArray[2].ToString(),
                            C_DEFAUT = dr.ItemArray[3].ToString(),
                            C_POSTE = dr.ItemArray[4].ToString(),
                            LAST_UPD = DateTime.Parse(dr.ItemArray[5].ToString()),
                            UID1 = dr.ItemArray[6].ToString(),

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
        [HttpPost]
        [Route("FACTSW")]
        public async Task<ApiPcbalog> FACTSW([FromBody] SerialNo Sn)
        {
            ApiPcbalog apis = new ApiPcbalog();
            QueryFn dbs = new QueryFn();
            apis = await dbs.PCBATEST(Sn.SN, 2);
            return apis;
        }

        public class ApiDqc31
        {
            public bool Status { get; set; }
            public List<ObDqc31> Data { get; set; }
            public string Note { get; set; }

        }

        [HttpPost]
        [Route("DQC31")]
        public async Task<ApiDqc31> DQC31([FromBody] SerialNo SN)
        {
            ApiDqc31 ob = new ApiDqc31();
            try
            {
                SN.SN = SN.SN.ToUpper();
                string SqlCMD = "SELECT MO_SN,WO_NO,MO_SR,MODEL,SUMMONS_NUMBER FROM DQC31 WHERE MO_SN = '" + SN.SN + "'";
                DataTable dt = new DataTable();
                List<ObDqc31> ls = new List<ObDqc31>();
                SqlDataEx db = new SqlDataEx();
                if (SN.SN.Length == 9) // INGDB
                {
                    dt = await db.QueryDataTable(SqlCMD);
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
                else if (SN.SN.Length > 9)  // INGTB
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
        //--------------------------------------------------------------- DQC892
        public class ApiDqc892
        {
            public bool Status { get; set; }
            public List<ObDqc892> Data { get; set; }
            public string Note { get; set; }
        }
        public class ApiClientDqc892
        {
            public string sn { get; set; }
            public string Locaion { get; set; }
        }

        [HttpPost("DQC892")]
        public async Task<ApiDqc892> DQC892([FromBody] ApiClientDqc892 ob)
        {
            ApiDqc892 data = new ApiDqc892();
            List<ObDqc892> Data = new List<ObDqc892>();
            try
            {
                QueryFn fn = new QueryFn();
                List<string> Aoi = new List<string>();
                List<ObDqc342> obDqcs = new List<ObDqc342>();
                DataTable dt = new DataTable();

                //string Cmd = $"SELECT DQC892A || DQC892B || DQC892C WHERE DQC892A.LAST_UPD LIKE '%{item}%' " +
                //	$"|| DQC892B.LAST_UPD LIKE '%{item}%' || DQC892C.LAST_UPD LIKE '%{item}%' " +
                //	$"AND DQC892A.LOCATION ='{ob.Locaion}' ||  DQC892B.LOCATION ='{ob.Locaion}' || DQC892C.LOCATION ='{ob.Locaion}'";
                string[] Cmd = { $"SELECT * FROM TMCP.DQC892A WHERE  LOCATION ='{ob.Locaion}' AND MO_SN ='{ob.sn}'",
                    $"SELECT * FROM TMCP.DQC892B WHERE  LOCATION ='{ob.Locaion}' AND MO_SN ='{ob.sn}'",
                    $"SELECT * FROM TMCP.DQC892C WHERE  LOCATION ='{ob.Locaion}' AND MO_SN ='{ob.sn}'" };

                foreach (string cmd in Cmd)
                {

                    if (ob.sn.Length == 9)
                    {
                        dt = await db.QueryDataTable(cmd);
                    }
                    else
                    {
                        dt = await db.QueryDataTableDX26(cmd);
                    }

                    if (dt.Rows.Count > 0 && dt != null)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            Data.Add(new ObDqc892
                            {
                                TRANS_NO = dr.ItemArray[0].ToString(),
                                MO_SN = dr.ItemArray[1].ToString(),
                                SITE_CODE = dr.ItemArray[2].ToString(),
                                LOAD_DT = DateTime.Parse(dr.ItemArray[3].ToString()),
                                INSP_DT = DateTime.Parse( dr.ItemArray[4].ToString()),
                                LINE_NO = dr.ItemArray[5].ToString(),
                                ASSY_PN = dr.ItemArray[7].ToString(),
                                ASSY_REV = dr.ItemArray[8].ToString(),
                                DA = dr.ItemArray[10].ToString(),
                                SON_PN = dr.ItemArray[11].ToString(),
                                SUPPLIER_CODE = dr.ItemArray[12].ToString(),
                                LOT_CD = dr.ItemArray[14].ToString(),
                                DATE_CODE = dr.ItemArray[15].ToString(),
                                LOAD_QTY = dr.ItemArray[17].ToString(),
                                LOCATION = dr.ItemArray[19].ToString(),
                                LAST_UPD = DateTime.Parse( dr.ItemArray[21].ToString()),
                            });
                        }
                        data.Data = Data;
                        data.Status = true;
                        data.Note = "OK";
                    }
                    else
                    {
                        data.Status = false;
                        data.Note = "NG";
                    }
                }

            }
            catch
            {
                data.Status = false;
                data.Note = "Catch";
            }
            return data;
        }

    }
}
