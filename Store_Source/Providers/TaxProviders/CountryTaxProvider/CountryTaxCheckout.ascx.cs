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
using System.Globalization;

using DotNetNuke.Modules.Store.Core.Admin;
using DotNetNuke.Modules.Store.Core.Cart;
using DotNetNuke.Modules.Store.Core.Customer;
using DotNetNuke.Modules.Store.Core.Providers;
using DotNetNuke.Modules.Store.Core.Providers.Address;

namespace DotNetNuke.Modules.Store.Providers.Tax.CountryTaxProvider
{
    /// <summary>
    /// Summary description for CountryTaxCheckout.
    /// </summary>
    public partial class CountryTaxCheckout : ProviderControlBase, ICheckoutControl
	{
		#region Private Members

		private StoreInfo _storeInfo;
        private ModuleSettings _moduleSettings;
        private IAddressInfo _shippingAddress;
        private IAddressInfo _billingAddress;
		private OrderInfo _orderInfo;
        private bool _includeVAT;
        private readonly NumberFormatInfo _localFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();

        #endregion

		#region Event Handlers

		protected void Page_Load(object sender, EventArgs e)
		{
            if (_storeInfo == null)
                _storeInfo = StoreController.GetStoreInfo(PortalId);
            if (_storeInfo.CurrencySymbol != string.Empty)
                _localFormat.CurrencySymbol = _storeInfo.CurrencySymbol;
            _moduleSettings = new ModuleSettings(ParentControl.ModuleId, ParentControl.TabId);
            _includeVAT = _moduleSettings.MainCart.IncludeVAT;

            UpdateTaxTotal();
		}

		#endregion

        #region Private Methods

        private void UpdateTaxTotal()
		{
            if (_orderInfo != null)
                lblOrderTaxTotal.Text = _orderInfo.TaxTotal.ToString("C", _localFormat);
			else 
                lblOrderTaxTotal.Text = 0M.ToString("C", _localFormat);
        }

        #endregion
		
		#region ICheckoutControl Members

        public void Hide() { Visible = false; }

        public StoreInfo StoreSettings
		{
            get { return _storeInfo; }
            set { _storeInfo = value; }
		}

		public IAddressInfo ShippingAddress
		{
            get { return _shippingAddress; }
            set { _shippingAddress = value; }
		}

		public IAddressInfo BillingAddress
		{
            get { return _billingAddress; }
            set { _billingAddress = value; }
		}

        public OrderInfo Order
        {
            get { return _orderInfo; }
            set
            {
                _orderInfo = value;
                UpdateTaxTotal();
            }
        }

        public bool IncludeVAT
        {
            get { return _includeVAT; }
            set { _includeVAT = value; }
        }

        public OrderInfo GetFinalizedOrderInfo() { return _orderInfo; }
        public OrderInfo GetOrderDetails() { return _orderInfo; }

		#endregion
    }
}
