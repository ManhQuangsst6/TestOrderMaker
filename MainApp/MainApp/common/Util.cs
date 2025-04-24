namespace TestOrderMaker.Common
{
    class Util
    {
        static public Logger logger = Logger.getInstance();

        #region 数字を漢字に変換
        static public string toKansuji(long number)
        {
            if (number == 0)
            {
                return "〇";
            }

            string[] kl = new string[] { "", "十", "百", "千" };
            string[] tl = new string[] { "", "万", "億", "兆", "京" };
            string[] nl = new string[] { "", "一", "二", "三", "四", "五", "六", "七", "八", "九" };

            string str = "";
            int keta = 0;
            while (number > 0)
            {
                int k = keta % 4;
                int n = (int)(number % 10);

                if (k == 0 && number % 10000 > 0)
                {
                    str = tl[keta / 4] + str;
                }

                if (k != 0 && n == 1)
                {
                    str = kl[k] + str;
                }
                else if (n != 0)
                {
                    str = nl[n] + kl[k] + str;
                }

                keta++;
                number /= 10;
            }
            return str;
        }
        #endregion

        #region 漢数字を半角カナに変換
        static public string toHankakuKana(string kansuji)
        {
            if (kansuji == "")
            {
                return "";
            }
            string ret = "";
            // char[] kanji_array = new char[] { '一', '二', '三', '四', '五', '六', '七', '八', '九', '十', '百', '千', '万', '億', '兆', '京' };
            string kanji = "一二三四五六七八九十百千万億兆京";
            string[] kana_array = new string[] { "ｲﾁ", "ﾆ", "ｻﾝ", "ﾖﾝ", "ｺﾞ", "ﾛｸ", "ﾅﾅ", "ﾊﾁ", "ｷｭｳ", "ｼﾞｭｳ", "ﾋｬｸ", "ｾﾝ", "ﾏﾝ", "ｵｸ", "ﾁｮｳ", "ｹｲ" };
            foreach (char c in kansuji)
            {
                int index = kanji.IndexOf(c);
                ret += kana_array[index];
            }
            return ret;
        }
        #endregion
    }
}
