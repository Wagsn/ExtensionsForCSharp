using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    /// 数值类型扩展
    /// </summary>
    public static class NumericExtension
    {
        private static readonly string[] hanzi = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
        //private static readonly List<char> hanziC = new List<char> { '零', '一', '二', '三', '四', '五', '六', '七', '八', '九' };
        // 进制单位 亿,兆,京,垓,姊,穰,沟,涧,正,载
        private static readonly string[] units = {"","万","亿","兆","京", "垓", "穰", "沟", "涧", "正", "载" };
        /// <summary>
        /// 转换为中文数值字符串，中数(万进系统)
        /// </summary>
        /// <param name="input">数值</param>
        /// <returns>中文数值字符串</returns>
        public static string ToChinese(this long input)
        {
            // "9123400000560" -> "九兆一千二百三十四亿零五百六"
            var numStr =  input.ToString();
            // 按四位分割成 {"0560","0000","1234","9"}
            var numStrs = new List<string>();
            for(int i = numStr.Length - 1; i > -1; i-=4)
            {
                if(i < 4)
                {
                    numStrs.Add(numStr.Substring(0, i + 1));
                }
                else
                {
                    numStrs.Add(numStr.Substring(i - 3, 4));
                }
            }
            var index = -1;
            numStrs = numStrs.Select(a => 
            {
                var temp = "";
                index++;
                if (a.Length > 0)
                {
                    temp = $"{hanzi[(a[a.Length - 1] - 48)]}";
                }
                if (a.Length > 1)
                {
                    temp = $"{hanzi[(a[a.Length - 2] - 48)]}十" + temp;
                }
                if (a.Length > 2)
                {
                    temp = $"{hanzi[(a[a.Length - 3] - 48)]}百" + temp;
                }
                if (a.Length > 3)
                {
                    temp = $"{hanzi[(a[a.Length - 4] - 48)]}千" + temp;
                }
                return temp+$"{units[index]}";
            }).ToList();
            numStrs.Reverse();
            var res = string.Join("", numStrs);
            res = Regex.Replace(res, "零[载正涧沟穰姊垓京兆亿万千百十]", "零");
            res = Regex.Replace(res, "零+", "零");
            // TODO "250" -> "二百五十零" 去掉末尾的零，去掉连续的最后一个单位："二百五十"->"二百五","三千六百"->"三千六"
            return res;
        }

        /// <summary>
        /// 转换为中文数值字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToChinese(this int input)
        {
            return ToChinese((long)input);
        }
    }
}
