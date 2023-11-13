using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBillApp.Models
{
    public class BillDetail
    {
        public int Id { get; set; }
        public string CustomerName{ get; set; }
        public string MobileNumber { get; set; }
        public string CustomerAddress { get; set; }
        public int TotalAmount { get; set; }
        public List<Items> Items { get; set; }

        public BillDetail()
        {
            Items = new List<Items>();
        }
    }
}