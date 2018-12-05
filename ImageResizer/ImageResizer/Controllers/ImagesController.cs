using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Achar_SaleCenter.Classes;

namespace ImageResizer.Controllers
{
    public class ImagesController : Controller
    {
        // GET: Images
        public string FolderRoute = Convert.ToString(WebConfigurationManager.AppSettings["folderRoute"]);
        // GET: Imagesontroller
        public ActionResult Resize(string folderName, string imageName, int width, int height,long quality)
        {
            string contentType = "";
            byte[] arr;
            string path = FolderRoute + "\\" + folderName + "\\" + imageName;
            var extension = Path.GetExtension(imageName);
            //var path =Server.MapPath(imageSend.ImagePath + imageSend.FileName);

            Image img = Image.FromFile(path);
            var newImage = UploadManager.ScaleImage(img, width, height);

            arr = UploadManager.GetImagesByte(newImage,quality,extension);


            contentType +=UploadManager.GetResponseFormat(extension);
            Response.ContentType = contentType;
            Response.OutputStream.Write(arr, 0, arr.Length);
            Response.Flush();
            return View();
        }
    }
}