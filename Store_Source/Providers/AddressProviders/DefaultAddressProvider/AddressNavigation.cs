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

using System.Collections.Specialized;

using DotNetNuke.Common.Utilities;

using DotNetNuke.Modules.Store.Core.Components;

namespace DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider
{
	/// <summary>
	/// Summary description for CustomerNavigation.
	/// </summary>
    public sealed class AddressNavigation : NavigateWrapper
	{
		#region Private Members

		private string _pageID = Null.NullString;
        private int _addressID = Null.NullInteger;
		private int _customerID = Null.NullInteger;

		#endregion

		#region Constructors

		public AddressNavigation()
		{
		}

		public AddressNavigation(NameValueCollection queryString) : base(queryString)
		{
		}

		#endregion

		#region Public Properties

		public string PageID
		{
			get { return _pageID; }
			set { _pageID = value; }
		}

		public int AddressID
		{
			get { return _addressID; }
			set { _addressID = value; }
		}

		public int CustomerID
		{
			get { return _customerID; }
			set { _customerID = value; }
		}

		#endregion
	}
}
