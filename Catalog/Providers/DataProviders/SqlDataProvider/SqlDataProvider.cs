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
using Microsoft.ApplicationBlocks.Data;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;

namespace DotNetNuke.Modules.Store.Catalog
{
	/// <summary>
    /// Concrete catalog data provider for SQL Server.
    /// </summary>
    public sealed class SqlDataProvider : DataProvider
	{
		#region Private Members

		private const string ProviderType = "data";
		private readonly ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
		private readonly string _connectionString;
		private readonly string _providerPath;
		private readonly string _objectQualifier;
		private readonly string _databaseOwner;

		#endregion

		#region Constructor

		public SqlDataProvider()
		{
			Provider objProvider = (Provider)_providerConfiguration.Providers[_providerConfiguration.DefaultProvider];

            _connectionString = Config.GetConnectionString();
            if (string.IsNullOrEmpty(_connectionString))
                _connectionString = objProvider.Attributes["connectionString"];

			_providerPath = objProvider.Attributes["providerPath"]; 
			_objectQualifier = objProvider.Attributes["objectQualifier"]; 
			
			if (!string.IsNullOrEmpty(_objectQualifier) && !_objectQualifier.EndsWith("_")) 
				_objectQualifier += "_"; 
			
			_databaseOwner = objProvider.Attributes["databaseOwner"]; 
			
			if (!string.IsNullOrEmpty(_databaseOwner) && !_databaseOwner.EndsWith(".")) 
				_databaseOwner += "."; 
		}

		#endregion

		#region Properties

		public string ConnectionString 
		{
            get { return _connectionString; } 
		} 

		public string ProviderPath 
		{
            get { return _providerPath; } 
		} 

		public string ObjectQualifier 
		{
            get { return _objectQualifier; } 
		} 

		public string DatabaseOwner 
		{
            get { return _databaseOwner; } 
		}

		#endregion

		#region Private Methods

		private object GetNull(object field) 
		{ 
			return Null.GetNull(field, DBNull.Value); 
		} 

		#endregion

		#region Public Methods

		// Categories
        public override int AddCategory(int portalID, string name, string seoName, string keywords, string description, string message, bool archived, string createdByUser, DateTime createdDate, int orderID, int parentCategoryID)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Categories_AddCategory", portalID, name, GetNull(seoName), GetNull(keywords), GetNull(description), GetNull(message), archived, createdByUser, createdDate, orderID, parentCategoryID));
        }

