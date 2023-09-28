using System;
using System.Collections.Generic;
using System.Text;

namespace SpectreConsoleDemo
{
    public class ReceivedEventArgs : EventArgs
    {
        public byte[] Data { get; private set; }

        public ReceivedEventArgs(byte[] data)
        {
            Data = data;
        }
    }
}
