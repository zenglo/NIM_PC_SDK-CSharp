/** @file NIMGlobalDef.cs
  * @brief NIM SDK提供的一些全局定义
  * @copyright (c) 2015, NetEase Inc. All rights reserved
  * @author Harrison
  * @date 2015/12/8
  */

using System;
using System.Runtime.InteropServices;

namespace NIM
{
    class NIMGlobal
    {
        /// <summary>
        /// The NIM native DLL
        /// </summary>
        public const string NIMNativeDLL = "nim.dll";

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void JsonTransportCb(string jsonParams, IntPtr userData);
    }
}
