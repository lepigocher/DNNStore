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
using System.Collections.Generic;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Log.EventLog;
using DotNetNuke.Services.Mail;
using DotNetNuke.Modules.Store.Catalog;
using DotNetNuke.Modules.Store.Customer;

namespace DotNetNuke.Modules.Store.Cart
{
    public partial class SystempayIPN : IPNPageBase
    {
        #region Private Members

        private SystempaySettings _settings;
        private string _portalLanguage;
        private string _userLanguage;

        private enum PaymentStatus
        {
            Invalid,
            Unattended,
            Abandoned,
            Authorised,
            Canceled,
            Captured,
            Expired,
            Refused,
            Pending
        }

        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            Load += Page_Load;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _portalLanguage = PortalSettings.DefaultLanguage;
            string subject = Localization.GetString("StoreSystempayGateway", LocalResourceFile, _portalLanguage) + Localization.GetString("IPNInfo", LocalResourceFile, _portalLanguage); ;
            string reason = string.Empty;
            bool sendEmail = false;
            _settings = new SystempaySettings(StoreSettings.GatewaySettings);
            SystempayIPNParameters ipn = new SystempayIPNParameters(Request.Form, _settings.Certificate);
            // Verify payment with Systempay certificate
            PaymentStatus status = VerifyPayment(ipn);
            // What's the user language?
            _userLanguage = Request.QueryString["language"];
            switch (status)
            {
                case PaymentStatus.Abandoned:
                case PaymentStatus.Canceled:
                case PaymentStatus.Captured:
                    break;
                case PaymentStatus.Authorised:
                    int portalId = PortalSettings.PortalId;
                    // Set order status to "Paid"...
                    OrderInfo order = UpdateOrderStatus(ipn.vads_order_id, OrderInfo.OrderStatusList.Paid, _userLanguage);
                    // Add User to Product Roles
                    OrderController orderController = new OrderController();
                    orderController.AddUserToRoles(portalId, order);
                    // Add User to Order Role
                    StoreInfo storeSetting = StoreController.GetStoreInfo(portalId);
                    if (storeSetting.OnOrderPaidRoleID != Null.NullInteger)
                        orderController.AddUserToPaidOrderRole(portalId, order.CustomerID, storeSetting.OnOrderPaidRoleID);
                    // Special case request validation
                    if (ipn.vads_trans_status == "AUTHORISED_TO_VALIDATE")
                        sendEmail = true;
                    break;
                case PaymentStatus.Expired:
                case PaymentStatus.Refused:
                case PaymentStatus.Pending:
                    // Inform Store Admin
                    sendEmail = true;
                    break;
                case PaymentStatus.Unattended:
                    // Alert Store Admin
                    subject = Localization.GetString("StoreSystempayGateway", LocalResourceFile, _portalLanguage) + Localization.GetString("IPNAlert", LocalResourceFile, _portalLanguage);
                    sendEmail = true;
                    break;
                default:
                    break;
            }
            // Do we need to send an email to the store admin?
            if (sendEmail)
            {
                string paymentStatus = Localization.GetString("PaymentStatus_" + ipn.vads_trans_status, LocalResourceFile, _portalLanguage);
                if (string.IsNullOrEmpty(paymentStatus))
                    paymentStatus = ipn.vads_trans_status;
                string emailIPN = Localization.GetString("EmailIPN", LocalResourceFile, _portalLanguage);
                string body = string.Format(emailIPN, ipn.vads_order_id, ipn.vads_trans_id, ipn.vads_page_action, paymentStatus, ipn.vads_auth_result, ipn.vads_payment_error);
                SendEmailToAdmin(subject, body);
            }
        }

        #endregion

        #region Private Methods

