/*
'  DotNetNuke -  http://www.dotnetnuke.com
'  Copyright (c) 2002-2011
'  by DotNetNuke Corporation
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

namespace DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider
{
	/// <summary>
	/// All address provider checkout controls must implement this abstract 
	/// class.
	/// </summary>
	public abstract class AddressCheckoutControlBase : ProviderControlBase
	{
        public event ShippingAddressChangedEventHandler ShippingAddressChanged;
        public event BillingAddressChangedEventHandler BillingAddressChanged;

		public abstract IAddressInfo ShippingAddress
		{
			get;
			set;
		}

		public abstract IAddressInfo BillingAddress
		{
			get;
			set;
		}

        protected virtual void OnShippingAddressChanged(AddressEventArgs e)
        {
            if (ShippingAddressChanged != null)
            {
                ShippingAddressChanged(this, e);
            }
        }

        protected virtual void OnBillingAddressChanged(AddressEventArgs e)
        {
            if (BillingAddressChanged != null)
            {
                BillingAddressChanged(this, e);
            }
        }

        protected void SendShippingAddressChangedEvent()
        {
            OnShippingAddressChanged(new AddressEventArgs(this.ShippingAddress));
        }

        protected void SendBillingAddressChangedEvent()
        {
            OnBillingAddressChanged(new AddressEventArgs(this.BillingAddress));
        }
	}

    public class AddressEventArgs : System.EventArgs
    {
        public IAddressInfo address;

        public AddressEventArgs(IAddressInfo address)
        {
            this.address = address;
        }
    }

    public delegate void ShippingAddressChangedEventHandler(object sender, AddressEventArgs e);
    public delegate void BillingAddressChangedEventHandler(object sender, AddressEventArgs e);
}
