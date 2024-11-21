
namespace TimeTrack_Pro.Helper
{
    public class DateTimeHelper
    {
        /// <summary>
        /// 获取当前月的天数
        /// </summary>
        /// <returns></returns>
        public static int GetDays()
        {
            // 获取当前月份第一天
            DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            // 获取下个月的第一天
            DateTime firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);
            // 获取本月最后一天
            DateTime lastDayOfMonth = firstDayOfNextMonth.AddDays(-1);
            // 获取本月的天数
            int daysInMonth = lastDayOfMonth.Day;
            return daysInMonth;
        }

        /// <summary>
        /// 获取今年某月的天数
        /// </summary>
        /// <returns></returns>
        public static int GetDays(int month)
        {
            month = ((month > 12) || (month < 1)) ? 1 : month;
            // 获取当前月份第一天
            DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, month, 1);
            // 获取下个月的第一天
            DateTime firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);
            // 获取本月最后一天
            DateTime lastDayOfMonth = firstDayOfNextMonth.AddDays(-1);
            // 获取本月的天数
            int daysInMonth = lastDayOfMonth.Day;
            return daysInMonth;
        }

        /// <summary>
        /// 获取当月某天的星期
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static int GetWeek(int day)
        {
            day = (day < 0 || day > 31) ? 1 : day;
            DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, day);
            return (int)firstDayOfMonth.DayOfWeek;
        }

        /// <summary>
        ///  获取某月某天的星期
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static int GetWeek(int month, int day)
        {
            month = (month < 0 || month > 12) ? DateTime.Today.Month : month;
            day = (day < 0 || day > 31) ? 1 : day;
            DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, month, day);
            return (int)firstDayOfMonth.DayOfWeek;
        }

        /// <summary>
        /// 获取当月天数并按星期进行排列的集合
        /// </summary>
        /// <returns></returns>
        public static string[] GetDaysByWeek()
        {
            // 获取当前月份第一天
            DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            // 获取这一天时星期几
            DayOfWeek dayOfWeek = firstDayOfMonth.DayOfWeek;
            // 获取下个月的第一天
            DateTime firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);
            // 获取本月最后一天
            DateTime lastDayOfMonth = firstDayOfNextMonth.AddDays(-1);
            // 获取本月的天数
            int daysInMonth = lastDayOfMonth.Day;
            string[] days = new string[daysInMonth];
            string[] weeks = { "日", "一", "二", "三", "四", "五", "六" };
            for (int i = 0; i < daysInMonth; i++)
            {
                days[i] = string.Format("{0:00}", i + 1) + " " + weeks[((int)dayOfWeek + i) % 7];
            }
            return days;
        }

        /// <summary>
        /// 获取某月天数并按星期进行排列的集合
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static string[] GetDaysByWeek(int month)
        {
            month = (month > 12 || month < 1) ? DateTime.Today.Month : month;
            // 获取当前月份第一天
            DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, month, 1);
            // 获取这一天时星期几
            DayOfWeek dayOfWeek = firstDayOfMonth.DayOfWeek;
            // 获取下个月的第一天
            DateTime firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);
            // 获取本月最后一天
            DateTime lastDayOfMonth = firstDayOfNextMonth.AddDays(-1);
            // 获取本月的天数
            int daysInMonth = lastDayOfMonth.Day;
            string[] days = new string[daysInMonth];
            string[] weeks = { "日", "一", "二", "三", "四", "五", "六" };
            for (int i = 0; i < daysInMonth; i++)
            {
                days[i] = string.Format("{0:00}", i + 1) + " " + weeks[((int)dayOfWeek + i) % 7];
            }
            return days;
        }
    }
}
