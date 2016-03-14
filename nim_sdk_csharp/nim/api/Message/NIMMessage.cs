/** @file NIMMessage.cs
  * @brief NIM SDK 消息数据的定义
  * @copyright (c) 2015, NetEase Inc. All rights reserved
  * @author Harrison
  * @date 2015/12/8
  */

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NIM
{
    public class NIMTextMessage : NIMIMMessage
    {
        [JsonProperty("msg_body")]
        public string TextContent { get; set; }

        public NIMTextMessage()
        {
            MessageType = NIMMessageType.kNIMMessageTypeText;
        }
    }

    public class NIMImageMessage : NIMIMMessage
    {
        [JsonProperty(AttachmentPath)]
        public NIMImageAttachment ImageAttachment { get; set; }

        public NIMImageMessage()
        {
            MessageType = NIMMessageType.kNIMMessageTypeImage;
        }
    }

    public class NIMFileMessage : NIMIMMessage
    {
        [JsonProperty(AttachmentPath)]
        public NIMMessageAttachment FileAttachment { get; set; }

        public NIMFileMessage()
        {
            MessageType = NIMMessageType.kNIMMessageTypeFile;
        }
    }

    public class NIMAudioMessage : NIMIMMessage
    {
        [JsonProperty(AttachmentPath)]
        public NIMAudioAttachment AudioAttachment { get; set; }

        public NIMAudioMessage()
        {
            MessageType = NIMMessageType.kNIMMessageTypeAudio;
        }
    }

    public class NIMVedioMessage : NIMIMMessage
    {
        [JsonProperty(AttachmentPath)]
        public NIMVedioAttachment VedioAttachment { get; set; }

        public NIMVedioMessage()
        {
            MessageType = NIMMessageType.kNIMMessageTypeVideo;
        }
    }

    public class NIMLocationMessage : NIMIMMessage
    {
        [JsonProperty(AttachmentPath)]
        public NIMLocationMsgInfo LocationInfo { get; set; }

        public NIMLocationMessage()
        {
            MessageType = NIMMessageType.kNIMMessageTypeLocation;
        }
    }

    public class NIMTeamNotification : NimUtility.NimJsonObject<NIMTeamNotification>
    {
        [JsonProperty("ids")]
        public List<string> IdCollection { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("tinfo")]
        public Team.NIMTeamInfo TeamInfo { get; set; }

        [JsonProperty("team_member")]
        public Team.NIMTeamMemberInfo MemberInfo { get; set; }
    }

    public class NotificationData
    {
        [JsonProperty("data")]
        public NIMTeamNotification Data { get; set; }

        [JsonProperty("id")]
        public Team.NIMNotificationType NotificationId { get; set; }
    }

    public class NIMTeamNotificationMessage : NIMIMMessage
    {
        [JsonProperty(AttachmentPath)]
        public NotificationData NotifyMsgData { get; set; }
    }

    public class NIMUnknownMessage : NIMIMMessage
    {
        public string RawMessage { get; set; }

        public NIMUnknownMessage()
        {
            MessageType = NIMMessageType.kNIMMessageTypeUnknown;
        }
    }

    public class NIMMessageAttachment:NimUtility.NimJsonObject<NIMMessageAttachment>
    {
        /// <summary>
        /// 文件内容MD5
        /// </summary>
        [JsonProperty("md5")]
        public string MD5 { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        [JsonProperty("size")]
        public long Size { get; set; }

        /// <summary>
        /// 上传云端后得到的文件下载地址
        /// </summary>
        [JsonProperty("url")]
        public string RemoteUrl { get; set; }

        /// <summary>
        /// 用于显示的文件名称
        /// </summary>
        [JsonProperty("ext")]
        public string DisplayName { get; set; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        [JsonProperty("name")]
        public string FileExtension { get; set; }

        [JsonProperty("res_id")]
        public string LocalResID { get; set; }
    }

    public class NIMImageAttachment : NIMMessageAttachment
    {
        /// <summary>
        /// 图片宽度
        /// </summary>
        [JsonProperty("w")]
        public int Width { get; set; }

        /// <summary>
        /// 图片高度
        /// </summary>
        [JsonProperty("h")]
        public int Height { get; set; }
    }

    public class NIMAudioAttachment : NIMMessageAttachment
    {

        [JsonProperty("dur")]
        public int Duration { get; set; }
    }

    public class NIMVedioAttachment : NIMMessageAttachment
    {
        [JsonProperty("dur")]
        public int Duration { get; set; }

        [JsonProperty("w")]
        public int Width { get; set; }

        [JsonProperty("h")]
        public int Height { get; set; }
    }

    public class NIMLocationMsgInfo
    {
        [JsonProperty("title")]
        public string Description { get; set; }

        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lng")]
        public double Longitude { get; set; }
    }

    public abstract class NIMIMMessage : NimUtility.NimJsonObject<NIMIMMessage>
    {
        /// <summary>
        /// 发送者ID
        /// </summary>
        [JsonProperty("from_id")]
        public string SenderID { get; set; }

        [JsonProperty("to_type")]
        public Session.NIMSessionType SessionType { get; set; }

        /// <summary>
        /// 接收者ID
        /// </summary>
        [JsonProperty("to_accid")]
        public string ReceiverID { get; set; }

        /// <summary>
        /// 消息时间戳（毫秒）
        /// </summary>
        [JsonProperty(PropertyName = "time", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long TimeStamp { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        [JsonProperty(MessageTypePath)]
        public NIMMessageType MessageType { get; protected set; }

        /// <summary>
        /// 消息ID（客户端）
        /// </summary>
        [JsonProperty("client_msg_id")]
        public string ClientMsgID { get; set; }

        [JsonProperty("local_res_path")]
        public string LocalFilePath { get; set; }

        /// <summary>
        /// 该消息是否存储云端历史
        /// </summary>
        [JsonProperty(PropertyName = "cloud_history", NullValueHandling = NullValueHandling.Ignore)]
        public NIMMessageSettingStatus? ServerSaveHistory { get; set; }

        /// <summary>
        /// 该消息是否支持漫游
        /// </summary>
        [JsonProperty(PropertyName = "roam_msg", NullValueHandling = NullValueHandling.Ignore)]
        public NIMMessageSettingStatus? Roaming { get; set; }

        /// <summary>
        /// 该消息是否支持发送者多端同步
        /// </summary>
        [JsonProperty(PropertyName = "sync_msg", NullValueHandling = NullValueHandling.Ignore)]
        public NIMMessageSettingStatus? MultiSync { get; set; }

        /// <summary>
        /// 消息是否要存离线
        /// </summary>
        [JsonIgnore]
        public NIMMessageSettingStatus? OfflineStorage { get; set; }

        /// <summary>
        /// 该消息是否在接收方被静音处理
        /// </summary>
        [JsonIgnore]
        public NIMMessageSettingStatus? BeMuted { get; set; }

        /// <summary>
        /// 是否需要推送
        /// </summary>
        [JsonProperty(PropertyName = "push_enable", NullValueHandling = NullValueHandling.Ignore)]
        public NIMMessageSettingStatus? NeedPush { get; set; }

        /// <summary>
        /// 是否要做消息计数
        /// </summary>
        [JsonProperty(PropertyName = "need_badge", NullValueHandling = NullValueHandling.Ignore)]
        public NIMMessageSettingStatus? NeedCounting { get; set; }

        /// <summary>
        /// 需要推送昵称
        /// </summary>
        [JsonProperty(PropertyName = "push_nick", NullValueHandling = NullValueHandling.Ignore)]
        public NIMMessageSettingStatus? NeedPushNick { get; set; }

        public override string Serialize()
        {
            ClientMsgID = NimUtility.Utilities.GenerateGuid();
            string jsonValue = base.Serialize();
            var rootObj = Newtonsoft.Json.Linq.JObject.Parse(jsonValue); 
            MessageFactory.ConvertAttachObjectToString(rootObj.Root);
            var newValue = rootObj.ToString(Formatting.None);
            return newValue;
        }

        public void SetMessageStatus(NIMMessageSetting setting)
        {
            this.BeMuted = setting.BeMuted;
            this.MultiSync = setting.MultiSync;
            this.NeedCounting = setting.NeedCounting;
            this.NeedPush = setting.NeedPush;
            this.NeedPushNick = setting.NeedPushNick;
            this.OfflineStorage = setting.OfflineStorage;
        }
        internal const string MessageTypePath = "msg_type";
        internal const string AttachmentPath = "msg_attach";
    }

    
}
