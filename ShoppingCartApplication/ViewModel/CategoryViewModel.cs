using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UploadDataToDB.ViewModel
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int BrandId { get; set; }
        public string CategoryDesc { get; set; }
    }
}