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
    /// Summary description for SystempayPayment.
	/// </summary>
	public partial class SystempayPayment : PaymentControlBase
	{
		#region Private Members

        private const string CookieName = "DotNetNuke_Store_Portal_";

		#endregion

		#region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            // Do we have any special handling?
            SystempayNavigation nav = new SystempayNavigation(Request.QueryString);
            switch (nav.GatewayExit.ToUpper())
            {
                case "CANCEL":
                {
                    InvokePaymentCancelled();
                    CheckoutControl.Hide();
                    pnlProceedToSystempay.Visible = false;
                    return;
                }
                case "ERROR":
                case "REFUSED":
                {
                    InvokePaymentFailed();
                    CheckoutControl.Hide();
                    pnlProceedToSystempay.Visible = false;
                    return;
                }
                case "RETURN":
                {
                    SystempaySettings settings = new SystempaySettings(StoreSettings.GatewaySettings);
                    SystempayIPNParameters ipn = new SystempayIPNParameters(Request.QueryString, settings.Certificate);
                    // Here there is no check about the validity of the Systempay response (IPN),
                    // because it's just a message displayed to the customer.
                    // Everything is checked in the NOTIFY case received from Systempay in the SystempayIPN.aspx page.
                    switch (ipn.vads_trans_status.ToLower())
                    {
                        case "authorised":
                        case "authorised_to_validate":
                            InvokePaymentSucceeded();
                            break;
                        default:
                            InvokePaymentRequiresConfirmation();
                            break;
                    }
                    CheckoutControl.Hide();
                    pnlProceedToSystempay.Visible = false;
                    return;
                }
            }

            if (nav.GatewayExit.Length > 0)
            {
                //If the SystempayExit is anything else with length > 0, then don't do any processing
                HttpContext.Current.Response.Redirect(Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID), false);
                return;
            }

            // Continue with display of payment control...
            if (Page.IsPostBack == false)
            {
                SystempaySettings settings = new SystempaySettings(StoreSettings.GatewaySettings);
                if (!settings.IsValid())
                {
                    lblError.Text = Localization.GetString("GatewayNotConfigured", LocalResourceFile);
                    lblError.Visible = true;
                    pnlProceedToSystempay.Visible = false;
                    return;
                }

                btnConfirmOrder.Attributes.Add("OnClick", ScriptAvoidDoubleClick(btnConfirmOrder, Localization.GetString("Processing", this.LocalResourceFile)));
                string message = Localization.GetString("lblConfirmMessage", LocalResourceFile);
                lblConfirmMessage.Text = string.Format(message, PortalSettings.PortalName);
                message = Localization.GetString("systempayimage", LocalResourceFile);
                systempayimage.AlternateText = message;
                systempayimage.ImageUrl = settings.ButtonURL;

                lblError.Text = string.Empty;
                lblError.Visible = false;
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
            pnlProceedToSystempay.Visible = false;

            // Set order status to "Awaiting Payment"...
            CheckoutControl.Order = UpdateOrderStatus(order, OrderInfo.OrderStatusList.AwaitingPayment);

            // Clear basket
            CurrentCart.DeleteCart(PortalId, StoreSettings.SecureCookie);

            // Clear cookies
            ClearOrderIdCookie();

            // Process transaction
            string urlAuthority = Request.Url.GetLeftPart(UriPartial.Authority);
            TransactionDetails transaction = new TransactionDetails();
            SystempayNavigation nav = new SystempayNavigation(Request.QueryString)
            {
                OrderID = order.OrderID,
                // Return URL
                GatewayExit = "return"
            };
            transaction.ReturnURL = AddAuthority(nav.GetNavigationUrl(), urlAuthority);
            // Refused URL
            nav.GatewayExit = "refused";
            transaction.RefusedURL = AddAuthority(nav.GetNavigationUrl(), urlAuthority);
            // Error URL
            nav.GatewayExit = "error";
            transaction.ErrorURL = AddAuthority(nav.GetNavigationUrl(), urlAuthority);
            // Cancel URL
            nav.GatewayExit = "cancel";
            transaction.CancelURL = AddAuthority(nav.GetNavigationUrl(), urlAuthority);
            // IPN URL
            string language = Request.QueryString["language"];
            if (string.IsNullOrEmpty(language))
                language = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();
            transaction.NotifyURL = urlAuthority + TemplateSourceDirectory + "/SystempayIPN.aspx?language=" + language;
            string messages = Localization.GetString("SystempayButtons", LocalResourceFile);
            transaction.Buttons = string.Format(messages, StoreSettings.Name);
            transaction.ShopName = StoreSettings.Name;
            transaction.Email = billingAddress.Email;

            SystempayGatewayProvider provider = new SystempayGatewayProvider(StoreSettings.GatewaySettings);
            provider.ProcessTransaction(CheckoutControl.BillingAddress, order, transaction);
        }

        private string AddAuthority(string url, string urlAuthority)
        {
            if (url.StartsWith(urlAuthority) == false)
                return urlAuthority + url;
            else
                return url;
        }

		#endregion
	}
}
