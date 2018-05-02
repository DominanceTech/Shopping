using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UploadDataToDB.Repositories;
using UploadDataToDB.ViewModel;
using UploadDataToDB.Models;
using System.IO;

using System.Xml.Linq;
namespace UploadDataToDB.Repositories
{
    public class ContentRepository
    {
        private readonly ShoppingCartAppEntities db = new ShoppingCartAppEntities();
        //public int UploadImageInDataBase(HttpPostedFileBase file, ContentViewModel contentViewModel)
        //{
        //    contentViewModel.Image = ConvertToBytes(file);
        //    var Content = new Content
        //    {
        //        Name = contentViewModel.Name,
        //        BrandId=contentViewModel.BrandId,
        //        CategoryId=contentViewModel.CategoryId,
        //        price = contentViewModel.price,
        //        Review = contentViewModel.Review,
        //        Description = contentViewModel.Description,
        //        Image = contentViewModel.Image


        //    };
        //    db.Contents.Add(Content);

        //    int i = db.SaveChanges();
        //    if (i == 1)
        //    {
        //        return 1;
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        public int AddUpdateUserData(UserDataViewModel userdataviewmodel)
        {

            if (userdataviewmodel.UserId == 0)
            {
                //User UserData= new User()
                var UserData = new User
                {
                    FirstName = userdataviewmodel.FirstName,
                    LastName = userdataviewmodel.LastName,
                    Gender = userdataviewmodel.Gender,
                    Email = userdataviewmodel.Email,
                    Password = userdataviewmodel.Password,
                    PhoneNo = userdataviewmodel.PhoneNo,
                    Address = userdataviewmodel.Address,
                    BirthDate = userdataviewmodel.BirthDate
                };
                db.Users.Add(UserData);
            }
            else
            {
                User updateData = (from a in db.Users
                                   where a.UserId == userdataviewmodel.UserId
                                       select a).FirstOrDefault();

                updateData.FirstName = userdataviewmodel.FirstName;
                updateData.LastName = userdataviewmodel.LastName;
                updateData.Gender = userdataviewmodel.Gender;
                updateData.Email = userdataviewmodel.Email;
                updateData.Password = userdataviewmodel.Password;
                updateData.PhoneNo = userdataviewmodel.PhoneNo;
                updateData.Address = userdataviewmodel.Address;
                updateData.BirthDate = userdataviewmodel.BirthDate;

            }
            int i = db.SaveChanges();
            if (i == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }

        public int AddUpdateConttentData(HttpPostedFileBase file, ContentViewModel contentViewModel)
        {
            byte[] imageData = ConvertToBytes(file);
            if (contentViewModel.ID == 0)
            {
                var Content = new Content
                {
                    Name = contentViewModel.Name,
                    BrandId = contentViewModel.BrandId, 
                    CategoryId=contentViewModel.CategoryId,
                       price = contentViewModel.price,
                    Review = contentViewModel.Review,
                    Description = contentViewModel.Description,
                    Image = imageData
                };
                db.Contents.Add(Content);
            }
            else
            {

                Content updateContent = (from c in db.Contents where c.ID == contentViewModel.ID select c).FirstOrDefault();
                updateContent.Name = contentViewModel.Name;
                updateContent.price = contentViewModel.price;
                updateContent.BrandId = contentViewModel.BrandId;
                updateContent.CategoryId = contentViewModel.CategoryId;
                updateContent.Description = contentViewModel.Description;
                updateContent.price = contentViewModel.price;
                updateContent.Review = contentViewModel.Review;
                updateContent.Image = imageData == null ? updateContent.Image : imageData;

            } int i = db.SaveChanges();
            if (i == 1)
            {
                return 1;
            }
            else {
                return 0;
            }
        }

   
    public byte[] ConvertToBytes(HttpPostedFileBase image)
    {
        if (image == null || image.ContentLength == 0)
            return null;
        byte[] imageBytes = null;
        BinaryReader reader = new BinaryReader(image.InputStream);
        imageBytes = reader.ReadBytes((int)image.ContentLength);
        return imageBytes;
    }
}
}
