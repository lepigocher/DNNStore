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
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using DotNetNuke.Common;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.Store.Admin;
using Ctlg = DotNetNuke.Modules.Store.Catalog;
using DotNetNuke.Modules.Store.Cart;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Coupon;
using DotNetNuke.Modules.Store.Customer;
using DotNetNuke.Modules.Store.Providers;
using DotNetNuke.Modules.Store.Providers.Address;
using DotNetNuke.Modules.Store.Providers.Shipping;
using DotNetNuke.Modules.Store.Providers.Tax;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for Account.
	/// </summary>
    //
    public partial class Checkout : StoreControlBase, ICheckoutControl, IStoreTabedControl
	{
		#region Controls

		protected AddressCheckoutControlBase AddressCheckoutControl;
		protected ICheckoutControl TaxControl;
		protected ICheckoutControl ShippingControl;
		protected PaymentControlBase PaymentControl;

		#endregion       

		#region Private Members

        private const string CookieName = "DotNetNuke_Store_Portal_";
        private const int LastStep = 3;
        private ModuleSettings _moduleSettings;
	    private readonly NumberFormatInfo _localFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
        private readonly OrderController _orderController = new OrderController();
        private readonly Ctlg.CategoryController _categoryController = new Ctlg.CategoryController();
	    private IAddressProvider _addressProvider;
	    private string _templatePath;
        //private bool createAccount;

		#endregion

        #region Properties

	    public int CheckoutStep { get; set; }

	    #endregion

        #region Events

		override protected void OnInit(EventArgs e)
		{
            Page.RegisterRequiresControlState(this); 
            base.OnInit(e);
		    if (StoreSettings != null)
		    {
                // On first load
                if (!IsPostBack)
                {
                    // Check if SSL is required
                    if (ForceSSL())
                        SSLHelper.RequestSecurePage();
                    // Init checkout step
                    CheckoutStep = 1;
                }

                // Various Init
                if (StoreSettings.CurrencySymbol != string.Empty)
                    _localFormat.CurrencySymbol = StoreSettings.CurrencySymbol;

                _templatePath = CssTools.GetTemplatePath(this, StoreSettings.PortalTemplates);

                _moduleSettings = new ModuleSettings(ModuleId, TabId);
                IncludeVAT = _moduleSettings.MainCart.IncludeVAT;

                // Load controls
                LoadAddressControl();
                LoadCartControl();
                LoadShippingCheckoutControl();
                LoadTaxCheckoutControl();
                LoadPaymentControl();
		    }
		}
		
        protected override void LoadControlState(object savedState)
        {
            if (savedState != null)
            {
                Pair p = savedState as Pair;
                if (p != null)
                {
                    base.LoadControlState(p.First);
                    CheckoutState state = p.Second as CheckoutState;
                    if (state != null)
                    {
                        Order = state.Order;
                        CheckoutStep = state.CheckoutStep;
                    }
                }
                else
                {
                    if (savedState is CheckoutState)
                    {
                        CheckoutState state = (CheckoutState) savedState;
                        Order = state.Order;
                        CheckoutStep = state.CheckoutStep;
                    }
                    else
                        base.LoadControlState(savedState);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
		{
            if (StoreSettings != null)
            {
                if (!IsPostBack )
                {
                    // Check if the customer returns from payment
                    if (Request.QueryString["GatewayExit"] == null)
                    {
                        // NO: Retrieve order from cookie or create a new order
                        Order = GetExistingOrder(GetOrderIDFromCookie(), true) ?? CreateOrder();
                    }
                    else
                    {
                        // YES: Retrieve order from querystring
                        if (Request.QueryString["OrderID"] != null)
                        {
                            int orderID = int.Parse(Request.QueryString["OrderID"]);
                            Order = GetExistingOrder(orderID, false);
                            if (Order != null)
                            {
                                // Display Done step
                                CheckoutStep = 4;
                            }
                            else
                            {
                                // Display ERROR!!!
                                divStoreCheckoutSteps.Visible = false;
                                plhCheckout.Visible = false;
                                plhError.Visible = true;
                                lblError.Text = "ERROR!!!";
                                plhCheckoutNavigation.Visible = false;
                            }
                        }
                    }
                }
                // Display the needed step
                GoToStep(CheckoutStep);
            }
            else
            {
                divStoreCheckoutSteps.Visible = false;
                plhCheckout.Visible = false;
            }
		}

        protected void paymentControl_AwaitingPayment(object sender, EventArgs e)
        {
            UpdateOrderResult(Localization.GetString("AwaitingPayment", LocalResourceFile));
        }

        protected void paymentControl_PaymentRequiresConfirmation(object sender, EventArgs e)
        {
            UpdateOrderResult(Localization.GetString("PaymentRequiresConfirmation", LocalResourceFile));
        }

        protected void paymentControl_PaymentSucceeded(object sender, EventArgs e)
		{
            UpdateOrderResult(Localization.GetString("PaymentSucceeded", LocalResourceFile));
        }

        protected void paymentControl_PaymentCancelled(object sender, EventArgs e)
		{
            UpdateOrderResult(Localization.GetString("PaymentCancelled", LocalResourceFile));
        }

        protected void paymentControl_PaymentFailed(object sender, EventArgs e)
		{
            UpdateOrderResult(Localization.GetString("PaymentFailed", LocalResourceFile));
        }

        protected void ctlAddressCheckout_BillingAddressChanged(object sender, EventArgs e)
        {
            UpdateAddresses();
        }

        protected void ctlAddressCheckout_ShippingAddressChanged(object sender, EventArgs e)
        {
            UpdateAddresses();
        }

		/// <summary>
		/// This event should occur each time a change is made to the cart.  
		/// All of the Checkout controls should be updated with the new cart information and the 
		/// order updated.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        protected void cartControl_EditComplete(object sender, EventArgs e)
		{
            CartInfo cart = CurrentCart.GetInfo(PortalId, StoreSettings.SecureCookie);
            if (cart != null && cart.Items > 0)
            {
                Order = _orderController.UpdateOrderDetails(Order.OrderID, cart.CartID);
                CalculateTaxAndShipping(null);
                UpdateCheckoutOrder();
            }
            else
            {
                Response.Redirect(Globals.NavigateURL(StoreSettings.StorePageID));                
            }
		}

        protected void lbApplyCoupon_Click(object sender, EventArgs e)
        {
            string couponCode = txtCouponCode.Text;
            if (!string.IsNullOrEmpty(couponCode))
            {
                CouponController controller = new CouponController();
                CouponInfo coupon = controller.GetCouponByCode(PortalId, couponCode);
                if (ValidateCoupon(coupon, Order))
                    SetCoupon(coupon);
                else
                    SetCoupon(null);
            }
            else
            {
                SetCoupon(null);
                lblDiscountMessage.Text = "";
            }
        }

	    protected void StoreAccountCheckoutPrevious_Click(object sender, EventArgs e)
        {
            int step = CheckoutStep;
            bool update = step == LastStep;
            step--;
            CheckoutStep = step;
            GoToStep(step);
            if (update)
            {
                CalculateTaxAndShipping(null);
                UpdateCheckoutOrder();
            }
        }

        protected void StoreAccountCheckoutNext_Click(object sender, EventArgs e)
        {
            int step = CheckoutStep;
            if (step < LastStep)
            {
                Page.Validate();
                if (Page.IsValid)
                {
                    bool update = step == 1;
                    step++;
                    CheckoutStep = step;
                    GoToStep(step);
                    if (update)
                    {
                        UpdateAddresses();
                        CalculateTaxAndShipping(null);
                        UpdateCheckoutOrder();
                    }
                }
            }
        }

        protected override object SaveControlState()
        {
            object obj = base.SaveControlState();

            if (Order != null)
            {
                CheckoutState state = new CheckoutState {Order = Order, CheckoutStep = CheckoutStep};

                if (obj != null)
                    return new Pair(obj, state);

                return (state);
            }

            return obj;
        }

		#endregion

		#region Private Methods

        private void GoToStep(int step)
        {
            // Reset steps classes
            liStepShipping.Attributes["class"] = "StoreCheckoutStep";
            liStepOrderReview.Attributes["class"] = "StoreCheckoutStep";
            liStepPayment.Attributes["class"] = "StoreCheckoutStep";
            // Switch to the required screen step
            switch (step)
            {
                case 0: // Return to Store
                    Response.Redirect(Globals.NavigateURL(StoreSettings.StorePageID), true);
                    break;
                case 1: // Show Addresses
                    // Display required screen
                    divStoreCheckoutAddresses.Visible = true;
                    divStoreCheckoutCart.Visible = false;
                    divStoreCheckoutGateway.Visible = false;
                    // Set as active step
                    liStepShipping.Attributes["class"] = "StoreCheckoutActiveStep";
                    StoreAccountCheckoutPrevious.Visible = false;
                    StoreAccountCheckoutNext.Visible = true;
                    // Check if the Account fieldset have to be displayed
                    //if (StoreSettings.CheckoutMode == CheckoutType.UserChoice && IsLogged == false)
                    //{
                    //    fsAccountInfos.Visible = true;
                    //}
                    break;
                case 2: // Show Final Cart
                    // Display required screen
                    divStoreCheckoutAddresses.Visible = false;
                    divStoreCheckoutCart.Visible = true;
                    divStoreCheckoutGateway.Visible = false;
                    // Set as active step
                    liStepOrderReview.Attributes["class"] = "StoreCheckoutActiveStep";
                    StoreAccountCheckoutPrevious.Visible = true;
                    StoreAccountCheckoutNext.Visible = true;
                    break;
                case 3: // Show Payment
                    // Display required screen
                    divStoreCheckoutAddresses.Visible = false;
                    divStoreCheckoutCart.Visible = false;
                    divStoreCheckoutGateway.Visible = true;
                    // Set as active step
                    liStepPayment.Attributes["class"] = "StoreCheckoutActiveStep";
                    StoreAccountCheckoutPrevious.Visible = false;
                    StoreAccountCheckoutNext.Visible = false;
                    break;
                case 4: // Return from payment or finished
                    // Display required screen
                    divStoreCheckoutAddresses.Visible = false;
                    divStoreCheckoutCart.Visible = false;
                    divStoreCheckoutGateway.Visible = true;
                    plhOrder.Visible = true;
                    plhCheckoutNavigation.Visible = false;
                    // Set as active step
                    liStepDone.Attributes["class"] = "StoreCheckoutActiveStep";
                    break;
            }
        }

        private string OrderNavigation()
        {
            CustomerNavigation nav = new CustomerNavigation
                                         {
                                             TabID = TabId,
                                             PageID = "CustomerOrders",
                                             CustomerID = Order.CustomerID,
                                             OrderID = Order.OrderID
                                         };
            return nav.GetNavigationUrl();
        }

        private void UpdateOrderResult(string orderProcessed)
        {
            // Update display
            lblOrderNumber.Text = String.Format(Localization.GetString("lblOrderNumber", LocalResourceFile), Order.OrderID);
            lblOrderProcessed.Text = orderProcessed;
            if (IsLogged)
            {
                btnDisplayOrder.Text = Localization.GetString("btnDisplayOrder", LocalResourceFile);
                btnDisplayOrder.PostBackUrl = OrderNavigation();
            }
            else
                btnDisplayOrder.Visible = false;
            GoToStep(4);
        }

		/// <summary>
		/// This informs the checkout controls that the order has been updated.
		/// </summary>
		private void UpdateCheckoutOrder()
		{
			if (Order != null)
			{
				TaxControl.Order = Order;
				ShippingControl.Order = Order;
                lblGrandTotal.Text = Order.GrandTotal.ToString("C", _localFormat);
			}
		}

		/// <summary>
		/// Examine the module setting "RequireSSL" to determine if checkout should force a 
		/// redirect to HTTPS.
		/// </summary>
		/// <returns>true if HTTPS should be used</returns>
		private bool ForceSSL()
		{
			// Determine if checkout should be forced to SSL according the module setting.
			string requireSSLSetting = (string)Settings["RequireSSL"];
			bool requireSSL = false;
            if (!string.IsNullOrEmpty(requireSSLSetting))
            {
                try
                {
                    requireSSL = bool.Parse(requireSSLSetting);
                }
                catch
                {
                    requireSSL = false;
                }
            }
		    return requireSSL;
		}

		///<summary>
		///Load the selected address checkout control and add to the address placeholder.
		///</summary>
        private void LoadAddressControl()
        {
            plhAddressCheckout.Controls.Clear();
            _addressProvider = StoreController.GetAddressProvider(StoreSettings.AddressName);
            AddressCheckoutControl = (AddressCheckoutControlBase)_addressProvider.GetCheckoutControl(this, ModulePath);
            AddressCheckoutControl.ID = "addresscheckout";
            AddressCheckoutControl.ModuleConfiguration = ModuleConfiguration;
		    AddressCheckoutControl.NoDelivery = StoreSettings.NoDelivery;
		    AddressCheckoutControl.CheckoutMode = StoreSettings.CheckoutMode;
            AddressCheckoutControl.BillingAddressChanged += ctlAddressCheckout_BillingAddressChanged;
            AddressCheckoutControl.ShippingAddressChanged += ctlAddressCheckout_ShippingAddressChanged;
            plhAddressCheckout.Controls.Add(AddressCheckoutControl);
        }

		private void LoadCartControl()
		{
			plhCart.Controls.Clear();
            CartDetail cartControl = (CartDetail)LoadControl(ModulePath + "CartDetail.ascx");
            // Read module settings and define cart properties
            MainCartSettings cartSettings = new ModuleSettings(ModuleId, TabId).MainCart;
            cartControl.ShowThumbnail = cartSettings.ShowThumbnail;
            cartControl.ThumbnailWidth = cartSettings.ThumbnailWidth;
            cartControl.GIFBgColor = cartSettings.GIFBgColor;
            cartControl.EnableImageCaching = cartSettings.EnableImageCaching;
            cartControl.CacheImageDuration = cartSettings.CacheImageDuration;
            cartControl.ProductColumn = cartSettings.ProductColumn.ToLower();
            cartControl.LinkToDetail = cartSettings.LinkToDetail;
            cartControl.IncludeVAT = cartSettings.IncludeVAT;
            cartControl.TemplatePath = _templatePath;
            cartControl.ID = "cartdetail";
            cartControl.ModuleConfiguration = ModuleConfiguration;
            cartControl.StoreSettings = StoreSettings;
			cartControl.EditComplete += cartControl_EditComplete;
			plhCart.Controls.Add(cartControl);
		}

		private void LoadShippingCheckoutControl()
		{
			plhShippingCheckout.Controls.Clear();
            IShippingProvider shippingProvider = StoreController.GetShippingProvider(StoreSettings.ShippingName);
			ShippingControl = (ICheckoutControl)shippingProvider.GetCheckoutControl(this, ModulePath);
            ShippingControl.StoreSettings = StoreSettings;
            ShippingControl.IncludeVAT = IncludeVAT;
			plhShippingCheckout.Controls.Add((ProviderControlBase)ShippingControl);
            if (StoreSettings.NoDelivery | Shipping == ShippingMode.None)
		        plhShippingCheckout.Visible = false;
		    else
		        plhShippingCheckout.Visible = true;
		}

		private void LoadTaxCheckoutControl() 
		{
			plhTaxCheckout.Controls.Clear();
            ITaxProvider taxProvider = StoreController.GetTaxProvider(StoreSettings.TaxName);
			TaxControl = (ICheckoutControl)taxProvider.GetCheckoutControl(this, ModulePath);
            TaxControl.StoreSettings = StoreSettings;
			plhTaxCheckout.Controls.Add((ProviderControlBase)TaxControl);
		}

		private void LoadPaymentControl()
		{
			GatewayController controller = new GatewayController(Server.MapPath(ModulePath));
            GatewayInfo gateway = controller.GetGateway(StoreSettings.GatewayName);

			if (gateway != null)
			{
                string controlPath = Path.Combine(gateway.GatewayPath, gateway.PaymentControl);
				if (File.Exists(controlPath))
				{
                    controlPath = controlPath.Replace(Server.MapPath(ModulePath), ModulePath).Replace(Path.DirectorySeparatorChar, '/');

					plhGateway.Controls.Clear();
					PaymentControl = (PaymentControlBase)LoadControl(controlPath);
                    PaymentControl.ID = gateway.PaymentControl.ToLower();
                    PaymentControl.ModuleConfiguration = ModuleConfiguration;
                    PaymentControl.StoreSettings = StoreSettings;
					PaymentControl.CheckoutControl = this;
                    PaymentControl.EnableViewState = true;
                    PaymentControl.AwaitingPayment += paymentControl_AwaitingPayment;
                    PaymentControl.PaymentRequiresConfirmation += paymentControl_PaymentRequiresConfirmation;
					PaymentControl.PaymentSucceeded += paymentControl_PaymentSucceeded;
					PaymentControl.PaymentCancelled += paymentControl_PaymentCancelled;
					PaymentControl.PaymentFailed += paymentControl_PaymentFailed;
					plhGateway.Controls.Add(PaymentControl);
				}
				else
				{
                    LiteralControl error = new LiteralControl("<span class=\"NormalRed\">" + Localization.GetString("ErrorCouldNotFind", LocalResourceFile) + " " + controlPath + ".</span>");
			
					plhGateway.Controls.Clear();
					plhGateway.Controls.Add(error);
				}
			}
			else
			{ 
                    LiteralControl error = new LiteralControl("<span class=\"NormalRed\">" + Localization.GetString("ErrorPaymentOption", LocalResourceFile) + "</span>");
			
					plhGateway.Controls.Clear();
					plhGateway.Controls.Add(error);				
			}
		}

		/// <summary>
		/// Retrieves the OrderID from the cookie if available; otherwise a -1 is returned
		/// </summary>
		/// <returns>OrderID if found in cookie; otherwise a -1 is returned.</returns>
		private int GetOrderIDFromCookie()
		{
			int orderID = Null.NullInteger;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieKey];
            if ((cookie != null) && (cookie["OrderID"] != null))
            {
                string cookieValue = cookie["OrderID"];
                if (string.IsNullOrEmpty(cookieValue) == false)
                {
                    int value;
                    // If it's NOT an integer, try to decrypt the value
                    if (int.TryParse(cookieValue, out value) == false)
                    {
                        string decrypted = SymmetricHelper.Decrypt(cookieValue);
                        orderID = int.Parse(decrypted);
                    }
                    else
                        orderID = value;
                }
            }
			return orderID;
		}

		private void SetOrderIdCookie(int orderID)
		{
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieKey] ?? new HttpCookie(CookieKey);
		    string cookieValue;
            if (StoreSettings.SecureCookie && SymmetricHelper.CanSafelyEncrypt)
                cookieValue = SymmetricHelper.Encrypt(orderID.ToString());
            else
                cookieValue = orderID.ToString();
            cookie["OrderID"] = cookieValue;
            HttpContext.Current.Response.Cookies.Add(cookie);
		}

		private string CookieKey
		{
            get { return CookieName + PortalId; }
		}

	    /// <summary>
	    /// Calculate the tax and shipping cost for the order.
	    /// </summary>
        private void CalculateTaxAndShipping(CouponInfo coupon)
		{
			if (Order != null)
			{
                List<ItemInfo> cartItems = CurrentCart.GetItems(PortalId, StoreSettings.SecureCookie);
			    IShippingInfo shippingInfo;

                // Calculate Shipping if enabled
                IShippingProvider shippingProvider = StoreController.GetShippingProvider(StoreSettings.ShippingName);
                if (StoreSettings.NoDelivery || Shipping == ShippingMode.None || ApplyFreeShipping(cartItems, ShippingAddress.CountryCode))
                    shippingInfo = shippingProvider.GetFreeShipping();
                else
                    shippingInfo = shippingProvider.CalculateShippingFee(PortalId, BillingAddress, ShippingAddress, cartItems);

                if (shippingInfo == null)
                {
                    plhCheckout.Visible = false;
                    lblError.Text = String.Format(Localization.GetString("ErrorShippingRates", LocalResourceFile), StoreSettings.DefaultEmailAddress);
                    plhError.Visible = true;
                    return;
                }

                plhCheckout.Visible = true;
                plhError.Visible = false;

                // Check for coupon validity
			    bool couponIsValid = false;
			    if (StoreSettings.AllowCoupons)
			    {
			        int couponID = Order.CouponID;
                    if (couponID != Null.NullInteger)
                    {
                        if (coupon == null || coupon.CouponID != couponID)
                        {
                            CouponController couponController = new CouponController();
                            coupon = couponController.GetCoupon(PortalId, couponID);
                        }
                        couponIsValid = ValidateCoupon(coupon, Order);
                    }
			        divStoreCheckoutCoupon.Visible = true;
			    }
			    else
			        divStoreCheckoutCoupon.Visible = false;

                // Apply Discount if coupon is valid
                if (couponIsValid)
                {
                    decimal discountTotal = 0;
                    switch (coupon.DiscountType)
                    {
                        case CouponDiscount.Percentage:
                            decimal discountPercentage = Convert.ToDecimal(coupon.DiscountPercentage)/100;
                            foreach (ItemInfo cartItem in cartItems)
                            {
                                switch (coupon.ApplyTo)
                                {
                                    case CouponApplyTo.Order:
                                        cartItem.Discount = cartItem.SubTotal * discountPercentage;
                                        discountTotal += cartItem.Discount;
                                        break;
                                    case CouponApplyTo.Category:
                                        if (ValidateCategoryCoupon(coupon, cartItem.CategoryID))
                                        {
                                            cartItem.Discount = cartItem.SubTotal * discountPercentage;
                                            discountTotal += cartItem.Discount;
                                        }
                                        break;
                                    case CouponApplyTo.Product:
                                        if (cartItem.ProductID == coupon.ItemID)
                                        {
                                            cartItem.Discount = cartItem.SubTotal * discountPercentage;
                                            discountTotal += cartItem.Discount;
                                        }
                                        break;
                                }
                            }
                            break;
                        case CouponDiscount.FixedAmount:
                            int itemsCount = cartItems.Count;
                            foreach (ItemInfo cartItem in cartItems)
                            {
                                switch (coupon.ApplyTo)
                                {
                                    case CouponApplyTo.Order:
                                        cartItem.Discount = coupon.DiscountAmount / itemsCount;
                                        discountTotal = coupon.DiscountAmount;
                                        break;
                                    case CouponApplyTo.Category:
                                        if (ValidateCategoryCoupon(coupon, cartItem.CategoryID))
                                        {
                                            cartItem.Discount = coupon.DiscountAmount * cartItem.Quantity;
                                            discountTotal += cartItem.Discount;
                                        }
                                        break;
                                    case CouponApplyTo.Product:
                                        if (cartItem.ProductID == coupon.ItemID)
                                        {
                                            cartItem.Discount = coupon.DiscountAmount * cartItem.Quantity;
                                            discountTotal += cartItem.Discount;
                                        }
                                        break;
                                }
                            }
                            break;
                        case CouponDiscount.FreeShipping:
                            discountTotal = shippingInfo.Cost;
                            // If Free Shipping exclude amount before tax computation!
                            shippingInfo.Cost = 0;
                            break;
                    }
                    Order.Discount = -discountTotal;
                    lblDiscount.Text = Order.Discount.ToString("C", _localFormat);
                    tbDiscount.Visible = Order.Discount > 0;
                }
                else
                {
                    // Reset order discount
                    Order.CouponID = Null.NullInteger;
                    Order.Discount = Null.NullDecimal;
                    foreach (ItemInfo cartItem in cartItems)
                        cartItem.Discount = 0;
                    tbDiscount.Visible = false;
                }

			    // Add Surcharges if any
                decimal fixedSurcharge = PaymentControl.SurchargeFixed;
                decimal percentSurcharge = PaymentControl.SurchargePercent;

                if (fixedSurcharge != 0 || percentSurcharge != 0)
                {
                    Order.ShippingCost = shippingInfo.Cost + fixedSurcharge + ((Order.OrderNetTotal + shippingInfo.Cost + fixedSurcharge) * (percentSurcharge / 100));
                    shippingInfo.Cost = Order.ShippingCost;
                }
                else
                    Order.ShippingCost = shippingInfo.Cost;

                plhShippingCheckout.Visible = Order.ShippingCost > 0;

			    // Calculate Tax Amount
                ITaxProvider taxProvider = StoreController.GetTaxProvider(StoreSettings.TaxName);
                ITaxInfo taxInfo = taxProvider.CalculateSalesTax(PortalId, cartItems, shippingInfo, BillingAddress);
                if (taxInfo.ShowTax && taxInfo.SalesTax > 0)
                {
                    plhTaxCheckout.Visible = true;
                    Order.TaxTotal = taxInfo.SalesTax;
                    Order.ShippingTax = taxInfo.ShippingTax;
                }
                else
                {
                    plhTaxCheckout.Visible = false;
                    Order.TaxTotal = 0;
                    Order.ShippingTax = 0;
                }
			}
		}

        private bool ValidateCoupon(CouponInfo coupon, OrderInfo order)
        {
            bool isValid = false;

            if (coupon != null)
            {
                DateTime today = DateTime.Now;
                if (coupon.StartDate <= today)
                {
                    bool validityChecked = false;
                    switch (coupon.Validity)
                    {
                        case CouponValidity.Permanent:
                            validityChecked = true;
                            break;
                        case CouponValidity.SingleUse:
                            if (_orderController.CountCouponUsage(PortalId, order.CustomerID, coupon.CouponID, order.OrderID) == 0)
                                validityChecked = true;
                            break;
                        case CouponValidity.Until:
                            if (coupon.EndDate >= today)
                                validityChecked = true;
                            break;
                    }
                    if (validityChecked)
                    {
                        switch (coupon.RuleType)
                        {
                            case CouponRule.OrderAnything:
                                isValid = true;
                                break;
                            case CouponRule.SpendsAtLeast:
                                if (order.OrderTotal >= coupon.RuleAmount)
                                    isValid = true;
                                break;
                            case CouponRule.OrdersAtLeast:
                                decimal totalOrdered = _orderController.GetTotalOrdered(PortalId, order.CustomerID);
                                if ((order.OrderTotal + totalOrdered) >= coupon.RuleAmount)
                                    isValid = true;
                                break;
                        }
                    }
                    if (coupon.DiscountType == CouponDiscount.FreeShipping && AddressCheckoutControl.Shipping == ShippingMode.None)
                        isValid = false;
                }
            }

            if (isValid)
            {
                lblDiscountMessage.Text = coupon.Description;
                lblDiscountMessage.CssClass = "Normal";
            }
            else
            {
                lblDiscountMessage.Text = Localization.GetString("DiscountError", LocalResourceFile);
                lblDiscountMessage.CssClass = "NormalRed";
            }

            return isValid;
        }

        private bool ValidateCategoryCoupon(CouponInfo coupon, int categoryID)
        {
            if (coupon.ItemID == categoryID)
                return true;

            if (coupon.IncludeSubCategories)
                return IsSubCategory(coupon.ItemID, categoryID);

            return false;
        }

        private bool IsSubCategory(int parentCategoryID, int searchedCategoryID)
        {
            bool found = false;
            List<Ctlg.CategoryInfo> categories = _categoryController.GetCategories(PortalId, false, parentCategoryID);
            foreach (Ctlg.CategoryInfo category in categories)
            {
                if (category.CategoryID == searchedCategoryID || IsSubCategory(category.CategoryID, searchedCategoryID))
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        private void SetCoupon(CouponInfo coupon)
        {
            if (coupon == null)
            {
                Order.CouponID = Null.NullInteger;
                CalculateTaxAndShipping(null);
            }
            else
            {
                Order.CouponID = coupon.CouponID;
                CalculateTaxAndShipping(coupon);
            }
            UpdateCheckoutOrder();
            _orderController.UpdateOrder(Order.OrderID, Order.OrderDate, Order.OrderNumber, Order.ShippingAddressID,
                                            Order.BillingAddressID, Order.TaxTotal, Order.ShippingCost, Order.CouponID,
                                            Order.Discount, Order.CustomerID);
        }

        private bool ApplyFreeShipping(IEnumerable<ItemInfo> cartItems, string shippingCountry)
        {
            bool apply = false;

            if (StoreSettings.AllowFreeShipping)
            {
                decimal totalCart = 0;
                foreach (ItemInfo cartItem in cartItems)
                    totalCart += cartItem.SubTotal;

                apply = totalCart >= StoreSettings.MinOrderAmount;

                if (apply && StoreSettings.RestrictToCountries)
                {
                    ListController ctlEntry = new ListController();
                    ListEntryInfoCollection entryCollection = ctlEntry.GetListEntryInfoCollection("Country");
                    string countryCode = string.Empty;
                    string authorizedCountries = StoreSettings.AuthorizedCountries;
                    apply = false;

                    if (!string.IsNullOrEmpty(shippingCountry) && entryCollection.Count > 0)
                    {
                        // Search for country name
                        foreach (ListEntryInfo country in entryCollection)
                        {
                            if (country.Text.Equals(shippingCountry, StringComparison.CurrentCultureIgnoreCase))
                            {
                                countryCode = country.Value;
                                break;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(authorizedCountries) && !string.IsNullOrEmpty(countryCode))
                    {
                        foreach (string country in authorizedCountries.Split(','))
                        {
                            apply = country.Equals(countryCode, StringComparison.CurrentCultureIgnoreCase);
                            if (apply)
                                break;
                        }
                    }
                }
            }

            return apply;
        }

		/// <summary>
		/// Retrieve the current order and addresses from the database.
		/// </summary>
		/// <returns></returns>
        private OrderInfo GetExistingOrder(int orderID, bool updateDetails)
		{
            OrderInfo order = null;

            if (orderID != Null.NullInteger)
			{				
				try
				{
					order = _orderController.GetOrder(PortalId, orderID);
                    if (order != null)
                    {
                        // Update order details if needed
                        if (updateDetails)
                            order = _orderController.UpdateOrderDetails(order.OrderID, CurrentCart.GetInfo(PortalId, StoreSettings.SecureCookie).CartID);
                        // Load billing address or create a new one
                        BillingAddress = _addressProvider.GetAddress(order.BillingAddressID);
                        // Load shipping address or create a new one
                        ShippingAddress = _addressProvider.GetAddress(order.ShippingAddressID);
                    }
				}
				catch 
				{
					order = null;	
				}
			}

			return order;
		}

		private OrderInfo CreateOrder()
		{
            // Get current cart ID
            string cartID = CurrentCart.GetInfo(PortalId, StoreSettings.SecureCookie).CartID;

            // Associate cart with this user or with impersonated user
			CartController cartController = new CartController();
            if (IsLogged)
                cartController.UpdateCart(cartID, UserId);
            else
                cartController.UpdateCart(cartID, StoreSettings.ImpersonatedUserID);

			// Create order from cart content
            OrderInfo order = _orderController.CreateOrder(cartID);
			if (order != null)
			{
				SetOrderIdCookie(order.OrderID);
                // Create billing and shipping addresses
			    BillingAddress = _addressProvider.GetAddress(Null.NullInteger);
                ShippingAddress = _addressProvider.GetAddress(Null.NullInteger);
			}

			return order;
		}

        private void UpdateAddresses()
        {
            OrderInfo order = Order;
            IAddressInfo billingAddress = BillingAddress;
            IAddressInfo shippingAddress = ShippingAddress;
            ShippingMode shipping = Shipping;
            bool updateOrder = false;

            // Define the current user ID
            int currentUser = IsLogged ? UserId : StoreSettings.ImpersonatedUserID;

            // If the user state has changed
            if (order.CustomerID != currentUser)
                updateOrder = true;

            // If the billing address exist but linked to a different user (probably anonymous or impersonated)
            if (billingAddress.AddressID != Null.NullInteger && billingAddress.UserID != currentUser)
            {
                // If address was user saved, create a new one
                if (billingAddress.UserSaved)
                {
                    billingAddress.AddressID = Null.NullInteger;
                    billingAddress.UserSaved = false;
                }
                billingAddress.PrimaryAddress = false;
                billingAddress.UserID = currentUser;
                billingAddress.Modified = true;
                updateOrder = true;
            }

            // If the shipping address exist but linked to a different user (probably anonymous or impersonated)
            if (shippingAddress.AddressID != Null.NullInteger && shippingAddress.UserID != currentUser)
            {
                // If address was user saved, create a new one
                if (shippingAddress.UserSaved)
                {
                    shippingAddress.AddressID = Null.NullInteger;
                    shippingAddress.UserSaved = false;
                }
                shippingAddress.PrimaryAddress = false;
                shippingAddress.UserID = currentUser;
                shippingAddress.Modified = true;
                updateOrder = true;
            }

            // Add or update billing address
            int billingAddressID = billingAddress.AddressID;
            if ((billingAddressID == Null.NullInteger && billingAddress.Modified) || billingAddressID == 0)
            {
                billingAddress.PortalID = PortalId;
                billingAddress.UserID = currentUser;
                if (string.IsNullOrEmpty(billingAddress.Description))
                    billingAddress.Description = string.Format(Localization.GetString("BillingDescription", LocalResourceFile), order.OrderID);
                billingAddressID = _addressProvider.AddAddress(billingAddress);
                BillingAddress = _addressProvider.GetAddress(billingAddressID);
                if (shipping == ShippingMode.SameAsBilling)
                    ShippingAddress = shippingAddress = BillingAddress;
            }
            else if (billingAddressID != Null.NullInteger && billingAddress.Modified)
                _addressProvider.UpdateAddress(billingAddress);
            if (billingAddressID != Order.BillingAddressID)
                updateOrder = true;

            // Add or update shipping address
            int shippingAddressID = shippingAddress.AddressID;
            if (shipping != ShippingMode.None && shippingAddressID != billingAddressID)
            {
                if ((shipping == ShippingMode.Other && shippingAddressID == Null.NullInteger && shippingAddress.Modified) || shippingAddressID == 0)
                {
                    shippingAddress.PortalID = PortalId;
                    shippingAddress.UserID = currentUser;
                    if (string.IsNullOrEmpty(shippingAddress.Description))
                        shippingAddress.Description = string.Format(Localization.GetString("ShippingDescription", LocalResourceFile), order.OrderID);
                    shippingAddressID = _addressProvider.AddAddress(shippingAddress);
                    ShippingAddress = _addressProvider.GetAddress(shippingAddressID);
                }
                else if (shipping == ShippingMode.SameAsBilling && shippingAddressID != billingAddressID)
                    shippingAddressID = billingAddressID;
                else if (shippingAddressID != Null.NullInteger && shippingAddress.Modified)
                    _addressProvider.UpdateAddress(shippingAddress);
            }
            if (shippingAddressID != Order.ShippingAddressID)
                updateOrder = true;

            // Update order if needed
            if (updateOrder)
            {
                Order.BillingAddressID = billingAddressID;
                Order.ShippingAddressID = shippingAddressID;
                _orderController.UpdateOrder(order.OrderID, order.OrderDate, order.OrderNumber, shippingAddressID,
                                             billingAddressID, order.TaxTotal, order.ShippingCost, order.CouponID,
                                             order.Discount, currentUser);
            }
        }

		#endregion

		#region ICheckoutControl Members

        public void Hide() 
        {
            plhCheckout.Visible = false;
        }

        public void HidePrevious()
        {
            StoreAccountCheckoutPrevious.Visible = false;
        }

		public IAddressInfo BillingAddress
		{
			get
			{
                if (AddressCheckoutControl != null)
                    return AddressCheckoutControl.BillingAddress;

                return null;
			}
			set
			{
                if (AddressCheckoutControl != null)
                    AddressCheckoutControl.BillingAddress = value;
			}
		}

		public IAddressInfo ShippingAddress
		{
			get
			{
                if (AddressCheckoutControl != null)
                    return AddressCheckoutControl.ShippingAddress;

                return null;
			}
			set
			{
                if (AddressCheckoutControl != null)
                    AddressCheckoutControl.ShippingAddress = value;
			}
		}

	    public ShippingMode Shipping
	    {
	        get
	        {
                if (AddressCheckoutControl != null)
                    return AddressCheckoutControl.Shipping;

	            return ShippingMode.Undefined;
	        }
	    }

	    public OrderInfo Order { get; set; }

	    public bool IncludeVAT { get; set; }

	    /// <summary>
		/// Calculate the final order and place it into a "waiting for payment" state.
		/// </summary>
		/// <returns>OrderInfo with the final cart, tax, and shipping totals.</returns>
		public OrderInfo GetFinalizedOrderInfo()
		{
			// Update tax and shipping.
            CalculateTaxAndShipping(null);

            OrderInfo order = Order;
            int billingAddressID = BillingAddress.AddressID;
            int shippingAddressID = ShippingAddress.AddressID;

			// Save order details
            _orderController.UpdateOrder(order.OrderID, DateTime.Now,
                "",
                shippingAddressID,
                billingAddressID,
                order.TaxTotal,
                order.ShippingCost,
                order.CouponID,
                order.Discount,
                true,
                (int)OrderInfo.OrderStatusList.AwaitingPayment,
                order.CustomerID);

            return order;
		}

        public OrderInfo GetOrderDetails()
        {
            return _orderController.GetOrder(PortalId, Order.OrderID);
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
