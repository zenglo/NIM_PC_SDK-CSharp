/** @file NIMAudioAPI.cs
  * @brief NIM 提供的语音录制和播放接口
  * @copyright (c) 2015, NetEase Inc. All rights reserved
  * @author Harrison
  * @date 2015/12/8
  */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using NIM;

namespace NIMAudio
{
    public class AudioAPI
    {
        //引用C中的方法
        #region NIM Audio C SDK native methods

        [DllImport(NIMAudio.NIMAudioNativeDLL, EntryPoint = "nim_audio_init_module", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool nim_audio_init_module([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string user_data_parent_path);

        [DllImport(NIMAudio.NIMAudioNativeDLL, EntryPoint = "nim_audio_uninit_module", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool nim_audio_uninit_module();

        [DllImport(NIMAudio.NIMAudioNativeDLL, EntryPoint = "nim_audio_play_audio", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool nim_audio_play_audio([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string file_path,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string call_id,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string res_id);

        [DllImport(NIMAudio.NIMAudioNativeDLL, EntryPoint = "nim_audio_stop_play_audio", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool nim_audio_stop_play_audio();

        [DllImport(NIMAudio.NIMAudioNativeDLL, EntryPoint = "nim_audio_reg_start_play_cb", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool nim_audio_reg_start_play_cb(NIMAudio.ResCodeIdCb cb);

        [DllImport(NIMAudio.NIMAudioNativeDLL, EntryPoint = "nim_audio_reg_stop_play_cb", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool nim_audio_reg_stop_play_cb(NIMAudio.ResCodeIdCb cb);

        #endregion

        /// <summary>
        /// NIM SDK 初始化语音模块
        /// </summary>
        /// <param name="userDataParentPath">用户目录</param>
        /// <returns><c>true</c> 调用成功, <c>false</c> 调用失败</returns>
        public static bool InitModule(string userDataParentPath)
        {
            return nim_audio_init_module(userDataParentPath);
        }

        /// <summary>
        /// NIM SDK 卸载语音模块（只有在主程序关闭时才有必要调用此接口）
        /// </summary>
        /// <returns><c>true</c> 调用成功, <c>false</c> 调用失败</returns>
        public static bool UninitModule()
        {
            return nim_audio_uninit_module();
        }

        /// <summary>
        /// NIM SDK 播放,通过回调获取开始播放状态
        /// </summary>
        /// <param name="filePath">播放文件绝对路径</param>
        /// <param name="callId">用以定位资源的一级ID，可选</param>
        /// <param name="resId">用以定位资源的二级ID，可选</param>
        /// <returns><c>true</c> 调用成功, <c>false</c> 调用失败</returns>
        public static bool PlayAudio(string filePath, string callId, string resId)
        {
            return nim_audio_play_audio(filePath, callId, resId);
        }

        /// <summary>
        /// NIM SDK 停止播放,通过回调获取停止播放状态
        /// </summary>
        /// <returns><c>true</c> 调用成功, <c>false</c> 调用失败</returns>
        public static bool StopPlayAudio()
        {
            return nim_audio_stop_play_audio();
        }

        /// <summary>
        /// NIM SDK 注册播放开始事件回调
        /// </summary>
        /// <param name="cb">播放开始事件的回调函数</param>
        /// <returns><c>true</c> 调用成功, <c>false</c> 调用失败</returns>
        public static bool RegStartPlayCb(NIMAudio.ResCodeIdCb cb)
        {
            return nim_audio_reg_start_play_cb(cb);

        }

        /// <summary>
        /// NIM SDK 注册播放结束事件回调
        /// </summary>
        /// <param name="cb">播放结束事件的回调函数</param>
        /// <returns><c>true</c> 调用成功, <c>false</c> 调用失败</returns>
        public static bool RegStopPlayCb(NIMAudio.ResCodeIdCb cb)
        {
            return nim_audio_reg_stop_play_cb(cb);
        }
    }
}
