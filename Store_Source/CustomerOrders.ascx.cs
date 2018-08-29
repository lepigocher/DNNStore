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
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Mail;

using DotNetNuke.Modules.Store.Core.Admin;
using DotNetNuke.Modules.Store.Core.Cart;
using DotNetNuke.Modules.Store.Core.Components;
using DotNetNuke.Modules.Store.Core.Customer;
using DotNetNuke.Modules.Store.Core.Providers.Address;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for CustomerOrders.
	/// </summary>
	public partial  class CustomerOrders : StoreControlBase, IStoreTabedControl
	{
		#region Private Members

        private static readonly Regex RegPaid = new Regex(@"\[IFPAID\](.+?)\[/IFPAID\]", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex RegStripStartNewLine = new Regex(@"^\r\n", RegexOptions.Compiled);
        private static readonly Regex RegStripEndNewLine = new Regex(@"\r\n$", RegexOptions.Compiled);
        private const string CookieName = "DotNetNuke_Store_Portal_";
        private CustomerNavigation _customerNav;
        private OrderController _orderController;
        private IAddressProvider _addressProvider;
        private string _message = string.Empty;
        private string _orderDateFormat = string.Empty;
        private readonly NumberFormatInfo _localFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
        private List<OrderStatus> _orderStatusList;
        private ModuleSettings _moduleSettings;
        private string _productColumn;
        private bool _canManageOrders;

		#endregion

        #region Public Members

        public bool ShowOrdersInStatus;
        public int OrderStatusID = Null.NullInteger;

        #endregion

        #region Events

        override protected void OnInit(EventArgs e)
		{
            if (StoreSettings != null)
            {
                grdOrders.ItemDataBound += grdOrders_ItemDataBound;
                grdOrderDetails.ItemDataBound += grdOrderDetails_ItemDataBound;
                lnkbtnSave.Click += lnkbtnSave_Click;
                btnBack.Click += btnBack_Click;
            }
			base.OnInit(e);
		}
		
		protected void Page_Load(object sender, EventArgs e)
		{
		    if (StoreSettings != null)
		    {
                // Read module settings
                _moduleSettings = new ModuleSettings(ModuleId, TabId);
                // Init vars
                _productColumn = _moduleSettings.MainCart.ProductColumn.ToLower();
                if (StoreSettings.CurrencySymbol != string.Empty)
                    _localFormat.CurrencySymbol = StoreSettings.CurrencySymbol;
                _orderDateFormat = Localization.GetString("OrderDateFormat", LocalResourceFile);
			    _customerNav = new CustomerNavigation(Request.QueryString);
			    lblError.Text = "";  //Initialize the Error Label.
                pnlOrdersError.Visible = false;
                _orderController = new OrderController();
                _addressProvider = StoreController.GetAddressProvider(StoreSettings.AddressName);

                CheckUserRoles();

                if (ShowOrdersInStatus && OrderStatusID != Null.NullInteger)
                {
                    plhGrid.Visible = true;
                    plhForm.Visible = false;

                    List<OrderInfo> orders = _orderController.GetOrders(PortalId, OrderStatusID);

                    if (orders.Count > 0)
                    {
                        _orderStatusList = _orderController.GetOrderStatuses();
                        grdOrders.DataSource = orders;
                        grdOrders.DataBind();
                    }
                    else
                    {
                        lblError.Text = Localization.GetString("NoOrdersFound", LocalResourceFile);
                        pnlOrdersError.Visible = true;
                        pnlOrders.Visible = false;
                        grdOrders.DataSource = null;
                        grdOrders.DataBind();
                    }
                }
                else
                {
                    if (_customerNav.OrderID != Null.NullInteger)
                    {
                        plhGrid.Visible = false;
                        plhForm.Visible = true;

                        if (_customerNav.OrderID != 0 && !IsPostBack)
                        {
                            lblEditTitle.Text = Localization.GetString("ViewDetails", LocalResourceFile);
                            ShowOrderDetails(_customerNav.OrderID);
                        }
                    }
                    else
                    {
                        if (_customerNav.CustomerID == Null.NullInteger)
                            _customerNav.CustomerID = UserId;
                        else if (_canManageOrders == false && _customerNav.CustomerID != UserId)
                        {
                            // Someone is trying to steal data
                            pnlOrders.Visible = false;
                            throw new Exception(Localization.GetString("Unexpected.Error", LocalSharedResourceFile));
                        }

                        if (_canManageOrders || StoreSettings.AuthorizeCancel)
                            grdOrders.Columns[6].Visible = true;
                        else
                            grdOrders.Columns[6].Visible = false;

                        plhGrid.Visible = true;
                        plhForm.Visible = false;
                        DisplayCustomerOrders();
                    }
                }
		    }
		    else
		    {
		        pnlOrdersError.Visible = false;
		        pnlOrders.Visible = false;
		    }
		}

		protected void grdOrders_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                OrderInfo order = (OrderInfo)e.Item.DataItem;

                //Order Date
                Label lblOrderDateText = (Label)e.Item.FindControl("lblOrderDateText");
                lblOrderDateText.Text = order.OrderDate.ToString(_orderDateFormat);

                //Order Status
                Label lblOrderStatusText = (Label)e.Item.FindControl("lblOrderStatusText");
                lblOrderStatusText.Text = GetOrderStatus(order.OrderStatusID);

                //Order Total
                Label lblOrderTotalText = (Label)e.Item.FindControl("lblOrderTotalText");
                lblOrderTotalText.Text = order.GrandTotal.ToString("C", _localFormat);

                //Ship Date (Status Date)
                Label lblShipDateText = (Label)e.Item.FindControl("lblShipDateText");
                if (order.OrderStatusID > 1)
                    lblShipDateText.Text = order.ShipDate.ToString(_orderDateFormat);
                else
                    lblShipDateText.Text = "";

                //Edit-Details link
                HyperLink lnkEdit = (HyperLink)e.Item.FindControl("lnkEdit");
                if (lnkEdit != null)
                {
                    _customerNav.OrderID = order.OrderID;
                    _customerNav.CustomerID = order.CustomerID;
                    lnkEdit.NavigateUrl = _customerNav.GetNavigationUrl();
                }

                //Cancel link
                if (_canManageOrders || StoreSettings.AuthorizeCancel)
                {
                    LinkButton lnkCancel = (LinkButton)e.Item.FindControl("lnkCancel");
                    if (lnkCancel != null)
                    {
                        if (order.OrderStatusID == (int)OrderInfo.OrderStatusList.AwaitingPayment
                            || order.OrderStatusID == (int)OrderInfo.OrderStatusList.AwaitingStock
                            || order.OrderStatusID == (int)OrderInfo.OrderStatusList.Paid)
                        {
                            lnkCancel.CommandArgument = order.OrderID.ToString();
                            lnkCancel.Click += lnkCancel_Click;
                            lnkCancel.Enabled = true;
                        }
                        else 
                            lnkCancel.Enabled = false;
                    }
                }
            }
		}

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            int orderID;
            if (Int32.TryParse(((LinkButton)sender).CommandArgument, out orderID))
                CancelOrder(orderID);
        }

        protected void btnPayNow_Click(object sender, EventArgs e)
        {
            int orderID;
            if (Int32.TryParse(((Button)sender).CommandArgument, out orderID))
            {
                // Get order details
                List<OrderDetailInfo> details = _orderController.GetOrderDetails(orderID);
                if (details != null)
                {
                    bool secureCookies = StoreSettings.SecureCookie;
                    // Populate cart with order details
                    foreach (OrderDetailInfo detail in details)
                        CurrentCart.AddItem(PortalId, secureCookies, detail.ProductID, detail.Quantity);
                    // Set cookie and redirect to checkout page
                    SetOrderIdCookie(orderID);
                    _customerNav = new CustomerNavigation
                    {
                        PageID = "checkout"
                    };
                    Response.Redirect(_customerNav.GetNavigationUrl(), true);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            int orderID;
            if (Int32.TryParse(((Button)sender).CommandArgument, out orderID))
                CancelOrder(orderID);
        }

        protected void grdOrderDetails_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                OrderDetailInfo detail = (OrderDetailInfo)e.Item.DataItem;

                //Product Title
                Label lblModelName = (Label)e.Item.FindControl("Label1");
                if (lblModelName != null)
                {
                    string productTitle = null;
                    switch (_productColumn)
                    {
                        case "modelnumber":
                            productTitle = detail.ModelNumber;
                            break;
                        case "modelname":
                            productTitle = detail.ModelName;
                            break;
                        case "producttitle":
                            productTitle = detail.ProductTitle;
                            break;
                    }
                    lblModelName.Text = productTitle;
                }

                //Unit Cost
                Label lblODPriceText = (Label)e.Item.FindControl("lblODPriceText");
                lblODPriceText.Text = detail.UnitCost.ToString("C", _localFormat);

                //Extended Amount
                Label lblODSubtotalText = (Label)e.Item.FindControl("lblODSubtotalText");
                lblODSubtotalText.Text = detail.ExtendedAmount.ToString("C", _localFormat);
            }
            
		}

        protected void editControl_EditComplete(object sender, EventArgs e)
		{
			_customerNav.OrderID = Null.NullInteger;
			Response.Redirect(_customerNav.GetNavigationUrl(), false);
		}

        protected void lnkbtnSave_Click(object sender, EventArgs e)
        {
            //Update the order status...
            OrderInfo Order = _orderController.GetOrder(PortalId, _customerNav.OrderID);
            
            if (Order != null)
            {
                int orderStatusID = Convert.ToInt32(ddlOrderStatus.SelectedValue);
                Order.OrderStatusID = orderStatusID;
                Order.Status = _orderController.GetStatusText(orderStatusID);
                // Update Status Order
                _orderController.UpdateOrder(Order.OrderID, Order.OrderDate, Order.OrderNumber, Order.ShippingAddressID, Order.BillingAddressID, Order.TaxTotal, Order.ShippingCost, Order.CouponID, Order.Discount, true, Order.OrderStatusID, Order.CustomerID);

                // If order is paid
                if (orderStatusID == 7)
                {
                    // Add User to Product Roles
                    _orderController.AddUserToRoles(PortalId, Order);
                    // Add User to Order Role
                    if (StoreSettings.OnOrderPaidRoleID != Null.NullInteger)
                        _orderController.AddUserToPaidOrderRole(PortalId, Order.CustomerID, StoreSettings.OnOrderPaidRoleID);
                }

                // If order is canceled
                if (orderStatusID == 6)
                {
                    if (StoreSettings.InventoryManagement)
                    {
                        // Update Stock Products
                        List<OrderDetailInfo> orderDetails = _orderController.GetOrderDetails(Order.OrderID);
                        foreach (OrderDetailInfo detail in orderDetails)
                            _orderController.UpdateStockQuantity(detail.ProductID, detail.Quantity);
                    }
                    // Remove User from Roles
                    _orderController.RemoveUserFromRoles(PortalId, Order);
                }

                // Send email
                if (chkSendEmailStatus.Checked)
                {
                    string commentToCustomer = txtCommentToCustomer.Text;
                    if (string.IsNullOrEmpty(commentToCustomer) == false)
                        Order.CommentToCustomer = commentToCustomer;
                    SendOrderStatusChangeEmail(Order);
                }

                // Redirect to order list
                _customerNav.OrderID = Null.NullInteger;
                Response.Redirect(_customerNav.GetNavigationUrl(), false);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            _customerNav.OrderID = Null.NullInteger;
            Response.Redirect(_customerNav.GetNavigationUrl(),  false);
        }

		#endregion

		#region Private Methods

        private string GetOrderStatus(int orderStatusID)
        {
            if (_orderStatusList == null)
                _orderStatusList = _orderController.GetOrderStatuses();

            string orderStatusText = "";
            foreach (OrderStatus orderStatus in _orderStatusList)
            {
                if (orderStatus.OrderStatusID == orderStatusID)
                {
                    orderStatusText = orderStatus.OrderStatusText;
                    break;
                }
            }
            return orderStatusText;
        }

        private void DisplayCustomerOrders()
        {
            // If the user is logged
            if (IsLogged)
            {
                List<OrderInfo> orders = _orderController.GetCustomerOrders(PortalId, _customerNav.CustomerID);

                if (orders.Count > 0)
                {
                    if (_orderStatusList == null)
                        _orderStatusList = _orderController.GetOrderStatuses();
                    grdOrders.DataSource = orders;
                    grdOrders.DataBind();
                }
                else
                {
                    lblError.Text = Localization.GetString("NoOrdersFound", LocalResourceFile);
                    pnlOrdersError.Visible = true;
                    pnlOrders.Visible = false;
                }
            }
            else
            {
                lblError.Text = Localization.GetString("UserNotLogged", LocalResourceFile);
                pnlOrdersError.Visible = true;
                pnlOrders.Visible = false;
            }
        }

        private void SendOrderStatusChangeEmail(OrderInfo order)
        {
            // Get Store Currency Symbol
            if (!string.IsNullOrEmpty(StoreSettings.CurrencySymbol))
                order.CurrencySymbol = StoreSettings.CurrencySymbol;

            // Get Order Date Format
            string orderDateFormat = Localization.GetString("OrderDateFormat", LocalResourceFile);
            if (!string.IsNullOrEmpty(orderDateFormat))
                order.DateFormat = orderDateFormat;

            // Get Customer Email
            IAddressInfo billingAddress = GetAddress(order.BillingAddressID, order.CustomerID);
            string customerEmail = billingAddress.Email;
            // Try to get email address from user account,
            // this could be required for older orders.
            if (string.IsNullOrEmpty(customerEmail))
            {
                UserController userControler = new UserController();
                UserInfo user = userControler.GetUser(PortalId, order.CustomerID);
                customerEmail = user.Email;
            }

            // Get Admin Email
            string adminEmail = StoreSettings.DefaultEmailAddress;

            // Customer Order Email Template
            string customerSubjectEmail = Localization.GetString("CustomerStatusChangedEmailSubject", LocalResourceFile);
            string customerBodyEmail = Localization.GetString("CustomerStatusChangedEmailBody", LocalResourceFile);

            // Extract or remove IFPAID token
            Match regPaidMatch = RegPaid.Match(customerBodyEmail);
            if (regPaidMatch.Success)
            {
                // If Status is Paid
                if (order.OrderStatusID == 7)
                {
                    // Replace IFPAID token by his content
                    string shippingCostTemplate = regPaidMatch.Groups[1].ToString();
                    shippingCostTemplate = RegStripStartNewLine.Replace(shippingCostTemplate, string.Empty);
                    shippingCostTemplate = RegStripEndNewLine.Replace(shippingCostTemplate, string.Empty);
                    customerBodyEmail = RegPaid.Replace(customerBodyEmail, shippingCostTemplate);
                }
                else // Remove IFPAID token
                    customerBodyEmail = RegPaid.Replace(customerBodyEmail, string.Empty);
            }

            // Admin Order Email Template
            string adminSubjectEmail = Localization.GetString("AdminStatusChangedEmailSubject", LocalResourceFile);
            string adminBodyEmail = Localization.GetString("AdminStatusChangedEmailBody", LocalResourceFile);

            // Init Email Order Replacement Tokens
            EmailOrderTokenReplace tkEmailOrder = new EmailOrderTokenReplace
            {
                StoreSettings = StoreSettings,
                Order = order
            };

            // Replace tokens
            customerSubjectEmail = tkEmailOrder.ReplaceEmailOrderTokens(customerSubjectEmail);
            customerBodyEmail = tkEmailOrder.ReplaceEmailOrderTokens(customerBodyEmail);
            adminSubjectEmail = tkEmailOrder.ReplaceEmailOrderTokens(adminSubjectEmail);
            adminBodyEmail = tkEmailOrder.ReplaceEmailOrderTokens(adminBodyEmail);

            try
            {
                // Send Customer Email
                Mail.SendMail(adminEmail, customerEmail, "", customerSubjectEmail, customerBodyEmail, "", HtmlUtils.IsHtml(customerBodyEmail) ? MailFormat.Html.ToString() : MailFormat.Text.ToString(), "", "", "", "");

                // Send Store Admin Email
                Mail.SendMail(adminEmail, adminEmail, "", adminSubjectEmail, adminBodyEmail, "", HtmlUtils.IsHtml(adminBodyEmail) ? MailFormat.Html.ToString() : MailFormat.Text.ToString(), "", "", "", "");
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

		private void ShowOrderDetails(int orderID)
		{
			// Get Order
			OrderInfo order = _orderController.GetOrder(PortalId, orderID);

            // If Order exist and current user is authorized to see it
            if (order != null && (order.CustomerID == UserId || _canManageOrders))
            {
                pnlOrderDetails.Visible = true;
                // Order Header
                lblOrderNumber.Text = order.OrderID.ToString();
                lblOrderDate.Text = order.OrderDate.ToString(_orderDateFormat);
                // Order Status
                if (_orderStatusList == null)
                    _orderStatusList = _orderController.GetOrderStatuses();
                string orderStatusText = "";
                foreach (OrderStatus orderStatus in _orderStatusList)
                {
                    if (orderStatus.OrderStatusID == order.OrderStatusID)
                    {
                        if (order.OrderStatusID > 1)
                            orderStatusText = orderStatus.OrderStatusText + " - " + order.ShipDate.ToString(_orderDateFormat);
                        else
                            orderStatusText = orderStatus.OrderStatusText;
                        break;
                    }
                }
                lblOrderStatus.Text = orderStatusText;
                //Cancel button
                btnCancel.Visible = false;
                if (StoreSettings.AuthorizeCancel && _canManageOrders == false)
                {
                    if (order.OrderStatusID == (int)OrderInfo.OrderStatusList.AwaitingPayment
                        || order.OrderStatusID == (int)OrderInfo.OrderStatusList.AwaitingStock
                        || order.OrderStatusID == (int)OrderInfo.OrderStatusList.Paid)
                    {
                        btnCancel.CommandArgument = order.OrderID.ToString();
                        btnCancel.Click += btnCancel_Click;
                        btnCancel.Visible = true;
                    }
                }
                //Pay Now button
                btnPayNow.Visible = false;
                if (StoreSettings.GatewayName != "EmailProvider")
                {
                    btnPayNow.Click += btnPayNow_Click;
                    if (order.OrderStatusID == (int)OrderInfo.OrderStatusList.AwaitingPayment)
                    {
                        btnPayNow.CommandArgument = order.OrderID.ToString();
                        btnPayNow.Click += btnPayNow_Click;
                        btnPayNow.Visible = true;
                    }
                }
                // Billing Address
                IAddressInfo billingAddress = GetAddress(order.BillingAddressID, order.CustomerID);
                if (billingAddress != null)
                    lblBillTo.Text = FormatAddress(billingAddress, _canManageOrders);
                else
                    lblBillTo.Text = string.Format(Localization.GetString("UserDeleted", LocalResourceFile), order.CustomerID);
                // ShippingAddress
                if (order.ShippingAddressID != order.BillingAddressID)
                {
                    if (order.ShippingAddressID != Null.NullInteger)
                    {
                        IAddressInfo shippingAddress = GetAddress(order.ShippingAddressID, order.CustomerID);
                        if (shippingAddress != null)
                            lblShipTo.Text = FormatAddress(shippingAddress, _canManageOrders);
                        else
                            lblShipTo.Text = Localization.GetString("ShipToAddressNotAvailable", LocalResourceFile);
                    }
                    else
                        lblShipTo.Text = Localization.GetString("NoShipping", LocalResourceFile);
                }
                else
                    lblShipTo.Text = Localization.GetString("SameAsBilling", LocalResourceFile);
                // Get Order Details
                List<OrderDetailInfo> orderDetails = _orderController.GetOrderDetails(orderID);
                // If Order detail was found, display it
                if (orderDetails != null)
                {
                    plhForm.Visible = true;

                    // Bind Items to GridControl
                    grdOrderDetails.DataSource = orderDetails;
                    grdOrderDetails.DataBind();
                }
                else
                {
                    // otherwise display an error message
                    lblError.Text = Localization.GetString("DetailsNotFound", LocalResourceFile);
                    pnlOrdersError.Visible = true;
                    plhForm.Visible = false;
                }

                // *** Footer ***

                // Total
                if (order.Discount != Null.NullDecimal || order.ShippingCost != 0 || order.TaxTotal != 0)
                    lblTotal.Text = order.OrderTotal.ToString("C", _localFormat);
                else
                    trTotal.Visible = false;
                // Discount
                if (order.Discount != Null.NullDecimal)
                    lblDiscount.Text = order.Discount.ToString("C", _localFormat);
                else
                    trDiscount.Visible = false;
                // Shipping Cost
                if (order.ShippingCost > 0)
                    lblPostage.Text = order.ShippingCost.ToString("C", _localFormat);
                else
                    trShipping.Visible = false;
                // Tax
                if (order.TaxTotal > 0)
                    lblTax.Text = order.TaxTotal.ToString("C", _localFormat);
                else
                    trTax.Visible = false;
                // Grand Total
                lblTotalIncPostage.Text = order.GrandTotal.ToString("C", _localFormat);

                // Status Management
                if (_canManageOrders)
                {
                    OrderStatusManagement.Visible = true;

                    // Bind Order status list...
                    ddlOrderStatus.DataSource = _orderStatusList;
                    ddlOrderStatus.DataTextField = "OrderStatusText";
                    ddlOrderStatus.DataValueField = "OrderStatusID";
                    ddlOrderStatus.DataBind();

                    // Select current status
                    ListItem currentStatus = ddlOrderStatus.Items.FindByValue(order.OrderStatusID.ToString());
                    if (currentStatus != null)
                        currentStatus.Selected = true;
                }
                else
                    OrderStatusManagement.Visible = false;
            }
            else
                plhForm.Visible = false;
		}

        private void CheckUserRoles()
        {
            if (UserInfo.IsSuperUser || UserInfo.IsInRole("Administrators"))
                _canManageOrders = true;
            else
            {
                if (StoreSettings.OrderRoleID != Null.NullInteger)
                {
                    RoleController roleController = new RoleController();
                    RoleInfo orderRole = roleController.GetRoleById(PortalId, StoreSettings.OrderRoleID);
                    if (UserInfo.IsInRole(orderRole.RoleName))
                        _canManageOrders = true;
                }
            }
        }

        private IAddressInfo GetAddress(int addressID, int customerID)
        {
            IAddressInfo address;

            if (addressID == 0)
                address = _addressProvider.GetRegistrationAddress(PortalId, customerID, "");
            else
                address = _addressProvider.GetAddress(addressID);

            return address;
        }

        private string FormatAddress(IAddressInfo address, Boolean showEmail)
        {
            string formatedAddress = string.Empty;

            if (address != null)
            {
                formatedAddress = address.Format(Localization.GetString("CustomerAddressTemplate", LocalResourceFile)).Replace("\r\n", "<br />");

                if (showEmail)
                {
                    string userEmail = string.Empty;

                    if (address.UserID != UserInfo.UserID)
                    {
                        if (address.UserID == StoreSettings.ImpersonatedUserID)
                            userEmail = address.Email;
                        else
                        {
                            UserController controller = new UserController();
                            UserInfo userInfo = controller.GetUser(PortalId, address.UserID);

                            if (userInfo != null)
                                userEmail = userInfo.Email;
                        }
                    }
                    else
                        userEmail = UserInfo.Email;

                    if (userEmail != string.Empty)
                        formatedAddress += "<br />" + string.Format("<a href=\"mailto:{0}\">{0}</a>", userEmail);
                }
            }

            return formatedAddress;
        }

        private void CancelOrder(int orderID)
        {
            OrderInfo order = _orderController.GetOrder(PortalId, orderID);

            if (_canManageOrders || order.CustomerID == UserId)
            {
                // Update Status Order
                order.OrderStatusID = (int)OrderInfo.OrderStatusList.Cancelled;
                order.Status = _orderController.GetStatusText(order.OrderStatusID);
                _orderController.UpdateOrder(order.OrderID, order.OrderDate, order.OrderNumber, order.ShippingAddressID, order.BillingAddressID, order.TaxTotal, order.ShippingCost, order.CouponID, order.Discount, true, order.OrderStatusID, order.CustomerID);

                if (StoreSettings.InventoryManagement)
                {
                    // Update Stock Products
                    List<OrderDetailInfo> orderDetails = _orderController.GetOrderDetails(order.OrderID);
                    foreach (OrderDetailInfo detail in orderDetails)
                        _orderController.UpdateStockQuantity(detail.ProductID, detail.Quantity);
                }

                // Remove User from Roles
                _orderController.RemoveUserFromRoles(PortalId, order);

                // Send email
                SendOrderStatusChangeEmail(order);
            }

            //Force a re-binding of the search grid
            _customerNav.OrderID = Null.NullInteger;
            Response.Redirect(_customerNav.GetNavigationUrl(), true);
        }

        private string CookieKey
        {
            get { return CookieName + PortalId; }
        }

        private void SetOrderIdCookie(int orderID)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieKey];
            if (cookie == null)
                cookie = new HttpCookie(CookieKey);
            string cookieValue;
            if (StoreSettings.SecureCookie && SymmetricHelper.CanSafelyEncrypt)
                cookieValue = SymmetricHelper.Encrypt(orderID.ToString());
            else
                cookieValue = orderID.ToString();
            cookie["OrderID"] = cookieValue;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

		#endregion

        #region IStoreTabedControl Members

        public string Title
        {
            get { return Localization.GetString("lblParentTitle", LocalResourceFile); }
        }

        #endregion
    }
}
