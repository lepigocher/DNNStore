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

namespace DotNetNuke.Modules.Store.Customer
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

		static DataProvider() 
		{ 
			CreateProvider(); 
		} 

		private static void CreateProvider() 
		{ 
			_provider = (DataProvider)Reflection.CreateObject("data", "DotNetNuke.Modules.Store.Customer", "DotNetNuke.Modules.Store.Customer");
		} 

		public static DataProvider Instance() 
		{ 
			return _provider; 
		}

		#endregion

		#region Abstract Methods

		//Orders
		public abstract IDataReader CreateOrder(string cartID);
		public abstract IDataReader GetCustomerOrders(int portalID, int customerID);
        public abstract IDataReader GetOrder(int portalID, int orderID);
		public abstract void UpdateOrder(int orderID, DateTime orderDate, string orderNumber, int shippingAddressID, int billingAddressID, decimal tax, decimal shippingCost, int couponID, decimal discount, int customerID);
        public abstract void UpdateOrder(int orderID, DateTime orderDate, string orderNumber, int shippingAddressID, int billingAddressID, decimal tax, decimal shippingCost, int couponID, decimal discount, bool orderIsPlaced, int customerID);
        public abstract void UpdateOrder(int orderID, DateTime orderDate, string orderNumber, int shippingAddressID, int billingAddressID, decimal tax, decimal shippingCost, int couponID, decimal discount, bool orderIsPlaced, int orderStatusID, int customerID);
		public abstract IDataReader GetCustomers(int portalID);
        public abstract IDataReader GetOrders(int portalID, int orderStatusID);
        public abstract void UpdateStockQuantity(int productID, int quantity);
        public abstract IDataReader GetDownloads(int portalID, int customerID);
	    public abstract int CountCouponUsage(int portalID, int customerID, int couponID, int orderID);
	    public abstract decimal GetTotalOrdered(int portalID, int customerID);

		//Order Detail
		public abstract IDataReader GetOrderDetails(int orderID);
        public abstract IDataReader GetOrderDetail(int orderDetailID);
		public abstract IDataReader UpdateOrderDetails(int orderID, string cartID);
        public abstract IDataReader UpdateOrderDetail(int orderDetailID, int orderID, int productID, int quantity, decimal unitCost, int roleID, int downloads);

        //Order Status
        public abstract IDataReader GetOrderStatuses();

		#endregion
	}
}
