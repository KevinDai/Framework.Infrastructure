using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.MessageBus
{
    public interface IMessageBusProvider
    {
        /// <summary>
        /// 获取默认的消息总线对象
        /// </summary>
        /// <returns></returns>
        IMessageBus GetMessageBus();

        /// <summary>
        /// 获取指定名称的消息总线对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IMessageBus GetMessageBus(string name);
    }
}
