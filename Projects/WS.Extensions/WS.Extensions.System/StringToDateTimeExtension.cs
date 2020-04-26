using System;
using System.Text.RegularExpressions;

namespace WS.Extensions.System
{
    /// <summary>
    /// String -> DateTime
    /// </summary>
    public static class StringToDateTimeExtension
    {
        /// <summary>
        /// 将中文数字字符串转换为阿拉伯数字字符串
        /// 如："2019年十月十八日十二点半" -> "2019年10月18日12点半"
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertToArabicNnumerialString(this string source)
        {
            return source.Replace("三十", "30").Replace("四十", "40").Replace("五十", "50")
                .Replace("二十三", "23").Replace("二十二", "22").Replace("二十一", "21").Replace("二十", "20")
                .Replace("十一", "11").Replace("十二", "12").Replace("十三", "13").Replace("十四", "14").Replace("十五", "15")
                .Replace("十六", "16").Replace("十七", "17").Replace("十八", "18").Replace("十九", "19").Replace("十", "10")
                .Replace("十", "10").Replace("九", "9").Replace("八", "8").Replace("七", "7").Replace("六", "6").Replace("五", "5")
                .Replace("四", "4").Replace("三", "3").Replace("二", "2").Replace("一", "1")
                .Replace("零", "0");
        }

        /// <summary>
        /// 将各种时间字符串解析成时间`DateTime`对象
        /// </summary>
        /// <param name="timeStr">时间字符串</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="FormatException"/>
        /// <returns></returns>
        public static DateTime ToDateTime(this string timeStr)
        {
            if (timeStr == null) throw new ArgumentNullException(nameof(timeStr));
            // 识别文字样例："2019年04月13日 11:10", "2019/4/13 11:10", "2019.4.13 11:10", "12点半", "十二点半", "后天上午十点半"
            // 时间正则表达式 $@"月" @"(月)(日)(时)(分)(秒)"
            var digit = @"[\d〇零一二三四五六七八九]";
            var year = $@"{digit}{{2,4}}年";
            var month = $@"{digit}{{1,2}}月";
            // 使便准化 "yyyy年MM月dd日 H时mm分"
            var handleValue = timeStr.Replace("：", ":")
                .Replace("点半", "点30分")
                // 俗语转正式
                .Replace("号", "日")
                .Replace("点", "时")
                // 中文数字字符串转阿拉伯数字字符串
                .ConvertToArabicNnumerialString();
            // 处理明天后天问题 0-今天, 1-明天, 2-后天, 3-大后天
            var addDays = 0;
            if (handleValue.Contains("今天"))
            {
                handleValue = handleValue.Replace("今天", "");
            }
            if (handleValue.Contains("明天"))
            {
                addDays = 1;
                handleValue = handleValue.Replace("明天", "");
            }
            if (handleValue.Contains("后天"))
            {
                addDays = 2;
                handleValue = handleValue.Replace("后天", "");
            }
            if (handleValue.Contains("大后天"))
            {
                addDays = 3;
                handleValue = handleValue.Replace("大后天", "");
            }
            // 处理上午下午问题 0-24, 0-上午, 12-下午
            var addHours = 0;
            if (handleValue.Contains("上午"))
            {
                handleValue = handleValue.Replace("上午", "");
            }
            if (handleValue.Contains("下午"))
            {
                addHours = 12;
                handleValue = handleValue.Replace("下午", "");
            }
            if (handleValue.Contains("晚上"))
            {
                addHours = 12;
                handleValue = handleValue.Replace("晚上", "");
            }
            // "xxxxx 12时20" -> "xxxxx 12时20分"
            handleValue = Regex.Replace(handleValue, @"^(.*?时)(\d+)(?!分)$", "$1$2分");
            // 20200115 "Xx月20 Xxx"->"Xx月20日 Xxx"
            handleValue = Regex.Replace(handleValue, @"^(.*?月)(\d+)(?<!日)([^日]*)$", "$1$2日$3");
            DateTime time = DateTime.Parse(handleValue);
            time = time.AddDays(addDays);
            if (time.Hour < 12)
            {
                time = time.AddHours(addHours);
            }
            return time;
        }
    }
}
