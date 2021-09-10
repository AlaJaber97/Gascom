using BLL.Enums;
using Mobile.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobile.Services
{
    public class PushNotificationActionService : IPushNotificationActionService
    {
        readonly Dictionary<string, PushNotificationAction> _actionMappings = new Dictionary<string, PushNotificationAction>
        {
            { "update_status_user", PushNotificationAction.UpdateUser },
            { "add_new_user", PushNotificationAction.UpdateUser }
        };

        public event EventHandler<PushNotificationAction> ActionTriggered = delegate { };
        public event EventHandler<string> MessageRecived = delegate { };

        public void TriggerAction(string action)
        {
            if (!_actionMappings.TryGetValue(action, out var pushDemoAction))
                return;

            List<Exception> exceptions = new List<Exception>();

            foreach (var handler in ActionTriggered?.GetInvocationList())
            {
                try
                {
                    handler.DynamicInvoke(this, pushDemoAction);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
                throw new AggregateException(exceptions);
        }
    }
}
