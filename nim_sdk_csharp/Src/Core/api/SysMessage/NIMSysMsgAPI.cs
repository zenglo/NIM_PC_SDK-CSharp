/** @file NIMSysMsgAPI.cs
  * @brief NIM SDK提供的系统消息历史接口 
  * @copyright (c) 2015, NetEase Inc. All rights reserved
  * @author Harrison
  * @date 2015/12/8
  */

using System;
using NimUtility;
using NIM.SysMessage.Delegate;

namespace NIM.SysMessage
{
    public delegate void ReceiveSysMsgResult(NIMSysMessag msg);

    public delegate void SendSysMsgResult(MessageAck arc);

    public delegate void QuerySysMsgResult(NIMSysMsgQueryResult result);

    public delegate void CommomOperateResult(ResponseCode response, int count);

    public class SysMsgAPI
    {
        public static EventHandler<MessageArcEventArgs> SendSysMsgHandler;
        public static EventHandler<NIMSysMsgEventArgs> ReceiveSysMsgHandler;

        private static readonly ReceiveSysMsgDelegate OnReceivingSysMsgDelegate = (result, ptr) =>
        {
            NimUtility.NimLogManager.NimCoreLog.InfoFormat("Receive System Message:{0}", result);
            if (ReceiveSysMsgHandler != null)
            {
                NIMSysMsgEventArgs args = null;
                if (!string.IsNullOrEmpty(result))
                {
                    var msg = NIMSysMessag.Deserialize(result);
                    args = new NIMSysMsgEventArgs(msg);
                }
                ReceiveSysMsgHandler(null, args);
            }
        };

        private static readonly CustomSysMsgArcDelegate OnSendMsgCompleted = (result, ptr) =>
        {
            if (SendSysMsgHandler != null)
            {
                MessageArcEventArgs args = null;
                if (!string.IsNullOrEmpty(result))
                {
                    var msg = MessageAck.Deserialize(result);
                    args = new MessageArcEventArgs(msg);
                }
                SendSysMsgHandler(null, args);
            }
        };

        private static readonly QuerySysMsgDelegate OnQuerySysMsgCompleted = (count, result, je, ptr) =>
        {
            if (ptr != IntPtr.Zero)
            {
                var msgs = NIMSysMsgQueryResult.Deserialize(result);
                if (msgs != null)
                    msgs.Count = count;
                ptr.Invoke<QuerySysMsgResult>(msgs);
            }
        };


        private static readonly OperateSysMsgDelegate OnQueryUnreadCompleted = (res, count, je, ptr) => { ptr.InvokeOnce<CommomOperateResult>((ResponseCode)res, count); };

        public static void RegisterCallbacks()
        {
            SysMsgNativeMethods.nim_sysmsg_reg_custom_notification_ack_cb(null, OnSendMsgCompleted, IntPtr.Zero);
            SysMsgNativeMethods.nim_sysmsg_reg_sysmsg_cb(null, OnReceivingSysMsgDelegate, IntPtr.Zero);
        }

        /// <summary>
        /// 发送自定义通知
        /// </summary>
        /// <param name="content"></param>
        public static void SendCustomMessage(NIMSysMessageContent content)
        {
            if (string.IsNullOrEmpty(content.ClientMsgId))
                content.GenerateMsgId();
            var jsonMsg = content.Serialize();
            SysMsgNativeMethods.nim_sysmsg_send_custom_notification(jsonMsg, null);
        }

        /// <summary>
        /// 查询本地系统消息（按时间逆序起查，逆序排列）
        /// </summary>
        /// <param name="limit">一次查询数量，建议20</param>
        /// <param name="lastTimetag">上次查询最后一条消息的时间戳（按时间逆序起查，即最小的时间戳）</param>
        /// <param name="cb">查询本地系统消息的回调函数</param>
        public static void QueryMessage(int limit, long lastTimetag, QuerySysMsgResult cb)
        {
            var ptr = DelegateConverter.ConvertToIntPtr(cb);
            SysMsgNativeMethods.nim_sysmsg_query_msg_async(limit, lastTimetag, null, OnQuerySysMsgCompleted, ptr);
        }

        /// <summary>
        /// 查询未读消息数
        /// </summary>
        /// <param name="cb"></param>
        public static void QueryUnreadCount(CommomOperateResult cb)
        {
            var ptr = DelegateConverter.ConvertToIntPtr(cb);
            SysMsgNativeMethods.nim_sysmsg_query_unread_count(null, OnQueryUnreadCompleted, ptr);
        }

        /// <summary>
        /// 设置消息状态
        /// </summary>
        /// <param name="msgId">消息id</param>
        /// <param name="status">消息状态</param>
        /// <param name="cb">设置消息状态的回调函数，</param>
        public static void SetMsgStatus(long msgId, NIMSysMsgStatus status, OperateSysMsgExternDelegate cb)
        {
            SysMsgNativeMethods.nim_sysmsg_set_status_async(msgId, status, null, cb, IntPtr.Zero);
        }

        /// <summary>
        /// 设置全部消息为已读
        /// </summary>
        /// <param name="cb"></param>
        public static void SetAllMsgRead(OperateSysMsgDelegate cb)
        {
            SysMsgNativeMethods.nim_sysmsg_read_all_async(null, cb, IntPtr.Zero);
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="msgId">消息id</param>
        /// <param name="cb"></param>
        public static void DeleteByMsgId(long msgId, OperateSysMsgExternDelegate cb)
        {
            SysMsgNativeMethods.nim_sysmsg_delete_async(msgId, null, cb, IntPtr.Zero);
        }

        /// <summary>
        /// 全部删除
        /// </summary>
        /// <param name="cb"></param>
        public static void DeleteAll(OperateSysMsgDelegate cb)
        {
            SysMsgNativeMethods.nim_sysmsg_delete_all_async(null, cb, IntPtr.Zero);
        }

        /// <summary>
        /// 按消息类型批量设置消息状态
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <param name="status">消息状态</param>
        /// <param name="cb"></param>
        public static void SetMsgStatusByType(NIMSysMsgType type, NIMSysMsgStatus status, OperateSysMsgDelegate cb)
        {
            SysMsgNativeMethods.nim_sysmsg_set_logs_status_by_type_async(type, status, null, cb, IntPtr.Zero);
        }

        /// <summary>
        /// 按消息类型批量删除消息
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <param name="cb"></param>
        public static void DeleteMsgByType(NIMSysMsgType type, OperateSysMsgDelegate cb)
        {
            SysMsgNativeMethods.nim_sysmsg_delete_logs_by_type_async(type, null, cb, IntPtr.Zero);
        }
    }
}