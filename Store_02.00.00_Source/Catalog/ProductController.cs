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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Text;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Search;

namespace DotNetNuke.Modules.Store.Catalog
{
	/// <summary>
    /// Product business class used to manage products.
    /// </summary>
    public sealed class ProductController : PortalModuleBase, IPortable, ISearchable
	{ 
        #region Private Members

        private static readonly List<string> CacheKeys = new List<string>();

        #endregion

		#region Public Methods

        public List<ProductInfo> GetPortalLowStockProducts(int portalID)
        {
            return CBO.FillCollection<ProductInfo>(DataProvider.Instance().GetPortalLowStockProducts(portalID));
        }

        public List<ProductInfo> GetPortalOutOfStockProducts(int portalID)
        {
            return CBO.FillCollection<ProductInfo>(DataProvider.Instance().GetPortalOutOfStockProducts(portalID));
        }

        public List<ProductInfo> GetPortalAllProducts(int portalID) 
		{
            string cacheKey = string.Format("StorePortalAllProducts_P{0}", portalID);
            List<ProductInfo> products = (List<ProductInfo>)DataCache.GetCache(cacheKey);
            if (products == null)
            {
                products = CBO.FillCollection<ProductInfo>(DataProvider.Instance().GetPortalAllProducts(portalID));
                if (products != null)
                {
                    DataCache.SetCache(cacheKey, products, false);
                    if (!CacheKeys.Contains(cacheKey))
                        CacheKeys.Add(cacheKey);
                }
            }
            return products;
		}

        public List<ProductInfo> GetPortalProducts(int portalID, bool featured, bool archived) 
		{
            string cacheKey = string.Format("StorePortalProducts_P{0}_F{1}_A{2}", portalID, featured, archived);
            List<ProductInfo> products = (List<ProductInfo>)DataCache.GetCache(cacheKey);
            if (products == null)
            {
                products = CBO.FillCollection<ProductInfo>(DataProvider.Instance().GetPortalProducts(portalID, featured, archived));
                if (products != null)
                {
                    DataCache.SetCache(cacheKey, products, false);
                    if (!CacheKeys.Contains(cacheKey))
                        CacheKeys.Add(cacheKey);
                }
            }
            return products;
		}

        public List<ProductInfo> GetPortalFeaturedProducts(int portalID, bool archived) 
		{
            string cacheKey = string.Format("StorePortalFeaturedProducts_P{0}_A{1}", portalID, archived);
            List<ProductInfo> products = (List<ProductInfo>)DataCache.GetCache(cacheKey);
            if (products == null)
            {
                products = CBO.FillCollection<ProductInfo>(DataProvider.Instance().GetPortalFeaturedProducts(portalID, archived));
                if (products != null)
                {
                    DataCache.SetCache(cacheKey, products, false);
                    if (!CacheKeys.Contains(cacheKey))
                        CacheKeys.Add(cacheKey);
                }
            }
            return products;
		}

        public List<ProductInfo> GetPortalNewProducts(int portalID, bool archived)
        {
            string cacheKey = string.Format("StorePortalNewProducts_P{0}_A{1}", portalID, archived);
            List<ProductInfo> products = (List<ProductInfo>)DataCache.GetCache(cacheKey);
            if (products == null)
            {
                products = CBO.FillCollection<ProductInfo>(DataProvider.Instance().GetPortalNewProducts(portalID, archived));
                if (products != null)
                {
                    DataCache.SetCache(cacheKey, products, false);
                    if (!CacheKeys.Contains(cacheKey))
                        CacheKeys.Add(cacheKey);
                }
            }
            return products;
        }

        public List<ProductInfo> GetCategoryProducts(int portalID, int categoryID, bool archived, int sortBy, string sortDir) 
		{
            string cacheKey = string.Format("StoreCategoryProducts_P{0}_C{1}_A{2}_S{3}_D{4}", portalID, categoryID, archived, sortBy, sortDir);
            List<ProductInfo> products = (List<ProductInfo>)DataCache.GetCache(cacheKey);
            if (products == null)
            {
                products = CBO.FillCollection<ProductInfo>(DataProvider.Instance().GetCategoryProducts(portalID, categoryID, archived, sortBy, sortDir));
                if (products != null)
                {
                    DataCache.SetCache(cacheKey, products, false);
                    if (!CacheKeys.Contains(cacheKey))
                        CacheKeys.Add(cacheKey);
                }
            }
            return products;
		}

