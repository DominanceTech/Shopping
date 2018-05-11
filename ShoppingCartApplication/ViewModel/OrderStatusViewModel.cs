using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UploadDataToDB.ViewModel
{
    public class OrderStatusViewModel
    {
        public int OrderStatusId { get; set; }
        public string OrderStatus { get; set; }
        //public IEnumerable<SelectListItem> OrderStatusval { get; set; }
    }
}