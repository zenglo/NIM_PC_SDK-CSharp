/** @file NIMRtsAPI.cs
  * @brief NIM RTS提供的实时会话（数据通道）相关接口，如果需要用到音视频功能请使用NIMDeviceAPI.cs中相关接口，并调用NIM.VChatAPI.Init初始化
  * @copyright (c) 2015, NetEase Inc. All rights reserved
  * @author gq
  * @date 2015/12/8
  */

using System;
using System.Runtime.InteropServices;

namespace NIM
{
    public class RtsAPI
    {
        public struct RtsNotifyHandlerInfo
        {
            public NIMRts.onStartNotify         onStartNotify;
            public NIMRts.onAckNotify           onAckNotify;
            public NIMRts.onSyncAckNotify       onSyncAckNotify;
            public NIMRts.onConnectNotify       onConnectNotify;
            public NIMRts.onMemberNotify        onMemberNotify;
            public NIMRts.onControlNotify       onControlNotify;
            public NIMRts.onHangupNotify        onHangupNotify;
            public NIMRts.onRecData             onRecData;
        }

        //引用C中的方法（考虑到不同平台下的C接口引用方式差异，如[DllImport("__Internal")]，[DllImport("nimapi")]等） 
        #region NIM C SDK native methods

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_rts_start", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_rts_start(int channel_type, string uid, 
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension,
            NIMRts.nim_rts_start_cb_func cb, IntPtr user_data);
        
        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_rts_set_start_notify_cb_func", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_rts_set_start_notify_cb_func(NIMRts.nim_rts_start_notify_cb_func cb, IntPtr user_data);
        
        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_rts_ack", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_rts_ack(string session_id, int channel_type, bool accept, 
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension,
            NIMRts.nim_rts_ack_res_cb_func cb, IntPtr user_data);
        
        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_rts_set_ack_notify_cb_func", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_rts_set_ack_notify_cb_func(NIMRts.nim_rts_ack_notify_cb_func cb, IntPtr user_data);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_rts_set_sync_ack_notify_cb_func", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_rts_set_sync_ack_notify_cb_func(NIMRts.nim_rts_sync_ack_notify_cb_func cb, IntPtr user_data);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_rts_set_connect_notify_cb_func", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_rts_set_connect_notify_cb_func(NIMRts.nim_rts_connect_notify_cb_func cb, IntPtr user_data);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_rts_set_member_change_cb_func", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_rts_set_member_change_cb_func(NIMRts.nim_rts_member_change_cb_func cb, IntPtr user_data);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_rts_control", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_rts_control(string session_id, string info,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension,
            NIMRts.nim_rts_control_res_cb_func cb, IntPtr user_data);
        
        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_rts_set_control_notify_cb_func", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_rts_set_control_notify_cb_func(NIMRts.nim_rts_control_notify_cb_func cb, IntPtr user_data);
        
        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_rts_set_vchat_mode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_rts_set_vchat_mode(string session_id, int mode,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension);
        
        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_rts_hangup", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_rts_hangup(string session_id,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension,
            NIMRts.nim_rts_hangup_res_cb_func cb, IntPtr user_data);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_rts_set_hangup_notify_cb_func", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_rts_set_hangup_notify_cb_func(NIMRts.nim_rts_hangup_notify_cb_func cb, IntPtr user_data);
        
        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_rts_send_data", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_rts_send_data(string session_id, int channel_type, IntPtr data, int size,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NimUtility.Utf8StringMarshaler))]string json_extension);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_rts_set_rec_data_cb_func", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_rts_set_rec_data_cb_func(NIMRts.nim_rts_rec_data_cb_func cb, IntPtr user_data);

        #endregion
        
        /// <summary>
        /// 设置rts的通知回调
        /// </summary>
        /// <param name="info">通知回调的结构</param>
        public static void SetNotifyHandler(RtsNotifyHandlerInfo info)
        {
            _RtsNotifyHandlerInfo = info;
            //注册收到邀请通知
            var ptr1 = NimUtility.DelegateConverter.ConvertToIntPtr(_RtsNotifyHandlerInfo.onStartNotify);
            nim_rts_set_start_notify_cb_func(_StartNotifyCb, ptr1);
            var ptr2 = NimUtility.DelegateConverter.ConvertToIntPtr(_RtsNotifyHandlerInfo.onAckNotify);
            nim_rts_set_ack_notify_cb_func(_AckNotifyCb, ptr2);
            var ptr3 = NimUtility.DelegateConverter.ConvertToIntPtr(_RtsNotifyHandlerInfo.onSyncAckNotify);
            nim_rts_set_sync_ack_notify_cb_func(_SyncAckNotifyCb, ptr3);
            var ptr4 = NimUtility.DelegateConverter.ConvertToIntPtr(_RtsNotifyHandlerInfo.onConnectNotify);
            nim_rts_set_connect_notify_cb_func(_ConnectNotifyCb, ptr4);
            var ptr5 = NimUtility.DelegateConverter.ConvertToIntPtr(_RtsNotifyHandlerInfo.onMemberNotify);
            nim_rts_set_member_change_cb_func(_MemberChangeCb, ptr5);
            var ptr6 = NimUtility.DelegateConverter.ConvertToIntPtr(_RtsNotifyHandlerInfo.onControlNotify);
            nim_rts_set_control_notify_cb_func(_ControlNotifyCb, ptr6);
            var ptr7 = NimUtility.DelegateConverter.ConvertToIntPtr(_RtsNotifyHandlerInfo.onHangupNotify);
            nim_rts_set_hangup_notify_cb_func(_HangupNotifyCb, ptr7);
            var ptr8 = NimUtility.DelegateConverter.ConvertToIntPtr(_RtsNotifyHandlerInfo.onRecData);
            nim_rts_set_rec_data_cb_func(_RecDataCb, ptr8);
        }
        private static RtsNotifyHandlerInfo _RtsNotifyHandlerInfo;
        private static NIMRts.nim_rts_start_notify_cb_func _StartNotifyCb = new NIMRts.nim_rts_start_notify_cb_func(StartNotifyCallback);
        private static NIMRts.nim_rts_ack_notify_cb_func _AckNotifyCb = new NIMRts.nim_rts_ack_notify_cb_func(AckNotifyCallback);
        private static NIMRts.nim_rts_sync_ack_notify_cb_func _SyncAckNotifyCb = new NIMRts.nim_rts_sync_ack_notify_cb_func(SyncAckNotifyCallback);
        private static NIMRts.nim_rts_connect_notify_cb_func _ConnectNotifyCb = new NIMRts.nim_rts_connect_notify_cb_func(ConnectNotifyCallback);
        private static NIMRts.nim_rts_member_change_cb_func _MemberChangeCb = new NIMRts.nim_rts_member_change_cb_func(MemberChangeCallback);
        private static NIMRts.nim_rts_control_notify_cb_func _ControlNotifyCb = new NIMRts.nim_rts_control_notify_cb_func(ControlNotifyCallback);
        private static NIMRts.nim_rts_hangup_notify_cb_func _HangupNotifyCb = new NIMRts.nim_rts_hangup_notify_cb_func(HangupNotifyCallback);
        private static NIMRts.nim_rts_rec_data_cb_func _RecDataCb = new NIMRts.nim_rts_rec_data_cb_func(RecDataCallback);
        private static void StartNotifyCallback(string session_id, int channel_type, string uid, string json_extension, IntPtr user_data)
        {
            NimUtility.DelegateConverter.Invoke<NIMRts.onStartNotify>(user_data, session_id, channel_type, uid);
        }
        private static void AckNotifyCallback(string session_id, int channel_type, bool accept, string uid, string json_extension, IntPtr user_data)
        {
            NimUtility.DelegateConverter.Invoke<NIMRts.onAckNotify>(user_data, session_id, channel_type, accept, uid);
        }
        private static void SyncAckNotifyCallback(string session_id, int channel_type, bool accept, string json_extension, IntPtr user_data)
        {
            NIMRts.RtsSyncAckInfo info = NIMRts.RtsSyncAckInfo.Deserialize(json_extension);
            NimUtility.DelegateConverter.Invoke<NIMRts.onSyncAckNotify>(user_data, session_id, channel_type, accept, info.client);
        }
        private static void ConnectNotifyCallback(string session_id, int channel_type, int code, string json_extension, IntPtr user_data)
        {
            //NIMRts.RtsConnectInfo info = NIMRts.RtsConnectInfo.Deserialize(json_extension);
            NimUtility.DelegateConverter.Invoke<NIMRts.onConnectNotify>(user_data, session_id, channel_type, code);
        }
        private static void MemberChangeCallback(string session_id, int channel_type, int type, string uid, string json_extension, IntPtr user_data)
        {
            NimUtility.DelegateConverter.Invoke<NIMRts.onMemberNotify>(user_data, session_id, channel_type, type, uid);
        }
        private static void ControlNotifyCallback(string session_id, string info, string uid, string json_extension, IntPtr user_data)
        {
            NimUtility.DelegateConverter.Invoke<NIMRts.onControlNotify>(user_data, session_id, info, uid);
        }
        private static void HangupNotifyCallback(string session_id, string uid, string json_extension, IntPtr user_data)
        {
            NimUtility.DelegateConverter.Invoke<NIMRts.onHangupNotify>(user_data, session_id, uid);
        }
        private static void RecDataCallback(string session_id, int type, string uid, IntPtr data, int size, string json_extension, IntPtr user_data)
        {
            NimUtility.DelegateConverter.Invoke<NIMRts.onRecData>(user_data, session_id, type, uid, data, size);
        }
        

        /// <summary>
        /// 创建rts会话
        /// </summary>
        /// <param name="channel_type">通道类型 如要tcp+音视频，则channel_type=kNIMRtsChannelTypeTcp|kNIMRtsChannelTypeVchat，同时整个SDK只允许一个音视频通道存在（包括vchat）</param>
        /// <param name="uid">对方帐号</param>
        /// <param name="info">发起扩展参数</param>
        /// <param name="StartResHandler">结果回调</param>
        public static void Start(int channel_type, string uid, NIMRts.RtsStartInfo info, NIMRts.StartResHandler StartResHandler)
        {
            var ptr = NimUtility.DelegateConverter.ConvertToIntPtr(StartResHandler);
            var json = info.Serialize();
            nim_rts_start(channel_type, uid, json, _StartResCb, ptr);
        }
        private static NIMRts.nim_rts_start_cb_func _StartResCb = new NIMRts.nim_rts_start_cb_func(StartResCallback);
        private static void StartResCallback(int code, string session_id, int channel_type, string uid, string json_extension, IntPtr user_data)
        {
            NimUtility.DelegateConverter.Invoke<NIMRts.StartResHandler>(user_data, code, session_id, channel_type, uid);
        }

        /// <summary>
        /// 回复收到的邀请
        /// </summary>
        /// <param name="session_id">会话id</param>
        /// <param name="channel_type">通道类型,暂时无效</param>
        /// <param name="accept">是否接受</param>
        /// <param name="info">接受时的发起信息扩展参数</param>
        /// <param name="AckResHandler">结果回调</param>
        public static void Ack(string session_id, int channel_type, bool accept, NIMRts.RtsStartInfo info, NIMRts.AckResHandler AckResHandler)
        {
            var ptr = NimUtility.DelegateConverter.ConvertToIntPtr(AckResHandler);
            var json = info.Serialize();
            nim_rts_ack(session_id, channel_type, accept, json, _AckResCb, ptr);
        }
        private static NIMRts.nim_rts_ack_res_cb_func _AckResCb = new NIMRts.nim_rts_ack_res_cb_func(AckResCallback);
        private static void AckResCallback(int code, string session_id, int channel_type, bool accept, string json_extension, IntPtr user_data)
        {
            NimUtility.DelegateConverter.Invoke<NIMRts.AckResHandler>(user_data, code, session_id, channel_type, accept);
        }

        /// <summary>
        /// 会话控制（透传）
        /// </summary>
        /// <param name="session_id">会话id</param>
        /// <param name="info">透传内容</param>
        /// <param name="ControlResHandler">结果回调</param>
        public static void Control(string session_id, string info, NIMRts.ControlResHandler ControlResHandler)
        {
            var ptr = NimUtility.DelegateConverter.ConvertToIntPtr(ControlResHandler);
            nim_rts_control(session_id, info, "", _ControlResCb, ptr);
        }
        private static NIMRts.nim_rts_control_res_cb_func _ControlResCb = new NIMRts.nim_rts_control_res_cb_func(ControlResCallback);
        private static void ControlResCallback(int code, string session_id, string info, string json_extension, IntPtr user_data)
        {
            NimUtility.DelegateConverter.Invoke<NIMRts.ControlResHandler>(user_data, code, session_id, info);
        }
        
        /// <summary>
        /// 修改音视频的模式
        /// </summary>
        /// <param name="session_id">会话id</param>
        /// <param name="mode">音频模式或视频模式</param>
        public static void SetVChatMode(string session_id, NIMRts.NIMRtsVideoChatMode mode)
        {
            nim_rts_set_vchat_mode(session_id, (int)mode, "");
        }
        
        /// <summary>
        /// 结束会话
        /// </summary>
        /// <param name="session_id">会话id</param>
        /// <param name="HangupResHandler">结果回调</param>
        public static void Hangup(string session_id, NIMRts.HangupResHandler HangupResHandler)
        {
            var ptr = NimUtility.DelegateConverter.ConvertToIntPtr(HangupResHandler);
            nim_rts_hangup(session_id, "", _HangupResCb, ptr);
        }
        private static NIMRts.nim_rts_hangup_res_cb_func _HangupResCb = new NIMRts.nim_rts_hangup_res_cb_func(HangupResCallback);
        private static void HangupResCallback(int code, string session_id, string json_extension, IntPtr user_data)
        {
            NimUtility.DelegateConverter.Invoke<NIMRts.HangupResHandler>(user_data, code, session_id);
        }
        
        /// <summary>
        /// 发送数据，暂时支持tcp通道，建议发送频率在20Hz以下
        /// </summary>
        /// <param name="session_id">会话id</param>
        /// <param name="channel_type">通道类型</param>
        /// <param name="data">发送数据</param>
        /// <param name="size">data的数据长度</param>
        public static void SendData(string session_id, int channel_type, IntPtr data, int size)
        {
            nim_rts_send_data(session_id, channel_type, data, size, "");
        }

    }

}