        private PaymentStatus VerifyPayment(SystempayIPNParameters ipn)
        {
            bool restoreStock = false;
            PaymentStatus status = PaymentStatus.Invalid;
            // Default Alert Reason
            string alertReason = Localization.GetString("InvalidIPN", LocalResourceFile, _portalLanguage);

            // Security cheking: Validate signature with the current certificate
            if (ipn.IsValid)
            {
                // Security checking: is this request come from right Systempay account ID
                if (IsFromSite(ipn.vads_site_id))
                {
                    // Security checking: compares some Systempay fields with order fields
                    alertReason = Localization.GetString("WrongOrderInfos", LocalResourceFile, _portalLanguage);
                    OrderController orderController = new OrderController();
                    OrderInfo order = orderController.GetOrder(PortalSettings.PortalId, ipn.vads_order_id);
                    // If this order exist
                    if (order != null)
                    {
                        // Currency MUST BE the same!
                        if (_settings.Currency == ipn.vads_currency)
                        {
                            // Everything looks good, validate the transaction!
                            switch (ipn.vads_trans_status.ToLower())
                            {
                                case "abandoned":
                                    restoreStock = true;
                                    status = PaymentStatus.Abandoned;
                                    break;
                                case "authorised":
                                case "authorised_to_validate":
                                    // Grand Total MUST BE the same!
                                    if (Math.Round(order.GrandTotal, 2, MidpointRounding.AwayFromZero) == ipn.vads_amount)
                                        status = PaymentStatus.Authorised;
                                    break;
                                case "canceled":
                                    restoreStock = true;
                                    status = PaymentStatus.Canceled;
                                    break;
                                case "captured":
                                    // Grand Total MUST BE the same!
                                    if (Math.Round(order.GrandTotal, 2, MidpointRounding.AwayFromZero) == ipn.vads_amount)
                                        status = PaymentStatus.Captured;
                                    break;
                                case "expired":
                                    restoreStock = true;
                                    status = PaymentStatus.Expired;
                                    break;
                                case "refused":
                                    restoreStock = true;
                                    status = PaymentStatus.Refused;
                                    break;
                                case "under_verification":
                                case "waiting_authorisation":
                                case "waiting_authorisation_to_validate":
                                    // Grand Total MUST BE the same!
                                    if (Math.Round(order.GrandTotal, 2, MidpointRounding.AwayFromZero) == ipn.vads_amount)
                                        status = PaymentStatus.Pending;
                                    break;
                                default:
                                    status = PaymentStatus.Unattended;
                                    break;
                            }

                            if (restoreStock)
                            {
                                List<OrderDetailInfo> orderDetails = orderController.GetOrderDetails(order.OrderID);
                                if (orderDetails != null)
                                {
                                    foreach (OrderDetailInfo detail in orderDetails)
                                    {
                                        orderController.UpdateStockQuantity(detail.ProductID, detail.Quantity);
                                    }
                                    ProductController productController = new ProductController();
                                    productController.ClearAllCaches();
                                }
                            }
                        }
                    }
                }
                else
                    alertReason = Localization.GetString("DifferentReceiverEmail", LocalResourceFile, _portalLanguage);
            }

            // If the transaction is invalid
            if ((status == PaymentStatus.Invalid) || (status == PaymentStatus.Unattended))
            {
                // Add an Admin Alert to the DNN Log
                string systempayGateway = Localization.GetString("StoreSystempayGateway", LocalResourceFile, _portalLanguage);
                string adminAlert = Localization.GetString("SecurityAlert", LocalResourceFile, _portalLanguage);
                LogProperties properties = new LogProperties();
                properties.Add(new LogDetailInfo(systempayGateway, adminAlert));
                properties.Add(new LogDetailInfo(Localization.GetString("AlertReason", LocalResourceFile, _portalLanguage), alertReason));
                properties.Add(new LogDetailInfo(Localization.GetString("FromIP", LocalResourceFile, _portalLanguage), Request.UserHostAddress));
                properties.Add(new LogDetailInfo(Localization.GetString("IPNPayload", LocalResourceFile, _portalLanguage), ipn.Payload));
                AddEventLog(EventLogController.EventLogType.ADMIN_ALERT.ToString(), properties, true);
                // Send an email to the store admin
                SendEmailToAdmin(systempayGateway + " " + adminAlert, Localization.GetString("EmailAlert", LocalResourceFile, _portalLanguage) + "\r\n\r\n" + alertReason);
            }

            return status;
        }

        private bool IsFromSite(string vads_site_id)
        {
            // vads_site_id MUST BE equal to the Systempay account ID
            return string.Compare(vads_site_id, _settings.SiteID, true) == 0;
        }
        
        private void SendEmailToAdmin(string subject, string body)
        {
            string storeEmail = StoreSettings.DefaultEmailAddress;
            string storeLink = "\r\n\r\n" + Request.Url.GetLeftPart(UriPartial.Authority);
            Mail.SendMail(storeEmail, storeEmail, "", subject, body + storeLink, "", "text", "", "", "", "");
        }

        #endregion
    }
}
