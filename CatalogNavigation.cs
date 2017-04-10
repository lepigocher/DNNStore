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

using System.Collections.Specialized;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Modules.Store.Components;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for CatalogNavigation.
	/// </summary>
    public sealed class CatalogNavigation : NavigateWrapper
	{
		#region Constructors

		public CatalogNavigation()
		{
		}

		public CatalogNavigation(NameValueCollection queryString) : base(queryString)
		{
		}

		public CatalogNavigation(int moduleId, NameValueCollection queryString) : base(moduleId, queryString)
		{
		}

		#endregion

		#region Private Members

        private string _edit = Null.NullString;
        private int _categoryID = Null.NullInteger;
        private string _category = Null.NullString;
        private int _productID = Null.NullInteger;
        private string _product = Null.NullString;
        private int _reviewID = Null.NullInteger;
        private int _pageIndex = Null.NullInteger;
        private int _sortID = Null.NullInteger;
        private string _sortDir = Null.NullString;
        private int _searchID = Null.NullInteger;
        private string _searchValue = Null.NullString;

		#endregion

		#region Properties

        public string Edit
        {
            get { return _edit; }
            set { _edit = value; }
        }

        public int CategoryID
		{
			get { return _categoryID; }
			set { _categoryID = value; }
		}

        public string Category
		{
            get { return _category; }
            set { _category = value; }
		}

		public int ProductID
		{
			get { return _productID; }
			set { _productID = value; }
		}

        public string Product
		{
            get { return _product; }
            set { _product = value; }
		}

		public int ReviewID
		{
			get { return _reviewID; }
			set { _reviewID = value; }
		}

		public int PageIndex
		{
			get { return _pageIndex; }
			set { _pageIndex = value; }
		}

		public int SortID
		{
            get { return _sortID; }
            set { _sortID = value; }
		}

        public string SortDir
        {
            get { return _sortDir; }
            set { _sortDir = value; }
        }

		public int SearchID
		{
            get { return _searchID; }
            set { _searchID = value; }
		}

        public string SearchValue
        {
            get { return _searchValue; }
            set { _searchValue = value; }
        }

		#endregion
	}
}
