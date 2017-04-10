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
using System.Collections;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Cart;
using DotNetNuke.Modules.Store.Providers;
using DotNetNuke.Modules.Store.Providers.Address;
using DotNetNuke.Modules.Store.Providers.Shipping;
using DotNetNuke.Modules.Store.Providers.Tax;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for Store.
	/// </summary>
	public partial class StoreAdmin : StoreControlBase, IStoreTabedControl
    {
        #region Private Members

        private string _hostTemplatesFolder;
        private string _portalTemplatesFolder;
        private StoreControlBase _gatewayControl;

		#endregion

		#region Events

		protected void Page_Load(object sender, EventArgs e)
		{
            _hostTemplatesFolder = MapPath(ModulePath) + "Templates";
            _portalTemplatesFolder = PortalSettings.HomeDirectoryMapPath + "Store\\Templates";
            if (!IsPostBack)
            {
                string storePageID = null;
                string cartPageID = null;
                int outOfStock = Null.NullInteger;
                int productBehavior = Null.NullInteger;
                int orderRoleID = Null.NullInteger;
                int catalogRoleID = Null.NullInteger;
                CheckoutType checkoutMode = CheckoutType.Registred;
                int impersonatedUserID = Null.NullInteger;
                int onOrderPaidRoleID = Null.NullInteger;
                string gatewayName = "EmailProvider";
                string addressName = "Default";
                string taxName = "Default";
                string shippingName = "Default";
                bool isSettingsExist = StoreSettings != null;

                if (isSettingsExist)
                {
                    txtStoreName.Text = StoreSettings.Name;
                    txtDescription.Text = StoreSettings.Description;
                    txtKeywords.Text = StoreSettings.Keywords;
                    if (StoreSettings.SEOFeature)
                        chkSEOFeature.Checked = true;
                    else
                    {
                        trDescription.Visible = false;
                        trKeywords.Visible = false;
                    }
                    txtEmail.Text = StoreSettings.DefaultEmailAddress;
                    txtCurrencySymbol.Text = StoreSettings.CurrencySymbol;
                    chkUsePortalTemplates.Checked = StoreSettings.PortalTemplates;
                    storePageID = StoreSettings.StorePageID.ToString(CultureInfo.InvariantCulture);
                    cartPageID = StoreSettings.ShoppingCartPageID.ToString(CultureInfo.InvariantCulture);
                    chkAuthorizeCancel.Checked = StoreSettings.AuthorizeCancel;
                    chkInventoryManagement.Checked = StoreSettings.InventoryManagement;
                    if (!StoreSettings.InventoryManagement)
                    {
                        trOutOfStock.Visible = false;
                        trProductsBehavior.Visible = false;
                        trAvoidNegativeStock.Visible = false;
                    }
                    outOfStock = StoreSettings.OutOfStock;
                    productBehavior = StoreSettings.ProductsBehavior;
                    chkAvoidNegativeStock.Checked = StoreSettings.AvoidNegativeStock;
                    orderRoleID = StoreSettings.OrderRoleID;
                    catalogRoleID = StoreSettings.CatalogRoleID;
                    if (!StoreSettings.SecureCookie && !SymmetricHelper.CanSafelyEncrypt)
                        rowSecureCookie.Visible = false;
                    chkSecureCookie.Checked = StoreSettings.SecureCookie;
                    checkoutMode = StoreSettings.CheckoutMode;
                    impersonatedUserID = StoreSettings.ImpersonatedUserID;
                    chkNoDelivery.Checked = StoreSettings.NoDelivery;
                    if (chkNoDelivery.Checked)
                    {
                        trShippingProviderSelection.Visible = false;
                        trShippingProvider.Visible = false;
                        trFreeShipping.Visible = false;
                    }
                    chkAllowVirtualProducts.Checked = StoreSettings.AllowVirtualProducts;
                    chkAllowCoupons.Checked = StoreSettings.AllowCoupons;
                    gatewayName = StoreSettings.GatewayName;
                    addressName = StoreSettings.AddressName;
                    taxName = StoreSettings.TaxName;
                    if (!chkNoDelivery.Checked)
                        shippingName = StoreSettings.ShippingName;
                    chkAllowFreeShipping.Checked = StoreSettings.AllowFreeShipping;
                    trFreeShipping.Visible = chkAllowFreeShipping.Checked;
                    decimal minOrderAmount = StoreSettings.MinOrderAmount;
                    if (minOrderAmount == Null.NullDecimal)
                        minOrderAmount = 0;
                    txtMinOrderAmount.Text = minOrderAmount.ToString("0.00");
                    chkRestrictToCountries.Checked = StoreSettings.RestrictToCountries;
                    trAuthorizedCountries.Visible = chkRestrictToCountries.Checked;
                    FillAuthorizedCountries(StoreSettings.AuthorizedCountries);
                    onOrderPaidRoleID = StoreSettings.OnOrderPaidRoleID;
                    dshAddressProvider.IsExpanded = false;
                    dshTaxProvider.IsExpanded = false;
                    dshShippingProvider.IsExpanded = false;
                    trProviders.Visible = false;
                }
                else
                {
                    trDescription.Visible = false;
                    trKeywords.Visible = false;
                    trOutOfStock.Visible = false;
                    trProductsBehavior.Visible = false;
                    trAvoidNegativeStock.Visible = false;
                    trFreeShipping.Visible = false;
                    trAuthorizedCountries.Visible = false;
                    FillAuthorizedCountries("");
                    trAddressProvider.Visible = false;
                    trTaxProvider.Visible = false;
                    trShippingProvider.Visible = false;
                }
                // Load available style sheet
                LoadStyleSheet(chkUsePortalTemplates.Checked);
                // Load available tabs to host:
                // - the catalog module
                // - the account module
                LoadTabs(storePageID, cartPageID);
                // Load Out Of Stock messages and Products Behavior
                LoadOutOfStockMessages(outOfStock, productBehavior);
                // Load portal roles
                LoadManagerRoles(orderRoleID, catalogRoleID);
                // Load checkout types
                LoadCheckoutType(checkoutMode);
                // Define Mode Interface
                DefineModeUI(lstCheckoutMode.SelectedIndex, impersonatedUserID);
                // Load order roles
                LoadOrderRoles(onOrderPaidRoleID);
                // Load available gateway providers
                LoadGatewayProviders(gatewayName);
                // Load available address providers
                LoadAddressProviders(addressName);
                // Load available tax providers
                LoadTaxProviders(taxName);
                // Load available shipping providers
                LoadShippingProviders(shippingName);
                // Open or close gateway provider section
                dshGatewayProvider.IsExpanded = !isSettingsExist;
            }

            // Load gateway admin control
            LoadGatewayAdmin(lstGateway.SelectedItem.Text);
            // Load address admin control
            LoadAddressAdmin(lstAddressProviders.SelectedValue);
            // Load tax admin control
            LoadTaxAdmin(lstTaxProviders.SelectedValue);
            // Load shipping admin control
            LoadShippingAdmin(lstShippingProviders.SelectedValue);
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
            if (Page.IsValid)
            {
                StoreController storeController = new StoreController();
                bool newStore = false;

                if (StoreSettings == null)
                {
                    StoreSettings = new StoreInfo();
                    newStore = true;
                }

                StoreSettings.PortalID = PortalId;
                StoreSettings.Name = txtStoreName.Text;
                StoreSettings.SEOFeature = chkSEOFeature.Checked;
                if (StoreSettings.SEOFeature)
                {
                    StoreSettings.Description = txtDescription.Text;
                    StoreSettings.Keywords = txtKeywords.Text;
                }
                StoreSettings.DefaultEmailAddress = txtEmail.Text;
                StoreSettings.CurrencySymbol = txtCurrencySymbol.Text;
                StoreSettings.PortalTemplates = chkUsePortalTemplates.Checked;
                StoreSettings.StyleSheet = lstStyleSheet.SelectedValue;
                StoreSettings.StorePageID = int.Parse(lstStorePageID.SelectedValue);
                StoreSettings.ShoppingCartPageID = int.Parse(lstShoppingCartPageID.SelectedValue);
                StoreSettings.AuthorizeCancel = chkAuthorizeCancel.Checked;
                StoreSettings.InventoryManagement = chkInventoryManagement.Checked;
                if (StoreSettings.InventoryManagement)
                {
                    StoreSettings.OutOfStock = lstOutOfStock.SelectedIndex;
                    StoreSettings.ProductsBehavior = lstProductsBehavior.SelectedIndex;
                    StoreSettings.AvoidNegativeStock = chkAvoidNegativeStock.Checked;
                }
                StoreSettings.OrderRoleID = int.Parse(lstOrderRole.SelectedValue);
                StoreSettings.CatalogRoleID = int.Parse(lstCatalogRole.SelectedValue);
                StoreSettings.SecureCookie = chkSecureCookie.Checked;
                StoreSettings.CheckoutMode = (CheckoutType)int.Parse(lstCheckoutMode.SelectedValue);
                if (StoreSettings.CheckoutMode != CheckoutType.Registred)
                    StoreSettings.ImpersonatedUserID = int.Parse(hidImpersonatedUserID.Value);
                else
                {
                    StoreSettings.ImpersonatedUserID = Null.NullInteger;
                    StoreSettings.AllowVirtualProducts = chkAllowVirtualProducts.Checked;
                }
                StoreSettings.NoDelivery = chkNoDelivery.Checked;
                StoreSettings.AllowCoupons = chkAllowCoupons.Checked;
                StoreSettings.AllowFreeShipping = chkAllowFreeShipping.Checked;
                StoreSettings.MinOrderAmount = chkAllowFreeShipping.Checked ? Decimal.Parse(txtMinOrderAmount.Text) : 0;
                StoreSettings.RestrictToCountries = chkRestrictToCountries.Checked;
                StoreSettings.AuthorizedCountries = GetSelectedCountries();
                StoreSettings.OnOrderPaidRoleID = Int32.Parse(lstOnPaidOrderRole.SelectedValue);
                StoreSettings.GatewayName = lstGateway.SelectedItem.Text;
                if ((_gatewayControl != null) && (_gatewayControl.DataSource != null))
                    StoreSettings.GatewaySettings = _gatewayControl.DataSource.ToString();
                StoreSettings.AddressName = lstAddressProviders.SelectedValue;
                StoreSettings.TaxName = lstTaxProviders.SelectedValue;
                if (StoreSettings.NoDelivery)
                    StoreSettings.ShippingName = "Default";
                else
                    StoreSettings.ShippingName = lstShippingProviders.SelectedValue;

                if (newStore)
                {
                    StoreSettings.CreatedByUser = UserInfo.Username;
                    storeController.AddStoreInfo(StoreSettings);
                    trAddressProvider.Visible = true;
                    dshAddressProvider.IsExpanded = true;
                    trTaxProvider.Visible = true;
                    dshTaxProvider.IsExpanded = true;
                    if (!chkNoDelivery.Checked)
                    {
                        trShippingProviderSelection.Visible = true;
                        trShippingProvider.Visible = true;
                        dshShippingProvider.IsExpanded = true;
                    }
                    else
                    {
                        trShippingProviderSelection.Visible = false;
                        trShippingProvider.Visible = false;
                        dshShippingProvider.IsExpanded = false;
                    }
                }
                else
                    storeController.UpdateStoreInfo(StoreSettings);

                if (StoreSettings.PortalTemplates)
                    CopyTemplates();
                InvokeEditComplete();
            }
		}

        protected void chkSEOFeature_CheckedChanged(object sender, EventArgs e)
        {
            trDescription.Visible = chkSEOFeature.Checked;
            trKeywords.Visible = chkSEOFeature.Checked;
        }

        protected void chkUsePortalTemplates_CheckedChanged(object sender, EventArgs e)
        {
            LoadStyleSheet(chkUsePortalTemplates.Checked);
        }

        protected void chkInventoryManagement_CheckedChanged(object sender, EventArgs e)
        {
            trOutOfStock.Visible = chkInventoryManagement.Checked;
            trProductsBehavior.Visible = chkInventoryManagement.Checked;
            trAvoidNegativeStock.Visible = chkInventoryManagement.Checked;
        }

        protected void lstCheckoutMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            int impersonatedUserID = Null.NullInteger;
            if (StoreSettings != null)
                impersonatedUserID = StoreSettings.ImpersonatedUserID;
            DefineModeUI(lstCheckoutMode.SelectedIndex, impersonatedUserID);
        }

        protected void lstImpersonatedRoleID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstImpersonatedRoleID.SelectedIndex > 0)
            {
                LoadUserAccounts(lstImpersonatedRoleID.SelectedItem.Text);
            }
            else
            {
                trImpersonatedUserID.Visible = false;
                trValidateUser.Visible = false;
            }
        }

        protected void btnValidateUser_Click(object sender, EventArgs e)
        {
            int userID = Convert.ToInt32(lstImpersonatedUserID.SelectedValue);
            DefineModeUI(lstCheckoutMode.SelectedIndex, userID);
        }

        protected void btnChangeImpersonatedUser_Click(object sender, EventArgs e)
        {
            DefineModeUI(lstCheckoutMode.SelectedIndex, Null.NullInteger);
        }

        protected void chkNoDelivery_CheckedChanged(object sender, EventArgs e)
        {
            trShippingProviderSelection.Visible = !chkNoDelivery.Checked;
            trShippingProvider.Visible = !chkNoDelivery.Checked;
        }

        protected void chkAllowFreeShipping_CheckedChanged(object sender, EventArgs e)
        {
            trFreeShipping.Visible = chkAllowFreeShipping.Checked;
        }

        protected void chkRestrictToCountries_CheckedChanged(object sender, EventArgs e)
        {
            trAuthorizedCountries.Visible = chkRestrictToCountries.Checked;
        }

		protected void lstGateway_SelectedIndexChanged(object sender, EventArgs e)
		{
            LoadGatewayAdmin(lstGateway.SelectedItem.Text);
            dshGatewayProvider.IsExpanded = true;
		}

        protected void lstAddressProviders_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAddressAdmin(lstAddressProviders.SelectedValue);
            dshAddressProvider.IsExpanded = true;
        }

        protected void lstTaxProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTaxAdmin(lstTaxProviders.SelectedValue);
            dshTaxProvider.IsExpanded = true;
        }

        protected void lstShippingProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadShippingAdmin(lstShippingProviders.SelectedValue);
            dshShippingProvider.IsExpanded = true;
        }

		#endregion

		#region Private Methods

		private void LoadTabs(string storePageID, string cartPageID)
		{
			TabController tabController = new TabController();
			ArrayList tabs = tabController.GetTabs(PortalId);
            string tabHidden = Localization.GetString("TabHidden", LocalResourceFile);

			foreach (TabInfo tabInfo in tabs)
			{
				if (!tabInfo.IsDeleted && !tabInfo.IsAdminTab && !tabInfo.IsSuperTab)
				{
                    string tabID = tabInfo.TabID.ToString(CultureInfo.InvariantCulture);
                    string tabName;
                    if (tabInfo.IsVisible)
                        tabName = tabInfo.TabName;
                    else
                        tabName = tabInfo.TabName + tabHidden;
                    lstStorePageID.Items.Add(new ListItem(tabName, tabID));
                    lstShoppingCartPageID.Items.Add(new ListItem(tabName, tabID));
                }
			}

            string select = Localization.GetString("EmptyComboValue", LocalResourceFile);
            ListItem none = new ListItem(select, "-1");
            lstStorePageID.Items.Insert(0, none);
            none = new ListItem(select, "-1");
            lstShoppingCartPageID.Items.Insert(0, none);

            if (!string.IsNullOrEmpty(storePageID))
            {
                ListItem storeItem = lstStorePageID.Items.FindByValue(storePageID);
                if (storeItem != null)
                    storeItem.Selected = true;
            }

            if (!string.IsNullOrEmpty(cartPageID))
            {
                ListItem cartItem = lstShoppingCartPageID.Items.FindByValue(cartPageID);
                if (cartItem != null)
                    cartItem.Selected = true;
            }
		}

        private void LoadOutOfStockMessages(int outOfStock, int productBehavior)
        {
            string message = Localization.GetString("OOStockQuantity", LocalResourceFile);
            lstOutOfStock.Items.Add(new ListItem(message, StockMessage.Quantity.ToString()));
            message = Localization.GetString("OOStockUnavailable", LocalResourceFile);
            lstOutOfStock.Items.Add(new ListItem(message, StockMessage.Unavailable.ToString()));
            message = Localization.GetString("OOStockRestocking", LocalResourceFile);
            lstOutOfStock.Items.Add(new ListItem(message, StockMessage.Restocking.ToString()));

            if (outOfStock != Null.NullInteger)
                lstOutOfStock.SelectedIndex = outOfStock;

            // Product's Behavior
            message = Localization.GetString("BehaviorAccept", LocalResourceFile);
            lstProductsBehavior.Items.Add(new ListItem(message, Behavior.Accept.ToString()));
            message = Localization.GetString("BehaviorHideControls", LocalResourceFile);
            lstProductsBehavior.Items.Add(new ListItem(message, Behavior.HideControls.ToString()));
            message = Localization.GetString("BehaviorHideProduct", LocalResourceFile);
            lstProductsBehavior.Items.Add(new ListItem(message, Behavior.HideProduct.ToString()));

            if (productBehavior != Null.NullInteger)
                lstProductsBehavior.SelectedIndex = productBehavior;
        }

        private void LoadCheckoutType(CheckoutType checkoutMode)
        {
            ListItem registred = new ListItem(Localization.GetString("CheckoutModeRegistred", LocalResourceFile), ((int)CheckoutType.Registred).ToString(CultureInfo.InvariantCulture));
            ListItem userchoice = new ListItem(Localization.GetString("CheckoutModeUserChoice", LocalResourceFile), ((int)CheckoutType.UserChoice).ToString(CultureInfo.InvariantCulture));
            ListItem anonymous = new ListItem(Localization.GetString("CheckoutModeAnonymous", LocalResourceFile), ((int)CheckoutType.Anonymous).ToString(CultureInfo.InvariantCulture));
            lstCheckoutMode.Items.Add(registred);
            lstCheckoutMode.Items.Add(userchoice);
            lstCheckoutMode.Items.Add(anonymous);
            ListItem selected = lstCheckoutMode.Items.FindByValue(((int)checkoutMode).ToString(CultureInfo.InvariantCulture));
            if (selected != null)
                selected.Selected = true;
        }

        private void LoadRoles()
        {
            RoleController controller = new RoleController();
            ArrayList roles = controller.GetPortalRoles(PortalId);

            lstImpersonatedRoleID.Items.Clear();
            lstImpersonatedRoleID.DataTextField = "RoleName";
            lstImpersonatedRoleID.DataValueField = "RoleID";
            lstImpersonatedRoleID.DataSource = roles;
            lstImpersonatedRoleID.DataBind();
            ListItem none = new ListItem(Localization.GetString("EmptyRoleComboValue", LocalResourceFile), "-1");
            lstImpersonatedRoleID.Items.Insert(0, none);

            tbImpersonatedUserSelection.Visible = true;
            trImpersonatedUserID.Visible = false;
            lstImpersonatedUserID.Items.Clear();
            trValidateUser.Visible = false;
            lblImpersonatedUser.Visible = false;
            btnChangeImpersonatedUser.Visible = false;
        }

        private void LoadOrderRoles(int roleId)
        {
            RoleController controller = new RoleController();
            ArrayList roles = controller.GetPortalRoles(PortalId);

            lstOnPaidOrderRole.Items.Clear();
            lstOnPaidOrderRole.DataTextField = "RoleName";
            lstOnPaidOrderRole.DataValueField = "RoleID";
            lstOnPaidOrderRole.DataSource = roles;
            lstOnPaidOrderRole.DataBind();
            ListItem none = new ListItem(Localization.GetString("EmptyRoleComboValue", LocalResourceFile), "-1");
            lstOnPaidOrderRole.Items.Insert(0, none);

            ListItem item = lstOnPaidOrderRole.Items.FindByValue(roleId.ToString());
            if (item != null)
                item.Selected = true;
        }

        private void LoadUserAccounts(string roleName)
        {
            RoleController controller = new RoleController();
            ArrayList users = controller.GetUsersByRoleName(PortalId, roleName);

            lstImpersonatedUserID.Items.Clear();
            lstImpersonatedUserID.DataTextField = "FullName";
            lstImpersonatedUserID.DataValueField = "UserID";
            lstImpersonatedUserID.DataSource = users;
            lstImpersonatedUserID.DataBind();
            ListItem none = new ListItem(Localization.GetString("EmptyUserComboValue", LocalResourceFile), "-1");
            lstImpersonatedUserID.Items.Insert(0, none);

            trImpersonatedUserID.Visible = true;
            trValidateUser.Visible = true;
        }

        private void LoadManagerRoles(int orderRoleID, int catalogRoleID)
        {
            RoleController roleController = new RoleController();
            ArrayList portalRoles = roleController.GetPortalRoles(PortalId);
            string chooseItem = Localization.GetString("EmptyComboValue", LocalResourceFile);

            lstOrderRole.Items.Insert(0, new ListItem(chooseItem, Null.NullInteger.ToString(CultureInfo.InvariantCulture)));
            lstOrderRole.AppendDataBoundItems = true;
            lstOrderRole.DataValueField = "RoleID";
            lstOrderRole.DataTextField = "RoleName";
            lstOrderRole.DataSource = portalRoles;
            lstOrderRole.DataBind();
            if (orderRoleID != Null.NullInteger)
            {
                ListItem orderRoleItem = lstOrderRole.Items.FindByValue(orderRoleID.ToString(CultureInfo.InvariantCulture));
                if (orderRoleItem != null)
                    orderRoleItem.Selected = true;
            }

            lstCatalogRole.Items.Insert(0, new ListItem(chooseItem, Null.NullInteger.ToString(CultureInfo.InvariantCulture)));
            lstCatalogRole.AppendDataBoundItems = true;
            lstCatalogRole.DataValueField = "RoleID";
            lstCatalogRole.DataTextField = "RoleName";
            lstCatalogRole.DataSource = portalRoles;
            lstCatalogRole.DataBind();
            if (catalogRoleID != Null.NullInteger)
            {
                ListItem catalogRoleItem = lstCatalogRole.Items.FindByValue(catalogRoleID.ToString(CultureInfo.InvariantCulture));
                if (catalogRoleItem != null)
                    catalogRoleItem.Selected = true;
            }
        }

        private void DefineModeUI(int mode, int impersonatedUserID)
        {
            hidImpersonatedUserID.Value = impersonatedUserID.ToString();
            switch (mode)
            {
                case 0:
                    {
                        trImpersonatedUser.Visible = false;
                        trAllowVirtualProducts.Visible = true;
                    }
                    break;
                case 1:
                case 2:
                    {
                        trImpersonatedUser.Visible = true;
                        trAllowVirtualProducts.Visible = false;
                        if (impersonatedUserID != Null.NullInteger)
                        {
                            UserController controller = new UserController();
                            UserInfo user = controller.GetUser(PortalId, impersonatedUserID);
                            if (user != null)
                            {
                                lblImpersonatedUser.Text = user.DisplayName;
                                lblImpersonatedUser.Visible = true;
                                btnChangeImpersonatedUser.Visible = true;
                                tbImpersonatedUserSelection.Visible = false;
                                return;
                            }
                        }

                        LoadRoles();
                    }
                    break;
                default:
                    {
                        trImpersonatedUser.Visible = false;
                        trAllowVirtualProducts.Visible = false;
                    }
                    break;
            }
        }

        private void LoadGatewayProviders(string providerName)
		{
			GatewayController controller = new GatewayController(Server.MapPath(ModulePath));
			lstGateway.DataTextField = "GatewayName";
			lstGateway.DataValueField = "GatewayPath";
			lstGateway.DataSource = controller.GetGateways();
			lstGateway.DataBind();

            if (!string.IsNullOrEmpty(providerName))
			{
                ListItem gateway = lstGateway.Items.FindByText(providerName);
                if (gateway != null)
                    gateway.Selected = true;
			}
            else
            {
                ListItem gateway = lstGateway.Items.FindByText("EmailProvider");
                if (gateway != null)
                    gateway.Selected = true;
            }
		}

		private void LoadGatewayAdmin(string gatewayName)
		{
			plhGatewayProvider.Controls.Clear();

			GatewayController controller = new GatewayController(Server.MapPath(ModulePath));
			GatewayInfo gateway = controller.GetGateway(gatewayName);
			if (gateway != null)
			{
				string controlPath = gateway.GatewayPath + "\\" + gateway.AdminControl;
				if (File.Exists(controlPath))
				{
					controlPath = controlPath.Replace(Server.MapPath(ModulePath), ModulePath);

					_gatewayControl = (StoreControlBase)LoadControl(controlPath);
                    _gatewayControl.ModuleConfiguration = ModuleConfiguration;
					_gatewayControl.EnableViewState = true;
					_gatewayControl.DataSource = gateway.GetSettings(PortalId);
                    _gatewayControl.ID = gatewayName;

					plhGatewayProvider.Controls.Add(_gatewayControl);
				}
				else
				{
					LiteralControl error = new LiteralControl("<span class=\"NormalRed\">" + Localization.GetString("CouldNotFind", LocalResourceFile) + " " + controlPath + ".</span>");
					plhGatewayProvider.Controls.Add(error);
				}
			}
			else
			{
				LiteralControl error = new LiteralControl("<span class=\"NormalRed\">" + Localization.GetString("GatewayNotSelected", LocalResourceFile) + "</span>");
				plhGatewayProvider.Controls.Add(error);
			}
		}

        private void LoadAddressProviders(string providerName)
        {
            lstAddressProviders.DataTextField = "Description";
            lstAddressProviders.DataValueField = "Name";
            lstAddressProviders.DataSource = StoreController.GetProviderList(StoreProviderType.Address);
            lstAddressProviders.DataBind();

            if (!string.IsNullOrEmpty(providerName))
            {
                ListItem provider = lstAddressProviders.Items.FindByValue(providerName);
                if (provider != null)
                    provider.Selected = true;
            }
        }

		private void LoadAddressAdmin(string addressName)
		{
            plhAddressProvider.Controls.Clear();

			//Get an instance of the provider
            IAddressProvider addressProvider = StoreController.GetAddressProvider(addressName);
			
			//Create an instance of the provider's admin control
            ProviderControlBase providerControl = addressProvider.GetAdminControl(this, ModulePath);
            providerControl.ID = "AddressProvider";

            plhAddressProvider.Controls.Add(providerControl);
		}

        private void LoadTaxProviders(string providerName)
        {
            lstTaxProviders.DataTextField = "Description";
            lstTaxProviders.DataValueField = "Name";
            lstTaxProviders.DataSource = StoreController.GetProviderList(StoreProviderType.Tax);
            lstTaxProviders.DataBind();

            if (!string.IsNullOrEmpty(providerName))
            {
                ListItem provider = lstTaxProviders.Items.FindByValue(providerName);
                if (provider != null)
                    provider.Selected = true;
            }
        }

        private void LoadTaxAdmin(string providerName)
		{
			plhTaxProvider.Controls.Clear();

			//Get an instance of the provider
            ITaxProvider taxProvider = StoreController.GetTaxProvider(providerName);
			
			//Create an instance of the provider's admin control
			ProviderControlBase providerControl = taxProvider.GetAdminControl(this, ModulePath);
            providerControl.ID = "TaxProvider";

			plhTaxProvider.Controls.Add(providerControl);
		}

        private void LoadShippingProviders(string providerName)
        {
            lstShippingProviders.DataTextField = "Description";
            lstShippingProviders.DataValueField = "Name";
            lstShippingProviders.DataSource = StoreController.GetProviderList(StoreProviderType.Shipping);
            lstShippingProviders.DataBind();

            if (!string.IsNullOrEmpty(providerName))
            {
                ListItem provider = lstShippingProviders.Items.FindByValue(providerName);
                if (provider != null)
                    provider.Selected = true;
            }
        }

        private void LoadShippingAdmin(string providerName)
		{
			plhShippingProvider.Controls.Clear();

			//Get an instance of the provider
            IShippingProvider shippingProvider = StoreController.GetShippingProvider(providerName);
			
			//Create an instance of the provider's admin control
			ProviderControlBase providerControl = shippingProvider.GetAdminControl(this, ModulePath);
            providerControl.ID = "ShippingProvider";

			plhShippingProvider.Controls.Add(providerControl);
		}

        private void LoadStyleSheet(bool fromPortal)
        {
            string[] fileList;
            lstStyleSheet.Items.Clear();

            if (fromPortal)
            {
                if (Directory.Exists(_portalTemplatesFolder))
                    fileList = Directory.GetFiles(_portalTemplatesFolder, "*.css");
                else
                    fileList = Directory.GetFiles(_hostTemplatesFolder, "*.css");
            }
            else
            {
                fileList = Directory.GetFiles(_hostTemplatesFolder, "*.css");
            }

            if (fileList.Length > 0)
            {
                foreach (string file in fileList)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    ListItem item = new ListItem(fileInfo.Name, fileInfo.Name.ToLower());
                    lstStyleSheet.Items.Add(item);
                }
            }

            if (StoreSettings != null)
            {
                ListItem item = lstStyleSheet.Items.FindByValue(StoreSettings.StyleSheet);
                if (item != null)
                    item.Selected = true;
            }
        }

        private void CopyTemplates()
        {
            string[] fileList;
            bool isDirty = false;

            // Templates
            if (!Directory.Exists(_portalTemplatesFolder))
            {
                Directory.CreateDirectory(_portalTemplatesFolder);

                fileList = Directory.GetFiles(_hostTemplatesFolder, "*.*");

                if (fileList.Length > 0)
                {
                    foreach (string file in fileList)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        fileInfo.CopyTo(_portalTemplatesFolder + "\\" + fileInfo.Name, false);
                    }
                    isDirty = true;
                }
            }

            // Images
            if (!Directory.Exists(_portalTemplatesFolder + "\\Images"))
            {
                Directory.CreateDirectory(_portalTemplatesFolder + "\\Images");

                fileList = Directory.GetFiles(_hostTemplatesFolder + "\\Images", "*.*");

                if (fileList.Length > 0)
                {
                    foreach (string file in fileList)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        fileInfo.CopyTo(_portalTemplatesFolder + "\\Images\\" + fileInfo.Name, false);
                    }
                    isDirty = true;
                }
            }

            // StyleSheet
            if (!Directory.Exists(_portalTemplatesFolder + "\\StyleSheet"))
            {
                Directory.CreateDirectory(_portalTemplatesFolder + "\\StyleSheet");

                fileList = Directory.GetFiles(_hostTemplatesFolder + "\\StyleSheet", "*.*");

                if(fileList.Length > 0)
                {
                    foreach (string file in fileList)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        fileInfo.CopyTo(_portalTemplatesFolder + "\\StyleSheet\\" + fileInfo.Name, false);
                    }
                    isDirty = true;
                }
            }

            // Synchronize filesystem if nedded
            if (isDirty)
                FileSystemUtils.SynchronizeFolder(PortalId, PortalSettings.HomeDirectoryMapPath, "", true, true, true);
        }

        private void FillAuthorizedCountries(string authorizedCountries)
        {
            ListController ctlEntry = new ListController();
            ListEntryInfoCollection entryCollection = ctlEntry.GetListEntryInfoCollection("Country");
            if (entryCollection.Count > 0)
            {
                lbAuthorizedCountries.DataSource = entryCollection;
                lbAuthorizedCountries.DataBind();

                if (!string.IsNullOrEmpty(authorizedCountries))
                {
                    foreach (string authorizedCountry in authorizedCountries.Split(','))
                    {
                        ListItem item = lbAuthorizedCountries.Items.FindByValue(authorizedCountry);
                        if (item != null)
                            item.Selected = true;
                    }
                }
            }
        }

        private string GetSelectedCountries()
        {
            string selectedCountries = string.Empty;

            foreach (ListItem item in lbAuthorizedCountries.Items)
            {
                if (item.Selected)
                    selectedCountries += item.Value + ",";
            }

            return selectedCountries.TrimEnd(',');
        }

		#endregion

        #region Public Methods

        public void ExpandShippingHeader()
        {
            dshShippingProvider.IsExpanded = true;
        }

        public void ExpandTaxHeader()
        {
            dshTaxProvider.IsExpanded = true;
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
