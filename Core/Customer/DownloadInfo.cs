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

using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;

namespace DotNetNuke.Modules.Store.Core.Customer
{
    /// <summary>
    /// Download class info.
    /// </summary>
    [Serializable]
    public sealed class DownloadInfo : IHydratable, IEquatable<DownloadInfo>
    {
        #region Private Members

        private int _orderDetailID;
        private int _orderID;
        private int _productID;
        private string _modelNumber;
        private string _modelName;
        private int _virtualFileID;
        private int _allowedDownloads;
        private int _quantity;
        private int _downloads;

        #endregion

        #region Properties

        public int OrderDetailID 
        {
            get { return _orderDetailID; }
            set { _orderDetailID = value; }
        }

        public int OrderID 
        {
          get { return _orderID; }
          set { _orderID = value; }
        }

        public int ProductID
        {
            get { return _productID; }
            set { _productID = value; }
        }

        public string ModelNumber 
        {
          get { return _modelNumber; }
          set { _modelNumber = value; }
        }

        public string ModelName 
        {
          get { return _modelName; }
          set { _modelName = value; }
        }

        public string ProductTitle
        {
            get
            {
                string title = _modelNumber.Trim();
                title += title == string.Empty ? _modelName.Trim() : " - " + _modelName.Trim();
                return title;
            }
        }

        public int VirtualFileID
        {
            get { return _virtualFileID; }
            set { _virtualFileID = value; }
        }

        public int AllowedDownloads
        {
            get { return _allowedDownloads; }
            set { _allowedDownloads = value; }
        }

        public int Quantity 
        {
          get { return _quantity; }
          set { _quantity = value; }
        }

        public int Downloads 
        {
            get { return _downloads; }
            set { _downloads = value; }
        }

        #endregion

        #region Object Overrides

        public override int GetHashCode() 
        {
            return _orderID.GetHashCode() ^ _productID.GetHashCode();
        }

        #endregion

        #region IHydratable Members

        public void Fill(System.Data.IDataReader dr)
        {
            _orderDetailID = Convert.ToInt32(dr["OrderDetailID"]);
            _orderID = Convert.ToInt32(dr["OrderID"]);
            _productID = Convert.ToInt32(dr["ProductID"]);
            _modelNumber = Convert.ToString(Null.SetNull(dr["ModelNumber"], _modelNumber));
            _modelName = Convert.ToString(Null.SetNull(dr["ModelName"], _modelName));
            _virtualFileID = Convert.ToInt32(dr["VirtualFileID"]);
            _allowedDownloads = Convert.ToInt32(Null.SetNull(dr["AllowedDownloads"], _allowedDownloads));
            _quantity = Convert.ToInt32(dr["Quantity"]);
            _downloads = Convert.ToInt32(Null.SetNull(dr["Downloads"], _downloads));
        }

        public int KeyID
        {
            get { return _orderDetailID; }
            set { _orderDetailID = value; }
        }

        #endregion

        #region IEquatable<DownloadInfo> Members

        public bool Equals(DownloadInfo other)
        {
            if (other == null)
                return false;
            return _orderID.Equals(other.OrderID) && _productID.Equals(other.ProductID);
        }

        #endregion
    }
}
