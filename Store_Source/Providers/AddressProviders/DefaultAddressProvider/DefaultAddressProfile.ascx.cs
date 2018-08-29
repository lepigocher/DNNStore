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
using System.Web.UI.WebControls;

using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;

using DotNetNuke.Modules.Store.Core.Providers;
using DotNetNuke.Modules.Store.Core.Providers.Address;

namespace DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider
{
	/// <summary>
    /// Summary description for DefaultAddressProfile.
	/// </summary>
	public partial class DefaultAddressProfile : ProviderControlBase
	{
		#region Private Members

		private AddressNavigation _addressNav;
		private int _addressID;
        private string _addressDefaultDescription;
		
		#endregion

		#region Event Handlers

		override protected void OnInit(EventArgs e)
		{
			grdAddresses.ItemDataBound += grdAddresses_ItemDataBound;
			base.OnInit(e);
		}

        protected void Page_Load(object sender, EventArgs e)
		{
			try 
			{
                // Get localized default address description
                _addressDefaultDescription = Localization.GetString("AddressDefaultDescription", LocalResourceFile);
				// Get the navigation settings and AddressID
				_addressNav = new AddressNavigation(Request.QueryString);
                _addressID = _addressNav.AddressID;

                if (!IsPostBack)
                {
                    // Get an Address controler
                    AddressController controller = new AddressController();

                    // Define default address settings
                    AddressSettings addressSettings = controller.GetAddressSettings(PortalId);
                    addressEdit.Settings = addressSettings;

                    // Edit Address
                    if (_addressID > 0)
                    {
                        plhGrid.Visible = false;
                        plhEditAddress.Visible = true;

                        AddressInfo address = (AddressInfo) controller.GetAddress(_addressID);
                        bool authorizedUser = (address.UserID == UserId);

                        if (authorizedUser)
                        {
                            if (string.IsNullOrEmpty(address.Email))
                                address.Email = UserInfo.Email;
                            addressEdit.Address = address;
                            addressEdit.ShowUserSaved = false;
                            addressEdit.ShowDescription = true;
                            // Set delete confirmation
                            cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");
                            cmdDelete.Visible = true;
                        }
                        else
                        {
                            ErrorMessage = Localization.GetString("ErrorLoading", LocalResourceFile);
                            // Someone is trying to steal an address!
                            // A log entry will be added in the parent control
                            InvokeSecurityProviderError();
                        }
                    }
                        // Add Address
                    else if (_addressID == 0)
                    {
                        plhGrid.Visible = false;
                        plhEditAddress.Visible = true;
                        addressEdit.Address = new AddressInfo {UserSaved = true};
                        addressEdit.ShowUserSaved = false;
                        addressEdit.ShowDescription = true;
                    }
                        // Display user's addresses
                    else
                    {
                        List<AddressInfo> addresses = controller.GetAddresses<AddressInfo>(PortalId, UserId, _addressDefaultDescription);

                        if (addresses.Count > 0)
                        {
                            grdAddresses.DataSource = addresses;
                            grdAddresses.DataBind();
                        }

                        plhGrid.Visible = true;
                        plhEditAddress.Visible = false;
                    }
                }
			} 
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		private void grdAddresses_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			IAddressInfo addressInfo = (IAddressInfo)e.Item.DataItem;

			Image imgPrimary = (Image)e.Item.FindControl("imgPrimary");
			if (imgPrimary != null)
			{
				if (addressInfo.PrimaryAddress)
					imgPrimary.Visible = true;
				else
					imgPrimary.Visible = false;
			}

			HyperLink lnkEdit = (HyperLink)e.Item.FindControl("lnkEdit");
			if (lnkEdit != null)
			{
				if (addressInfo.AddressID == 0)
				{
					//This is the registration address, so the profile editor should be used to modify this address
                    string[] urlparams = {"UserId=" + UserId};
                    lnkEdit.NavigateUrl = Globals.NavigateURL(PortalSettings.UserTabId, "Profile", urlparams);
				}
				else 
				{
					_addressNav.AddressID = addressInfo.AddressID;
					lnkEdit.NavigateUrl = _addressNav.GetNavigationUrl();
				}
			}
		}

		protected void btnAdd_Click(object sender, EventArgs e)
		{
			_addressNav.AddressID = 0;
			Response.Redirect(_addressNav.GetNavigationUrl(), false);
		}

		protected void cmdUpdate_Click(object sender, EventArgs e)
		{
			try 
			{
				if (Page.IsValid) 
				{
					IAddressInfo address = addressEdit.Address;

					address.AddressID = _addressID;
					address.PortalID = PortalId;
					address.UserID = UserId;
					address.CreatedByUser = UserId.ToString();
					address.CreatedDate	= DateTime.Now;

					AddressController controller = new AddressController();

					if (address.AddressID > 0)
					    controller.UpdateAddress(address);
					else 
						controller.AddAddress(address);

				    InvokeEditComplete();
				}
			} 
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		protected void cmdCancel_Click(object sender, EventArgs e)
		{
			InvokeEditComplete();
		}

		protected void cmdDelete_Click(object sender, EventArgs e)
		{
			try 
			{
				if (_addressID > 0) 
				{
					AddressController controller = new AddressController();
					controller.DeleteAddress(_addressID);

					_addressNav.AddressID = Null.NullInteger;
				}

				InvokeEditComplete();
			} 
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		#endregion
	}
}
