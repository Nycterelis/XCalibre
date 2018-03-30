using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace XCalibre.Models.Helpers
{
    public class ImageUploadValidator
    {
        public static bool IsWebFriendlyImage(HttpPostedFileBase file)
        {
            if (file == null)
                return false;

            if (file.ContentLength > 1024 * 1024 * 1024 || file.ContentLength < 1024)
                return false;
            string fileExt = Path.GetExtension(file.FileName).ToLower();
            if (fileExt == ".pdf" || fileExt == ".doc" || fileExt == ".docx" || fileExt == "xls" || fileExt == ".jpg" || fileExt == ".png" || fileExt == ".gif" || fileExt == ".bmp")
            {
                return true;
            }
            else
            {
                return false;
            }
            //try
            //{
            //    using (var img = Image.FromStream(file.InputStream))
            //    {
            //        return ImageFormat.Jpeg.Equals(img.RawFormat) ||
            //               ImageFormat.Png.Equals(img.RawFormat) ||
            //               ImageFormat.Gif.Equals(img.RawFormat);
            //    }
            //}
            //catch
            //{
            //    return false;
            //}
        }
    }
}