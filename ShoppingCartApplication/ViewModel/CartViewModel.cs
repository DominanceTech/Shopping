using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UploadDataToDB.Models;

namespace UploadDataToDB.ViewModel
{
    public class CartViewModel
    {
        [Key]
        public int IdentityKey { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime DateTime { get; set; }
       // public UserData UserId { get; set; }
    }
}