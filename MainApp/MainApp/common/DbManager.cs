using System;
using System.Arctec.Ar1000k.Common.Utility;
using System.Arctec.Ar1000k.DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestOrderMaker.Common
{
    public class DbManager
    {
        #region クラス変数
        static private DbManager db_manager = new DbManager();

        static public DataAccessControl db_ctl = DataAccessControl.GetInstance();
        static Logger logger = Logger.getInstance();
        #endregion

        #region コンストラクタ
        private DbManager()
        {
      
        }
        #endregion

        #region インスタンス取得
        static public DbManager getInstance()
        {
            return db_manager;
        }
        #endregion

        #region init
        public bool init(string iis_path)
        {
            if (!iis_path.StartsWith("http://"))
            {
                return false;
            }
            db_ctl.SetAccessUrl(iis_path);
            return true;
        }
        #endregion

        #region MedicalCheckList全取得
        public MedicalCheckList[] getMedicalCheckList()
        {
            MedicalCheckList[] ret = null;
            try
            {
                ret = db_ctl.GetMedicalCheckListAlls();
            }
            catch (Exception e)
            {
                logger.logError(e.StackTrace);
            }
            return ret;
        }
        #endregion

        #region MedicalItemListの全取得
        public MedicalItemListRecord[] getMedicalItemListRecordAll()
        {
            MedicalItemListRecord[] ret = null;

            try
            {
                ret = db_ctl.GetMedicalItemListRecordAll();
            }
            catch (Exception e)
            {
                logger.logError(e.StackTrace);
            }
            return ret;
        }
        #endregion

        #region TermID取得
        public Dictionary<string, string> getTermID()
        {
            MedicalMachinesDataRecord[] records = db_ctl.GetMachinesDataRecordAll();
            if (records == null)
            {
                return null;
            }
            Dictionary<string, string> ret = new Dictionary<string, string>();
            foreach (MedicalMachinesDataRecord record in records)
            {
                string mc_no = record.MedicalCheckNo;
                string term_id = record.TermId;
                if (ret.ContainsKey(mc_no))
                {
                    string contains_term_id = ret[mc_no];
                    if (Int32.Parse(contains_term_id) > Int32.Parse(term_id))
                    {
                        // MedicalCheckNoが等しい場合、番号の若いTermIDを採用する
                        ret[mc_no] = term_id;
                    }
                    continue;
                }
                ret.Add(mc_no, record.TermId);
            }
            return ret;
        }
        #endregion

        #region ClientIDの取得
        public string[] getClientIdByPrimaryKey(string date, string client_id_start, string client_id_end, string division, string table_name, bool is_distinct = true)
        {
            string distinct = is_distinct ? "DISTINCT " : "";
            #region sql
            string sql =
                "SELECT " + distinct
                    + "ClientID, MedicalCheckDate, Division "
                + "FROM " + table_name + " ";

            bool has_where = false;
            if (date != "")
            {
                sql += "WHERE MedicalCheckDate = '" + date + "' ";
                has_where = true;
            }
            if (client_id_start.Trim() != "" && client_id_end.Trim() != "")
            {
                sql += has_where ? "AND " : "WHERE ";
                sql += "ClientID BETWEEN '" + client_id_start + "' AND '" + client_id_end + "' ";
                has_where = true;
            }
            if (division != "")
            {
                sql += has_where ? "AND " : "WHERE ";
                sql += "Division = '" + division + "';";
            }
            #endregion

            DataTable data_table = db_ctl.GetClientDataTableListCheckNoCustom(sql);
            if (data_table == null || data_table.Rows.Count == 0)
            {
                return null;
            }

            string[] ret = new string[data_table.Rows.Count];
            for (int i = 0; i < data_table.Rows.Count; i++)
            {
                ret[i] = data_table.Rows[i]["ClientID"].ToString();
            }
            return ret;
        }
        #endregion

        #region 主キーによるレコード削除
        public bool deleteByPrimaryKey(string date, string client_id, string division, string table_name)
        {
            #region SQL
            string sql = "DELETE FROM " + table_name + " ";
            sql += getWhere(date, client_id, division);
            #endregion

            DataTable data_table = db_ctl.GetClientDataTableListCheckNoCustom(sql);
            return data_table == null ? false : true;
        }
        #endregion

        #region WHERE句の取得
        private string getWhere(string date, string client_id, string division)
        {
            string sql = "";
            bool has_where = false;
            if (date != "")
            {
                sql += "WHERE MedicalCheckDate = '" + date + "' ";
                has_where = true;
            }
            if (client_id != "")
            {
                sql += has_where ? "AND " : "WHERE ";
                sql += "ClientID = '" + client_id + "' ";
                has_where = true;
            }
            if (division != "")
            {
                sql += has_where ? "AND " : "WHERE ";
                sql += "Division = '" + division + "';";
            }
            return sql;
        }
        #endregion

        #region ClientInformation登録
        public int insertClientInformation(string date, string client_id, string division, int client_id_pad_num, string sex, string birth_day, string course_id, bool is_old)
        {
            string time = is_old ? "00:00:00" : "07:00:00";
            string client_id_kansuji = Util.toKansuji(Int32.Parse(client_id.TrimStart()));
            string client_id_hankaku_kana = Util.toHankakuKana(client_id_kansuji);
            #region sql
            string sql =
                "INSERT INTO ClientInformation "
                    + "(MedicalCheckDate,"
                    + "ClientID,"
                    + "Division,"
                    + "StartDate,"
                    + "TelegramHeader,"
                    + "RegistrationNo,"
                    + "KanjiName,"
                    + "KanaName,"
                    + "Age,"
                    + "Sex,"
                    + "BirthDay,"
                    + "CourseID,"
                    + "MedicalCheckFlag,"
                    + "TermSendFlag,"
                    + "PastMedicalCheckDate,"
                    + "StayFlag,"
                    + "Data1,"
                    + "Data5"
                    + ")"
                + " VALUES"
                    + "("
                    + "'" + date + "',"
                    + "'" + client_id  + "',"
                    + "'" + division + "',"
                    + "'" + date + "',"
                    + "'',"
                    + "'" + client_id.TrimStart().PadLeft(client_id_pad_num, '0') + "',"
                    + "'テスト 受診者" + client_id_kansuji + "',"
                    + "'ﾃｽﾄｼﾞｭｼﾝｼｬ " + client_id_hankaku_kana + "',"
                    + "30,"
                    + "'" + sex + "',"
                    + "'" + birth_day + "',"
                    + "'" + course_id + "',"
                    + "1,"
                    + "0,"
                    + "'',"
                    + "0,"
                    + "'" + date + " " + time + "',"
                    + "''"
                    + ");";
            #endregion
            DataTable data_table = db_ctl.GetClientDataTableListCheckNoCustom(sql);
            return data_table == null ? 0 : 1;
        }
        #endregion

        #region ClientAddInformation登録
        public int insertClientAddInformation(string date, string client_id, string division)
        {
            #region sql
            string sql =
                "INSERT INTO ClientAddInformation "
                    + "(MedicalCheckDate,"
                    + "ClientID,"
                    + "Division"
                    + ")"
                + " VALUES"
                    + "("
                    + "'" + date + "',"
                    + "'" + client_id + "',"
                    + "'" + division + "'"
                    + ");";
            #endregion
            DataTable data_table = db_ctl.GetClientDataTableListCheckNoCustom(sql);
            return data_table == null ? 0 : 1;
        }
        #endregion

        #region ClientAddInformation2登録
        public int insertClientAddInformation2(string date, string client_id, string division)
        {
            #region sql
            string sql =
                "INSERT INTO ClientAddInformation2 "
                    + "(MedicalCheckDate,"
                    + "ClientID,"
                    + "Division"
                    + ")"
                + " VALUES"
                    + "("
                    + "'" + date + "',"
                    + "'" + client_id + "',"
                    + "'" + division + "'"
                    + ");";
            #endregion
            DataTable data_table = db_ctl.GetClientDataTableListCheckNoCustom(sql);
            return data_table == null ? 0 : 1;
        }
        #endregion

        #region MedicalCheckItem登録
        public int insertMedicalCheckItem(string date, string client_id, string division, Dictionary<string, string[]> data_list)
        {
            #region sql
            string sql =
                "INSERT INTO MedicalCheckItem "
                    + "(MedicalCheckDate,"
                    + "ClientID,"
                    + "Division, "
                    + "MedicalCheckNo,"
                    + "{REPLACE_STR1}"
                    + ")"
                + " VALUES"
                    + "("
                    + "'" + date + "',"
                    + "'" + client_id + "',"
                    + "'" + division + "',"
                    + "{REPLACE_STR3},"
                    + "{REPLACE_STR2}"
                    + ");";

            StringBuilder column_names = new StringBuilder();
            StringBuilder state_data = new StringBuilder();
            string executable_sql = "";
            int insert_num = 0;
            foreach (KeyValuePair<string, string[]> pair in data_list)
            {
                column_names.Clear();
                state_data.Clear();
                for (int i = 0; i < pair.Value.Length; i++)
                {
                    column_names.Append("CheckItemNo" + (i + 1));
                    state_data.Append(pair.Value[i] != null ? pair.Value[i] : "NULL");
                    if (i != pair.Value.Length - 1)
                    {
                        column_names.Append(", ");
                        state_data.Append(", ");
                    }
                }
                string medical_check_no = pair.Key;
                executable_sql = sql.Replace("{REPLACE_STR1}", column_names.ToString()).Replace("{REPLACE_STR2}", state_data.ToString()).Replace("{REPLACE_STR3}", medical_check_no);

                DataTable data_table = db_ctl.GetClientDataTableListCheckNoCustom(executable_sql);
                insert_num += data_table == null ? 0 : 1;
            }
            #endregion
            return insert_num;
        }
        #endregion

        #region MedicalCheckState登録
        public int insertMedicalCheckState(string date, string client_id, string division, Dictionary<string, string> data_array)
        {
            string sql =
                "INSERT INTO MedicalCheckState "
                    + "(MedicalCheckDate,"
                    + "ClientID,"
                    + "Division,"
                    + "TransferFlag,"
                    + "{REPLACE_STR1}"
                    + ")"
                + " VALUES"
                    + "("
                    + "'" + date + "',"
                    + "'" + client_id + "',"
                    + "'" + division + "',"
                    + "0,"
                    + "{REPLACE_STR2}"
                    + ");";
            #region sql100
            string column_names = "";
            string state_data = "";
            bool is_first = true;
            foreach (KeyValuePair<string, string> pair in data_array)
            {
                if (is_first)
                {
                    is_first = false;
                }
                else
                {
                    column_names += ", ";
                    state_data += ", ";
                }
                column_names += "State" + pair.Key;
                state_data += pair.Value;
            }
            string sql100 = sql.Replace("{REPLACE_STR1}", column_names).Replace("{REPLACE_STR2}", state_data);
            #endregion
            DataTable data_table = db_ctl.GetClientDataTableListCheckNoCustom(sql100);
            if (data_table == null)
            {
                #region sql50
                column_names = "";
                state_data = "";
                is_first = true;
                int i = 0;
                foreach (KeyValuePair<string, string> pair in data_array)
                {

                    if (i == 50)
                    {
                        break;
                    }
                    i++;
                    if (is_first)
                    {
                        is_first = false;
                    }
                    else
                    {
                        column_names += ", ";
                        state_data += ", ";
                    }
                    column_names += "State" + pair.Key;
                    state_data += pair.Value;
                }
                string sql50 = sql.Replace("{REPLACE_STR1}", column_names).Replace("{REPLACE_STR2}", state_data);
                #endregion
                data_table = db_ctl.GetClientDataTableListCheckNoCustom(sql50);
            }
            return data_table == null ? 0 : 1;
        }
        #endregion

        #region MedicalCheckData登録
        public int insertMedicalCheckData(string date, string client_id, string division, Dictionary<string, string[]> data_list, Dictionary<string, string> term_id_dict, Dictionary<string, List<string>> regist_data)
        {
            #region sql
            string sql =
                "INSERT INTO MedicalCheckData "
                    + "(MedicalCheckDate,"
                    + "ClientID,"
                    + "Division, "
                    + "MedicalCheckNo,"
                    + "HostTransfer,"
                    + "ReceiveOrder,"
                    + "TermID,"
                    + "{REPLACE_STR1}"
                    + ")"
                + " VALUES"
                    + "("
                    + "'" + date + "',"
                    + "'" + client_id + "',"
                    + "'" + division + "',"
                    + "{REPLACE_STR3},"
                    + "1,"
                    + "1,"
                    + "'{REPLACE_STR4}',"
                    + "{REPLACE_STR2}"
                    + ");";

            StringBuilder column_names = new StringBuilder();
            StringBuilder data = new StringBuilder();
            string executable_sql = "";
            int medical_check_no = 1;
            int insert_num = 0;

            // MedicalCheckNoのループ
            foreach (KeyValuePair<string, List<string>> pair in regist_data)
            {
                column_names.Clear();
                data.Clear();
                List<string> data_array = pair.Value;
                // Data列のループ
                for (int i = 0; i < data_array.Count; i++)
                {
                    column_names.Append("Data" + (i + 1));
                    if (data_array[i] != null)
                    {
                        data.Append("'");
                        data.Append(data_array[i].PadLeft(10, ' '));
                        data.Append("'");
                    }
                    else
                    {
                        data.Append("NULL");
                    }

                    if (i != data_array.Count - 1)
                    {
                        column_names.Append(", ");
                        data.Append(", ");
                    }
                }

                string term_id = term_id_dict.ContainsKey(medical_check_no.ToString()) ? term_id_dict[medical_check_no.ToString()]  : "99999";
                executable_sql = sql.Replace("{REPLACE_STR1}", column_names.ToString()).Replace("{REPLACE_STR2}", data.ToString()).Replace("{REPLACE_STR3}", medical_check_no.ToString()).Replace("{REPLACE_STR4}", term_id); ;
                medical_check_no++;

                DataTable data_table = db_ctl.GetClientDataTableListCheckNoCustom(executable_sql);
                insert_num += data_table == null ? 0 : 1;
            }
            #endregion
            return insert_num;
        }
        #endregion

        #region NextGuideData登録
        public int insertNextGuideData(string date, string client_id, string division)
        {
            #region sql
            string sql =
                "INSERT INTO NextGuideData "
                    + "(MedicalCheckDate,"
                    + "ClientID,"
                    + "Division,"
                    + "ChildGroupFlg,"
                    + "Value,"
                    + "LastUpdate"
                    + ")"
                + " VALUES"
                    + "("
                    + "'" + date + "',"
                    + "'" + client_id + "',"
                    + "'" + division + "',"
                    + "'False',"
                    + "1,"
                    + "'" + date + " 07:00:00'"
                    + ");";
            #endregion
            DataTable data_table = db_ctl.GetClientDataTableListCheckNoCustom(sql);
            return data_table == null ? 0 : 1;
        }
        #endregion

        #region 主キーによるレコード更新(MedicalCheckDateを本日日付にする)
        public bool updateByPrimaryKey(string date, string client_id, string division, string table_name)
        {
            #region SQL
            string sql = "UPDATE " + table_name + " SET MedicalCheckDate = CONVERT(NVARCHAR, GETDATE(), 111) ";
            sql += getWhere(date, client_id, division);
            #endregion

            DataTable data_table = db_ctl.GetClientDataTableListCheckNoCustom(sql);
            return data_table == null ? false : true;
        }
        #endregion

        #region ClientInformation更新(MedicalCheckDateを本日日付にする)
        public bool updateClientInformation(string date, string client_id, string division)
        {
            #region SQL
            string sql = "UPDATE ClientInformation SET MedicalCheckDate = CONVERT(NVARCHAR, GETDATE(), 111), Data1 = (CONVERT(NVARCHAR, GETDATE(), 111) + ' 07:00:00')";
            sql += getWhere(date, client_id, division);
            #endregion

            DataTable data_table = db_ctl.GetClientDataTableListCheckNoCustom(sql);
            return data_table == null ? false : true;
        }
        #endregion

        #region NextGuideData更新(MedicalCheckDateを本日日付にする)
        public bool updateNextGuideData(string date, string client_id, string division)
        {
            #region SQL
            string sql = "UPDATE NextGuideData SET MedicalCheckDate = CONVERT(NVARCHAR, GETDATE(), 111), LastUpdate = (CONVERT(NVARCHAR, GETDATE(), 111) + ' 07:00:00')";
            sql += getWhere(date, client_id, division);
            #endregion

            DataTable data_table = db_ctl.GetClientDataTableListCheckNoCustom(sql);
            return data_table == null ? false : true;
        }
        #endregion

        #region InputFormat取得
        public Dictionary<string, List<string>> getInputFormatDictionary()
        {
            #region sql
            string sql =
                "SELECT t1.MedicalCheckNo, t1.ItemNo, t2.InputFormat "
                    + "FROM MedicalItemSetting AS t1 "
                    + "JOIN InputFormatSetting AS t2 "
                    + "ON t1.InputFormatId = t2.InputFormatId;";
            #endregion
            DataTable data_table = db_ctl.GetClientDataTableListCheckNoCustom(sql);
            if (data_table == null || data_table.Rows.Count == 0)
            {
                return null;
            }
            Dictionary<string, List<string>> ret = new Dictionary<string, List<string>>();
            List<string> temp_list = new List<string>();
            for (int i = 0; i < data_table.Rows.Count; i++)
            {
                int medical_check_no = Int32.Parse(data_table.Rows[i]["MedicalCheckNo"].ToString());
                string input_format = data_table.Rows[i]["InputFormat"].ToString();
                temp_list.Add(input_format);
                if (i + 1 < data_table.Rows.Count - 1)
                {
                    // 次の行がある場合
                    int next_medical_check_no = Int32.Parse(data_table.Rows[i + 1]["MedicalCheckNo"].ToString());
                    if (medical_check_no != next_medical_check_no)
                    {
                        try
                        {
                            // MedicalCheckNoが変わる場合
                            ret.Add(medical_check_no.ToString(), temp_list);
                            temp_list = new List<string>();
                        }
                        catch
                        { }
                    }
                }
                else
                {
                    try
                    {
                        ret.Add(medical_check_no.ToString(), temp_list);
                    }
                    catch { }
                }
            }
            return ret;
        }
        #endregion
    }
}
