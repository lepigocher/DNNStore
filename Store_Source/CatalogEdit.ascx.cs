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
using DotNetNuke.Services.Exceptions;

namespace DotNetNuke.Modules.Store.WebControls
{
	public partial class CatalogEdit : PortalModuleBase
	{
		#region Private Members

		private CatalogNavigation _nav;
		
		#endregion

		#region Events

		protected void Page_Load(object sender, EventArgs e)
		{
			try 
			{
				// Get the navigation settings
				_nav = new CatalogNavigation(Request.QueryString);

				if (_nav.Edit.ToLower() == "product")
				{
					ProductEdit productEdit = (ProductEdit)LoadControl(ControlPath + "ProductEdit.ascx");
                    productEdit.ID = "ProductEdit";
					productEdit.DataSource = _nav.ProductID;
					productEdit.EditComplete += editControl_EditComplete;

					plhControls.Controls.Add(productEdit);
				}
				else if (_nav.Edit.ToLower() == "category")
				{
					CategoryEdit categoryEdit = (CategoryEdit)LoadControl(ControlPath + "CategoryEdit.ascx");
                    categoryEdit.ID = "CategoryEdit";
					categoryEdit.DataSource = _nav.CategoryID;
					categoryEdit.EditComplete += editControl_EditComplete;

					plhControls.Controls.Add(categoryEdit);
				}
			} 
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		private void editControl_EditComplete(object sender, EventArgs e)
		{
            if (_nav.Edit.ToLower() == "category")
                _nav.CategoryID = Null.NullInteger;
            _nav.Edit = Null.NullString;
            _nav.ProductID = Null.NullInteger;
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		#endregion
	}
}
