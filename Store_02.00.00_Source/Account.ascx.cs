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

using System;
using DotNetNuke.Common;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Modules.Store.Cart;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for Account.
	/// </summary>
    public partial class Account : StoreControlBase
    {
        #region Private Members

        private ModuleSettings _moduleSettings;
        private CustomerNavigation _nav;

        #endregion

		#region Events

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
		    if (StoreSettings != null)
		    {
                try
                {
                    _nav = new CustomerNavigation(Request.QueryString);
                    switch (_nav.PageID.ToLower())
                    {
                        case "checkout":
                        case "customercart":
                        case "customerprofile":
                        case "customerorders":
                        case "customerdownloads":
                            break;
                        default:
                            // Load the default control
                            _moduleSettings = new ModuleSettings(ModuleId, TabId);
                            _nav.PageID = _moduleSettings.MainCart.DefaultView;
                            break;
                    }
                    LoadAccountControl();
                }
                catch (Exception ex)
                {
                    string error = Localization.GetString("Unexpected.Error", TemplateSourceDirectory + "/" + Localization.LocalResourceDirectory + "/" + Localization.LocalSharedResourceFile);
                    Exceptions.ProcessModuleLoadException(error, this, ex, true);
                }
		    }
		}

        protected void Page_Load(object sender, EventArgs e)
		{
            if (StoreSettings != null)
            {
                string templatePath = CssTools.GetTemplatePath(this, StoreSettings.PortalTemplates);
                CssTools.AddCss(Page, templatePath, StoreSettings.StyleSheet);
                divControls.Visible = true;

                if (IsLogged == false)
                {
                    lblSpacer2.Visible = false;
                    btnProfile.Visible = false;
                    lblSpacer3.Visible = false;
                    btnOrders.Visible = false;
                    lblSpacer4.Visible = false;
                    btnDownloads.Visible = false;
                }
                else
                {
                    if (StoreSettings.AllowVirtualProducts == false)
                    {
                        lblSpacer4.Visible = false;
                        btnDownloads.Visible = false;
                    }
                }
            }
            else
            {
                if (UserInfo.IsSuperUser)
                {
                    string errorSettings = Localization.GetString("ErrorSettings", LocalResourceFile);
                    string errorSettingsHeading = Localization.GetString("ErrorSettingsHeading", LocalResourceFile);
                    UI.Skins.Skin.AddModuleMessage(this, errorSettingsHeading, errorSettings, UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning);
                }
                divControls.Visible = false;
            }
		}

        protected void btnStore_Click(object sender, EventArgs e)
        {
			Response.Redirect(Globals.NavigateURL(StoreSettings.StorePageID), true);
        }

        protected void btnCart_Click(object sender, EventArgs e)
		{
            _nav = new CustomerNavigation {PageID = "CustomerCart"};
            Response.Redirect(_nav.GetNavigationUrl(), true);
		}

		protected void btnProfile_Click(object sender, EventArgs e)
		{
            _nav = new CustomerNavigation {PageID = "CustomerProfile"};
		    Response.Redirect(_nav.GetNavigationUrl(), true);
		}

		protected void btnOrders_Click(object sender, EventArgs e)
		{
            _nav = new CustomerNavigation();
			_nav.PageID = "CustomerOrders";
			Response.Redirect(_nav.GetNavigationUrl(), true);
		}

        protected void btnDownloads_Click(object sender, EventArgs e)
		{
            _nav = new CustomerNavigation {PageID = "CustomerDownloads"};
            Response.Redirect(_nav.GetNavigationUrl(), true);
		}

		private void accountControl_EditComplete(object sender, EventArgs e)
		{
			Response.Redirect(_nav.GetNavigationUrl(), true);
		}

        protected void StoreAccountContinueShopping_Click(object sender, EventArgs e)
        {
			Response.Redirect(Globals.NavigateURL(StoreSettings.StorePageID), true);
        }

		#endregion

		#region Private Methods

		private void LoadAccountControl()
		{
			// TODO: We may want to use caching here
			StoreControlBase control = (StoreControlBase)LoadControl(ModulePath + _nav.PageID + ".ascx");
            control.ID = _nav.PageID.ToLower();
            control.ModuleConfiguration = ModuleConfiguration;
            control.StoreSettings = StoreSettings;
			control.EditComplete += accountControl_EditComplete;
			plhAccountControl.Controls.Clear();
			plhAccountControl.Controls.Add(control);
            if (control is IStoreTabedControl)
                lblParentTitle.Text = ((IStoreTabedControl)control).Title;
		}

		#endregion
    }
}
