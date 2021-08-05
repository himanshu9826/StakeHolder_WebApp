using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace StakeHolder.UI.API.App.Helper
{
    public class APIFileHelper
    {

        /// <summary>
        /// Return the uploaded file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Return the uploaded extention of file
        /// </summary>
        public string FileExt { get; set; }
        /// <summary>
        /// Return the uploaded file byte array
        /// </summary>
        public byte[] FileBytes { get; set; }

        /// <summary>
        /// Return the thumbnail byte array of uploaded image (if upload file type is image)
        /// </summary>
        public byte[] ThumbFileBytes { get; set; }
        /// <summary>
        /// Return the uploaded file length
        /// </summary>
        public int FileLength { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public APIFileHelper()
        {

        }

        /// <summary>
        /// Return required properties of file
        /// </summary>
        /// <param name="postedFile">HttpPostedFileBase object</param>
        public APIFileHelper(MultipartFileData postedFile, string localImagePath = "")
        {

            if (postedFile != null && !string.IsNullOrEmpty(postedFile.Headers.ContentDisposition.FileName))
            {

                string siteURL = ConfigurationManager.AppSettings["BaseURL"].ToString();
                string fileName = postedFile.Headers.ContentDisposition.FileName;

                if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                {
                    fileName = fileName.Trim('"');
                }
                if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                {
                    fileName = Path.GetFileName(fileName);
                }
                FileName = Guid.NewGuid() + "_" + fileName;

                File.Move(postedFile.LocalFileName, Path.Combine(localImagePath, FileName));
                var webClient = new WebClient();
                FileBytes = webClient.DownloadData(siteURL + "Content/UploadedImage/" + FileName);
                ThumbFileBytes = GetThumbnail(Path.Combine(localImagePath, FileName));
                File.Delete(Path.Combine(localImagePath, FileName));
            }

        }
        
        /// <summary>
        /// Method to convert posted file to byte array
        /// </summary>
        /// <param name="postedFile">HttpPostedFileBase</param>
        /// <param name="LocalImageFile">string</param>
        /// <returns>byte[]</returns>
        private byte[] GetThumbnail(string filePath)
        {
            string[] extentions = { ".png", ".jpg", ".jpeg", ".gif", ".tif" };
            byte[] thumbByteArray = null;
            if (!string.IsNullOrEmpty(filePath))
            {
                Image img = Image.FromFile(filePath);
                int imgHeight = 300;
                int imgWidth = 300;
                if (img.Width < img.Height)
                {
                    //portrait image  
                    imgHeight = 300;
                    var imgRatio = (float)imgHeight / (float)img.Height;
                    imgWidth = Convert.ToInt32(img.Height * imgRatio);
                }
                else if (img.Height < img.Width)
                {
                    //landscape image  
                    imgWidth = 300;
                    var imgRatio = (float)imgWidth / (float)img.Width;
                    imgHeight = Convert.ToInt32(img.Height * imgRatio);
                }
                Image thumb = img.GetThumbnailImage(imgWidth, imgHeight, () => false, IntPtr.Zero);

                var ms = new MemoryStream();
                thumb.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg); // Use appropriate format here
                thumbByteArray = ms.ToArray();
                img.Dispose();
            }
            return thumbByteArray;
        }
    }
}