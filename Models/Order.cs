using System;
using System.Collections.Generic;

namespace Web.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
