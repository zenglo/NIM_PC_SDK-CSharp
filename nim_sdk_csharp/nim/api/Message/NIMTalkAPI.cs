/** @file NIMTalkAPI.cs
  * @brief NIM SDK提供的talk接口
  * @copyright (c) 2015, NetEase Inc. All rights reserved
  * @author Harrison
  * @date 2015/12/8
  */

using System;

namespace NIM
{
    public delegate void ReportUploadProgressDelegate(long uploadedSize, long totalSize);
    public static class TalkAPI
    {
        private static readonly IMReceiveMessageCallback ReceivedMessageCallback;
        private static readonly IMMessageArcCallback MessageArcCallback;
        private static readonly UploadFileCallback UploadFileProgressChanged;
        /// <summary>
        /// 接收消息事件通知
        /// </summary>
        public static EventHandler<NIMReceiveMessageEventArgs> OnReceiveMessageHandler { get; set; }
        public static EventHandler<MessageArcEventArgs> OnSendMessageCompleted { get; set; }

        static TalkAPI()
        {
            ReceivedMessageCallback = new IMReceiveMessageCallback(OnReceiveIMMessage);
            MessageArcCallback = new IMMessageArcCallback(OnReceiveMessageArc);
            UploadFileProgressChanged = new UploadFileCallback(OnUploadFileProgressChanged);
            TalkNativeMethods.nim_talk_reg_arc_cb("", MessageArcCallback, IntPtr.Zero);
            TalkNativeMethods.nim_talk_reg_receive_cb("", ReceivedMessageCallback, IntPtr.Zero);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息对象</param>
        public static void SendMessage(NIMIMMessage message, ReportUploadProgressDelegate action = null)
        {
            System.Diagnostics.Debug.Assert(message != null && !string.IsNullOrEmpty(message.ReceiverID));
            var msg = message.Serialize();
            IntPtr ptr = IntPtr.Zero;
            if(action != null)
                ptr = NimUtility.DelegateConverter.ConvertToIntPtr(action);
            TalkNativeMethods.nim_talk_send_msg(msg, null, UploadFileProgressChanged, ptr);
        }

        private static void OnUploadFileProgressChanged(long uploadedSize, long totalSize, string jsonExtension, IntPtr userData)
        {
            NimUtility.DelegateConverter.Invoke<ReportUploadProgressDelegate>(userData, uploadedSize, totalSize);
        }

        private static void OnReceiveMessageArc(string arcResult, IntPtr userData)
        {
            if (string.IsNullOrEmpty(arcResult))
                return;
            var arc = MessageArc.Deserialize(arcResult);
            if (OnSendMessageCompleted != null)
            {
                OnSendMessageCompleted(null, new MessageArcEventArgs(arc));
            }
        }

        static void OnReceiveIMMessage(string content, string jsonArcResult, IntPtr userData)
        {
            if (string.IsNullOrEmpty(content) || OnReceiveMessageHandler == null)
                return;
            var obj = Newtonsoft.Json.Linq.JObject.Parse(content);
            NIMReceivedMessage msg = new NIMReceivedMessage();
            var resCode = obj.SelectToken(NIMReceivedMessage.ResCodePath);
            var feature = obj.SelectToken(NIMReceivedMessage.FeaturePath);
            var token = obj.SelectToken(NIMReceivedMessage.MessageContentPath);
            if (resCode != null)
                msg.ResponseCode = resCode.ToObject<ResponseCode>();
            if (feature != null)
                msg.Feature = feature.ToObject<NIMMessageFeature>();

            if (token != null && token.Type == Newtonsoft.Json.Linq.JTokenType.Object)
            {
                var contentObj = token.ToObject<Newtonsoft.Json.Linq.JObject>();
                var realMsg = MessageFactory.CreateMessage(contentObj);
                msg.MessageContent = realMsg;
                OnReceiveMessageHandler(null, new NIMReceiveMessageEventArgs(msg));
            }
        }
    }
}
