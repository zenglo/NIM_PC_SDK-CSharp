/** @file NIMHttpDef.cs
  * @brief NIM HTTP提供的http传输相关的定义
  * @copyright (c) 2015, NetEase Inc. All rights reserved
  * @author gq
  * @date 2015/12/8
  */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace NIMHttp
{
    public class NIMHttp
    {
        /// <summary>
        /// The NIM Http native DLL
        /// </summary>
        public const string NIMHttpNativeDLL = "nim_tools_http.dll";
        
        /// <summary>
        /// http传输结果回调
        /// </summary>
        /// <param name="user_data">回传的自定义数据</param>
        /// <param name="result">传输结果，true代表传输成功，false代表传输失败</param>
        /// <param name="response_code">http响应码</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CompletedCb(IntPtr user_data, bool result, int response_code);
        
        /// <summary>
        /// http请求结果回调
        /// </summary>
        /// <param name="user_data">回传的自定义数据</param>
        /// <param name="result">传输结果，true代表传输成功，false代表传输失败</param>
        /// <param name="response_code">http响应码</param>
        /// <param name="response_content">http响应实体内容</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ResponseCb(IntPtr user_data, bool result, int response_code, string response_content);
        
        /// <summary>
        /// http传输进度回调
        /// </summary>
        /// <param name="user_data">回传的自定义数据</param>
        /// <param name="uploaded_size">已经上传的字节数</param>
        /// <param name="total_upload_size">总的待上传的字节数</param>
        /// <param name="downloaded_size">已经下载的字节数</param>
        /// <param name="total_download_size">总的待下载的字节数</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ProgressCb(IntPtr user_data, Int64 uploaded_size, Int64 total_upload_size, Int64 downloaded_size, Int64 total_download_size);
        
    }
}
