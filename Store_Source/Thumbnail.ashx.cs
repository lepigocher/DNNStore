/*
'  DNN (formerly DotNetNuke) - http://www.dnnsoftware.com
'  Copyright (c) 2002-2018
'  by DNN Corp
' 
'  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
'  documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
'  the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
'  to permit persons to whom the Software is furnished to do so, subject to the following conditions:
' 
'  The above copyright notice and this permission notice shall be included in all copies or substantial portions 
'  of the Software.
' 
'  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
'  TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
'  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
'  CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
'  DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.Log.EventLog;

using DotNetNuke.Modules.Store.Core.Catalog;

namespace DotNetNuke.Modules.Store.WebControls
{
    public class Thumbnail : IHttpHandler
    {
        private enum ImageTypeName
        {
            URI,
            FilePath
        }

        #region IHttpHandler Members

        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            Image image;
            ImageFormat imageFormat;
            string contentType = null;
            MemoryStream memStream = null;
            ImageCache imageCache;
            bool enableCaching = true;
            bool cacheValid = false;
            int minutes = Null.NullInteger;
            string cacheKey = null;
            string eTag = null;
            // Get params
            string imagePath = context.Request.QueryString["IP"];
            string thumbWidth = context.Request.QueryString["IW"];
            string background = context.Request.QueryString["BC"];
            string duration = context.Request.QueryString["CD"];
            // Get Header Etag
            string headerEtag = context.Request.Headers["If-None-Match"];
            // When image param start by http or https,
            // it's probably an external image!
            ImageTypeName imageType;
            if (imagePath.IndexOf("http://") > -1 || imagePath.IndexOf("https://") > -1)
                // Image will be get from an http request
                imageType = ImageTypeName.URI;
            else
            {
                // Image will be read from disk
                imageType = ImageTypeName.FilePath;
                imagePath = context.Server.MapPath(imagePath);
            }
            // Default image if not found
            string defaultImage = context.Server.MapPath("~/images/thumbnail.jpg");

            try
            {
                if (string.IsNullOrEmpty(imagePath) == false)
                {
                    // Initializations
                    if (string.IsNullOrEmpty(thumbWidth))
                        thumbWidth = "175";
                    if (string.IsNullOrEmpty(duration))
                        enableCaching = false;
                    else
                        minutes = Convert.ToInt32(duration);
                    // Get cached image thumbnail
                    if (enableCaching)
                    {
                        cacheKey = string.Format("Store_{0}_{1}_{2}", imagePath, thumbWidth, background);
                        eTag = cacheKey.GetHashCode().ToString("X");
                        imageCache = (ImageCache)DataCache.GetCache(cacheKey);
                        if (imageCache != null)
                        {
                            contentType = imageCache.ContentType;
                            memStream = new MemoryStream(imageCache.Data);
                            cacheValid = true;
                        }
                    }
                    // If cache is null, create the thumbnail
                    if (!cacheValid)
                    {
                        image = GetImage(imageType, imagePath, defaultImage);
                        imageFormat = image.RawFormat;
                        contentType = GetContentType(imageFormat);
                        Size thumbSize = ThumbSize(image.Width, image.Height, Convert.ToInt16(thumbWidth));
                        Bitmap thumb = new Bitmap(thumbSize.Width, thumbSize.Height);
                        Graphics g = Graphics.FromImage(thumb);
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        // Fill background with specified color
                        // to mimics original GIF transparency
                        if (imageFormat.Equals(ImageFormat.Gif))
                        {
                            Brush bgBrush = GetBrush(GetHexDigits(background));
                            g.FillRectangle(bgBrush, 0, 0, thumbSize.Width, thumbSize.Height);
                        }
                        g.DrawImage(image, 0, 0, thumbSize.Width, thumbSize.Height);
                        // Create memory stream from thumbnail
                        memStream = new MemoryStream();
                        thumb.Save(memStream, imageFormat);
                        image.Dispose();
                        thumb.Dispose();
                        // Save thumbnail to cache
                        if (enableCaching)
                        {
                            imageCache = new ImageCache(contentType, memStream.ToArray());
                            TimeSpan sliding = new TimeSpan(0, Convert.ToInt32(duration), 0);
                            DataCache.SetCache(cacheKey, imageCache, sliding);
                        }
                    }
                    // If an Etag is present and valid
                    if (string.IsNullOrEmpty(headerEtag) == false && headerEtag == eTag)
                    {
                        // Return 304 Not Modified
                        context.Response.Clear();
                        context.Response.StatusCode = (int)HttpStatusCode.NotModified;
                        context.Response.SuppressContent = true;
                    }
                    else
                    {
                        // Return thumbnail to browser
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        context.Response.ContentType = contentType;
                        if (enableCaching)
                        {
                            HttpCachePolicy contextCache = context.Response.Cache;
                            contextCache.SetCacheability(HttpCacheability.ServerAndPrivate);
                            contextCache.SetExpires(DateTime.Now.AddMinutes(minutes).ToUniversalTime());
                            contextCache.SetLastModified(DateTime.Now.ToUniversalTime());
                            contextCache.SetETag(eTag);
                        }
                        memStream.WriteTo(context.Response.OutputStream);
                    }
                    memStream.Close();
                    memStream.Dispose();
                }
            }
            catch
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Retrieves an Image from a URI.  URI's external to the project
        /// are restricted because DNN's default trust level is Medium.
        /// In this case the thumnail.jpg shipped with DNN is displayed.
        /// 
        /// NOTE: 
        /// Changing the trust level to Full will allow external web requests.
        /// Ex. &lt;trust level="Full" originUrl=""&gt;
        /// 
        /// see http://msdn2.microsoft.com/en-US/library/tkscy493(VS.80).aspx
        /// </summary>
        /// <returns>Image reference to the loaded image</returns>
        private static Image GetImage(ImageTypeName imageType, string imagePath, string defaultImage)
        {
            Image imageResult = null;
            try
            {
                if (imageType == ImageTypeName.URI)
                {
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(imagePath);
                    webRequest.Credentials = CredentialCache.DefaultCredentials;
                    HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                    using(Stream responseStream = webResponse.GetResponseStream())
                    {
                        if(responseStream != null)
                        {
                            imageResult = Image.FromStream(responseStream);
                            webResponse.Close();
                            responseStream.Close();
                        }
                        else
                            webResponse.Close();
                    }
                }
                else
                {
                    string filePath = imagePath;
                    imageResult = Image.FromFile(filePath);
                }
            }
            catch (Exception ex)
            {
                PortalSettings settings = PortalController.Instance.GetCurrentPortalSettings();
                EventLogController controler = new EventLogController();
                string message = "Exception: " + ex.GetType().Name + " - " + ex.Message;
                if (ex.InnerException != null)
                    message += " - Inner Exception: " + ex.InnerException.GetType().Name + " - " + ex.InnerException.Message;
                controler.AddLog("Store Thumbnail Error", message, settings, -1, EventLogController.EventLogType.ADMIN_ALERT);
                imageResult = Image.FromFile(defaultImage);
            }
            return imageResult;
        }

        private static Size ThumbSize(int currentWidth, int currentHeight, int newWidth)
        {
            double iMultiplier;

            if (currentWidth > newWidth)
                iMultiplier = Convert.ToDouble(newWidth) / currentWidth;
            else
                iMultiplier = 1;

            return new Size(Convert.ToInt16(currentWidth * iMultiplier), Convert.ToInt16(currentHeight * iMultiplier));
        }

        private static string GetContentType(ImageFormat imageFormat)
        {
            if (imageFormat.Equals(ImageFormat.Bmp))
            {
                return "image/x-ms-bmp";
            }
            if (imageFormat.Equals(ImageFormat.Gif))
            {
                return "image/gif";
            }
            if (imageFormat.Equals(ImageFormat.Jpeg))
            {
                return "image/jpeg";
            }
            if (imageFormat.Equals(ImageFormat.Png))
            {
                return "image/png";
            }
            if (imageFormat.Equals(ImageFormat.Tiff))
            {
                return "image/tiff";
            }

            return "image/jpeg";
        }

        public static string GetHexDigits(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                // Remove any characters that are not hex digits like #
                return Regex.Replace(input, "[^\\da-fA-F]", "");
            }

            return null;
        }

        private static Brush GetBrush(string background)
        {
            // Default to White background
            Brush bgBrush = Brushes.White;

            if (string.IsNullOrEmpty(background) == false)
            {
                string r;
                string g;
                string b;

                // Get RBG values from Hex background color
                switch (background.Length)
                {
                    case 3:
                        // Short value e.g. FFF
                        r = string.Format("{0}{0}", background.Substring(0, 1));
                        g = string.Format("{0}{0}", background.Substring(1, 1));
                        b = string.Format("{0}{0}", background.Substring(2, 1));
                        break;
                    case 6:
                        // Long value e.g. FFFFFF
                        r = background.Substring(0, 2);
                        g = background.Substring(2, 2);
                        b = background.Substring(4, 2);
                        break;
                    default:
                        // Default to White background
                        r = "FF";
                        g = "FF";
                        b = "FF";
                        break;
                }

                try
                {
                    // Try to convert hex to int
                    int ri = Int32.Parse(r, System.Globalization.NumberStyles.HexNumber);
                    int gi = Int32.Parse(g, System.Globalization.NumberStyles.HexNumber);
                    int bi = Int32.Parse(b, System.Globalization.NumberStyles.HexNumber);
                    // Get the corresponding brush
                    bgBrush = new SolidBrush(Color.FromArgb(ri, gi, bi));
                }
                catch
                {
                    // Default to White background
                }
            }

            return bgBrush;
        }

        #endregion
    }
}
