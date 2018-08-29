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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;

using DotNetNuke.Modules.Store.Core.Catalog;
using DotNetNuke.Modules.Store.Core.Components;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for Store.
	/// </summary>
	public partial class ReviewAdmin : StoreControlBase, IStoreTabedControl
	{
		#region Private Members

		private AdminNavigation _nav;

		#endregion

		#region Events

        override protected void OnInit(EventArgs e)
        {
            linkAddNew.Click += linkAddNew_Click;
            grdReviews.ItemDataBound += grdReviews_ItemDataBound;
            grdReviews.PageIndexChanged += grdReviews_PageIndexChanged;
            base.OnInit(e);
        }

	    protected void Page_Load(object sender, EventArgs e)
		{
			_nav = new AdminNavigation(Request.QueryString);

			// Do we show list or edit view?
			if (_nav.ReviewID != Null.NullInteger)
			{
				ShowEditControl();
			}
			else
			{
				if (!IsPostBack)
				{
					FillStatusCombo();
					FillCategoryCombo();
					FillProductCombo();
					FillReviewGrid();
				}
			}
		}

		private void grdReviews_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			ReviewInfo reviewInfo = e.Item.DataItem as ReviewInfo;
			if (reviewInfo != null)
			{
				// Update edit link using this item's ID
				HyperLink linkEdit = (e.Item.FindControl("linkEdit") as HyperLink);
				if (linkEdit != null)
				{
                    StringDictionary replaceParams = new StringDictionary
                    {
                        ["ReviewID"] = reviewInfo.ReviewID.ToString()
                    };
                    linkEdit.NavigateUrl = _nav.GetNavigationUrl(replaceParams);
				}

				// Add rating images
				PlaceHolder phRating = (e.Item.FindControl("phRating") as PlaceHolder);
				if (phRating != null)
				{
					phRating.Controls.Add(GetRatingImages(reviewInfo.Rating));
				}
			}
		}

		protected void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
		{
			_nav.PageIndex = Null.NullInteger;
			_nav.StatusID = int.Parse(cmbStatus.SelectedValue);
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		protected void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
		{
			_nav.PageIndex = Null.NullInteger;
			_nav.CategoryID = int.Parse(cmbCategory.SelectedValue);
			_nav.ProductID = Null.NullInteger;
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		protected void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
		{
            _nav.PageIndex = Null.NullInteger;
			_nav.ProductID = int.Parse(cmbProduct.SelectedValue);
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		private void linkAddNew_Click(object sender, EventArgs e)
		{
			_nav.ReviewID = 0;
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		private void reviewEdit_EditComplete(object sender, EventArgs e)
		{
			_nav.ReviewID = Null.NullInteger;
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		private void grdReviews_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			_nav.PageIndex = e.NewPageIndex;
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		#endregion

		#region Private Methods

		private void FillReviewGrid()
		{
			panelList.Visible = true;
			panelEdit.Visible = false;

			// Get the status filter
			ReviewController.StatusFilter filter = ReviewController.StatusFilter.All;
			if (cmbStatus.SelectedValue == "0")
			{
				filter = ReviewController.StatusFilter.NotApproved;
			}
			else if (cmbStatus.SelectedValue == "1")
			{
				filter = ReviewController.StatusFilter.Approved;
			}

			// Get the review list...
            List<ReviewInfo> reviewList;
			ReviewController controller = new ReviewController();
			if (cmbProduct.SelectedValue != Null.NullInteger.ToString())
			{
				// Select by product
				reviewList = controller.GetReviewsByProduct(PortalId, int.Parse(cmbProduct.SelectedValue), filter);
			}
			else if (cmbCategory.SelectedValue != Null.NullInteger.ToString())
			{
				// Select by category
                reviewList = controller.GetReviewsByCategory(PortalId, int.Parse(cmbCategory.SelectedValue), filter);
			}
			else
			{
				// Select all reviews
				reviewList = controller.GetReviews(PortalId, filter);
			}

			// Has page index been initialized?
			if (_nav.PageIndex == Null.NullInteger)
			{
				_nav.PageIndex = 0;
			}

			// Update the grid
            if (reviewList.Count <= grdReviews.PageSize)
			    grdReviews.AllowPaging = false;
			grdReviews.DataSource = reviewList;
			grdReviews.CurrentPageIndex = _nav.PageIndex;
			grdReviews.DataBind();
		}

		private void FillStatusCombo()
		{
			cmbStatus.Items.Clear();
			cmbStatus.Items.Add(new ListItem("--- " + Localization.GetString("All.Text") + " ---", Null.NullInteger.ToString()));
			cmbStatus.Items.Add(new ListItem(Localization.GetString("NotApproved", LocalResourceFile), "0"));
			cmbStatus.Items.Add(new ListItem(Localization.GetString("ApprovedOnly", LocalResourceFile), "1"));

            cmbStatus.SelectedValue = _nav.StatusID.ToString();
		}

		private void FillCategoryCombo()
		{
			CategoryController controller = new CategoryController();
            List<CategoryInfo> categoryList = controller.GetCategoriesPath(PortalId, true, Null.NullInteger);
			cmbCategory.DataSource = categoryList;
			cmbCategory.DataBind();
			cmbCategory.Items.Insert(0, new ListItem("--- " + Localization.GetString("All.Text") + " ---", Null.NullInteger.ToString()));

			cmbCategory.SelectedValue = _nav.CategoryID.ToString();
		}

		private void FillProductCombo()
		{
			ProductController controller = new ProductController();
            List<ProductInfo> productList;

			string categoryID = cmbCategory.SelectedValue;
			if (categoryID == Null.NullInteger.ToString())
			{
				productList = controller.GetPortalProducts(PortalId, false, false);
			}
			else
			{
                productList = controller.GetCategoryProducts(PortalId, int.Parse(categoryID), true, 2, "ASC");
			}

			cmbProduct.DataSource = productList;
			cmbProduct.DataBind();
			cmbProduct.Items.Insert(0, new ListItem("--- " + Localization.GetString("All.Text") + " ---", Null.NullInteger.ToString()));

			cmbProduct.SelectedValue = _nav.ProductID.ToString();			
		}

		private void ShowEditControl()
		{
			panelList.Visible = false;
			panelEdit.Visible = true;

			// Inject the edit control
			StoreControlBase reviewEdit = (StoreControlBase)LoadControl(ControlPath + "ReviewEdit.ascx");
            reviewEdit.ModuleConfiguration = ModuleConfiguration;
            reviewEdit.StoreSettings = StoreSettings;
			reviewEdit.DataSource = _nav.ReviewID;
			reviewEdit.EditComplete += reviewEdit_EditComplete;
            panelEdit.Controls.Clear();
            panelEdit.Controls.Add(reviewEdit);
		}

		private Table GetRatingImages(int rating)
		{
			TableRow row = new TableRow();
			for(int i = 0; i < rating; i++)
			{
                Image image = new Image
                {
                    ImageUrl = "~/images/ratingplus.gif"
                };

                TableCell cell = new TableCell();
				cell.Controls.Add(image);

				row.Cells.Add(cell);
			}

            Table table = new Table
            {
                BorderWidth = 0,
                CellPadding = 0,
                CellSpacing = 0
            };
            table.Rows.Add(row);

			return table;
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
