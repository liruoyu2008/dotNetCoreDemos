using HJ212_2017Demo.Protocol.CommandParameters.SubCommandParameter;
using HJ212_2017Demo.Protocol.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HJ212_2017Demo.Protocol.CommandParameters
{
    /// <summary>
    /// 空参CP指令
    /// </summary>
    public abstract class CP
    {
        /// <summary>
        /// 转换为指令参数字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var props = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (props.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new();
            foreach (var prop in props)
            {
                if (prop.Name != "SubCP")
                {
                    var k = prop.Name.ValidateCP();
                    var v = prop.GetValue(this).ToCPString(prop.Name).ValidateCP();
                    if (v != null)
                    {
                        sb.Append($"{k}={v};");
                    }
                }
                // SubCP
                else
                {
                    var dic = prop.GetValue(this) as Dictionary<string, SubCP>;
                    if (dic == null)
                    {
                        continue;
                    }

                    foreach (var k in dic)
                    {
                        var subCPStr = k.Value.ToString();
                        if (subCPStr != null)
                        {
                            sb.Append($"{subCPStr};");
                        }
                    }
                    continue;
                }
            }
            return sb.ToString().TrimEnd(';');
        }

        /// <summary>
        /// 将字符串转换为指定的指令参数对象
        /// </summary>
        /// <param name="cpStr"></param>
        /// <returns></returns>
        public static T Parse<T>(string cpStr) where T : CP, new()
        {
            var cp = new T();
            Dictionary<string, SubCP> subCPDic = new Dictionary<string, SubCP>();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var list1 = cpStr.Split(';');
            foreach (var item in list1)
            {
                if (!item.Contains(','))
                {
                    // 不含等于号跳过
                    if (!item.Contains('='))
                    {
                        continue;
                    }

                    var kv = item.Split('=');
                    var k = kv[0].ValidateCP();
                    var v = kv[1].ValidateCP();
                    var prop = props.FirstOrDefault(it => it.Name == k);
                    if (prop != null)
                    {
                        prop.SetValue(cp, v.ToCPValue(prop.Name, prop.PropertyType));
                    }
                }
                // SubCP
                else
                {
                    var subCP = SubCP.Parse(typeof(T), item);
                    subCPDic.Add(subCP.Name, subCP);
                }
            }

            if (subCPDic.Count > 0)
            {
                var prop = props.FirstOrDefault(it => it.Name == "SubCP");
                if (prop != null)
                {
                    prop.SetValue(cp, subCPDic);
                }
            }

            return cp;
        }

        /// <summary>
        /// 将字符串转换为指定命令号、命令类型所对应的指令参数对象
        /// </summary>
        /// <param name="cn"></param>
        /// <param name="ct"></param>
        /// <param name="cpStr"></param>
        /// <returns></returns>
        public static CP Parse(int cn, CommandType ct, string cpStr)
        {
            // todo 补全映射表
            switch ((cn, ct))
            {
                case (2011, CommandType.UPLOAD): return Parse<CP2011_Upload>(cpStr);
                default: return null;
            }
        }
    }
}
