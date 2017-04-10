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

using System.Reflection;
using System.Runtime.CompilerServices;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Modules.Store.Components;

namespace DotNetNuke.Modules.Store.Catalog
{
    /// <summary>
    /// Module settings for the catalog module.
    /// </summary>
    public sealed class ModuleSettings
    {
        #region Public Members

        public GeneralSettings General;
		public CategoryProductsSettings CategoryProducts;
		public SearchProductsSettings SearchProducts;
        public SortProductsSettings SortProducts;
		public ProductDetailSettings ProductDetail;
        public AlsoBoughtProductsSettings AlsoBoughtProducts;
        public NewProductsSettings NewProducts;
        public FeaturedProductsSettings FeaturedProducts;
		public PopularProductsSettings PopularProducts;
		public CategoryMenuSettings CategoryMenu;

        #endregion

        #region Public Methods

        public ModuleSettings(int moduleId, int tabId)
		{
            string cacheKey = string.Format("_Mid{0}_Tid{1}", moduleId, tabId);

            General = (GeneralSettings)DataCache.GetCache("StoreGeneralSettings" + cacheKey);
            if (General == null)
            {
                General = new GeneralSettings(moduleId, tabId);
                DataCache.SetCache("StoreGeneralSettings" + cacheKey, General, false);
            }
            CategoryProducts = (CategoryProductsSettings)DataCache.GetCache("StoreCategoryProductsSettings" + cacheKey);
            if (CategoryProducts == null)
            {
 			    CategoryProducts = new CategoryProductsSettings(moduleId, tabId);
                DataCache.SetCache("StoreCategoryProductsSettings" + cacheKey, CategoryProducts, false);
            }
            SearchProducts = (SearchProductsSettings)DataCache.GetCache("StoreSearchProductsSettings" + cacheKey);
            if (SearchProducts == null)
            {
                SearchProducts = new SearchProductsSettings(moduleId, tabId);
                DataCache.SetCache("StoreSearchProductsSettings" + cacheKey, SearchProducts, false);
            }
            SortProducts = (SortProductsSettings)DataCache.GetCache("StoreSortProductsSettings" + cacheKey);
            if (SortProducts == null)
            {
                SortProducts = new SortProductsSettings(moduleId, tabId);
                DataCache.SetCache("StoreSortProductsSettings" + cacheKey, SortProducts, false);
            }
            ProductDetail = (ProductDetailSettings)DataCache.GetCache("StoreProductDetailSettings" + cacheKey);
            if (ProductDetail == null)
            {
			    ProductDetail = new ProductDetailSettings(moduleId, tabId);
                DataCache.SetCache("StoreProductDetailSettings" + cacheKey, ProductDetail, false);
            }
            AlsoBoughtProducts = (AlsoBoughtProductsSettings)DataCache.GetCache("StoreAlsoBoughtProductsSettings" + cacheKey);
            if (AlsoBoughtProducts == null)
            {
                AlsoBoughtProducts = new AlsoBoughtProductsSettings(moduleId, tabId);
                DataCache.SetCache("StoreAlsoBoughtProductsSettings" + cacheKey, AlsoBoughtProducts, false);
            }
            NewProducts = (NewProductsSettings)DataCache.GetCache("StoreNewProductsSettings" + cacheKey);
            if (NewProducts == null)
            {
                NewProducts = new NewProductsSettings(moduleId, tabId);
                DataCache.SetCache("StoreNewProductsSettings" + cacheKey, NewProducts, false);
            }
            FeaturedProducts = (FeaturedProductsSettings)DataCache.GetCache("StoreFeaturedProductsSettings" + cacheKey);
            if (FeaturedProducts == null)
            {
                FeaturedProducts = new FeaturedProductsSettings(moduleId, tabId);
                DataCache.SetCache("StoreFeaturedProductsSettings" + cacheKey, FeaturedProducts, false);
            }
            PopularProducts = (PopularProductsSettings)DataCache.GetCache("StorePopularProductsSettings" + cacheKey);
            if (PopularProducts == null)
            {
			    PopularProducts = new PopularProductsSettings(moduleId, tabId);
                DataCache.SetCache("StorePopularProductsSettings" + cacheKey, PopularProducts, false);
            }
            CategoryMenu = (CategoryMenuSettings)DataCache.GetCache("StoreCategoryMenuSettings" + cacheKey);
            if (CategoryMenu == null)
            {
			    CategoryMenu = new CategoryMenuSettings(moduleId, tabId);
                DataCache.SetCache("StoreCategoryMenuSettings" + cacheKey, CategoryMenu, false);
            }
        }

