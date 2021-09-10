using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Configration
{
    public static class FirebaseConfigration
    {
        public static string DatabaseURL => "https://gas-project-5d680-default-rtdb.firebaseio.com/";
        public static string StorageURL => "gs://gas-project-5d680.appspot.com/";
        public static string AuthKey { get; set; }
        public static string ServerKey => "AAAAYQwozww:APA91bFZ_DCWMyZkFiwABAPLE8PEhP38NDHL2_tkt_UJVKZooulmTo7ihwB_kL9fWQDzYp57i5fTWqMld_n135E1yUZijxB_WG_X4kjklyjc2lG4p1hJMviraFt8VktJu-tsTSfyYeF8";
        public static string ApiKey => "AIzaSyBlIQH-XD-9PDp7uwdcKhdZgCWrVljLMP0";
    }
}
