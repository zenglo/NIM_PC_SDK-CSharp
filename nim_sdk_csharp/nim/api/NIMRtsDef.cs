/** @file NIMRtsDef.cs
  * @brief NIM RTS提供的实时会话（数据通道）接口定义
  * @copyright (c) 2015, NetEase Inc. All rights reserved
  * @author gq
  * @date 2015/12/8
  */

using System;
using System.Runtime.InteropServices;

namespace NIM
{
    public class NIMRts
    {
        /// <summary>
        /// rts通道类型
        /// </summary>
        public enum NIMRtsChannelType
        {
            /// <summary>
            /// 无通道
            /// </summary>
	        kNIMRtsChannelTypeNone	= 0,
            /// <summary>
            /// tcp通道
            /// </summary>
	        kNIMRtsChannelTypeTcp	= 1,
            /// <summary>
            /// udp通道 暂不支持
            /// </summary>
	        kNIMRtsChannelTypeUdp	= 2,
            /// <summary>
            /// 音视频通道
            /// </summary>
	        kNIMRtsChannelTypeVchat	= 4,
        };

        /// <summary>
        /// 成员变化类型
        /// </summary>
        public enum NIMRtsMemberStatus
        {
            /// <summary>
            /// 成员进入
            /// </summary>
	        kNIMRtsMemberStatusJoined           = 0,
            /// <summary>
            /// 成员退出
            /// </summary>
	        kNIMRtsMemberStatusLeaved           = 1,
        };

        /// <summary>
        /// 音视频通话类型
        /// </summary>
        public enum NIMRtsVideoChatMode
        {
            /// <summary>
            /// 语音通话模式
            /// </summary>
	        kNIMRtsVideoChatModeAudio	=	1,
            /// <summary>
            /// 视频通话模式
            /// </summary>
	        kNIMRtsVideoChatModeVideo	=	2
        };

        
        /// <summary>
        /// 音视频服务器连接状态类型
        /// </summary>
        public enum NIMRtsConnectStatus
        {
            /// <summary>
            /// 断开连接
            /// </summary>
	        kNIMRtsConnectStatusDisconn			= 0,
            /// <summary>
            /// 启动失败
            /// </summary>
	        kNIMRtsConnectStatusStartFail		= 1,
            /// <summary>
            /// 超时
            /// </summary>
	        kNIMRtsConnectStatusTimeout			= 101,
            /// <summary>
            /// 成功
            /// </summary>
	        kNIMRtsConnectStatusSuccess			= 200,
            /// <summary>
            /// 错误参数
            /// </summary>
	        kNIMRtsConnectStatusInvalidParam	= 400,
            /// <summary>
            /// 密码加密错误
            /// </summary>
	        kNIMRtsConnectStatusDesKey			= 401,
            /// <summary>
            /// 错误请求
            /// </summary>
	        kNIMRtsConnectStatusInvalidRequst	= 417,
            /// <summary>
            /// 服务器内部错误
            /// </summary>
	        kNIMRtsConnectStatusServerUnknown	= 500,
            /// <summary>
            /// 退出
            /// </summary>
	        kNIMRtsConnectStatusLogout			= 1001,
        };

        /// <summary>
        /// 发起rts或者接起rts时的配置参数
        /// </summary>
        public class RtsStartInfo : NimUtility.NimJsonObject<RtsStartInfo>
        {
            /// <summary>
            /// 视频通道的发起模式NIMRtsVideoChatMode，非视频模式时不会发送视频数据
            /// </summary>
            [Newtonsoft.Json.JsonProperty("mode")]
            public int Mode { get; set; }

            /// <summary>
            /// 是否用自定义音频数据（PCM）
            /// </summary>
            [Newtonsoft.Json.JsonProperty("custom_audio")]
            public int CustomAudio { get; set; }

            /// <summary>
            /// 是否用自定义视频数据（i420）
            /// </summary>
            [Newtonsoft.Json.JsonProperty("custom_video")]
            public int CustomVideo { get; set; }
            
            /// <summary>
            /// 推送用的文本，接收方无效
            /// </summary>
            [Newtonsoft.Json.JsonProperty("apns")]
            public string ApnsText { get; set; }

            /// <summary>
            /// 自定义数据，透传给被邀请方，接收方无效
            /// </summary>
            [Newtonsoft.Json.JsonProperty("custom_info")]
            public string CustomInfo { get; set; }

            public RtsStartInfo()
            {
                Mode = (int)NIMRtsVideoChatMode.kNIMRtsVideoChatModeAudio;
                CustomAudio = 0;
                CustomVideo = 0;
            }
        }
        /// <summary>
        /// 收到本人其他端已经处理的通知
        /// </summary>
        public class RtsSyncAckInfo : NimUtility.NimJsonObject<RtsSyncAckInfo>
        {
            /// <summary>
            /// 客户端类型
            /// </summary>
            [Newtonsoft.Json.JsonProperty("client_type")]
            public int client { get; set; }
        }
        /// <summary>
        /// 通道连接成功后会返回服务器录制信息
        /// </summary>
        public class RtsConnectInfo : NimUtility.NimJsonObject<RtsConnectInfo>
        {
            /// <summary>
            /// 录制地址（服务器开启录制时有效）
            /// </summary>
            [Newtonsoft.Json.JsonProperty("record_addr")]
            public string RecordAddr { get; set; }
            /// <summary>
            /// 录制文件名（服务器开启录制时有效）
            /// </summary>
            [Newtonsoft.Json.JsonProperty("record_file")]
            public string RecordFile { get; set; }

