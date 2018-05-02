using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace UploadDataToDB.ViewModel
{
    public class PaginationModel
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }
    }

    public class ContentViewModel
    {
        [Key]

        [Required]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DisplayFormat(DataFormatString ="{0:N2}")]
        public decimal price { get; set; }
        [Required]
        public byte[] Image { get; set; }
        [Required]
        public string Review { get; set; }
        [Required]
        public int BrandId { get; set; }
        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Category { get; set; }

        public string CategoryDesc { get; set; }

        public string BrandDesc { get; set; }
        public int Quantity { get; set; }
    }
    
    public class ResultModel
    {
        public string PartialView { get; set; }

        public string Pagination { get; set; }
    }
}