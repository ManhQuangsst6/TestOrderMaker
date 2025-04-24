using System;
using System.Data;
using System.IO;

namespace TestOrderMaker.Common
{
    class XmlIO
    {
        #region クラス変数
        // データセット
        private static DataSet data = new DataSet();
        #endregion

        #region App.configパラメータ取得
        static public string getConfigParam(string key)
        {
            return key;
        }
        #endregion

        #region XMLファイルの読み込み
        static public void init(string file_path)
        {
            StreamReader reader = null;

            try
            {
                reader = new StreamReader(file_path);
                // Xmlの読み込み
                data.ReadXml(reader);
            }
            catch (Exception e)
            {
                // 例外は上位にスロー
                throw e;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                reader = null;
            }
        }
        #endregion

        #region プロパティ
        static public DataTable getTable(string table_name)
        {
            return data.Tables[table_name];
        }
        #endregion
    }
}
