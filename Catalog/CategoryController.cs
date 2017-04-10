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
using System.Xml;
using System.Text;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;

namespace DotNetNuke.Modules.Store.Catalog
{
	/// <summary>
	/// Category business class used to manage categories.
	/// </summary>
    public sealed class CategoryController : PortalModuleBase, IPortable
	{
        #region Private Members

        private const string Cachekey = "StoreCategories_P{0}_A{1}_PC{2}";
        private const string Cachekeypath = "StoreCategoriesPath_P{0}_A{1}_PC{2}";

        private static readonly List<string> CacheKeys = new List<string>();

        #endregion

        #region Private Methods

        private string CreatePath(int portalID, int parentId, string categoryName)
        {
            string pathName = categoryName;
            CategoryInfo category;
            while (parentId > 0)
            {
                category = GetCategory(portalID, parentId);
                pathName = category.Name + " > " + pathName;
                parentId = category.ParentCategoryID;
            }
            return pathName;
        }

        #endregion

		#region Public Methods

        public CategoryInfo GetCategoryPath(int portalID, int categoryId)
        {
            CategoryInfo category = GetCategory(portalID, categoryId);
            if (category.ParentCategoryID > 0)
                category.CategoryPathName = CreatePath(portalID, category.ParentCategoryID, category.Name);
            else
                category.CategoryPathName = category.Name;
            return category;
        }

        public List<CategoryInfo> GetCategoriesPath(int portalID, bool includeArchived, int parentCategoryID)
        {
            string cacheKey = string.Format(Cachekeypath, portalID, includeArchived, parentCategoryID);
            List<CategoryInfo> categories = (List<CategoryInfo>)DataCache.GetCache(cacheKey);
            if (categories == null)
            {
                categories = GetCategories(portalID, includeArchived, parentCategoryID);
                if (categories != null)
                {
                    foreach (CategoryInfo category in categories)
                    {
                        if (category.ParentCategoryID > 0)
                            category.CategoryPathName = CreatePath(portalID, category.ParentCategoryID, category.Name);
                        else
                            category.CategoryPathName = category.Name;
                    }
                    categories.Sort(new CategoryPathNameComparer());
                    DataCache.SetCache(cacheKey, categories, false);
                    if (!CacheKeys.Contains(cacheKey))
                        CacheKeys.Add(cacheKey);
                }
            }
            return categories;
        }

        public CategoryInfo GetCategory(int portalId, int categoryID)
		{
            return CBO.FillObject<CategoryInfo>(DataProvider.Instance().GetCategory(portalId, categoryID));
		}

        public List<CategoryInfo> GetCategories(int portalID, bool includeArchived, int parentCategoryID)
		{
            string cacheKey = string.Format(Cachekey, portalID, includeArchived, parentCategoryID);
            List<CategoryInfo> categories = (List<CategoryInfo>)DataCache.GetCache(cacheKey);
            if (categories == null)
            {
                categories = CBO.FillCollection<CategoryInfo>(DataProvider.Instance().GetCategories(portalID, includeArchived, parentCategoryID));
                if (categories != null)
                {
                    DataCache.SetCache(cacheKey, categories, false);
                    if (!CacheKeys.Contains(cacheKey))
                        CacheKeys.Add(cacheKey);
                }
            }
            return categories;
		}

        public void ClearAllCaches()
        {
            foreach (string cacheKey in CacheKeys)
                DataCache.RemoveCache(cacheKey);
        }

		public int CategoryCount(int portalID)
		{
			return DataProvider.Instance().CategoryCount(portalID);
		}

        public int AddCategory(CategoryInfo categoryInfo)
        {
            int categoryId = DataProvider.Instance().AddCategory(categoryInfo.PortalID, categoryInfo.Name, categoryInfo.SEOName,
                categoryInfo.Keywords, categoryInfo.Description, categoryInfo.Message, categoryInfo.Archived,
                categoryInfo.CreatedByUser, categoryInfo.CreatedDate, categoryInfo.OrderID, categoryInfo.ParentCategoryID);
            ClearAllCaches();
            return categoryId;
        }

		public void UpdateCategory(CategoryInfo categoryInfo)
		{
            DataProvider.Instance().UpdateCategory(categoryInfo.CategoryID, categoryInfo.Name, categoryInfo.SEOName, categoryInfo.Keywords,
                 categoryInfo.Description, categoryInfo.Message, categoryInfo.Archived, categoryInfo.OrderID, categoryInfo.ParentCategoryID);
            ClearAllCaches();
		}

		public void DeleteCategory(int categoryID)
		{
			DataProvider.Instance().DeleteCategory(categoryID);
            ClearAllCaches();
		}

		public void DeleteCategories(int portalID)
		{
			DataProvider.Instance().DeleteCategories(portalID);
            ClearAllCaches();
		}

		#endregion

        #region IPortable Members

        string IPortable.ExportModule(int moduleID)
        {
            // Export categories
            List<CategoryInfo> categories = GetCategories(PortalId, true, -3);

            if (categories.Count > 0)
            {
                StringBuilder strXML = new StringBuilder();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                using (XmlWriter writer = XmlWriter.Create(strXML, settings))
                {
                    writer.WriteStartElement("Categories");
                    foreach (CategoryInfo category in categories)
                    {
                        writer.WriteStartElement("Category");
                        writer.WriteElementString("CategoryId", category.CategoryID.ToString());
                        writer.WriteElementString("Name", category.Name);
                        writer.WriteElementString("SEOName", category.SEOName);
                        writer.WriteElementString("Keywords", category.Keywords);
                        writer.WriteElementString("Description", category.Description);
                        writer.WriteElementString("OrderId", category.OrderID.ToString());
                        writer.WriteElementString("ParentCategoryId", category.ParentCategoryID.ToString());
                        writer.WriteElementString("Archived", category.Archived.ToString());
                        writer.WriteElementString("Message", category.Message);
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
            XmlNode xmlCategories = Common.Globals.GetContent(content, "Categories");
            SortedList slCategories = new SortedList(xmlCategories.ChildNodes.Count);
            int indexID = Null.NullInteger;

            foreach (XmlNode xmlCategory in xmlCategories)
            {
                CategoryInfo category = new CategoryInfo();
                category.PortalID = PortalId;
                category.Name = xmlCategory["Name"].InnerText;
                category.SEOName = xmlCategory["SEOName"].InnerText;
                category.Keywords = xmlCategory["Keywords"].InnerText;
                category.Description = xmlCategory["Description"].InnerText;
                category.OrderID = Convert.ToInt32(xmlCategory["OrderId"].InnerText);
                indexID = slCategories.IndexOfKey(Convert.ToInt32(xmlCategory["ParentCategoryId"].InnerText));
                if (indexID > -1)
                    category.ParentCategoryID = (int)slCategories.GetByIndex(indexID);
                else
                    category.ParentCategoryID = -1;
                category.Archived = Convert.ToBoolean(xmlCategory["Archived"].InnerText);
                category.Message = xmlCategory["Message"].InnerText;
                category.CreatedByUser = UserId.ToString();
                category.CreatedDate = DateTime.Now;
                slCategories.Add(Convert.ToInt32(xmlCategory["CategoryId"].InnerText), AddCategory(category));
            }
        }

        #endregion

        #region Nested Class CategoryPathName Comparer

        private class CategoryPathNameComparer : IComparer<CategoryInfo>
        {
            #region IComparer<CategoryInfo> Interface

            public int Compare(CategoryInfo x, CategoryInfo y)
            {
                return x.CategoryPathName.CompareTo(y.CategoryPathName);
            }

            #endregion
        }

        #endregion
    }
}
