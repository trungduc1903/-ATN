using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopDGHouse.Models
{
    public class CustomerViewModel
    {
        public string CustumerName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int PayMent { get; set; }
        public int UserId { get; set; }
    }
}