        public void UpdateCache(int moduleId, int tabId)
        {
            string cacheKey = string.Format("_Mid{0}_Tid{1}", moduleId, tabId);

            DataCache.SetCache("StoreGeneralSettings" + cacheKey, General, false);
            DataCache.SetCache("StoreCategoryProductsSettings" + cacheKey, CategoryProducts, false);
            DataCache.SetCache("StoreSearchProductsSettings" + cacheKey, SearchProducts, false);
            DataCache.SetCache("StoreSortProductsSettings" + cacheKey, SortProducts, false);
            DataCache.SetCache("StoreProductDetailSettings" + cacheKey, ProductDetail, false);
            DataCache.SetCache("StoreAlsoBoughtProductsSettings" + cacheKey, AlsoBoughtProducts, false);
            DataCache.SetCache("StoreNewProductsSettings" + cacheKey, NewProducts, false);
            DataCache.SetCache("StoreFeaturedProductsSettings" + cacheKey, FeaturedProducts, false);
            DataCache.SetCache("StorePopularProductsSettings" + cacheKey, PopularProducts, false);
            DataCache.SetCache("StoreCategoryMenuSettings" + cacheKey, CategoryMenu, false);
        }

        #endregion
    }

    #region General Settings

    public sealed class GeneralSettings : SettingsWrapper
	{
        #region Constructor

        public GeneralSettings(int moduleId, int tabId) : base(moduleId, tabId)
		{
        }

        #endregion

        #region Properties

