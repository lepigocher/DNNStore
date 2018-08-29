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

using DotNetNuke.Modules.Store.Core.Providers.Address;

namespace DotNetNuke.Modules.Store.Core.Providers
{
    /// <summary>
    /// All address provider checkout controls must inherit from this class.
    /// </summary>
    public class AddressCheckoutControlBase : ProviderControlBase
    {
        public event ShippingAddressChangedEventHandler ShippingAddressChanged;
        public event BillingAddressChangedEventHandler BillingAddressChanged;

        public virtual IAddressInfo ShippingAddress { get; set; }

        public virtual IAddressInfo BillingAddress { get; set; }

        public virtual bool NoDelivery { get; set; }

        public virtual ShippingMode Shipping { get; protected set; }

        public virtual CheckoutType CheckoutMode { get; set; }

        protected virtual void OnShippingAddressChanged(EventArgs e)
        {
            ShippingAddressChanged?.Invoke(this, e);
        }

        protected virtual void OnBillingAddressChanged(EventArgs e)
        {
            BillingAddressChanged?.Invoke(this, e);
        }

        protected void SendShippingAddressChangedEvent()
        {
            OnShippingAddressChanged(new EventArgs());
        }

        protected void SendBillingAddressChangedEvent()
        {
            OnBillingAddressChanged(new EventArgs());
        }
    }

    public delegate void ShippingAddressChangedEventHandler(object sender, EventArgs e);
    public delegate void BillingAddressChangedEventHandler(object sender, EventArgs e);
}