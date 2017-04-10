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
using System.Web.UI.WebControls;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Security;
using DotNetNuke.Modules.Store.Providers.Address;
using DotNetNuke.Modules.Store.Customer;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for AuthNetCheckout.
	/// </summary>
    public partial class AuthorizeNetPayment : PaymentControlBase
	{
        #region Private Members

        private const string CookieName = "DotNetNuke_Store_Portal_";

        #endregion

		#region Events

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!Request.IsSecureConnection)
            {
                AuthNetSettings settings = new AuthNetSettings(StoreSettings.GatewaySettings);
                if (!settings.IsTest)
                    throw new ApplicationException(Localization.GetString("ErrorNotSecured", LocalResourceFile));
            }

			if(!Page.IsPostBack)
			{
                btnProcess.Attributes.Add("OnClick", ScriptAvoidDoubleClick(btnProcess, Localization.GetString("Processing", LocalResourceFile)));
                string message = Localization.GetString("lblConfirmMessage", LocalResourceFile);
                lblConfirmMessage.Text = string.Format(message, PortalSettings.PortalName);
                for (int i = DateTime.Now.Year; i < DateTime.Now.Year + 10; i++)
					ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlMonth.Items.FindByValue(DateTime.Now.Month.ToString("00")).Selected = true;
			}
		}

		protected void btnProcess_Click(object sender, EventArgs e)
		{
            Page.Validate();
            if (!Page.IsValid)
                return;

			PortalSecurity security = new PortalSecurity();
	
			TransactionDetails transaction = new TransactionDetails();
			transaction.CardNumber = security.InputFilter(txtNumber.Text, PortalSecurity.FilterFlag.NoMarkup);
			transaction.VerificationCode = security.InputFilter(txtVer.Text, PortalSecurity.FilterFlag.NoMarkup);
			transaction.ExpirationMonth = int.Parse(ddlMonth.SelectedValue);
			transaction.ExpirationYear = int.Parse(ddlYear.SelectedValue);

			if (transaction.IsValid())
			{
			    IAddressInfo shippingAddress = CheckoutControl.ShippingAddress;
			    IAddressInfo billingAddress = CheckoutControl.BillingAddress;
                //Adds order to db...
			    OrderInfo order = CheckoutControl.GetFinalizedOrderInfo();

                GenerateOrderConfirmation();

                //Process transaction
                AuthNetGatewayProvider provider = new AuthNetGatewayProvider(StoreSettings.GatewaySettings);
				TransactionResult orderResult = provider.ProcessTransaction(shippingAddress, billingAddress, order, transaction);
                if (!orderResult.Succeeded)
				{
                    string errorMessage = string.Empty;
                    string localizedReason = string.Empty;
                    // Try to get the corresponding localized reason message
                    localizedReason = Localization.GetString("ReasonCode" + orderResult.ReasonCode, LocalResourceFile);
                    // If a localized message do not exist use the original message
                    if (localizedReason == string.Empty | localizedReason == null)
                    {
                        localizedReason = orderResult.Message.ToString();
                    }
                    switch (orderResult.ResultCode)
                    {
                        case -5:
                            errorMessage = Localization.GetString("ErrorCardInformation", LocalResourceFile);
                            break;
                        case -4:
                            errorMessage = Localization.GetString("ErrorBillingAddress", LocalResourceFile);
                            break;
                        case -3:
                            errorMessage = Localization.GetString("ErrorPaymentOption", LocalResourceFile);
                            break;
                        case -2:
                            errorMessage = Localization.GetString("ErrorConnection", LocalResourceFile);
                            break;
                        case -1:
                            errorMessage = Localization.GetString("ErrorUnexpected", LocalResourceFile);
                            break;
                        case 2:
                            errorMessage = string.Format(Localization.GetString("ReasonMessage", LocalResourceFile), Localization.GetString("ResponseCode2", LocalResourceFile), orderResult.ReasonCode, "");
                            CheckoutControl.Order = UpdateOrderStatus(order, OrderInfo.OrderStatusList.AwaitingPayment);
                            CheckoutControl.Hide();
                            pnlProceedToAuthorize.Visible = false;
                            InvokePaymentFailed();
                            CurrentCart.DeleteCart(PortalId, StoreSettings.SecureCookie);
                            ClearOrderIdCookie();
                            break;
                        case 3:
                            errorMessage = string.Format(Localization.GetString("ReasonMessage", LocalResourceFile), Localization.GetString("ResponseCode3", LocalResourceFile), orderResult.ReasonCode, localizedReason);
                            break;
                        case 4:
                            errorMessage = string.Format(Localization.GetString("ReasonMessage", LocalResourceFile), Localization.GetString("ResponseCode4", LocalResourceFile), orderResult.ReasonCode, localizedReason);
                            CheckoutControl.Order = UpdateOrderStatus(order, OrderInfo.OrderStatusList.AwaitingPayment);
                            CheckoutControl.Hide();
                            pnlProceedToAuthorize.Visible = false;
                            InvokePaymentRequiresConfirmation();
                            CurrentCart.DeleteCart(PortalId, StoreSettings.SecureCookie);
                            ClearOrderIdCookie();
                            break;
                        default:
                            errorMessage = string.Format(Localization.GetString("ReasonMessage", LocalResourceFile), Localization.GetString("ErrorUnexpected", LocalResourceFile), orderResult.ReasonCode, localizedReason);
                            break;
                    }
                    lblError.Visible = true;
                    lblError.Text = errorMessage;
                }
				else
				{
                    int portalId = PortalSettings.PortalId;
                    // Set order status to "Paid"...
                    CheckoutControl.Order = UpdateOrderStatus(order, OrderInfo.OrderStatusList.Paid);
                    // Add User to Product Roles
                    OrderController orderController = new OrderController();
                    orderController.AddUserToRoles(PortalId, order);
                    // Add User to Order Role
                    StoreInfo storeSetting = StoreController.GetStoreInfo(PortalSettings.PortalId);
                    if (storeSetting.OnOrderPaidRoleID != Null.NullInteger)
                        orderController.AddUserToPaidOrderRole(portalId, order.CustomerID, storeSetting.OnOrderPaidRoleID);
                    CheckoutControl.Hide();
                    pnlProceedToAuthorize.Visible = false;
                    lblError.Visible = false;
					InvokePaymentSucceeded();
                    CurrentCart.DeleteCart(PortalId, StoreSettings.SecureCookie);
                    ClearOrderIdCookie();
				}
			}
			else
			{
                lblError.Visible = true;
                lblError.Text = Localization.GetString("ErrorCardNotValid", LocalResourceFile);
			}
            btnProcess.Enabled = true;
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

		#endregion
	}
}
