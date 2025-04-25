using System.Collections.Generic;

namespace MainApp.Tables
{
    class NextGuideDataTable : TableBase
    {
        #region コンストラクタ
        public NextGuideDataTable(string table_name, bool isChecked)
            : base(table_name, isChecked)
        {

        }
        #endregion

        #region 登録処理
        override public void insert(string date, string client_id, string division, int client_id_pad_num, Dictionary<string, string[]> data_list, Dictionary<string, string> data_array,
                                    string sex, string birth_day, string course_id)
        {
            if (!isChecked)
            {
                return;
            }
            insert_num += db_manager.insertNextGuideData(date, client_id, division);
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
            insert_num_old += db_manager.insertNextGuideData(date, client_id, division);
        }
        #endregion
    }
}
