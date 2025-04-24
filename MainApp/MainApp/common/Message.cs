using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOrderMaker.Common
{
    public class Message
    {
        #region 変数
        static private Message message;
        private DataTable resource_table;
        #endregion

        #region コンストラクタ
        private Message()
        {
            resource_table = XmlIO.getTable("Message");
        }
        #endregion

        #region getInstance
        static public Message getInstance()
        {
            if (message == null)
            {
                message = new Message();
            }
            return message;
        }
        #endregion

        #region メッセージ取得
        public string getMessage(string msgId)
        {
            DataRow[] rows = resource_table.Select("ID = '" + msgId + "'");
            Logger logger = Logger.getInstance();

            if (rows.Length <= 0)
            {
                logger.logError("メッセージ取得失敗 msgID: " + msgId);
                return "";            
            }

            string text = "";
            try
            {
                text = rows[0]["Text"].ToString();
            }
            catch (Exception e)
            {
                logger.logError(e.StackTrace);
                return "";
            }
            
            if (text == null)
            {
                logger.logError("【メッセージ取得失敗】textがnull msgID: " + msgId);
                return "";
            }
            return text;
        }
        #endregion
    }
}
