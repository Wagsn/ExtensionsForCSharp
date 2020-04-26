using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace System
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExtesion
    {
        /// <summary>
        /// In a specified input string, replaces all strings that match a specified regular
        /// expression with a specified replacement string.
        /// </summary>
        /// <param name="input">The string to search for a match.</param>
        /// <param name="pattern">The regular expression pattern to match.</param>
        /// <param name="replacement">The replacement string.</param>
        /// <returns></returns>
        public static string ReplaceByRegex(this string input, string pattern, string replacement)
        {
            return Regex.Replace(input, pattern, replacement);
        }

        

        //public class Numeral
        //{
        //    /// <summary>
        //    /// 字面量 三千五 三万零八 一亿零三万零二百五
        //    /// </summary>
        //    public string Literal { get; set; }
        //    /// <summary>
        //    /// 实际值 3500 30008 100030250
        //    /// </summary>
        //    public int Value { get; set; }
        //    /// <summary>
        //    /// 数量级 0-个 1-十 2-百 3-千 4-万 8-亿 12-兆 16-京
        //    /// </summary>
        //    public int Order { get; set; } = 1;
        //    /// <summary>
        //    /// 单位 个 十 百 千 万 亿 兆 京
        //    /// </summary>
        //    public string Unit { get; set; }
        //}
    }
}
