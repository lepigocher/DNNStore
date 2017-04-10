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
using System.Data;
using DotNetNuke.Framework;

namespace DotNetNuke.Modules.Store.Catalog
{
	/// <summary>
    /// DataProvider abstract class.
    /// </summary>
	public abstract class DataProvider
	{
		#region Private Members

		private static DataProvider _provider; 

		#endregion

		#region Constructors

		static DataProvider() 
		{ 
			CreateProvider(); 
		} 

		private static void CreateProvider() 
		{ 
			_provider = (DataProvider)Reflection.CreateObject("data", "DotNetNuke.Modules.Store.Catalog", "DotNetNuke.Modules.Store.Catalog"); 
		} 

		public static DataProvider Instance() 
		{ 
			return _provider; 
		}

		#endregion

		#region Abstract Methods

		// Categories
        public abstract int AddCategory(int portalID, string name, string seoName, string keywords, string description, string message, bool archived, string createdByUser, DateTime createdDate, int orderID, int parentCategoryId);
        public abstract void UpdateCategory(int categoryID, string name, string seoName, string keywords, string description, string message, bool archived, int orderID, int parentCategoryId);
		public abstract void DeleteCategories(int portalID);
        public abstract void DeleteCategory(int categoryID);
		public abstract int CategoryCount(int portalID);
		public abstract IDataReader GetCategories(int portalID, bool includeArchived, int parentCategoryID);
        public abstract IDataReader GetCategory(int portalID, int categoryID);

		// Products
        public abstract int AddProduct(int portalID, int categoryID, string manufacturer, string modelNumber, string modelName, string seoName, string productImage, decimal regularPrice, decimal unitCost, string keywords, string summary, string description, bool featured, bool archived, string createdByUser, DateTime createdDate, Decimal productWeight, Decimal productHeight, Decimal productLength, Decimal productWidth, DateTime saleStartDate, DateTime saleEndDate, Decimal salePrice, int stockQuantity, int lowThreshold, int highThreshold, int deliveryTime, decimal purchasePrice, int roleID, bool isVirtual, int virtualFileID, int allowedDownloads);
        public abstract void UpdateProduct(int productID, int categoryID, string manufacturer, string modelNumber, string modelName, string seoName, string productImage, decimal regularPrice, decimal unitCost, string keywords, string summary, string description, bool featured, bool archived, Decimal productWeight, Decimal productHeight, Decimal productLength, Decimal productWidth, DateTime saleStartDate, DateTime saleEndDate, Decimal salePrice, int stockQuantity, int lowThreshold, int highThreshold, int deliveryTime, decimal purchasePrice, int roleID, bool isVirtual, int virtualFileID, int allowedDownloads);
		public abstract void DeleteProduct(int productID);
        public abstract IDataReader GetProduct(int portalID, int productID);
        public abstract IDataReader GetPortalAllProducts(int portalID);
        public abstract IDataReader GetPortalProducts(int portalID, bool featured, bool archived);
        public abstract IDataReader GetPortalFeaturedProducts(int portalID, bool archived);
        public abstract IDataReader GetPortalNewProducts(int portalID, bool archived);
        public abstract IDataReader GetCategoryProducts(int portalID, int categoryID, bool archived, int sortBy, string sortDir);
        public abstract IDataReader GetFeaturedProducts(int portalID, int categoryID, bool archived);
        public abstract IDataReader GetNewProducts(int portalID, int categoryID, bool archived);
        public abstract IDataReader GetPopularProducts(int portalID, int categoryID, bool archived);
        public abstract IDataReader GetPortalPopularProducts(int portalID, bool archived);
		public abstract IDataReader GetAlsoBoughtProducts(int portalID, int productID, bool archived);
        public abstract IDataReader GetPortalOutOfStockProducts(int portalID);
        public abstract IDataReader GetPortalLowStockProducts(int portalID);
        public abstract IDataReader GetSearchedProducts(int portalID, int searchColumn, string searchValue, int sortBy, string sortDir);

		// Reviews
		public abstract int AddReview(int portalID, int productID, string userName, int rating, string comments, bool authorized, DateTime createdDate);
		public abstract void UpdateReview(int reviewID, string userName, int rating, string comments, bool authorized);
		public abstract void DeleteReview(int reviewID);
		public abstract IDataReader GetReview(int portalID, int reviewID);
		public abstract IDataReader GetReviews(int portalID);
		public abstract IDataReader GetReviewsByProduct(int portalID, int productID);

		#endregion
	}
}
