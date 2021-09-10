using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BLL.Models
{
    public class Customers
    {
        public List<BLL.Models.Customer> ListOfCustomer { get; set; }
    }
    public class Customer
    {

        [JsonProperty("ID")]
        public string ID { get; set; }

        [JsonProperty("City")]
        public string City { get; set; }

        [JsonProperty("Countrey")]
        public string Countrey { get; set; }

        [JsonProperty("Cylinder-Weight")]
        public decimal CylinderWeight { get; set; }

        [JsonProperty("Weight Percentage")]
        public decimal WeightPercentage { get; set; }

        [JsonProperty("Date")]
        public string Date { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Irrlevant")]
        public string Irrlevant { get; set; }

        [JsonProperty("Latitude")]
        public string LatitudeStr { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public double Latitude => double.TryParse(LatitudeStr, out double Lat) ? Lat : 0;

        [JsonProperty("Longtitude")]
        public string LongtitudeStr { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public double Longtitude => double.TryParse(LongtitudeStr, out double Long) ? Long : 0;

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Neighborhood")]
        public string Neighborhood { get; set; }

        [JsonProperty("Order_State")]
        public int OrderState { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("Phone")]
        public string Phone { get; set; }

        [JsonProperty("Precinct")]
        public string Precinct { get; set; }

        [JsonProperty("Time")]
        public string Time { get; set; }

        [JsonProperty("Url Loation")]
        public string UrlLoation { get; set; }

        [JsonProperty("Booked_At")]
        public string BookedAtStr { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public DateTime BookedAt;
        [Newtonsoft.Json.JsonIgnore]
        public bool IsBooked => DateTime.TryParse(BookedAtStr, out BookedAt);

        [JsonProperty("Booked_By")]
        public string BookedBy { get; set; }
    }
}
