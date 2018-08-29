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

using DotNetNuke.Modules.Store.Core.Components;

namespace DotNetNuke.Modules.Store.Core.Coupon
{
    /// <summary>
    /// Coupon business class.
    /// </summary>
    public sealed class CouponController
    {
        #region Public Methods

        public void AddCoupon(CouponInfo coupon)
        {
            DataProvider.Instance().ExecuteNonQuery("Store_Coupons_AddCoupon",
                coupon.PortalID,
                coupon.Code,
                coupon.Description,
                (int)coupon.RuleType,
                DataHelper.GetNull(coupon.RuleAmount),
                (int)coupon.DiscountType,
                DataHelper.GetNull(coupon.DiscountPercentage),
                DataHelper.GetNull(coupon.DiscountAmount),
                (int)coupon.ApplyTo,
                DataHelper.GetNull(coupon.ItemID),
                coupon.IncludeSubCategories,
                coupon.StartDate,
                coupon.EndDate,
                (int)coupon.Validity,
                coupon.CreatedByUserID);
        }

        public void DeleteCoupon(int couponID)
        {
            DataProvider.Instance().ExecuteNonQuery("Store_Coupons_DeleteCoupon", couponID);
        }

        public List<CouponInfo> GetAll(int portalID)
        {
            return CBO.FillCollection<CouponInfo>(DataProvider.Instance().ExecuteReader("Store_Coupons_GetAll", portalID));
        }

        public CouponInfo GetCoupon(int portalID, int couponID)
        {
            return CBO.FillObject<CouponInfo>(DataProvider.Instance().ExecuteReader("Store_Coupons_GetCoupon", portalID, couponID));
        }

        public CouponInfo GetCouponByCode(int portalID, string couponCode)
        {
            return CBO.FillObject<CouponInfo>(DataProvider.Instance().ExecuteReader("Store_Coupons_GetCouponByCode", portalID, couponCode));
        }

        public void UpdateCoupon(CouponInfo coupon)
        {
            DataProvider.Instance().ExecuteReader("Store_Coupons_UpdateCoupon",
                coupon.CouponID,
                coupon.PortalID,
                coupon.Code,
                coupon.Description,
                (int)coupon.RuleType,
                DataHelper.GetNull(coupon.RuleAmount),
                (int)coupon.DiscountType,
                DataHelper.GetNull(coupon.DiscountPercentage),
                DataHelper.GetNull(coupon.DiscountAmount),
                (int)coupon.ApplyTo,
                DataHelper.GetNull(coupon.ItemID),
                coupon.IncludeSubCategories,
                coupon.StartDate,
                coupon.EndDate,
                (int)coupon.Validity,
                coupon.CreatedByUserID);
        }

        #endregion
    }
}
