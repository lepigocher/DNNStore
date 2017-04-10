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
using Microsoft.ApplicationBlocks.Data;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;

namespace DotNetNuke.Modules.Store.Admin
{
	/// <summary>
    /// Concrete store data provider for SQL Server.
	/// </summary>
    public sealed class SqlDataProvider : DataProvider
	{
		#region Private Members

		private const string ProviderType = "data";
		private readonly ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
		private readonly string _connectionString;
		private readonly string _providerPath;
		private readonly string _objectQualifier;
		private readonly string _databaseOwner;

		#endregion

		#region Constructors

		public SqlDataProvider()
		{
			Provider provider = (Provider)_providerConfiguration.Providers[_providerConfiguration.DefaultProvider];
            _connectionString = Config.GetConnectionString();
            if (string.IsNullOrEmpty(_connectionString))
                _connectionString = provider.Attributes["connectionString"];
			
			_providerPath = provider.Attributes["providerPath"]; 
			_objectQualifier = provider.Attributes["objectQualifier"]; 
			
			if (!string.IsNullOrEmpty(_objectQualifier) && !_objectQualifier.EndsWith("_")) 
				_objectQualifier += "_"; 
			
			_databaseOwner = provider.Attributes["databaseOwner"]; 
			
			if (!string.IsNullOrEmpty(_databaseOwner) && !_databaseOwner.EndsWith(".")) 
				_databaseOwner += "."; 
		}

		#endregion

		#region Properties

		public string ConnectionString 
		{
            get { return _connectionString; } 
		} 

		public string ProviderPath 
		{
            get { return _providerPath; } 
		} 

		public string ObjectQualifier 
		{
            get { return _objectQualifier; } 
		} 

		public string DatabaseOwner 
		{
            get { return _databaseOwner; } 
		}

		#endregion

		#region Private Methods

		private static object GetNull(object field) 
		{ 
			return Null.GetNull(field, DBNull.Value); 
		} 

		#endregion

		#region Public Methods

        public override Int32 AddStoreInfo(int portalID, string name, string description, string keywords, bool seoFeature, string gatewayName, string gatewaySettings, string defaultEmailAddress, int shoppingCartPageID, string createdByUser, int storePageID, string currencySymbol, bool portalTemplates, string styleSheet, bool authorizeCancel, bool inventoryManagement, int outOfStock, int productsBehavior, bool avoidNegativeStock, int orderRoleID, int catalogRoleID, bool secureCookie, int checkoutMode, int impersonatedUserID, bool noDelivery, bool allowVirtualProducts, bool allowCoupons, bool allowFreeShipping, decimal minOrderAmount, bool restrictToCountries, string authorizedCountries, int onOrderPaidRoleID, string taxName, string shippingName, string addressName)
		{
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Administration_AddStoreInfo", portalID, name, GetNull(description), GetNull(keywords), seoFeature, gatewayName, GetNull(gatewaySettings), defaultEmailAddress, GetNull(shoppingCartPageID), createdByUser, GetNull(storePageID), GetNull(currencySymbol), portalTemplates, styleSheet, authorizeCancel, inventoryManagement, outOfStock, productsBehavior, avoidNegativeStock, orderRoleID, catalogRoleID, secureCookie, checkoutMode, GetNull(impersonatedUserID), noDelivery, allowVirtualProducts, allowCoupons, allowFreeShipping, minOrderAmount, restrictToCountries, authorizedCountries, onOrderPaidRoleID, taxName, shippingName, addressName));
		}

        public override void UpdateStoreInfo(int portalID, string name, string description, string keywords, bool seoFeature, string gatewayName, string gatewaySettings, string defaultEmailAddress, int shoppingCartPageID, int storePageID, string currencySymbol, bool portalTemplates, string styleSheet, bool authorizeCancel, bool inventoryManagement, int outOfStock, int productsBehavior, bool avoidNegativeStock, int orderRoleID, int catalogRoleID, bool secureCookie, int checkoutMode, int impersonatedUserID, bool noDelivery, bool allowVirtualProducts, bool allowCoupons, bool allowFreeShipping, decimal minOrderAmount, bool restrictToCountries, string authorizedCountries, int onOrderPaidRoleID, string taxName, string shippingName, string addressName)
		{
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Administration_UpdateStoreInfo", portalID, name, GetNull(description), GetNull(keywords), seoFeature, gatewayName, GetNull(gatewaySettings), defaultEmailAddress, GetNull(shoppingCartPageID), GetNull(storePageID), GetNull(currencySymbol), portalTemplates, styleSheet, authorizeCancel, inventoryManagement, outOfStock, productsBehavior, avoidNegativeStock, orderRoleID, catalogRoleID, secureCookie, checkoutMode, GetNull(impersonatedUserID), noDelivery, allowVirtualProducts, allowCoupons, allowFreeShipping, minOrderAmount, restrictToCountries, authorizedCountries, onOrderPaidRoleID, taxName, shippingName, addressName);
		}

		public override IDataReader GetStoreInfo(int portalID)
		{
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Administration_GetStoreInfo", portalID);
		}

//		public override Int32 AddStoreProvider(int PortalID, int ProviderType, string Name, string Path)
//		{
//			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Administration_AddStoreProvider", PortalID, ProviderType, Name, Path));
//		}
//
//		public override void UpdateStoreProvider(int PortalID, int ProviderType, string Name, string Path)
//		{
//			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Administration_UpdateStoreProvider", PortalID, ProviderType, Name, Path);
//		}
//
//		public override IDataReader GetStoreProvider(int PortalID, int ProviderType)
//		{
//			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Administration_GetStoreProvider", PortalID, ProviderType);
//		}
//
//		public override IDataReader GetStoreProviders(int PortalID)
//		{
//			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Administration_GetStoreProviders", PortalID);
//		}

		#endregion
	}
}
