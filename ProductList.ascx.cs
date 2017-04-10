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
using System.Globalization;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Modules.Store.Catalog;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for Media.
	/// </summary>
	public partial class ProductList : StoreControlBase
	{
		#region Private Members

        private ModuleSettings _moduleSettings;
        private CatalogNavigation _moduleNav;
		private ProductInfo _productInfo;
        private CategoryController _categoryControler;
        private int _categoryID;
		private string _templatePath = "";
		private string _imagesPath = "";
		private string _title = "";
        private string _containerTemplate = "";
        private string _containerCssClass = "";
		private string _template = "";
        private string _itemCssClass = "";
        private string _alternatingItemCssClass = "";
		private int _rowCount = 10;
		private int _columnCount = 2;
		private int _columnWidth = 200;
        private string _direction = "";
		private bool _showThumbnail = true;
		private int _thumbnailWidth = 90;
        private string _gifBgColor = "";
        private bool _enableImageCaching = true;
        private int _cacheImageDuration = 2;
        private bool _showDetail = true;
		private int _detailPageID;
        private ProductListTypes _listType;
        private readonly NumberFormatInfo _localFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
        private readonly List<Label> _labelsPageInfo = new List<Label>();
        private readonly List<Label> _labelsPageNav = new List<Label>();
        private readonly List<HyperLink> _buttonsPrevious = new List<HyperLink>();
        private readonly List<HyperLink> _buttonsNext = new List<HyperLink>();
        private readonly List<PlaceHolder> _placeholdersPageList = new List<PlaceHolder>();
        private DataList _lstProducts;
        private Repeater _rlProducts;
        private List<ProductInfo> _productList;
        private enum SortColumn
        {
            Manufacturer = 1,
            ModelNumber = 2,
            ModelName = 4,
            UnitPrice = 8,
            Date = 16
        }
        private enum SearchColumn
        {
            Manufacturer = 1,
            ModelNumber = 2,
            ModelName = 4,
            ProductSummay = 8,
            ProductDescription = 16
        }

        #endregion

		#region Properties

        public ModuleSettings ModuleSettings
        {
            get { return _moduleSettings; }
            set { _moduleSettings = value; }
        }

        public int CategoryID
		{
			get {return _categoryID;}
			set {_categoryID = value;}
		}

		public string Title
		{
			get {return _title;}
			set {_title = value;}
		}

        public string TemplatePath
		{
            get { return _templatePath; }
            set { _templatePath = value; }
		}

        public string ContainerTemplate
		{
            get { return _containerTemplate; }
            set { _containerTemplate = value; }
		}

        public string ContainerCssClass
		{
            get { return _containerCssClass; }
            set { _containerCssClass = value; }
		}

		public string Template
		{
			get {return _template;}
			set {_template = value;}
		}

		public string ItemCssClass
		{
            get { return _itemCssClass; }
            set { _itemCssClass = value; }
		}

        public string AlternatingItemCssClass
		{
            get { return _alternatingItemCssClass; }
            set { _alternatingItemCssClass = value; }
		}

		public int RowCount
		{
			get {return _rowCount;}
			set {_rowCount = value;}
		}

		public int ColumnCount
		{
			get {return _columnCount;}
			set {_columnCount = value;}
		}

		public int ColumnWidth
		{
			get {return _columnWidth;}
			set {_columnWidth = value;}
		}

        public string Direction
		{
            get { return _direction; }
            set { _direction = value; }
		}

		public bool ShowThumbnail
		{
			get {return _showThumbnail;}
			set {_showThumbnail = value;}
		}
		
		public int ThumbnailWidth
		{
			get {return _thumbnailWidth;}
			set {_thumbnailWidth = value;}
		}

        public string GIFBgColor
		{
            get { return _gifBgColor; }
            set { _gifBgColor = value; }
		}

        public bool EnableImageCaching
        {
            get { return _enableImageCaching; }
            set { _enableImageCaching = value; }
        }

        public int CacheImageDuration
        {
            get { return _cacheImageDuration; }
            set { _cacheImageDuration = value; }
        }

		public bool ShowDetail
		{
            get { return _showDetail; }
            set { _showDetail = value; }
		}

		public int DetailPage
		{
			get { return _detailPageID; }
			set { _detailPageID = value; }
		}

        public ProductListTypes ListType
        {
            get { return _listType; }
            set { _listType = value; }
        }

		#endregion

		#region Events

		protected void Page_Load(object sender, EventArgs e)
		{
            _imagesPath = _templatePath + "Images/";

            if (!string.IsNullOrEmpty(StoreSettings.CurrencySymbol))
                _localFormat.CurrencySymbol = StoreSettings.CurrencySymbol;

            _moduleNav = new CatalogNavigation(Request.QueryString);

            //0 indicates that no detail page is being used, so use current tabid
			if (DetailPage == 0) 
				DetailPage = TabId;

			if (_moduleNav.PageIndex == Null.NullInteger)
				_moduleNav.PageIndex = 1;

            // Get the product data
            _productList = (DataSource as List<ProductInfo>);

            if (_containerTemplate == string.Empty)
                Controls.Add(TemplateController.ParseTemplate(MapPath(_templatePath), "ListContainer.htm", Localization.GetString("TemplateError", LocalSharedResourceFile), IsLogged, new ProcessTokenDelegate(ProcessToken)));
            else
                Controls.Add(TemplateController.ParseTemplate(MapPath(_templatePath), _containerTemplate, Localization.GetString("TemplateError", LocalSharedResourceFile), IsLogged, new ProcessTokenDelegate(ProcessToken)));

            if (ListType == ProductListTypes.Category || ListType == ProductListTypes.SearchResults)
                BindPagedData();
            else
            {
                if (_lstProducts != null)
                {
                    _lstProducts.DataSource = _productList;
                    _lstProducts.DataBind();
                }
                else
                {
                    _rlProducts.DataSource = _productList;
                    _rlProducts.DataBind();
                }
            }
        }

        protected void lstProducts_ItemDataBound(object sender, DataListItemEventArgs e)
		{
			_productInfo = (ProductInfo)e.Item.DataItem;

            if (_columnWidth > 0)
                e.Item.Width = _columnWidth;

            switch (e.Item.ItemType)
            {
                case ListItemType.Item:
                    e.Item.CssClass = _itemCssClass;
                    break;
                case ListItemType.AlternatingItem:
                    e.Item.CssClass = _alternatingItemCssClass;
                    break;
            }

            ProductDetail productListItem = (ProductDetail)LoadControl(ModulePath + "ProductDetail.ascx");
            productListItem.ModuleConfiguration = ModuleConfiguration;
            productListItem.StoreSettings = StoreSettings;
            productListItem.TemplatePath = _templatePath;
            productListItem.Template = _template;
            productListItem.CategoryID = _productInfo.CategoryID;
            productListItem.ShowThumbnail = _showThumbnail;
            productListItem.ThumbnailWidth = _thumbnailWidth;
            productListItem.GIFBgColor = _gifBgColor;
            productListItem.EnableImageCaching = _enableImageCaching;
            productListItem.CacheImageDuration = _cacheImageDuration;
            productListItem.DataSource = _productInfo;
            productListItem.ShowDetail = _showDetail & (_detailPageID != Null.NullInteger);
            productListItem.DetailID = _detailPageID;
            productListItem.InList = true;

            e.Item.Controls.Add(productListItem);
		}

        protected void rlProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                _productInfo = (ProductInfo)e.Item.DataItem;

                ProductDetail productListItem = (ProductDetail)LoadControl(ModulePath + "ProductDetail.ascx");
                productListItem.ModuleConfiguration = ModuleConfiguration;
                productListItem.StoreSettings = StoreSettings;
                productListItem.TemplatePath = _templatePath;
                productListItem.Template = _template;
                productListItem.CategoryID = _productInfo.CategoryID;
                productListItem.ShowThumbnail = _showThumbnail;
                productListItem.ThumbnailWidth = _thumbnailWidth;
                productListItem.GIFBgColor = _gifBgColor;
                productListItem.EnableImageCaching = _enableImageCaching;
                productListItem.CacheImageDuration = _cacheImageDuration;
                productListItem.DataSource = _productInfo;
                productListItem.ShowDetail = !_showDetail & (_detailPageID == Null.NullInteger) ? false : true;
                productListItem.DetailID = _detailPageID;
                productListItem.InList = true;

                LiteralControl litItem = null;
                switch (e.Item.ItemType)
                {
                    case ListItemType.Item:
                        litItem = new LiteralControl("<li class=\"" + _itemCssClass + "\">");
                        break;
                    case ListItemType.AlternatingItem:
                        litItem = new LiteralControl("<li class=\"" + _alternatingItemCssClass + "\">");
                        break;
                }

                if (litItem != null)
                {
                    e.Item.Controls.Add(litItem);
                    e.Item.Controls.Add(productListItem);
                    e.Item.Controls.Add(new LiteralControl("</li>"));
                }
            }
        }

        protected void ddlSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlSortBy = (sender as DropDownList);
            if (ddlSortBy != null)
            {
                _moduleNav.SortID = int.Parse(ddlSortBy.SelectedValue);
                _moduleNav.SortDir = "ASC";
                _moduleNav.PageIndex = Null.NullInteger;
            }
            Response.Redirect(_moduleNav.GetNavigationUrl());
        }

        protected void btnSortDir_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton button = (sender as ImageButton);
            if (button != null)
            {
                _moduleNav.PageIndex = Null.NullInteger;
                if (button.CommandArgument.ToUpper() == "ASC")
                    _moduleNav.SortDir = "DESC";
                else
                    _moduleNav.SortDir = "ASC";
                Response.Redirect(_moduleNav.GetNavigationUrl());
            }
        }

		#endregion

		#region Private Methods

		private void BindPagedData()
		{
			if (_productList.Count == 0)
			{
                foreach (Label lblPageNav in _labelsPageNav)
                    lblPageNav.Visible = false;
            }
			else
			{
			    int pageSize = _rowCount*_columnCount;
                int currentPage = _moduleNav.PageIndex - 1;	// Convert to zero-based index

                foreach (Label lblPageNav in _labelsPageNav)
                    lblPageNav.Visible = true;

				// Create paged data source
                PagedDataSource pagedData = new PagedDataSource();
                pagedData.DataSource = _productList;
				pagedData.AllowPaging = true;
				pagedData.PageSize = pageSize;
				pagedData.CurrentPageIndex = currentPage;

                UpdatePagingControls(pagedData.PageCount, _moduleNav.PageIndex);

			    // Databind with paged data source
                if (_lstProducts != null)
                {
                    _lstProducts.DataSource = pagedData;
                    _lstProducts.DataBind();
                }
                else
                {
                    _rlProducts.DataSource = pagedData;
                    _rlProducts.DataBind();
                }
			}
        }

        private void UpdatePagingControls(int totalPages, int currentPage)
		{
			// Hide and return if only one page
			if (totalPages == 1)
			{
                foreach (Label lblPageNav in _labelsPageNav)
                    lblPageNav.Visible = false;
                return;
			}

			////////////////////////////
			// Previous/Next Buttons
			int prevIndex = currentPage - 1;
			if ((prevIndex < 1) || (prevIndex > totalPages))
				prevIndex = 1;

			StringDictionary replaceParams = new StringDictionary();

			replaceParams["PageIndex"] = prevIndex.ToString();
            foreach (HyperLink btnPrev in _buttonsPrevious)
            {
                if (_moduleSettings.CategoryProducts.Repositioning)
                    btnPrev.NavigateUrl = _moduleNav.GetNavigationUrl(replaceParams) + "#" + ModuleId;
                else
                    btnPrev.NavigateUrl = _moduleNav.GetNavigationUrl(replaceParams);
            }

			int nextIndex = currentPage + 1;
			if (nextIndex >= totalPages)
				nextIndex = totalPages;

			replaceParams["PageIndex"] = nextIndex.ToString();
            foreach (HyperLink btnNext in _buttonsNext)
            {
                if (_moduleSettings.CategoryProducts.Repositioning)
                    btnNext.NavigateUrl = _moduleNav.GetNavigationUrl(replaceParams) + "#" + ModuleId;
                else
                    btnNext.NavigateUrl = _moduleNav.GetNavigationUrl(replaceParams);
            }

			////////////////////////////
			// Page Index List

			// Determine page range to display
			int rangeMin = currentPage - 10;
			int rangeMax = currentPage + 10;

			if (rangeMin < 1)
			{
				rangeMax = rangeMax + Math.Abs(rangeMin) + 1;
				rangeMin = 1;
			}

			if (rangeMax >= totalPages)
			{
				rangeMin = rangeMin - (rangeMax - totalPages);
				if (rangeMin <= 1)
					rangeMin = 1;
				rangeMax = totalPages;
			}

            foreach (PlaceHolder phPageList in _placeholdersPageList)
            {
                // Create link for each page
                for (int i = rangeMin; i <= rangeMax; i++)
                {
                    replaceParams["PageIndex"] = i.ToString();

                    if (i == currentPage)
                    {
                        Label pageLabel = new Label();
                        pageLabel.Text = i.ToString();
                        pageLabel.CssClass = "StoreCurrentPageNumber";
                        phPageList.Controls.Add(pageLabel);
                        phPageList.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                    }
                    else
                    {
                        HyperLink pageLink = new HyperLink();
                        pageLink.Text = i.ToString();
                        if (_moduleSettings.CategoryProducts.Repositioning)
                            pageLink.NavigateUrl = _moduleNav.GetNavigationUrl(replaceParams) + "#" + ModuleId;
                        else
                            pageLink.NavigateUrl = _moduleNav.GetNavigationUrl(replaceParams);
                        pageLink.CssClass = "StorePageNumber";
                        phPageList.Controls.Add(pageLink);
                        phPageList.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                    }

                }
            }

            foreach (Label lblPageInfo in _labelsPageInfo)
            {
                lblPageInfo.Text = string.Format(Localization.GetString("PageInfo.Text", LocalResourceFile), currentPage, totalPages);
            }
        }

		private Control ProcessToken(string tokenName)
		{
			switch (tokenName.ToUpper())
			{
                case "LISTTITLE":
                    Label lblTitle = new Label();
                    lblTitle.CssClass = "StoreListTitle";
                    lblTitle.Text = _title;
                    return lblTitle;

                case "PAGENAV":
                    if ((ListType == ProductListTypes.Category || ListType == ProductListTypes.SearchResults) && _productList.Count > 0)
                    {
                        HyperLink btnPrevious = new HyperLink();
                        btnPrevious.Text = Localization.GetString("Previous", LocalResourceFile);
                        btnPrevious.CssClass = "StorePagePrevious";
                        _buttonsPrevious.Add(btnPrevious);

                        Literal lblSpace = new Literal();
                        lblSpace.Text = "&nbsp;&nbsp;";

                        HyperLink btnNext = new HyperLink();
                        btnNext.Text = Localization.GetString("Next", LocalResourceFile);
                        btnNext.CssClass = "StorePageNext";
                        _buttonsNext.Add(btnNext);

                        PlaceHolder phPageList = new PlaceHolder();
                        _placeholdersPageList.Add(phPageList);

                        Label lblPageNav = new Label();
                        lblPageNav.Controls.Add(btnPrevious);
                        lblPageNav.Controls.Add(lblSpace);
                        lblPageNav.Controls.Add(phPageList);
                        lblPageNav.Controls.Add(btnNext);
                        lblPageNav.CssClass = "StorePageNav";
                        _labelsPageNav.Add(lblPageNav);
                        return lblPageNav;
                    }

                    return null;

                case "PAGEINFO":
                    if (_productList.Count > 0)
                    {
                        Label lblPageInfo = new Label();
                        lblPageInfo.CssClass = "StorePageInfo";
                        lblPageInfo.Text = string.Format(Localization.GetString("PageInfo", LocalResourceFile), 1, 1);
                        _labelsPageInfo.Add(lblPageInfo);
                        return lblPageInfo;
                    }

                    return null;

                case "PRODUCTS":
                    if (_productList.Count > 0)
                    {
                        _lstProducts = new DataList();
                        _lstProducts.ID = "ProductsList";
                        _lstProducts.CssClass = _containerCssClass;
                        _lstProducts.RepeatLayout = RepeatLayout.Table;
                        switch (_direction)
                        {
                            case "V":
                                _lstProducts.RepeatDirection = RepeatDirection.Vertical;
                                break;
                            case "H":
                            default:
                                _lstProducts.RepeatDirection = RepeatDirection.Horizontal;
                                break;
                        }
                        _lstProducts.RepeatColumns = _columnCount;
                        _lstProducts.ItemDataBound += lstProducts_ItemDataBound;
                        return _lstProducts;
                    }

                    Label lblEmpty = new Label();
                    lblEmpty.CssClass = "StoreEmptyList";
                    if (ListType == ProductListTypes.Category)
                    {
                        lblEmpty.Text = Localization.GetString("CategoryEmpty", LocalResourceFile);
                    }
                    else if (ListType == ProductListTypes.SearchResults)
                    {
                        lblEmpty.Text = Localization.GetString("SearchEmpty", LocalResourceFile);
                    }
                    return lblEmpty;

                case "ULISTPRODUCTS":
                    if (_productList.Count > 0)
                    {
                        _rlProducts = new Repeater();
                        _rlProducts.ID = "UnorderedProductsList";
                        _rlProducts.HeaderTemplate = new ListTemplate("<ul>");
                        _rlProducts.ItemTemplate = new ListTemplate();
                        _rlProducts.FooterTemplate = new ListTemplate("</ul>");
                        _rlProducts.ItemDataBound += rlProducts_ItemDataBound;
                        return _rlProducts;
                    }

                    return null;

                case "ITEMSCOUNT":
                    if (_productList != null)
                    {
                        Label lblItems = new Label();
                        lblItems.CssClass = "StoreItemsCount";
                        lblItems.Text = string.Format(Localization.GetString("ItemsCount", LocalResourceFile), _productList.Count);
                        return lblItems;
                    }

                    return null;

                case "SELECTEDCATEGORY":
                    if (ListType == ProductListTypes.Category)
                    {
                        if (_moduleNav.CategoryID == Null.NullInteger && _moduleSettings.General.DisplayAllProducts)
                        {
                            Label lblProductCategory = new Label();
                            lblProductCategory.Text = Localization.GetString("FullCatalog", LocalResourceFile);
                            lblProductCategory.CssClass = "StoreSelectedCategory";
                            return lblProductCategory;
                        }

                        _categoryControler = new CategoryController();
                        CategoryInfo categoryInfo = _categoryControler.GetCategory(PortalId, CategoryID);
                        if (categoryInfo != null)
                        {
                            Label lblProductCategory = new Label();
                            lblProductCategory.Text = string.Format(Localization.GetString("SelectedCategory", LocalResourceFile), categoryInfo.Name);
                            lblProductCategory.CssClass = "StoreSelectedCategory";
                            return lblProductCategory;
                        }
                    }

                    return null;

                case "CATEGORIESBREADCRUMB":
                    if (ListType == ProductListTypes.Category && CategoryID != Null.NullInteger)
                    {
                        _categoryControler = new CategoryController();
                        CategoryInfo categoryInfo = _categoryControler.GetCategory(PortalId, CategoryID);
                        if (categoryInfo != null)
                        {
                            // Create label to contains all other controls
                            Label lblCategoriesBreadcrumb = new Label();
                            lblCategoriesBreadcrumb.CssClass = "StoreCategoriesBreadcrumb";
                            // Create "before" label with locale resource
                            Label lblBeforeBreadcrumb = new Label();
                            lblBeforeBreadcrumb.Text = Localization.GetString("BeforeCategoriesBreadcrumb", LocalResourceFile);
                            lblBeforeBreadcrumb.CssClass = "StoreBeforeBreadcrumb";
                            lblCategoriesBreadcrumb.Controls.Add(lblBeforeBreadcrumb);
                            // Create label to contains categories
                            Label lblBreadcrumb = new Label();
                            lblBreadcrumb.CssClass = "StoreBreadcrumb";
                            lblCategoriesBreadcrumb.Controls.Add(lblBreadcrumb);
                            // Create literal with selected category name
                            Literal litSelectedCategory = new Literal();
                            litSelectedCategory.Text = categoryInfo.Name;
                            lblBreadcrumb.Controls.Add(litSelectedCategory);
                            // Create "between" label with locale resource
                            Literal litBetwenBreadcrumb;
                            String betweenBreadcrumb = Localization.GetString("BetweenCategoriesBreadcrumb", LocalResourceFile);
                            // Create catalog navigation object to compute hyperlink URL
                            CatalogNavigation categoryNav = new CatalogNavigation();
                            // Loop for parent categories (if any)
                            int parentCategoryID = categoryInfo.ParentCategoryID;
                            while (parentCategoryID != Null.NullInteger)
                            {
                                // Get parent category
                                categoryInfo = _categoryControler.GetCategory(PortalId, parentCategoryID);
                                if (categoryInfo != null)
                                {
                                    // Insert separator
                                    litBetwenBreadcrumb = new Literal();
                                    litBetwenBreadcrumb.Text = betweenBreadcrumb;
                                    lblBreadcrumb.Controls.AddAt(0, litBetwenBreadcrumb);
                                    // Create hyperlink with the parent category
                                    HyperLink hlCategory = new HyperLink();
                                    hlCategory.Text = categoryInfo.Name;
                                    categoryNav.CategoryID = categoryInfo.CategoryID;
                                    if (StoreSettings.SEOFeature)
                                        categoryNav.Category = categoryInfo.SEOName;
                                    hlCategory.NavigateUrl = categoryNav.GetNavigationUrl();
                                    lblBreadcrumb.Controls.AddAt(0, hlCategory);
                                    parentCategoryID = categoryInfo.ParentCategoryID;
                                }
                                else
                                    parentCategoryID = Null.NullInteger;
                            }
                            // Create "after" label with locale resource
                            Label lblAfterBreadcrumb = new Label();
                            lblAfterBreadcrumb.Text = Localization.GetString("AfterCategoriesBreadcrumb", LocalResourceFile);
                            lblAfterBreadcrumb.CssClass = "StoreAfterBreadcrumb";
                            lblCategoriesBreadcrumb.Controls.Add(lblAfterBreadcrumb);
                            return lblCategoriesBreadcrumb;
                        }
                    }

                    return null;

                case "SORTBY":
                    if (_moduleSettings.SortProducts.SortColumns != 0 && (ListType == ProductListTypes.Category || ListType == ProductListTypes.SearchResults))
                    {
                        // Create label to contains all other controls
                        Label lblSortBy = new Label();
                        lblSortBy.CssClass = "StoreSortBy";
                        // Create literal text with locale resource
                        Literal litSortBy = new Literal();
                        litSortBy.Text = Localization.GetString("SortBy", LocalResourceFile);
                        lblSortBy.Controls.Add(litSortBy);
                        // Create DropDownList with column names
                        DropDownList ddlSortBy = new DropDownList();
                        ddlSortBy.AutoPostBack = true;
                        ddlSortBy.CssClass = "StoreSortByColumns";
                        int sortColumns = _moduleSettings.SortProducts.SortColumns;
                        if ((sortColumns & (int)SortColumn.Manufacturer) != 0)
                            ddlSortBy.Items.Add(new ListItem(Localization.GetString("SortManufacturer", LocalResourceFile), "0"));
                        if ((sortColumns & (int)SortColumn.ModelNumber) != 0)
                            ddlSortBy.Items.Add(new ListItem(Localization.GetString("SortModelNumber", LocalResourceFile), "1"));
                        if ((sortColumns & (int)SortColumn.ModelName) != 0)
                            ddlSortBy.Items.Add(new ListItem(Localization.GetString("SortModelName", LocalResourceFile), "2"));
                        if ((sortColumns & (int)SortColumn.UnitPrice) != 0)
                            ddlSortBy.Items.Add(new ListItem(Localization.GetString("SortUnitPrice", LocalResourceFile), "3"));
                        if ((sortColumns & (int)SortColumn.Date) != 0)
                            ddlSortBy.Items.Add(new ListItem(Localization.GetString("SortCreatedDate", LocalResourceFile), "4"));
                        string sortColumn;
                        // Define sort column
                        if (_moduleNav.SortID != Null.NullInteger)
                        {
                            // Currently selected sort column
                            sortColumn = _moduleNav.SortID.ToString();
                        }
                        else
                        {
                            // Default sort column
                            sortColumn = _moduleSettings.SortProducts.SortBy.ToString();
                        }
                        ListItem itemSortColumn = ddlSortBy.Items.FindByValue(sortColumn);
                        if (itemSortColumn != null)
                            itemSortColumn.Selected = true;
                        ddlSortBy.SelectedIndexChanged += ddlSortBy_SelectedIndexChanged;
                        if (_productList.Count == 0)
                            ddlSortBy.Enabled = false;
                        lblSortBy.Controls.Add(ddlSortBy);
                        // Create Sort Order image button
                        ImageButton btnSortDir = new ImageButton();
                        btnSortDir.CssClass = "StoreSortByLinkButton";
                        string sortDir;
                        if (_moduleNav.SortDir != Null.NullString)
                        {
                            // Currently selected sort direction
                            sortDir = _moduleNav.SortDir;
                        }
                        else
                        {
                            // Default sort direction
                            sortDir = _moduleSettings.SortProducts.SortDir;
                        }
                        string imageName;
                        string altText;
                        if (sortDir.ToUpper() == "ASC")
                        {
                            imageName = "arrow_up.png";
                            altText = Localization.GetString("SortAscending", LocalResourceFile);
                        }
                        else
                        {
                            imageName = "arrow_down.png";
                            altText = Localization.GetString("SortDescending", LocalResourceFile);
                        }
                        btnSortDir.CommandArgument = sortDir;
                        btnSortDir.AlternateText = altText;
                        if (StoreSettings.PortalTemplates)
                        {
                            btnSortDir.ImageUrl = PortalSettings.HomeDirectory + "Store/Templates/Images/" + imageName;
                        }
                        else
                        {
                            btnSortDir.ImageUrl = TemplateSourceDirectory + "/Templates/Images/" + imageName;
                        }
                        btnSortDir.Click += btnSortDir_Click;
                        if (_productList.Count == 0)
                            btnSortDir.Enabled = false;
                        lblSortBy.Controls.Add(btnSortDir);
                        return lblSortBy;
                    }

                    return null;

				default:
					LiteralControl litText = new LiteralControl(tokenName);
					return litText;
			}
		}

		#endregion
	}
}
