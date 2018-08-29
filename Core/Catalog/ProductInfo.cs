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
using System.Collections.Generic;

using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Tokens;

namespace DotNetNuke.Modules.Store.Core.Catalog
{
	/// <summary>
	/// Product class info.
	/// </summary>
    [Serializable]
    public sealed class ProductInfo : IComparable<ProductInfo>, IHydratable, IEquatable<ProductInfo>, IPropertyAccess
	{
		#region Private Members

		private int _productID;
		private int _portalID;
        private int _storePageID;
		private int _categoryID;
		private string _manufacturer;
		private string _modelNumber;
		private string _modelName;
        private string _SEOName;
        private string _productImage;
        private decimal _regularPrice;
        private decimal _unitCost;
		private string _keywords;
		private string _summary;
		private string _description;
		private bool _featured;
		private bool _archived;
		private string _createdByUser;
		private DateTime _createdDate;
        private decimal _productWeight;
        private decimal _productHeight;
        private decimal _productLength;
        private decimal _productWidth;
        private DateTime _saleStartDate;
        private DateTime _saleEndDate;
        private decimal _salePrice;
        private decimal _VATPrice;
        private int _stockQuantity;
        private int _lowThreshold;
        private int _highThreshold;
        private int _deliveryTime;
        private decimal _purchasePrice;
        private int _roleID;
		private bool _isVirtual;
        private int _virtualFileID;
        private int _allowedDownloads;

		#endregion

		#region Properties

		public int ProductID
		{
			get { return _productID; }
			set { _productID = value; }
		}

		public int PortalID
		{
			get { return _portalID; }
			set { _portalID = value; }
		}

        public int StorePageID
		{
            get { return _storePageID; }
            set { _storePageID = value; }
		}

		public int CategoryID
		{
			get { return _categoryID; }
			set { _categoryID = value; }
		}

