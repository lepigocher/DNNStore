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
using System.Globalization;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Cart;
using DotNetNuke.Modules.Store.Customer;
using DotNetNuke.Modules.Store.Providers.Address;


namespace DotNetNuke.Modules.Store.Providers.Shipping.DefaultShippingProvider
{
    /// <summary>
    /// Summary description for CoreProfile.
    /// </summary>
    public partial class DefaultShippingCheckout : ProviderControlBase, ICheckoutControl
    {
        #region Private Members

        private StoreInfo _storeInfo;
        private OrderInfo _orderInfo;
        private bool _includeVAT;
        private readonly NumberFormatInfo _localFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            if (_storeInfo == null)
                _storeInfo = StoreController.GetStoreInfo(PortalId);
            if (string.IsNullOrEmpty(_storeInfo.CurrencySymbol) == false)
                _localFormat.CurrencySymbol = _storeInfo.CurrencySymbol;

            UpdateShippingTotal();
        }

        #endregion

        #region Private Methods

        private void UpdateShippingTotal()
        {
            if (_orderInfo != null)
            {
                if (_includeVAT)
                    lblOrderShippingCost.Text = (_orderInfo.ShippingCost + _orderInfo.ShippingTax).ToString("C", _localFormat);
                else
                    lblOrderShippingCost.Text = _orderInfo.ShippingCost.ToString("C", _localFormat);
            }
            else
                lblOrderShippingCost.Text = 0M.ToString("C", _localFormat);
        }

        #endregion

        #region ICheckoutControl Members

        public void Hide() { Visible = false; }

        public StoreInfo StoreSettings
        {
            get { return _storeInfo; }
            set { _storeInfo = value; }
        }

        public IAddressInfo ShippingAddress { get; set; }

        public IAddressInfo BillingAddress { get; set; }

        public OrderInfo Order
        {
            get { return _orderInfo; }
            set
            {
                _orderInfo = value;
                UpdateShippingTotal();
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