        public List<ProductInfo> GetFeaturedProducts(int portalID, int categoryID, bool archived) 
		{
            string cacheKey = string.Format("StoreFeaturedProducts_P{0}_C{1}_A{2}", portalID, categoryID, archived);
            List<ProductInfo> products = (List<ProductInfo>)DataCache.GetCache(cacheKey);
            if (products == null)
            {
                products = CBO.FillCollection<ProductInfo>(DataProvider.Instance().GetFeaturedProducts(portalID, categoryID, archived));
                if (products != null)
                {
                    DataCache.SetCache(cacheKey, products, false);
                    if (!CacheKeys.Contains(cacheKey))
                        CacheKeys.Add(cacheKey);
                }
            }
            return products;
		}

        public List<ProductInfo> GetNewProducts(int portalID, int categoryID, bool archived)
        {
            string cacheKey = string.Format("StoreNewProducts_P{0}_C{1}_A{2}", portalID, categoryID, archived);
            List<ProductInfo> products = (List<ProductInfo>)DataCache.GetCache(cacheKey);
            if (products == null)
            {
                products = CBO.FillCollection<ProductInfo>(DataProvider.Instance().GetNewProducts(portalID, categoryID, archived));
                if (products != null)
                {
                    DataCache.SetCache(cacheKey, products, false);
                    if (!CacheKeys.Contains(cacheKey))
                        CacheKeys.Add(cacheKey);
                }
            }
            return products;
        }

        public List<ProductInfo> GetPopularProducts(int portalID, int categoryID, bool archived) 
		{
            string cacheKey = string.Format("StorePopularProducts_P{0}_C{1}_A{2}", portalID, categoryID, archived);
            List<ProductInfo> products = (List<ProductInfo>)DataCache.GetCache(cacheKey);
            if (products == null)
            {
                products = CBO.FillCollection<ProductInfo>(DataProvider.Instance().GetPopularProducts(portalID, categoryID, archived));
                if (products != null)
                {
                    DataCache.SetCache(cacheKey, products, false);
                    if (!CacheKeys.Contains(cacheKey))
                        CacheKeys.Add(cacheKey);
                }
            }
            return products;
		}

        public List<ProductInfo> GetPortalPopularProducts(int portalID, bool archived) 
		{
            string cacheKey = string.Format("StorePortalPopularProducts_P{0}_A{1}", portalID, archived);
            List<ProductInfo> products = (List<ProductInfo>)DataCache.GetCache(cacheKey);
            if (products == null)
            {
                products = CBO.FillCollection<ProductInfo>(DataProvider.Instance().GetPortalPopularProducts(portalID, archived));
                if (products != null)
                {
                    DataCache.SetCache(cacheKey, products, false);
                    if (!CacheKeys.Contains(cacheKey))
                        CacheKeys.Add(cacheKey);
                }
            }
            return products;
		}

        public List<ProductInfo> GetSearchedProducts(int portalID, int searchColumn, string searchValue, int sortBy, string sortDir)
        {
            string sqlValue = "%" + searchValue.Replace("%", "").Trim() + "%";
            string cacheKey = string.Format("StoreSearchedProducts_P{0}_SC{1}_SV{2}_S{3}_D{4}", portalID, searchColumn, sqlValue.GetHashCode(), sortBy, sortDir);
            List<ProductInfo> products = (List<ProductInfo>)DataCache.GetCache(cacheKey);
            if (products == null)
            {
                products = CBO.FillCollection<ProductInfo>(DataProvider.Instance().GetSearchedProducts(portalID, searchColumn, sqlValue, sortBy, sortDir));
                if (products != null)
                {
                    DataCache.SetCache(cacheKey, products, false);
                    if (!CacheKeys.Contains(cacheKey))
                        CacheKeys.Add(cacheKey);
                }
            }
            return products;
        } 

