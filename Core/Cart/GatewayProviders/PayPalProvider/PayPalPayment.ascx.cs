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

using DotNetNuke.Services.Localization;

using DotNetNuke.Modules.Store.Core.Customer;
using DotNetNuke.Modules.Store.Core.Providers.Address;

namespace DotNetNuke.Modules.Store.Core.Cart
{
	/// <summary>
    /// Summary description for PayPalPayment.
	/// </summary>
	public partial class PayPalPayment : PaymentControlBase
	{
		#region Private Members

        private const string CookieName = "DotNetNuke_Store_Portal_";

		#endregion

		#region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            // Do we have any special handling?
            PayPalNavigation nav = new PayPalNavigation(Request.QueryString);
            switch (nav.GatewayExit.ToUpper())
            {
                case "CANCEL":
                {
                    InvokePaymentCancelled();
                    CheckoutControl.Hide();
                    pnlProceedToPayPal.Visible = false;
                    return;
                }
                case "RETURN":
                {
                    PayPalIPNParameters ipn = new PayPalIPNParameters(Request.Form, Request.BinaryRead(Request.ContentLength));
                    // Here there is no check about the validity of the PayPal response (IPN),
                    // because it's just a message displayed to the customer.
                    // Everything is checked in the NOTIFY case received from PayPal in the PayPalIPN.aspx page.
                    switch (ipn.payment_status.ToLower())
                    {
                        case "completed":
                            InvokePaymentSucceeded();
                            break;
                        default:
                            InvokePaymentRequiresConfirmation();
                            break;
                    }
                    CheckoutControl.Hide();
                    pnlProceedToPayPal.Visible = false;
                    return;
                }
            }

            if (nav.GatewayExit.Length > 0)
            {
                //If the PayPalExit is anything else with length > 0, then don't do any processing
                HttpContext.Current.Response.Redirect(Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID), false);
                return;
            }

            // Continue with display of payment control...
            if (Page.IsPostBack == false)
            {
                PayPalSettings settings = new PayPalSettings(StoreSettings.GatewaySettings);
                if (!settings.IsValid())
                {
                    lblError.Text = Localization.GetString("GatewayNotConfigured", LocalResourceFile);
                    lblError.Visible = true;
                    pnlProceedToPayPal.Visible = false;
                    return;
                }

                SurchargePercent = settings.SurchargePercent;
                SurchargeFixed = settings.SurchargeFixed;

                btnConfirmOrder.Attributes.Add("OnClick", ScriptAvoidDoubleClick(btnConfirmOrder, Localization.GetString("Processing", this.LocalResourceFile)));
                string message = Localization.GetString("lblConfirmMessage", LocalResourceFile);
                lblConfirmMessage.Text = string.Format(message, PortalSettings.PortalName);
                message = Localization.GetString("paypalimage", LocalResourceFile);
                paypalimage.AlternateText = message;

                lblError.Text = string.Empty;
                lblError.Visible = false;
                paypalimage.ImageUrl = settings.ButtonURL;
            }
        }

        protected void btnConfirmOrder_Click(object sender, EventArgs e)
        {
            ConfirmOrder();
        }

		#endregion

		#region Private Methods

        private void ClearOrderIdCookie()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieKey];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Today.AddDays(-100);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        private string CookieKey
        {
            get { return CookieName + PortalId; }
        }

        private void ConfirmOrder()
        {
            Page.Validate();
            if (Page.IsValid == false)
                return;

            // Adds order to db...
            OrderInfo order = CheckoutControl.GetFinalizedOrderInfo();
            IAddressInfo billingAddress = CheckoutControl.BillingAddress;

            GenerateOrderConfirmation();

            CheckoutControl.Hide();
            pnlProceedToPayPal.Visible = false;

            // Set order status to "Awaiting Payment"...
            CheckoutControl.Order = UpdateOrderStatus(order, OrderInfo.OrderStatusList.AwaitingPayment);

            // Clear basket
            CurrentCart.DeleteCart(PortalId, StoreSettings.SecureCookie);

            // Clear cookies
            ClearOrderIdCookie();

            // Process transaction
            string url;
            string urlAuthority = Request.Url.GetLeftPart(UriPartial.Authority);
            TransactionDetails transaction = new TransactionDetails();
            PayPalNavigation nav = new PayPalNavigation(Request.QueryString)
            {
                OrderID = order.OrderID,
                GatewayExit = "return"
            };
            url = nav.GetNavigationUrl();
            if (url.StartsWith(urlAuthority) == false)
                url = urlAuthority + url;
            transaction.ReturnURL = url;
            nav.GatewayExit = "cancel";
            url = nav.GetNavigationUrl();
            if (url.StartsWith(urlAuthority) == false)
                url = urlAuthority + url;
            transaction.CancelURL = url;
            string language = Request.QueryString["language"];
            if (string.IsNullOrEmpty(language))
                language = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();
            transaction.NotifyURL = urlAuthority + TemplateSourceDirectory + "/PayPalIPN.aspx?language=" + language;
            string message = Localization.GetString("PayPalReturnTo", LocalResourceFile);
            transaction.Cbt = string.Format(message, PortalSettings.PortalName);
            transaction.Email = billingAddress.Email;

            PayPalGatewayProvider provider = new PayPalGatewayProvider(StoreSettings.GatewaySettings);
            provider.ProcessTransaction(CheckoutControl.BillingAddress, order, transaction);
        }

		#endregion
	}
}
