using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Settings
{
    public static class Configration
    {
        public static readonly BLL.Enums.DevServer Server = BLL.Enums.DevServer.Local;
        public static string ApiServerAddress
        {
            get
            {
                return Server switch
                {
                    Enums.DevServer.Local => "http://192.168.0.103:5000",
                    Enums.DevServer.Publish => "https://gascom.azurewebsites.net",
                    _ => throw new NotImplementedException(),
                };
            }
        }
        public static string HubServerAddress => $"{ApiServerAddress}/gascom-signalr";
    }
}