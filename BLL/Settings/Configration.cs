using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Settings
{
    public static class Configration
    {
        public static readonly BLL.Enums.DevServer Server = BLL.Enums.DevServer.Local;
        public static string ServerAddress
        {
            get
            {
                return Server switch
                {
                    Enums.DevServer.Local => "http://192.168.0.199:5000",
                    Enums.DevServer.Publish => throw new NotImplementedException(),
                    _ => throw new NotImplementedException(),
                };
            }
        }
    }
}