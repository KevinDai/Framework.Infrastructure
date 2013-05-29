using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.MessageBus.RabbitMQ
{
    public class SubscriptionAction
    {
        public SubscriptionAction(bool isSingleUse)
        {
            IsSingleUse = isSingleUse;
            ClearAction();
        }

        public void ClearAction()
        {
            Action = () => { };
        }

        public Action Action { get; set; }
        public bool IsSingleUse { get; private set; }
        public bool IsMultiUse { get { return !IsSingleUse; } }
    }
}
