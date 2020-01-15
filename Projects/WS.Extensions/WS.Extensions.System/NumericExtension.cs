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
            return ToChinese(input.ToString());
        }

        // 载正涧沟穰姊垓京兆亿万千百十 零一二三四五六七八九
        private static string ToChinese(this string input)
        {
            // 目标 "9123400000560" -> "九兆一千二百三十四亿零五百六"
            var numStr = input;
            // "9123400000560" -> "0009123400000560"
            var max = (int)Math.Ceiling((double)numStr.Length / 4);
            numStr = numStr.PadLeft(max*4, '0');
            // 按四位分割成 {"0560","0000","1234","0009"}
            var numStrs = new List<string>();
            for (int i = numStr.Length; i > 0; i -= 4)
            {
                numStrs.Add(numStr.Substring(i - 4, 4));
            }
            var index = -1;
            numStrs = numStrs.Select(full =>
            {
                index++;
                return $"{hanzi[full[0] - 48]}千{hanzi[full[1] - 48]}百{hanzi[full[2] - 48]}十{hanzi[full[3] - 48]}{units[index]}";
            }).ToList();
            numStrs.Reverse();
            var res = string.Join("", numStrs);
            // "三千零百零十零" -> "三千零零十零"
            res = Regex.Replace(res, "零[载正涧沟穰姊垓京兆亿万千百十]", "零");
            // "三千零零十零" -> "三千零十零"
            res = Regex.Replace(res, "零+", "零");
            // 去掉末尾的零 "250" -> "二百五十零"
            res = Regex.Replace(res, "(^零)|(零$)", "");
            // 可选 去掉末尾连续单位的最后一个单位 "二百五十"->"二百五","三万六千"->"三万六"
            res = Regex.Replace(res, "([载正涧沟穰姊垓京兆亿万千百])([一二三四五六七八九])([正涧沟穰姊垓京兆亿万千百十])$", "$1$2");
            // 可选 去掉开头“一十”的“一” "一十九"->"十九" "一十九万"->"十九万"
            res = Regex.Replace(res, "^一十", "十");
            return res;
        }

        /// <summary>
        /// 转换为中文数值字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToChinese(this int input)
        {
            return ToChinese(input.ToString());
        }
    }
}