            RtsConnectInfo()
            {
                RecordAddr = null;
                RecordFile = null;
            }
        }

        /// <summary>
        /// 创建通道返回结果
        /// </summary>
        /// <param name="code">调用结果</param>
        /// <param name="session_id">会话id</param>
        /// <param name="channel_type">通道类型 如要tcp+音视频，则channel_type=kNIMRtsChannelTypeTcp|kNIMRtsChannelTypeVchat</param>
        /// <param name="uid">对方帐号</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void StartResHandler(int code, string session_id, int channel_type, string uid);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void nim_rts_start_cb_func(int code, string session_id, int channel_type, string uid,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension,
            IntPtr user_data);
        
        /// <summary>
        /// 收到对方创建通道通知
        /// </summary>
        /// <param name="session_id">会话id</param>
        /// <param name="channel_type">通道类型 如要tcp+音视频，则channel_type=kNIMRtsChannelTypeTcp|kNIMRtsChannelTypeVchat</param>
        /// <param name="uid">对方帐号</param>
        /// <param name="custom_info">透传数据</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void onStartNotify(string session_id, int channel_type, string uid,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string custom_info);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void nim_rts_start_notify_cb_func(string session_id, int channel_type, string uid,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension,
            IntPtr user_data);
        
        /// <summary>
        /// 回复收到邀请的结果
        /// </summary>
        /// <param name="code">调用结果</param>
        /// <param name="session_id">会话id</param>
        /// <param name="channel_type">通道类型</param>
        /// <param name="accept">是否接受</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void AckResHandler(int code, string session_id, int channel_type, bool accept);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void nim_rts_ack_res_cb_func(int code, string session_id, int channel_type, bool accept,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension,
            IntPtr user_data);
        
        /// <summary>
        /// 收到对方回复邀请的通知
        /// </summary>
        /// <param name="session_id">会话id</param>
        /// <param name="channel_type">通道类型</param>
        /// <param name="accept">是否接受</param>
        /// <param name="uid">对方帐号</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void onAckNotify(string session_id, int channel_type, bool accept, string uid);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void nim_rts_ack_notify_cb_func(string session_id, int channel_type, bool accept, string uid,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension,
            IntPtr user_data);
        
        /// <summary>
        /// 收到本人在其他端回复邀请的同步通知
        /// </summary>
        /// <param name="session_id">会话id</param>
        /// <param name="channel_type">通道类型</param>
        /// <param name="accept">是否接受</param>
        /// <param name="client">客户端类型NIMClientType</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void onSyncAckNotify(string session_id, int channel_type, bool accept, int client);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void nim_rts_sync_ack_notify_cb_func(string session_id, int channel_type, bool accept,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension,
            IntPtr user_data);

        /// <summary>
        /// 通道连接状态通知
        /// </summary>
        /// <param name="session_id">会话id</param>
        /// <param name="channel_type">通道类型</param>
        /// <param name="code">连接状态 非200都是未连接</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void onConnectNotify(string session_id, int channel_type, int code);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void nim_rts_connect_notify_cb_func(string session_id, int channel_type, int code,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension,
            IntPtr user_data);

        /// <summary>
        /// 通道连接成员变化通知
        /// </summary>
        /// <param name="session_id">会话id</param>
        /// <param name="channel_type">通道类型</param>
        /// <param name="type">成员变化类型NIMRtsMemberStatus</param>
        /// <param name="uid">对方帐号</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void onMemberNotify(string session_id, int channel_type, int type, string uid);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void nim_rts_member_change_cb_func(string session_id, int channel_type, int type, string uid,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension,
            IntPtr user_data);

        /// <summary>
        /// 控制接口调用结果
        /// </summary>
        /// <param name="code">调用结果</param>
        /// <param name="session_id">会话id</param>
        /// <param name="info">透传数据</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ControlResHandler(int code, string session_id, string info);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void nim_rts_control_res_cb_func(int code, string session_id, string info,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension,
            IntPtr user_data);

        /// <summary>
        /// 控制消息通知回调
        /// </summary>
        /// <param name="session_id">会话id</param>
        /// <param name="info">透传数据</param>
        /// <param name="uid">对方帐号</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void onControlNotify(string session_id, string info, string uid);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void nim_rts_control_notify_cb_func(string session_id, string info, string uid,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension,
            IntPtr user_data);

        /// <summary>
        /// 挂断会话调用结果
        /// </summary>
        /// <param name="code">调用结果</param>
        /// <param name="session_id">会话id</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void HangupResHandler(int code, string session_id);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void nim_rts_hangup_res_cb_func(int code, string session_id,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension,
            IntPtr user_data);

        /// <summary>
        /// 挂断会话通知回调
        /// </summary>
        /// <param name="session_id">会话id</param>
        /// <param name="uid">对方帐号</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void onHangupNotify(string session_id, string uid);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void nim_rts_hangup_notify_cb_func(string session_id, string uid,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension,
            IntPtr user_data);

        /// <summary>
        /// 数据监听回调
        /// </summary>
        /// <param name="session_id">会话id</param>
        /// <param name="channel_type">通道类型</param>
        /// <param name="uid">对方帐号</param>
        /// <param name="data">接受的数据</param>
        /// <param name="size">data的数据长度</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void onRecData(string session_id, int channel_type, string uid, IntPtr data, int size);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void nim_rts_rec_data_cb_func(string session_id, int channel_type, string uid, IntPtr data, int size,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension,
            IntPtr user_data);


    }
}