        public override void UpdateCategory(int categoryID, string name, string seoName, string keywords, string description, string message, bool archived, int orderID, int parentCategoryID)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Categories_UpdateCategory", categoryID, name, GetNull(seoName), GetNull(keywords), GetNull(description), GetNull(message), archived, orderID, parentCategoryID);
        }

		public override void DeleteCategories(int portalID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Categories_DeleteAll", portalID);
		}

		public override void DeleteCategory(int categoryID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Categories_DeleteCategory", categoryID);
		}

		public override int CategoryCount(int portalID)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Categories_CountAll", portalID));
		}
		public override IDataReader GetCategories(int portalID, bool includeArchived, int parentCategoryID)
		{
			//TODO: Handle exclusion of archived categories, when requested
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Categories_GetAll", portalID, parentCategoryID);
		}

        public override IDataReader GetCategory(int portalID, int categoryID)
		{
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Categories_GetCategory", portalID, categoryID);
		}

		// Products
        public override int AddProduct(int portalID, int categoryID, string manufacturer, string modelNumber, string modelName, string seoName, string productImage, decimal regularPrice, decimal unitCost, string keywords, string summary, string description, bool featured, bool archived, string createdByUser, DateTime createdDate, decimal productWeight, decimal productHeight, decimal productLength, decimal productWidth, DateTime saleStartDate, DateTime saleEndDate, decimal salePrice, int stockQuantity, int lowThreshold, int highThreshold, int deliveryTime, decimal purchasePrice, int roleID, bool isVirtual, int virtualFileID, int allowedDownloads) 
		{
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_AddProduct", portalID, categoryID, GetNull(manufacturer), GetNull(modelNumber), GetNull(modelName), GetNull(seoName), GetNull(productImage), regularPrice, unitCost, keywords, GetNull(summary), GetNull(description), featured, archived, GetNull(createdByUser), GetNull(createdDate), productWeight, productHeight, productLength, productWidth, GetNull(saleStartDate), GetNull(saleEndDate), GetNull(salePrice), stockQuantity, lowThreshold, highThreshold, deliveryTime, purchasePrice, GetNull(roleID), isVirtual, GetNull(virtualFileID), GetNull(allowedDownloads)));
		}

        public override void UpdateProduct(int productID, int categoryID, string manufacturer, string modelNumber, string modelName, string seoName, string productImage, decimal regularPrice, decimal unitCost, string keywords, string summary, string description, bool featured, bool archived, decimal productWeight, decimal productHeight, decimal productLength, decimal productWidth, DateTime saleStartDate, DateTime saleEndDate, decimal salePrice, int stockQuantity, int lowThreshold, int highThreshold, int deliveryTime, decimal purchasePrice, int roleID, bool isVirtual, int virtualFileID, int allowedDownloads) 
		{
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_UpdateProduct", productID, categoryID, GetNull(manufacturer), GetNull(modelNumber), GetNull(modelName), GetNull(seoName), GetNull(productImage), regularPrice, unitCost, keywords, GetNull(summary), GetNull(description), featured, archived, productWeight, productHeight, productLength, productWidth, GetNull(saleStartDate), GetNull(saleEndDate), GetNull(salePrice), stockQuantity, lowThreshold, highThreshold, deliveryTime, purchasePrice, GetNull(roleID), isVirtual, GetNull(virtualFileID), GetNull(allowedDownloads));
		}

		public override void DeleteProduct(int productID) 
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_DeleteProduct", productID);
		}

        public override IDataReader GetProduct(int portalID, int productID) 
		{
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetProduct", portalID, productID);
		}

		public override IDataReader GetPortalAllProducts(int portalID) 
		{
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetPortalAllProducts", portalID);
		}

		public override IDataReader GetPortalProducts(int portalID, bool featured, bool archived) 
		{
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetPortalProducts", portalID, featured, archived);
		}

		public override IDataReader GetPortalFeaturedProducts(int portalID, bool archived) 
		{
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetPortalFeaturedProducts", portalID, archived);
		}

        public override IDataReader GetPortalNewProducts(int portalID, bool archived)
        {
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetPortalNewProducts", portalID, archived);
        }

        public override IDataReader GetCategoryProducts(int portalID, int categoryID, bool archived, int sortBy, string sortDir) 
		{
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetProducts", portalID, categoryID, archived, sortBy, sortDir);
		}

        public override IDataReader GetFeaturedProducts(int portalID, int categoryID, bool archived) 
		{
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetFeaturedProducts", portalID, categoryID, archived);
		}

        public override IDataReader GetNewProducts(int portalID, int categoryID, bool archived)
        {
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetNewProducts", portalID, categoryID, archived);
        }

		public override IDataReader GetPopularProducts(int portalID, int categoryID, bool archived) 
		{
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetPopularProducts", portalID, categoryID, archived);
		}

		public override IDataReader GetPortalPopularProducts(int portalID, bool archived) 
		{
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetPortalPopularProducts", portalID, archived);
		}

		public override IDataReader GetAlsoBoughtProducts(int portalID, int productID, bool archived) 
		{
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetAlsoBoughtProducts", portalID, productID, archived);
		}

        public override IDataReader GetPortalOutOfStockProducts(int portalID) 
		{
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetPortalOutOfStockProducts", portalID);
		}

        public override IDataReader GetPortalLowStockProducts(int portalID) 
		{
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetPortalLowStockProducts", portalID);
		}

        public override IDataReader GetSearchedProducts(int portalID, int searchColumn, string searchValue, int sortBy, string sortDir)
        {
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetSearchedProducts", portalID, searchColumn, searchValue, sortBy, sortDir);
        }

		// Reviews
		public override int AddReview(int portalID, int productID, string userName, int rating, string comments, bool authorized, DateTime createdDate)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Reviews_AddReview", portalID, productID, userName, rating, GetNull(comments), authorized, GetNull(createdDate)));
		}

		public override void UpdateReview(int reviewID, string userName, int rating, string comments, bool authorized)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Reviews_UpdateReview", reviewID, userName, rating, GetNull(comments), authorized);
		}

		public override void DeleteReview(int reviewID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Reviews_DeleteReview", reviewID);
		}

		public override IDataReader GetReview(int portalID, int reviewID)
		{
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Reviews_GetReview", portalID, reviewID);
		}

		public override IDataReader GetReviews(int portalID)
		{
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Reviews_GetAll", portalID);
		}

		public override IDataReader GetReviewsByProduct(int portalID, int productID)
		{
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Reviews_GetByProduct", portalID, productID);
		}

		#endregion
	}
}
