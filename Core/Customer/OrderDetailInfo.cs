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

using System;
using System.Globalization;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Tokens;

namespace DotNetNuke.Modules.Store.Core.Customer
{
    /// <summary>
    /// Order details class info.
    /// </summary>
    [Serializable]
    public sealed class OrderDetailInfo : IHydratable, IEquatable<OrderDetailInfo>, IPropertyAccess
    {
        #region Private Members

        private int _orderDetailID;
        private int _orderID;
        private int _productID;
        private string _modelNumber;
        private string _modelName;
        private int _quantity;
        private decimal _unitCost;
        private decimal _extendedAmount;
        private int _roleID;
        private string _currencySymbol;
        private int _downloads;

        #endregion

        #region Properties

        public int OrderDetailID 
        {
            get { return _orderDetailID; }
            set { _orderDetailID = value; }
        }

        public int OrderID 
        {
          get { return _orderID; }
          set { _orderID = value; }
        }

        public int ProductID
        {
            get { return _productID; }
            set { _productID = value; }
        }

        public string ModelNumber 
        {
          get { return _modelNumber; }
          set { _modelNumber = value; }
        }

        public string ModelName 
        {
          get { return _modelName; }
          set { _modelName = value; }
        }

        public string ProductTitle
        {
          get
          {
              string title = string.Empty;
              title += _modelNumber.Trim();
              title += title == string.Empty ? _modelName.Trim() : " - " + _modelName.Trim();
              return title;
          }
        }

        public int Quantity 
        {
          get { return _quantity; }
          set { _quantity = value; }
        }

        public decimal UnitCost 
        {
          get { return _unitCost; }
          set { _unitCost = value; }
        }

        public decimal ExtendedAmount 
        {
          get { return _extendedAmount; }
          set { _extendedAmount = value; }
        }

        public int RoleID 
        {
            get { return _roleID; }
            set { _roleID = value; }
        }

        public string CurrencySymbol
        {
            get { return _currencySymbol; }
            set { _currencySymbol = value; }
        }

        public int Downloads 
        {
            get { return _downloads; }
            set { _downloads = value; }
        }

        #endregion

        #region Object Overrides

        public override int GetHashCode() 
        {
            return _orderID.GetHashCode() ^ _productID.GetHashCode();
        }

        #endregion

        #region IHydratable Interface

        public void Fill(System.Data.IDataReader dr)
        {
            _orderDetailID = Convert.ToInt32(dr["OrderDetailID"]);
            _orderID = Convert.ToInt32(dr["OrderID"]);
            _productID = Convert.ToInt32(dr["ProductID"]);
            _modelNumber = Convert.ToString(Null.SetNull(dr["ModelNumber"], _modelNumber));
            _modelName = Convert.ToString(Null.SetNull(dr["ModelName"], _modelName));
            _quantity = Convert.ToInt32(dr["Quantity"]);
            _unitCost = Convert.ToDecimal(dr["UnitCost"]);
            _extendedAmount = Convert.ToDecimal(dr["ExtendedAmount"]);
            _roleID = Convert.ToInt32(Null.SetNull(dr["RoleID"], _roleID));
            _downloads = Convert.ToInt32(Null.SetNull(dr["Downloads"], _downloads));
        }

        public int KeyID
        {
            get { return _orderDetailID; }
            set { _orderDetailID = value; }
        }

        #endregion

        #region IEquatable<OrderDetailsInfo> Interface

        public bool Equals(OrderDetailInfo other)
        {
            if (other == null)
                return false;
            return _orderID.Equals(other.OrderID) && _productID.Equals(other.ProductID);
        }

        #endregion

        #region IPropertyAccess Members

        CacheLevel IPropertyAccess.Cacheability
        {
            get { return CacheLevel.fullyCacheable; }
        }

        string IPropertyAccess.GetProperty(string propertyName, string format, CultureInfo formatProvider, UserInfo accessingUser, Scope accessLevel, ref bool propertyNotFound)
        {
            string propertyValue = null;
            switch (propertyName.ToLower())
            {
                case "productid":
                    propertyValue = _productID.ToString();
                    break;
                case "modelnumber":
                    propertyValue = _modelNumber;
                    break;
                case "modelname":
                    propertyValue = _modelName;
                    break;
                case "producttitle":
                    propertyValue = ProductTitle;
                    break;
                case "quantity":
                    propertyValue = _quantity.ToString();
                    break;
                case "unitcost":
                    if (formatProvider != null)
                    {
                        if (!string.IsNullOrEmpty(CurrencySymbol))
                            formatProvider.NumberFormat.CurrencySymbol = CurrencySymbol;
                        if (!string.IsNullOrEmpty(format))
                            propertyValue = _unitCost.ToString(format, formatProvider.NumberFormat);
                        else
                            propertyValue = _unitCost.ToString("C", formatProvider.NumberFormat);
                    }
                    else
                        propertyValue = _unitCost.ToString("C");
                    break;
                case "extendedamount":
                    if (formatProvider != null)
                    {
                        if (!string.IsNullOrEmpty(CurrencySymbol))
                            formatProvider.NumberFormat.CurrencySymbol = CurrencySymbol;
                        if (!string.IsNullOrEmpty(format))
                            propertyValue = _extendedAmount.ToString(format, formatProvider.NumberFormat);
                        else
                            propertyValue = _extendedAmount.ToString("C", formatProvider.NumberFormat);
                    }
                    else
                        propertyValue = _extendedAmount.ToString("C");
                    break;
                //case "downloadlink":
                //    propertyValue = _orderStatusID.ToString();
                //    break;
                default:
                    propertyNotFound = true;
                    break;
            }
            return propertyValue;
        }

        #endregion
    }
}
