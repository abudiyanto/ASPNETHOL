using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Training.Models
{
    [Table("Provinces")]
    public class Province
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
    }
    [Table("Regencies")]
    public class Regency
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }

        public Province Province { get; set; }
    }
    [Table("Districts")]
    public class District
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }

        public Regency Regency { get; set; }
    }
    [Table("Villages")]
    public class Village
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }

        public District District { get; set; }
    }

}