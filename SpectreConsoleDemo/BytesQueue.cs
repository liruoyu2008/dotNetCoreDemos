using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace SpectreConsoleDemo
{
    /// <summary>
    /// 处理字节的队列，采用一维数组实现，允许字节序列(而不是单字节)入队或出队.
    /// </summary>
    public class BytesQueue
    {
        /// <summary>
        /// 队列尺寸.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// 队列是否为空.
        /// </summary>
        public bool IsEmpty { get; private set; }


        // 环形结构的首位标记
        int _head = 0;
        int _tail = 0;

        // 数组
        byte[] _buffer;

        // 锁
        readonly object _locker = new object();

        /// <summary>
        /// 处理字节的队列，采用一维数组实现，允许字节序列(而不是单字节)入队或出队.
        /// </summary>
        /// <param name="size">队列最大容量</param>
        /// <exception cref="Exception"></exception>
        public BytesQueue(int size)
        {
            if (size <= 0)
            {
                throw new Exception($"Specified size must be a positive number.");
            }

            Size = size;
            _buffer = new byte[Size];
            IsEmpty = true;
        }

        /// <summary>
        /// 获取已用容量.
        /// </summary>
        /// <returns></returns>
        private int GetUsedCount()
        {
            if (_head < _tail)
            {
                return (_tail - _head);
            }
            else if (_head > _tail)
            {
                return (_tail - _head) + Size;
            }
            else
            {
                return IsEmpty ? 0 : Size;
            }
        }

        /// <summary>
        /// 获取剩余可用容量.
        /// </summary>
        /// <returns></returns>
        private int GetRestCount()
        {
            return Size - GetUsedCount();
        }

        /// <summary>
        /// 入队，并返回本次入队的长度. 若可用容量不够，会直接丢弃整个输入数组.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public int EnQueue(byte[] data)
        {
            lock (_locker)
            {
                if (data == null || data.Length == 0 || data.Length > GetRestCount())
                    return 0;

                // 尾标右侧可直接放下
                if (data.Length <= Size - _tail)
                {
                    Array.Copy(data, 0, _buffer, _tail, data.Length);
                }
                else
                {
                    // 尾标右侧不够，则折回_buffer起点，分两段放
                    Array.Copy(data, 0, _buffer, _tail, Size - _tail);
                    Array.Copy(data, Size - _tail, _buffer, 0, data.Length - (Size - _tail));
                }

                // 移动尾标
                _tail = (_tail + data.Length) % Size;
                IsEmpty = false;

                return data.Length;
            }
        }

        /// <summary>
        /// 指定长度出队，出队数据将被移除出队列. 若无数据或数据长度不够，则返回null.
        /// </summary>
        public byte[] DeQueue(int len)
        {
            lock (_locker)
            {
                return GetData(len, true);
            }
        }

        /// <summary>
        /// 取出队列中的元素，但并不将其从队列移除.
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public byte[] Peek(int len = 1)
        {
            lock (_locker)
            {
                return GetData(len, false);
            }
        }

        /// <summary>
        /// 取数据，并指定是否将这个数据移除出队列.
        /// </summary>
        /// <param name="len"></param>
        /// <param name="removeFlag"></param>
        /// <returns></returns>
        private byte[] GetData(int len, bool removeFlag)
        {
            if (len <= 0 || len > GetUsedCount())
                return null;

            var res = new byte[len];

            // 头标右侧可直接取出
            if (len <= Size - _head)
            {
                Array.Copy(_buffer, _head, res, 0, len);
            }
            else
            {
                // 头标右侧不够，则折回_buffer起点，分两段取
                Array.Copy(_buffer, _head, res, 0, Size - _head);
                Array.Copy(_buffer, 0, res, Size - _head, len - (Size - _head));
            }

            // 移动头标
            if (removeFlag)
            {
                _head = (+_head + len) % Size;
                IsEmpty = _head == +_tail;
            }

            return res;
        }

        /// <summary>
        /// 在队列中寻找指定的字节序列，并返回找到的首个结果相对于队列头的位置. 若未找到，返回-1.
        /// </summary>
        /// <returns></returns>
        public int Find(byte[] pattern)
        {
            lock (_locker)
            {
                if (pattern == null || pattern.Length == 0 || pattern.Length > GetUsedCount())

                    return -1;

                // 根据有效载荷长度进行外循环
                for (int i = 0; i < GetUsedCount() - pattern.Length + 1; i++)
                {
                    // 根据待匹配子串进行内循环
                    for (int j = 0; j < pattern.Length; j++)
                    {
                        int index = (_head + i + j) % Size;
                        if (pattern[j] != _buffer[index])
                        {
                            break;
                        }

                        // 若内循环匹配至子串尾部，则匹配成功
                        if (j == pattern.Length - 1)
                        {
                            return (index - _head + Size - j) % Size;
                        }
                    }
                }

                return -1;
            }
        }
    }
}
