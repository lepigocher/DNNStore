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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Security.Roles;
using DotNetNuke.Modules.Store.Components;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for Media.
	/// </summary>
    public partial class Store : StoreControlBase
    {
        #region Private Members

        private AdminNavigation _adminNav;
        private bool _canManageStore;
        private bool _canManageOrders;
        private bool _canManageCatalog;

        #endregion

		#region Events

		protected void Page_Load(object sender, EventArgs e)
		{
            if (StoreSettings == null)
            {
                string templatePath = CssTools.GetTemplatePath(this, false);
                CssTools.AddCss(Page, templatePath, "Template.css");

            }
            else
            {
                string templatePath = CssTools.GetTemplatePath(this, StoreSettings.PortalTemplates);
                CssTools.AddCss(Page, templatePath, StoreSettings.StyleSheet);
            }

            CheckUserRoles();

            _adminNav = new AdminNavigation(Request.QueryString);
            if (_adminNav.PageID == Null.NullString)
            {
                // Define the default control
                _adminNav = new AdminNavigation();
                if (_canManageStore)
                    _adminNav.PageID = "StoreAdmin";
                else if (_canManageOrders)
                    _adminNav.PageID = "CustomerAdmin";
                else if (_canManageCatalog)
                    _adminNav.PageID = "CategoryAdmin";
            }
            else
            {
                switch (_adminNav.PageID.ToLower())
                {
                    case "storeadmin":
                        if (!_canManageStore)
                            _adminNav.PageID = "";
                        break;
                    case "customeradmin":
                        if (!_canManageOrders)
                            _adminNav.PageID = "";
                        break;
                    case "categoryadmin":
                    case "productadmin":
                    case "reviewadmin":
                    case "couponadmin":
                        if (!_canManageCatalog)
                            _adminNav.PageID = "";
                        break;
                    default:
                        _adminNav.PageID = "";
                        break;
                }
            }

			LoadAdminControl();
		}

        protected void btnStoreInfo_Click(object sender, EventArgs e)
		{
			_adminNav = new AdminNavigation();
			_adminNav.PageID = "StoreAdmin";
			Response.Redirect(_adminNav.GetNavigationUrl(), true);
		}

        protected void btnCustomers_Click(object sender, EventArgs e)
		{
			_adminNav = new AdminNavigation();
			_adminNav.PageID = "CustomerAdmin";
            Response.Redirect(_adminNav.GetNavigationUrl(), true);
		}

        protected void btnCategories_Click(object sender, EventArgs e)
		{
			_adminNav = new AdminNavigation();
			_adminNav.PageID = "CategoryAdmin";
            Response.Redirect(_adminNav.GetNavigationUrl(), true);
		}

        protected void btnProducts_Click(object sender, EventArgs e)
		{
			_adminNav = new AdminNavigation();
			_adminNav.PageID = "ProductAdmin";
            Response.Redirect(_adminNav.GetNavigationUrl(), true);
		}

        protected void btnReviews_Click(object sender, EventArgs e)
		{
			_adminNav = new AdminNavigation();
			_adminNav.PageID = "ReviewAdmin";
            Response.Redirect(_adminNav.GetNavigationUrl(), true);
		}

        protected void btnCoupons_Click(object sender, EventArgs e)
		{
			_adminNav = new AdminNavigation();
            _adminNav.PageID = "CouponAdmin";
            Response.Redirect(_adminNav.GetNavigationUrl(), true);
		}

        protected void btnHelp_Click(object sender, EventArgs e)
		{
			_adminNav = new AdminNavigation();
			_adminNav.PageID = "HelpAdmin";
            Response.Redirect(_adminNav.GetNavigationUrl(), true);
		}

		private void adminControl_EditComplete(object sender, EventArgs e)
		{
            Response.Redirect(_adminNav.GetNavigationUrl(), true);
		}

		#endregion

		#region Private Methods

        private void CheckUserRoles()
        {
            if (UserInfo.IsSuperUser || UserInfo.IsInRole("Administrators"))
            {
                _canManageStore = true;
                plhStoreInfo.Visible = true;
                if (StoreSettings != null)
                {
                    _canManageOrders = true;
                    plhOrders.Visible = true;
                    _canManageCatalog = true;
                    plhCatalog.Visible = true;
                    if (StoreSettings.AllowCoupons)
                        plhCoupons.Visible = true;
                }
            }
            else
            {
                plhStoreInfo.Visible = false;

                if (StoreSettings != null)
                {
                    RoleController roleController = new RoleController();

                    if (StoreSettings.OrderRoleID != Null.NullInteger)
                    {
                        RoleInfo orderRole = roleController.GetRole(StoreSettings.OrderRoleID, PortalId);
                        if (UserInfo.IsInRole(orderRole.RoleName))
                            _canManageOrders = true;
                    }
                    if (_canManageOrders)
                        plhOrders.Visible = true;
                    else
                        plhOrders.Visible = false;

                    if (StoreSettings.CatalogRoleID != Null.NullInteger)
                    {
                        RoleInfo catalogRole = roleController.GetRole(StoreSettings.CatalogRoleID, PortalId);
                        if (UserInfo.IsInRole(catalogRole.RoleName))
                            _canManageCatalog = true;
                    }
                    if (_canManageCatalog)
                        plhCatalog.Visible = true;
                    else
                        plhCatalog.Visible = false;
                }
            }
        }

		private void LoadAdminControl()
		{
			plhAdminControl.Controls.Clear();

            if (!String.IsNullOrEmpty(_adminNav.PageID))
            {
                // TODO: We may want to use caching here
                StoreControlBase adminControl = (StoreControlBase)LoadControl(ModulePath + _adminNav.PageID + ".ascx");
                adminControl.ModuleConfiguration = ModuleConfiguration;
                adminControl.StoreSettings = StoreSettings;
                adminControl.EditComplete += adminControl_EditComplete;
                adminControl.ID = _adminNav.PageID;
                plhAdminControl.Controls.Add(adminControl);
                if (adminControl is IStoreTabedControl)
                    lblParentTitle.Text = ((IStoreTabedControl)adminControl).Title;
            }
		}

		#endregion
    }
}