		public string Manufacturer
		{
			get { return _manufacturer; }
			set { _manufacturer = value; }
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

        public string SEOName
        {
            get { return _SEOName; }
            set { _SEOName = value; }
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
		
		public string ProductImage
		{
			get { return _productImage; }
			set { _productImage = value; }
		}

        public decimal RegularPrice
		{
            get { return _regularPrice; }
            set { _regularPrice = value; }
		}

		public decimal UnitCost
		{
			get { return _unitCost; }
			set { _unitCost = value; }
		}

		public string Keywords
		{
			get { return _keywords; }
            set { _keywords = value; }
		}

		public string Summary
		{
			get { return _summary; }
			set { _summary = value; }
		}
		
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		public bool Featured
		{
			get { return _featured; }
			set { _featured = value; }
		}

		public bool Archived
		{
			get { return _archived; }
			set { _archived = value; }
		}

		public string CreatedByUser
		{
			get { return _createdByUser; }
			set { _createdByUser = value; }
		}

		public DateTime CreatedDate
		{
			get { return _createdDate; }
			set { _createdDate = value; }
		}

        public decimal ProductWeight
        {
            get { return _productWeight; }
            set { _productWeight = value; }
        }

        public decimal ProductHeight
        {
            get { return _productHeight; }
            set { _productHeight = value; }
        }

        public decimal ProductLength
        {
            get { return _productLength; }
            set { _productLength = value; }
        }

        public decimal ProductWidth
        {
            get { return _productWidth; }
            set { _productWidth = value; }
        }

        public DateTime SaleStartDate
        {
            get { return _saleStartDate; }
            set { _saleStartDate = value; }
        }

        public DateTime SaleEndDate
        {
            get { return _saleEndDate; }
            set { _saleEndDate = value; }
        }

        public decimal SalePrice
        {
            get { return _salePrice; }
            set { _salePrice = value; }
        }

        public decimal VATPrice
        {
            get { return _VATPrice; }
            set { _VATPrice = value; }
        }

        public int StockQuantity
        {
            get { return _stockQuantity; }
            set { _stockQuantity = value; }
        }

        public int LowThreshold
        {
            get { return _lowThreshold; }
            set { _lowThreshold = value; }
        }

        public int HighThreshold
        {
            get { return _highThreshold; }
            set { _highThreshold = value; }
        }

        public int DeliveryTime
        {
            get { return _deliveryTime; }
            set { _deliveryTime = value; }
        }

        public decimal PurchasePrice
        {
            get { return _purchasePrice; }
            set { _purchasePrice = value; }
        }

        public int RoleID
        {
            get { return _roleID; }
            set { _roleID = value; }
        }

        public bool IsVirtual
		{
            get { return _isVirtual; }
            set { _isVirtual = value; }
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

        public static IComparer<ProductInfo> ManufacturerSorter
        {
            get { return new ManufacturerComparer(); }
        }

        public static IComparer<ProductInfo> ModelNumberSorter
        {
            get { return new ModelNumberComparer(); }
        }

        public static IComparer<ProductInfo> UnitCostSorter
        {
            get { return new UnitCostComparer(); }
        }

        public static IComparer<ProductInfo> CreatedDateSorter
        {
            get { return new CreatedDateComparer(); }
        }

        #endregion

		#region Object Overrides

		public override int GetHashCode() 
		{
			return _productID.GetHashCode();
		}

		#endregion

        #region IComparable Interface

        public int CompareTo(ProductInfo other)
        {
            return _modelName.CompareTo(other.ModelName);
        } 

        #endregion

        #region Nested Classes Comparers

        private class ManufacturerComparer : IComparer<ProductInfo>
        {
            #region IComparer<ProductInfo> Members

            public int Compare(ProductInfo first, ProductInfo second)
            {
                return first.Manufacturer.CompareTo(second.Manufacturer);
            }

            #endregion
        }

        private class ModelNumberComparer : IComparer<ProductInfo>
        {
            #region IComparer<ProductInfo> Members

            public int Compare(ProductInfo first, ProductInfo second)
            {
                return first.ModelNumber.CompareTo(second.ModelNumber);
            }

            #endregion
        }

        private class SEONameComparer : IComparer<ProductInfo>
        {
            #region IComparer<ProductInfo> Members

            public int Compare(ProductInfo first, ProductInfo second)
            {
                return first.SEOName.CompareTo(second.SEOName);
            }

            #endregion
        }

        private class UnitCostComparer : IComparer<ProductInfo>
        {
            #region IComparer<ProductInfo> Members

            public int Compare(ProductInfo first, ProductInfo second)
            {
                return first.UnitCost.CompareTo(second.UnitCost);
            }

            #endregion
        }

        private class CreatedDateComparer : IComparer<ProductInfo>
        {
            #region IComparer<ProductInfo> Members

            public int Compare(ProductInfo first, ProductInfo second)
            {
                return first.CreatedDate.CompareTo(second.CreatedDate);
            }

            #endregion
        }

        #endregion

        #region IHydratable Interface

        public void Fill(System.Data.IDataReader dr)
        {
            _productID = Convert.ToInt32(dr["ProductID"]);
            _portalID = Convert.ToInt32(dr["PortalID"]);
            _categoryID = Convert.ToInt32(dr["CategoryID"]);
            _manufacturer = Convert.ToString(Null.SetNull(dr["Manufacturer"], _manufacturer));
            _modelNumber = Convert.ToString(Null.SetNull(dr["ModelNumber"], _modelNumber));
            _modelName = Convert.ToString(Null.SetNull(dr["ModelName"], _modelName));
            _SEOName = Convert.ToString(Null.SetNull(dr["SEOName"], _SEOName));
            _productImage = Convert.ToString(Null.SetNull(dr["ProductImage"], _productImage));
            _regularPrice = Convert.ToDecimal(dr["RegularPrice"]);
            _unitCost = Convert.ToDecimal(dr["UnitCost"]);
            _keywords = Convert.ToString(Null.SetNull(dr["Keywords"], _keywords));
            _summary = Convert.ToString(Null.SetNull(dr["Summary"], _summary));
            _description = Convert.ToString(Null.SetNull(dr["Description"], _description));
            _featured = Convert.ToBoolean(dr["Featured"]);
            _archived = Convert.ToBoolean(dr["Archived"]);
            _createdByUser = Convert.ToString(Null.SetNull(dr["CreatedByUser"], _createdByUser));
            _createdDate = Convert.ToDateTime(Null.SetNull(dr["CreatedDate"], _createdDate));
            _productWeight = Convert.ToDecimal(dr["ProductWeight"]);
            _productHeight = Convert.ToDecimal(dr["ProductHeight"]);
            _productLength = Convert.ToDecimal(dr["ProductLength"]);
            _productWidth = Convert.ToDecimal(dr["ProductWidth"]);
            _saleStartDate = Convert.ToDateTime(Null.SetNull(dr["SaleStartDate"], _saleStartDate));
            _saleEndDate = Convert.ToDateTime(Null.SetNull(dr["SaleEndDate"], _saleEndDate));
            _salePrice = Convert.ToDecimal(Null.SetNull(dr["SalePrice"], _salePrice));
            _stockQuantity = Convert.ToInt32(dr["StockQuantity"]);
            _lowThreshold = Convert.ToInt32(dr["LowThreshold"]);
            _highThreshold = Convert.ToInt32(dr["HighThreshold"]);
            _deliveryTime = Convert.ToInt32(dr["DeliveryTime"]);
            _purchasePrice = Convert.ToDecimal(dr["PurchasePrice"]);
            _roleID = Convert.ToInt32(Null.SetNull(dr["RoleID"], _roleID));
            _isVirtual = Convert.ToBoolean(dr["Virtual"]);
            _virtualFileID = Convert.ToInt32(Null.SetNull(dr["VirtualFileID"], _virtualFileID));
            _allowedDownloads = Convert.ToInt32(Null.SetNull(dr["AllowedDownloads"], _allowedDownloads));
        }

        public int KeyID
        {
            get { return _productID; }
            set { _productID = value; }
        }

        #endregion

        #region IEquatable<ProductInfo> Interface

        public bool Equals(ProductInfo other)
        {
            if (other == null)
                return false;
            return _productID.Equals(other.ProductID);
        }

        #endregion

        #region IPropertyAccess Interface

        public CacheLevel Cacheability
        {
            get { return CacheLevel.fullyCacheable; }
        }

        public string GetProperty(string strPropertyName, string strFormat, System.Globalization.CultureInfo formatProvider, Entities.Users.UserInfo accessingUser, Scope accessLevel, ref bool propertyNotFound)
        {
            string propertyValue = null;
            switch (strPropertyName.ToLower())
            {
                case "manufacturer":
                    propertyValue = _manufacturer;
                    break;
                case "modelnumber":
                    propertyValue = _modelNumber;
                    break;
                case "modelname":
                    propertyValue = _modelName;
                    break;
                case "producttitle":
                    propertyValue = ProductTitle;
                    break;
                case "productpagelink":
                    propertyValue = Globals.NavigateURL(_storePageID, "", new string[] { "categoryid=" + _categoryID, "productid=" + _productID });
                    break;
                default:
                    propertyNotFound = true;
                    break;
            }
            return propertyValue;
        }

        #endregion
    }
}
