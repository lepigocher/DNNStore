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
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Log.EventLog;
using DotNetNuke.Services.Mail;

using DotNetNuke.Modules.Store.Core.Admin;
using DotNetNuke.Modules.Store.Core.Catalog;
using DotNetNuke.Modules.Store.Core.Components;
using DotNetNuke.Modules.Store.Core.Customer;
using DotNetNuke.Modules.Store.Core.Providers.Address;

namespace DotNetNuke.Modules.Store.Core.Cart
{
    /// <summary>
    /// Base class used by payment controls. Gateway providers have to inherit from this class.
    /// </summary>
    public class PaymentControlBase : PortalModuleBase
	{
		#region Private Members

	    private StoreInfo _storeSettings;
        private bool _onLinePayment;
        private string _moduleSharedResourceFile;
        private static readonly Regex RegDetails = new Regex(@"\[Details\](.+?)\[/Details\]", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex RegStripStartNewLine = new Regex(@"^\r\n", RegexOptions.Compiled);
        private static readonly Regex RegStripEndNewLine = new Regex(@"\r\n$", RegexOptions.Compiled);
	    private static readonly Regex RegDiscount = new Regex(@"\[IFDISCOUNT\](.+?)\[/IFDISCOUNT\]", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
	    private static readonly Regex RegShipping = new Regex(@"\[IFSHIPPING\](.+?)\[/IFSHIPPING\]", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
	    private static readonly Regex RegShippingCost = new Regex(@"\[IFSHIPPINGCOST\](.+?)\[/IFSHIPPINGCOST\]", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex RegPickup = new Regex(@"\[IFPICKUP\](.+?)\[/IFPICKUP\]", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex RegPaid = new Regex(@"\[IFPAID\](.+?)\[/IFPAID\]", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

		#endregion

		#region Properties/Events

        public event EventHandler AwaitingPayment;
        public event EventHandler PaymentRequiresConfirmation;
		public event EventHandler PaymentSucceeded;
		public event EventHandler PaymentCancelled;
		public event EventHandler PaymentFailed;

	    public ICheckoutControl CheckoutControl { get; set; }

	    public StoreInfo StoreSettings
        {
            get
            {
                if (_storeSettings == null)
                {
                    _storeSettings = StoreController.GetStoreInfo(PortalId);
                }
                return _storeSettings;
            }
            set { _storeSettings = value; }
        }

        public bool OnLinePayment
		{
            get { return _onLinePayment; }
            set { _onLinePayment = value; }
		}

	    public decimal SurchargeFixed
	    {
	        get
	        {
	            object value = Session["StorePayementControlSurchargeFixed"];
	            return value == null ? 0M : (decimal) value;
	        }
            protected set { Session["StorePayementControlSurchargeFixed"] = value; }
	    }

        public decimal SurchargePercent
	    {
	        get
            {
                object value = Session["StorePayementControlSurchargePercent"];
                return value == null ? 0M : (decimal)value;
	        }
            protected set { Session["StorePayementControlSurchargePercent"] = value; }
	    }

        public string ModuleSharedResourceFile
        {
            get { return _moduleSharedResourceFile; }
        }

		#endregion

		#region Protected Methods

        protected void InvokeAwaitingPayment()
        {
            AwaitingPayment?.Invoke(this, null);
        }

        protected void InvokePaymentRequiresConfirmation()
        {
            PaymentRequiresConfirmation?.Invoke(this, null);
        }

		protected void InvokePaymentSucceeded()
		{
            PaymentSucceeded?.Invoke(this, null);
        }

		protected void InvokePaymentCancelled()
		{
            PaymentCancelled?.Invoke(this, null);
        }

		protected void InvokePaymentFailed()
		{
            PaymentFailed?.Invoke(this, null);
        }

		protected virtual void GenerateOrderConfirmation()
		{
            OrderInfo order = CheckoutControl.Order;
            if (order != null)
            {
                // Store admin
                string adminDetailTemplate = string.Empty;
                bool adminDetail = false;
                // Customer
                string customerDetailTemplate = string.Empty;
                bool customerDetail = false;

                IAddressInfo billingAddress = CheckoutControl.BillingAddress;
                IAddressInfo shippingAddress = CheckoutControl.ShippingAddress;

                // Get Customer Email
                string customerEmail = billingAddress.Email;

                // Get Admin Email
                string adminEmail = StoreSettings.DefaultEmailAddress;

                // Get Store Currency Symbol
                if (!string.IsNullOrEmpty(StoreSettings.CurrencySymbol))
                    order.CurrencySymbol = StoreSettings.CurrencySymbol;

                // Get Order Date Format
                string orderDateFormat = Localization.GetString("OrderDateFormat", LocalResourceFile);
                if (string.IsNullOrEmpty(orderDateFormat) == false)
                    order.DateFormat = orderDateFormat;

                // Customer Order Email Template
                string customerSubjectEmail = Localization.GetString("CustomerOrderEmailSubject", LocalResourceFile);
                string customerBodyEmail = Localization.GetString("CustomerOrderEmailBody", LocalResourceFile);
                string customerAddressTemplate = Localization.GetString("CustomerAddressTemplate", LocalResourceFile);

                // Extract Detail Order from Customer EmailTemplate
                Match regCustomerMatch = RegDetails.Match(customerBodyEmail);
                if (regCustomerMatch.Success)
                {
                    customerDetail = true;
                    customerDetailTemplate = regCustomerMatch.Groups[1].ToString();
                    customerDetailTemplate = RegStripStartNewLine.Replace(customerDetailTemplate, string.Empty);
                    customerDetailTemplate = RegStripEndNewLine.Replace(customerDetailTemplate, string.Empty);
                }

                // Extract or remove IFDISCOUNT token
                Match regCustomerDiscountMatch = RegDiscount.Match(customerBodyEmail);
                if (regCustomerDiscountMatch.Success)
                {
                    if (order.CouponID != Null.NullInteger && order.Discount != Null.NullDecimal)
                    {
                        // Replace IFDISCOUNT token by his content
                        string discountTemplate = regCustomerDiscountMatch.Groups[1].ToString();
                        discountTemplate = RegStripStartNewLine.Replace(discountTemplate, string.Empty);
                        discountTemplate = RegStripEndNewLine.Replace(discountTemplate, string.Empty);
                        customerBodyEmail = RegDiscount.Replace(customerBodyEmail, discountTemplate);
                    }
                    else // Remove IFDISCOUNT token
                        customerBodyEmail = RegDiscount.Replace(customerBodyEmail, string.Empty);
                }

                // Extract or remove IFSHIPPINGCOST token
                Match regShippingCostMatch = RegShippingCost.Match(customerBodyEmail);
                if (regShippingCostMatch.Success)
                {
                    if (order.ShippingCost > 0)
                    {
                        // Replace IFSHIPPINGCOST token by his content
                        string shippingCostTemplate = regShippingCostMatch.Groups[1].ToString();
                        shippingCostTemplate = RegStripStartNewLine.Replace(shippingCostTemplate, string.Empty);
                        shippingCostTemplate = RegStripEndNewLine.Replace(shippingCostTemplate, string.Empty);
                        customerBodyEmail = RegShippingCost.Replace(customerBodyEmail, shippingCostTemplate);
                    }
                    else // Remove IFSHIPPINGCOST token
                        customerBodyEmail = RegShippingCost.Replace(customerBodyEmail, string.Empty);
                }

                // Replace BillingAddress token (if present) by the the formated Billing Address
                if (customerBodyEmail.IndexOf("[BillingAddress]", StringComparison.InvariantCultureIgnoreCase) != Null.NullInteger)
                    customerBodyEmail = customerBodyEmail.Replace("[BillingAddress]", billingAddress.Format(customerAddressTemplate));

                // Extract or remove Shipping Address Template from Customer Email Template
                if (shippingAddress.AddressID != Null.NullInteger)
                {
                    // Remove Pickup Template
                    customerBodyEmail = RegPickup.Replace(customerBodyEmail, string.Empty);
                    // Replace Shipping Address Template
                    Match regShippingMatch = RegShipping.Match(customerBodyEmail);
                    if (regShippingMatch.Success)
                    {
                        string shippingTemplate = regShippingMatch.Groups[1].ToString();
                        shippingTemplate = RegStripStartNewLine.Replace(shippingTemplate, string.Empty);
                        shippingTemplate = RegStripEndNewLine.Replace(shippingTemplate, string.Empty);
                        customerBodyEmail = RegShipping.Replace(customerBodyEmail, shippingTemplate);
                    }

                    // Replace ShippingAddress token (if present) by the the formated Shipping Address
                    if (customerBodyEmail.IndexOf("[ShippingAddress]", StringComparison.InvariantCultureIgnoreCase) != Null.NullInteger)
                        customerBodyEmail = customerBodyEmail.Replace("[ShippingAddress]", shippingAddress.Format(customerAddressTemplate));
                }
                else
                {
                    // Remove Shipping Address Template
                    customerBodyEmail = RegShipping.Replace(customerBodyEmail, string.Empty);
                    // Replace Pickup Template
                    Match regPickupMatch = RegPickup.Match(customerBodyEmail);
                    if (regPickupMatch.Success)
                    {
                        string pickupTemplate = regPickupMatch.Groups[1].ToString();
                        pickupTemplate = RegStripStartNewLine.Replace(pickupTemplate, string.Empty);
                        pickupTemplate = RegStripEndNewLine.Replace(pickupTemplate, string.Empty);
                        customerBodyEmail = RegPickup.Replace(customerBodyEmail, pickupTemplate);
                    }
                }

                // Admin Order Email Template
                string adminSubjectEmail = Localization.GetString("AdminOrderEmailSubject", LocalResourceFile);
                string adminBodyEmail = Localization.GetString("AdminOrderEmailBody", LocalResourceFile);

                // Extract Detail Order from Admin EmailTemplate
                Match regAdminMatch = RegDetails.Match(adminBodyEmail);
                if (regAdminMatch.Success)
                {
                    adminDetail = true;
                    adminDetailTemplate = regAdminMatch.Groups[1].ToString();
                    adminDetailTemplate = RegStripStartNewLine.Replace(adminDetailTemplate, string.Empty);
                    adminDetailTemplate = RegStripEndNewLine.Replace(adminDetailTemplate, string.Empty);
                }

                // Extract or remove IFDISCOUNT token
                Match regAdminDiscountMatch = RegDiscount.Match(adminBodyEmail);
                if (regAdminDiscountMatch.Success)
                {
                    if (order.CouponID != Null.NullInteger && order.Discount != Null.NullDecimal)
                    {
                        // Replace IFDISCOUNT token by his content
                        string discountTemplate = regAdminDiscountMatch.Groups[1].ToString();
                        discountTemplate = RegStripStartNewLine.Replace(discountTemplate, string.Empty);
                        discountTemplate = RegStripEndNewLine.Replace(discountTemplate, string.Empty);
                        adminBodyEmail = RegDiscount.Replace(adminBodyEmail, discountTemplate);
                    }
                    else // Remove IFDISCOUNT token
                        adminBodyEmail = RegDiscount.Replace(adminBodyEmail, string.Empty);
                }

                // Extract or remove IFSHIPPINGCOST token
                Match regAdminShippingCostMatch = RegShippingCost.Match(adminBodyEmail);
                if (regAdminShippingCostMatch.Success)
                {
                    if (order.ShippingCost > 0)
                    {
                        // Replace IFSHIPPINGCOST token by his content
                        string shippingCostTemplate = regAdminShippingCostMatch.Groups[1].ToString();
                        shippingCostTemplate = RegStripStartNewLine.Replace(shippingCostTemplate, string.Empty);
                        shippingCostTemplate = RegStripEndNewLine.Replace(shippingCostTemplate, string.Empty);
                        adminBodyEmail = RegShippingCost.Replace(adminBodyEmail, shippingCostTemplate);
                    }
                    else // Remove IFSHIPPINGCOST token
                        adminBodyEmail = RegShippingCost.Replace(adminBodyEmail, string.Empty);
                }

                // Replace BillingAddress token (if present) by the the formated Billing Address
                if (adminBodyEmail.IndexOf("[BillingAddress]", StringComparison.InvariantCultureIgnoreCase) != Null.NullInteger)
                    adminBodyEmail = adminBodyEmail.Replace("[BillingAddress]", billingAddress.Format(customerAddressTemplate));

                // Extract or remove Shipping Address Template from Admin Email Template
                if (shippingAddress.AddressID != Null.NullInteger)
                {
                    // Remove Pickup Template
                    adminBodyEmail = RegPickup.Replace(adminBodyEmail, string.Empty);
                    // Replace Shipping Address Template
                    Match regShippingMatch = RegShipping.Match(adminBodyEmail);
                    if (regShippingMatch.Success)
                    {
                        string shippingTemplate = regShippingMatch.Groups[1].ToString();
                        shippingTemplate = RegStripStartNewLine.Replace(shippingTemplate, string.Empty);
                        shippingTemplate = RegStripEndNewLine.Replace(shippingTemplate, string.Empty);
                        adminBodyEmail = RegShipping.Replace(adminBodyEmail, shippingTemplate);
                    }

                    // Replace ShippingAddress token (if present) by the the formated Shipping Address
                    if (adminBodyEmail.IndexOf("[ShippingAddress]", StringComparison.InvariantCultureIgnoreCase) != Null.NullInteger)
                        adminBodyEmail = adminBodyEmail.Replace("[ShippingAddress]", shippingAddress.Format(customerAddressTemplate));
                }
                else
                {
                    // Remove Shipping Address Template
                    adminBodyEmail = RegShipping.Replace(adminBodyEmail, string.Empty);
                    // Replace Pickup Template
                    Match regPickupMatch = RegPickup.Match(adminBodyEmail);
                    if (regPickupMatch.Success)
                    {
                        string pickupTemplate = regPickupMatch.Groups[1].ToString();
                        pickupTemplate = RegStripStartNewLine.Replace(pickupTemplate, string.Empty);
                        pickupTemplate = RegStripEndNewLine.Replace(pickupTemplate, string.Empty);
                        adminBodyEmail = RegShipping.Replace(adminBodyEmail, pickupTemplate);
                    }
                }

                // Init Email Order Replacement Tokens
                EmailOrderTokenReplace tkEmailOrder = new EmailOrderTokenReplace
                {
                    StoreSettings = StoreSettings,
                    Order = order,
                    BillingAddress = billingAddress,
                    ShippingAddress = shippingAddress
                };

                // Replace tokens
                customerSubjectEmail = tkEmailOrder.ReplaceEmailOrderTokens(customerSubjectEmail);
                customerBodyEmail = tkEmailOrder.ReplaceEmailOrderTokens(customerBodyEmail);
                adminSubjectEmail = tkEmailOrder.ReplaceEmailOrderTokens(adminSubjectEmail);
                adminBodyEmail = tkEmailOrder.ReplaceEmailOrderTokens(adminBodyEmail);

                // Order Details Template
                if (customerDetail || adminDetail)
                {
                    // Get Order Details
                    OrderController orderController = new OrderController();
                    List<OrderDetailInfo> orderDetails = orderController.GetOrderDetails(order.OrderID);
                    if (orderDetails != null)
                    {
                        // Update Stock Products if needed
                        if (StoreSettings.InventoryManagement)
                            DecreaseStock(orderDetails);

                        // Replace Order Detail Tokens
                        StringBuilder customerDetailText = new StringBuilder();
                        StringBuilder adminDetailText = new StringBuilder();
                        OrderDetailTokenReplace tkOrderDetail = new OrderDetailTokenReplace();

                        foreach (OrderDetailInfo detail in orderDetails)
                        {
                            tkOrderDetail.OrderDetail = detail;
                            if (customerDetail)
                                customerDetailText.AppendLine(tkOrderDetail.ReplaceOrderDetailTokens(customerDetailTemplate));
                            if (adminDetail)
                                adminDetailText.AppendLine(tkOrderDetail.ReplaceOrderDetailTokens(adminDetailTemplate));
                        }
                        if (customerDetail)
                            customerBodyEmail = RegDetails.Replace(customerBodyEmail, RegStripEndNewLine.Replace(customerDetailText.ToString(), string.Empty));
                        if (adminDetail)
                            adminBodyEmail = RegDetails.Replace(customerBodyEmail, RegStripEndNewLine.Replace(adminDetailText.ToString(), string.Empty));
                    }
                }

                try
                {
                    // Send Customer Email
                    string result = Mail.SendMail(adminEmail, customerEmail, "", customerSubjectEmail, customerBodyEmail, "", HtmlUtils.IsHtml(customerBodyEmail) ? MailFormat.Html.ToString() : MailFormat.Text.ToString(), "", "", "", "");
                    if (!string.IsNullOrEmpty(result))
                        LogSMTPError(customerEmail, result);

                    // Send Store Admin Email
                    result = Mail.SendMail(adminEmail, adminEmail, "", adminSubjectEmail, adminBodyEmail, "", HtmlUtils.IsHtml(adminBodyEmail) ? MailFormat.Html.ToString() : MailFormat.Text.ToString(), "", "", "", "");
                    if (!string.IsNullOrEmpty(result))
                        LogSMTPError(customerEmail, result);
                }
                catch (Exception ex)
                {
                    Exceptions.ProcessModuleLoadException(this, ex);
                }
            }
		}

        protected virtual OrderInfo UpdateOrderStatus(OrderInfo order, OrderInfo.OrderStatusList orderStatus)
        {
            // Update Status Order
            OrderController orderController = new OrderController();
            order.OrderStatusID = (int)orderStatus;
            order.Status = orderController.GetStatusText(order.OrderStatusID);
            orderController.UpdateOrder(order.OrderID, order.OrderDate, order.OrderNumber, order.ShippingAddressID, order.BillingAddressID, order.TaxTotal, order.ShippingCost, order.CouponID, order.Discount, true, order.OrderStatusID, order.CustomerID);

            // Send order status change email
            SendOrderStatusChangeEmail(order);

            return order;
        }

        protected virtual void AddEventLog(string logType, LogProperties properties, bool bypassBuffering)
        {
            LogInfo logInfo = new LogInfo
            {
                LogPortalID = PortalId,
                LogTypeKey = logType,
                LogProperties = properties,
                BypassBuffering = bypassBuffering
            };
            EventLogController logControler = new EventLogController();
            logControler.AddLog(logInfo);
        }

        protected virtual void SendEmailToAdmin(string subject, string body)
        {
            string storeEmail = StoreSettings.DefaultEmailAddress;
            string storeLink = "\r\n\r\n" + PortalAlias.HTTPAlias;
            Mail.SendMail(storeEmail, storeEmail, "", subject, body + storeLink, "", "text", "", "", "", "");
        }

        protected virtual string ScriptAvoidDoubleClick(Button confirmButton, string buttonText)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("if (typeof(Page_ClientValidate) == 'function') { ");
            sb.Append("var oldPage_IsValid = Page_IsValid; var oldPage_BlockSubmit = Page_BlockSubmit;");
            sb.Append("if (Page_ClientValidate('" + confirmButton.ValidationGroup + "') == false) {");
            sb.Append(" Page_IsValid = oldPage_IsValid; Page_BlockSubmit = oldPage_BlockSubmit; return false; }} ");
            if (string.IsNullOrEmpty(buttonText) == false)
                sb.Append("this.value = '" + buttonText + "';");
            sb.Append("this.disabled = true;");
            sb.Append(Page.ClientScript.GetPostBackEventReference(confirmButton, null) + ";");
            sb.Append("return true;");
            return sb.ToString();
        }

		#endregion

        #region PortalModuleBase Overrides

        protected override void OnInit(EventArgs e)
        {
            _moduleSharedResourceFile = ((StoreControlBase)CheckoutControl).LocalSharedResourceFile;
            Type type = GetType().BaseType;
            if (type != null)
                LocalResourceFile = Localization.GetResourceFile(this, type.Name + ".ascx");
            // Defaut OnLinePayment to True
            _onLinePayment = true;
            base.OnInit(e);
        }

        #endregion

        #region Private Methods

        private void DecreaseStock(IEnumerable<OrderDetailInfo> orderDetails)
        {
            OrderController orderController = new OrderController();
            foreach (OrderDetailInfo detail in orderDetails)
            {
                orderController.UpdateStockQuantity(detail.ProductID, -detail.Quantity);
            }
            ProductController productController = new ProductController();
            productController.ClearAllCaches();
        }

        private void IncreaseStock(IEnumerable<OrderDetailInfo> orderDetails)
        {
            OrderController orderController = new OrderController();
            foreach (OrderDetailInfo detail in orderDetails)
            {
                orderController.UpdateStockQuantity(detail.ProductID, detail.Quantity);
            }
            ProductController productController = new ProductController();
            productController.ClearAllCaches();
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
            string customerEmail = CheckoutControl.BillingAddress.Email;

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
                    string paidTemplate = regPaidMatch.Groups[1].ToString();
                    paidTemplate = RegStripStartNewLine.Replace(paidTemplate, string.Empty);
                    paidTemplate = RegStripEndNewLine.Replace(paidTemplate, string.Empty);
                    customerBodyEmail = RegPaid.Replace(customerBodyEmail, paidTemplate);
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
                string result = Mail.SendMail(adminEmail, customerEmail, "", customerSubjectEmail, customerBodyEmail, "", HtmlUtils.IsHtml(customerBodyEmail) ? MailFormat.Html.ToString() : MailFormat.Text.ToString(), "", "", "", "");
                if (!string.IsNullOrEmpty(result))
                    LogSMTPError(customerEmail, result);

                // Send Store Admin Email
                result = Mail.SendMail(adminEmail, adminEmail, "", adminSubjectEmail, adminBodyEmail, "", HtmlUtils.IsHtml(adminBodyEmail) ? MailFormat.Html.ToString() : MailFormat.Text.ToString(), "", "", "", "");
                if (!string.IsNullOrEmpty(result))
                    LogSMTPError(customerEmail, result);
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        private void LogSMTPError(string email, string message)
        {
            string smtpError = Localization.GetString("SMTPError", ModuleSharedResourceFile);
            string smtpMessage = Localization.GetString("SMTPMessage", ModuleSharedResourceFile);
            LogProperties properties = new LogProperties
            {
                new LogDetailInfo(smtpError, email),
                new LogDetailInfo(smtpMessage, message)
            };
            AddEventLog(EventLogController.EventLogType.ADMIN_ALERT.ToString(), properties, true);
        }

        #endregion
    }
}