        public List<ProductInfo> GetAlsoBoughtProducts(int portalID, int productID, bool archived) 
		{
            string cacheKey = string.Format("StoreAlsoBoughtProducts_P{0}_P{1}_A{2}", portalID, productID, archived);
            List<ProductInfo> products = (List<ProductInfo>)DataCache.GetCache(cacheKey);
            if (products == null)
            {
                products = CBO.FillCollection<ProductInfo>(DataProvider.Instance().GetAlsoBoughtProducts(portalID, productID, archived));
                if (products != null)
                {
                    DataCache.SetCache(cacheKey, products, false);
                    if (!CacheKeys.Contains(cacheKey))
                        CacheKeys.Add(cacheKey);
                }
            }
            return products;
		}

        public void ClearAllCaches()
        {
            foreach (string cacheKey in CacheKeys)
            {
                DataCache.RemoveCache(cacheKey);
            }
        }

        public ProductInfo GetProduct(int portalID, int productID) 
		{
            return CBO.FillObject<ProductInfo>(DataProvider.Instance().GetProduct(portalID, productID)); 
		}

		public void AddProduct(ProductInfo productInfo)
		{
			DataProvider.Instance().AddProduct(productInfo.PortalID, productInfo.CategoryID,
                productInfo.Manufacturer, productInfo.ModelNumber, productInfo.ModelName, productInfo.SEOName,
                productInfo.ProductImage, productInfo.RegularPrice, productInfo.UnitCost, productInfo.Keywords,
                productInfo.Summary, productInfo.Description, productInfo.Featured, productInfo.Archived, 
				productInfo.CreatedByUser, productInfo.CreatedDate, productInfo.ProductWeight, 
                productInfo.ProductHeight, productInfo.ProductLength, productInfo.ProductWidth, 
                productInfo.SaleStartDate, productInfo.SaleEndDate, productInfo.SalePrice, productInfo.StockQuantity,
                productInfo.LowThreshold, productInfo.HighThreshold, productInfo.DeliveryTime, productInfo.PurchasePrice,
                productInfo.RoleID, productInfo.IsVirtual, productInfo.VirtualFileID, productInfo.AllowedDownloads);
            ClearAllCaches();
		}

		public void UpdateProduct(ProductInfo productInfo)
		{
			DataProvider.Instance().UpdateProduct(productInfo.ProductID, productInfo.CategoryID,
                productInfo.Manufacturer, productInfo.ModelNumber, productInfo.ModelName, productInfo.SEOName,
                productInfo.ProductImage, productInfo.RegularPrice, productInfo.UnitCost, productInfo.Keywords,
                productInfo.Summary, productInfo.Description, productInfo.Featured, productInfo.Archived,
                productInfo.ProductWeight, productInfo.ProductHeight, productInfo.ProductLength, productInfo.ProductWidth,
                productInfo.SaleStartDate, productInfo.SaleEndDate, productInfo.SalePrice, productInfo.StockQuantity,
                productInfo.LowThreshold, productInfo.HighThreshold, productInfo.DeliveryTime, productInfo.PurchasePrice,
                productInfo.RoleID, productInfo.IsVirtual, productInfo.VirtualFileID, productInfo.AllowedDownloads);
            ClearAllCaches();
        }

		public void DeleteProduct(int productID)
		{
			DataProvider.Instance().DeleteProduct(productID);
            ClearAllCaches();
        }

		#endregion

		#region ISearchable Members

