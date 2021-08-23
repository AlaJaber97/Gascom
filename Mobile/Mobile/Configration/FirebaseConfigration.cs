using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Configration
{
    public static class FirebaseConfigration
    {
        public static string DatabaseURL => "https://gas-project-5d680-default-rtdb.firebaseio.com/";
        public static string StorageURL => "gs://gas-project-5d680.appspot.com/";
        public static string AuthKey { get; set; }
        public static string ApiKey => "AIzaSyBlIQH-XD-9PDp7uwdcKhdZgCWrVljLMP0";
    }
}
