using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Achar_SaleCenter.Classes
{
    public class UploadManager
    {
        public static Image SaveImageWithNewSize(Image img, string imagePath, string fileName, string path, int width, int height)
        {
            var fullFileNameRoot = HttpContext.Current.Server.MapPath(path + fileName);
            var fullFileNameRelative = HttpContext.Current.Server.MapPath(path + imagePath + "/" + fileName);
            using (img)
            {
                using (var newImage = ScaleImage(img, width, height))
                {
                    //if (!File.Exists(fullFileNameRelative))
                    //{
                    //    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                    //    Encoder myEncoder = Encoder.Quality;
                    //    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    //    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                    //    myEncoderParameters.Param[0] = myEncoderParameter;
                    //    newImage.Save(fullFileNameRelative, jpgEncoder, myEncoderParameters);
                    //}
                    return newImage;
                }
            }
        }
        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Max(ratioX, ratioY);

            var newWidth = Math.Min((int)(image.Width * ratio), image.Width);
            var newHeight = Math.Min((int)(image.Height * ratio), image.Height);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            }


            return newImage;
        }
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public static byte[] GetImagesByte(Image newImage, long quality,string extension)
        {
            byte[] arr; 
            var ext= GetImageFormat(extension);
            ImageCodecInfo jpgEncoder = GetEncoder(ext);
            Encoder myEncoder = Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, quality);
            myEncoderParameters.Param[0] = myEncoderParameter;
            using (MemoryStream ms = new MemoryStream())
            {
                newImage.Save(ms, jpgEncoder, myEncoderParameters);
                arr = ms.ToArray();
            }

            return arr;
        }

        public static ImageFormat GetImageFormat(string extention)
        {
            switch (extention.ToLower())
            {
                case ".png":
                    return ImageFormat.Png; 
                default:
                    return ImageFormat.Jpeg;
            }
        }
        public static string GetResponseFormat(string extention)
        {
            switch (extention.ToLower())
            {
                case ".png":
                    return "image/png"; 
                default:
                    return "image/jpg";
            }
        }
    }
}