        public SearchItemInfoCollection GetSearchItems(ModuleInfo moduleInfo)
		{
            ModuleSettings moduleSettings = new ModuleSettings(moduleInfo.ModuleID, moduleInfo.TabID);

            if (moduleSettings.General.EnableContentIndexing)
            {
                // Get all products
                List<ProductInfo> productList = GetPortalAllProducts(moduleInfo.PortalID);

                // Create search item collection
                SearchItemInfoCollection searchItemList = new SearchItemInfoCollection();
                foreach (ProductInfo product in productList)
                {
                    // Get user identifier
                    int userID = Null.NullInteger;
                    if (int.TryParse(product.CreatedByUser, out userID) == false)
                    {
                        UserInfo user = UserController.GetUserByName(PortalId, product.CreatedByUser);
                        if (user != null)
                            userID = user.UserID;
                    }

                    // Create title
                    string title = HttpUtility.HtmlDecode(product.ProductTitle);

                    // Create content
                    string content = HttpUtility.HtmlDecode(title) + ": " + HttpUtility.HtmlDecode(product.Description) + " " + HttpUtility.HtmlDecode(product.Summary);

                    SearchItemInfo searchItem = new SearchItemInfo(title, HttpUtility.HtmlDecode(product.Summary), userID, product.CreatedDate, moduleInfo.ModuleID,
                        product.ProductID.ToString(), content, "ProductID=" + product.ProductID);

                    searchItemList.Add(searchItem);
                }

                return searchItemList;
            }

            return null;
		}

		#endregion

        #region IPortable Members

