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
using System.Globalization;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Tokens;

namespace DotNetNuke.Modules.Store.Customer
{
    /// <summary>
    /// Order class info.
    /// </summary>
    [Serializable]
    public sealed class OrderInfo : IHydratable, IEquatable<OrderInfo>, IPropertyAccess
    {
        #region Private Members

        private int _orderID;
        private int _customerID;
        private string _orderNumber;
        private DateTime _orderDate;
        private DateTime _shipDate;
        private int _shippingAddressID;
        private int _billingAddressID;
        private decimal _orderTotal;
        private decimal _taxTotal;
        private decimal _shippingCost;
        private decimal _shippingTax;
        private int _couponID;
        private decimal _discount ;
        private int _orderStatusID;
        private string _status;
        private string _dateFormat;
        private string _currencySymbol;
        private string _commentToCustomer;

        #endregion

        #region Properties

        public enum OrderStatusList
        {
            Processing = 1,
            AwaitingPayment = 2,
            AwaitingStock = 3,
            Packing = 4,
            Dispatched = 5,
            Cancelled = 6,
            Paid = 7
        }

        public int OrderID
        {
            get { return _orderID; }
            set { _orderID = value; }
        }

        public int CustomerID
        {
            get { return _customerID; }
            set { _customerID = value; }
        }

        public string OrderNumber
        {
            get { return _orderNumber; }
            set { _orderNumber = value; }
        }

        public DateTime OrderDate
        {
            get { return _orderDate; }
            set { _orderDate = value; }
        }

        public DateTime ShipDate
        {
            get { return _shipDate; }
            set { _shipDate = value; }
        }

        public int ShippingAddressID
        {
            get { return _shippingAddressID; }
            set { _shippingAddressID = value; }
        }

        public int BillingAddressID
        {
            get { return _billingAddressID; }
            set { _billingAddressID = value; }
        }

        /// <summary>
        /// The subtotal of the cart items in the order.
        /// NOTE: Does NOT include tax and shipping charges.
        /// </summary>
        public decimal OrderTotal
        {
            get { return _orderTotal; }
            set { _orderTotal = value; }
        }

        /// <summary>
        /// The order net total is equal to the order total minus discount amount.
        /// </summary>
        public decimal OrderNetTotal
        {
            get { return _orderTotal + (_discount == Null.NullDecimal ? 0 : _discount); }
        }

        /// <summary>
        /// The grand total summing the OrderTotal, Discount, TaxTotal, and ShippingCost
        /// </summary>
        public decimal GrandTotal
        {
            get
            {
                if (_couponID != Null.NullInteger)
                    return _orderTotal + _discount + _taxTotal + _shippingCost;

                return _orderTotal + _taxTotal + _shippingCost;
            }
        }

        /// <summary>
        /// Total tax that has been calculated for this order.
        /// </summary>
        public decimal TaxTotal
        {
            get { return _taxTotal; }
            set { _taxTotal = value; }
        }

        /// <summary>
        /// Shiping costs associated with this order.
        /// </summary>
        public decimal ShippingCost
        {
            get { return _shippingCost; }
            set { _shippingCost = value; }
        }

        /// <summary>
        /// Shiping tax that has been calculated for this order.
        /// </summary>
        public decimal ShippingTax
        {
            get { return _shippingTax; }
            set { _shippingTax = value; }
        }

        /// <summary>
        /// Coupon ID that has been used for this order.
        /// </summary>
        public int CouponID
        {
            get { return _couponID; }
            set { _couponID = value; }
        }

        /// <summary>
        /// Discount that has been applied to this order.
        /// </summary>
        public decimal Discount
        {
            get { return _discount; }
            set { _discount = value; }
        }

