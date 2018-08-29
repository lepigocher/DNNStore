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

using System.Collections.Generic;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using DotNetNuke.Entities.Users;

using DotNetNuke.Modules.Store.Core.Components;
using DotNetNuke.Modules.Store.Core.Providers;
using DotNetNuke.Modules.Store.Core.Providers.Address;
using DotNetNuke.Modules.Store.Core.Providers.Shipping;
using DotNetNuke.Modules.Store.Core.Providers.Tax;

namespace DotNetNuke.Modules.Store.Core.Admin
{
	/// <summary>
	/// Store business class used to manage the store settings and defined providers.
	/// </summary>
	public sealed class StoreController
	{
		#region Public Methods

        /// <summary>
        /// Get the store settings.
        /// </summary>
        /// <param name="portalID">Current portal id</param>
        /// <returns>Store settings as a StoreInfo class</returns>
		public static StoreInfo GetStoreInfo(int portalID) 
		{
            StoreInfo storeInfo = (StoreInfo)DataCache.GetCache("StoreInfo" + portalID);
            if (storeInfo == null)
            {
                storeInfo = CBO.FillObject<StoreInfo>(DataProvider.Instance().ExecuteReader("Store_Administration_GetStoreInfo", portalID));
                if (storeInfo != null)
                    DataCache.SetCache("StoreInfo" + portalID, storeInfo);
            }
            return storeInfo;
        }

        /// <summary>
        /// Add a new store
        /// </summary>
        /// <param name="storeInfo">Store settings as a StoreInfo class</param>
		public void AddStoreInfo(StoreInfo storeInfo)
		{
            DataProvider.Instance().ExecuteNonQuery("Store_Administration_AddStoreInfo",
                storeInfo.PortalID,
                storeInfo.Name,
                DataHelper.GetNull(storeInfo.Description),
                DataHelper.GetNull(storeInfo.Keywords),
                storeInfo.SEOFeature,
                storeInfo.GatewayName,
                DataHelper.GetNull(storeInfo.GatewaySettings),
                storeInfo.DefaultEmailAddress,
                DataHelper.GetNull(storeInfo.ShoppingCartPageID),
                UserController.Instance.GetCurrentUserInfo().UserID,
                DataHelper.GetNull(storeInfo.StorePageID),
                DataHelper.GetNull(storeInfo.CurrencySymbol),
                storeInfo.PortalTemplates,
                storeInfo.StyleSheet,
                storeInfo.AuthorizeCancel,
                storeInfo.InventoryManagement,
                storeInfo.OutOfStock,
                storeInfo.ProductsBehavior,
                storeInfo.AvoidNegativeStock,
                storeInfo.OrderRoleID,
                storeInfo.CatalogRoleID,
                storeInfo.SecureCookie,
                (int)storeInfo.CheckoutMode,
                DataHelper.GetNull(storeInfo.ImpersonatedUserID),
                storeInfo.NoDelivery,
                storeInfo.AllowVirtualProducts,
                storeInfo.AllowCoupons,
                storeInfo.AllowFreeShipping,
                storeInfo.MinOrderAmount,
                storeInfo.RestrictToCountries,
                storeInfo.AuthorizedCountries,
                storeInfo.OnOrderPaidRoleID,
                storeInfo.TaxName,
                storeInfo.ShippingName,
                storeInfo.AddressName);
		}

