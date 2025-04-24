using System.Collections.Generic;
using TestOrderMaker.Common;

namespace TestOrderMaker.Tables
{
    abstract class TableBase
    {
        #region メンバ変数
        static protected DbManager db_manager = DbManager.getInstance();
        private string table_name;
        protected bool isChecked;
        private string[] registed_client_id_array;
        protected int insert_num;
        private string[] registed_client_id_array_old;
        protected int insert_num_old;
        private int update_target_num;
        private int update_failure_num;
        private int delete_target_num;
        private int delete_failure_num;
        #endregion

        #region コンストラクタ
        public TableBase(string table_name, bool isChecked)
        {
            this.table_name = table_name;
            this.isChecked = isChecked;
        }
        #endregion

        #region 既に登録済ClientIDの取得
        public bool getRegistedClientID(string date, string client_id_start, string client_id_end, string division)
        {
            if (!isChecked)
            {
                return false;
            }
            registed_client_id_array = db_manager.getClientIdByPrimaryKey(date, client_id_start, client_id_end, division, table_name);
            return (registed_client_id_array != null);
        }
        #endregion

        #region 既に登録済ClientIDの取得(過去用)
        public bool getRegistedClientID_Old(string date, string client_id_start, string client_id_end, string division)
        {
            if (!isChecked)
            {
                return false;
            }
            registed_client_id_array_old = db_manager.getClientIdByPrimaryKey(date, client_id_start, client_id_end, division, table_name);
            return (registed_client_id_array_old != null);
        }
        #endregion

        #region 登録時警告メッセージ取得
        public string getWarningMsg()
        {
            if (!isChecked)
            {
                return "";
            }
            if (registed_client_id_array == null)
            {
                return "";
            }
            string warning_msg = "\r\n【" + table_name + "】\r\n";
            foreach (string client_id in registed_client_id_array)
            {
                warning_msg += client_id + "\r\n";
            }
            return warning_msg;
        }
        #endregion

        #region 登録時警告メッセージ取得(過去用)
        public string getWarningMsgOld()
        {
            if (!isChecked)
            {
                return "";
            }
            if (registed_client_id_array_old == null)
            {
                return "";
            }
            string warning_msg = "\r\n【" + table_name + "】\r\n";
            foreach (string client_id in registed_client_id_array_old)
            {
                warning_msg += client_id + "\r\n";
            }
            return warning_msg;
        }
        #endregion

        #region 主キーによるレコード削除
        public void deleteByPrimaryKey(string date, string client_id, string division)
        {
            if (!isChecked)
            {
                return;
            }
            if (registed_client_id_array == null && delete_target_num <= 0 && registed_client_id_array_old == null)
            {
                return;
            }
            db_manager.deleteByPrimaryKey(date, client_id, division, table_name);
        }
        #endregion

        #region 登録数の初期化
        public void resetInsertNum()
        {
            insert_num = 0;
            insert_num_old = 0;
        }
        #endregion

        #region 登録結果メッセージ取得
        public string getInsertResultMsg(int total_num, int medical_check_no_count)
        {
            if (!isChecked)
            {
                return "";
            }
            if (table_name == "MedicalCheckItem")
            {
                total_num *= medical_check_no_count;
            }
            return "\n【" + table_name + "】" + insert_num + "件成功, " + (total_num - insert_num) + "件失敗";
        }
        #endregion

        #region 登録結果メッセージ取得
        public string getInsertResultMsgOld(int total_num, int medical_check_no_count)
        {
            if (!isChecked)
            {
                return "";
            }
            if (table_name == "MedicalCheckItem" || table_name == "MedicalCheckData")
            {
                total_num *= medical_check_no_count;
            }
            return "\n【" + table_name + "】" + insert_num_old + "件成功, " + (total_num - insert_num_old) + "件失敗";
        }
        #endregion

        #region 対象レコード数取得
        private int getTargetNum(string date, string client_id_start, string client_id_end, string division)
        {
            if (!isChecked)
            {
                return 0;
            }
            bool is_distinct = true;
            if (table_name == "MedicalCheckItem" || table_name == "MedicalCheckData")
            {
                is_distinct = false;
            }
            string[] temp_array = db_manager.getClientIdByPrimaryKey(date, client_id_start, client_id_end, division, table_name, is_distinct);
            return temp_array != null ? temp_array.Length : 0;
        }
        #endregion

        #region 削除対象レコード数取得
        public void getDeleteTargetNum(string date, string client_id_start, string client_id_end, string division)
        {
            delete_target_num = getTargetNum(date, client_id_start, client_id_end, division);
        }
        #endregion

        #region 削除失敗レコード数の取得
        public void getDeleteFailureNum(string date, string client_id_start, string client_id_end, string division)
        {
            delete_failure_num = getTargetNum(date, client_id_start, client_id_end, division);
        }
        #endregion

        #region 削除結果メッセージ取得
        public string getDeleteResultMsg()
        {
            if (!isChecked)
            {
                return "";
            }
            return "\n【" + table_name + "】" + (delete_target_num - delete_failure_num) + "件成功, " + delete_failure_num + "件失敗";
        }
        #endregion

        #region 更新対象レコード数取得
        public void getUpdateTargetNum(string date, string client_id_start, string client_id_end, string division)
        {
            update_target_num = getTargetNum(date, client_id_start, client_id_end, division);
        }
        #endregion

        #region 主キーによるレコード更新(MedicalCheckDateを本日日付にする)
        public void updateByPrimaryKey(string date, string client_id, string division)
        {
            if (!isChecked)
            {
                return;
            }
            if (update_target_num <= 0)
            {
                return;
            }
            switch (table_name)
            {
                case "ClientInformation":
                    db_manager.updateClientInformation(date, client_id, division);
                    break;
                case "NextGuideData":
                    db_manager.updateNextGuideData(date, client_id, division);
                    break;
                default:
                    db_manager.updateByPrimaryKey(date, client_id, division, table_name);
                    break;
            }

        }
        #endregion

        #region 更新失敗レコード数の取得
        public void getUpdateFailureNum(string date, string client_id_start, string client_id_end, string division)
        {
            update_failure_num = getTargetNum(date, client_id_start, client_id_end, division);
        }
        #endregion

        #region 更新結果メッセージ取得
        public string getUpdateResultMsg()
        {
            if (!isChecked)
            {
                return "";
            }
            return "\n【" + table_name + "】" + (update_target_num - update_failure_num) + "件成功, " + update_failure_num + "件失敗";
        }
        #endregion

        #region プロパティ
        public string TableName
        {
            get
            {
                return table_name;
            }
        }
        public bool Checked
        {
            get
            {
                return isChecked;
            }
        }
        #endregion

        #region 登録処理
        abstract public void insert(string date, string client_id, string division, int client_id_pad_num, Dictionary<string, string[]> data_list, Dictionary<string, string> data_array,
                            string sex, string birth_day, string course_id);
        #endregion

        #region 登録処理(過去用)
        abstract public void insertOld(string date, string client_id, string division, int client_id_pad_num, Dictionary<string, string[]> data_list, Dictionary<string, string> data_array,
                                        string sex, string birth_day, string course_id);
        #endregion
    }
}