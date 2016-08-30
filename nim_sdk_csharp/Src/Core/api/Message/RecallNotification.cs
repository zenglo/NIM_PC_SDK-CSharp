using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NIM
{
    /// <summary>
    /// 消息撤回通知
    /// </summary>
    public class RecallNotification : NimUtility.NimJsonObject<RecallNotification>
    {
        /// <summary>
        /// 会话类型
        /// </summary>
        [JsonProperty("to_type")]
        public NIM.Session.NIMSessionType SessionType { get; set; }

        /// <summary>
        /// 消息发送方ID
        /// </summary>
        [JsonProperty("from_id")]
        public string SenderId { get; set; }

        /// <summary>
        /// 消息接收方ID
        /// </summary>
        [JsonProperty("to_id")]
        public string ReceiverId { get; set; }

        /// <summary>
        /// 客户端消息ID
        /// </summary>
        [JsonProperty("msg_id")]
        public string MsgId { get; set; }

        /// <summary>
        /// 自定义通知文案
        /// </summary>
        [JsonProperty("notify")]
        public string NOtify { get; set; }
    }
}
