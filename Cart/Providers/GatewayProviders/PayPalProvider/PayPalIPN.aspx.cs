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
using System.IO;
using System.Net;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Log.EventLog;
using DotNetNuke.Services.Mail;
using DotNetNuke.Modules.Store.Customer;

namespace DotNetNuke.Modules.Store.Cart
{
    public partial class PayPalIPN : IPNPageBase
    {
        #region Private Members

        private const string SandboxVerificationURL = "https://www.sandbox.paypal.com/cgi-bin/webscr/";
        private PayPalSettings _settings;
        private string _portalLanguage;
        private string _userLanguage;
        private string _verificationURL = string.Empty;

        private enum PaymentStatus
        {
            Payed,
            Pending,
            Refunded,
            Reversed,
            Unattended,
            Invalid
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
            string subject = string.Empty;
            string reason = string.Empty;
            bool sendEmail = false;
            PayPalIPNParameters ipn = new PayPalIPNParameters(Request.Form, Request.BinaryRead(Request.ContentLength));
            _settings = new PayPalSettings(StoreSettings.GatewaySettings);
            _verificationURL = _settings.UseSandbox ? SandboxVerificationURL : _settings.VerificationURL;
            // Verify payment with PayPal
            PaymentStatus status = VerifyPayment(ipn);
            // What's the user language?
            _userLanguage = Request.QueryString["language"];
            switch (status)
            {
                case PaymentStatus.Payed:
                    int portalId = PortalSettings.PortalId;
                    // Set order status to "Paid"...
                    OrderInfo order = UpdateOrderStatus(ipn.invoice, OrderInfo.OrderStatusList.Paid, _userLanguage);
                    // Add User to Product Roles
                    OrderController orderController = new OrderController();
                    orderController.AddUserToRoles(portalId, order);
                    // Add User to Order Role
                    StoreInfo storeSetting = StoreController.GetStoreInfo(portalId);
                    if (storeSetting.OnOrderPaidRoleID != Null.NullInteger)
                        orderController.AddUserToPaidOrderRole(portalId, order.CustomerID, storeSetting.OnOrderPaidRoleID);
                    break;
                case PaymentStatus.Pending:
                    // Inform Store Admin
                    subject = Localization.GetString("StorePayPalGateway", LocalResourceFile, _portalLanguage) + Localization.GetString("IPNInfo", LocalResourceFile, _portalLanguage);
                    reason = Localization.GetString("PendingReason_" + ipn.pending_reason, LocalResourceFile, _portalLanguage);
                    sendEmail = true;
                    break;
                case PaymentStatus.Refunded:
                    // Inform Store Admin
                    subject = Localization.GetString("StorePayPalGateway", LocalResourceFile, _portalLanguage) + Localization.GetString("IPNInfo", LocalResourceFile, _portalLanguage);
                    reason = Localization.GetString("ReasonCode_" + ipn.reason_code, LocalResourceFile, _portalLanguage);
                    sendEmail = true;
                    break;
                case PaymentStatus.Reversed:
                    // Inform Store Admin
                    subject = Localization.GetString("StorePayPalGateway", LocalResourceFile, _portalLanguage) + Localization.GetString("IPNInfo", LocalResourceFile, _portalLanguage);
                    reason = Localization.GetString("ReasonCode_" + ipn.reason_code, LocalResourceFile, _portalLanguage);
                    sendEmail = true;
                    break;
                case PaymentStatus.Unattended:
                    // Alert Store Admin
                    subject = Localization.GetString("StorePayPalGateway", LocalResourceFile, _portalLanguage) + Localization.GetString("IPNAlert", LocalResourceFile, _portalLanguage);
                    reason = Localization.GetString("ReasonCode_" + ipn.reason_code, LocalResourceFile, _portalLanguage);
                    sendEmail = true;
                    break;
                default:
                    break;
            }
            // Do we need to send an email to the store admin?
            if (sendEmail)
            {
                if (string.IsNullOrEmpty(reason))
                    reason = ipn.reason_code;
                string paymentType = Localization.GetString("PaymentType_" + ipn.payment_type, LocalResourceFile, _portalLanguage);
                if (string.IsNullOrEmpty(paymentType))
                    paymentType = ipn.payment_type;
                string paymentStatus = Localization.GetString("PaymentStatus_" + ipn.payment_status, LocalResourceFile, _portalLanguage);
                if (string.IsNullOrEmpty(paymentStatus))
                    paymentStatus = ipn.payment_status;
                string emailIPN = Localization.GetString("EmailIPN", LocalResourceFile, _portalLanguage);
                string body = string.Format(emailIPN, ipn.invoice, ipn.txn_id, paymentType, paymentStatus, reason);
                SendEmailToAdmin(subject, body);
            }
        }

