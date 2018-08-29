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
using System;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Localization;

using DotNetNuke.Modules.Store.Core.Components;

namespace DotNetNuke.Modules.Store.Core.Customer
{
    /// <summary>
    /// Order business class used to manage customer orders.
    /// </summary>
    public sealed class OrderController
	{
		#region Public Methods
		
		public OrderInfo CreateOrder(string cartID)
		{
            return CBO.FillObject<OrderInfo>(DataProvider.Instance().ExecuteReader("Store_Orders_CreateOrder", cartID));
		}

        public void UpdateOrder(int orderID, DateTime orderDate, string orderNumber, int shippingAddressID, int billingAddressID, decimal tax, decimal shippingCost, int couponID, decimal discount, int customerID)
		{
            UpdateOrder(orderID,
                orderDate,
                orderNumber,
                shippingAddressID,
                billingAddressID,
                tax,
                shippingCost,
                couponID,
                discount,
                false,
                1,
                customerID);
		}

        public void UpdateOrder(int orderID, DateTime orderDate, string orderNumber, int shippingAddressID, int billingAddressID, decimal tax, decimal shippingCost, int couponID, decimal discount, bool orderIsPlaced, int customerID)
        {
            UpdateOrder(orderID,
                orderDate,
                orderNumber,
                shippingAddressID,
                billingAddressID,
                tax,
                shippingCost,
                couponID, 
                discount,
                orderIsPlaced,
                1,
                customerID);
        }

        public void UpdateOrder(int orderID, DateTime orderDate, string orderNumber, int shippingAddressID, int billingAddressID, decimal tax, decimal shippingCost, int couponID, decimal discount, bool orderIsPlaced, int orderStatusID, int customerID)
        {
            DataProvider.Instance().ExecuteNonQuery("Store_Orders_UpdateOrder",
                orderID,
                orderDate,
                orderNumber,
                shippingAddressID,
                billingAddressID,
                tax,
                shippingCost,
                DataHelper.GetNull(couponID),
                DataHelper.GetNull(discount),
                orderIsPlaced,
                orderStatusID,
                customerID);
        }

        public void PurgeOrders(DateTime purgeDate)
        {
            DataProvider.Instance().ExecuteNonQuery("Store_Orders_PurgeOrders", purgeDate);
        }

        public OrderInfo UpdateOrderDetails(int orderID, string cartID)
		{
			return CBO.FillObject<OrderInfo>(DataProvider.Instance().ExecuteReader("Store_Orders_UpdateOrderDetails", orderID, cartID));
		}

        public OrderInfo UpdateOrderDetail(int orderDetailID, int orderID, int productID, int quantity, decimal unitCost, int roleID, int downloads)
		{
            return CBO.FillObject<OrderInfo>(DataProvider.Instance().ExecuteReader("Store_Orders_UpdateOrderDetail",
                orderDetailID,
                orderID,
                productID,
                quantity,
                unitCost,
                DataHelper.GetNull(roleID),
                DataHelper.GetNull(downloads)));
		}

		public OrderInfo GetOrder(int portalID, int orderID)
		{
            return CBO.FillObject<OrderInfo>(DataProvider.Instance().ExecuteReader("Store_Orders_GetOrder", portalID, orderID));
		}

        public List<OrderInfo> GetOrders(int portalID, int orderStatusID)
        {
            return CBO.FillCollection<OrderInfo>(DataProvider.Instance().ExecuteReader("Store_Orders_GetOrdersByStatusID", portalID, orderStatusID));
        }

        public List<OrderInfo> GetCustomerOrders(int portalID, int userID)
		{
			return CBO.FillCollection<OrderInfo>(DataProvider.Instance().ExecuteReader("Store_Orders_GetCustomerOrders", portalID, userID));
		}

