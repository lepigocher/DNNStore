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

using System.Collections.Generic;
using DotNetNuke.Common.Utilities;

namespace DotNetNuke.Modules.Store.Catalog
{
	/// <summary>
    /// Product business class used to manage user's reviews.
    /// </summary>
    public sealed class ReviewController
	{
		public enum StatusFilter
		{
			All,
			Approved,
			NotApproved
		}

		#region Public Methods

		public ReviewInfo GetReview(int portalID, int reviewID)
		{
            return CBO.FillObject<ReviewInfo>(DataProvider.Instance().GetReview(portalID, reviewID));
		}

		public List<ReviewInfo> GetReviews(int portalID)
		{
            return CBO.FillCollection<ReviewInfo>(DataProvider.Instance().GetReviews(portalID));
		}

        public List<ReviewInfo> GetReviews(int portalID, StatusFilter filter)
		{
			return GetFilteredList(GetReviews(portalID), filter);
		}

        public List<ReviewInfo> GetReviewsByProduct(int portalID, int productID, StatusFilter filter)
		{
			return GetFilteredList(CBO.FillCollection<ReviewInfo>(DataProvider.Instance().GetReviewsByProduct(portalID, productID)), filter);
		}

        public List<ReviewInfo> GetReviewsByCategory(int portalID, int categoryID, StatusFilter filter)
		{
			ProductController productController = new ProductController();
            List<ProductInfo> productList = productController.GetCategoryProducts(portalID, categoryID, true, 2, "ASC");

            List<ReviewInfo> reviewList = new List<ReviewInfo>();
			foreach(ProductInfo productInfo in productList)
			{
				reviewList.AddRange(GetReviewsByProduct(portalID, productInfo.ProductID, filter));
			}
			return reviewList;
		}

		public void AddReview(ReviewInfo reviewInfo)
		{
			DataProvider.Instance().AddReview(
				reviewInfo.PortalID, 
				reviewInfo.ProductID, 
				reviewInfo.UserName, 
				reviewInfo.Rating, 
				reviewInfo.Comments, 
				reviewInfo.Authorized, 
				reviewInfo.CreatedDate);
		}

		public void UpdateReview(ReviewInfo reviewInfo)
		{
			DataProvider.Instance().UpdateReview(
				reviewInfo.ReviewID, 
				reviewInfo.UserName, 
				reviewInfo.Rating, 
				reviewInfo.Comments, 
				reviewInfo.Authorized);
		}

		public void DeleteReview(int reviewID)
		{
			DataProvider.Instance().DeleteReview(reviewID);
		}

		#endregion

		#region Private Methods

        private static List<ReviewInfo> GetFilteredList(List<ReviewInfo> fullList, StatusFilter filter)
		{
			// Should the list be filtered?
			if (filter == StatusFilter.All)
			{
				return fullList;
			}

			// Create filtered list
            List<ReviewInfo> filteredList = new List<ReviewInfo>();
			foreach(ReviewInfo reviewInfo in fullList)
			{
				bool filterByApproved = (filter == StatusFilter.Approved);
				if (filterByApproved == reviewInfo.Authorized)
				{
					filteredList.Add(reviewInfo);
				}
			}

			return filteredList;
		}

		#endregion
	}
}
