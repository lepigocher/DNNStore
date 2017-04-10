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
using System.Web.UI.WebControls;
using DotNetNuke.Modules.Store.Catalog;
using DotNetNuke.Modules.Store.Components;

namespace DotNetNuke.Modules.Store.WebControls
{
    public partial  class ReviewList : StoreControlBase
	{
		#region Private Members

		private CatalogNavigation _nav;

		#endregion

		#region Events

		override protected void OnInit(EventArgs e)
		{
			lstReviews.ItemDataBound += lstReviews_ItemDataBound;
			base.OnInit(e);
		}
		
        protected void Page_Load(object sender, EventArgs e) 
		{
            // Obtain and databind a list of all reviews of a product

			// Obtain ProductID from Page State
			_nav = new CatalogNavigation(Request.QueryString);
			
			ReviewController controller = new ReviewController();
            lstReviews.DataSource = controller.GetReviewsByProduct(PortalId, _nav.ProductID, ReviewController.StatusFilter.Approved);
            lstReviews.DataBind();
        }

		protected void btnAddReview_Click(object sender, EventArgs e)
		{
            _nav.ReviewID = 0;
            Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		private void lstReviews_ItemDataBound(object sender, DataListItemEventArgs e)
		{
			ReviewInfo reviewInfo = e.Item.DataItem as ReviewInfo;
			if (reviewInfo != null)
			{
				PlaceHolder phRating = (e.Item.FindControl("plhRating") as PlaceHolder);
				if (phRating != null)
				{
					phRating.Controls.Add(GetRatingImages(reviewInfo.Rating));
				}
			}
		}

		#endregion

		#region Private Methods

		private static Table GetRatingImages(int rating)
		{
			TableRow row = new TableRow();
			for(int i = 0; i < rating; i++)
			{
				Image image = new Image();
				image.ImageUrl = "~/images/ratingplus.gif";

				TableCell cell = new TableCell();
				cell.Controls.Add(image);

				row.Cells.Add(cell);
			}

			Table table = new Table();
			table.BorderWidth = 0;
			table.CellPadding = 0;
			table.CellSpacing = 0;
			table.Rows.Add(row);

			return table;
		}

		#endregion
	}
}
