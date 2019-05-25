using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrabalhoDM106.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string customerEmail { get; set; }

        public DateTime orderDate { get; set; }

        public DateTime? deliverDate { get; set; }

        public string status { get; set; }

        public double totalPrice { get; set; }

        public double totalWeight { get; set; }

        public double deliverPrice { get; set; }

        public virtual List<OrderItem> orderItems { get; set; }

    }
}