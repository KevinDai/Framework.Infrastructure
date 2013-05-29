using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Framework.Infrastructure.MessageBus
{
    public class MessageProperties
    {
        public MessageProperties()
        {
            Headers = new Hashtable();
        }

        private bool contentTypePresent = false;
        private bool contentEncodingPresent = false;
        private bool headersPresent = false;
        private bool deliveryModePresent = false;
        private bool priorityPresent = false;
        private bool correlationIdPresent = false;
        private bool replyToPresent = false;
        private bool expirationPresent = false;
        private bool messageIdPresent = false;
        private bool timestampPresent = false;
        private bool typePresent = false;
        private bool userIdPresent = false;
        private bool appIdPresent = false;
        private bool clusterIdPresent = false;

        private string contentType;

        /// <summary>
        /// MIME内容格式
        /// </summary>
        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; contentTypePresent = true; }
        }

        private string contentEncoding;

        /// <summary>
        /// MIME内容编码
        /// </summary>
        public string ContentEncoding
        {
            get { return contentEncoding; }
            set { contentEncoding = value; contentEncodingPresent = true; }
        }

        private IDictionary headers;

        /// <summary>
        /// 消息头信息字典
        /// </summary>
        public IDictionary Headers
        {
            get { return headers; }
            set { headers = value; headersPresent = true; }
        }

        private byte deliveryMode;

        /// <summary>
        /// 是否持久化：1-否，2-是 
        /// </summary>
        public byte DeliveryMode
        {
            get { return deliveryMode; }
            set { deliveryMode = value; deliveryModePresent = true; }
        }

        private byte priority;

        /// <summary>
        /// 消息优先级 0 至 9 
        /// </summary>
        public byte Priority
        {
            get { return priority; }
            set { priority = value; priorityPresent = true; }
        }

        private string correlationId;

        /// <summary>
        /// 应用相关的Id
        /// </summary>
        public string CorrelationId
        {
            get { return correlationId; }
            set { correlationId = value; correlationIdPresent = true; }
        }

        private string replyTo;

        /// <summary>
        /// 回复地址
        /// </summary>
        public string ReplyTo
        {
            get { return replyTo; }
            set { replyTo = value; replyToPresent = true; }
        }

        private string expiration;

        /// <summary>
        /// 消息过期时间 
        /// </summary>
        public string Expiration
        {
            get { return expiration; }
            set { expiration = value; expirationPresent = true; }
        }

        private string messageId;

        /// <summary>
        /// 消息Id 
        /// </summary>
        public string MessageId
        {
            get { return messageId; }
            set { messageId = value; messageIdPresent = true; }
        }

        private long timestamp;

        /// <summary>
        /// 发送时间
        /// </summary>
        public long Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; timestampPresent = true; }
        }

        private string type;

        /// <summary>
        /// 消息类型
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; typePresent = true; }
        }

        private string userId;

        /// <summary>
        /// 消息发送人
        /// </summary>
        public string UserId
        {
            get { return userId; }
            set { userId = value; userIdPresent = true; }
        }

        private string appId;

        /// <summary>
        /// 消息发送应用Id
        /// </summary>
        public string AppId
        {
            get { return appId; }
            set { appId = value; appIdPresent = true; }
        }

        private string clusterId;

        /// <summary>
        /// 消息总线集群内路由标识
        /// </summary>
        public string ClusterId
        {
            get { return clusterId; }
            set { clusterId = value; clusterIdPresent = true; }
        }

        public bool IsHeadersPresent()
        {
            return headersPresent;
        }

        public bool IsContentTypePresent()
        {
            return contentTypePresent;
        }

        public bool IsContentEncodingPresent()
        {
            return contentEncodingPresent;
        }

        public bool IsDeliveryModePresent()
        {
            return deliveryModePresent;
        }

        public bool IsPriorityPresent()
        {
            return priorityPresent;
        }

        public bool IsCorrelationIdPresent()
        {
            return correlationIdPresent;
        }

        public bool IsExpirationPresent()
        {
            return expirationPresent;
        }

        public bool IsMessageIdPresent()
        {
            return messageIdPresent;
        }

        public bool IsTimestampPresent()
        {
            return timestampPresent;
        }

        public bool IsTypePresent()
        {
            return typePresent;
        }

        public bool IsUserIdPresent()
        {
            return userIdPresent;
        }

        public bool IsAppIdPresent()
        {
            return appIdPresent;
        }

        public bool IsReplyToPresent()
        {
            return replyToPresent;
        }

        public bool IsClusterIdPresent()
        {
            return clusterIdPresent;
        }
    }
}
