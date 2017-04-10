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
using System.Text.RegularExpressions;

using DotNetNuke.Framework;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Log.EventLog;
using DotNetNuke.Services.Mail;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Customer;
using DotNetNuke.Modules.Store.Providers.Address;

namespace DotNetNuke.Modules.Store.Cart
{
    /// <summary>
    /// Page base class used by gateways when an IPN page is required.
    /// </summary>
    public abstract class IPNPageBase : PageBase
    {
        private StoreInfo _storeSettings;
        private static readonly Regex RegStripStartNewLine = new Regex(@"^\r\n", RegexOptions.Compiled);
        private static readonly Regex RegStripEndNewLine = new Regex(@"\r\n$", RegexOptions.Compiled);
        private static readonly Regex RegPaid = new Regex(@"\[IFPAID\](.+?)\[/IFPAID\]", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public StoreInfo StoreSettings
        {
            get
            {
                if (_storeSettings == null)
                    _storeSettings = StoreController.GetStoreInfo(PortalSettings.PortalId);
                return _storeSettings;
            }
        }

        protected virtual OrderInfo UpdateOrderStatus(int orderID, OrderInfo.OrderStatusList orderStatus, string language)
        {
            // Update order status
            OrderController orderController = new OrderController();
            OrderInfo order = orderController.GetOrder(PortalSettings.PortalId, orderID);
            order.OrderStatusID = (int)orderStatus;
            order.Status = orderController.GetStatusText(order.OrderStatusID);
            orderController.UpdateOrder(order.OrderID, order.OrderDate, order.OrderNumber, order.ShippingAddressID, order.BillingAddressID, order.TaxTotal, order.ShippingCost, order.CouponID, order.Discount, true, order.OrderStatusID, order.CustomerID);

            // Send order status change email
            SendOrderStatusChangeEmail(order);

            return order;
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
            IAddressProvider controler = StoreController.GetAddressProvider(StoreSettings.AddressName);
            IAddressInfo billingAddress = controler.GetAddress(order.BillingAddressID);
            string customerEmail = billingAddress.Email;

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
            EmailOrderTokenReplace tkEmailOrder = new EmailOrderTokenReplace();
            tkEmailOrder.StoreSettings = StoreSettings;
            tkEmailOrder.Order = order;

            // Replace tokens
            customerSubjectEmail = tkEmailOrder.ReplaceEmailOrderTokens(customerSubjectEmail);
            customerBodyEmail = tkEmailOrder.ReplaceEmailOrderTokens(customerBodyEmail);
            adminSubjectEmail = tkEmailOrder.ReplaceEmailOrderTokens(adminSubjectEmail);
            adminBodyEmail = tkEmailOrder.ReplaceEmailOrderTokens(adminBodyEmail);

            try
            {
                // Send Customer Email
                string result = Mail.SendMail(adminEmail, customerEmail, "", customerSubjectEmail, customerBodyEmail, "", Mail.IsHTMLMail(customerBodyEmail) ? MailFormat.Html.ToString() : MailFormat.Text.ToString(), "", "", "", "");
                if (!string.IsNullOrEmpty(result))
                    LogSMTPError(customerEmail, result);

                // Send Store Admin Email
                result = Mail.SendMail(adminEmail, adminEmail, "", adminSubjectEmail, adminBodyEmail, "", Mail.IsHTMLMail(adminBodyEmail) ? MailFormat.Html.ToString() : MailFormat.Text.ToString(), "", "", "", "");
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
            string smtpError = Localization.GetString("SMTPError", LocalResourceFile);
            string smtpMessage = Localization.GetString("SMTPMessage", LocalResourceFile);
            LogProperties properties = new LogProperties();
            properties.Add(new LogDetailInfo(smtpError, email));
            properties.Add(new LogDetailInfo(smtpMessage, message));
            AddEventLog(EventLogController.EventLogType.ADMIN_ALERT.ToString(), properties, true);
        }

        protected virtual void AddEventLog(string logType, LogProperties properties, bool bypassBuffering)
        {
            LogInfo logInfo = new LogInfo();
            logInfo.LogPortalID = PortalSettings.PortalId;
            logInfo.LogTypeKey = logType;
            logInfo.LogProperties = properties;
            logInfo.BypassBuffering = bypassBuffering;
            EventLogController logControler = new EventLogController();
            logControler.AddLog(logInfo);
        }
    }
}
