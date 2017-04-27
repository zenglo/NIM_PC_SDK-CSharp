﻿/** @file NIMVChatDef.cs
  * @brief NIM VChat提供的音视频接口定义，
  * @copyright (c) 2015, NetEase Inc. All rights reserved
  * @author gq
  * @date 2015/12/8
  */
#if !UNITY
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NIM
{
    /// <summary>
    /// NIMVideoChatSessionType 音视频通话状态通知类型 
    /// </summary>
    public enum NIMVideoChatSessionType
    {
        /// <summary>
        /// 创建通话结果
        /// </summary>
        kNIMVideoChatSessionTypeStartRes = 1,
		/// <summary>
		/// 通话邀请
		/// </summary>
        kNIMVideoChatSessionTypeInviteNotify = 2,		
        /// <summary>
        /// 确认通话，接受拒绝结果
        /// </summary>
        kNIMVideoChatSessionTypeCalleeAckRes = 3,		
        /// <summary>
        /// 确认通话，接受拒绝通知
        /// </summary>
        kNIMVideoChatSessionTypeCalleeAckNotify = 4,		
        /// <summary>
        /// NIMVChatControlType 结果
        /// </summary>
        kNIMVideoChatSessionTypeControlRes = 5,		
        /// <summary>
        /// NIMVChatControlType 通知
        /// </summary>
        kNIMVideoChatSessionTypeControlNotify = 6,		
        /// <summary>
        /// 通话中链接状态通知
        /// </summary>
        kNIMVideoChatSessionTypeConnect = 7,	
        /// <summary>
        /// 通话中成员状态
        /// </summary>
        kNIMVideoChatSessionTypePeopleStatus = 8,		
        /// <summary>
        /// 通话中网络状态
        /// </summary>
        kNIMVideoChatSessionTypeNetStatus = 9,		
        /// <summary>
        /// 通话挂断结果
        /// </summary>
        kNIMVideoChatSessionTypeHangupRes = 10,		
        /// <summary>
        /// 通话被挂断通知
        /// </summary>
        kNIMVideoChatSessionTypeHangupNotify = 11,		
        /// <summary>
        /// 通话接听挂断同步通知
        /// </summary>
        kNIMVideoChatSessionTypeSyncAckNotify = 12,     
        /// <summary>
        /// 通知MP4录制状态，包括开始录制和结束录制
        /// </summary>
        kNIMVideoChatSessionTypeMp4Notify = 13,   
        /// <summary>
        /// 通知实时音视频数据状态
        /// </summary>
        kNIMVideoChatSessionTypeInfoNotify = 14,        
        /// <summary>
        /// 通知实时音频发送和混音的音量状态
        /// </summary>
        kNIMVideoChatSessionTypeVolumeNotify = 15,    
    };

    /// <summary>
    /// 音视频通话控制类型
    /// </summary>
    public enum NIMVChatControlType
    {
        /// <summary>
        /// 打开音频
        /// </summary>
        kNIMTagControlOpenAudio = 1,
        /// <summary>
        /// 关闭音频
        /// </summary>
        kNIMTagControlCloseAudio = 2,
        /// <summary>
        /// 打开视频
        /// </summary>
        kNIMTagControlOpenVideo = 3,
        /// <summary>
        /// 关闭视频
        /// </summary>
        kNIMTagControlCloseVideo = 4,
        /// <summary>
        /// 请求从音频切换到视频
        /// </summary>
        kNIMTagControlAudioToVideo = 5,
        /// <summary>
        /// 同意从音频切换到视频
        /// </summary>
        kNIMTagControlAgreeAudioToVideo = 6,
        /// <summary>
        /// 拒绝从音频切换到视频
        /// </summary>
        kNIMTagControlRejectAudioToVideo = 7,
        /// <summary>
        /// 从视频切换到音频
        /// </summary>
        kNIMTagControlVideoToAudio = 8,
        /// <summary>
        /// 占线
        /// </summary>
        kNIMTagControlBusyLine = 9,
        /// <summary>
        /// 告诉对方自己的摄像头不可用
        /// </summary>
        kNIMTagControlCamaraNotAvailable = 10,
        /// <summary>
        /// 告诉对方自已处于后台
        /// </summary>
        kNIMTagControlEnterBackground = 11,
        /// <summary>
        /// 告诉发送方自己已经收到请求了（用于通知发送方开始播放提示音）
        /// </summary>
        kNIMTagControlReceiveStartNotifyFeedback = 12,
        /// <summary>
        /// 告诉发送方自己开始录制 
        /// </summary>
        kNIMTagControlMp4StartRecord = 13,
        /// <summary>
        /// 告诉发送方自己结束录制
        /// </summary>
        kNIMTagControlMp4StopRecord = 14,
    };

    /// <summary>
    /// 音视频通话类型
    /// </summary>
    public enum NIMVideoChatMode
    {
        /// <summary>
        /// 语音通话模式
        /// </summary>
        kNIMVideoChatModeAudio = 1,
        /// <summary>
        /// 视频通话模式
        /// </summary>
        kNIMVideoChatModeVideo = 2,
    };

    /// <summary>
    /// 音视频通话成员变化类型
    /// </summary>
    public enum NIMVideoChatSessionStatus
    {
        /// <summary>
        /// 成员进入
        /// </summary>
        kNIMVideoChatSessionStatusJoined = 0,
        /// <summary>
        /// 成员退出
        /// </summary>
        kNIMVideoChatSessionStatusLeaved = 1,
    };

    /// <summary>
    /// 音视频通话网络变化类型
    /// </summary>
    public enum NIMVideoChatSessionNetStat
    {
        /// <summary>
        /// 网络状态很好
        /// </summary>
        kNIMVideoChatSessionNetStatVeryGood = 0,
        /// <summary>
        /// 网络状态较好
        /// </summary>
        kNIMVideoChatSessionNetStatGood = 1,
        /// <summary>
        /// 网络状态较差
        /// </summary>
        kNIMVideoChatSessionNetStatBad = 2,
        /// <summary>
        /// 网络状态很差
        /// </summary>
        kNIMVideoChatSessionNetStatVeryBad = 3,
    };

    /// <summary>
    /// 视频通话分辨率，最终长宽比不保证
    /// </summary>
    public enum NIMVChatVideoQuality
    {
        /// <summary>
        /// 视频默认分辨率 480x320
        /// </summary>
        kNIMVChatVideoQualityNormal = 0,
        ///<summary>
        ///视频低分辨率176x144
        /// </summary>       
        kNIMVChatVideoQualityLow = 1,
        ///<summary>
        ///视频中分辨率 352x288
        /// </summary>      
        kNIMVChatVideoQualityMedium = 2,
        ///<summary>
        ///视频高分辨率 480x320
        /// </summary>    
        kNIMVChatVideoQualityHigh = 3,
        ///<summary>
        ///视频超高分辨率 640x480
        /// </summary>       
        kNIMVChatVideoQualitySuper = 4,
        ///<summary>
        ///用于桌面分享级别的分辨率1280x720，需要使用高清摄像头并指定对应的分辨率，或者自定义通道传输 
        /// </summary>    
        kNIMVChatVideoQuality720p = 5,
    };

    /// <summary>
    /// NIMVChatVideoFrameRate 视频通话帧率，实际帧率因画面采集频率和机器性能限制可能达不到期望值
    /// </summary>
    public enum NIMVChatVideoFrameRate
    {
        /// <summary>
        /// 视频通话帧率默认值,最大取每秒15帧
        /// </summary>
        kNIMVChatVideoFrameRateNormal = 0,
        /// <summary>
        /// 视频通话帧率 最大取每秒5帧
        /// </summary>
        kNIMVChatVideoFrameRate5 = 1,
        /// <summary>
        /// 视频通话帧率 最大取每秒10帧
        /// </summary>
        kNIMVChatVideoFrameRate10 = 2,
        /// <summary>
        /// 视频通话帧率 最大取每秒15帧
        /// </summary>
        kNIMVChatVideoFrameRate15 = 3,
        /// <summary>
        /// 视频通话帧率 最大取每秒20帧
        /// </summary>
        kNIMVChatVideoFrameRate20 = 4,
        /// <summary>
        /// 视频通话帧率 最大取每秒25帧
        /// </summary>
        kNIMVChatVideoFrameRate25 = 5,
    };

    /// <summary>
    /// 音视频服务器连接状态类型
    /// </summary>
    public enum NIMVChatConnectErrorCode
    {
        /// <summary>
        /// 断开连接
        /// </summary>
        kNIMVChatConnectDisconn = 0,
        /// <summary>
        /// 启动失败
        /// </summary>
        kNIMVChatConnectStartFail = 1,
        /// <summary>
        /// 超时
        /// </summary>
        kNIMVChatConnectTimeout = 101,
        /// <summary>
        /// 会议模式错误
        /// </summary>
        kNIMVChatConnectMeetingModeError = 102,
        /// <summary>
        /// 非rtmp用户加入rtmp频道
        /// </summary>	  
        kNIMVChatConnectRtmpModeError = 103,
        /// <summary>
        /// 超过频道最多rtmp人数限制
        /// </summary>
        kNIMVChatConnectRtmpNodesError = 104,
        /// <summary>
        /// 已经存在一个主播
        /// </summary>     
        kNIMVChatConnectRtmpHostError = 105,
        /// <summary>
        /// 成功
        /// </summary>
        kNIMVChatConnectSuccess = 200,
        /// <summary>
        /// 错误参数
        /// </summary>
        kNIMVChatConnectInvalidParam = 400,
        /// <summary>
        /// 密码加密错误
        /// </summary>
        kNIMVChatConnectDesKey = 401,
        /// <summary>
        /// 错误请求
        /// </summary>
        kNIMVChatConnectInvalidRequst = 417,
        /// <summary>
        /// 服务器内部错误
        /// </summary>
        kNIMVChatConnectServerUnknown = 500,
        /// <summary>
        /// 退出
        /// </summary>
        kNIMVChatConnectLogout = 1001,
        /// <summary>
        /// 发起失败
        /// </summary>
        kNIMVChatChannelStartFail = 11000,
        /// <summary>
        /// 断开连接
        /// </summary>
        kNIMVChatChannelDisconnected = 11001,
        /// <summary>
        /// 本人SDK版本太低不兼容
        /// </summary>
        kNIMVChatVersionSelfLow = 11002,
        /// <summary>
        /// 对方SDK版本太低不兼容
        /// </summary>
        kNIMVChatVersionRemoteLow = 11003,
    };

    /// <summary>
    /// NIMVChatMp4RecordCode mp4录制状态
    /// </summary>
    public enum NIMVChatMp4RecordCode
    {
        /// <summary>
        /// MP4结束
        /// </summary>
        kNIMVChatMp4RecordClose = 0,
        /// <summary>
        /// MP4结束，视频画面大小变化
        /// </summary>	       
        kNIMVChatMp4RecordVideoSizeError = 1,
        /// <summary>
        /// MP4结束，磁盘空间不足
        /// </summary>
        kNIMVChatMp4RecordOutDiskSpace = 2,
        /// <summary>
        /// MP4文件创建
        /// </summary>    
        kNIMVChatMp4RecordCreate = 200,
        /// <summary>
        /// MP4文件已经存在
        /// </summary>
        kNIMVChatMp4RecordExsit = 400,
        /// <summary>
        /// MP4文件创建失败
        /// </summary>   
        kNIMVChatMp4RecordCreateError = 403,
        /// <summary>
        /// 通话不存在
        /// </summary>
        kNIMVChatMp4RecordInvalid = 404,
    };

	/// <summary>
	/// NIMVChatAudioRecordCode 音频录制状态 */
	/// </summary>
	public enum NIMVChatAudioRecordCode
	{
		/// <summary>
		/// 录制正常结束
		/// </summary>
		kNIMVChatAudioRecordClose = 0,
		/// <summary>
		/// 录制结束，磁盘空间不足
		/// </summary>
		kNIMVChatAudioRecordOutDiskSpace = 2,
		/// <summary>
		/// 文件创建成功
		/// </summary>
		kNIMVChatAudioRecordCreate = 200,
		/// <summary>
		/// 已经存在
		/// </summary>
		kNIMVChatAudioRecordExsit = 400,        
		/// <summary>
		/// 文件创建失败
		/// </summary>
		kNIMVChatAudioRecordCreateError = 403,
		/// <summary>
		/// 通话不存在
		/// </summary>
		kNIMVChatAudioRecordInvalid = 404,      
	};

	/* 3.6.0 sdk已不支持
	/// <summary>
	/// NIMVChatSetStreamingModeCode 设置推流模式返回码
	/// </summary>
	public enum NIMVChatSetStreamingModeCode
    {
        /// <summary>
        /// 无效的操作
        /// </summary>
        kNIMVChatBypassStreamingInvalid = 0,	
		/// <summary>
        /// 设置成功
		/// </summary>
        kNIMVChatBypassStreamingSuccess = 200,	
		/// <summary>
        /// 超过最大允许直播节点数量
		/// </summary>
        kNIMVChatBypassStreamingErrorExceedMax = 202,	
	    /// <summary>
        /// 必须由主播第一个开启直播
	    /// </summary>
        kNIMVChatBypassStreamingErrorHostNotJoined = 203,
		/// <summary>
		///  互动直播服务器错误
		/// </summary>
        kNIMVChatBypassStreamingErrorServerError = 204,	
		/// <summary>
		/// 互动直播其他错误
		/// </summary>
        kNIMVChatBypassStreamingErrorOtherError = 205,			
        /// <summary>
        ///  互动直播服务器没有响应
        /// </summary>
        kNIMVChatBypassStreamingErrorNoResponse = 404,			
        /// <summary>
        /// 重连过程中无法进行相关操作，稍后再试
        /// </summary>
        kNIMVChatBypassStreamingErrorReconnecting = 405,	
		/// <summary>
		/// 互动直播设置超时
		/// </summary>
        kNIMVChatBypassStreamingErrorTimeout = 408,	
    };
	*/

	/// <summary>
	/// @enum NIMVChatVideoFrameScaleType 视频画面长宽比，裁剪时不改变横竖屏，如4：3，代表宽高横屏4：3或者竖屏3：4  *
	/// </summary>
	public enum NIMVChatVideoFrameScaleType
	{
		/// <summary>
		/// 默认，不裁剪
		/// </summary>
		kNIMVChatVideoFrameScaleNone = 0,
		/// <summary>
		///  裁剪成1：1的形状
		/// </summary>   
		kNIMVChatVideoFrameScale1x1 = 1,
		/// <summary>
		/// 裁剪成4：3的形状，如果是
		/// </summary>
		kNIMVChatVideoFrameScale4x3 = 2,
		/// <summary>
		/// 裁剪成16：9的形状
		/// </summary>
		kNIMVChatVideoFrameScale16x9 = 3, 
	};

	/// <summary>
	///  NIMVChatVideoSplitMode 主播设置的直播分屏模式 
	/// </summary>
	public enum NIMVChatVideoSplitMode
    {
       /// <summary>
       ///  底部横排浮窗
       /// </summary>
        kNIMVChatSplitBottomHorFloating = 0,	
		/// <summary>
		/// 顶部横排浮窗
		/// </summary>
        kNIMVChatSplitTopHorFloating = 1,			
       /// <summary>
       /// 平铺
       /// </summary>
        kNIMVChatSplitLatticeTile = 2,		
	   /// <summary>
	   /// 裁剪平铺
	   /// </summary>
        kNIMVChatSplitLatticeCuttingTile = 3,	
    };

    /// <summary>
    /// 发起和接受通话时的参数
    /// </summary>
    public class NIMVChatInfo : NimUtility.NimJsonObject<NIMVChatInfo>
    {
		/// <summary>
		/// 成员id列表，主动发起非空(必填)
		/// </summary>
		[Newtonsoft.Json.JsonProperty(PropertyName = "uids", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public List<string> Uids { get; set; }

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
		/// 是否需要录制音频数据 >0表示是 （需要服务器配置支持，本地录制直接调用接口函数）
		/// </summary>
		[Newtonsoft.Json.JsonProperty("record")]
		public int ServerAudioRecord { get; set; }

		/// <summary>
		/// 是否需要录制视频数据 >0表示是 （需要服务器配置支持，本地录制直接调用接口函数）
		/// </summary>
		[Newtonsoft.Json.JsonProperty("video_record")]
		public int ServerVideoRecord { get; set; }

        /// <summary>
        ///  视频发送编码码率 >=100000 <=600000有效
        /// </summary>
        [Newtonsoft.Json.JsonProperty("max_video_rate")]
        public int MaxVideoRate { get; set; }

        /// <summary>
        /// 视频聊天分辨率选择 NIMVChatVideoQuality
        /// </summary>
        [Newtonsoft.Json.JsonProperty("video_quality")]
        public int VideoQuality { get; set; }

        /// <summary>
        /// 视频画面帧率 NIMVChatVideoFrameRate
        /// </summary>
        [Newtonsoft.Json.JsonProperty("frame_rate")]
        public int FrameRate { get; set; }

		/// <summary>
		/// 直播推流地址(加入多人时有效)，非空代表主播旁路直播， kNIMVChatBypassRtmp决定是否开始推流
		/// </summary>
		[Newtonsoft.Json.JsonProperty("rtmp_url")]
        public string RtmpUrl { get; set; }

        /// <summary>
        /// 是否是旁路直播观众，此时MeetingMode无效 >0表示是
        /// </summary>
        [Newtonsoft.Json.JsonProperty("bypass_rtmp")]
        public int BypassRtmp { get; set; }

        /// <summary>
        /// 是否开启服务器对直播推流录制（需要开启服务器能力）， >0表示是
        /// </summary>
        [Newtonsoft.Json.JsonProperty("rtmp_record")]
        public int RtmpRecord { get; set; }

        /// <summary>
        /// 主播控制的直播推流时的分屏模式 NIMVChatVideoSplitMode
        /// </summary>
        [Newtonsoft.Json.JsonProperty("split_mode")]
        public int SplitMode { get; set; }

        /// <summary>
        /// 是否需要推送 >0表示是 默认是
        /// </summary>
        [Newtonsoft.Json.JsonProperty("push_enable")]
        public int PushEnable { get; set; }

        /// <summary>
        /// 是否需要角标计数 >0表示是 默认是
        /// </summary>
        [Newtonsoft.Json.JsonProperty("need_badge")]
        public int NeedBadge { get; set; }

        /// <summary>
        /// 是否需要推送昵称 >0表示是 默认是
        /// </summary>
        [Newtonsoft.Json.JsonProperty("need_nick")]
        public int NeedNick { get; set; }


        [Newtonsoft.Json.JsonProperty("high_rate")]
        public int AudioHighRate { get; set; }

        /// <summary>
        /// JSON格式,推送payload
        /// </summary>
        [Newtonsoft.Json.JsonProperty("payload")]
        public string PayLoad { get; set; }

        /// <summary>
        /// 推送声音
        /// </summary>
        [Newtonsoft.Json.JsonProperty("sound")]
        public string Sound { get; set; }

		/// <summary>
		/// int, 是否强制持续呼叫（对方离线也会呼叫）,1表示是，0表示否。默认是
		/// </summary>
		[Newtonsoft.Json.JsonProperty("keepcalling")]
		public int KeepCalling { get; set; }


      
        public NIMVChatInfo()
        {
            CustomAudio = 0;
            CustomVideo = 0;
			ServerAudioRecord = 0;
			ServerVideoRecord = 0;
            AudioHighRate = 0;
            MaxVideoRate = 0;
            VideoQuality = 0;
            FrameRate = 0;
            RtmpUrl = "";
            BypassRtmp = 0;
            SplitMode = 0;
            PushEnable = 1;
            NeedBadge = 1;
            NeedNick = 1;
            PayLoad = "";
            Sound = "";
			KeepCalling = 1;
            Uids = null;
        }
    }

    public class NIMVChatSessionInfo : NimUtility.NimJsonObject<NIMVChatSessionInfo>
    {
		/// <summary>
		/// 用户账号uid
		/// </summary>
		[Newtonsoft.Json.JsonProperty(PropertyName = "uid", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Uid { get; set; }

		/// <summary>
		/// 状态
		/// </summary>
		[Newtonsoft.Json.JsonProperty(PropertyName = "status", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Status { get; set; }

		/// <summary>
		/// 录制地址（服务器开启录制时有效）
		/// </summary>
		[Newtonsoft.Json.JsonProperty(PropertyName = "record_addr", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string RecordAddr { get; set; }

		/// <summary>
		/// 服务器音频录制文件名（服务器开启录制时有效）
		/// </summary>
		[Newtonsoft.Json.JsonProperty(PropertyName = "record_file", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string RecordFile { get; set; }

		/// <summary>
		/// 服务器视频录制文件名（服务器开启录制时有效）
		/// </summary>
		[Newtonsoft.Json.JsonProperty(PropertyName = "video_record_file", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
		public string VideoRecordFile { get; set; }

		/// <summary>
		/// 状态
		/// </summary>
		[Newtonsoft.Json.JsonProperty(PropertyName = "type", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Type { get; set; }

		/// <summary>
		/// 时间 单位毫秒
		/// </summary>
		[Newtonsoft.Json.JsonProperty(PropertyName = "time", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public long Time { get; set; }

		/// <summary>
		/// 是否接受 >0表示接受
		/// </summary>
		[Newtonsoft.Json.JsonProperty(PropertyName = "accept", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Accept { get; set; }

		/// <summary>
		/// 客户端类型 NIMClientType 见NIMClientDef.cs
		/// </summary>
		[Newtonsoft.Json.JsonProperty(PropertyName = "client", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Client { get; set; }

        public NIMVChatSessionInfo()
        {
            Uid = null;
            Status = 0;
            RecordAddr = null;
            RecordFile = null;
			VideoRecordFile = null;
            Type = 0;
            Time = 0;
            Accept = 0;
            Client = 0;
        }
    }

    /// <summary>
    /// 音量状态
    /// </summary>
    public class NIMVchatAudioVolumeState : NimUtility.NimJsonObject<NIMVchatAudioVolumeState>
    {
        public class AudioVolume
        {
            public class State
            {
                [Newtonsoft.Json.JsonProperty(PropertyName = "status", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
                public int Status { get; set; }
            }

            public class ReceiverState : State
            {
                [Newtonsoft.Json.JsonProperty(PropertyName = "uid", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
                public string Uid { get; set; }
            }

            /// <summary>
            /// 自己的状态
            /// </summary>
            [Newtonsoft.Json.JsonProperty(PropertyName = "self", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public State Self { get; set; }

            /// <summary>
            /// 接收方状态
            /// </summary>
            [Newtonsoft.Json.JsonProperty(PropertyName = "receiver", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public ReceiverState[] Receivers { get; set; }
        }

		/// <summary>
		/// 音频实时音量通知，包含发送的音量kNIMVChatSelf和接收音量kNIMVChatReceiver，kNIMVChatStatus的音量值是pcm的平均值最大为int16_max
		/// </summary>
		[Newtonsoft.Json.JsonProperty(PropertyName = "audio_volume", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public AudioVolume Volume { get; set; }
    }

    /// <summary>
    /// 实时状态 
    /// </summary>
    public class NIMVChatRealtimeState : NimUtility.NimJsonObject<NIMVChatRealtimeState>
    {
        public class StateInfo
        {
            public class Video : Audio
            {
                [Newtonsoft.Json.JsonProperty(PropertyName = "width", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
                public int Width { get; set; }

                [Newtonsoft.Json.JsonProperty(PropertyName = "height", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
                public int Height { get; set; }
            }

            public class Audio
            {
				/// <summary>
				/// 每秒帧率或者每秒发包数
				/// </summary>
				[Newtonsoft.Json.JsonProperty(PropertyName = "fps", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
                public int FPS { get; set; }

				/// <summary>
				/// 每秒流量，单位为“千字节”
				/// </summary>
				[Newtonsoft.Json.JsonProperty(PropertyName = "KBps", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
                public int KBps { get; set; }
            }

            /// <summary>
            /// 视频信息
            /// </summary>
            [Newtonsoft.Json.JsonProperty(PropertyName = "video", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public Video VideoState { get; set; }

            /// <summary>
            /// 语音信息
            /// </summary>
            [Newtonsoft.Json.JsonProperty(PropertyName = "audio", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public Audio AudioState { get; set; }
        }

		/// <summary>
		/// 音视频实时状态
		/// </summary>
		[Newtonsoft.Json.JsonProperty(PropertyName = "static_info", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public StateInfo Info { get; set; }
    }


	/// <summary>
	/// 录制MP4文件接口封装的json类
	/// </summary>
	public class NIMVChatMP4RecordJsonEx : NimUtility.NimJsonObject<NIMVChatMP4RecordJsonEx>
	{
		/// <summary>
		/// kNIMVChatUid录制的成员，如果是自己填空
		/// </summary>
		[Newtonsoft.Json.JsonProperty(PropertyName = "uid", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
		public string RecordUid { get; set; }

		public NIMVChatMP4RecordJsonEx()
		{
			RecordUid = "";
		}
	}
	/// <summary>
	/// 加入聊天室的josn拓展封装类
	/// </summary>
	public class NIMJoinRoomJsonEx:NimUtility.NimJsonObject<NIMJoinRoomJsonEx>
	{
		//{"custom_video":0, "custom_audio":0, "video_quality":0, "session_id":"1231sda", "rtmp_url":"", "bypass_rtmp":0}
		/// <summary>
		/// 是否用自主的视频数据 >0表示是
		/// </summary>
		[Newtonsoft.Json.JsonProperty(PropertyName = "custom_video", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
		public int CustomVideo { get; set; }

		/// <summary>
		/// 是否用自主的音频数据 >0表示是
		/// </summary>
		[Newtonsoft.Json.JsonProperty(PropertyName = "custom_audio", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
		public int CustomAudio { get; set; }

		/// <summary>
		/// 视频聊天分辨率选择 NIMVChatVideoQuality
		/// </summary>
		[Newtonsoft.Json.JsonProperty(PropertyName = "video_quality", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
		public int VideoQuality { get; set; }

		/// <summary>
		/// 发起会话的标识id，将在创建通话及结束通话时有效，帮助针对无channelid的情况下进行映射
		/// </summary>
		[Newtonsoft.Json.JsonProperty(PropertyName = "session_id", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
		public string SessionId { get; set; }

		/// <summary>
		/// 直播推流地址(加入多人时有效)，非空代表主播旁路直播， BypassRtmp决定是否开始推流
		/// </summary>
		[Newtonsoft.Json.JsonProperty(PropertyName = "rtmp_url", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
		public string RtmpUrl { get; set; }

		/// <summary>
		/// 是否旁路推流（如果rtmpurl为空是连麦观众，非空是主播的推流控制）， >0表示是
		/// </summary>
		[Newtonsoft.Json.JsonProperty(PropertyName = "bypass_rtmp", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
		public int BypassRtmp { get; set; }

		public NIMJoinRoomJsonEx()
		{
			CustomVideo = 0;
			CustomAudio = 0;
			VideoQuality = 0;
			SessionId = "";
			RtmpUrl = "";
			BypassRtmp = 0;
		}
	}


	public class NIMVChatCustomAudioJsonEx : NimUtility.NimJsonObject<NIMVChatCustomAudioJsonEx>
	{

		/// <summary>
		/// 采样频率
		/// </summary>
		[Newtonsoft.Json.JsonProperty(PropertyName = "sample_rate", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
		public int SampleRate { get; set; }

		/// <summary>
		/// 采样位深
		/// </summary>
		[Newtonsoft.Json.JsonProperty(PropertyName = "sample_bit", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
		public int SampleBit { get; set; }

		public NIMVChatCustomAudioJsonEx()
		{
			SampleRate = 16000;
			SampleBit = 16;
		}
	}

	public class NIMVChatCustomVideoJsonEx: NimUtility.NimJsonObject<NIMVChatCustomVideoJsonEx>
	{
		/// <summary>
		/// 视频数据类型，NIMVideoSubType
		/// </summary>
		[Newtonsoft.Json.JsonProperty(PropertyName = "subtype", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
		public int VideoSubType { get; set; }
		public NIMVChatCustomVideoJsonEx()
		{
			VideoSubType = Convert.ToInt32(NIMVideoSubType.kNIMVideoSubTypeI420);
		}
	}


	/// <summary>
	/// 调用接口回调
	/// </summary>
	/// <param name="channel_id">频道id</param>
	/// <param name="code">结果</param>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void onSessionHandler(long channel_id, int code);

    /// <summary>
    /// 收到邀请
    /// </summary>
    /// <param name="channel_id">频道id</param>
    /// <param name="uid">对方uid</param>
    /// <param name="mode">通话类型</param>
    /// <param name="time">毫秒级 时间戳</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void onSessionInviteNotify(long channel_id, string uid, int mode, long time);

    /// <summary>
    /// 确认通话，接受拒绝通知
    /// </summary>
    /// <param name="channel_id">频道id</param>
    /// <param name="uid">对方uid</param>
    /// <param name="mode">通话类型</param>
    /// <param name="accept">结果</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void onSessionCalleeAckNotify(long channel_id, string uid, int mode, bool accept);

    /// <summary>
    /// 控制操作结果
    /// </summary>
    /// <param name="channel_id">频道id</param>
    /// <param name="code">结果</param>
    /// <param name="type">操作类型</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void onSessionControlRes(long channel_id, int code, int type);

    /// <summary>
    /// 控制操作通知
    /// </summary>
    /// <param name="channel_id">频道id</param>
    /// <param name="uid">对方uid</param>
    /// <param name="type">操作类型</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void onSessionControlNotify(long channel_id, string uid, int type);

    /// <summary>
    /// 通话中链接状态通知
    /// </summary>
    /// <param name="channel_id">频道id</param>
    /// <param name="code">结果状态</param>
    /// <param name="record_addr">录制音频文件名（服务器开启录制时有效）</param>
    /// <param name="record_file">录制视频文件名（服务器开启录制时有效）</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void onSessionConnectNotify(long channel_id, int code, string record_file, string video_record_file);

    /// <summary>
    /// 通话中成员状态
    /// </summary>
    /// <param name="channel_id">频道id</param>
    /// <param name="uid">对方uid</param>
    /// <param name="status">状态</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void onSessionPeopleStatus(long channel_id, string uid, int status);

    /// <summary>
    /// 通话中网络状态
    /// </summary>
    /// <param name="channel_id">频道id</param>
    /// <param name="status">状态</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void onSessionNetStatus(long channel_id, int status);

    /// <summary>
    /// 其他端接听挂断后的同步通知
    /// </summary>
    /// <param name="channel_id">频道id</param>
    /// <param name="uid">对方uid</param>
    /// <param name="mode">通话类型</param>
    /// <param name="accept">结果</param>
    /// <param name="time">毫秒级 时间戳</param>
    /// <param name="client">客户端类型</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void onSessionSyncAckNotify(long channel_id, int code, string uid, int mode, bool accept, long time, int client);

    /// <summary>
    /// 音量状态通知
    /// </summary>
    /// <param name="channel_id">频道id</param>
    /// <param name="code">结果状态</param>
    /// <param name="state">音量状态信息</param>
    public delegate void onSessionVolumeNotify(long channel_id, int code, NIMVchatAudioVolumeState state);

    /// <summary>
    /// 视频实时状态信息通知
    /// </summary>
    /// <param name="channel_id">频道id</param>
    /// <param name="code">结果状态</param>
    /// <param name="state">实时状态信息</param>
    public delegate void onSessionRealtimeInfoNotify(long channel_id, int code, NIMVChatRealtimeState state);
}
#endif