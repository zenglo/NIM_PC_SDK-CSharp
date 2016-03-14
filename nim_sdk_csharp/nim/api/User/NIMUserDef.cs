/** @file NIMUserDef.cs
  * @brief NIM SDK提供的User相关定义
  * @copyright (c) 2015, NetEase Inc. All rights reserved
  * @author Harrison
  * @date 2015/12/8
  */

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NIM.User
{
    /// <summary>
    /// 用户特殊关系数据变更类型
    /// </summary>
    public enum NIMUserRelationshipChangeType
    {
        /// <summary>
        /// 添加删除黑名单
        /// </summary>
        AddRemoveBlacklist = 1,
        /// <summary>
        /// 添加删除静音名单
        /// </summary>
        AddRemoveMute = 2,
        /// <summary>
        /// 同步黑名单和静音名单
        /// </summary>
        SyncMuteAndBlackList = 3,
    }

    /// <summary>
    /// 好友关系
    /// </summary>
    public class UserSpecialRelationshipItem : NimUtility.NimJsonObject<UserSpecialRelationshipItem>
    {
        [JsonProperty("accid")]
        public string AccountId { get; set; }

        [JsonProperty("create_timetag")]
        public long CratedTime { get; set; }

        [JsonProperty("update_timetag")]
        public long UpdatedTime { get; set; }

        [JsonProperty("is_black")]
        public bool IsBlacklist { get; set; }

        [JsonProperty("is_mute")]
        public bool IsMuted { get; set; }
    }

    public class UserNameCard : NimUtility.NimJsonObject<UserNameCard>
    {
        [JsonProperty(PropertyName = "accid", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountId { get; set; }

        
        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        public string NickName { get; set; }

        [JsonProperty(PropertyName = "icon", NullValueHandling = NullValueHandling.Ignore)]
        public string IconUrl { get; set; }

        [JsonProperty(PropertyName = "sign", NullValueHandling = NullValueHandling.Ignore)]
        public string Signature { get; set; }

        [JsonProperty(PropertyName = "gender", NullValueHandling = NullValueHandling.Ignore)]
        public int? Gender { get; set; }

        [JsonProperty(PropertyName = "email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "birth", NullValueHandling = NullValueHandling.Ignore)]
        public string Birthday { get; set; }

        [JsonProperty(PropertyName = "mobile", NullValueHandling = NullValueHandling.Ignore)]
        public string Mobile { get; set; }

        [JsonProperty(PropertyName = "ex", NullValueHandling = NullValueHandling.Ignore)]
        public string ExpandedData { get; set; }

        [JsonProperty(PropertyName = "create_timetag", NullValueHandling = NullValueHandling.Ignore)]
        public long? CreateTime { get; set; }

        [JsonProperty(PropertyName = "update_timetag", NullValueHandling = NullValueHandling.Ignore)]
        public long? UpdatedTime { get; set; }

    }

    public class UserNameCardChangedArgs : EventArgs
    {
        public List<UserNameCard> UserNameCardList { get; private set; }

        public UserNameCardChangedArgs(List<UserNameCard> cards)
        {
            UserNameCardList = cards;
        }
    }

    public class UserRelationshipSyncArgs : EventArgs
    {
        public UserSpecialRelationshipItem[] Items { get; private set; }

        public UserRelationshipSyncArgs(UserSpecialRelationshipItem[] items)
        {
            Items = items;
        }
    }

    public class UserRelationshipChangedArgs : EventArgs
    {
        public string AccountId { get; private set; }

        public bool IsSetted { get; private set; }

        public NIMUserRelationshipChangeType ChangedType { get; private set; }

        public UserRelationshipChangedArgs(NIMUserRelationshipChangeType type,string id,bool value)
        {
            ChangedType = type;
            IsSetted = value;
            AccountId = id;
        }
    }
}
