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
	/// Category class info.
	/// </summary>
    [Serializable]
    public sealed class CategoryInfo : IComparable<CategoryInfo>, IHydratable, IEquatable<CategoryInfo>, IPropertyAccess
	{
		#region Private Declarations

        private int _categoryID;
		private int _portalID;
        private int _storePageID;
        private string _name;
		private string _seoName;
		private string _keywords;
		private string _description;
		private string _message;
		private bool _archived;
		private string _createdByUser;
		private DateTime _createdDate;
        private int _orderID;
        private int _parentCategoryID;
        private string _parentCategoryName;
        private string _categoryPathName;

		#endregion

		#region Properties

		public int CategoryID 
		{
            get { return _categoryID; }
            set { _categoryID = value; }
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

		public string Name 
		{
			get { return _name; }
			set { _name = value; }
		}

        public string SEOName 
		{
            get { return _seoName; }
            set { _seoName = value; }
		}

        public string Keywords 
		{
            get { return _keywords; }
            set { _keywords = value; }
		}

		public string Description 
		{
			get { return _description; }
			set { _description = value; }
		}

		public string Message 
		{
			get { return _message; }
			set { _message = value; }
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

        public int OrderID
        {
            get { return _orderID; }
            set { _orderID = value; }
        }

        public int ParentCategoryID
        {
            get { return _parentCategoryID; }
            set { _parentCategoryID = value; }
        }

        public string ParentCategoryName
        {
            get { return _parentCategoryName; }
            set { _parentCategoryName = value; }
        }

        public string CategoryPathName
        {

            get { return _categoryPathName; }
            set { _categoryPathName = value; }
        }

        public static IComparer<CategoryInfo> OrderIDSorter
        {
            get {return new OrderIDComparer(); }
        }

        public static IComparer<CategoryInfo> CreatedDateSorter
        {
            get { return new CreatedDateComparer(); }
        }

        public static IComparer<CategoryInfo> ParentCategoryNameSorter
        {
            get { return new ParentCategoryNameComparer(); }
        }

		#endregion

		#region Object Overrides

		public override int GetHashCode() 
		{
            return _categoryID.GetHashCode();
		}

		#endregion

        #region IComparable Interface

        public int CompareTo(CategoryInfo other)
        {
            return _name.CompareTo(other.Name);
        }

        #endregion

        #region Nested Classes Comparers

        private class OrderIDComparer : IComparer<CategoryInfo>
        {
            #region IComparer<CategoryInfo> Members

            public int Compare(CategoryInfo first, CategoryInfo second)
            {
                return first.OrderID.CompareTo(second.OrderID);
            }

            #endregion
        }

        private class CreatedDateComparer : IComparer<CategoryInfo>
        {
            #region IComparer<CategoryInfo> Members

            public int Compare(CategoryInfo first, CategoryInfo second)
            {
                return first.CreatedDate.CompareTo(second.CreatedDate);
            }

            #endregion
        }

        private class ParentCategoryNameComparer : IComparer<CategoryInfo>
        {
            #region IComparer<CategoryInfo> Members

            public int Compare(CategoryInfo first, CategoryInfo second)
            {
                return first.ParentCategoryName.CompareTo(second.ParentCategoryName);
            }

            #endregion
        }

        #endregion

        #region IHydratable Interface

        public void Fill(System.Data.IDataReader dr)
        {
            _categoryID = Convert.ToInt32(dr["CategoryID"]);
            _portalID = Convert.ToInt32(dr["PortalID"]);
            _name = Convert.ToString(Null.SetNull(dr["CategoryName"], _name));
            _seoName = Convert.ToString(Null.SetNull(dr["SEOName"], _seoName));
            _keywords = Convert.ToString(Null.SetNull(dr["CategoryKeywords"], _keywords));
            _description = Convert.ToString(Null.SetNull(dr["CategoryDescription"], _description));
            _message = Convert.ToString(Null.SetNull(dr["Message"], _message));
            _archived = Convert.ToBoolean(dr["Archived"]);
            _createdByUser = Convert.ToString(Null.SetNull(dr["CreatedByUser"], _createdByUser));
            _createdDate = Convert.ToDateTime(Null.SetNull(dr["CreatedDate"], _createdDate));
            _orderID = Convert.ToInt32(Null.SetNull(dr["OrderID"], _orderID));
            _parentCategoryID = Convert.ToInt32(Null.SetNull(dr["ParentCategoryID"], _parentCategoryID));
            _parentCategoryName = Convert.ToString(Null.SetNull(dr["ParentCategoryName"], _parentCategoryName));
        }

        public int KeyID
        {
            get { return _categoryID; }
            set { _categoryID = value; }
        }

        #endregion

        #region IEquatable<CategoryInfo> Interface

        public bool Equals(CategoryInfo other)
        {
            if (other == null)
                return false;
            return _categoryID.Equals(other.CategoryID);
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
                case "name":
                    propertyValue = _name;
                    break;
                case "categorypagelink":
                    propertyValue = Globals.NavigateURL(_storePageID, "", new string[] { "categoryid=" + _categoryID });
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
