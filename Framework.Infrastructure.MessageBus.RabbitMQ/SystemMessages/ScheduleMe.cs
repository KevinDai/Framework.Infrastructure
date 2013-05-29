using System;

namespace Framework.Infrastructure.MessageBus.RabbitMQ.SystemMessages
{
    [Serializable]
    public class ScheduleMe
    {
        public DateTime WakeTime { get; set; }
        public string BindingKey { get; set; }
        public byte[] InnerMessage { get; set; }
    }
}