/** @file NIMVChatAPI.cs
  * @brief NIM VChat提供的音视频相关接口，相关功能调用前需要先Init()
  * @copyright (c) 2015, NetEase Inc. All rights reserved
  * @author gq
  * @date 2015/12/8
  */

using System;
using System.Runtime.InteropServices;

namespace NIM
{
    /// <summary>
    /// 音视频相关的回调
    /// </summary>
    public struct NIMVChatSessionStatus
    {
        /// <summary>
        /// 发起结果回调
        /// </summary>
        public onSessionHandler             onSessionStartRes;
        /// <summary>
        /// 邀请通知
        /// </summary>
        public onSessionInviteNotify        onSessionInviteNotify;
        /// <summary>
        /// 邀请响应的结果回调
        /// </summary>
        public onSessionHandler             onSessionCalleeAckRes;
        /// <summary>
        /// 发起后对方响应通知
        /// </summary>
        public onSessionCalleeAckNotify     onSessionCalleeAckNotify;
        /// <summary>
        /// 控制操作结果回调
        /// </summary>
        public onSessionControlRes          onSessionControlRes;
        /// <summary>
        /// 控制操作通知
        /// </summary>
        public onSessionControlNotify       onSessionControlNotify;
        /// <summary>
        /// 链接通知
        /// </summary>
        public onSessionConnectNotify       onSessionConnectNotify;
        /// <summary>
        /// 成员状态通知
        /// </summary>
        public onSessionPeopleStatus        onSessionPeopleStatus;
        /// <summary>
        /// 网络状态通知
        /// </summary>
        public onSessionNetStatus           onSessionNetStatus;
        /// <summary>
        /// 主动挂断结果回调
        /// </summary>
        public onSessionHandler             onSessionHangupRes;
        /// <summary>
        /// 对方挂断通知
        /// </summary>
        public onSessionHandler             onSessionHangupNotify;
        /// <summary>
        /// 本人其他端响应通知
        /// </summary>
        public onSessionSyncAckNotify       onSessionSyncAckNotify;
    }
    public class VChatAPI
    {
        //引用C中的方法（考虑到不同平台下的C接口引用方式差异，如[DllImport("__Internal")]，[DllImport("nimapi")]等） 
        #region NIM C SDK native methods

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_vchat_init", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool nim_vchat_init(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_vchat_cleanup", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void nim_vchat_cleanup(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_vchat_set_cb_func", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void nim_vchat_set_cb_func(nim_vchat_cb_func cb, IntPtr user_data);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_vchat_start", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool nim_vchat_start(NIMVideoChatMode mode,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string apns_text,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string custom_info,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension, IntPtr user_data);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_vchat_set_talking_mode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool nim_vchat_set_talking_mode(NIMVideoChatMode mode,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_vchat_callee_ack", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool nim_vchat_callee_ack(long channel_id, bool accept,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension, IntPtr user_data);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_vchat_control", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool nim_vchat_control(long channel_id, NIMVChatControlType type,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension, IntPtr user_data);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_vchat_end", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void nim_vchat_end([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension);

        #endregion


        /// <summary>
        /// VCHAT初始化，需要在SDK的Client.Init成功之后
        /// </summary>
        /// <returns></returns>
        public static bool Init()
        {
            ClientAPI.GuaranteeInitialized();
            return nim_vchat_init("");
        }

        /// <summary>
        /// VCHAT初始化，需要在SDK的Client.Cleanup成功之后
        /// </summary>
        /// <returns></returns>
        public static void Cleanup()
        {
            nim_vchat_cleanup("");
        }


        static nim_vchat_cb_func VChatStatusCb = new nim_vchat_cb_func(VChatSessionStatusCallback);
        /// <summary>
        /// 设置统一的通话回调或者服务器通知
        /// </summary>
        /// <param name="session">回调通知对象</param>
        public static void SetSessionStatusCb(NIMVChatSessionStatus session)
        {
            session_status = session;
            nim_vchat_set_cb_func(VChatStatusCb, IntPtr.Zero);
        }
        private static NIMVChatSessionStatus session_status;
        private static void VChatSessionStatusCallback(NIMVideoChatSessionType type, long channel_id, int code, string json_extension, IntPtr user_data)
        {
            if(json_extension == null)
            {
                return;
            }
            NIMVChatSessionInfo info = NIMVChatSessionInfo.Deserialize(json_extension);
            switch (type)
            {
                case NIMVideoChatSessionType.kNIMVideoChatSessionTypeStartRes:
                    {
                        if (session_status.onSessionStartRes != null)
                        {
                            session_status.onSessionStartRes.DynamicInvoke(channel_id, code);
                        }
                    } break;
                case NIMVideoChatSessionType.kNIMVideoChatSessionTypeInviteNotify:
                    {
                        if (session_status.onSessionInviteNotify != null)
                        {
                            session_status.onSessionInviteNotify.DynamicInvoke(channel_id, info.Uid, info.Type, info.Time);
                        }
                    } break;
                case NIMVideoChatSessionType.kNIMVideoChatSessionTypeCalleeAckRes:
                    {
                        if (session_status.onSessionCalleeAckRes != null)
                        {
                            session_status.onSessionCalleeAckRes.DynamicInvoke(channel_id, code);
                        }
                    } break;
                case NIMVideoChatSessionType.kNIMVideoChatSessionTypeCalleeAckNotify:
                    {
                        if (session_status.onSessionCalleeAckNotify != null)
                        {
                            session_status.onSessionCalleeAckNotify.DynamicInvoke(channel_id, info.Uid, info.Type, info.Accept > 0);
                        }
                    } break;
                case NIMVideoChatSessionType.kNIMVideoChatSessionTypeControlRes:
                    {
                        if (session_status.onSessionControlRes != null)
                        {
                            session_status.onSessionControlRes.DynamicInvoke(channel_id, code, info.Type);
                        }
                    } break;
                case NIMVideoChatSessionType.kNIMVideoChatSessionTypeControlNotify:
                    {
                        if (session_status.onSessionControlNotify != null)
                        {
                            session_status.onSessionControlNotify.DynamicInvoke(channel_id, info.Uid, info.Type);
                        }
                    } break;
                case NIMVideoChatSessionType.kNIMVideoChatSessionTypeConnect:
                    {
                        if (session_status.onSessionConnectNotify != null)
                        {
                            if (info != null)
                                session_status.onSessionConnectNotify.DynamicInvoke(channel_id, code, info.RecordAddr, info.RecordFile);
                            else
                                session_status.onSessionConnectNotify.DynamicInvoke(channel_id, code, null, null);
                        }
                    } break;
                case NIMVideoChatSessionType.kNIMVideoChatSessionTypePeopleStatus:
                    {
                        if (session_status.onSessionPeopleStatus != null)
                        {
                            session_status.onSessionPeopleStatus.DynamicInvoke(channel_id, info.Uid, code);
                        }
                    } break;
                case NIMVideoChatSessionType.kNIMVideoChatSessionTypeNetStatus:
                    {
                        if (session_status.onSessionNetStatus != null)
                        {
                            session_status.onSessionNetStatus.DynamicInvoke(channel_id, code);
                        }
                    } break;
                case NIMVideoChatSessionType.kNIMVideoChatSessionTypeHangupRes:
                    {
                        if (session_status.onSessionHangupRes != null)
                        {
                            session_status.onSessionHangupRes.DynamicInvoke(channel_id, code);
                        }
                    } break;
                case NIMVideoChatSessionType.kNIMVideoChatSessionTypeHangupNotify:
                    {
                        if (session_status.onSessionHangupNotify != null)
                        {
                            session_status.onSessionHangupNotify.DynamicInvoke(channel_id, code);
                        }
                    } break;
                case NIMVideoChatSessionType.kNIMVideoChatSessionTypeSyncAckNotify:
                    {
                        if (session_status.onSessionSyncAckNotify != null)
                        {
                            session_status.onSessionSyncAckNotify.DynamicInvoke(channel_id, code, info.Uid, info.Type, info.Accept > 0, info.Time, info.Client);
                        }
                    } break;
            }
        }

        /// <summary>
        /// 启动通话
        /// </summary>
        /// <param name="mode">启动音视频通话类型</param>
        /// <param name="json_extension">扩展，kNIMVChatUids成员id列表(必填),其他可选 如{"uids":["uid_temp"],"custom_video":0, "custom_audio":0}</param>
        /// <returns></returns>
        public static bool Start(NIMVideoChatMode mode, NIMVChatInfo info)
        {
            string json_extension = info.Serialize();
            return nim_vchat_start(mode,null,null, json_extension, IntPtr.Zero);
        }

        /// <summary>
        /// 设置通话模式，在更改通话模式后，通知底层
        /// </summary>
        /// <param name="mode">音视频通话类型</param>
        /// <returns></returns>
        public static bool SetMode(NIMVideoChatMode mode)
        {
            return nim_vchat_set_talking_mode(mode, "");
        }

        /// <summary>
        /// 回应音视频通话邀请
        /// </summary>
        /// <param name="channel_id">音视频通话通道id</param>
        /// <param name="accept">true 接受，false 拒绝</param>
        /// <param name="json_extension">接起时有效 参数可选 如{"custom_video":0, "custom_audio":0}</param>
        /// <returns></returns>
        public static bool CalleeAck(long channel_id, bool accept, NIMVChatInfo info)
        {
            string json_extension = info.Serialize();
            return nim_vchat_callee_ack(channel_id, accept, json_extension, IntPtr.Zero);
        }

        /// <summary>
        /// 音视频通话控制操作
        /// </summary>
        /// <param name="channel_id">音视频通话通道id</param>
        /// <param name="type">操作类型</param>
        /// <returns></returns>
        public static bool ChatControl(long channel_id, NIMVChatControlType type)
        {
            return nim_vchat_control(channel_id, type, "", IntPtr.Zero);
        }

        /// <summary>
        /// 结束通话(需要主动在通话结束后调用，用于底层挂断和清理数据)
        /// </summary>
        public static void End()
        {
            nim_vchat_end("");
        }

    }
}
