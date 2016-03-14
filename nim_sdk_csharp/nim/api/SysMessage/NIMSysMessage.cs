/** @file NIMSysMessage.cs
  * @brief NIM SDK提供的系统消息相关的定义 
  * @copyright (c) 2015, NetEase Inc. All rights reserved
  * @author Harrison
  * @date 2015/12/8
  */

using System;
using Newtonsoft.Json;

namespace NIM.SysMessage
{
    public class NIMSysMessag:NimUtility.NimJsonObject<NIMSysMessag>
    {
        //__int64 timetag_;           /**< 通知时间戳（毫秒） */
        //NIMSysMsgType type_;            /**< 通知类型 */
        //std::string receiver_accid_;    /**< 接收者ID */
        //std::string sender_accid_;      /**< 发送者ID */
        //std::string content_;           /**< 通知内容 */
        //std::string attach_;            /**< 通知附件 */
        //__int64 id_;                /**< 通知ID */
        //BoolStatus support_offline_;    /**< 是否支持离线消息*/
        //std::string apns_text_;         /**< 推送通知内容 */
        //NIMSysMsgStatus status_;        /**< 通知状态 */

        //Json::Value push_payload_;      /**< 第三方自定义的推送属性，限制数据类型Json，长度2048 */
        //BoolStatus push_enable_;        /**< 是否需要推送*/
        //BoolStatus push_need_badge_;    /**< 推送是否需要做消息计数*/
        //BoolStatus push_need_nick_; /**< 推送是否需要昵称*/

        //NIMResCode rescode_;            /**< 通知错误码 */
        //NIMMessageFeature feature_; /**< 通知属性 */
        //int total_unread_count_;/**< 总计的通知未读数 */
        //std::string client_msg_id_;		/**< 通知ID（客户端） */

        /// <summary>
        ///通知错误码 
        /// </summary>
        [JsonProperty("rescode")]
        public ResponseCode Response { get; set; }

        /// <summary>
        ///通知属性 
        /// </summary>
        [JsonProperty("feature")]
        public NIMMessageFeature Feature { get; set; }

        /// <summary>
        ///总计的通知未读数 
        /// </summary>
        [JsonProperty("unread_count")]
        int TotalUnread { get; set; }

        /// <summary>
        ///通知内容 
        /// </summary>
        [JsonProperty("content")]
        public NIMSysMessageContent Content { get; set; }

    }

    public class NIMSysMessageContent:NimUtility.NimJsonObject<NIMSysMessageContent>
    {
        [JsonProperty("msg_time")]
        public long Timetag { get; set; }

        [JsonProperty("msg_type")]
        public NIMSysMsgType MsgType { get; set; }

        [JsonProperty("to_account")]
        public string ReceiverId { get; set; }

        [JsonProperty("from_account")]
        public string SenderId { get; set; }

        [JsonProperty("attach")]
        public string Attachment { get; set; }

        [JsonProperty("msg_id")]
        public long Id { get; set; }

        [JsonProperty("custom_save_flag")]
        public NIMMessageSettingStatus SupportOffline { get; set; }

        [JsonProperty("custom_apns_text")]
        public string PushContent { get; set; }

        [JsonProperty("log_status")]
        public NIMSysMsgStatus Status { get; set; }

        [JsonProperty("push_enable")]
        public NIMMessageSettingStatus NeedPush { get; set; }

        [JsonProperty("need_badge")]
        public NIMMessageSettingStatus NeedPushCount { get; set; }

        [JsonProperty("push_nick")]
        public NIMMessageSettingStatus NeedPushNick { get; set; }

        [JsonProperty("client_msg_id")]
        public string ClientMsgId { get; set; }

        [JsonProperty("push_payload")]
        public string CustomPushContent { get; set; }

        public string GenerateMsgId()
        {
            return NimUtility.Utilities.GenerateGuid();
        }
    }

    public class NIMSysMsgQueryResult : NimUtility.NimJsonObject<NIMSysMsgQueryResult>
    {
        [JsonIgnore]
        public int Count { get; set; }

        [JsonProperty("unread_count")]
        public int UnreadCount { get; private set; }

        [JsonProperty("content")]
        public NIMSysMessageContent[] MsgCollection { get;private set; }
    }

    public class NIMSysMsgEventArgs : EventArgs
    {
        public NIMSysMessag Message { get; private set; }

        public NIMSysMsgEventArgs(NIMSysMessag msg)
        {
            Message = msg;
        }
    }
}
