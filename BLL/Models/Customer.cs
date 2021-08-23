using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BLL.Models
{
    public class Customer
    {
        public string PhoneNumber { get; set; }

        [JsonPropertyName("battary_percentage")]
        public double Battary_Percentage { get; set; }

        [JsonPropertyName("cylinder_weigth")]
        public double Cylinder_Weigth { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("irrlevant")]
        public string Irrlevant { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("url_location")]
        public string Url_Location { get; set; }

        [JsonPropertyName("weight_percentage")]
        public double Weight_Percentage { get; set; }
    }
}
