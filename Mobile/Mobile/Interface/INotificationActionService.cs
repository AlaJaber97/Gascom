using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Interface
{
    public interface INotificationActionService
    {
        void TriggerAction(string action);
    }
}
