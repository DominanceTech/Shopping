using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UploadDataToDB.Models;
using UploadDataToDB.Repositories;
using UploadDataToDB.ViewModel;
using UploadDataToDB.Controllers;
using System.Web.Security;
using System;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Net.Configuration;
using PagedList;

using PagedList.Mvc;
using System.Text;
using System.IO;
namespace UploadDataToDB.Controllers
{
    public class HomeController : Controller
    {
        private ShoppingCartAppEntities db = new ShoppingCartAppEntities();
        private int? UserId;

        public ActionResult Login()
        {
          //  string Email = Session["Email"].ToString();
            UserDataViewModel model = new UserDataViewModel()
            {
             //Email= Email,
             //Password=Password
            };
            //if (Request.Cookies["Login"] != null)
            //{
            //    model.Email = Request.Cookies["Login"].Values["Email"];
            //    model.Password = Request.Cookies["Login"].Values["Password"];
            //}

            if (Request.Cookies["Login"].Values["Email"] != null && Request.Cookies["Login"].Values["Password"] != null)
            {
                model.Email = Request.Cookies["Login"].Values["Email"];
                model.Password = Request.Cookies["Login"].Values["Password"];
            }
            return View(model);
        }


        [HttpPost,ValidateInput(false)]
        [ValidateAntiForgeryToken]

