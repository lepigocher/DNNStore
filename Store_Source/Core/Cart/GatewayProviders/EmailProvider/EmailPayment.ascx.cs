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

using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

using DotNetNuke.Modules.Store.Core.Customer;

namespace DotNetNuke.Modules.Store.Core.Cart
{
	/// <summary>
	/// Summary description for EmailCheckout.
	/// </summary>
	public partial class EmailPayment : PaymentControlBase
	{
		#region Private Members

		//private EmailSettings _settings;
        private const string CookieName = "DotNetNuke_Store_Portal_";

		#endregion

		#region Events

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
            // This provider DO NOT support OnLinePayment!
            OnLinePayment = false;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
                if (!Page.IsPostBack)
                {
                    btnConfirmOrder.Attributes.Add("OnClick", ScriptAvoidDoubleClick(btnConfirmOrder, Localization.GetString("Processing", LocalResourceFile)));
			        lblError.Text = string.Empty;
			        lblError.Visible = false;
                    string message = Localization.GetString("lblConfirmMessage", LocalResourceFile);
                    lblConfirmMessage.Text = string.Format(message, PortalSettings.PortalName);
                }

                EmailNavigation nav = new EmailNavigation(Request.QueryString);
                if (nav.GatewayExit.ToUpper() == "RETURN")
                {
                    CheckoutControl.Hide();
                    pnlProceedToEmail.Visible = false;
                    InvokeAwaitingPayment();
                }
			}
			catch(Exception ex)
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

        protected void btnConfirmOrder_Click(object sender, EventArgs e)
        {
            ConfirmOrder();
        }

		#endregion

		#region Private Methods

        private void ConfirmOrder()
        {
            //Adds order to db...
            OrderInfo order = CheckoutControl.GetFinalizedOrderInfo();
            GenerateOrderConfirmation();
            // Set order status to "Awaiting Payment"...
            CheckoutControl.Order = UpdateOrderStatus(order, OrderInfo.OrderStatusList.AwaitingPayment);

            //Clear basket
            CurrentCart.DeleteCart(PortalId, StoreSettings.SecureCookie);
            //Clear cookies
            ClearOrderIdCookie();

            EmailNavigation nav = new EmailNavigation(Request.QueryString)
            {
                GatewayExit = "return",
                OrderID = order.OrderID
            };
            Response.Redirect(nav.GetNavigationUrl());
        }

        private void ClearOrderIdCookie()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieKey];
            if (cookie != null)
                cookie["OrderID"] = null;
        }

        private string CookieKey
        {
            get { return CookieName + PortalId; }
        }

		#endregion
	}
}
