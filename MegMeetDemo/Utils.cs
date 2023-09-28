using System;
using System.Collections.Generic;
using System.Text;

namespace MegMeetDemo
{
    internal static class Utils
    {
        /// <summary>
        /// 将报文转换为十六进制格式字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (byte b in bytes)
            {
                var ch = b.ToString("X2");
                sb.Append(ch);
                i++;
                if (i % 2 == 0)
                {
                    sb.Append(" ");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 将十六进制格式字符串转换为等价字节数组
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this string msg)
        {
            List<byte> bs = new List<byte>();

            var data = msg.ToLower().Replace("0x", "").Replace(" ", "");
            int i = 0;
            while (true)
            {
                var b = Convert.ToByte(data.Substring(i, 2), 16);
                bs.Add(b);

                i += 2;
                if (i > data.Length - 2)
                {
                    break;
                }
            }
            return bs.ToArray();
        }
    }
}
