using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestOrderMaker.Common
{
    class IO
    {
        #region ファイル読み込み
        static public List<string[]> ReadData(string file_path, string encoding)
        {
            List<string[]> list = new List<string[]>();

            // encoding文字が不正のときの例外処理
            StreamReader reader = null;
            try
            {
                Logger logger = Logger.getInstance();
                logger.logFatal("");
                reader = new StreamReader(file_path, Encoding.GetEncoding(encoding));
            }
            catch (Exception e)
            {
                // encoding文字が不正
                Logger logger = Logger.getInstance();
                logger.logFatal(e.StackTrace);
                return null;
            }

            string temp = "";
            while (reader.Peek() > 0)
            {
                temp = reader.ReadLine();
                list.Add(temp.Split(','));
            }
            reader.Close();
            return list;
        }
        #endregion

        #region ファイル書き込み(追記)
        static public void Write(string text, string path, string file_name, string encoding)
        {
            if (path != null && path != "")
            {
                // 出力先ディレクトリが無い場合、作成する
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                file_name = path + "\\" + file_name;
            }

            StreamWriter writer;
            try
            {
                writer = new StreamWriter(file_name, true, System.Text.Encoding.GetEncoding(encoding));
            }
            catch (Exception e)
            {
                Logger logger = Logger.getInstance();
                logger.logFatal(e.StackTrace);
                return;
            }

            writer.WriteLine(text);
            writer.Close();
        }
        #endregion

        #region ファイル書き込み(上書き)
        static public bool Write(Dictionary<string, string> dict, string path, string file_name, string encoding)
        {
            string[] array = new string[dict.Count];
            int i = 0;
            foreach (string key in dict.Keys)
            {
                array[i] = key + "," + dict[key];
                i++;
            }
            return Write(array, path, file_name, encoding);
        }
        #endregion

        #region ファイル書き込み(上書き)
        static public bool Write(string[] data_array, string path, string file_name, string encoding)
        {
            if (path != null && path != "")
            {
                // 出力先ディレクトリが無い場合、作成する
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                file_name = path + "\\" + file_name;
            }

            StreamWriter writer;
            try
            {
                writer = new StreamWriter(file_name, false, System.Text.Encoding.GetEncoding(encoding));
            }
            catch (Exception e)
            {
                // encoding文字が不正
                Logger logger = Logger.getInstance();
                logger.logFatal(e.StackTrace);
                return false;
            }

            foreach (string data in data_array)
            {
                writer.WriteLine(data);
            }
            writer.Close();
            return true;
        }
        #endregion
    }
}
