using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Training.ViewModels
{
    public class District
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Regency { get; set; }
        public string Province { get; set; }
    }
    public class DistrictData : DataTableResult
    {
        public List<District> data { get; set; }
    }
    public class Village
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string District { get; set; }
        public string Regency { get; set; }
        public string Province { get; set; }
    }
    public class VillageData : DataTableResult
    {
        public List<Village> data { get; set; }
    }
}