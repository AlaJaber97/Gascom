using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Interface
{
    public interface IPushNotificationActionService : INotificationActionService
    {
        event EventHandler<BLL.Enums.PushNotificationAction> ActionTriggered;
    }
}