        #endregion

        #region Private Methods

        private PaymentStatus VerifyPayment(PayPalIPNParameters ipn)
        {
            PaymentStatus status = PaymentStatus.Invalid;
            // Default Alert Reason
            string alertReason = Localization.GetString("InvalidIPN", LocalResourceFile, _portalLanguage);

            if (ipn.IsValid)
            {
                // Process notification validation
                HttpWebRequest request = WebRequest.Create(_verificationURL) as HttpWebRequest;
                if (request != null)
                {
                    request.Method = "POST";
                    request.ContentLength = ipn.PostString.Length;
                    request.ContentType = "application/x-www-form-urlencoded";

                    using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                    {
                        writer.Write(ipn.PostString);
                        writer.Close();
                    }

                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    if (response != null)
                    {
                        string responseString;
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            responseString = reader.ReadToEnd();
                            reader.Close();
                        }

                        // If the transaction is VERIFIED by PayPal
                        if (string.Compare(responseString, "VERIFIED", true) == 0)
                        {
                            // Security checking: is this request come from right PayPal account ID
                            if (IsFromReceiver(ipn))
                            {
                                // Security checking: compares some PayPal fields with order fields
                                alertReason = Localization.GetString("WrongOrderInfos", LocalResourceFile, _portalLanguage);
                                OrderController orderController = new OrderController();
                                OrderInfo order = orderController.GetOrder(PortalSettings.PortalId, ipn.invoice);
                                // If this order exist
                                if (order != null)
                                {
                                    // Currency MUST BE the same!
                                    if (_settings.Currency == ipn.mc_currency)
                                    {
                                        // Everything looks good, validate the transaction!
                                        switch (ipn.payment_status.ToLower())
                                        {
                                            case "completed":
                                                // Grand Total MUST BE the same!
                                                if (Math.Round(order.GrandTotal, 2, MidpointRounding.AwayFromZero) == ipn.mc_gross)
                                                    status = PaymentStatus.Payed;
                                                break;
                                            case "pending":
                                            case "in-progress":
                                                // Grand Total MUST BE the same!
                                                if (Math.Round(order.GrandTotal, 2, MidpointRounding.AwayFromZero) == ipn.mc_gross)
                                                    status = PaymentStatus.Pending;
                                                break;
                                            case "refunded":
                                                status = PaymentStatus.Refunded;
                                                break;
                                            case "reversed":
                                                status = PaymentStatus.Reversed;
                                                break;
                                            default:
                                                status = PaymentStatus.Unattended;
                                                break;
                                        }
                                    }
                                }
                            }
                            else
                                alertReason = Localization.GetString("DifferentReceiverEmail", LocalResourceFile, _portalLanguage);
                        }
                        else
                            alertReason = Localization.GetString("VerificationFailed", LocalResourceFile, _portalLanguage);
                    }
                }
            }

            // If the transaction is invalid
            if ((status == PaymentStatus.Invalid) || (status == PaymentStatus.Unattended))
            {
                // Add an Admin Alert to the DNN Log
                string paypalGateway = Localization.GetString("StorePayPalGateway", LocalResourceFile, _portalLanguage);
                string adminAlert = Localization.GetString("SecurityAlert", LocalResourceFile, _portalLanguage);
                LogProperties properties = new LogProperties();
                properties.Add(new LogDetailInfo(paypalGateway, adminAlert));
                properties.Add(new LogDetailInfo(Localization.GetString("AlertReason", LocalResourceFile, _portalLanguage), alertReason));
                properties.Add(new LogDetailInfo(Localization.GetString("FromIP", LocalResourceFile, _portalLanguage), Request.UserHostAddress));
                properties.Add(new LogDetailInfo(Localization.GetString("IPNPOSTString", LocalResourceFile, _portalLanguage), ipn.PostString));
                AddEventLog(EventLogController.EventLogType.ADMIN_ALERT.ToString(), properties, true);
                // Send an email to the store admin
                SendEmailToAdmin(paypalGateway + " " + adminAlert, Localization.GetString("EmailAlert", LocalResourceFile, _portalLanguage) + "\r\n\r\n" + alertReason);
            }

            return status;
        }

        private bool IsFromReceiver(PayPalIPNParameters ipn)
        {
            // Receiver_email MUST BE equal to the PayPal account ID
            bool verified = string.Compare(ipn.receiver_email, _settings.PayPalID, true) == 0;
            // If Secure Merchand ID is present and equal to the receiver ID
            if (verified && !string.IsNullOrEmpty(_settings.SecureID))
                verified = verified & (string.Compare(_settings.SecureID, ipn.receiver_id, true) == 0);
            return verified;
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
