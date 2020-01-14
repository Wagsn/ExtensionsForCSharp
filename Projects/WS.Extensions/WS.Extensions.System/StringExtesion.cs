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

        private static readonly char[] digitsC = { '一', '二', '三', '四', '五', '六', '七', '八', '九' };

        ///// <summary>
        ///// 复数的中文数值字符串转阿拉伯数值字符串
        ///// 如： "一万三千一百五十六，二百五，一万零三。" -> "13156，250，10003。"
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public static string ToArabicNumeralMult(this string input)
        //{
        //    // 中文数字字符串转阿拉伯数字字符串策略 以四级为进 万 亿 兆 京 从高向低拆
        //    // "一万三千一百五十六，二百五，一万零三。"
        //    // 拆分
        //    // -> { {"一万三千一百五十六", isNumeral:true}, {"，", false}, {"二百五", true}, {"，", false}, {"一万零三", true}, {"。", false}}
        //    // 转化 如 "一百亿零二百万三千零四"
        //    //    -> 拆分 按 "亿" "万" 递归简并 为 "一百" "亿" "零二百" "万" "三千零四"
        //    //    -> 组合 100*10^8+200*10^4+3004 -> 100,0200,3004
        //    // -> { { {"13156", true}, {"，", false}, {"250", true}, {"，", false}, {"10003", true}, {"。", false}}
        //    // 组合
        //    // -> "13156，250，10003。"
        //    return input;
        //}

        // 拍米(pm,10^15),兆米(Mm,10^6),千米(km),分米(dm),厘米(cm),毫米(mm),微米(μm),纳米(nm),皮米(pm),飞米(fm),阿米(am, 10^-18),仄米(zepto,10^-21),幺米(ym,10^-24)
        // 分(10^-1)厘(10^-2)毫(10^-3)丝(10^-4)忽(10^-5)微(10^-6)纤(10^-7)沙(10^-8)尘(10^-9)埃(10^-10)渺(10^-11)漠(10^-12)模糊、逡巡、须臾、瞬息、弹指、刹那、六德、虚空、清净、阿赖耶、阿摩罗、涅槃寂静
        // https://baike.baidu.com/item/%E6%95%B0%E5%AD%97%E5%8D%95%E4%BD%8D%E5%88%B6/394982?fr=aladdin
        // 亿,兆,京,垓,姊,穰,沟,涧,正,载
        // 每个大数单位进制原则在万以下都为十进制；万以上出现分化。具体分为上数（自乘系统）；中数（万进系统）；下数（十进系统）。
        // 上数(自乘系统)：万万为亿，亿亿为兆，兆兆为京；万(10^4=10^(2^2)),亿(10^8=10^(2^3)),兆(10^16=10^(2^4)),京(10^(2^5)),...,载(10^4096=10^(2^12))
        // 中数(万进系统)：万万为亿，万亿为兆，万兆为京；万(10^4),亿(10^8),兆(10^12),京(10^(4*4)),...,载(10^(11*4))
        //               ：亿,兆,京,垓,姊,穰,沟,涧,正,载,极(10^48)一恒何沙(10^52)阿僧祇(10^56)那由他(10^60)不可思议(10^64)无量(10^68)大数(10^72)
        //               ：在日本，韩国，新加坡以及台湾地区得以保留和沿用。
        // 下数(十进系统)：十万为亿，百万为兆(10^6=100*10^4)，千万为京。
        // 中文数值字符串转阿拉伯数值字符串
        // "三万万" -> "300000000"
        /// <summary>
        /// Chinese numeric string to Arabic numeric string
        /// </summary>
        /// <param name="input">Chinese numeric string, e.g. "九亿八千七百六十五万四千三百二十一"</param>
        /// <returns></returns>
        public static string ToArabicNumeral(this string input)
        {
            var output = "";
            if (input.Contains("京"))
            {
                // "一京" "一京零一" "一京京"
                var numStrs = input.Split('京');
                var order = 16;
                for(int i = numStrs.Length - 1; i >= 0; i--)
                {
                    // 如果是空的 如："一京" 加16个0
                    if (string.IsNullOrWhiteSpace(numStrs[i]))
                    {
                        output = "".PadLeft(order, '0') + output;
                        continue;
                    }
                    // 如果最后一位是单个的数字 如："一京三" 加该数字以及15个0
                    if((i == numStrs.Length - 1) && numStrs.Length == 1 && numStrs[i].All(a => digitsC.Any(b => a == b)))
                    {
                        output = $"{Char2Int(numStrs[i][0])}".PadRight(order, '0') + output;
                        continue;
                    }
                    // 其它情况 如："一京零一" 这里需要将"零一"补充到16位
                    output = ToArabicNumeral(numStrs[i]).PadLeft(order, '0') + output;
                }
            }
            else if (input.Contains('兆'))
            {
                // "一兆" "一兆零一" "一兆兆"
                var numStrs = input.Split('兆');
                var order = 12;
                for (int i = numStrs.Length - 1; i >= 0; i--)
                {
                    // 如果是空的 如："一兆" 加12个0
                    if (string.IsNullOrWhiteSpace(numStrs[i]))
                    {
                        output = "".PadLeft(order, '0') + output;
                        continue;
                    }
                    // 如果最后一位是单个的数字 如："一兆三" 加该数字以及11个0
                    if ((i == numStrs.Length - 1) && numStrs[i].Length == 1 && numStrs[i].All(a => digitsC.Any(b => a == b)))
                    {
                        output = $"{Char2Int(numStrs[i][0])}".PadRight(order, '0') + output;
                        continue;
                    }
                    // 其它情况 如："一兆零一" 这里需要将"零一"补充到12位
                    output = ToArabicNumeral(numStrs[i]).PadLeft(order, '0') + output;
                }
            }
            else if (input.Contains('亿'))
            {
                var numStrs = input.Split('亿');
                var order = 8;
                for (int i = numStrs.Length - 1; i >= 0; i--)
                {
                    if (string.IsNullOrWhiteSpace(numStrs[i]))
                    {
                        output = "".PadLeft(order, '0') + output;
                        continue;
                    }
                    if ((i == numStrs.Length - 1) && numStrs[i].Length == 1 && numStrs[i].All(a => digitsC.Any(b => a == b)))
                    {
                        output = $"{Char2Int(numStrs[i][0])}".PadRight(order, '0') + output;
                        continue;
                    }
                    output = ToArabicNumeral(numStrs[i]).PadLeft(order, '0') + output;
                }
            }
            // "xxx一京五" 简写 -> 全写 "xxx一京五千亿" 
            // "xxx一亿五" 简写 -> 全写 "xxx一亿五千万"
            // "xxx一万五" 简写 -> 全写 "xxx一万五千"
            // "xxx一千五" 简写 -> 全写 "xxx一千五百"
            // "xxx一百五" 简写 -> 全写 "xxx一百五十"
            // TODO 大于四位的数值拆分 当不是最后一节，但是末尾是中文数字而倒数第二位不是零，则该数字不合法 如："一百三万零一" 非法 -> "一百三十万零一" 合法
            else if (input.Contains('万'))
            {
                var numStrs = input.Split('万');
                var order = 4;
                for (int i = numStrs.Length - 1; i >= 0; i--)
                {
                    if (string.IsNullOrWhiteSpace(numStrs[i]))
                    {
                        output = "".PadLeft(order, '0') + output;
                        continue;
                    }
                    if ((i == numStrs.Length - 1) && numStrs[i].Length == 1 && numStrs[i].All(a => digitsC.Any(b => a == b)))
                    {
                        output = $"{Char2Int(numStrs[i][0])}".PadRight(order, '0') + output;
                        continue;
                    }
                    output = ToArabicNumeral(numStrs[i]).PadLeft(order, '0') + output;
                }
            }
            // TODO 当数值小于四位时，拆分后的左侧只能有一位数字 如："三千六" 合法 "二十三千零六" 非法 -> "二万三千零六" 合法
            else if (input.Contains('千'))
            {
                var numStrs = input.Split('千');
                var order = 3;
                for (int i = numStrs.Length - 1; i >= 0; i--)
                {
                    if (string.IsNullOrWhiteSpace(numStrs[i]))
                    {
                        output = "".PadLeft(order, '0') + output;
                        continue;
                    }
                    if ((i == numStrs.Length - 1) && numStrs[i].Length == 1 && numStrs[i].All(a => digitsC.Any(b => a == b)))
                    {
                        output = $"{Char2Int(numStrs[i][0])}".PadRight(order, '0') + output;
                        continue;
                    }
                    output = ToArabicNumeral(numStrs[i]).PadLeft(order, '0') + output;
                }
            }
            // TODO 当数值小于四位时，拆分后的左侧只能有一位数字 如："三千六" 合法 "二十三千零六" 非法 -> "二万三千零六" 合法
            else if (input.Contains('百'))
            {
                var numStrs = input.Split('百');
                var order = 2;
                for (int i = numStrs.Length - 1; i >= 0; i--)
                {
                    if (string.IsNullOrWhiteSpace(numStrs[i]))
                    {
                        output = "".PadLeft(order, '0') + output;
                        continue;
                    }
                    if ((i == numStrs.Length - 1) && numStrs[i].Length == 1 && numStrs[i].All(a => digitsC.Any(b => a == b)))
                    {
                        output = $"{Char2Int(numStrs[i][0])}".PadRight(order, '0') + output;
                        continue;
                    }
                    output = ToArabicNumeral(numStrs[i]).PadLeft(order, '0') + output;
                }
            }
            // TODO 当数值小于四位时，拆分后的左侧只能有一位数字 如："三千六" 合法 "二十三千零六" 非法 -> "二万三千零六" 合法
            else if (input.Contains('十'))
            {
                var numStrs = input.Split('十');
                var order = 1;
                for (int i = numStrs.Length - 1; i >= 0; i--)
                {
                    if (string.IsNullOrWhiteSpace(numStrs[i]))
                    {
                        output = "".PadLeft(order, '0') + output;
                        continue;
                    }
                    if ((i == numStrs.Length - 1) && numStrs[i].Length == 1 && numStrs[i].All(a => digitsC.Any(b => a == b)))
                    {
                        output = $"{Char2Int(numStrs[i][0])}".PadRight(order, '0') + output;
                        continue;
                    }
                    output = ToArabicNumeral(numStrs[i]).PadLeft(order, '0') + output;
                }
            }
            // 个位
            else
            {
                return output = input;
            }
            // 将不含单位的中文数字处理为阿拉伯数字
            return output.Replace("零", "0")
                .Replace("一", "1")
                .Replace("壹", "1")
                .Replace("二", "2")
                .Replace("两", "2")
                .Replace("贰", "2")
                .Replace("三", "3")
                .Replace("叁", "3")
                .Replace("四", "4")
                .Replace("肆", "4")
                .Replace("五", "5")
                .Replace("伍", "5")
                .Replace("六", "6")
                .Replace("陆", "6")
                .Replace("七", "7")
                .Replace("柒", "7")
                .Replace("八", "8")
                .Replace("捌", "8")
                .Replace("九", "9")
                .Replace("玖", "9")
                .TrimStart('0');
        }

        private static readonly Dictionary<char, int> charDic 
            = new Dictionary<char, int> { { '一', 1 }, { '二', 2 }, { '三', 3 }, { '四', 4 }, { '五', 5 }, { '六', 6 }, { '七', 7 }, { '八', 8 }, { '九', 9 }, { '零', 0 } }; 

        private static int Char2Int(char c)
        {
            if (charDic.ContainsKey(c))
            {
                return charDic[c];
            }
            throw new Exception("输入的字符不属于中文数字字符");
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
