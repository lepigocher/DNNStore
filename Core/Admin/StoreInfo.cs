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

using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Tokens;

using DotNetNuke.Modules.Store.Core.Providers;

namespace DotNetNuke.Modules.Store.Core.Admin
{
    /// <summary>
    /// Store class info.
    /// </summary>
    [Serializable]
    public sealed class StoreInfo : IHydratable, IEquatable<StoreInfo>, IPropertyAccess
    {
        #region Private Members

        private int _portalID;
        private string _name;
        private string _description;
        private string _keywords;
        private bool _seoFeature;
        private string _gatewayName;
        private string _gatewaySettings;
        private string _defaultEmailAddress;
        private int _shoppingCartPageID;
        private string _createdByUser;
        private DateTime _createdDate;
        private int _storePageID;
        private string _currencySymbol;
        private bool _portalTemplates;
        private string _styleSheet;
        private bool _authorizeCancel;
        private bool _inventoryManagement;
        private int _outOfStock;
        private int _productsBehavior;
        private bool _avoidNegativeStock;
        private int _orderRoleID;
        private int _catalogRoleID;
        private bool _secureCookie;
        private CheckoutType _checkoutMode;
        private int _impersonatedUserID;
        private bool _noDelivery;
        private bool _allowVirtualProducts;
        private bool _allowCoupons;
        private bool _allowFreeShipping;
        private decimal _minOrderAmount;
        private bool _restrictToCountries;
        private string _authorizedCountries;
        private int _onOrderPaidRoleID;
        private string _taxName;
        private string _shippingName;
        private string _addressName;

        #endregion

        #region Properties

