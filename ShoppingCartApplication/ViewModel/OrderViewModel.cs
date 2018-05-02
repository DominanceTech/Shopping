using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UploadDataToDB.ViewModel
{
    public class OrderViewModel
    {
        public int SerialNo { get; set; }
        public string OrderId { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public int? Quantity { get; set; }
        public string BrandDesc { get; set; }
        public byte[] Image { get; set; }
        public string OrderStatus { get; set; }
        //public int Price { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal ? Price { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal? TotalPrice { get; set; }
        public DateTime? DateTime { get; set; }
        
    }
}