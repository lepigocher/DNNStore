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
using DotNetNuke.Modules.Store.Providers.Address;
using DotNetNuke.Modules.Store.Providers.Cart;

namespace DotNetNuke.Modules.Store.Providers.Shipping.DefaultShippingProvider
{
	/// <summary>
	/// Summary description for AddresssController.
	/// </summary>
    public sealed class ShippingController : ProviderControllerBase, IShippingProvider
	{
		#region Public Methods

		public ShippingInfo GetShippingRate(int portalId, int shippingRateID)
		{
            return CBO.FillObject< ShippingInfo>(DataProvider.Instance().GetShippingRate(portalId, shippingRateID));
		}

        public List<ShippingInfo> GetShippingRates(int portalId)
        {
            return CBO.FillCollection<ShippingInfo>(DataProvider.Instance().GetShippingRates(portalId)); 
        }

		public void UpdateShippingRate(ShippingInfo shippingInfo)
		{
            DataProvider.Instance().UpdateShippingRate(shippingInfo.ID, shippingInfo.Description, shippingInfo.MinWeight, shippingInfo.MaxWeight, shippingInfo.Cost, shippingInfo.ApplyTaxRate);
		}

        public void AddShippingRate(int portalID, ShippingInfo shippingInfo)
        {
            DataProvider.Instance().AddShippingRate(portalID, shippingInfo.Description, shippingInfo.MinWeight, shippingInfo.MaxWeight, shippingInfo.Cost, shippingInfo.ApplyTaxRate);
        }

        public void DeleteShippingRate(int shippingRateID)
        {
            DataProvider.Instance().DeleteShippingRate(shippingRateID);
        }

		#endregion

		#region IShippingProvider Members

        public IShippingInfo CalculateShippingFee<T>(int portalId, IAddressInfo billingAddress, IAddressInfo shippingAddress, List<T> cartItems) where T : ICartItemInfo
        {
            decimal cartWeight = 0;
            foreach (T itemInfo in cartItems)
            {
                cartWeight += (itemInfo.ProductWeight * itemInfo.Quantity);
            }
            return CBO.FillObject<ShippingInfo>(DataProvider.Instance().GetShippingFee(portalId, cartWeight));
        }

        public IShippingInfo GetFreeShipping()
        {
            return new ShippingInfo();
        }

		#endregion
    }
}
