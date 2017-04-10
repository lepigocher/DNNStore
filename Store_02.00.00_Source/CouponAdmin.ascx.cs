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
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Modules.Store.Coupon;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Store.WebControls
{
    public partial class CouponAdmin : StoreControlBase, IStoreTabedControl
    {
        #region Private Members

        private AdminNavigation _nav;
        private List<CouponInfo> _couponList;
        private string _navigateUrl;

        private SortDirection CouponsSortDirection
        {
            get
            {
                if (Session["CouponsSortDirection"] == null)
                    Session["CouponsSortDirection"] = SortDirection.Ascending;
                return (SortDirection)Session["CouponsSortDirection"];
            }
            set { Session["CouponsSortDirection"] = value; }
        }

        private string CouponsSortExpression
        {
            get
            {
                if (Session["CouponsSortExpression"] == null)
                    Session["CouponsSortExpression"] = "Code";
                return (string)Session["CouponsSortExpression"];
            }
            set { Session["CouponsSortExpression"] = value; }
        }

        private int CouponsPageIndex
        {
            get
            {
                if (Session["CouponsPageIndex"] == null)
                    Session["CouponsPageIndex"] = 0;
                return (int)Session["CouponsPageIndex"];
            }
            set { Session["CouponsPageIndex"] = value; }
        }

        #endregion

        #region Events

        override protected void OnInit(EventArgs e)
        {
            linkAddNew.Click += linkAddNew_Click;
            linkAddImage.Click += linkAddNew_Click;
            base.OnInit(e);
        }
		
        protected void Page_Load(object sender, EventArgs e)
        {
            _nav = new AdminNavigation(Request.QueryString);
            if (_nav.CouponID != Null.NullInteger)
            {
                plhGrid.Visible = false;
                plhForm.Visible = true;

                if (_nav.CategoryID == 0)
                {
                    lblEditTitle.Text = Localization.GetString("AddCoupon", LocalResourceFile);
                    LoadEditControl(Null.NullInteger);
                }
                else
                {
                    lblEditTitle.Text = Localization.GetString("EditCoupon", LocalResourceFile);
                    LoadEditControl(_nav.CouponID);
                }
            }
            else
            {
                plhGrid.Visible = true;
                plhForm.Visible = false;

                StringDictionary replaceParams = new StringDictionary();
                replaceParams["CouponID"] = "{0}";
                _navigateUrl = _nav.GetNavigationUrl(replaceParams);

                if (IsPostBack == false)
                {
                    Localization.LocalizeGridView(ref gvCoupons, LocalResourceFile);
                    SortAndBind();
                }
            }
        }

        protected void linkAddNew_Click(object sender, EventArgs e)
        {
            _nav.CouponID = 0;
            Response.Redirect(_nav.GetNavigationUrl());
        }

        protected void editControl_EditComplete(object sender, EventArgs e)
        {
            _nav.CouponID = Null.NullInteger;
            Response.Redirect(_nav.GetNavigationUrl());
        }

        protected void gvCoupons_DataBinding(object sender, EventArgs e)
        {
            HyperLinkField hlfEdit = (HyperLinkField)gvCoupons.Columns[gvCoupons.Columns.Count - 1];
            hlfEdit.DataNavigateUrlFormatString = _navigateUrl;
        }

        protected void gvCoupons_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int sortColumnIndex = GetSortColumnIndex();
                if (sortColumnIndex != -1)
                    AddSortImage(sortColumnIndex, e.Row);
            }
        }

        protected void gvCoupons_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            // Reset sort if it's not the same column
            if (sortExpression != CouponsSortExpression)
                CouponsSortDirection = SortDirection.Ascending;
            else
            {
                // Invert sort order
                if (CouponsSortDirection == SortDirection.Ascending)
                    CouponsSortDirection = SortDirection.Descending;
                else
                    CouponsSortDirection = SortDirection.Ascending;
            }
            CouponsSortExpression = sortExpression;
            CouponsPageIndex = 0;
            SortAndBind();
        }

        protected void gvCoupons_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CouponsPageIndex = e.NewPageIndex;
            SortAndBind();
        }

        #endregion

        #region Private Methods

        private void LoadEditControl(int couponID)
        {
            plhEditControl.Controls.Clear();
            StoreControlBase editControl = (StoreControlBase)LoadControl(ModulePath + "CouponEdit.ascx");
            editControl.ModuleConfiguration = ModuleConfiguration;
            editControl.StoreSettings = StoreSettings;
            editControl.DataSource = couponID;
            editControl.EditComplete += editControl_EditComplete;
            plhEditControl.Controls.Add(editControl);
        }

        private void SortAndBind()
        {
            CouponController couponController = new CouponController();
            _couponList = couponController.GetAll(PortalId);

            switch (CouponsSortExpression)
            {
                case "Code":
                    _couponList.Sort();
                    break;
                case "Description":
                    _couponList.Sort(CouponInfo.DescriptionSorter);
                    break;
                case "StartDate":
                    _couponList.Sort(CouponInfo.StartDateSorter);
                    break;
                default:
                    break;
            }

            if (CouponsSortDirection == SortDirection.Descending)
                _couponList.Reverse();

            gvCoupons.PageIndex = CouponsPageIndex;
            gvCoupons.DataSource = _couponList;
            gvCoupons.DataBind();
        }

        private int GetSortColumnIndex()
        {
            foreach (DataControlField field in gvCoupons.Columns)
            {
                if (field.SortExpression == CouponsSortExpression)
                    return gvCoupons.Columns.IndexOf(field);
            }
            return Null.NullInteger;
        }

        private void AddSortImage(int columnIndex, GridViewRow headerRow)
        {
            // Create the sorting image based on the sort direction.
            Image sortImage = new Image();

            if (CouponsSortDirection == SortDirection.Ascending)
            {
                sortImage.ImageUrl = "~/images/sortascending.gif";
                sortImage.AlternateText = Localization.GetString("SortAscending", LocalResourceFile);
            }
            else
            {
                sortImage.ImageUrl = "~/images/sortdescending.gif";
                sortImage.AlternateText = Localization.GetString("SortDescending", LocalResourceFile);
            }

            // Add the image to the appropriate header cell.
            headerRow.Cells[columnIndex].Controls.Add(sortImage);
        }

        #endregion

        #region IStoreTabedControl Members

        string IStoreTabedControl.Title
        {
            get { return Localization.GetString("lblParentTitle", LocalResourceFile); }
        }

        #endregion
    }
}