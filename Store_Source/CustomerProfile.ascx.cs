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
using System.IO;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

using DotNetNuke.Modules.Store.Core.Admin;
using DotNetNuke.Modules.Store.Core.Components;
using DotNetNuke.Modules.Store.Core.Providers;
using DotNetNuke.Modules.Store.Core.Providers.Address;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for CustomerProfile.
	/// </summary>
	public partial  class CustomerProfile : StoreControlBase, IStoreTabedControl
	{
		#region Private Members

		private CustomerNavigation _accountNav;

		#endregion

		#region Events

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
		    if (StoreSettings != null)
		    {
			    try
			    {
				    _accountNav = new CustomerNavigation(Request.QueryString);
				    pnlLoginMessage.Visible = !IsLogged;
                    pnlAddressProvider.Visible = IsLogged;
                    if (IsLogged)
                        LoadAddressProvider();
                    else
                        lblLoginMessage.Text = Localization.GetString("lblLoginMessage", LocalResourceFile);
			    }
			    catch (Exception ex)
			    {
				    Exceptions.ProcessModuleLoadException(this, ex);
			    }
		    }
		    else
		        pnlLoginMessage.Visible = false;
		}
		
		private void editControl_EditComplete(object sender, EventArgs e)
		{
            _accountNav.AddressID = Null.NullString;
            Response.Redirect(_accountNav.GetNavigationUrl(), true);
		}

        void editControl_Error(object sender, ErrorEventArgs e)
        {
            pnlAddressProvider.Visible = false;
            pnlLoginMessage.Visible = true;
            Exception ex = e.GetException();
            if (ex is ProviderSecurityException)
            {
                // TO DO: Log an Admin Alert
            }
            lblLoginMessage.Text = ex.Message;
        }

		#endregion

		#region Private Methods

		private void LoadAddressProvider()
		{
			plhAddressProvider.Controls.Clear();

            //Get an instance of the provider
            IAddressProvider addressProvider = StoreController.GetAddressProvider(StoreSettings.AddressName);
			
			//Create an instance of the provider's profile control
			ProviderControlBase providerControl = addressProvider.GetProfileControl(this, ControlPath);
            providerControl.ModuleConfiguration = ModuleConfiguration;
            providerControl.ParentControl = this;
			providerControl.EditComplete += editControl_EditComplete;
            providerControl.ProviderError += editControl_Error;

			plhAddressProvider.Controls.Add(providerControl);
		}

		#endregion

        #region IStoreTabedControl Members

        string IStoreTabedControl.Title
        {
            get { return Localization.GetString("lblParentTitle", LocalResourceFile); }
        }

        #endregion
    }
}
