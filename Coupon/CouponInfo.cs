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
using System.Collections.Generic;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;

namespace DotNetNuke.Modules.Store.Coupon
{
    /// <summary>
    /// Coupon class info.
    /// </summary>
    [Serializable]
    public sealed class CouponInfo : IComparable<CouponInfo>, IHydratable, IEquatable<CouponInfo>
    {
        #region Private Members

        private int _couponID;
        private int _portalID;
        private string _code;
        private string _description;
        private CouponRule _ruleType;
        private decimal _ruleAmount;
        private CouponDiscount _discountType;
        private int _discountPercentage;
        private decimal _discountAmount;
        private CouponApplyTo _applyTo;
        private int _itemID;
        private bool _includeSubCategories;
        private DateTime _startDate;
        private DateTime _endDate;
        private CouponValidity _validity;
        private int _createdByUserID;
        private DateTime _createdDate;

        #endregion

        #region Public Properties

        public int CouponID
        {
            get { return _couponID; }
            set { _couponID = value; }
        }

        public int PortalID
        {
            get { return _portalID; }
            set { _portalID = value; }
        }

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public CouponRule RuleType
        {
            get { return _ruleType; }
            set { _ruleType = value; }
        }

        public decimal RuleAmount
        {
            get { return _ruleAmount; }
            set { _ruleAmount = value; }
        }

        public CouponDiscount DiscountType
        {
            get { return _discountType; }
            set { _discountType = value; }
        }

        public int DiscountPercentage
        {
            get { return _discountPercentage; }
            set { _discountPercentage = value; }
        }

        public decimal DiscountAmount
        {
            get { return _discountAmount; }
            set { _discountAmount = value; }
        }

        public CouponApplyTo ApplyTo
        {
            get { return _applyTo; }
            set { _applyTo = value; }
        }

        public int ItemID
        {
            get { return _itemID; }
            set { _itemID = value; }
        }

        public bool IncludeSubCategories
        {
            get { return _includeSubCategories; }
            set { _includeSubCategories = value; }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        public CouponValidity Validity
        {
            get { return _validity; }
            set { _validity = value; }
        }

        public int CreatedByUserID
        {
            get { return _createdByUserID; }
            set { _createdByUserID = value; }
        }

        public DateTime CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        public static IComparer<CouponInfo> StartDateSorter
        {
            get { return new StartDateComparer(); }
        }

        public static IComparer<CouponInfo> DescriptionSorter
        {
            get { return new DescriptionComparer(); }
        }

        #endregion

        #region Object Overrides

        public override int GetHashCode()
        {
            return _couponID.GetHashCode();
        }

        #endregion

        #region IComparable Interface

        public int CompareTo(CouponInfo other)
        {
            return String.CompareOrdinal(_code, other.Code);
        }

        #endregion

        #region Nested Classes Comparers

        private class StartDateComparer : IComparer<CouponInfo>
        {
            #region IComparer<CouponInfo> Members

            public int Compare(CouponInfo first, CouponInfo second)
            {
                return first.StartDate.CompareTo(second.StartDate);
            }

            #endregion
        }

        private class DescriptionComparer : IComparer<CouponInfo>
        {
            #region IComparer<CategoryInfo> Members

            public int Compare(CouponInfo first, CouponInfo second)
            {
                return String.CompareOrdinal(first.Description, second.Description);
            }

            #endregion
        }

        #endregion

        #region IHydratable Interface

        public void Fill(System.Data.IDataReader dr)
        {
            _couponID = Convert.ToInt32(dr["CouponID"]);
            _portalID = Convert.ToInt32(dr["PortalID"]);
            _code = Convert.ToString(dr["Code"]);
            _description = Convert.ToString(dr["Description"]);
            _ruleType = (CouponRule)(dr["RuleType"]);
            _ruleAmount = Convert.ToDecimal(Null.SetNull(dr["RuleAmount"], _ruleAmount));
            _discountType = (CouponDiscount)(dr["DiscountType"]);
            _discountPercentage = Convert.ToInt32((Null.SetNull(dr["DiscountPercentage"], _discountPercentage)));
            _discountAmount = Convert.ToDecimal((Null.SetNull(dr["DiscountAmount"], _discountAmount)));
            _applyTo = (CouponApplyTo)(dr["ApplyTo"]);
            _itemID = Convert.ToInt32(Null.SetNull(dr["ItemID"], _itemID));
            _includeSubCategories = Convert.ToBoolean(Null.SetNull(dr["IncludeSubCategories"], _includeSubCategories));
            _startDate = Convert.ToDateTime(dr["StartDate"]);
            _endDate = Convert.ToDateTime(dr["EndDate"]);
            _validity = (CouponValidity)(dr["Validity"]);
            _createdByUserID = Convert.ToInt32(dr["CreatedByUserID"]);
            _createdDate = Convert.ToDateTime(dr["CreatedDate"]);
        }

        public int KeyID
        {
            get { return _couponID; }
            set { _couponID = value; }
        }

        #endregion

        #region IEquatable<CouponInfo> Interface

        public bool Equals(CouponInfo other)
        {
            if (other == null)
                return false;
            return _couponID.Equals(other.CouponID);
        }

        #endregion
    }
}
