/** @file NIMNosAPI.cs
  * @brief NIM SDK提供的NOS云存储服务接口 
  * @copyright (c) 2015, NetEase Inc. All rights reserved
  * @author gq
  * @date 2015/12/8
  */

using System;
using NimUtility;
#if UNITY
using UnityEngine;
using MonoPInvokeCallbackAttribute = AOT.MonoPInvokeCallbackAttribute;
#endif

namespace NIM.Nos
{
    /// <summary>
    ///     下载结果回调
    /// </summary>
    /// <param name="rescode">下载结果，一切正常200</param>
    /// <param name="filePath">下载资源文件本地绝对路径</param>
    /// <param name="callId">如果下载的是消息中的资源，则为消息所属的会话id，否则为空</param>
    /// <param name="resId">如果下载的是消息中的资源，则为消息id，否则为空</param>
    public delegate void DownloadResultHandler(int rescode, string filePath, string callId, string resId);

    /// <summary>
    ///     上传结果回调
    /// </summary>
    /// <param name="rescode">上传结果，一切正常200</param>
    /// <param name="url">url地址</param>
    public delegate void UploadResultHandler(int rescode, string url);

    /// <summary>
    /// 上传/下载进度回调数据
    /// </summary>
    public class ProgressData
    {
        /// <summary>
        /// 目标url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 消息体
        /// </summary>
        public NIMIMMessage Message { get; set; }

        /// <summary>
        /// 上传文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 用户定义数据
        /// </summary>
        public object UserData { get; set; }

        /// <summary>
        /// 当前字节数
        /// </summary>
        public long CurrentSize { get; set; }

        /// <summary>
        /// 数据总字节数
        /// </summary>
        public long TotalSize { get; set; }
    }

    /// <summary>
    ///     传输进度回调
    /// </summary>
    /// <param name="ProgressData">回调数据</param>
    public delegate void ProgressResultHandler(ProgressData prgData);

    class ProgressPair
    {
        public ProgressData Data { get; set; }
        public ProgressResultHandler Action { get; set; }

        public ProgressPair(ProgressData data, ProgressResultHandler action)
        {
            Data = data;
            Action = action;
        }
    }

    public class NosAPI
    {
        private static readonly DownloadCb DownloadCb = DownloadCallback;

        private static readonly DownloadPrgCb DownloadPrgCb = DownloadProgressCallback;

        private static readonly UploadCb UploadCb = UploadCallback;

        private static readonly UploadPrgCb UploadPrgCb = UploadProgressCallback;

        /// <summary>
        ///     注册下载回调，通过注册回调获得http下载结果通知，刷新资源
        /// </summary>
        /// <param name="handler">下载的结果回调</param>
        public static void RegDownloadCb(DownloadResultHandler handler)
        {
            var ptr = DelegateConverter.ConvertToIntPtr(handler);
            NosNativeMethods.nim_nos_reg_download_cb(DownloadCb, ptr);
        }

        /// <summary>
        ///     获取资源
        /// </summary>
        /// <param name="msg">消息体,NIMVedioMessage NIMAudioMessage NIMFileMessage等带msg_attach属性的有下载信息的消息</param>
        /// <param name="resHandler">下载的结果回调</param>
        /// <param name="prgHandler">下载进度的回调</param>
        public static void DownloadMedia(NIMIMMessage msg, DownloadResultHandler resHandler, ProgressResultHandler prgHandler,object userData = null)
        {
            ProgressData data = new ProgressData();
            data.Message = msg;
            data.UserData = userData;
            ProgressPair pair = new ProgressPair(data, prgHandler);
            var ptr1 = DelegateConverter.ConvertToIntPtr(resHandler);
            var ptr2 = DelegateConverter.ConvertToIntPtr(pair);
            var msgJson = msg.Serialize();
            NosNativeMethods.nim_nos_download_media(msgJson, DownloadCb, ptr1, DownloadPrgCb, ptr2);
        }

        /// <summary>
        ///     停止获取资源（目前仅对文件消息类型有效）
        /// </summary>
        /// <param name="msg">消息体</param>
        [Obsolete]
        public static void StopDownloadMedia(NIMIMMessage msg)
        {
            var msgJson = msg.Serialize();
            NosNativeMethods.nim_nos_stop_download_media(msgJson);
        }

        /// <summary>
        ///     上传资源
        /// </summary>
        /// <param name="localFile">本地文件的完整路径</param>
        /// <param name="resHandler">上传的结果回调</param>
        /// <param name="prgHandler">上传进度的回调</param>
        public static void Upload(string localFile, UploadResultHandler resHandler, ProgressResultHandler prgHandler,object userData = null)
        {
            ProgressData data = new ProgressData();
            data.FilePath = localFile;
            data.UserData = userData;
            ProgressPair pair = new ProgressPair(data, prgHandler);
            var ptr1 = DelegateConverter.ConvertToIntPtr(resHandler);
            var ptr2 = DelegateConverter.ConvertToIntPtr(pair);
            NosNativeMethods.nim_nos_upload(localFile, UploadCb, ptr1, UploadPrgCb, ptr2);
        }

        /// <summary>
        ///     下载资源
        /// </summary>
        /// <param name="nosUrl">下载资源的URL</param>
        /// <param name="resHandler">下载的结果回调</param>
        /// <param name="prgHandler">下载进度的回调</param>
        public static void Download(string nosUrl, DownloadResultHandler resHandler, ProgressResultHandler prgHandler,object userData = null)
        {
            ProgressData data = new ProgressData();
            data.Url = nosUrl;
            data.UserData = userData;
            ProgressPair pair = new ProgressPair(data, prgHandler);
            var ptr1 = DelegateConverter.ConvertToIntPtr(resHandler);
            var ptr2 = DelegateConverter.ConvertToIntPtr(pair);

            NosNativeMethods.nim_nos_download(nosUrl, DownloadCb, ptr1, DownloadPrgCb, ptr2);
        }

        [MonoPInvokeCallback(typeof(DownloadCb))]
        private static void DownloadCallback(int rescode, string filePath, string callId, string resId, string jsonExtension, IntPtr userData)
        {
            userData.Invoke<DownloadResultHandler>(rescode, filePath, callId, resId);
        }

        [MonoPInvokeCallback(typeof(DownloadPrgCb))]
        private static void DownloadProgressCallback(long curSize, long fileSize, string jsonExtension, IntPtr userData)
        {
            var pair = DelegateConverter.ConvertFromIntPtr<ProgressPair>(userData);
            if(pair != null)
            {
                pair.Data.CurrentSize = curSize;
                pair.Data.TotalSize = fileSize;
                if(pair.Action != null)
                {
                    pair.Action(pair.Data);
                }
            }
        }

        [MonoPInvokeCallback(typeof(UploadCb))]
        private static void UploadCallback(int rescode, string url, string jsonExtension, IntPtr userData)
        {
            userData.InvokeOnce<UploadResultHandler>(rescode, url);
        }

        [MonoPInvokeCallback(typeof(UploadPrgCb))]
        private static void UploadProgressCallback(long curSize, long fileSize, string jsonExtension, IntPtr userData)
        {
            var pair = DelegateConverter.ConvertFromIntPtr<ProgressPair>(userData);
            if (pair != null)
            {
                pair.Data.CurrentSize = curSize;
                pair.Data.TotalSize = fileSize;
                if (pair.Action != null)
                {
                    pair.Action(pair.Data);
                }
            }
        }
    }
}