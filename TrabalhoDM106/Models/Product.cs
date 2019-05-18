using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TrabalhoDM106.Models
{
    public class Product
    {

        public int Id { get; set; }

        [Required]
        public string name { get; set; }

        public string desc { get; set; }

        public string color { get; set; }

        [Required]
        public string model { get; set; }

        [Required]
        public string cod { get; set; }

        public double price { get; set; }

        public double weight { get; set; }

        public double height { get; set; }

        public double width { get; set; }

        public double length { get; set; }

        public double diameter { get; set; }

        public string url { get; set; }




    }
}