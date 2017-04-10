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
	/// Summary description for AdminNavigation.
	/// </summary>
    public sealed class AdminNavigation : NavigateWrapper
	{
		#region Private Members

		private string _pageID = Null.NullString;
		private string _editID = Null.NullString;
		private int _categoryID = Null.NullInteger;
		private int _productID = Null.NullInteger;
		private int _copyProductID = Null.NullInteger;
		private int _reviewID = Null.NullInteger;
		private int _statusID = Null.NullInteger;
		private int _customerID = Null.NullInteger;
		private int _orderID = Null.NullInteger;
		private int _couponID = Null.NullInteger;
		private int _pageIndex = Null.NullInteger;

		#endregion

		#region Constructors

		public AdminNavigation()
		{
		}

		public AdminNavigation(NameValueCollection queryString) : base(queryString)
		{
		}

		#endregion

		#region Properties

		public string PageID
		{
			get { return _pageID; }
			set { _pageID = value; }
		}

		public string EditID
		{
			get { return _editID; }
			set { _editID = value; }
		}

		public int CategoryID
		{
			get { return _categoryID; }
			set { _categoryID = value; }
		}

		public int ProductID
		{
			get { return _productID; }
			set { _productID = value; }
		}

		public int CopyProductID
		{
            get { return _copyProductID; }
            set { _copyProductID = value; }
		}

		public int ReviewID
		{
			get { return _reviewID; }
			set { _reviewID = value; }
		}

		public int StatusID
		{
			get { return _statusID; }
			set { _statusID = value; }
		}

		public int CustomerID
		{
			get { return _customerID; }
			set { _customerID = value; }
		}

		public int OrderID
		{
			get { return _orderID; }
			set { _orderID = value; }
		}

        public int CouponID
		{
            get { return _couponID; }
            set { _couponID = value; }
		}

		public int PageIndex
		{
			get { return _pageIndex; }
			set { _pageIndex = value; }
		}

		#endregion
	}
}
