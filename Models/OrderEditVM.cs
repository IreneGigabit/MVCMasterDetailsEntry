using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCMasterDetailsEntry.Models
{
    public class OrderEditVM
    {
        public Order Order { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
    }
}