        public int PortalID
        {
            get { return _portalID; }
            set { _portalID = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Keywords
        {
            get { return _keywords; }
            set { _keywords = value; }
        }

        public bool SEOFeature
        {
            get { return _seoFeature; }
            set { _seoFeature = value; }
        }

        public string GatewayName
        {
            get { return _gatewayName; }
            set { _gatewayName = value; }
        }

        public string GatewaySettings
        {
            get { return _gatewaySettings; }
            set { _gatewaySettings = value; }
        }

        public string DefaultEmailAddress
        {
            get { return _defaultEmailAddress; }
            set { _defaultEmailAddress = value; }
        }

        public int ShoppingCartPageID
        {
            get { return _shoppingCartPageID; }
            set { _shoppingCartPageID = value; }
        }

        public string CreatedByUser
        {
            get { return _createdByUser; }
            set { _createdByUser = value; }
        }

        public DateTime CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        public int StorePageID
        {
            get { return _storePageID; }
            set { _storePageID = value; }
        }

        public string CurrencySymbol
        {
            get { return _currencySymbol; }
            set { _currencySymbol = value; }
        }

        public bool PortalTemplates
        {
            get { return _portalTemplates; }
            set { _portalTemplates = value; }
        }

        public string StyleSheet
        {
            get
            {
                if (string.IsNullOrEmpty(_styleSheet))
                    return "template.css";

                return _styleSheet;
            }
            set { _styleSheet = value; }
        }

        public bool AuthorizeCancel
        {
            get { return _authorizeCancel; }
            set { _authorizeCancel = value; }
        }

        public bool InventoryManagement
        {
            get { return _inventoryManagement; }
            set { _inventoryManagement = value; }
        }

        public int OutOfStock
        {
            get { return _outOfStock; }
            set { _outOfStock = value; }
        }

        public int ProductsBehavior
        {
            get { return _productsBehavior; }
            set { _productsBehavior = value; }
        }

        public bool AvoidNegativeStock
        {
            get { return _avoidNegativeStock; }
            set { _avoidNegativeStock = value; }
        }

        public int OrderRoleID
        {
            get { return _orderRoleID; }
            set { _orderRoleID = value; }
        }

        public int CatalogRoleID
        {
            get { return _catalogRoleID; }
            set { _catalogRoleID = value; }
        }

        public bool SecureCookie
        {
            get { return _secureCookie; }
            set { _secureCookie = value; }
        }

        public CheckoutType CheckoutMode
        {
            get { return _checkoutMode; }
            set { _checkoutMode = value; }
        }

        public int ImpersonatedUserID
        {
            get { return _impersonatedUserID; }
            set { _impersonatedUserID = value; }
        }

        public bool NoDelivery
        {
            get { return _noDelivery; }
            set { _noDelivery = value; }
        }

        public bool AllowVirtualProducts
        {
            get { return _allowVirtualProducts; }
            set { _allowVirtualProducts = value; }
        }

        public bool AllowCoupons
        {
            get { return _allowCoupons; }
            set { _allowCoupons = value; }
        }

        public bool AllowFreeShipping
        {
            get { return _allowFreeShipping; }
            set { _allowFreeShipping = value; }
        }

        public decimal MinOrderAmount
        {
            get { return _minOrderAmount; }
            set { _minOrderAmount = value; }
        }

        public bool RestrictToCountries
        {
            get { return _restrictToCountries; }
            set { _restrictToCountries = value; }
        }

        public string AuthorizedCountries
        {
            get { return _authorizedCountries; }
            set { _authorizedCountries = value; }
        }

        public int OnOrderPaidRoleID
        {
            get { return _onOrderPaidRoleID; }
            set { _onOrderPaidRoleID = value; }
        }

        public string TaxName
        {
            get { return _taxName; }
            set { _taxName = value; }
        }

        public string ShippingName
        {
            get { return _shippingName; }
            set { _shippingName = value; }
        }

        public string AddressName
        {
            get { return _addressName; }
            set { _addressName = value; }
        }

        #endregion

        #region Object Overrides

        public override int GetHashCode()
        {
            return _portalID.GetHashCode();
        }

        #endregion

        #region IHydratable Members

        public void Fill(System.Data.IDataReader dr)
        {
            _portalID = Convert.ToInt32(dr["PortalID"]);
            _name = Convert.ToString(Null.SetNull(dr["Name"], _name));
            _description = Convert.ToString(Null.SetNull(dr["Description"], _description));
            _keywords = Convert.ToString(Null.SetNull(dr["Keywords"], _keywords));
            _seoFeature = Convert.ToBoolean(dr["SEOFeature"]);
            _gatewayName = Convert.ToString(dr["GatewayName"]);
            _gatewaySettings = Convert.ToString(Null.SetNull(dr["GatewaySettings"], _gatewaySettings));
            _defaultEmailAddress = Convert.ToString(Null.SetNull(dr["DefaultEmailAddress"], _defaultEmailAddress));
            _shoppingCartPageID = Convert.ToInt32(Null.SetNull(dr["ShoppingCartPageID"], _shoppingCartPageID));
            _createdByUser = Convert.ToString(dr["CreatedByUser"]);
            _createdDate = Convert.ToDateTime(dr["CreatedDate"]);
            _storePageID = Convert.ToInt32(Null.SetNull(dr["StorePageID"], _storePageID));
            _currencySymbol = Convert.ToString(Null.SetNull(dr["CurrencySymbol"], _currencySymbol));
            _portalTemplates = Convert.ToBoolean(dr["PortalTemplates"]);
            _styleSheet = Convert.ToString(Null.SetNull(dr["StyleSheet"], _styleSheet));
            _authorizeCancel = Convert.ToBoolean(dr["AuthorizeCancel"]);
            _inventoryManagement = Convert.ToBoolean(dr["InventoryManagement"]);
            _outOfStock = Convert.ToInt32(dr["OutOfStock"]);
            _productsBehavior = Convert.ToInt32(dr["ProductsBehavior"]);
            _avoidNegativeStock = Convert.ToBoolean(dr["AvoidNegativeStock"]);
            _orderRoleID = Convert.ToInt32(dr["OrderRoleID"]);
            _catalogRoleID = Convert.ToInt32(dr["CatalogRoleID"]);
            _secureCookie = Convert.ToBoolean(dr["SecureCookie"]);
            _checkoutMode = (CheckoutType)(dr["CheckoutMode"]);
            _impersonatedUserID = Convert.ToInt32(Null.SetNull(dr["ImpersonatedUserID"], _impersonatedUserID));
            _noDelivery = Convert.ToBoolean(dr["NoDelivery"]);
            _allowVirtualProducts = Convert.ToBoolean(dr["AllowVirtualProducts"]);
            _allowCoupons = Convert.ToBoolean(dr["AllowCoupons"]);
            _allowFreeShipping = Convert.ToBoolean(dr["AllowFreeShipping"]);
            _minOrderAmount = Convert.ToDecimal(Null.SetNull(dr["MinOrderAmount"], _minOrderAmount));
            _restrictToCountries = Convert.ToBoolean(dr["RestrictToCountries"]);
            _authorizedCountries = Convert.ToString(Null.SetNull(dr["AuthorizedCountries"], _authorizedCountries));
            _onOrderPaidRoleID = Convert.ToInt32(dr["OnOrderPaidRoleID"]);
            _taxName = Convert.ToString(dr["TaxName"]);
            _shippingName = Convert.ToString(dr["ShippingName"]);
            _addressName = Convert.ToString(dr["AddressName"]);
        }

        public int KeyID
        {
            get { return _portalID; }
            set { _portalID = value; }
        }

        #endregion

        #region IEquatable<StoreInfo> Members

        public bool Equals(StoreInfo other)
        {
            if (other == null)
                return false;

            return _portalID.Equals(other.PortalID);
        }

        #endregion

        #region IPropertyAccess Members

        CacheLevel IPropertyAccess.Cacheability
        {
            get { return CacheLevel.fullyCacheable; }
        }

        string IPropertyAccess.GetProperty(string strPropertyName, string strFormat, CultureInfo formatProvider, UserInfo accessingUser, Scope accessLevel, ref bool propertyNotFound)
        {
            TabController controler = new TabController();
            TabInfo tab;
            string propertyValue = null;

            switch (strPropertyName.ToLower())
            {
                case "name":
                    propertyValue = _name;
                    break;
                case "description":
                    propertyValue = _description;
                    break;
                case "email":
                    propertyValue = _defaultEmailAddress;
                    break;
                case "shoppingcartpagename":
                    tab = controler.GetTab(_shoppingCartPageID, _portalID, false);
                    if (tab != null)
                        propertyValue = tab.TabName;
                    break;
                case "shoppingcartpagelink":
                    propertyValue = Globals.NavigateURL(_shoppingCartPageID);
                    break;
                case "catalogpagename":
                    tab = controler.GetTab(_storePageID, _portalID, false);
                    if (tab != null)
                        propertyValue = tab.TabName;
                    break;
                case "catalogpagelink":
                    propertyValue = Globals.NavigateURL(_storePageID);
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
