using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class Countries : Dictionary<string, City>
    {
        //public string Name { get; set; }
        //public List<Region> Cities { get; set; }
    }
    public class City : Dictionary<string, Region>
    {
        //public string Name { get; set; }
        //public List<Region> Regions { get; set; }
    }
    public class Region : Dictionary<string, District>
    {
        //public string Name { get; set; }
        //public List<District> Regions { get; set; }
    }
    public class District
    {
        public string Name { get; set; }
    }
}
