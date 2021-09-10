using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Utils
{
    public static class Diagnostic
    {
        public static void Log(Exception error, string Message = null)
        {
            IDictionary<string, string> propreties = null;
            if (!string.IsNullOrEmpty(Message)) propreties = new Dictionary<string, string>() { { "Message", Message } };
            Crashes.TrackError(error, propreties);
        }
    }
}
