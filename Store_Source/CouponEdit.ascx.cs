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
using System.Web.UI.WebControls;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

using DotNetNuke.Modules.Store.Core.Catalog;
using DotNetNuke.Modules.Store.Core.Components;
using DotNetNuke.Modules.Store.Core.Coupon;

namespace DotNetNuke.Modules.Store.WebControls
{
    public partial class CouponEdit : StoreControlBase
    {
        #region Private Members

        private readonly CouponController _controller = new CouponController();
        private int _couponId = Null.NullInteger;

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
			// Get Coupon ID
			_couponId = (int)DataSource;

            // Handle CouponID=0 as Null.NullInteger
            if (_couponId == 0)
                _couponId = Null.NullInteger;
            
            if (Page.IsPostBack == false)
			{
                cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");

                if (_couponId != Null.NullInteger)
                {
                    // Get the selected coupon to edit
                    CouponInfo coupon = _controller.GetCoupon(PortalId, _couponId);
                    if (coupon != null)
                    {
                        txtCode.Text = coupon.Code;
                        txtDescription.Text = coupon.Description;
                        rblRuleType.SelectedIndex = (int) coupon.RuleType;
                        decimal ruleAmount = coupon.RuleAmount;
                        txtRuleAmount.Text = Null.IsNull(ruleAmount) ? "0" : ruleAmount.ToString("0.00");
                        rblDiscountType.SelectedIndex = (int) coupon.DiscountType;
                        int discountPercentage = coupon.DiscountPercentage;
                        txtDiscountPercentage.Text = Null.IsNull(discountPercentage) ? "0" : discountPercentage.ToString("0");
                        decimal discountAmount = coupon.DiscountAmount;
                        txtDiscountAmount.Text = Null.IsNull(discountAmount) ? "0" : discountAmount.ToString("0.00");
                        rblApplyTo.SelectedIndex = (int) coupon.ApplyTo;
                        hfItemID.Value = coupon.ItemID.ToString();
                        chkIncludeSubCategories.Checked = coupon.IncludeSubCategories;
                        txtStartDate.Text = coupon.StartDate.ToShortDateString();
                        rblValidity.SelectedIndex = (int) coupon.Validity;
                        txtEndDate.Text = coupon.EndDate.ToShortDateString();
                        cmdDelete.Visible = true;
                    }
                }
                else
                {
                    // Initialize default values
                    txtRuleAmount.Text = "0";
                    txtDiscountPercentage.Text = "0";
                    txtDiscountAmount.Text = "0";
                    hfItemID.Value = "-1";
                    txtStartDate.Text = DateTime.Now.ToShortDateString();
                    txtEndDate.Text = "";
                }

                // Update UI elements
                trRuleAmount.Visible = rblRuleType.SelectedIndex != 0;
                DisplayDiscountTypeRows();
			    DisplaySelectedItem((CouponApplyTo) rblApplyTo.SelectedIndex, Int32.Parse(hfItemID.Value));
			    DisplayValidityRows();
			}
			MakeCalendars();
        }

