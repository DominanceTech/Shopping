using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UploadDataToDB.Models;
using UploadDataToDB.Repositories;
using UploadDataToDB.ViewModel;
using UploadDataToDB.Controllers;
namespace UploadDataToDB.Controllers
{
    [RoutePrefix("Content")]
    [ValidateInput(false)]
    public class ContentController : Controller
    {

        // GET: Content

        
        public ActionResult Index()
        {
            return View();
        }
        

       //private DBContext db = new DBContext();
       // [Route("Display")]
       // [HttpGet]
       // public ActionResult Display()
       // {
       //     var content = db.Contents.Select(s => new
       //     {
       //         s.ID,
       //         s.Name,
       //         s.price,
       //         s.Description,
       //         s.Image,
       //     });

       //     List<ContentViewModel> contentModel = content.Select(item => new ContentViewModel()
       //     {
       //        ID=item.ID,
       //         Name = item.Name,
       //         Price = item.price,
       //         Description = item.Description,
       //         Image = item.Image,



       //     }).ToList();
       //     return View(contentModel);
          
           
       // }

       // public ActionResult RetriveId(int id)
       // {
       //     byte[] cover = GetImageFromDataBase(id);
       //     if(cover!=null)
       //     {

       //         return File(cover,"image/jpeg");

       //     }
       //     else
       //     {
       //         return null;
       //     }

       // }

       // public byte[] GetImageFromDataBase(int id)
       // {
       //     var q = from temp in db.Contents where temp.ID == id select temp.Image;
       //     byte[] cover = q.First();
       //     return cover;

       // }
       // [HttpGet]
       // public ActionResult Create()
       // {
       //     return View();

       // }

       // [HttpPost]
       // public ActionResult Create(ContentViewModel model)
       // {
       //     HttpPostedFileBase file = Request.Files["ImageData"];
       //     ContentRepository service = new ContentRepository();
       //     int i = service.UploadImageInDataBase(file, model);
       //     if (i == 1)
       //     {
       //         return RedirectToAction("Display");
       //     }
       //     return View(model);

       // }

       
    }
}