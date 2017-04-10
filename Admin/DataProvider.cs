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
using System.Data;
using DotNetNuke.Framework;

namespace DotNetNuke.Modules.Store.Admin
{
	/// <summary>
	/// DataProvider abstract class.
	/// </summary>
	public abstract class DataProvider
	{
		#region Private Members

		private static DataProvider _provider; 

		#endregion

		#region Constructors

        /// <summary>
        /// Static constructor used to initialize a singleton of the concrete data provider.
        /// </summary>
		static DataProvider() 
		{ 
			CreateProvider(); 
		} 

        /// <summary>
        /// Create the singleton of the concrete data provider.
        /// </summary>
		private static void CreateProvider() 
		{ 
			_provider = ((DataProvider)(Reflection.CreateObject("data", "DotNetNuke.Modules.Store.Admin", "DotNetNuke.Modules.Store.Admin"))); 
		} 

        /// <summary>
        /// Used to access the singleton.
        /// </summary>
        /// <returns>An instance of the concrete data provider</returns>
		public static DataProvider Instance() 
		{ 
			return _provider; 
		}

		#endregion

		#region Abstract Methods

        public abstract Int32 AddStoreInfo(int portalID, string name, string description, string keywords, bool seoFeature, string gatewayName, string gatewaySettings, string defaultEmailAddress, int shoppingCartPageID, string createdByUser, int storePageID, string currencySymbol, bool portalTemplates, string styleSheet, bool authorizeCancel, bool inventoryManagement, int outOfStock, int productsBehavior, bool avoidNegativeStock, int orderRoleID, int catalogRoleID, bool secureCookie, int checkoutMode, int impersonatedUserID, bool noDelivery, bool allowVirtualProducts, bool allowCoupons, bool allowFreeShipping, decimal minOrderAmount, bool restrictToCountries, string authorizedCountries, int onOrderPaidRoleID, string taxName, string shippingName, string addressName);
        public abstract void UpdateStoreInfo(int portalID, string name, string description, string keywords, bool seoFeature, string gatewayName, string gatewaySettings, string defaultEmailAddress, int shoppingCartPageID, int storePageID, string currencySymbol, bool portalTemplates, string styleSheet, bool authorizeCancel, bool inventoryManagement, int outOfStock, int productsBehavior, bool avoidNegativeStock, int orderRoleID, int catalogRoleID, bool secureCookie, int checkoutMode, int impersonatedUserID, bool noDelivery, bool allowVirtualProducts, bool allowCoupons, bool allowFreeShipping, decimal minOrderAmount, bool restrictToCountries, string authorizedCountries, int onOrderPaidRoleID, string taxName, string shippingName, string addressName);
        public abstract IDataReader GetStoreInfo(int portalID);

//		public abstract Int32 AddStoreProvider(int PortalID, int ProviderType, string Name, string Path);
//		public abstract void UpdateStoreProvider(int PortalID, int ProviderType, string Name, string Path);
//		public abstract IDataReader GetStoreProvider(int PortalID, int ProviderType);
//		public abstract IDataReader GetStoreProviders(int PortalID);

		#endregion
	}
}