        public List<OrderStatus> GetOrderStatuses()
        {
            List<OrderStatus> lstOrderStatus = CBO.FillCollection<OrderStatus>(DataProvider.Instance().ExecuteReader("Store_Orders_GetOrderStatuses"));

            // Get the corresponding localized OrderStatusText
            foreach (OrderStatus orderStatus in lstOrderStatus)
            {
                orderStatus.OrderStatusText = Localization.GetString(orderStatus.OrderStatusText, "~/DesktopModules/Store/App_LocalResources/CustomerOrders.ascx");
            }
            return lstOrderStatus;
        }

        public List<OrderStatus> GetOrderStatuses(string language)
        {
            List<OrderStatus> lstOrderStatus = CBO.FillCollection<OrderStatus>(DataProvider.Instance().ExecuteReader("Store_Orders_GetOrderStatuses"));

            // Get the corresponding localized OrderStatusText
            foreach (OrderStatus orderStatus in lstOrderStatus)
            {
                orderStatus.OrderStatusText = Localization.GetString(orderStatus.OrderStatusText, "~/DesktopModules/Store/App_LocalResources/CustomerOrders.ascx", language);
            }
            return lstOrderStatus;
        }

        public string GetStatusText(int orderStatusID)
        {
            List<OrderStatus> lstOrderStatus = GetOrderStatuses();
            string statusText = "";

            // Find status text
            foreach (OrderStatus status in lstOrderStatus)
            {
                if (status.OrderStatusID == orderStatusID)
                {
                    statusText = status.OrderStatusText;
                    break;
                }
            }
            return statusText;
        }

        public string GetStatusText(int orderStatusID, string language)
        {
            List<OrderStatus> lstOrderStatus = GetOrderStatuses(language);
            string statusText = "";

            // Find status text
            foreach (OrderStatus status in lstOrderStatus)
            {
                if (status.OrderStatusID == orderStatusID)
                {
                    statusText = status.OrderStatusText;
                    break;
                }
            }
            return statusText;
        }

        public List<OrderDetailInfo> GetOrderDetails(int orderID)
		{
            return CBO.FillCollection<OrderDetailInfo>(DataProvider.Instance().ExecuteReader("Store_Orders_GetOrderDetails", orderID));
		}

        public OrderDetailInfo GetOrderDetail(int orderDetailID)
		{
            return CBO.FillObject<OrderDetailInfo>(DataProvider.Instance().ExecuteReader("Store_Orders_GetOrderDetail", orderDetailID));
		}

		public List<CustomerInfo> GetCustomers(int portalID)
		{
			return CBO.FillCollection<CustomerInfo>(DataProvider.Instance().ExecuteReader("Store_Orders_GetCustomers", portalID));
		}

        public void UpdateStockQuantity(int productID, int quantity)
        {
            DataProvider.Instance().ExecuteNonQuery("Store_Orders_UpdateStockQuantity", productID, quantity);
        }

        public List<DownloadInfo> GetDownloads(int portalID, int userID)
        {
            return CBO.FillCollection<DownloadInfo>(DataProvider.Instance().ExecuteReader("Store_Orders_GetDownloads", portalID, userID));
        }

        public int CountCouponUsage(int portalID, int userID, int couponID, int currentOrderID)
        {
            return DataProvider.Instance().ExecuteScalar<int>("Store_Orders_CountCouponUsage", portalID, userID, couponID, currentOrderID);
        }

        public decimal GetTotalOrdered(int portalID, int userID)
        {
            return DataProvider.Instance().ExecuteScalar<decimal>("Store_Orders_GetTotalOrdered", portalID, userID);
        }

