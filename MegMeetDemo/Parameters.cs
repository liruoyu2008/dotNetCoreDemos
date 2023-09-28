using System;
using System.Collections.Generic;
using System.Text;

namespace MegMeetDemo
{
    /// <summary>
    /// 焊接参数.
    /// </summary>
    public class Parameters
    {
        /// <summary>
        /// Gets or sets 主故障码.
        /// </summary>
        public byte ErrorCode1 { get; set; }

        /// <summary>
        /// Gets or sets 辅助故障码.
        /// </summary>
        public byte ErrorCode2 { get; set; }

        /// <summary>
        /// Gets or sets 设定焊接电流(A).
        /// </summary>
        public ushort SetCurrent { get; set; }

        /// <summary>
        /// Gets or sets 设定焊接电压(0.1V).
        /// </summary>
        public ushort SetVoltage { get; set; }

        /// <summary>
        /// Gets or sets 送丝速度(0.1米/分钟).
        /// </summary>
        public ushort WireSpeed { get; set; }

        /// <summary>
        /// Gets or sets 实际电流(A).
        /// </summary>
        public ushort Current { get; set; }

        /// <summary>
        /// Gets or sets 实际电压(0.1V).
        /// </summary>
        public ushort Voltage { get; set; }
    }
}
