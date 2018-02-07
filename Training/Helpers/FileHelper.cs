using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Training.Helpers
{
    public static class FileHelper
    {
        public static string UploadImage(HttpPostedFileBase file)
        {
            var path = "/Content/images/uploads/";
            if (file == null || file.ContentLength == 0)
            {
                return null;
            }
            try
            {
                var fileName = Path.GetFileName(file.FileName);
                var resultPath = Path.Combine(HttpContext.Current.Server.MapPath(path), fileName);
                file.SaveAs(resultPath);
                return path + fileName;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
            }
            return null;
        }
    }
}