        /// <summary>
        /// Update a store
        /// </summary>
        /// <param name="storeInfo">Store settings as a StoreInfo class</param>
		public void UpdateStoreInfo(StoreInfo storeInfo)
		{
            DataCache.SetCache("StoreInfo" + storeInfo.PortalID, storeInfo);
            DataProvider.Instance().ExecuteNonQuery("Store_Administration_UpdateStoreInfo",
                storeInfo.PortalID,
                storeInfo.Name,
                DataHelper.GetNull(storeInfo.Description),
                DataHelper.GetNull(storeInfo.Keywords),
                storeInfo.SEOFeature,
                storeInfo.GatewayName,
                DataHelper.GetNull(storeInfo.GatewaySettings),
                storeInfo.DefaultEmailAddress,
                DataHelper.GetNull(storeInfo.ShoppingCartPageID),
                DataHelper.GetNull(storeInfo.StorePageID),
                DataHelper.GetNull(storeInfo.CurrencySymbol),
                storeInfo.PortalTemplates,
                storeInfo.StyleSheet,
                storeInfo.AuthorizeCancel,
                storeInfo.InventoryManagement,
                storeInfo.OutOfStock,
                storeInfo.ProductsBehavior,
                storeInfo.AvoidNegativeStock,
                storeInfo.OrderRoleID,
                storeInfo.CatalogRoleID,
                storeInfo.SecureCookie,
                (int)storeInfo.CheckoutMode,
                DataHelper.GetNull(storeInfo.ImpersonatedUserID),
                storeInfo.NoDelivery,
                storeInfo.AllowVirtualProducts,
                storeInfo.AllowCoupons,
                storeInfo.AllowFreeShipping,
                storeInfo.MinOrderAmount,
                storeInfo.RestrictToCountries,
                storeInfo.AuthorizedCountries,
                storeInfo.OnOrderPaidRoleID,
                storeInfo.TaxName,
                storeInfo.ShippingName,
                storeInfo.AddressName);
		}

        /// <summary>
        /// Get the list of installed providers for the specified type.
        /// </summary>
        /// <param name="providerType">Type of the provider</param>
        /// <returns>List of installed providers</returns>
        public static List<ProviderInfo> GetProviderList(StoreProviderType providerType)
        {
            //Initialize a provider controller
            ProviderController providerController = new ProviderController(providerType);
            //Get the provider list
            return providerController.GetProviders();
        }

        /// <summary>
        /// Get an instance of the specified address provider.
        /// </summary>
        /// <param name="providerName">Name of the provider</param>
        /// <returns>An instance of the specified provider</returns>
        public static IAddressProvider GetAddressProvider(string providerName)
		{
            string cacheKey = string.Format("Store{0}AddressProvider", providerName);
            IAddressProvider addressProvider = DataCache.GetCache(cacheKey) as IAddressProvider;
            if (addressProvider == null)
            {
                //Initialize an address provider controller
                ProviderController providerController = new ProviderController(StoreProviderType.Address);

                //Get the provider info
                ProviderInfo providerInfo = providerController.GetProvider(providerName);

                //Create an instance of the provider
                addressProvider = ProviderFactory.CreateProvider(providerInfo) as IAddressProvider;
                if (addressProvider != null)
                    DataCache.SetCache(cacheKey, addressProvider);
            }
            return addressProvider;
		}

        /// <summary>
        /// Get an instance of the specified shipping provider.
        /// </summary>
        /// <param name="providerName">Name of the provider</param>
        /// <returns>An instance of the specified provider</returns>
        public static IShippingProvider GetShippingProvider(string providerName)
		{
            string cacheKey = string.Format("Store{0}ShippingProvider", providerName);
            IShippingProvider shippingProvider = DataCache.GetCache(cacheKey) as IShippingProvider;
            if (shippingProvider == null)
            {
                //Initialize a shipping provider controller
                ProviderController providerController = new ProviderController(StoreProviderType.Shipping);

                //Get the provider info
                ProviderInfo providerInfo = providerController.GetProvider(providerName);

                //Create an instance of the provider
                shippingProvider = ProviderFactory.CreateProvider(providerInfo) as IShippingProvider;
                if (shippingProvider != null)
                    DataCache.SetCache(cacheKey, shippingProvider);
            }
            return shippingProvider;
		}

        /// <summary>
        /// Get an instance of the specified tax provider.
        /// </summary>
        /// <param name="providerName">Name of the provider</param>
        /// <returns>An instance of the specified provider</returns>
        public static ITaxProvider GetTaxProvider(string providerName)
		{
            string cacheKey = string.Format("Store{0}TaxProvider", providerName);
            ITaxProvider taxProvider = DataCache.GetCache(cacheKey) as ITaxProvider;
            if (taxProvider == null)
            {
                //Initialize a tax provider controller
                ProviderController providerController = new ProviderController(StoreProviderType.Tax);

                //Get the provider info
                ProviderInfo providerInfo = providerController.GetProvider(providerName);

                //Create an instance of the provider
                taxProvider = ProviderFactory.CreateProvider(providerInfo) as ITaxProvider;
                if (taxProvider != null)
                    DataCache.SetCache(cacheKey, taxProvider);
            }
            return taxProvider;
		}

		#endregion
	}
}
