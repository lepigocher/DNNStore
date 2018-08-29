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
using System.Web;

using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;

using DotNetNuke.Modules.Store.Core.Cart;
using DotNetNuke.Modules.Store.Core.Components;
using DotNetNuke.Modules.Store.Core.Providers;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for CustomerCart.
	/// </summary>
	public partial class CustomerCart : StoreControlBase, IStoreTabedControl
    {
        #region Private Members

        private int _itemsCount;

        #endregion

		#region Events

		protected void Page_Load(object sender, EventArgs e)
		{
		    if (StoreSettings != null)
		    {
                string templatePath = CssTools.GetTemplatePath(this, StoreSettings.PortalTemplates);
                // Read module settings and define cart properties
                MainCartSettings cartSettings = new ModuleSettings(ModuleId, TabId).MainCart;
                cartControl.ShowThumbnail = cartSettings.ShowThumbnail;
                cartControl.ThumbnailWidth = cartSettings.ThumbnailWidth;
                cartControl.GIFBgColor = cartSettings.GIFBgColor;
                cartControl.EnableImageCaching = cartSettings.EnableImageCaching;
                cartControl.CacheImageDuration = cartSettings.CacheImageDuration;
                cartControl.ProductColumn = cartSettings.ProductColumn.ToLower();
                cartControl.LinkToDetail = cartSettings.LinkToDetail;
                cartControl.IncludeVAT = cartSettings.IncludeVAT;
                cartControl.TemplatePath = templatePath;
                cartControl.ModuleConfiguration = ModuleConfiguration;
                cartControl.StoreSettings = StoreSettings;
                cartControl.EditComplete += cartControl_EditComplete;

                _itemsCount = CurrentCart.GetInfo(PortalId, StoreSettings.SecureCookie).Items;
                if (IsPostBack == false)
                    CheckState(_itemsCount);
		    }
		}

        protected void btnCheckout_Click(object sender, EventArgs e)
		{
            if (_itemsCount <= 0)
                Response.Redirect(Globals.NavigateURL(StoreSettings.StorePageID), true);
            else if (StoreSettings.CheckoutMode == CheckoutType.Registred && IsLogged == false)
            {
                string returnURL = Request.RawUrl;
                int posReturnParam = returnURL.IndexOf("?returnurl=");
                if (posReturnParam != Null.NullInteger)
                    returnURL = returnURL.Substring(0, posReturnParam);
                returnURL = "returnurl=" + HttpUtility.UrlEncode(returnURL);

                if (PortalSettings.LoginTabId != Null.NullInteger && string.IsNullOrEmpty(Request.QueryString["override"]))
                {
                    Response.Redirect(Globals.NavigateURL(PortalSettings.LoginTabId, "", new string[] { returnURL }), true);
                }
                else
                {
                    int targetTabId = PortalSettings.HomeTabId;
                    if (targetTabId == Null.NullInteger)
                        targetTabId = TabId;
                    Response.Redirect(Globals.NavigateURL(targetTabId, "login", new string[] { returnURL }), true);
                }
            }
            else
            {
                Page.Validate();
                if (Page.IsValid)
                {
                    CustomerNavigation nav = new CustomerNavigation
                    {
                        PageID = "Checkout"
                    };
                    Response.Redirect(nav.GetNavigationUrl(), true);
                }
            }
        }

        protected void cartControl_EditComplete(object sender, EventArgs e)
        {
            if (cartControl.ItemsCount == 0)
                InvokeEditComplete();
        }

		#endregion

		#region Private Methods

        private void CheckState(int itemsCount)
        {
            if (itemsCount <= 0)
            {
                lblCheckout.Text = Localization.GetString("AddItemsToCheckout", LocalResourceFile);
                pCheckoutMessage.Visible = false;
            }
            else
            {
                // Check if the checkout mode screen have to be displayed
                bool canCheckout = true;
                string checkoutMessage = "";
                switch (StoreSettings.CheckoutMode)
                {
                    case CheckoutType.Registred:
                        if (IsLogged == false)
                        {
                            checkoutMessage = Localization.GetString("RegistrationRequired", LocalResourceFile);
                            lblCheckout.Text = Localization.GetString("LoginToCheckout", LocalResourceFile);
                            canCheckout = false;
                        }
                        break;
                    case CheckoutType.UserChoice:
                        if (IsLogged == false)
                        {
                            checkoutMessage = Localization.GetString("RegistrationUserChoice", LocalResourceFile);
                            lblCheckout.Text = Localization.GetString("btnCheckout", LocalResourceFile);
                            canCheckout = false;
                        }
                        break;
                    case CheckoutType.Anonymous:
                        lblCheckout.Text = Localization.GetString("btnCheckout", LocalResourceFile);
                        break;
                    default:
                        break;
                }
                if (canCheckout == false)
                {
                    lblCheckoutMode.Text = checkoutMessage;
                    pCheckoutMessage.Visible = true;
                }
                else
                {
                    CustomerNavigation nav = new CustomerNavigation
                    {
                        PageID = "Checkout"
                    };
                    Response.Redirect(nav.GetNavigationUrl(), true);
                }
            }
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