        public ActionResult Login(UserDataViewModel objUser)
        {
            //UserDataViewModel model = new UserDataViewModel();
            try
            {
                if (ModelState.IsValid)
                {
                    using (ShoppingCartAppEntities db = new ShoppingCartAppEntities())
                    {
                        if (objUser.Email != null && objUser.Password != null)
                        {
                            string rank = db.Users.Where(a => a.Email == objUser.Email).FirstOrDefault().Role;
                            var obj = db.Users.Where(a => a.Email.Equals(objUser.Email) && a.Password.Equals(objUser.Password)).FirstOrDefault();
                            if (obj != null && rank == "Admin")
                            {
                                FormsAuthentication.SetAuthCookie(objUser.Email, objUser.Remember);
                                FormsAuthentication.SetAuthCookie(objUser.Password, objUser.Remember);
                                Session["UserId"] = obj.UserId.ToString();
                                Session["Email"] = obj.Email.ToString();
                                Session["Role"] = obj.Role.ToString();
                               if(objUser.Remember)
                                {
                                    HttpCookie cookie = new HttpCookie("Login");
                                    cookie.Values.Add("Email",obj.Email);
                                    cookie.Values.Add("Password", obj.Password);
                                    cookie.Expires = DateTime.Now.AddDays(15);
                                    Response.Cookies.Add(cookie);
                                }
                                else
                                {
                                    Response.Cookies["Email"].Expires = DateTime.Now.AddDays(-1);
                                    Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);
                                }
                                //Response.Cookies["UserName"].Value = obj.Email.Trim();
                                //Response.Cookies["Password"].Value = obj.Password.Trim();
                                return RedirectToAction("Display");
                            }
                            else if (obj != null && rank == "User")
                            {
                                FormsAuthentication.SetAuthCookie(objUser.Email, objUser.Remember);
                                FormsAuthentication.SetAuthCookie(objUser.Password, objUser.Remember);
                                Session["UserId"] = obj.UserId.ToString();
                                Session["Email"] = obj.Email.ToString();
                                //return RedirectToAction("UserDataDisplay");

                                if (objUser.Remember)
                                {
                                    HttpCookie cookie = new HttpCookie("Login");
                                    cookie.Values.Add("Email", obj.Email);
                                    cookie.Values.Add("Password", obj.Password);
                                    cookie.Expires = DateTime.Now.AddDays(15);
                                    Response.Cookies.Add(cookie);
                                }
                                else
                                {
                                    Response.Cookies["Email"].Expires = DateTime.Now.AddDays(-1);
                                    Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);
                                }
                                Response.Cookies["Email"].Value = obj.Email.Trim();
                                Response.Cookies["Password"].Value = obj.Password.Trim();
                                return RedirectToAction("Display");
                            }
                            else
                            {
                                ViewBag.Message = "Your Password is Wrong";
                                return View("Login");
                            }
                        }

                        else
                        {
                            ViewBag.Message = "Please Fill the UserName and Password.";
                            return View("Login");
                        }
                       
                    }
                }
                return View(objUser);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Please Fill the UserName and Password Correct.";                
                return View("Login");
            }

        }




        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Login");

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }




        [HttpGet]
        public ActionResult Edit(int Id)
        {
            try
            {
                int UserId = Convert.ToInt32(Session["UserId"].ToString());
                string Role = Session["Role"].ToString();
                if (UserId != null)
                {
                   
                        var obj = db.Contents.Where(a => a.ID == Id).FirstOrDefault();
                        ContentViewModel model = new ContentViewModel()
                        {
                            BrandId = obj.BrandId == null ? 0 : obj.BrandId.Value,
                            Name = obj.Name,
                            Description = obj.Description,
                            price = obj.price,
                            Image = obj.Image,
                            Review = obj.Review,
                            CategoryId = obj.CategoryId == null ? 0 : obj.CategoryId.Value
                        };
                        return View(model);
                 
                    //else
                    //{
                    //    var obj = db.userDatas.Where(a => a.UserId == UserId).FirstOrDefault();

                    //}
                    
                }
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login");
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContentViewModel obj)
        {
            try
            {
                using (ShoppingCartAppEntities db = new ShoppingCartAppEntities())
                {
                    HttpPostedFileBase file = Request.Files["ImageData"];
                    ContentRepository service = new ContentRepository();

                    int i = service.AddUpdateConttentData(file, obj); if (i == 1)
                    {
                        return RedirectToAction("Display");
                    }
                    db.SaveChanges();
                    return RedirectToAction("Display");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login");
            }

        }

        //POst
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Content content = db.Contents.Find(id);
            db.Contents.Remove(content);
            db.SaveChanges();
            return RedirectToAction("Display");
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult SearchPartial(string searching)
        {
            ShoppingCartAppEntities db = new ShoppingCartAppEntities();

            //if (option == "BrandId")
            //{

            //    return PartialView("SearchPartial",db.Contents.Where(x => x.BrandId.ToString() == search || search == null).ToList());
            //}
            //else if (option == "CategoryId")
            //{
            //    return PartialView("SearchPartial",db.Contents.Where(x => x.CategoryId.ToString() == search || search == null).ToList());
            //}
            //else
            //{
            //    return PartialView("SearchPartial",db.Contents.Where(x => x.Name.StartsWith(search) || search == null).ToList());
            //}



            // return View("Display", db.Contents.Where(x => x.Name.Contains(searching) || searching == null).ToList());

            return PartialView("SearchPartial");
        }


        [Route("Display")]
        [HttpGet]
        public ActionResult Display(int? page, int? PageSize, string searching/*,int? PageNumber*/)
        {
            try
            {
                List<ContentViewModel> contentModel = db.Contents.Select(item => new ContentViewModel()
                {
                    ID = item.ID,
                    Name = item.Name,
                    price = item.price,
                    Review = item.Review,
                    Image = item.Image,

                    BrandId = item.BrandId == null ? 0 : item.BrandId.Value,
                    CategoryId = item.CategoryId == null ? 0 : item.CategoryId.Value,
                    Description = item.Description,
                    BrandDesc = item.Brand.BrandName,
                    CategoryDesc = item.Category.CategoryName,


                }).ToList();

                ViewBag.PageSize = new List<SelectListItem>()
            {
                new SelectListItem() { Value="5", Text= "5" },
                new SelectListItem() { Value="10", Text= "10" },
                new SelectListItem() { Value="15", Text= "15" },
                new SelectListItem() { Value="25", Text= "25" },
                new SelectListItem() { Value="50", Text= "50" },
            };
                int pageNumber = (page ?? 1);
                int pagesize = (PageSize ?? 5);
                ViewBag.pageNumber = pageNumber;
                ViewBag.psize = pagesize;
                ViewBag.Count = contentModel.Count;

                //ProjectEntities1 db = new ProjectEntities1();

                List<BrandViewModel> brlist = new List<BrandViewModel>();
                brlist.Add(new BrandViewModel { BrandId = 0, BrandName = "ALL" });
                var BrandList = this.db.Brands.Select(b => new BrandViewModel()
                {
                    BrandId = b.BrandId,
                    BrandName = b.BrandName
                }).ToList();
                brlist.AddRange(BrandList);
                ViewBag.Brands = brlist;


                int UserId = System.Convert.ToInt32(Session["UserId"].ToString());
                var count = (from a in db.UserCarts
                             where a.UserId == (UserId)
                             select a.Quantity).Sum();
                ViewBag.cartCount = count;


                //var item1 = new List<SelectListItem>();
                //foreach(var item in CategoryList)
                //{
                //    var SelectedItem = false;
                //    items.Add(new SelectListItem { Selected = SelectedItem, Text = item.CategoryName, Value = item.CategoryId.ToString() });
                //}
                //   ViewData["CategoryList"] = CategoryList;
                List<CategoryViewModel> catList = new List<CategoryViewModel>();
                catList.Add(new CategoryViewModel { CategoryId = 0, CategoryName = "ALL" });

                var CategoryList = this.db.Categories.Select(b => new CategoryViewModel()
                {
                    CategoryId = b.CategoryId,
                    CategoryName = b.CategoryName
                }).ToList();
                catList.AddRange(CategoryList);

                ViewBag.Category = catList;



                // return View(db.Employees.OrderBy(c => c.Id).ToList().ToPagedList(pageNumber, pagesize));
                if (searching != null)
                {

                    //   return View("Display", contentModel.Where(x => x.Name.Contains(searching) || searching == null).ToList().ToPagedList(pageNumber, pagesize));
                    // return View("Display", contentModel.Where(x => string.IsNullOrEmpty(searching) || x.Name.Contains(searching)).ToList().ToPagedList(pageNumber, pagesize));
                    return View("Display", contentModel.Where(x => string.IsNullOrEmpty(searching) || x.Name.IndexOf(searching, System.StringComparison.OrdinalIgnoreCase) >= 0).ToList().ToPagedList(pageNumber, pagesize));

                }
                else
                {
                    return View("Display", contentModel.OrderBy(c => c.ID).ToList().ToPagedList(pageNumber, pagesize));

                }

        }
            catch (Exception ex)
            {
                return RedirectToAction("Login");
               // throw;
            }

        }


        [HttpGet]
        public ActionResult GetCategoriesByBrandId(int BrandId)
        {
            List<CategoryViewModel> catList = new List<CategoryViewModel>();
            catList.Add(new CategoryViewModel { CategoryId = 0, CategoryName = "ALL" });
            if (BrandId == 0)
            {
                //List<CategoryViewModel> catList = new List<CategoryViewModel>();
                //catList.Add(new CategoryViewModel { CategoryId = 0, CategoryName = "ALL" });


                var CategoryList = this.db.Categories.Select(b => new CategoryViewModel()
                {
                    CategoryId = b.CategoryId,
                    CategoryName = b.CategoryName
                }).ToList();
                catList.AddRange(CategoryList);

                ViewBag.Category = catList;
            }
            else
            {
                var CategoryList = (from t1 in db.BrandCategoryMappings
                                    join t2 in db.Categories
                                    on t1.CategoryId equals t2.CategoryId
                                    where t1.BrandId == BrandId
                                    select new CategoryViewModel
                                    {
                                        //t2.CategoryId,
                                        //t2.CategoryName,
                                        CategoryId = t2.CategoryId,
                                        CategoryName = t2.CategoryName,
                                    }).ToList();


                if (CategoryList != null && CategoryList.Count > 0)
                    catList.AddRange(CategoryList);

                ViewBag.Category = catList;
                //    ViewBag.Category = CategoryList;
            }
            return Json(ViewBag.Category, JsonRequestBehavior.AllowGet);

        }


        [Route("DisplayPartial")]
        [HttpGet]
        public ActionResult DisplayPartial(SearchViewModel searchViewModel)
        {
            try
            {
                List<ContentViewModel> contentModel = db.Contents.Select(item => new ContentViewModel()
                {
                    ID = item.ID,
                    Name = item.Name,
                    price = item.price,
                    Review = item.Review,
                    Image = item.Image,

                    BrandId = item.BrandId == null ? 0 : item.BrandId.Value,
                    CategoryId = item.CategoryId == null ? 0 : item.CategoryId.Value,
                    Description = item.Description,
                    BrandDesc = item.Brand.BrandName,
                    CategoryDesc = item.Category.CategoryName,


                }).ToList();

                ViewBag.PageSize = new List<SelectListItem>()
            {
                new SelectListItem() { Value="5", Text= "5" },
                new SelectListItem() { Value="10", Text= "10" },
                new SelectListItem() { Value="15", Text= "15" },
                new SelectListItem() { Value="25", Text= "25" },
                new SelectListItem() { Value="50", Text= "50" },
            };

                int pageNumber = (searchViewModel.Page ?? 1);
                int pagesize = (searchViewModel.PageSize ?? 5);
                // int PageCount=(searchViewModel.PageCount ??)

                ViewBag.pageNumber = pageNumber;
                ViewBag.psize = pagesize;
                ViewBag.Count = contentModel.Count;
                int UserId = System.Convert.ToInt32(Session["UserId"].ToString());
                var count = (from a in db.UserCarts
                             where a.UserId == (UserId)
                             select a.Quantity).Sum();
                ViewBag.cartCount = count;

                List<ContentViewModel> result = new List<ContentViewModel>();

                //result = contentModel.Where(x => (string.IsNullOrEmpty(searchViewModel.SearchText) || (x.Name.ToLower().Contains(searchViewModel.SearchText.ToLower())))
                //                                && (searchViewModel.BrandId == 0 || x.BrandId == searchViewModel.BrandId)
                //                                && (searchViewModel.CategoryId == 0 || x.CategoryId == searchViewModel.CategoryId)
                //                                ).ToList().ToPagedList(pageNumber, pagesize).ToList();
                //result= contentModel.Where(x => (string.IsNullOrEmpty(searchViewModel.SearchText) || (x.Name.ToLower().Contains(searchViewModel.SearchText.ToLower())))
                //                              && (searchViewModel.BrandId == 0 || x.BrandId == searchViewModel.BrandId)
                //                              && (searchViewModel.CategoryId == 0 || x.CategoryId == searchViewModel.CategoryId)
                //                              ).ToList().ToPagedList(pageNumber, pagesize).ToList();

                //return View("DisplayPartial", result);


                return View("DisplayPartial", contentModel.Where(x => (string.IsNullOrEmpty(searchViewModel.SearchText) || (x.Name.ToLower().Contains(searchViewModel.SearchText.ToLower())))
                                              && (searchViewModel.BrandId == 0 || x.BrandId == searchViewModel.BrandId)
                                              && (searchViewModel.CategoryId == 0 || x.CategoryId == searchViewModel.CategoryId)
                                              ).ToList().ToPagedList(pageNumber, pagesize));
            }
            catch (Exception ex)
            {
                //return RedirectToAction("Login");
                throw;
            }

        }



        public ActionResult RetriveId(int id)
        {
            byte[] cover = GetImageFromDataBase(id);
            if (cover != null)
            {

                return File(cover, "image/jpeg");

            }
            else
            {
                return null;
            }

        }

        public byte[] GetImageFromDataBase(int id)
        {
            var q = from temp in db.Contents where temp.ID == id select temp.Image;
            byte[] cover = q.First();
            return cover;

        }

        ////[ChildActionOnly]
        ////public ActionResult SearchPartial()
        ////{
        ////    ProjectEntities1 db = new ProjectEntities1();

        ////    var BrandList = this.db.Brands.ToList();
        ////    var items = new List<SelectListItem>();
        ////    foreach (var item in BrandList)
        ////    {
        ////        var selectedItem = false;
        ////        items.Add(new SelectListItem { Selected = selectedItem, Text = item.BrandName, Value = item.BrandId.ToString() });
        ////    }
        ////    ViewData["BrandList"] = BrandList;
        ////    ViewData["CategoryList"] = items;
        ////    //return this.PartialView("BrandLayout", items);
        ////    return View();


        ////}




        [HttpGet]
        public ActionResult Create()
        {
            try
            {
                int UserId = Convert.ToInt32(Session["UserId"].ToString());
                if (UserId != null)
                {
                    ShoppingCartAppEntities db = new ShoppingCartAppEntities();
                    ViewBag.BrandId = new SelectList(db.Brands.OrderBy(x => x.BrandName), "BrandId", "BrandName");

                    ViewBag.CateGoryId = new SelectList(db.Categories.OrderBy(x => x.CategoryName), "CategoryId", "CategoryName");
                    return View();
                }
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login");
            }

        }

        [HttpPost]
        public ActionResult Create(ContentViewModel model)
        {
            try
            {

                int UserId = Convert.ToInt32(Session["UserId"].ToString());
                if (UserId != null)
                {
                    HttpPostedFileBase file = Request.Files["ImageData"];
                    ContentRepository service = new ContentRepository();
                    int i = service.AddUpdateConttentData(file, model);
                    if (i == 1)
                    {
                        return RedirectToAction("Display");
                    }
                    return View(model);
                }
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login");
            }


        }
        [HttpGet]
        public ActionResult AddUser()
        {
            try
            {
                int UserId = Convert.ToInt32(Session["UserId"].ToString());
                if (UserId != null)
                {

                    return View();
                }
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login");
            }


        }
        [HttpPost]
        public ActionResult AddUser(User model)
        {

            using (ShoppingCartAppEntities entities = new ShoppingCartAppEntities())
            {
                entities.Users.Add(model);
                entities.SaveChanges();
                int UserId = model.UserId;
                //return RedirectToAction("Display");
                ViewBag.result = " Registration Successful!";

            }
            SendActivationEmail(model);
            return View(model);

        }


        [HttpGet]
        public ActionResult ProductDetails(int? Id)
        {
            try
            {
                int UserId = System.Convert.ToInt32(Session["UserId"].ToString());
                if (Id != null)
                {
                    Content obj = db.Contents.Single(X => X.ID == Id.Value);

                    var count = (from a in db.UserCarts
                                 where a.UserId == (UserId)
                                 select a.Quantity).Sum();
                    ViewBag.cartCount = count;

                    return View(obj);
                }
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login");
            }

        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ProductDetails(Content obj)
        //{

        //    using (ProjectEntities1 db = new ProjectEntities1())
        //    {
        //        Content contentDetails = (from c in db.Contents where c.ID == obj.ID select c).FirstOrDefault();

        //        contentDetails.Name = obj.Name;
        //        contentDetails.price = obj.price;
        //        contentDetails.Description = obj.Description;
        //        contentDetails.Image = obj.Image == null ? contentDetails.Image : obj.Image;
        //        contentDetails.price = obj.price;
        //        contentDetails.Review = obj.Review;

        //        return View("ProductDetails");
        //    }


        //}

        private void SendActivationEmail(User user)
        {

            ShoppingCartAppEntities projectEntities = new ShoppingCartAppEntities();
            SmtpSection secObj = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            using (MailMessage mm = new MailMessage("jyotisonwane5@gmail.com", user.Email))
            {
                if (Session["Role"] != null)
                {
                    mm.Subject = "Account Activation";
                    string body = "Hello " + user.FirstName + ",";

                    body = "Your Account has been created in Our shoppingCart. Your credentials are: ";
                    body += "<br /><br /> UserId :" + user.Email + ", ";
                    body += "<br /> Password :" + user.Password + ",";

                    body += "<br /><br />Thanks";
                    mm.Body = body;
                }
                else
                {
                    mm.Subject = "Order status";
                    string body = "Hello " + user.FirstName + ",";

                    body = "Your Order has been Placed Successfully: ";

                    body += "<br /><br />Thanks";
                    mm.Body = body;
                }

                  
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                // smtp.Host = "smtp.gmail.com";

                // NetworkCredential NetworkCred = new NetworkCredential("jyotisonwane5@gmail.com", "padmin1432");
                smtp.Host = secObj.Network.Host;

                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("jyotisonwane5@gmail.com", secObj.Network.Password);
                smtp.ServicePoint.MaxIdleTime = 1;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }



        }


        [HttpGet]
        public ActionResult Add(int ProductId, int UserId)
        {
            ShoppingCartAppEntities db = new ShoppingCartAppEntities();
            int Quantity;
            CartViewModel cm = new CartViewModel();
            // var roductId = db.UserCarts.AsNoTracking().Where(p => p.ProductId == ProductId).FirstOrDefault();
            // var roductId = db.UserCarts.Find(ProductId);
            //   var roductId = from u in db.UserCarts.Where(p => p.ProductId == ProductId).Select(v => v.ProductId) select ProductId;
            // var data = db.UserCarts.Count(a => a.ProductId == ProductId);
            var data = (from a in db.UserCarts
                        where a.UserId == UserId && a.ProductId == ProductId
                        select a.ProductId).Count();
            var data_user = db.UserCarts.Count(a => a.UserId == UserId);
            if (data_user == 0)
            {
                Quantity = 1;
                UserCart c = new UserCart()
                {
                    UserId = UserId,
                    ProductId = ProductId,
                    DateTime = System.DateTime.Now,
                    Quantity = Quantity,

                };

                db.UserCarts.Add(c);
                db.SaveChanges();


            }
            else
            {
                if (data == 0)
                {

                    Quantity = 1;
                    UserCart c = new UserCart()
                    {
                        UserId = UserId,
                        ProductId = ProductId,
                        DateTime = System.DateTime.Now,
                        Quantity = Quantity,

                    };

                    db.UserCarts.Add(c);
                    db.SaveChanges();
                }
                else
                {
                    var abc = (from a in db.UserCarts
                               where a.ProductId == (ProductId) && a.UserId == (UserId)
                               select a.Quantity).Single();

                    int pqr = System.Convert.ToInt32(abc);
                    pqr++;

                    var result = db.UserCarts.SingleOrDefault(b => b.ProductId == ProductId && b.UserId == UserId);
                    result.ProductId = ProductId;
                    result.Quantity = pqr;
                    result.UserId = UserId;
                    result.DateTime = System.DateTime.Now;

                    db.SaveChanges();
                }

            }


            var count = (from a in db.UserCarts
                         where a.UserId == (UserId)
                         select a.Quantity).Sum();
            ViewBag.cartCount = count;


            //  TempData["ProductAddedToCart"] = "Product added to cart successfully";
            //  return RedirectToAction("Display", ViewBag.cartCount);
            return RedirectToAction("DisplayPartial", ViewBag.cartCount);

        }
        [HttpGet]
        public ActionResult MyorderPartial()
        {
            try
            {
                ShoppingCartAppEntities db = new ShoppingCartAppEntities();
                var sum = 0;
                var Price = 0;
                int UserId = System.Convert.ToInt32(Session["UserId"].ToString());
                //var Quantity =from a in db.UserCarts
                //              where a.UserId== UserId
                //              select a;
                var item = (from a in db.UserCarts
                            join b in db.Contents
                            on a.ProductId equals b.ID
                            where a.UserId == UserId
                            select new ContentViewModel
                            {
                                ID = b.ID,
                                Name = b.Name,
                                Image = b.Image,  
                                Quantity = a.Quantity == null ? 0 : a.Quantity.Value,
                                price = b.price,
                            }).ToList();

                foreach (var totalsum in item)
                {
                    //sum = sum + System.Convert.ToInt32(totalsum.price);
                    //  sum = sum - ((totalsum.price * totalsum.Offer) / 100);
                    Price = System.Convert.ToInt32(totalsum.price) * System.Convert.ToInt32(totalsum.Quantity);
                    sum = sum + /*System.Convert.ToInt32(totalsum.price)*/ Price;
                    
                }
                ViewBag.Total = sum;
                //String.Format("{0:n}", ViewBag.Total);
                ViewBag.Price = Price;
                var count = (from a in db.UserCarts
                             where a.UserId == (UserId)
                             select a.Quantity).Sum();
                ViewBag.cartCount = count;
                return View("Myorder", item);

                // return View((List<Content>)Session["cart"]);

            }
            catch (Exception ex)
            {
                return RedirectToAction("Login");
            }
        }


        [HttpGet]
        public ActionResult AddQuantity(int ProductId, int UserId)
        {
            ShoppingCartAppEntities db = new ShoppingCartAppEntities();
            int Quantity;
            CartViewModel cm = new CartViewModel();

            var data = (from a in db.UserCarts
                        where a.UserId == UserId && a.ProductId == ProductId
                        select a.ProductId).Count();
            var data_user = db.UserCarts.Count(a => a.UserId == UserId);
            if (data_user == 0)
            {
                Quantity = 1;
                UserCart c = new UserCart()
                {
                    UserId = UserId,
                    ProductId = ProductId,
                    DateTime = System.DateTime.Now,
                    Quantity = Quantity,
                   
                };

                db.UserCarts.Add(c);
                db.SaveChanges();


            }
            else
            {
                if (data == 0)
                {

                    Quantity = 1;
                    UserCart c = new UserCart()
                    {
                        UserId = UserId,
                        ProductId = ProductId,
                        DateTime = System.DateTime.Now,
                        Quantity = Quantity,

                    };

                    db.UserCarts.Add(c);
                    db.SaveChanges();
                }
                else
                {
                    var abc = (from a in db.UserCarts
                               where a.ProductId == (ProductId) && a.UserId == (UserId)
                               select a.Quantity).Single();

                    int QuantityData = System.Convert.ToInt32(abc);
                    QuantityData++;

                    var result = db.UserCarts.SingleOrDefault(b => b.ProductId == ProductId && b.UserId == UserId);
                    result.ProductId = ProductId;
                    result.Quantity = QuantityData;
                    result.UserId = UserId;
                    result.DateTime = System.DateTime.Now;

                    db.SaveChanges();
                }

            }


            var count = (from a in db.UserCarts
                         where a.UserId == (UserId)
                         select a.Quantity).Sum();
            ViewBag.cartCount = count;


            return Redirect("MyorderPartial");

        }

        [HttpGet]
        public ActionResult Remove(int UserId, int ProductId)
        {
            var userCart = db.UserCarts.FirstOrDefault(s => s.ProductId == ProductId && s.UserId == UserId);


            int Quantity;
            CartViewModel cm = new CartViewModel();


            var abc = (from a in db.UserCarts
                       where a.ProductId == (ProductId) && a.UserId == (UserId)
                       select a.Quantity).Single();

            int QuantityData = System.Convert.ToInt32(abc);

            if (abc == 0)
            {
                db.UserCarts.Remove(userCart);
                db.SaveChanges();
            }
            else
            {
                QuantityData--;

                if (QuantityData == 0)
                {
                    db.UserCarts.Remove(userCart);
                    db.SaveChanges();
                }
                else
                {
                    var result = db.UserCarts.SingleOrDefault(b => b.ProductId == ProductId && b.UserId == UserId);
                    result.ProductId = ProductId;
                    result.Quantity = QuantityData;
                    result.UserId = UserId;
                    result.DateTime = System.DateTime.Now;

                    db.SaveChanges();
                }

            }

            return Redirect("MyorderPartial");
        }

        [HttpGet]
        public ActionResult RemoveALL(int UserId, int ProductId)
        {
            var userCart = db.UserCarts.FirstOrDefault(s => s.ProductId == ProductId && s.UserId == UserId);
            if (userCart != null)
            {
                db.UserCarts.Remove(userCart);
                db.SaveChanges();
            }

            return Redirect("MyorderPartial");
        }

        [HttpGet]
        public ActionResult OrderConfirmation(int UserId)
        {
            try
            {
                List<OrderViewModel> order = new List<OrderViewModel>();
                //int price = Convert.ToInt32(content.price);
                DateTime billgeneratedate = DateTime.Now;

                var OrderData = (from a in db.UserCarts
                                 join b in db.Contents
                                 on a.ProductId equals (b.ID)
                                 where a.UserId == (UserId)
                                 select new OrderViewModel
                                 {
                                     UserId = a.UserId == null ? 0 : a.UserId.Value,
                                     ProductId = a.ProductId == null ? 0 : a.ProductId.Value,
                                     Quantity = a.Quantity == null ? 0 : a.Quantity.Value,
                                     Price = b.price,
                                    // // Quantity = Convert.ToInt32(a.Quantity),
                                     //Price = Int32.Parse(b.price.ToString()),

                                     // DateTime = a.DateTime
                                     DateTime = billgeneratedate
                                 }).ToList();

                var orderRec = db.UserOrders.OrderByDescending(o => o.UserOrderId).FirstOrDefault();
                string orderNumber = "";
          
                if (orderRec == null)
                {
                    orderNumber = "SC-BL-01";
                    //orderNumber = (orderNumber).Substring(6, 2);

                }

                else
                {
                    //int oldOrderNo = Convert.ToInt32(orderRec.OrderId);
                    //      int newOrdernumber = (oldOrderNo)++;
                    // orderNumber = (orderNumber).Substring(6, 2);
                    string oldOrderNo = orderRec.OrderId;

                    orderNumber = (oldOrderNo).Substring(6, 2);
                    int newOrdernumber = Convert.ToInt32(orderNumber);
                    newOrdernumber= ++(newOrdernumber);
                    // orderNumber = "SC-BL-02";
         
                    orderNumber = (newOrdernumber).ToString();
                    orderNumber = "SC-BL-0"+ orderNumber;
                  // orderNumber = "SC-BL- "'+orderNumber+'" ";

                }
            
              //   SerialNo = orderRec.SerialNo;
                List < UserOrder > orderList = new List<UserOrder>();
                foreach (var item in OrderData)
                {
                 
                    orderList.Add(new UserOrder()
                    {
                        UserId = item.UserId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = (item.Price)* (item.Quantity),
                        Date = item.DateTime,
                       OrderId = orderNumber,
                       
                    });
                }

                db.UserOrders.AddRange(orderList);
           
          
                db.SaveChanges();

                var all = from c in db.UserCarts
                          where c.UserId == UserId
                          select c;
                db.UserCarts.RemoveRange(all);
                db.SaveChanges();


                var Email = Session["Email"].ToString();
                User userdata = new User()
                {
                    UserId = UserId,
                    Email = Email,

                };

                SendActivationEmail(userdata);
            }
            catch (Exception ex)
            {
                throw;
            }
            //return RedirectToAction("MyorderPartial");
            return RedirectToAction("CheckOrders");
        }

        [HttpGet]
        public ActionResult OrderClick(string OrderId)
        {
            ShoppingCartAppEntities db = new ShoppingCartAppEntities();
            var sum = 0;
            int Price = 0;
            int UserId = System.Convert.ToInt32(Session["UserId"].ToString());

            var item = (from a in db.UserOrders
                        join b in db.Contents
                        on a.ProductId equals b.ID
                        where a.UserId == UserId && a.OrderId==OrderId
                        select new OrderViewModel
                        {
                            ProductId = b.ID,
                            Name = b.Name,
                            BrandDesc=b.Brand.BrandName,
                            Image = b.Image,
                            Price = b.price,
                            TotalPrice = a.Price,
                            OrderId=a.OrderId,
                            Quantity = a.Quantity == null ? 0 : a.Quantity.Value,
                            DateTime = a.Date,
                            OrderStatus=a.OrderStatus,
                        }).ToList();

            foreach (var totalsum in item)
            {
                Price = System.Convert.ToInt32(totalsum.Price) * System.Convert.ToInt32(totalsum.Quantity);
                sum = sum + /*System.Convert.ToInt32(totalsum.price)*/ Price;
            }
            ViewBag.Total = sum;
            var count = (from a in db.UserCarts
                         where a.UserId == (UserId)
                         select a.Quantity).Sum();
            ViewBag.cartCount = count;
           
          
            return View("OrderClick",item);
        }

        [HttpGet]
        public ActionResult CheckOrders()
        {
            ShoppingCartAppEntities db = new ShoppingCartAppEntities();
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            var item = db.UserOrders.GroupBy(r =>
             new
             {
                 r.OrderId,
                 r.Date,
                 r.UserId,
                 r.OrderStatus,

             }).Select(r => new OrderViewModel()
             {
                 DateTime = r.Key.Date,
                 OrderId = r.Key.OrderId,
                 UserId= r.Key.UserId,
                 OrderStatus=r.Key.OrderStatus,
                 Quantity=r.Sum(a=>a.Quantity),
                 Price = r.Sum(a => a.Price)
             }).ToList()
              .Where(g => g.UserId == UserId);


            //foreach (var itemdata in item )
            //{
            // var data = from a in db.UserOrders
            //               where a.UserId == UserId && a.OrderId == itemdata.OrderId
            //               select a.OrderStatus;


            //}
            //ViewBag.data = data;
            var count = (from a in db.UserCarts
                         where a.UserId == (UserId)
                         select a.Quantity).Sum();
            ViewBag.cartCount = count;


            return View("CheckOrders",item);


        }
        [HttpGet]
        public ActionResult UserProfile()
        {
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            var data = db.Users.FirstOrDefault(a => a.UserId == UserId);
            return View("UserProfile",data);
        }
        [HttpGet]
        public ActionResult EditYourProfile(int UserId)
        {
            //int UserId = Convert.ToInt32(Session["UserId"].ToString());
            var obj = db.Users.Where(a => a.UserId == UserId).FirstOrDefault();
            UserDataViewModel model = new UserDataViewModel()
            {
             
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Gender = obj.Gender,
                Email = obj.Email,
                Password = obj.Password,
                PhoneNo = obj.PhoneNo == null ? 0 : obj.PhoneNo.Value,
                Address = obj.Address,
                BirthDate = obj.BirthDate,
              
            };

            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditYourProfile(UserDataViewModel obj)
        {
            using (ShoppingCartAppEntities db = new ShoppingCartAppEntities())
            {
               
                ContentRepository service = new ContentRepository();

                int i = service.AddUpdateUserData(obj); if (i == 1)
                {
                    return RedirectToAction("UserProfile");
                }
                db.SaveChanges();
                return RedirectToAction("UserProfile");
            }
        }

    }
}