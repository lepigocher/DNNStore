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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Text;
using System.Web;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Services.Search.Entities;

using DotNetNuke.Modules.Store.Core.Components;

namespace DotNetNuke.Modules.Store.Core.Catalog
{
    /// <summary>
    /// Product business class used to manage products.
    /// </summary>
    public sealed class ProductController : ModuleSearchBase, IPortable
	{ 
        #region Private Members

        private static readonly List<string> CacheKeys = new List<string>();

        #endregion

		#region Public Methods

        public List<ProductInfo> GetPortalLowStockProducts(int portalID)
        {
            return CBO.FillCollection<ProductInfo>(DataProvider.Instance().ExecuteReader("Store_Products_GetPortalLowStockProducts", portalID));
        }

        public List<ProductInfo> GetPortalOutOfStockProducts(int portalID)
        {
            return CBO.FillCollection<ProductInfo>(DataProvider.Instance().ExecuteReader("Store_Products_GetPortalOutOfStockProducts", portalID));
        }

        public List<ProductInfo> GetPortalAllProducts(int portalID) 
		{
            string cacheKey = string.Format("StorePortalAllProducts_P{0}", portalID);
            List<ProductInfo> products = (List<ProductInfo>)DataCache.GetCache(cacheKey);
            if (products == null)
            {
                products = CBO.FillCollection<ProductInfo>(DataProvider.Instance().ExecuteReader("Store_Products_GetPortalAllProducts", portalID));
                if (products != null)
                {
                    DataCache.SetCache(cacheKey, products);
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
                products = CBO.FillCollection<ProductInfo>(DataProvider.Instance().ExecuteReader("Store_Products_GetPortalProducts", portalID, featured, archived));
                if (products != null)
                {
                    DataCache.SetCache(cacheKey, products);
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
                products = CBO.FillCollection<ProductInfo>(DataProvider.Instance().ExecuteReader("Store_Products_GetPortalFeaturedProducts", portalID, archived));
                if (products != null)
                {
                    DataCache.SetCache(cacheKey, products);
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
                products = CBO.FillCollection<ProductInfo>(DataProvider.Instance().ExecuteReader("Store_Products_GetPortalNewProducts", portalID, archived));
                if (products != null)
                {
                    DataCache.SetCache(cacheKey, products);
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
                products = CBO.FillCollection<ProductInfo>(DataProvider.Instance().ExecuteReader("Store_Products_GetProducts", portalID, categoryID, archived, sortBy, sortDir));
                if (products != null)
                {
                    DataCache.SetCache(cacheKey, products);
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
                products = CBO.FillCollection<ProductInfo>(DataProvider.Instance().ExecuteReader("Store_Products_GetFeaturedProducts", portalID, categoryID, archived));
                if (products != null)
                {
                    DataCache.SetCache(cacheKey, products);
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
                products = CBO.FillCollection<ProductInfo>(DataProvider.Instance().ExecuteReader("Store_Products_GetNewProducts", portalID, categoryID, archived));
                if (products != null)
                {
                    DataCache.SetCache(cacheKey, products);
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
                products = CBO.FillCollection<ProductInfo>(DataProvider.Instance().ExecuteReader("Store_Products_GetPopularProducts", portalID, categoryID, archived));
                if (products != null)
                {
                    DataCache.SetCache(cacheKey, products);
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
                products = CBO.FillCollection<ProductInfo>(DataProvider.Instance().ExecuteReader("Store_Products_GetPortalPopularProducts", portalID, archived));
                if (products != null)
                {
                    DataCache.SetCache(cacheKey, products);
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
                products = CBO.FillCollection<ProductInfo>(DataProvider.Instance().ExecuteReader("Store_Products_GetSearchedProducts", portalID, searchColumn, sqlValue, sortBy, sortDir));
                if (products != null)
                {
                    DataCache.SetCache(cacheKey, products);
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
                products = CBO.FillCollection<ProductInfo>(DataProvider.Instance().ExecuteReader("Store_Products_GetAlsoBoughtProducts", portalID, productID, archived));
                if (products != null)
                {
                    DataCache.SetCache(cacheKey, products);
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
            return CBO.FillObject<ProductInfo>(DataProvider.Instance().ExecuteReader("Store_Products_GetProduct", portalID, productID)); 
		}

		public void AddProduct(ProductInfo productInfo)
		{
			DataProvider.Instance().ExecuteNonQuery("Store_Products_AddProduct",
                productInfo.PortalID,
                productInfo.CategoryID,
                DataHelper.GetNull(productInfo.Manufacturer),
                DataHelper.GetNull(productInfo.ModelNumber),
                DataHelper.GetNull(productInfo.ModelName),
                DataHelper.GetNull(productInfo.SEOName),
                DataHelper.GetNull(productInfo.ProductImage),
                productInfo.RegularPrice,
                productInfo.UnitCost,
                productInfo.Keywords,
                DataHelper.GetNull(productInfo.Summary),
                DataHelper.GetNull(productInfo.Description),
                productInfo.Featured,
                productInfo.Archived,
                DataHelper.GetNull(productInfo.CreatedByUser),
                DataHelper.GetNull(productInfo.CreatedDate),
                productInfo.ProductWeight, 
                productInfo.ProductHeight,
                productInfo.ProductLength,
                productInfo.ProductWidth,
                DataHelper.GetNull(productInfo.SaleStartDate),
                DataHelper.GetNull(productInfo.SaleEndDate),
                DataHelper.GetNull(productInfo.SalePrice),
                productInfo.StockQuantity,
                productInfo.LowThreshold,
                productInfo.HighThreshold,
                productInfo.DeliveryTime,
                productInfo.PurchasePrice,
                DataHelper.GetNull(productInfo.RoleID),
                productInfo.IsVirtual,
                DataHelper.GetNull(productInfo.VirtualFileID),
                DataHelper.GetNull(productInfo.AllowedDownloads));

            ClearAllCaches();
		}

		public void UpdateProduct(ProductInfo productInfo)
		{
			DataProvider.Instance().ExecuteNonQuery("Store_Products_UpdateProduct",
                productInfo.ProductID,
                productInfo.CategoryID,
                DataHelper.GetNull(productInfo.Manufacturer),
                DataHelper.GetNull(productInfo.ModelNumber),
                DataHelper.GetNull(productInfo.ModelName),
                DataHelper.GetNull(productInfo.SEOName),
                DataHelper.GetNull(productInfo.ProductImage),
                productInfo.RegularPrice,
                productInfo.UnitCost,
                productInfo.Keywords,
                DataHelper.GetNull(productInfo.Summary),
                DataHelper.GetNull(productInfo.Description),
                productInfo.Featured,
                productInfo.Archived,
                productInfo.ProductWeight,
                productInfo.ProductHeight,
                productInfo.ProductLength,
                productInfo.ProductWidth,
                DataHelper.GetNull(productInfo.SaleStartDate),
                DataHelper.GetNull(productInfo.SaleEndDate),
                DataHelper.GetNull(productInfo.SalePrice),
                productInfo.StockQuantity,
                productInfo.LowThreshold,
                productInfo.HighThreshold,
                productInfo.DeliveryTime,
                productInfo.PurchasePrice,
                DataHelper.GetNull(productInfo.RoleID),
                productInfo.IsVirtual,
                DataHelper.GetNull(productInfo.VirtualFileID),
                DataHelper.GetNull(productInfo.AllowedDownloads));

            ClearAllCaches();
        }

		public void DeleteProduct(int productID)
		{
			DataProvider.Instance().ExecuteNonQuery("Store_Products_DeleteProduct", productID);

            ClearAllCaches();
        }

        #endregion

        #region ModuleSearchBase Members

        public override IList<SearchDocument> GetModifiedSearchDocuments(ModuleInfo module, DateTime beginDateUtc)
        {
            int tabId = module.TabID;
            ModuleSettings moduleSettings = new ModuleSettings(module.ModuleID, tabId);

            if (!moduleSettings.General.EnableContentIndexing)
                return new List<SearchDocument>();

            int portalId = module.PortalID;

            // Get all products
            List<ProductInfo> productList = GetPortalAllProducts(portalId);

            // Create search item collection
            IList<SearchDocument> documents = new List<SearchDocument>(productList.Count);
            foreach (ProductInfo product in productList)
            {
                // Get user identifier
                int userID = Null.NullInteger;
                if (int.TryParse(product.CreatedByUser, out userID) == false)
                {
                    UserInfo user = UserController.GetUserByName(portalId, product.CreatedByUser);
                    if (user != null)
                        userID = user.UserID;
                }

                SearchDocument document = new SearchDocument
                {
                    PortalId = portalId,
                    TabId = tabId,
                    UniqueKey = string.Format("StoreProducts_ID_{0}", product.ProductID),
                    Title = HttpUtility.HtmlDecode(product.ProductTitle),
                    Body = HttpUtility.HtmlDecode(product.Description),
                    Description = HttpUtility.HtmlDecode(product.Summary),
                    AuthorUserId = userID,
                    ModifiedTimeUtc = product.CreatedDate.ToUniversalTime(),
                    QueryString = string.Format("ProductID={0}", product.ProductID)
                };
                documents.Add(document);
            }

            return documents;
        }

        #endregion

        #region IPortable Members

        string IPortable.ExportModule(int moduleID)
        {
            int portalId = PortalController.Instance.GetCurrentPortalSettings().PortalId;

            // Export categories
            CategoryController categoriesControler = new CategoryController();
            List<CategoryInfo> alCategories = categoriesControler.GetCategories(portalId, true, -3);

            if (alCategories.Count > 0)
            {
                StringBuilder strXML = new StringBuilder();

                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    OmitXmlDeclaration = true
                };
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
                    List<ProductInfo> alProducts = GetPortalAllProducts(portalId);

                    if (alProducts.Count > 0)
                    {
                        CultureInfo invariantCulture = CultureInfo.InvariantCulture;
                        string strProductImage = Null.NullString;

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
                            if (productInfo.IsVirtual)
                            {
                                IFileInfo file = FileManager.Instance.GetFile(productInfo.VirtualFileID);

                                if (file != null)
                                {
                                    writer.WriteElementString("Virtual", productInfo.IsVirtual.ToString());
                                    writer.WriteElementString("RelativePath", file.RelativePath);
                                    writer.WriteElementString("AllowedDownloads", productInfo.AllowedDownloads.ToString());
                                }
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
                            if (productInfo.RoleID > 0)
                            {
                                RoleInfo role = RoleController.Instance.GetRoleById(portalId, productInfo.RoleID);

                                if (role != null)
                                    writer.WriteElementString("RoleName", role.RoleName);
                            }
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
                                    strProductImage = productInfo.ProductImage.Replace("Portals/" + portalId, "[PortalId]");
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
            int portalId = PortalController.Instance.GetCurrentPortalSettings().PortalId;
            ModuleInfo moduleInfo = ModuleController.Instance.GetModule(moduleID, Null.NullInteger, true);
            Version importVersion = new Version(version);
            Version moduleVersion = new Version(moduleInfo.DesktopModule.Version);

            if (importVersion == moduleVersion)
            {
                XmlNode xmlCategories = Common.Globals.GetContent(content, "Catalog/Categories");
                SortedList slCategories = new SortedList(xmlCategories.ChildNodes.Count);
                int indexID = Null.NullInteger;

                CategoryController categoriesControler = new CategoryController();

                foreach (XmlNode xmlCategory in xmlCategories)
                {
                    CategoryInfo categoryInfo = new CategoryInfo
                    {
                        PortalID = portalId,
                        Name = xmlCategory["Name"].InnerText,
                        SEOName = xmlCategory["SEOName"].InnerText,
                        Keywords = xmlCategory["Keywords"].InnerText,
                        Description = xmlCategory["Description"].InnerText,
                        OrderID = Convert.ToInt32(xmlCategory["OrderId"].InnerText)
                    };
                    indexID = slCategories.IndexOfKey(Convert.ToInt32(xmlCategory["ParentCategoryId"].InnerText));
                    if (indexID != Null.NullInteger)
                        categoryInfo.ParentCategoryID = (int)slCategories.GetByIndex(indexID);
                    else
                        categoryInfo.ParentCategoryID = Null.NullInteger;
                    categoryInfo.Archived = Convert.ToBoolean(xmlCategory["Archived"].InnerText);
                    categoryInfo.Message = xmlCategory["Message"].InnerText;
                    categoryInfo.CreatedByUser = userID.ToString();
                    categoryInfo.CreatedDate = DateTime.Now;
                    slCategories.Add(Convert.ToInt32(xmlCategory["CategoryId"].InnerText), categoriesControler.AddCategory(categoryInfo));
                }

                XmlNode xmlProducts = Common.Globals.GetContent(content, "Catalog/Products");

                string value = Null.NullString;
                CultureInfo invariantCulture = CultureInfo.InvariantCulture;

                foreach (XmlNode xmlProduct in xmlProducts)
                {
                    ProductInfo productInfo = new ProductInfo
                    {
                        PortalID = portalId,
                        CategoryID = (int)slCategories[Convert.ToInt32(xmlProduct["CategoryID"].InnerText)],
                        Manufacturer = xmlProduct["Manufacturer"].InnerText,
                        ModelNumber = xmlProduct["ModelNumber"].InnerText,
                        ModelName = xmlProduct["ModelName"].InnerText,
                        SEOName = xmlProduct["SEOName"].InnerText,
                        Keywords = xmlProduct["Keywords"].InnerText,
                        Summary = xmlProduct["Summary"].InnerText,
                        RegularPrice = Convert.ToDecimal(xmlProduct["RegularPrice"].InnerText, invariantCulture),
                        UnitCost = Convert.ToDecimal(xmlProduct["UnitCost"].InnerText, invariantCulture)
                    };
                    if (xmlProduct["Virtual"] != null && Convert.ToBoolean(xmlProduct["Virtual"].InnerText))
                    {
                        string relativePath = xmlProduct["RelativePath"].InnerText;
                        IFileInfo file = FileManager.Instance.GetFile(portalId, relativePath);

                        if (file != null)
                        {
                            productInfo.IsVirtual = true;
                            productInfo.VirtualFileID = file.FileId;
                            productInfo.AllowedDownloads = Convert.ToInt32(xmlProduct["AllowedDownloads"].InnerText);
                        }
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
                    if (xmlProduct["RoleName"] != null)
                    {
                        string roleName = xmlProduct["RoleName"].InnerText;
                        RoleInfo role = RoleController.Instance.GetRoleByName(portalId, roleName);

                        if (role != null)
                            productInfo.RoleID = role.RoleID;
                    }
                    productInfo.Featured = Convert.ToBoolean(xmlProduct["Featured"].InnerText);
                    if (productInfo.Featured)
                    {
                        productInfo.SaleStartDate = Convert.ToDateTime(xmlProduct["SaleStartDate"].InnerText, invariantCulture);
                        productInfo.SaleEndDate = Convert.ToDateTime(xmlProduct["SaleEndDate"].InnerText, invariantCulture);
                        productInfo.SalePrice = Convert.ToDecimal(xmlProduct["SalePrice"].InnerText, invariantCulture);
                    }
                    if (xmlProduct["ProductImage"] != null)
                    {
                        value = xmlProduct["ProductImage"].InnerText;
                        if (string.IsNullOrEmpty(value) == false)
                        {
                            if (value.StartsWith("http", StringComparison.InvariantCultureIgnoreCase) == false)
                                productInfo.ProductImage = value.Replace("[PortalId]", "Portals/" + portalId);
                            else
                                productInfo.ProductImage = value;
                        }
                    }
                    productInfo.Description = xmlProduct["Description"].InnerText;
                    productInfo.CreatedByUser = userID.ToString();
                    productInfo.CreatedDate = DateTime.Now;
                    AddProduct(productInfo);
                }
            }
        }

        #endregion
    }
}
