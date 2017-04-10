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

namespace DotNetNuke.Modules.Store.Customer
{
	/// <summary>
    /// Concrete customer and order data provider for SQL Server.
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
			Provider objProvider = (Provider)_providerConfiguration.Providers[_providerConfiguration.DefaultProvider];

            _connectionString = Config.GetConnectionString();
            if (string.IsNullOrEmpty(_connectionString))
                _connectionString = objProvider.Attributes["connectionString"];
			
			_providerPath = objProvider.Attributes["providerPath"]; 
			_objectQualifier = objProvider.Attributes["objectQualifier"]; 
			
			if (!string.IsNullOrEmpty(_objectQualifier) && !_objectQualifier.EndsWith("_")) 
				_objectQualifier += "_"; 
			
			_databaseOwner = objProvider.Attributes["databaseOwner"]; 
			
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

		private object GetNull(object Field) 
		{ 
			return Null.GetNull(Field, DBNull.Value); 
		} 

		#endregion

		#region Public Methods

        public override IDataReader CreateOrder(string cartID)
		{
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Orders_CreateOrder", cartID);
		}

        public override IDataReader GetCustomerOrders(Int32 portalID, int userID)
		{
			return  SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Orders_GetCustomerOrders", portalID, userID);
		}

        public override IDataReader GetOrders(Int32 portalID, int orderStatusID)
        {
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Orders_GetOrdersByStatusID", portalID, orderStatusID);
        }

		public override IDataReader GetOrder(int portalID, int orderID)
		{
			return  SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Orders_GetOrder", portalID, orderID);
		}

		public override IDataReader GetOrderDetails(Int32 orderID)
		{
			return  SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Orders_GetOrderDetails", orderID);
		}

        public override IDataReader GetOrderDetail(int orderDetailID)
        {
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Orders_GetOrderDetail", orderDetailID);
        }

		public override IDataReader UpdateOrderDetails(Int32 orderID, string cartID)
		{
			return  SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Orders_UpdateOrderDetails", orderID, cartID);
		}

        public override IDataReader UpdateOrderDetail(int orderDetailID, int orderID, int productID, int quantity, decimal unitCost, int roleID, int downloads)
        {
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Orders_UpdateOrderDetail", orderDetailID, orderID, productID, quantity, unitCost, GetNull(roleID), GetNull(downloads));
        }

        public override void UpdateOrder(int orderID, DateTime orderDate, string orderNumber, int shippingAddressID, int billingAddressID, decimal tax, decimal shippingCost, int couponID, decimal discount, int customerID)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Orders_UpdateOrder", orderID, orderDate, orderNumber, shippingAddressID, billingAddressID, tax, shippingCost, GetNull(couponID), GetNull(discount), false, 1, customerID);
        }

        public override void UpdateOrder(int orderID, DateTime orderDate, string orderNumber, int shippingAddressID, int billingAddressID, decimal tax, decimal shippingCost, int couponID, decimal discount, bool orderIsPlaced, int customerID)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Orders_UpdateOrder", orderID, orderDate, orderNumber, shippingAddressID, billingAddressID, tax, shippingCost, GetNull(couponID), GetNull(discount), orderIsPlaced, 1, customerID);
        }

        public override void UpdateOrder(int orderID, DateTime orderDate, string orderNumber, int shippingAddressID, int billingAddressID, decimal tax, decimal shippingCost, int couponID, decimal discount, bool orderIsPlaced, int orderStatusID, int customerID)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Orders_UpdateOrder", orderID, orderDate, orderNumber, shippingAddressID, billingAddressID, tax, shippingCost, GetNull(couponID), GetNull(discount), orderIsPlaced, orderStatusID, customerID);
        }

        public override IDataReader GetOrderStatuses()
        {
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Orders_GetOrderStatuses");
        }

		public override IDataReader GetCustomers(int portalID)
		{
			return  SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Orders_GetCustomers", portalID);
		}

        public override void UpdateStockQuantity(int productID, int quantity)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Orders_UpdateStockQuantity", productID, quantity);
        }

        public override IDataReader GetDownloads(Int32 portalID, int userID)
        {
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Orders_GetDownloads", portalID, userID);
        }

        public override int CountCouponUsage(int portalID, int userID, int couponID, int orderID)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Orders_CountCouponUsage", portalID, userID, couponID, orderID));
        }

        public override decimal GetTotalOrdered(int portalID, int userID)
        {
            return Convert.ToDecimal(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Orders_GetTotalOrdered", portalID, userID));
        }

		#endregion
	}
}
