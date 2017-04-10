/*
'  DNN (formerly DotNetNuke) - http://www.dnnsoftware.com
'  Copyright (c) 2002-2016
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

using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.FileSystem;

namespace DotNetNuke.Modules.Store.Components
{
    public sealed class FileSystemHelper
    {
        #region "Private Members"

        private static readonly FileController FileControler = new FileController();

        #endregion

        #region Public Methods

        public static int GetFileID(string filePath, PortalSettings settings)
        {
            return FileControler.ConvertFilePathToFileId(filePath.Replace(settings.HomeDirectory, ""), settings.PortalId);
        }

        public static string GetUrlFileID(string filePath, PortalSettings settings)
        {
            return "FileID=" + GetFileID(filePath, settings);
        }

        public static string GetFilePath(int fileID, PortalSettings settings)
        {
            FileInfo fileInfo = FileControler.GetFileById(fileID, settings.PortalId);
            if (fileInfo != null)
                return settings.HomeDirectory + fileInfo.Folder + fileInfo.FileName;

            return null;
        }

        public static string GetFilePath(string fileID, PortalSettings settings)
        {
            if (fileID.StartsWith("FileID="))
            {
                int intFileID = int.Parse(fileID.Substring(7));
                return GetFilePath(intFileID, settings);
            }
            return null;
        }

        #endregion
    }
}
