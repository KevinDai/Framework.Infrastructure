using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using System.Collections;

namespace Framework.Infrastructure.MessageBus.RabbitMQ
{

    public static class MessagePropertiesExtension
    {

        public static void CopyTo(this IBasicProperties basicProperties, MessageProperties messageProperties)
        {
            if (basicProperties.IsContentTypePresent()) messageProperties.ContentType = basicProperties.ContentType;
            if (basicProperties.IsContentEncodingPresent()) messageProperties.ContentEncoding = basicProperties.ContentEncoding;
            if (basicProperties.IsDeliveryModePresent()) messageProperties.DeliveryMode = basicProperties.DeliveryMode;
            if (basicProperties.IsPriorityPresent()) messageProperties.Priority = basicProperties.Priority;
            if (basicProperties.IsCorrelationIdPresent()) messageProperties.CorrelationId = basicProperties.CorrelationId;
            if (basicProperties.IsReplyToPresent()) messageProperties.ReplyTo = basicProperties.ReplyTo;
            if (basicProperties.IsExpirationPresent()) messageProperties.Expiration = basicProperties.Expiration;
            if (basicProperties.IsMessageIdPresent()) messageProperties.MessageId = basicProperties.MessageId;
            if (basicProperties.IsTimestampPresent()) messageProperties.Timestamp = basicProperties.Timestamp.UnixTime;
            if (basicProperties.IsTypePresent()) messageProperties.Type = basicProperties.Type;
            if (basicProperties.IsUserIdPresent()) messageProperties.UserId = basicProperties.UserId;
            if (basicProperties.IsAppIdPresent()) messageProperties.AppId = basicProperties.AppId;
            if (basicProperties.IsClusterIdPresent()) messageProperties.ClusterId = basicProperties.ClusterId;

            if (basicProperties.IsHeadersPresent())
            {
                foreach (DictionaryEntry header in basicProperties.Headers)
                {
                    messageProperties.Headers.Add(header.Key, header.Value);
                }
            }
        }

        public static void CopyTo(this MessageProperties messageProperties, IBasicProperties basicProperties)
        {
            if (messageProperties.IsContentTypePresent()) basicProperties.ContentType = messageProperties.ContentType;
            if (messageProperties.IsContentEncodingPresent()) basicProperties.ContentEncoding = messageProperties.ContentEncoding;
            if (messageProperties.IsDeliveryModePresent()) basicProperties.DeliveryMode = messageProperties.DeliveryMode;
            if (messageProperties.IsPriorityPresent()) basicProperties.Priority = messageProperties.Priority;
            if (messageProperties.IsCorrelationIdPresent()) basicProperties.CorrelationId = messageProperties.CorrelationId;
            if (messageProperties.IsReplyToPresent()) basicProperties.ReplyTo = messageProperties.ReplyTo;
            if (messageProperties.IsExpirationPresent()) basicProperties.Expiration = messageProperties.Expiration;
            if (messageProperties.IsMessageIdPresent()) basicProperties.MessageId = messageProperties.MessageId;
            if (messageProperties.IsTimestampPresent()) basicProperties.Timestamp = new AmqpTimestamp(messageProperties.Timestamp);
            if (messageProperties.IsTypePresent()) basicProperties.Type = messageProperties.Type;
            if (messageProperties.IsUserIdPresent()) basicProperties.UserId = messageProperties.UserId;
            if (messageProperties.IsAppIdPresent()) basicProperties.AppId = messageProperties.AppId;
            if (messageProperties.IsClusterIdPresent()) basicProperties.ClusterId = messageProperties.ClusterId;

            if (messageProperties.IsHeadersPresent())
            {
                basicProperties.Headers = new Hashtable(messageProperties.Headers);
            }
        }
    }

}
