using System.Collections.Generic;

namespace MainApp.Tables
{
    class MedicalCheckDataTable : TableBase
    {
        #region 変数
        private Dictionary<string, string> term_id_dict;
        private Dictionary<string, List<string>> data_old;
        #endregion

        #region コンストラクタ
        public MedicalCheckDataTable(string table_name, bool isChecked)
            : base(table_name, isChecked)
        {
        }
        #endregion

        #region TermID辞書の設定
        public void setTermIdDictionary(Dictionary<string, string> dict)
        {
            this.term_id_dict = dict;
        }
        #endregion

        #region InputFormat辞書の設定
        public void setDataOldDictionary(Dictionary<string, List<string>> dict)
        {
            this.data_old = dict;
        }
        #endregion

        #region 登録処理
        override public void insert(string date, string client_id, string division, int client_id_pad_num, Dictionary<string, string[]> data_list, Dictionary<string, string> data_array,
                                    string sex, string birth_day, string course_id)
        {
            //insert_num += db_manager.insertMedicalCheckData(date, client_id, division, data_list, term_id_dict);
            // 登録対象外
            return;
        }
        #endregion

        #region 登録処理(過去値)
        override public void insertOld(string date, string client_id, string division, int client_id_pad_num, Dictionary<string, string[]> data_list, Dictionary<string, string> data_array,
                                    string sex, string birth_day, string course_id)
        {
            if (!isChecked)
            {
                return;
            }
            insert_num_old += db_manager.insertMedicalCheckData(date, client_id, division, data_list, term_id_dict, data_old);
        }
        #endregion
    }
}
