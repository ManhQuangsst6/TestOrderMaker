using System.Collections.Generic;

namespace TestOrderMaker.Tables
{
    class MedicalCheckItemTable : TableBase
    {
        #region コンストラクタ
        public MedicalCheckItemTable(string table_name, bool isChecked)
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
            insert_num += db_manager.insertMedicalCheckItem(date, client_id, division, data_list);
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
            insert_num_old += db_manager.insertMedicalCheckItem(date, client_id, division, data_list);
        }
        #endregion
    }
}
