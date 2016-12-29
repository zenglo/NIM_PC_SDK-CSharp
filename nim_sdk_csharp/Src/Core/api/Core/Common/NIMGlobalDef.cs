/** @file NIMGlobalDef.cs
  * @brief NIM SDK提供的一些全局定义
  * @copyright (c) 2015, NetEase Inc. All rights reserved
  * @author Harrison
  * @date 2015/12/8
  */

using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace NIM
{
    internal class NIMGlobal
    {
        /// <summary>
        ///     The NIM native DLL
        /// </summary> 
#if UNITY
#if UNITY_IOS
            public const string NIMNativeDLL = "__Internal";
#elif UNITY_ANDROID || UNITY_STANDALONE_LINUX
            public const string NIMNativeDLL = "nim";
#elif UNITY_STANDALONE_WIN
        public const string NIMNativeDLL = "nim";
#endif

#elif DEBUG
        public const string NIMNativeDLL = "nim.dll";
#else
        public const string NIMNativeDLL = "nim.dll";
#endif
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void JsonTransportCb(string jsonParams, IntPtr userData);
    }

    delegate void nim_global_net_detect_cb_func(int rescode,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))] string json_params, 
        IntPtr user_data);

    /// <summary>
    /// 网络探测类型
    /// </summary>
    public enum NIMNetDetectType
    {
        kNIMNetDetectTypeUdp = 0, //默认udp探测
    }

    /// <summary>
    /// 网络探测错误
    /// </summary>
    public enum NetDetectionRes
    {
        /// <summary>
        ///流程错误 
        /// </summary>
        ProcessError = 0,
        /// <summary>
        ///成功 
        /// </summary>
        Success = 200,
        /// <summary>
        ///非法请求格式 
        /// </summary>
        InvalidRequest = 400,
        /// <summary>
        /// 请求数据不对
        /// </summary>
        DataError = 417,
        /// <summary>
        /// ip为内网ip
        /// </summary>
        InnerIP = 606,
        /// <summary>
        /// 频率超限
        /// </summary>
        OutOfLimit = 607,
        /// <summary>
        ///探测类型错误 
        /// </summary>
        TypeError = 20001,
        /// <summary>
        ///ip错误 
        /// </summary>
        IPError = 20002,
        /// <summary>
        ///sock错误 
        /// </summary>
        SocketError = 20003
    }

    /// <summary>
    /// 网络探测结果
    /// </summary>
    public class NetDetectResult : NimUtility.NimJsonObject<NetDetectResult>
    {
        [JsonProperty("loss")]
        public int Loss { get; set; }

        [JsonProperty("rttmax")]
        public int RTTMax { get; set; }

        [JsonProperty("rttmin")]
        public int RTTMin { get; set; }

        [JsonProperty("rttavg")]
        public int RTTAvg { get; set; }

        [JsonProperty("rttmdev")]
        public int RTTMdev { get; set; }

        [JsonProperty("detailinfo")]
        public string Info { get; set; }
    }
}