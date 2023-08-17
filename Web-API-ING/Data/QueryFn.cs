using System.Data;
using Web_API_ING.Model;
using static Web_API_ING.Controllers.INGController;

namespace Web_API_ING.Data
{
    public class QueryFn
    {

        public async Task<ApiPcbalog> PCBATEST(string SN,int mode)
        {
            ApiPcbalog ob = new ApiPcbalog();
            try
            {
                List<ObPCBALOG> obDqcs = new List<ObPCBALOG>();
                SqlDataEx ex = new SqlDataEx();
                string SqlCmd = "";
                if (mode == 1)
                {
                    SqlCmd =  "SELECT SERIAL_NUMBER,SUMMONS_NUMBER,STATUS,MACHINEID,TEST_AP,LAST_UPD,DETAIL_VALUES,ATTRIBUTE_NAME " +
                   "FROM PCBALOG_GPES " +
                   "WHERE ATTRIBUTE_NAME = 'item' AND SERIAL_NUMBER = '" + SN + "'";
                }
                 else if (mode == 2)
                {
                    SqlCmd = "SELECT SERIAL_NUMBER,SUMMONS_NUMBER,STATUS,COMPUTER_NAME,STATION_NAME,LAST_UPD,DESCRIPTION,ATTRIBUTE_NAME FROM PCBALOG_FACTSW WHERE SERIAL_NUMBER ='" + SN + "' AND ATTRIBUTE_NAME='description'";
                }

                DataTable dt = new DataTable();
                if (SN.Length == 9)
                {
                    dt = await ex.QueryDataTable(SqlCmd);
                }
                else if (SN.Length > 9)
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
                ob = new ApiPcbalog
                { 
                    Data = obDqcs,
                    Note = "OK",
                    Status = true
                };
                return ob;

            }
            catch (Exception ex)
            {
                ob = new ApiPcbalog
                {
                    Note = "Query Error",
                    Status = false
                };
                return ob;
            }
           
        }

        public async Task<List<ObDqc342>>  QueryDqc342(string Sn)
        {
            List<ObDqc342> obDqcs = new List<ObDqc342>();

            try
            {
                SqlDataEx ex = new SqlDataEx();
                string SqlCmd = "SELECT SER_SN, MO_SN, JO_NO, " +
                "MODEL, M_CUST, LINE_NO, STATION, " +
                "DEF_ITEM, TEST_UID, TEST_DTTM, " +
                "INSP_DT, LAST_UPD, " +
                "UID1, CUST_ECO_NO, SN_TYPE, LOCATION, " +
                "PROD_TYPE, SUMMONS_NUMBER " +
                "FROM DQC342  " +
                "WHERE MO_SN = '" + Sn + "'";
                DataTable dt = new DataTable();
                if (Sn.Length == 9)
                {
                    dt = await ex.QueryDataTable(SqlCmd);
                }
                else if (Sn.Length > 9)
                {
                    dt = await ex.QueryDataTableDX26(SqlCmd);
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
            }
            catch { }

            return obDqcs;
        }
    }
}