        public void AddUserToRoles(Int32 portalID, OrderInfo order)
        {
            int userID = order.CustomerID;
            RoleController roleController = new RoleController();
            List<OrderDetailInfo> orderDetails = GetOrderDetails(order.OrderID);

            foreach (OrderDetailInfo detail in orderDetails)
            {
                int roleID = detail.RoleID;
                if (roleID != Null.NullInteger)
                {
                    RoleInfo role = roleController.GetRoleById(portalID, roleID);
                    DateTime today = DateTime.Now;
                    DateTime expiryDate = Null.NullDate;
                    // If a billing period is defined
                    if (role.BillingPeriod != Null.NullInteger)
                    {
                        switch (role.BillingFrequency)
                        {
                            case "D":
                                expiryDate = today.AddDays(role.BillingPeriod * detail.Quantity);
                                break;
                            case "M":
                                expiryDate = today.AddMonths(role.BillingPeriod * detail.Quantity);
                                break;
                            case "W":
                                expiryDate = today.AddDays(role.BillingPeriod * 7 * detail.Quantity);
                                break;
                            case "Y":
                                expiryDate = today.AddYears(role.BillingPeriod * detail.Quantity);
                                break;
                            default:
                                // If 'None' or 'One Time Fee' is defined,
                                // then the role never expire!
                                break;
                        }
                    }
                    // If an expiry date is defined
                    if (expiryDate != Null.NullDate)
                    {
                        // Verify if the user is already in the corresponding role
                        UserRoleInfo userRole = roleController.GetUserRole(portalID, userID, roleID);
                        if (userRole != null)
                        {
                            DateTime roleExpiryDate = userRole.ExpiryDate;
                            // If remaining days exist
                            if (roleExpiryDate != Null.NullDate && roleExpiryDate > today)
                            {
                                TimeSpan remain = roleExpiryDate - today;
                                expiryDate = expiryDate.Add(remain);
                            }
                        }
                    }
                    roleController.AddUserRole(portalID, userID, roleID, DateTime.Now, expiryDate);
                }
            }
        }

	    public void AddUserToPaidOrderRole(Int32 portalID, int userID, int roleID)
	    {
	        if (userID != Null.NullInteger && roleID != Null.NullInteger)
	        {
	            RoleController roleController = new RoleController();
                RoleInfo role = roleController.GetRoleById(portalID, roleID);
                if (role != null)
                    roleController.AddUserRole(portalID, userID, roleID, DateTime.Now, Null.NullDate);
            }
        }

	    public void RemoveUserFromRoles(Int32 portalID, OrderInfo order)
        {
            int userID = order.CustomerID;

            RoleController roleController = new RoleController();
            List<OrderDetailInfo> orderDetails = GetOrderDetails(order.OrderID);

            foreach (OrderDetailInfo detail in orderDetails)
            {
                int roleID = detail.RoleID;
                if (roleID != Null.NullInteger)
                {
                    DateTime today = DateTime.Now;
                    UserRoleInfo userRole = roleController.GetUserRole(portalID, userID, roleID);
                    if (userRole != null)
                    {
                        DateTime roleExpiryDate = userRole.ExpiryDate;
                        // If remaining days exist
                        if (roleExpiryDate != Null.NullDate && roleExpiryDate > today)
                        {
                            RoleInfo role = roleController.GetRoleById(portalID, roleID);
                            DateTime expiryDate = Null.NullDate;
                            // If a billing period is defined
                            if (role.BillingPeriod != Null.NullInteger)
                            {
                                switch (role.BillingFrequency)
                                {
                                    case "D":
                                        expiryDate = roleExpiryDate.AddDays(-(role.BillingPeriod * detail.Quantity));
                                        break;
                                    case "M":
                                        expiryDate = roleExpiryDate.AddMonths(-(role.BillingPeriod * detail.Quantity));
                                        break;
                                    case "W":
                                        expiryDate = roleExpiryDate.AddDays(-(role.BillingPeriod * 7 * detail.Quantity));
                                        break;
                                    case "Y":
                                        expiryDate = roleExpiryDate.AddYears(-(role.BillingPeriod * detail.Quantity));
                                        break;
                                    default:
                                        // If 'None' or 'One Time Fee' is defined,
                                        // then the role never expire!
                                        break;
                                }
                            }
                            // If an expiry date is defined
                            if (expiryDate != Null.NullDate)
                                roleController.AddUserRole(portalID, userID, roleID, userRole.EffectiveDate, expiryDate);
                        }
                    }
                }
            }
        }

        #endregion
	}
}
