using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace NIMChatRoom
{
    /// <summary>
    /// 设置成员的成员身份
    /// </summary>
    public enum NIMChatRoomMemberAttribute
    {
        /// <summary>
        ///固定成员,operator必须是创建者 
        /// </summary>
        kNIMChatRoomMemberAttributeAdminister = 1,

        /// <summary>
        ///非固定成员,operator必须是创建者或管理员 
        /// </summary>
        kNIMChatRoomMemberAttributeNomalSold = 2,

        /// <summary>
        ///黑名单,operator必须是创建者或管理员 
        /// </summary>
        kNIMChatRoomMemberAttributeBlackList = -1,

        /// <summary>
        ///禁言,operator必须是创建者或管理员 
        /// </summary>
        kNIMChatRoomMemberAttributeMuteList = -2
    }

    public class MemberProperty:NimUtility.NimJsonObject<MemberProperty>
    {
        [JsonProperty("account_id")]
        public string MemberId { get;private set; }

        [JsonProperty("attribute")]
        public NIMChatRoomMemberAttribute Attribute { get; set; }

        [JsonProperty("opt")]
        public bool Optional { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("notify_ext")]
        public string NotifyExtension { get; set; }
    }
}