        [ModuleSetting("template", "Catalog.htm")]
		public string Template
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return GetSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value);
			}
		}

		[ModuleSetting("usedefaultcategory", "false")]
		public bool UseDefaultCategory
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return bool.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

		[ModuleSetting("defaultcategoryid", "0")]
		public int DefaultCategoryID
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return int.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

        [ModuleSetting("displayallproducts", "false")]
        public bool DisplayAllProducts
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return bool.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

		[ModuleSetting("showmessage", "true")]
		public bool ShowMessage
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return bool.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

		[ModuleSetting("showcategoryproducts", "true")]
		public bool ShowCategoryProducts
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return bool.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

		[ModuleSetting("showproductdetail", "true")]
		public bool ShowProductDetail
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return bool.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

        [ModuleSetting("showalsobought", "false")]
        public bool ShowAlsoBought
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return bool.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

        [ModuleSetting("shownewproducts", "false")]
        public bool ShowNewProducts
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return bool.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

		[ModuleSetting("showfeaturedproducts", "false")]
		public bool ShowFeaturedProducts
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return bool.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

		[ModuleSetting("showpopularproducts", "false")]
		public bool ShowPopularProducts
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return bool.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

        [ModuleSetting("allowprint", "false")]
        public bool AllowPrint
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return bool.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("enablecontentindexing", "true")]
        public bool EnableContentIndexing
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return bool.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("enableimagecaching", "true")]
        public bool EnableImageCaching
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return bool.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("cacheimageduration", "20")]
        public int CacheImageDuration
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return int.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        #endregion
    }

	#endregion

    #region Also Bought Product Settings

    public sealed class AlsoBoughtProductsSettings : SettingsWrapper
    {
        #region Contructor

        public AlsoBoughtProductsSettings(int moduleId, int tabId) : base(moduleId, tabId)
        {
        }

        #endregion

        #region Properties

        [ModuleSetting("ablcontainertemplate", "ListContainer.htm")]
        public string ContainerTemplate
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

        [ModuleSetting("abltemplate", "AlsoBoughtProduct.htm")]
        public string Template
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

        [ModuleSetting("ablrowcount", "10")]
        public int RowCount
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return int.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("ablcolumncount", "2")]
        public int ColumnCount
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return int.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("ablcolumnwidth", "200")]
        public int ColumnWidth
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return int.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("ablrepeatdirection", "H")]
        public string RepeatDirection
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

        [ModuleSetting("ablshowthumbnail", "true")]
        public bool ShowThumbnail
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return bool.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("ablthumbnailwidth", "90")]
        public int ThumbnailWidth
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return int.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("ablgifbgcolor", "FFF")]
        public string GIFBgColor
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

        [ModuleSetting("abldetailtabid", "0")]
        public int DetailPage
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return int.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        #endregion
    }

    #endregion

	#region Category Product Settings

    public sealed class CategoryProductsSettings : SettingsWrapper
    {
        #region Constructor

        public CategoryProductsSettings(int moduleId, int tabId) : base(moduleId, tabId)
        {
        }

        #endregion

        #region Properties

        [ModuleSetting("cplcontainertemplate", "CategoryContainer.htm")]
        public string ContainerTemplate
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

		[ModuleSetting("cpltemplate", "ProductList.htm")]
		public string Template
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return GetSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value);
			}
		}

		[ModuleSetting("cplrowcount", "10")]
		public int RowCount
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return int.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

		[ModuleSetting("cplcolumncount", "2")]
		public int ColumnCount
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return int.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

		[ModuleSetting("cplcolumnwidth", "200")]
		public int ColumnWidth
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return int.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

        [ModuleSetting("cplrepeatdirection", "H")]
        public string RepeatDirection
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

        [ModuleSetting("cplshowthumbnail", "true")]
		public bool ShowThumbnail
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return bool.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

		[ModuleSetting("cplthumbnailwidth", "90")]
		public int ThumbnailWidth
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return int.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

        [ModuleSetting("cplgifbgcolor", "FFF")]
        public string GIFBgColor
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

		[ModuleSetting("cpldetailtabid", "0")]
		public int DetailPage
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return int.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

        [ModuleSetting("cplsubcategories", "true")]
        public bool SubCategories
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return bool.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("cplrepositioning", "false")]
        public bool Repositioning
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return bool.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        #endregion
    }

	#endregion

	#region Search Product Settings

    public sealed class SearchProductsSettings : SettingsWrapper
    {
        #region Constructor

        public SearchProductsSettings(int moduleId, int tabId) : base(moduleId, tabId)
		{
		}

        #endregion

        #region Properties

        [ModuleSetting("srltemplate", "SearchResultsList.htm")]
        public string SearchResultsTemplate
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

        [ModuleSetting("cplsearchcolumns", "31")]
        public int SearchColumns
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return int.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("cplsearchby", "2")]
        public int SearchBy
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return int.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        #endregion
    }

	#endregion

    #region Sort Product Settings

    public sealed class SortProductsSettings : SettingsWrapper
    {
        #region Constructor

        public SortProductsSettings(int moduleId, int tabId) : base(moduleId, tabId)
        {
        }

        #endregion

        #region Properties

        [ModuleSetting("cplsortcolumns", "31")]
        public int SortColumns
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return int.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("cplsortby", "2")]
        public int SortBy
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return int.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("cplsortdir", "ASC")]
        public string SortDir
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

        #endregion
    }

    #endregion

    #region Product Detail Settings

    public sealed class ProductDetailSettings : SettingsWrapper
    {
        #region Constructor

        public ProductDetailSettings(int moduleId, int tabId) : base(moduleId, tabId)
		{
		}

        #endregion

        #region Properties

        [ModuleSetting("detailtemplate", "ProductDetail.htm")]
		public string Template
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return GetSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value);
			}
		}

		[ModuleSetting("detailshowthumbnail", "true")]
		public bool ShowThumbnail
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return bool.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

        [ModuleSetting("detailcartwarning", "false")]
		public bool CartWarning
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return bool.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

		[ModuleSetting("detailthumbnailwidth", "300")]
		public int ThumbnailWidth
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return int.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

        [ModuleSetting("detailgifbgcolor", "FFF")]
        public string GIFBgColor
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

        [ModuleSetting("detailshowreviews", "true")]
        public bool ShowReviews
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return bool.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("pdlcategoriestabid", "0")]
        public int ReturnPage
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return int.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        #endregion
    }

	#endregion

    #region New Product Settings

    public sealed class NewProductsSettings : SettingsWrapper
    {
        #region Constructor

        public NewProductsSettings(int moduleId, int tabId) : base(moduleId, tabId)
        {
        }

        #endregion

        #region Properties

        [ModuleSetting("nplcontainertemplate", "ListContainer.htm")]
        public string ContainerTemplate
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

        [ModuleSetting("npltemplate", "NewProduct.htm")]
        public string Template
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

        [ModuleSetting("nplrowcount", "10")]
        public int RowCount
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return int.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("nplcolumncount", "2")]
        public int ColumnCount
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return int.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("nplcolumnwidth", "200")]
        public int ColumnWidth
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return int.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("nplrepeatdirection", "H")]
        public string RepeatDirection
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

        [ModuleSetting("npldisplaybycategory", "true")]
        public bool DisplayByCategory
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return bool.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("nplshowthumbnail", "true")]
        public bool ShowThumbnail
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return bool.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("nplthumbnailwidth", "90")]
        public int ThumbnailWidth
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return int.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("nplgifbgcolor", "FFF")]
        public string GIFBgColor
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

        [ModuleSetting("npldetailtabid", "0")]
        public int DetailPage
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return int.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        #endregion
    }

    #endregion

	#region Featured Product Settings

    public sealed class FeaturedProductsSettings : SettingsWrapper
    {
        #region Constructor

        public FeaturedProductsSettings(int moduleId, int tabId) : base(moduleId, tabId)
		{
		}

        #endregion

        #region Properties

        [ModuleSetting("fplcontainertemplate", "ListContainer.htm")]
        public string ContainerTemplate
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

        [ModuleSetting("fpltemplate", "FeaturedProduct.htm")]
		public string Template
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return GetSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value);
			}
		}

		[ModuleSetting("fplrowcount", "10")]
		public int RowCount
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return int.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

		[ModuleSetting("fplcolumncount", "2")]
		public int ColumnCount
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return int.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

		[ModuleSetting("fplcolumnwidth", "200")]
		public int ColumnWidth
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return int.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

        [ModuleSetting("fplrepeatdirection", "H")]
        public string RepeatDirection
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

        [ModuleSetting("fpldisplaybycategory", "true")]
        public bool DisplayByCategory
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return bool.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("fplshowthumbnail", "true")]
		public bool ShowThumbnail
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return bool.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

		[ModuleSetting("fplthumbnailwidth", "90")]
		public int ThumbnailWidth
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return int.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

        [ModuleSetting("fplgifbgcolor", "FFF")]
        public string GIFBgColor
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

		[ModuleSetting("fpldetailtabid", "0")]
		public int DetailPage
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return int.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
        }

        #endregion
    }

	#endregion

	#region Popular Product Settings

    public sealed class PopularProductsSettings : SettingsWrapper
    {
        #region Constructor

        public PopularProductsSettings(int moduleId, int tabId) : base(moduleId, tabId)
		{
		}

        #endregion

        #region Properties

        [ModuleSetting("pplcontainertemplate", "ListContainer.htm")]
        public string ContainerTemplate
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

        [ModuleSetting("ppltemplate", "PopularProduct.htm")]
		public string Template
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return GetSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value);
			}
		}

		[ModuleSetting("pplrowcount", "10")]
		public int RowCount
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return int.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

		[ModuleSetting("pplcolumncount", "2")]
		public int ColumnCount
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return int.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

		[ModuleSetting("pplcolumnwidth", "200")]
		public int ColumnWidth
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return int.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

        [ModuleSetting("pplrepeatdirection", "H")]
        public string RepeatDirection
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

        [ModuleSetting("pplshowthumbnail", "true")]
		public bool ShowThumbnail
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return bool.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

		[ModuleSetting("pplthumbnailwidth", "90")]
		public int ThumbnailWidth
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return int.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

        [ModuleSetting("pplgifbgcolor", "FFF")]
        public string GIFBgColor
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

		[ModuleSetting("ppldetailtabid", "0")]
		public int DetailPage
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return int.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
        }

        #endregion
    }

	#endregion

	#region Category Menu Settings

    public sealed class CategoryMenuSettings : SettingsWrapper
    {
        #region Constructor

        public CategoryMenuSettings(int moduleId, int tabId) : base(moduleId, tabId)
		{
		}

        #endregion

        #region Properties

        [ModuleSetting("MenuDisplayMode", "T")]
        public string DisplayMode
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return GetSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value);
			}
		}

		[ModuleSetting("MenuColumnCount", "1")]
		public int ColumnCount
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return int.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
		}

		[ModuleSetting("MenuCatalogTabId", "0")]
		public int CatalogPage
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return int.Parse(GetSetting(m));
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				SetSetting(m, value.ToString());
			}
        }

        #endregion
    }

	#endregion
}
