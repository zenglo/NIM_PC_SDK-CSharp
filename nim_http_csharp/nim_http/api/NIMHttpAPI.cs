/** @file NIMHttpDef.cs
  * @brief NIM HTTP提供的http传输相关接口
  * @copyright (c) 2015, NetEase Inc. All rights reserved
  * @author gq
  * @date 2015/12/8
  */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using NIM;

namespace NIMHttp
{
    public class HttpAPI
    {
        //引用C中的方法
        #region NIM Http C SDK native methods

        [DllImport(NIMHttp.NIMHttpNativeDLL, EntryPoint = "nim_http_init", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void nim_http_init();

        [DllImport(NIMHttp.NIMHttpNativeDLL, EntryPoint = "nim_http_uninit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void nim_http_uninit();

        [DllImport(NIMHttp.NIMHttpNativeDLL, EntryPoint = "nim_http_post_request", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int nim_http_post_request(IntPtr request_handle);

        [DllImport(NIMHttp.NIMHttpNativeDLL, EntryPoint = "nim_http_remove_request", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void nim_http_remove_request(int request_id);

        [DllImport(NIMHttp.NIMHttpNativeDLL, EntryPoint = "nim_http_create_download_file_request", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr nim_http_create_download_file_request([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string url, 
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string download_file_path,
            NIMHttp.CompletedCb complete_cb, IntPtr user_data);

        [DllImport(NIMHttp.NIMHttpNativeDLL, EntryPoint = "nim_http_create_download_file_range_request", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr nim_http_create_download_file_range_request([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string url, 
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string download_file_path,
            Int64 range_start, NIMHttp.CompletedCb complete_cb, IntPtr user_data);

        [DllImport(NIMHttp.NIMHttpNativeDLL, EntryPoint = "nim_http_create_request", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr nim_http_create_request([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string url, 
            IntPtr post_body, int post_body_size, NIMHttp.ResponseCb response_cb, IntPtr user_data);
        
        [DllImport(NIMHttp.NIMHttpNativeDLL, EntryPoint = "nim_http_add_request_header", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void nim_http_add_request_header(IntPtr request_handle,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string key,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string value);

        [DllImport(NIMHttp.NIMHttpNativeDLL, EntryPoint = "nim_http_set_request_progress_cb", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void nim_http_set_request_progress_cb(IntPtr request_handle, NIMHttp.ProgressCb progress_cb, IntPtr user_data);

        [DllImport(NIMHttp.NIMHttpNativeDLL, EntryPoint = "nim_http_set_request_method_as_post", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void nim_http_set_request_method_as_post(IntPtr request_handle);

        [DllImport(NIMHttp.NIMHttpNativeDLL, EntryPoint = "nim_http_set_timeout", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void nim_http_set_timeout(IntPtr request_handle, int timeout_ms);

        #endregion
        
        /// <summary>
        /// NIM HTTP 初始化
        /// </summary>
        public static void Init()
        {
            nim_http_init();
        }

        /// <summary>
        /// NIM HTTP 反初始化
        /// </summary>
        public static void UnInit()
        {
            nim_http_uninit();
        }
        
        /// <summary>
        /// 发起任务
        /// </summary>
        /// <param name="request_handle">http任务句柄</param>
        /// <returns>任务id</returns>
        public static int PostRequest(IntPtr request_handle)
        {
            return nim_http_post_request(request_handle);
        }
        
        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="http_request_id">任务id</param>
        public static void RemoveRequest(int http_request_id)
        {
            nim_http_remove_request(http_request_id);
        }

        /// <summary>
        /// 创建下载文件任务
        /// </summary>
        /// <param name="url">资源地址</param>
        /// <param name="download_file_path">下载文件保存的本地路径</param>
        /// <param name="complete_cb">结束回调</param>
        /// <param name="user_data">自定义数据</param>
        /// <returns>http任务句柄</returns>
        public static IntPtr CreateDownloadFileRequest(string url, string download_file_path, NIMHttp.CompletedCb complete_cb, IntPtr user_data)
        {
            return nim_http_create_download_file_request(url, download_file_path, complete_cb, user_data);
        }
        
        /// <summary>
        /// 创建下载文件任务，支持断点续传
        /// </summary>
        /// <param name="url">资源地址</param>
        /// <param name="download_file_path">下载文件保存的本地路径</param>
        /// <param name="complete_cb">下载文件的起始点</param>
        /// <param name="complete_cb">结束回调</param>
        /// <param name="user_data">自定义数据</param>
        /// <returns>http任务句柄</returns>
        public static IntPtr CreateDownloadFileRangeRequest(string url, string download_file_path, Int64 range_start, NIMHttp.CompletedCb complete_cb, IntPtr user_data)
        {
            return nim_http_create_download_file_range_request(url, download_file_path, range_start, complete_cb, user_data);
        }

        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="url">资源地址</param>
        /// <param name="post_body">上传内容</param>
        /// <param name="post_body_size">上传内容大小</param>
        /// <param name="response_cb">结束回调，响应实体内容</param>
        /// <param name="user_data">自定义数据</param>
        /// <returns>http任务句柄</returns>
        public static IntPtr CreateRequest(string url, IntPtr post_body, int post_body_size, NIMHttp.ResponseCb response_cb, IntPtr user_data)
        {
            return nim_http_create_request(url, post_body, post_body_size, response_cb, user_data);
        }

        /// <summary>
        /// 天假header
        /// </summary>
        /// <param name="request_handle">http任务句柄</param>
        /// <param name="key">头的key</param>
        /// <param name="value">头的value</param>
        public static void AddHeader(IntPtr request_handle, string key, string value)
        {
            nim_http_add_request_header(request_handle, key, value);
        }

        /// <summary>
        /// 设置进度回调
        /// </summary>
        /// <param name="request_handle">http任务句柄</param>
        /// <param name="progress_cb">进度回调函数</param>
        /// <param name="user_data">自定义数据</param>
        public static void SetProgressCb(IntPtr request_handle, NIMHttp.ProgressCb progress_cb, IntPtr user_data)
        {
            nim_http_set_request_progress_cb(request_handle, progress_cb, user_data);
        }
        
        /// <summary>
        /// 强制设置http请求方法为post
        /// </summary>
        /// <param name="request_handle">http任务句柄</param>
        public static void SetRequestAsPost(IntPtr request_handle)
        {
            nim_http_set_request_method_as_post(request_handle);
        }
        
        /// <summary>
        /// 设置超时
        /// </summary>
        /// <param name="request_handle">http任务句柄</param>
        /// <param name="timeout_ms">超时时间，单位是毫秒</param>
        public static void SetTimeout(IntPtr request_handle, int timeout_ms)
        {
            nim_http_set_timeout(request_handle, timeout_ms);
        }
    }
}
