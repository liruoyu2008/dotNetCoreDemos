using System;
using System.Collections.Generic;
using System.Text;

namespace MegMeetDemo
{
    /// <summary>
    /// 焊接参数.
    /// </summary>
    public class WeldParameters
    {
        /// <summary>
        /// Gets or sets 焊丝直径.
        /// </summary>
        public float Diameter { get; set; }

        /// <summary>
        /// Gets or sets 焊材类型.
        /// </summary>
        public string MaterialType { get; set; }

        /// <summary>
        /// Gets or sets 焊接控制.
        /// </summary>
        public string WeldControl { get; set; }

        /// <summary>
        /// Gets or sets 焊接方法.
        /// </summary>
        public string WeldMethod { get; set; }

        /// <summary>
        /// Gets or sets 操作控制.
        /// </summary>
        public string OperationContorol { get; set; }

        /// <summary>
        /// Gets or sets 起弧时间上限.
        /// </summary>
        public float ArcUp { get; set; }

        /// <summary>
        /// Gets or sets 起弧时间下限.
        /// </summary>
        public float ArcDown { get; set; }

        /// <summary>
        /// Gets or sets 焊接电流中心值.
        /// </summary>
        public short CurrentCenter { get; set; }

        /// <summary>
        /// Gets or sets 焊接电流微调范围.
        /// </summary>
        public short CurrentTrim { get; set; }

        /// <summary>
        /// Gets or sets 焊接电压中心值.
        /// </summary>
        public float Voltage { get; set; }

        /// <summary>
        /// Gets or sets 焊接电压微调范围.
        /// </summary>
        public float VoltageTrim { get; set; }
    }

    /// <summary>
    /// 焊接参数的值映射表
    /// </summary>
    internal static class WeldParameterMaps
    {
        public static Dictionary<byte, float> DiameterSet = new Dictionary<byte, float>()
        {
            { 0 , 0.8f },
            { 1 , 0.9f },
            { 2 , 1.0f },
            { 3 , 1.2f },
            { 4 , 1.4f },
            { 5 , 1.6f },
        };

        public static Dictionary<byte, string> MatetialTypeSet = new Dictionary<byte, string>()
        {
            { 0 , "100%CO2实芯碳钢" },
            { 1 , "100%CO2药芯碳钢" },
            { 2 , "100%CO2药芯不锈钢" },
            { 3 , "80%Ar+20%CO2实芯碳钢" },
            { 4 , "97.5%Ar+2.5%CO2实芯不锈钢" },
            { 5 , "100%Ar纯铝" },
            { 6 , "100%Ar铝硅合金" },
            { 7 , "100%Ar纯铝镁合金" },
        };

        public static Dictionary<byte, string> WeldControlSet = new Dictionary<byte, string>()
        {
            { 0 , "2步" },
            { 1 , "4步" },
            { 2 , "特殊4步" },
            { 3 , "点焊" },
        };

        public static Dictionary<byte, string> WeldMethodSet = new Dictionary<byte, string>()
        {
            { 0 , "直流" },
            { 1 , "脉冲" },
            { 2 , "双脉冲" },
            { 3 , "电焊条" },
        };
    }
}
