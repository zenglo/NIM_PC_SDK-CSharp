/** @file NIMGlobalAPI.cs
  * @brief NIM SDK提供的一些全局接口
  * @copyright (c) 2015, NetEase Inc. All rights reserved
  * @author Harrison
  * @date 2015/12/8
  */

using System;
using System.Runtime.InteropServices;

namespace NIM
{
    public class GlobalAPI
    {
        #region NIM C SDK native methods

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_global_free_str_buf", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void nim_global_free_str_buf(IntPtr str);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_global_free_buf", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void nim_global_free_buf(IntPtr data);

        #endregion

        /// <summary>
        /// 释放SDK内部分配的内存
        /// </summary>
        /// <param name="str">由SDK内部分配内存的字符串</param>
        public static void FreeStringBuffer(IntPtr str)
        {
            nim_global_free_str_buf(str);            
        }

        /// <summary>
        /// 释放SDK内部分配的内存
        /// </summary>
        /// <param name="data">由SDK内部分配的内存</param>
        public static void FreeBuffer(IntPtr data)
        {
            nim_global_free_buf(data);
        }
    }
}
