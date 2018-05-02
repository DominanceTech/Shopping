using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UploadDataToDB.Models;
using System.ComponentModel.DataAnnotations;

namespace UploadDataToDB.ViewModel
{
    public class UserDataViewModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public Nullable<long> PhoneNo { get; set; }
        public string Address { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Role { get; set; }

        [Display(Name="Remember Me?")]
        public bool Remember { get; set;}

    }
}