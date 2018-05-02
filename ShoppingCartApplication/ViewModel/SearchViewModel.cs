using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.ComponentModel.DataAnnotations;

namespace UploadDataToDB.ViewModel
{
    public class SearchViewModel
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string SearchText { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
    }
}