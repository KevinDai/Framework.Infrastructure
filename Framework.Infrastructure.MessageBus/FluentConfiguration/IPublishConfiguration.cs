using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.MessageBus.FluentConfiguration
{
    public interface IPublishConfiguration
    {
        IPublishConfiguration OnSuccess(Action successCallback);

        IPublishConfiguration OnFailure(Action failureCallback);
    }

    public class AdvancedPublishConfiguration : IPublishConfiguration
    {
        public Action SuccessCallback { get; private set; }
        public Action FailureCallback { get; private set; }

        public IPublishConfiguration OnSuccess(Action successCallback)
        {
            this.SuccessCallback = successCallback;
            return this;
        }

        public IPublishConfiguration OnFailure(Action failureCallback)
        {
            this.FailureCallback = failureCallback;
            return this;
        }
    }
}