        public int OrderStatusID
        {
            get { return _orderStatusID; }
            set { _orderStatusID = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public string DateFormat
        {
            get { return _dateFormat; }
            set { _dateFormat = value; }
        }

        public string CurrencySymbol
        {
            get { return _currencySymbol; }
            set { _currencySymbol = value; }
        }

        public string CommentToCustomer
        {
            get { return _commentToCustomer; }
            set { _commentToCustomer = value; }
        }

        #endregion

        #region Object Overrides

        public override int GetHashCode()
        {
            return _orderID.GetHashCode();
        }

        #endregion

        #region IHydratable Interface

        public void Fill(System.Data.IDataReader dr)
        {
            _orderID = Convert.ToInt32(dr["OrderID"]);
            _customerID = Convert.ToInt32(Null.SetNull(dr["CustomerID"], _customerID));
            _orderNumber = Convert.ToString(Null.SetNull(dr["OrderNumber"], _orderNumber));
            _orderDate = Convert.ToDateTime(dr["OrderDate"]);
            _shipDate = Convert.ToDateTime(dr["ShipDate"]);
            _shippingAddressID = Convert.ToInt32(dr["ShippingAddressID"]);
            _billingAddressID = Convert.ToInt32(dr["BillingAddressID"]);
            _orderTotal = Convert.ToDecimal(Null.SetNull(dr["OrderTotal"], _orderTotal));
            _taxTotal = Convert.ToDecimal(Null.SetNull(dr["Tax"], _taxTotal));
            _shippingCost = Convert.ToDecimal(Null.SetNull(dr["ShippingCost"], _shippingCost));
            _orderStatusID = Convert.ToInt32(dr["OrderStatusID"]);
            _couponID = Convert.ToInt32(Null.SetNull(dr["CouponID"], _couponID));
            _discount = Convert.ToDecimal(Null.SetNull(dr["Discount"], _discount));
            //_ShippingTax = Convert.ToDecimal(Null.SetNull(dr["ShippingTax"], _ShippingTax));
            //mOrderIsPlaced = Convert.ToBoolean(Null.SetNull(dr["OrderIsPlaced"], mOrderIsPlaced));
        }

        public int KeyID
        {
            get { return _orderID; }
            set { _orderID = value; }
        }

        #endregion

        #region IEquatable<OrderInfo> Members

        public bool Equals(OrderInfo other)
        {
            if (other == null)
                return false;
            return _orderID.Equals(other.OrderID);
        }

        #endregion

        #region IPropertyAccess Members

        CacheLevel IPropertyAccess.Cacheability
        {
            get { return CacheLevel.fullyCacheable; }
        }

        string IPropertyAccess.GetProperty(string strPropertyName, string strFormat, CultureInfo formatProvider, UserInfo accessingUser, Scope accessLevel, ref bool propertyNotFound)
        {
            string propertyValue = null;
            switch (strPropertyName.ToLower())
            {
                case "orderid":
                    propertyValue = _orderID.ToString();
                    break;
                //case "ordernumber":
                //    propertyValue = _OrderNumber.ToString();
                //    break;
                case "orderdate":
                    if (!string.IsNullOrEmpty(strFormat))
                        propertyValue = _orderDate.ToString(strFormat);
                    else
                        propertyValue = _orderDate.ToString(_dateFormat);
                    break;
                case "shipdate":
                    if (!string.IsNullOrEmpty(strFormat))
                        propertyValue = _shipDate.ToString(strFormat);
                    else
                        propertyValue = _shipDate.ToString(_dateFormat);
                    break;
                case "ordertotal":
                    if (formatProvider != null)
                    {
                        if (!string.IsNullOrEmpty(CurrencySymbol))
                            formatProvider.NumberFormat.CurrencySymbol = CurrencySymbol;
                        if (!string.IsNullOrEmpty(strFormat))
                            propertyValue = _orderTotal.ToString(strFormat, formatProvider.NumberFormat);
                        else
                            propertyValue = _orderTotal.ToString("C", formatProvider.NumberFormat);
                    }
                    else
                        propertyValue = _orderTotal.ToString("C");
                    break;
                case "grandtotal":
                    if (formatProvider != null)
                    {
                        if (!string.IsNullOrEmpty(CurrencySymbol))
                            formatProvider.NumberFormat.CurrencySymbol = CurrencySymbol;
                        if (!string.IsNullOrEmpty(strFormat))
                            propertyValue = GrandTotal.ToString(strFormat, formatProvider.NumberFormat);
                        else
                            propertyValue = GrandTotal.ToString("C", formatProvider.NumberFormat);
                    }
                    else
                        propertyValue = GrandTotal.ToString("C");
                    break;
                case "taxtotal":
                    if (formatProvider != null)
                    {
                        if (!string.IsNullOrEmpty(CurrencySymbol))
                            formatProvider.NumberFormat.CurrencySymbol = CurrencySymbol;
                        if (!string.IsNullOrEmpty(strFormat))
                            propertyValue = _taxTotal.ToString(strFormat, formatProvider.NumberFormat);
                        else
                            propertyValue = _taxTotal.ToString("C", formatProvider.NumberFormat);
                    }
                    else
                        propertyValue = _taxTotal.ToString("C");
                    break;
                case "shippingcost":
                    if (formatProvider != null)
                    {
                        if (!string.IsNullOrEmpty(CurrencySymbol))
                            formatProvider.NumberFormat.CurrencySymbol = CurrencySymbol;
                        if (!string.IsNullOrEmpty(strFormat))
                            propertyValue = _shippingCost.ToString(strFormat, formatProvider.NumberFormat);
                        else
                            propertyValue = _shippingCost.ToString("C", formatProvider.NumberFormat);
                    }
                    else
                        propertyValue = _shippingCost.ToString("C");
                    break;
                case "shippingtax":
                    if (formatProvider != null)
                    {
                        if (!string.IsNullOrEmpty(CurrencySymbol))
                            formatProvider.NumberFormat.CurrencySymbol = CurrencySymbol;
                        if (!string.IsNullOrEmpty(strFormat))
                            propertyValue = _shippingTax.ToString(strFormat, formatProvider.NumberFormat);
                        else
                            propertyValue = _shippingTax.ToString("C", formatProvider.NumberFormat);
                    }
                    else
                        propertyValue = _shippingTax.ToString("C");
                    break;
                case "discount":
                    if (formatProvider != null)
                    {
                        if (!string.IsNullOrEmpty(CurrencySymbol))
                            formatProvider.NumberFormat.CurrencySymbol = CurrencySymbol;
                        if (!string.IsNullOrEmpty(strFormat))
                            propertyValue = _discount.ToString(strFormat, formatProvider.NumberFormat);
                        else
                            propertyValue = _discount.ToString("C", formatProvider.NumberFormat);
                    }
                    else
                        propertyValue = _discount.ToString("C");
                    break;
                case "status":
                    propertyValue = _status;
                    break;
                case "commenttocustomer":
                    propertyValue = _commentToCustomer;
                    break;
                default:
                    propertyNotFound = true;
                    break;
            }
            return propertyValue;
        }

        #endregion
    }
}