        protected void rblRuleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            trRuleAmount.Visible = rblRuleType.SelectedIndex != 0;
        }

        protected void rblDiscountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayDiscountTypeRows();
        }

        protected void rblApplyTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayApplyToRows();
        }

        protected void btnValidateCategory_Click(object sender, EventArgs e)
        {
            hfItemID.Value = lstCategory.SelectedValue;
            lblItemName.Text = lstCategory.SelectedItem.Text;
            trCategory.Visible = false;
            trIncludeSubCategories.Visible = true;
            trSelectedItem.Visible = true;
        }

        protected void btnChangeItem_Click(object sender, EventArgs e)
        {
            DisplayApplyToRows();
        }

        protected void lstCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            CouponApplyTo applyTo = (CouponApplyTo) rblApplyTo.SelectedIndex;

            if (applyTo == CouponApplyTo.Product)
            {
                if (lstCategory.SelectedIndex != 0)
                {
                    BindProducts();
                    trProduct.Visible = true;
                }
                else
                    trProduct.Visible = false;
            }
        }

        protected void btnValidateProduct_Click(object sender, EventArgs e)
        {
            hfItemID.Value = lstProduct.SelectedValue;
            lblItemName.Text = lstProduct.SelectedItem.Text;
            trCategory.Visible = false;
            trIncludeSubCategories.Visible = false;
            trProduct.Visible = false;
            trSelectedItem.Visible = true;
        }

        protected void rblValidity_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayValidityRows();
        }

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                CouponInfo coupon = new CouponInfo
                {
                    CouponID = _couponId,
                    PortalID = PortalId,
                    Code = txtCode.Text,
                    Description = txtDescription.Text,
                    RuleType = (CouponRule)rblRuleType.SelectedIndex
                };
                if (coupon.RuleType == CouponRule.OrderAnything)
                    coupon.RuleAmount = Null.NullDecimal;
                else
                    coupon.RuleAmount = Decimal.Parse(txtRuleAmount.Text);
                coupon.DiscountType = (CouponDiscount) rblDiscountType.SelectedIndex;
                switch (coupon.DiscountType)
                {
                    case CouponDiscount.Percentage:
                        coupon.DiscountPercentage = Int32.Parse(txtDiscountPercentage.Text);
                        break;
                    case CouponDiscount.FixedAmount:
                        coupon.DiscountAmount = Decimal.Parse(txtDiscountAmount.Text);
                        break;
                    case CouponDiscount.FreeShipping:
                        coupon.DiscountPercentage = Null.NullInteger;
                        coupon.DiscountAmount = Null.NullDecimal;
                        break;
                }
                coupon.ApplyTo = (CouponApplyTo) rblApplyTo.SelectedIndex;
                if (coupon.ApplyTo == CouponApplyTo.Order)
                    coupon.ItemID = Null.NullInteger;
                else
                {
                    coupon.ItemID = Int32.Parse(hfItemID.Value);
                    if (coupon.ApplyTo == CouponApplyTo.Category)
                        coupon.IncludeSubCategories = chkIncludeSubCategories.Checked;
                }
                coupon.StartDate = DateTime.Parse(txtStartDate.Text);
                coupon.Validity = (CouponValidity) rblValidity.SelectedIndex;
                switch (coupon.Validity)
                {
                    case CouponValidity.Permanent:
                    case CouponValidity.SingleUse:
                        coupon.EndDate = DateTime.MaxValue;
                        break;
                    case CouponValidity.Until:
                    coupon.EndDate = DateTime.Parse(txtEndDate.Text);
                        break;
                }
                coupon.CreatedByUserID = UserId;

                if (_couponId == Null.NullInteger)
                    _controller.AddCoupon(coupon);
                else
                    _controller.UpdateCoupon(coupon);
                InvokeEditComplete();
            }
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            InvokeEditComplete();
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (_couponId != Null.NullInteger)
                    _controller.DeleteCoupon(_couponId);
                InvokeEditComplete();
            }
            catch
            {
                string errorDelete = Localization.GetString("ErrorDelete", LocalResourceFile);
                Exceptions.ProcessModuleLoadException(errorDelete, this, new Exception(""));
            }
        }

        protected void valCustValidateItem_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            CouponApplyTo applyTo = (CouponApplyTo)rblApplyTo.SelectedIndex;

            if ((applyTo == CouponApplyTo.Category || applyTo == CouponApplyTo.Product) && string.IsNullOrEmpty(hfItemID.Value))
            {
                valCustValidateItem.ErrorMessage = Localization.GetString("valCustValidateItem", LocalResourceFile);
                args.IsValid = false;
            }
        }

        #endregion

        #region Private Methods

        private void MakeCalendars()
        {
            string localizedCalendarText = Localization.GetString("Calendar");
            string calendarText = "<img src='" + ResolveUrl("~/images/calendar.png") + "' border='0' alt='" + localizedCalendarText + "'>&nbsp;" + localizedCalendarText;

            cmdStartDate.NavigateUrl = Common.Utilities.Calendar.InvokePopupCal(txtStartDate);
            cmdStartDate.Text = calendarText;

            cmdEndDate.NavigateUrl = Common.Utilities.Calendar.InvokePopupCal(txtEndDate);
            cmdEndDate.Text = calendarText;
        }

        private void DisplayDiscountTypeRows()
        {
            CouponDiscount discountType = (CouponDiscount) rblDiscountType.SelectedIndex;
            switch (discountType)
            {
                case CouponDiscount.Percentage:
                    trDiscountPercentage.Visible = true;
                    trDiscountAmount.Visible = false;
                    break;
                case CouponDiscount.FixedAmount :
                    trDiscountPercentage.Visible = false;
                    trDiscountAmount.Visible = true;
                    break;
                case CouponDiscount.FreeShipping:
                    trDiscountPercentage.Visible = false;
                    trDiscountAmount.Visible = false;
                    break;
            }
        }

        private void DisplayApplyToRows()
        {
            hfItemID.Value = string.Empty;
            CouponApplyTo applyTo = (CouponApplyTo) rblApplyTo.SelectedIndex;

            switch (applyTo)
            {
                case CouponApplyTo.Order:
                    trCategory.Visible = false;
                    trIncludeSubCategories.Visible = false;
                    lstCategory.AutoPostBack = false;
                    break;
                case CouponApplyTo.Category:
                    BindCategories();
                    trCategory.Visible = true;
                    trIncludeSubCategories.Visible = false;
                    lstCategory.AutoPostBack = false;
                    btnValidateCategory.Visible = true;
                    break;
                case CouponApplyTo.Product:
                    BindCategories();
                    trCategory.Visible = true;
                    trIncludeSubCategories.Visible = false;
                    lstCategory.AutoPostBack = true;
                    btnValidateCategory.Visible = false;
                    break;
            }
            trProduct.Visible = false;
            trSelectedItem.Visible = false;
        }

        private void BindCategories()
        {
            if (lstCategory.Items.Count == 0)
            {
                CategoryController categoryController = new CategoryController();
                lstCategory.DataValueField = "CategoryID";
                lstCategory.DataTextField = "CategoryPathName";
                lstCategory.DataSource = categoryController.GetCategoriesPath(PortalId, false, Null.NullInteger);
                lstCategory.DataBind();

                string select = Localization.GetString("EmptyComboValue", LocalResourceFile);
                ListItem none = new ListItem(select, "-1");
                lstCategory.Items.Insert(0, none);
            }
            lstCategory.SelectedIndex = 0;
        }

        private void BindProducts()
        {
            int categoryID = Int32.Parse(lstCategory.SelectedValue);
            ProductController productController = new ProductController();
            lstProduct.DataValueField = "ProductID";
            lstProduct.DataTextField = "ModelName";
            lstProduct.DataSource = productController.GetCategoryProducts(PortalId, categoryID, false, 2, "ASC");
            lstProduct.DataBind();

            string select = Localization.GetString("EmptyComboValue", LocalResourceFile);
            ListItem none = new ListItem(select, "-1");
            lstProduct.Items.Insert(0, none);
            lstProduct.SelectedIndex = 0;
        }

        private void DisplaySelectedItem(CouponApplyTo applyTo, int itemID)
        {
            switch (applyTo)
            {
                case CouponApplyTo.Order:
                    // Do nothing
                    break;
                case CouponApplyTo.Category:
                    if (itemID != Null.NullInteger)
                    {
                        CategoryController categoryController = new CategoryController();
                        CategoryInfo category = categoryController.GetCategoryPath(PortalId, itemID);
                        if (category != null)
                        {
                            // Show category path name
                            trSelectedItem.Visible = true;
                            lblItemName.Text = category.CategoryPathName;
                            trIncludeSubCategories.Visible = true;
                            return;
                        }
                    }
                    else
                        DisplayApplyToRows();
                    break;
                case CouponApplyTo.Product:
                    if (itemID != Null.NullInteger)
                    {
                        ProductController productController = new ProductController();
                        ProductInfo product = productController.GetProduct(PortalId, itemID);
                        if (product != null)
                        {
                            // Show product name
                            trSelectedItem.Visible = true;
                            lblItemName.Text = product.ModelName;
                            return;
                        }
                    }
                    else
                        DisplayApplyToRows();
                    break;
            }
        }

        private void DisplayValidityRows()
        {
            CouponValidity validity = (CouponValidity) rblValidity.SelectedIndex;

            switch (validity)
            {
                case CouponValidity.Permanent:
                case CouponValidity.SingleUse:
                    trEndDate.Visible = false;
                    break;
                case CouponValidity.Until:
                    trEndDate.Visible = true;
                    break;
            }
        }

        #endregion
    }
}