        string IPortable.ExportModule(int moduleID)
        {
            // Export categories
            CategoryController categoriesControler = new CategoryController();
            List<CategoryInfo> alCategories = categoriesControler.GetCategories(PortalId, true, -3);

            if (alCategories.Count > 0)
            {
                StringBuilder strXML = new StringBuilder();

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                using (XmlWriter writer = XmlWriter.Create(strXML, settings))
                {
                    writer.WriteStartElement("Catalog");
                    writer.WriteStartElement("Categories");
                    foreach (CategoryInfo categoryInfo in alCategories)
                    {
                        writer.WriteStartElement("Category");
                        writer.WriteElementString("CategoryId", categoryInfo.CategoryID.ToString());
                        writer.WriteElementString("Name", categoryInfo.Name);
                        writer.WriteElementString("SEOName", categoryInfo.SEOName);
                        writer.WriteElementString("Keywords", categoryInfo.Keywords);
                        writer.WriteElementString("Description", categoryInfo.Description);
                        writer.WriteElementString("OrderId", categoryInfo.OrderID.ToString());
                        writer.WriteElementString("ParentCategoryId", categoryInfo.ParentCategoryID.ToString());
                        writer.WriteElementString("Archived", categoryInfo.Archived.ToString());
                        writer.WriteElementString("Message", categoryInfo.Message);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();

                    // Export Products
                    List<ProductInfo> alProducts = GetPortalAllProducts(PortalId);
                    CultureInfo invariantCulture = CultureInfo.InvariantCulture;
                    string strProductImage = Null.NullString;

                    if (alProducts.Count > 0)
                    {
                        writer.WriteStartElement("Products");
                        foreach (ProductInfo productInfo in alProducts)
                        {
                            writer.WriteStartElement("Product");
                            writer.WriteElementString("CategoryID", productInfo.CategoryID.ToString());
                            writer.WriteElementString("Manufacturer", productInfo.Manufacturer);
                            writer.WriteElementString("ModelNumber", productInfo.ModelNumber);
                            writer.WriteElementString("ModelName", productInfo.ModelName);
                            writer.WriteElementString("SEOName", productInfo.SEOName);
                            writer.WriteElementString("Keywords", productInfo.Keywords);
                            writer.WriteElementString("Summary", productInfo.Summary);
                            writer.WriteElementString("RegularPrice", productInfo.RegularPrice.ToString("0.00", invariantCulture));
                            writer.WriteElementString("UnitCost", productInfo.UnitCost.ToString("0.00", invariantCulture));
                            writer.WriteElementString("Virtual", productInfo.IsVirtual.ToString());
                            if (productInfo.IsVirtual)
                            {
                                writer.WriteElementString("VirtualFileID", productInfo.VirtualFileID.ToString());
                                writer.WriteElementString("AllowedDownloads", productInfo.AllowedDownloads.ToString());
                            }
                            else
                            {
                                writer.WriteElementString("ProductWeight", productInfo.ProductWeight.ToString("0.00", invariantCulture));
                                writer.WriteElementString("ProductHeight", productInfo.ProductHeight.ToString("0.00", invariantCulture));
                                writer.WriteElementString("ProductLength", productInfo.ProductLength.ToString("0.00", invariantCulture));
                                writer.WriteElementString("ProductWidth", productInfo.ProductWidth.ToString("0.00", invariantCulture));
                            }
                            writer.WriteElementString("StockQuantity", productInfo.StockQuantity.ToString());
                            writer.WriteElementString("LowThreshold", productInfo.LowThreshold.ToString());
                            writer.WriteElementString("HighThreshold", productInfo.HighThreshold.ToString());
                            writer.WriteElementString("DeliveryTime", productInfo.DeliveryTime.ToString());
                            writer.WriteElementString("PurchasePrice", productInfo.PurchasePrice.ToString("0.00", invariantCulture));
                            writer.WriteElementString("Archived", productInfo.Archived.ToString());
                            //TODO: Apply Role!
                            writer.WriteElementString("Featured", productInfo.Featured.ToString());
                            if (productInfo.Featured)
                            {
                                writer.WriteElementString("SaleStartDate", productInfo.SaleStartDate.ToString(invariantCulture));
                                writer.WriteElementString("SaleEndDate", productInfo.SaleEndDate.ToString(invariantCulture));
                                writer.WriteElementString("SalePrice", productInfo.SalePrice.ToString("0.00", invariantCulture));
                            }
                            if (string.IsNullOrEmpty(productInfo.ProductImage) == false)
                            {
                                if (!productInfo.ProductImage.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
                                    strProductImage = productInfo.ProductImage.Replace("Portals/" + PortalId, "[PortalId]");
                                writer.WriteElementString("ProductImage", strProductImage);
                            }
                            writer.WriteElementString("Description", productInfo.Description);
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.Close();
                }
                return strXML.ToString();
            }

            return string.Empty;
        }

        void IPortable.ImportModule(int moduleID, string content, string version, int userID)
        {
            ModuleController moduleControler = new ModuleController();
            ModuleInfo moduleInfo = moduleControler.GetModule(moduleID, Null.NullInteger);
            Version importVersion = new Version(version);
            Version moduleVersion = new Version(moduleInfo.Version);

            if (importVersion == moduleVersion)
            {
                XmlNode xmlCategories = Common.Globals.GetContent(content, "Catalog/Categories");
                SortedList slCategories = new SortedList(xmlCategories.ChildNodes.Count);
                int indexID = Null.NullInteger;

                CategoryController categoriesControler = new CategoryController();

                foreach (XmlNode xmlCategory in xmlCategories)
                {
                    CategoryInfo categoryInfo = new CategoryInfo();
                    categoryInfo.PortalID = PortalId;
                    categoryInfo.Name = xmlCategory["Name"].InnerText;
                    categoryInfo.SEOName = xmlCategory["SEOName"].InnerText;
                    categoryInfo.Keywords = xmlCategory["Keywords"].InnerText;
                    categoryInfo.Description = xmlCategory["Description"].InnerText;
                    categoryInfo.OrderID = Convert.ToInt32(xmlCategory["OrderId"].InnerText);
                    indexID = slCategories.IndexOfKey(Convert.ToInt32(xmlCategory["ParentCategoryId"].InnerText));
                    if (indexID != Null.NullInteger)
                        categoryInfo.ParentCategoryID = (int)slCategories.GetByIndex(indexID);
                    else
                        categoryInfo.ParentCategoryID = Null.NullInteger;
                    categoryInfo.Archived = Convert.ToBoolean(xmlCategory["Archived"].InnerText);
                    categoryInfo.Message = xmlCategory["Message"].InnerText;
                    categoryInfo.CreatedByUser = UserId.ToString();
                    categoryInfo.CreatedDate = DateTime.Now;
                    slCategories.Add(Convert.ToInt32(xmlCategory["CategoryId"].InnerText), categoriesControler.AddCategory(categoryInfo));
                }

                XmlNode xmlProducts = Common.Globals.GetContent(content, "Catalog/Products");

                string value = Null.NullString;
                CultureInfo invariantCulture = CultureInfo.InvariantCulture;

                foreach (XmlNode xmlProduct in xmlProducts)
                {
                    ProductInfo productInfo = new ProductInfo();
                    productInfo.PortalID = PortalId;
                    productInfo.CategoryID = (int)slCategories.GetByIndex(slCategories.IndexOfKey(Convert.ToInt32(xmlProduct["CategoryID"].InnerText)));
                    productInfo.Manufacturer = xmlProduct["Manufacturer"].InnerText;
                    productInfo.ModelNumber = xmlProduct["ModelNumber"].InnerText;
                    productInfo.ModelName = xmlProduct["ModelName"].InnerText;
                    productInfo.SEOName = xmlProduct["SEOName"].InnerText;
                    productInfo.Keywords = xmlProduct["Keywords"].InnerText;
                    productInfo.Summary = xmlProduct["Summary"].InnerText;
                    productInfo.RegularPrice = Convert.ToDecimal(xmlProduct["RegularPrice"].InnerText, invariantCulture);
                    productInfo.UnitCost = Convert.ToDecimal(xmlProduct["UnitCost"].InnerText, invariantCulture);
                    productInfo.IsVirtual = Convert.ToBoolean(xmlProduct["Virtual"].InnerText);
                    if (productInfo.IsVirtual)
                    {
                        productInfo.VirtualFileID = Convert.ToInt32(xmlProduct["VirtualFileID"].InnerText);
                        productInfo.AllowedDownloads = Convert.ToInt32(xmlProduct["AllowedDownloads"].InnerText);
                    }
                    else
                    {
                        productInfo.ProductWeight = Convert.ToDecimal(xmlProduct["ProductWeight"].InnerText, invariantCulture);
                        productInfo.ProductHeight = Convert.ToDecimal(xmlProduct["ProductHeight"].InnerText, invariantCulture);
                        productInfo.ProductLength = Convert.ToDecimal(xmlProduct["ProductLength"].InnerText, invariantCulture);
                        productInfo.ProductWidth = Convert.ToDecimal(xmlProduct["ProductWidth"].InnerText, invariantCulture);
                    }
                    productInfo.StockQuantity = Convert.ToInt32(xmlProduct["StockQuantity"].InnerText);
                    productInfo.LowThreshold = Convert.ToInt32(xmlProduct["LowThreshold"].InnerText);
                    productInfo.HighThreshold = Convert.ToInt32(xmlProduct["HighThreshold"].InnerText);
                    productInfo.DeliveryTime = Convert.ToInt32(xmlProduct["DeliveryTime"].InnerText);
                    productInfo.PurchasePrice = Convert.ToDecimal(xmlProduct["PurchasePrice"].InnerText, invariantCulture);
                    productInfo.Archived = Convert.ToBoolean(xmlProduct["Archived"].InnerText);
                    //TODO: Apply Role!
                    productInfo.Featured = Convert.ToBoolean(xmlProduct["Featured"].InnerText);
                    if (productInfo.Featured)
                    {
                        productInfo.SaleStartDate = Convert.ToDateTime(xmlProduct["SaleStartDate"].InnerText, invariantCulture);
                        productInfo.SaleEndDate = Convert.ToDateTime(xmlProduct["SaleEndDate"].InnerText, invariantCulture);
                        productInfo.SalePrice = Convert.ToDecimal(xmlProduct["SalePrice"].InnerText, invariantCulture);
                    }
                    value = xmlProduct["ProductImage"].InnerText;
                    if (string.IsNullOrEmpty(value) == false)
                    {
                        if (value.StartsWith("http", StringComparison.InvariantCultureIgnoreCase) == false)
                            productInfo.ProductImage = value.Replace("[PortalId]", "Portals/" + PortalId);
                        else
                            productInfo.ProductImage = value;
                    }
                    productInfo.Description = xmlProduct["Description"].InnerText;
                    productInfo.CreatedByUser = UserId.ToString();
                    productInfo.CreatedDate = DateTime.Now;
                    AddProduct(productInfo);
                }
            }
        }

        #endregion
    }
}
