using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entities;

namespace WebUI.Models
{
    public class SessionAndOrderDetails
    {
        public Cart Cart { get; set; }
        public OrderDetails OrderDetails { get; set; }
    }
}