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

using System.Collections.Generic;
using DotNetNuke.Common.Utilities;

namespace DotNetNuke.Modules.Store.Providers.Tax.DefaultTaxProvider
{
	/// <summary>
	/// Summary description for TaxController.
	/// </summary>
    public sealed class TaxController : ProviderControllerBase, ITaxProvider
	{
		#region Public Methods

		public TaxInfo GetTaxRates(int portalID) 
		{
            TaxInfo taxInfo = (TaxInfo)DataCache.GetCache("StoreTaxRates" + portalID);
            if (taxInfo == null)
            {
                taxInfo = CBO.FillObject<TaxInfo>(DataProvider.Instance().GetTaxRates(portalID));
                if (taxInfo != null)
                    DataCache.SetCache("StoreTaxRates" + portalID, taxInfo, false);
            }
            return taxInfo;
        }

		public void UpdateTaxRates(int portalID, decimal rate, bool showTax)
		{
			DataProvider.Instance().UpdateTaxRates(portalID, rate, showTax);
            DataCache.RemoveCache("StoreTaxRates" + portalID);
		}

		#endregion

		#region ITaxProvider Members

		/// <summary>
		/// Calculate tax according the region in the deliveryAddress argument.
		/// </summary>
		/// <param name="portalID">ID of the portal</param>
        /// <param name="cartItems">List of ICartItemInfo that need to have taxes calculated on.</param>
		/// <param name="shippingInfo">ShippingInfo in the case that taxes need to be applied to shipping</param>
		/// <param name="deliveryAddress">The address that the taxes should be applied for.</param>
		/// <returns>ITaxInfo with the total amount of tax due for the cart items shipping cost.</returns>
        public ITaxInfo CalculateSalesTax<T>(int portalID, List<T> cartItems, Shipping.IShippingInfo shippingInfo, Address.IAddressInfo deliveryAddress) where T : Cart.ICartItemInfo
		{
			TaxInfo taxInfo = GetTaxRates(portalID);

            if (taxInfo != null && taxInfo.ShowTax)
            {
                decimal taxRate = (taxInfo.DefaultTaxRate / 100);
                // Compute Shipping Tax if needed
                taxInfo.ShippingTax = shippingInfo.ApplyTaxRate ? shippingInfo.Cost * taxRate : 0M;
                // Compute Sales Tax
                decimal cartTotal = 0M;
                foreach (T itemInfo in cartItems)
                    cartTotal += (itemInfo.SubTotal - itemInfo.Discount) * taxRate;
                taxInfo.SalesTax = cartTotal + taxInfo.ShippingTax;
            }
            else
            {
                if (taxInfo == null)
                    taxInfo = new TaxInfo();
                taxInfo.ShowTax = false;
                taxInfo.DefaultTaxRate = 0M;
                taxInfo.SalesTax = 0M;
                taxInfo.ShippingTax = 0M;
            }
			return taxInfo;
		}

        public ITaxInfo GetDefautTaxRates(int portalID)
        {
            return GetTaxRates(portalID);
        }

		#endregion
	}
}
