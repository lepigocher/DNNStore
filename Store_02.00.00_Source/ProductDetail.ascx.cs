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
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Skins;

using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Cart;
using DotNetNuke.Modules.Store.Catalog;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Providers.Tax;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
    /// Summary description for ProductDetail.
	/// </summary>
	public partial  class ProductDetail : StoreControlBase
	{
		#region Private Members

        private string _template = "";
        private CatalogNavigation _catalogNav;
		private ProductInfo _product;
        private readonly CategoryController _categoryControler = new CategoryController();
        private CategoryInfo _category;
        private int _categoryID;
        private int _returnPage;
        private bool _cartWarning;
		private bool _showThumbnail = true;
		private int _thumbnailWidth = 90;
        private string _gifBgColor = "";
        private bool _enableImageCaching = true;
        private int _cacheImageDuration = 2;
		private bool _showReviews = true;
		private bool _showDetail = true;
        private int _detailID;
        private bool _inList;
        private decimal _defaultTaxRate;
        private bool _showTax;
        private string _templatePath = "";
        private string _imagesPath = "";
        private readonly string _currentCulture = CultureInfo.CurrentCulture.Name;
        private readonly NumberFormatInfo _localFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
        private enum MetaTags
        {
            Title = 1,
            Keywords,
            Description
        }
        private readonly Regex _regTokens = new Regex("\\[\\w+\\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        #endregion

		#region Properties

        public string TemplatePath
        {
            get { return _templatePath; }
            set { _templatePath = value; }
        }

        public string Template
        {
            get { return _template; }
            set { _template = value; }
        }
        
        public int CategoryID
		{
			get {return _categoryID;}
			set {_categoryID = value;}
		}

        public int ReturnPage
        {
            get { return _returnPage; }
            set { _returnPage = value; }
        }

        public bool CartWarning
		{
            get { return _cartWarning; }
            set { _cartWarning = value; }
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

        public bool ShowReviews
		{
            get { return _showReviews; }
            set { _showReviews = value; }
		}

        public bool ShowDetail
		{
            get { return _showDetail; }
            set { _showDetail = value; }
		}
		
		public int DetailID
		{
            get { return _detailID; }
            set { _detailID = value; }
		}

        public bool InList
        {
            get { return _inList; }
            set { _inList = value; }
        }

        public CDefault BasePage
        {
            get { return (CDefault)Page; }
        }

        #endregion

		#region Events

		protected void Page_Load(object sender, EventArgs e)
		{
	        _catalogNav = new CatalogNavigation(Request.QueryString);
            _imagesPath = _templatePath + "Images/";

            if (!string.IsNullOrEmpty(StoreSettings.CurrencySymbol))
                _localFormat.CurrencySymbol = StoreSettings.CurrencySymbol;

            ITaxProvider taxProvider = StoreController.GetTaxProvider(StoreSettings.TaxName);
            ITaxInfo taxInfo = taxProvider.GetDefautTaxRates(PortalId);
            _defaultTaxRate = taxInfo.DefaultTaxRate;
            _showTax = taxInfo.ShowTax;

			_product = (DataSource as ProductInfo);
			if (_product != null)
			{
                _category = _categoryControler.GetCategory(PortalId, _product.CategoryID);
                if (StoreSettings.SEOFeature)
                {
                    _catalogNav.Category = _category.SEOName;
                    if (InList == false)
                    {
                        BasePage.Title = SEO(Localization.GetString("DetailsSEOTitle", LocalResourceFile), MetaTags.Title);
                        BasePage.Description = SEO(Localization.GetString("DetailsSEODescription", LocalResourceFile), MetaTags.Description);
                        BasePage.KeyWords = SEO(Localization.GetString("DetailsSEOKeywords", LocalResourceFile), MetaTags.Keywords);
                        CatalogNavigation canonical = new CatalogNavigation();
                        canonical.CategoryID = _product.CategoryID;
                        canonical.ProductID = _product.ProductID;
                        string domain = Request.Url.GetLeftPart(UriPartial.Authority);
                        string url = canonical.GetNavigationUrl();
                        if (url.StartsWith(domain, true, CultureInfo.CurrentCulture) == false)
                            url = domain + url;
                        HeaderHelper.AddCanonicalLink(Page, url);
                    }
                }
                plhDetails.Controls.Add(TemplateController.ParseTemplate(MapPath(_templatePath), _template, Localization.GetString("TemplateError", LocalSharedResourceFile), IsLogged, new ProcessTokenDelegate(ProcessToken)));
            }

            // Clear error message
            lblError.Visible = false;

            // Show review panel ?
            if (ShowReviews && !InList)
            {
                int reviewID = _catalogNav.ReviewID;
			    // Show review list or edit?
                if (reviewID != Null.NullInteger)
                {
                    if (reviewID > 0 && !CanManageReviews())
                    {
                        _catalogNav.ReviewID = Null.NullInteger;
                        Response.Redirect(_catalogNav.GetNavigationUrl());
                    }
                    else
				        LoadReviewEditControl();
			    }
			    else
				    LoadReviewListControl();
                pnlReviews.Visible = true;
            }
            else
                pnlReviews.Visible = false;

            // Show Return link?
            if (InList == false)
            {
                if (_catalogNav.SearchID == Null.NullInteger)
                {
                    if (_catalogNav.CategoryID == Null.NullInteger)
                        lnkReturn.Text = Localization.GetString("lnkReturnToCatalog", LocalResourceFile);
                    else
                        lnkReturn.Text = Localization.GetString("lnkReturn", LocalResourceFile);
                }
                else
                    lnkReturn.Text = Localization.GetString("lnkReturnToSearch", LocalResourceFile);
                lnkReturn.NavigateUrl = GetReturnUrl(ReturnPage);
                pnlReturn.Visible = true;
            }
            else
                pnlReturn.Visible = false;
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            LinkButton button = (sender as LinkButton);
            if (button != null)
                AddToCart(int.Parse(button.CommandArgument), GetQuantity(("txtQuantity" + button.CommandArgument)), false);
        }

        protected void btnAddToCartImg_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton button = (sender as ImageButton);
            if (button != null)
                AddToCart(int.Parse(button.CommandArgument), GetQuantity(("txtQuantity" + button.CommandArgument)), false);
        }

        protected void btnPurchase_Click(object sender, EventArgs e)
        {
            LinkButton button = (sender as LinkButton);
            if (button != null)
                AddToCart(int.Parse(button.CommandArgument), GetQuantity(("txtQuantity" + button.CommandArgument)), true);
        }

        protected void btnPurchaseImg_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton button = (sender as ImageButton);
            if (button != null)
                AddToCart(int.Parse(button.CommandArgument), GetQuantity(("txtQuantity" + button.CommandArgument)), true);
        }

        protected void btnEdit_Click(object sender, ImageClickEventArgs e)
		{
            ImageButton button = (sender as ImageButton);
			if (button != null)
			{
                CatalogNavigation editNav = new CatalogNavigation(ModuleId, Request.QueryString);
                editNav.ProductID = int.Parse(button.CommandArgument);
                editNav.Edit = "Product";
                Response.Redirect(editNav.GetEditUrl());
			}
		}

        protected void btnLinkDetailImg_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton button = (sender as ImageButton);
            if (button != null)
            {
                _catalogNav.TabID = _detailID;
                _catalogNav.ProductID = int.Parse(button.CommandArgument);
                Response.Redirect(_catalogNav.GetNavigationUrl());
            }
        }

        protected void reviewList_EditComplete(object sender, EventArgs e)
		{
			Response.Redirect(_catalogNav.GetNavigationUrl());
		}

        protected void reviewEdit_EditComplete(object sender, EventArgs e)
		{
			_catalogNav.ReviewID = Null.NullInteger;
			Response.Redirect(_catalogNav.GetNavigationUrl());
		}

		#endregion

		#region Private Methods

        private int GetQuantity(string quantityID)
        {
            int quantity = 1;
            TextBox txtQuantity = (TextBox)FindControl(quantityID);
            if (txtQuantity != null && int.TryParse(txtQuantity.Text, out quantity) == false)
                    quantity = 1;
            return quantity;
        }

        private void AddToCart(int productID, int quantity, bool buyNow)
        {
            if (StoreSettings.InventoryManagement && StoreSettings.AvoidNegativeStock)
            {
                ProductController controler = new ProductController();
                ProductInfo currentProduct = controler.GetProduct(PortalId, productID);

                if (currentProduct.StockQuantity < quantity)
                {
                    lblError.Text = Localization.GetString("NotEnoughProducts", LocalResourceFile);
                    lblError.Visible = true;
                    return;
                }
            }
            CurrentCart.AddItem(PortalId, StoreSettings.SecureCookie, productID, quantity);
            if (buyNow)
            {
                _catalogNav = new CatalogNavigation();
                _catalogNav.TabID = StoreSettings.ShoppingCartPageID;
            }
            Response.Redirect(_catalogNav.GetNavigationUrl());
        }

        private string GetImagePath(string image)
        {
            // Get the template base file name
            string baseFileName = Localization.GetString(image, LocalResourceFile);
            // Build the localized filename with images path base and current culture
            string basePathName = _imagesPath + String.Format(baseFileName, _currentCulture);
            // If the localized file do not exists,
            // fallback to the default en-US version
            if (File.Exists(MapPath(basePathName)) == false)
                basePathName = _imagesPath + String.Format(baseFileName, "en-US");
            return basePathName;
        }

        private string GetReturnUrl(int returnTabId)
        {
            _catalogNav.ProductID = Null.NullInteger;
            _catalogNav.Product = Null.NullString;
            if (returnTabId == 0)
                _catalogNav.TabID = TabId;
            else
                _catalogNav.TabID = returnTabId;
            return _catalogNav.GetNavigationUrl();
        }

		private string GetImageUrl(string image) 
		{
            if (_enableImageCaching)
                return ModulePath + "Thumbnail.ashx?IP=" + image + "&IW=" + _thumbnailWidth + "&BC=" + GIFBgColor + "&CD=" + _cacheImageDuration;

            return ModulePath + "Thumbnail.ashx?IP=" + image + "&IW=" + _thumbnailWidth + "&BC=" + GIFBgColor;
		}

        private bool CanManageReviews()
        {
            if (UserInfo.IsSuperUser || UserInfo.IsInRole("Administrators"))
                return true;

            if (StoreSettings.CatalogRoleID != Null.NullInteger)
            {
                RoleController roleController = new RoleController();
                RoleInfo catalogRole = roleController.GetRole(StoreSettings.CatalogRoleID, PortalId);
                if (UserInfo.IsInRole(catalogRole.RoleName))
                    return true;
            }

            return false;
        }

		private void LoadReviewListControl()
		{
			// TODO: We may want to use caching here
			StoreControlBase reviewList = (StoreControlBase)LoadControl(ModulePath + "ReviewList.ascx");
            reviewList.ModuleConfiguration = ModuleConfiguration;
            reviewList.StoreSettings = StoreSettings;
			reviewList.EditComplete += reviewList_EditComplete;
			plhReviews.Controls.Clear();
			plhReviews.Controls.Add(reviewList);
		}

		private void LoadReviewEditControl()
		{
			// Inject the edit control
			StoreControlBase reviewEdit = (StoreControlBase)LoadControl(ModulePath + "ReviewEdit.ascx");
            reviewEdit.ModuleConfiguration = ModuleConfiguration;
            reviewEdit.StoreSettings = StoreSettings;
			reviewEdit.DataSource = _catalogNav.ReviewID;
			reviewEdit.EditComplete += reviewEdit_EditComplete;
			plhReviews.Controls.Clear();
			plhReviews.Controls.Add(reviewEdit);
		}

		private Control ProcessToken(string tokenName)
		{
			switch (tokenName.ToUpper())
			{
                case "DETAILTITLE":
                    if (!_inList)
                    {
                        Label lblTitle = new Label();
                        lblTitle.CssClass = "StoreDetailTitle";
                        lblTitle.Text = Localization.GetString("DetailTitle", LocalResourceFile);
                        return lblTitle;
                    }

                    return null;

                case "CARTWARNING":
                    if (!_inList && _cartWarning && CurrentCart.ProductIsInCart(PortalId, StoreSettings.SecureCookie, _product.ProductID))
                    {
                        Label lblWarningMessage = new Label();
                        lblWarningMessage.CssClass = "StoreDetailWarningMessage";
                        lblWarningMessage.Text = Localization.GetString("WarningMessage", LocalResourceFile);
                        return lblWarningMessage;
                    }

                    return null;

                case "IMAGE":
                    // No control in this case
                    if (!_showThumbnail || string.IsNullOrEmpty(_product.ProductImage))
                        return null;

                    Image imgProduct = new Image();
                    imgProduct.ImageUrl = GetImageUrl(_product.ProductImage);
                    imgProduct.AlternateText = Localization.GetString("ImageAlt.Text", LocalResourceFile);
                    imgProduct.CssClass = "StoreProductImage";

                    if (_inList & _showDetail)
                    {
                        StringDictionary rplcImage = new StringDictionary();
                        rplcImage.Add("ProductID", _product.ProductID.ToString());
                        if (StoreSettings.SEOFeature == true)
                            rplcImage.Add("Product", _product.SEOName);

                        HyperLink lnkImage = new HyperLink();
                        lnkImage.NavigateUrl = _catalogNav.GetNavigationUrl(_detailID, rplcImage);
                        lnkImage.ToolTip = SEO(Localization.GetString("LinkDetail", LocalResourceFile), MetaTags.Title);
                        lnkImage.CssClass = "StoreProductLinkImage";
                        lnkImage.Controls.Add(imgProduct);
                        return lnkImage;
                    }

                    imgProduct.ToolTip = SEO(Localization.GetString("ImageTitle.Text", LocalResourceFile), MetaTags.Title);
                    return imgProduct;

                case "IMAGEURL":
                    // No control in this case
                    if (string.IsNullOrEmpty(_product.ProductImage))
                        return null;

                    return new LiteralControl(GetImageUrl(_product.ProductImage));

                case "IMAGERAWURL":
                    // No control in this case
                    if (string.IsNullOrEmpty(_product.ProductImage))
                        return null;

                    return new LiteralControl(_product.ProductImage);

				case "EDIT":
                    if (IsEditable)
                    {
                        ImageButton imgEdit = new ImageButton();
                        imgEdit.CssClass = "StoreButtonEdit";
                        imgEdit.CommandArgument = _product.ProductID.ToString();
                        imgEdit.Click += btnEdit_Click;
                        imgEdit.ImageUrl = "~/images/Edit.gif";
                        imgEdit.AlternateText = Localization.GetString("Edit.Text", LocalResourceFile);
                        return imgEdit;
                    }

                    return null;

                case "CATEGORYNAME":
                    Label lblProductCategory = new Label();
                    lblProductCategory.Text = string.Format(Localization.GetString("CategoryName.Text", LocalResourceFile), _category.Name);
                    lblProductCategory.CssClass = "StoreCategoryName";
                    return lblProductCategory;

                case "MANUFACTURER":
                    Label lblManufacturer = new Label();
                    lblManufacturer.Text = string.Format(Localization.GetString("Manufacturer.Text", LocalResourceFile), _product.Manufacturer);
                    lblManufacturer.CssClass = "StoreProductManufacturer";
                    return lblManufacturer;

                case "MODELNUMBER":
                    Label lblModelNumber = new Label();
                    lblModelNumber.Text = string.Format(Localization.GetString("ModelNumber.Text", LocalResourceFile), _product.ModelNumber);
                    lblModelNumber.CssClass = "StoreProductModelNumber";
                    return lblModelNumber;

                case "MODELNAME":
                    Label lblModelName = new Label();
                    lblModelName.Text = string.Format(Localization.GetString("ModelName.Text", LocalResourceFile), _product.ModelName);
                    lblModelName.CssClass = "StoreProductModelName";
                    return lblModelName;

                case "TITLE":
                    if (_inList & _showDetail)
                    {
                        StringDictionary rplcTitle = new StringDictionary();
                        rplcTitle.Add("ProductID", _product.ProductID.ToString());
                        if (StoreSettings.SEOFeature)
                            rplcTitle.Add("Product", _product.SEOName);

                        HyperLink lnkTitle = new HyperLink();
                        lnkTitle.Text = _product.ProductTitle;
                        lnkTitle.NavigateUrl = _catalogNav.GetNavigationUrl(_detailID, rplcTitle);
                        lnkTitle.CssClass = "CommandButton StoreProductLinkTitle";
                        return lnkTitle;
                    }

                    Label lblProductTitle = new Label();
                    lblProductTitle.Text = _product.ProductTitle;
                    lblProductTitle.CssClass = "StoreProductTitle";
                    return lblProductTitle;

                case "LINKDETAIL":
                    if (_inList & _showDetail)
                    {
                        StringDictionary rplcLink = new StringDictionary();
                        rplcLink.Add("ProductID", _product.ProductID.ToString());
                        if (StoreSettings.SEOFeature)
                            rplcLink.Add("Product", _product.SEOName);

                        HyperLink lnkDetail = new HyperLink();
                        lnkDetail.Text = Localization.GetString("LinkDetail", LocalResourceFile);
                        lnkDetail.NavigateUrl = _catalogNav.GetNavigationUrl(_detailID, rplcLink);
                        lnkDetail.CssClass = "CommandButton StoreLinkDetail";
                        return lnkDetail;
                    }

                    return null;

                case "PRINTDETAIL":
                    if (!_inList)
                    {
                        string url = _catalogNav.GetNavigationUrl() + "&mid=" + ModuleId + "&SkinSrc=" + Globals.QueryStringEncode("[G]" + SkinInfo.RootSkin + "/" + Globals.glbHostSkinFolder + "/" + "No Skin") + "&ContainerSrc=" + Globals.QueryStringEncode("[G]" + SkinInfo.RootContainer + "/" + Globals.glbHostSkinFolder + "/" + "No Container");

                        HyperLink lnkPrintDetail = new HyperLink();
                        lnkPrintDetail.Text = Localization.GetString("PrintDetail", LocalResourceFile);
                        lnkPrintDetail.NavigateUrl = url;
                        lnkPrintDetail.Target = "_blank";
                        lnkPrintDetail.CssClass = "CommandButton StorePrintDetail";
                        return lnkPrintDetail;
                    }

                    return null;

                case "LINKDETAILIMG":
                    if (_inList & _showDetail)
                    {
                        ImageButton btnLinkDetailImg = new ImageButton();
                        btnLinkDetailImg.ImageUrl = GetImagePath("LinkDetailImg");
                        btnLinkDetailImg.ToolTip = Localization.GetString("LinkDetail", LocalResourceFile);
                        btnLinkDetailImg.CommandArgument = _product.ProductID.ToString();
                        btnLinkDetailImg.Click += btnLinkDetailImg_Click;
                        btnLinkDetailImg.CssClass = "StoreLinkDetailImg";
                        return btnLinkDetailImg;
                    }

                    return null;

                case "SUMMARY":
					Label lblSummary = new Label();
                    lblSummary.Text = string.Format(Localization.GetString("Summary.Text", LocalResourceFile), _product.Summary);
                    lblSummary.CssClass = "StoreProductSummary";
					return lblSummary;

                case "WEIGHT":
                    Label lblWeight = new Label();
                    lblWeight.Text = string.Format(Localization.GetString("WeightText", LocalResourceFile), _product.ProductWeight.ToString(Localization.GetString("WeightFormat", LocalResourceFile), _localFormat));
                    lblWeight.CssClass = "StoreProductWeight";
                    return lblWeight;

                case "HEIGHT":
                    Label lblHeight = new Label();
                    lblHeight.Text = string.Format(Localization.GetString("HeightText", LocalResourceFile), _product.ProductHeight.ToString(Localization.GetString("HeightFormat", LocalResourceFile), _localFormat));
                    lblHeight.CssClass = "StoreProductHeight";
                    return lblHeight;

                case "LENGTH":
                    Label lblLength = new Label();
                    lblLength.Text = string.Format(Localization.GetString("LengthText", LocalResourceFile), _product.ProductLength.ToString(Localization.GetString("LengthFormat", LocalResourceFile), _localFormat));
                    lblLength.CssClass = "StoreProductLength";
                    return lblLength;

                case "WIDTH":
                    Label lblWidth = new Label();
                    lblWidth.Text = string.Format(Localization.GetString("WidthText", LocalResourceFile), _product.ProductWidth.ToString(Localization.GetString("WidthFormat", LocalResourceFile), _localFormat));
                    lblWidth.CssClass = "StoreProductWidth";
                    return lblWidth;

                case "SURFACE":
                    Label lblSurface = new Label();
                    decimal dblSurface = _product.ProductLength * _product.ProductWidth;
                    lblSurface.Text = string.Format(Localization.GetString("SurfaceText", LocalResourceFile), dblSurface.ToString(Localization.GetString("SurfaceFormat", LocalResourceFile), _localFormat));
                    lblSurface.CssClass = "StoreProductSurface";
                    return lblSurface;

                case "VOLUME":
                    Label lblVolume = new Label();
                    decimal dblVolume = _product.ProductHeight * _product.ProductLength * _product.ProductWidth;
                    lblVolume.Text = string.Format(Localization.GetString("VolumeText", LocalResourceFile), dblVolume.ToString(Localization.GetString("VolumeFormat", LocalResourceFile), _localFormat));
                    lblVolume.CssClass = "StoreProductVolume";
                    return lblVolume;

                case "DIMENSIONS":
                    Label lblDimensions = new Label();
                    string strHeight = _product.ProductHeight.ToString(Localization.GetString("HeightFormat", LocalResourceFile), _localFormat);
                    string strLength = _product.ProductLength.ToString(Localization.GetString("LengthFormat", LocalResourceFile), _localFormat);
                    string strWidth = _product.ProductWidth.ToString(Localization.GetString("WidthFormat", LocalResourceFile), _localFormat);
                    lblDimensions.Text = string.Format(Localization.GetString("DimensionsText", LocalResourceFile), strHeight, strLength, strWidth);
                    lblDimensions.CssClass = "StoreProductDimensions";
                    return lblDimensions;

                case "PRICE":
                    Label lblPrice = new Label();
                    string formatedPrice = string.Format(Localization.GetString("Price", LocalResourceFile), _product.UnitCost.ToString("C", _localFormat));
                    if (_product.Featured && DateTime.Now > _product.SaleStartDate && DateTime.Now < _product.SaleEndDate)
                    {
                        //Product is on sale...
                        string formatedSalePrice = string.Format(Localization.GetString("Price", LocalResourceFile), _product.SalePrice.ToString("C", _localFormat));
                        lblPrice.Text = string.Format(Localization.GetString("SpecialOffer", LocalResourceFile), formatedPrice, formatedSalePrice);
                        lblPrice.CssClass = "StoreProductPrice StoreProductSpecialOffer";
                    }
                    else
                    {
                        lblPrice.Text = formatedPrice;
                        lblPrice.CssClass = "StoreProductPrice";
                    }
                    return lblPrice;

				case "VATPRICE":
                    if (_showTax)
                    {
                        Label lblVATPrice = new Label();
                        decimal dblVATPrice = (_product.UnitCost + (_product.UnitCost * (_defaultTaxRate / 100)));
                        string formatedVATPrice = string.Format(Localization.GetString("VATPrice", LocalResourceFile), dblVATPrice.ToString("C", _localFormat));
                        if (_product.Featured && DateTime.Now > _product.SaleStartDate && DateTime.Now < _product.SaleEndDate)
                        {
                            //Product is on sale...
                            decimal dblVATSalePrice = (_product.SalePrice + (_product.SalePrice * (_defaultTaxRate / 100)));
                            string formatedVATSalePrice = string.Format(Localization.GetString("VATPrice", LocalResourceFile), dblVATSalePrice.ToString("C", _localFormat));
                            lblVATPrice.Text = string.Format(Localization.GetString("SpecialOffer", LocalResourceFile), formatedVATPrice, formatedVATSalePrice);
                            lblVATPrice.CssClass = "StoreProductVATPrice StoreProductSpecialOffer";
                        }
                        else
                        {
                            lblVATPrice.Text = formatedVATPrice;
                            lblVATPrice.CssClass = "StoreProductVATPrice";
                        }
                        lblVATPrice.CssClass = "StoreProductVATPrice";
                        return lblVATPrice;
                    }

                    return null;

                case "REGULARPRICE":
                    Label lblRegularPrice = new Label();
                    lblRegularPrice.Text = string.Format(Localization.GetString("RegularPrice", LocalResourceFile), _product.RegularPrice.ToString("C", _localFormat));
                    lblRegularPrice.CssClass = "StoreProductRegularPrice";
                    return lblRegularPrice;

                case "REGULARVATPRICE":
                    if (_showTax)
                    {
                        Label lblRegularVATPrice = new Label();
                        decimal dblRegularVATPrice = (_product.RegularPrice + (_product.RegularPrice * (_defaultTaxRate / 100)));
                        lblRegularVATPrice.Text = string.Format(Localization.GetString("RegularVATPrice", LocalResourceFile), dblRegularVATPrice.ToString("C", _localFormat));
                        lblRegularVATPrice.CssClass = "StoreProductRegularVATPrice";
                        return lblRegularVATPrice;
                    }

                    return null;

                case "PURCHASE":
                    // Hide control in this case
                    if (StoreSettings.InventoryManagement && StoreSettings.ProductsBehavior == (int)Behavior.HideControls && _product.StockQuantity < 1)
                        return null;

                    LinkButton btnPurchase = new LinkButton();
                    btnPurchase.Text = Localization.GetString("Purchase", LocalResourceFile);
                    btnPurchase.CommandArgument = _product.ProductID.ToString();
                    btnPurchase.Click += btnPurchase_Click;
                    btnPurchase.CssClass = "CommandButton StoreButtonPurchase";
                    return btnPurchase;

                case "PURCHASEIMG":
                    // Hide control in this case
                    if (StoreSettings.InventoryManagement && StoreSettings.ProductsBehavior == (int)Behavior.HideControls & _product.StockQuantity < 1)
                        return null;

                    ImageButton btnPurchaseImg = new ImageButton();
                    btnPurchaseImg.ImageUrl = GetImagePath("PurchaseImg");
                    btnPurchaseImg.ToolTip = Localization.GetString("Purchase", LocalResourceFile);
                    btnPurchaseImg.CommandArgument = _product.ProductID.ToString();
                    btnPurchaseImg.Click += btnPurchaseImg_Click;
                    btnPurchaseImg.CssClass = "StoreButtonPurchaseImg";
                    return btnPurchaseImg;

                case "ADDTOCART":
                    // Hide control in this case
                    if (StoreSettings.InventoryManagement && StoreSettings.ProductsBehavior == (int)Behavior.HideControls & _product.StockQuantity < 1)
                        return null;

                    LinkButton btnAddToCart = new LinkButton();
                    btnAddToCart.Text = Localization.GetString("AddToCart", LocalResourceFile);
                    btnAddToCart.CommandArgument = _product.ProductID.ToString();
                    btnAddToCart.Click += btnAddToCart_Click;
                    btnAddToCart.CssClass = "CommandButton StoreButtonAddToCart";
                    return btnAddToCart;

                case "ADDTOCARTIMG":
                    // Hide control in this case
                    if (StoreSettings.InventoryManagement && StoreSettings.ProductsBehavior == (int)Behavior.HideControls & _product.StockQuantity < 1)
                        return null;

                    ImageButton btnAddToCartImg = new ImageButton();
                    btnAddToCartImg.ImageUrl = GetImagePath("AddToCartImg");
                    btnAddToCartImg.ToolTip = Localization.GetString("AddToCart", LocalResourceFile);
                    btnAddToCartImg.CommandArgument = _product.ProductID.ToString();
                    btnAddToCartImg.Click += btnAddToCartImg_Click;
                    btnAddToCartImg.CssClass = "StoreButtonAddToCartImg";
                    return btnAddToCartImg;

                case "ADDQUANTITY":
                    // Hide control in this case
                    if (StoreSettings.InventoryManagement && StoreSettings.ProductsBehavior == (int)Behavior.HideControls & _product.StockQuantity < 1)
                        return null;

                    Label lblQuantity = new Label();
                    LiteralControl litQuantity = new LiteralControl(Localization.GetString("Quantity", LocalResourceFile));
                    TextBox txtAddToCartQty = new TextBox();
                    txtAddToCartQty.ID = "txtQuantity" + _product.ProductID;
                    txtAddToCartQty.CssClass = "StoreQuantityTextBox";
                    txtAddToCartQty.Text = "1";
                    lblQuantity.Controls.Add(litQuantity);
                    lblQuantity.Controls.Add(txtAddToCartQty);
                    lblQuantity.CssClass = "StoreAddQuantity";
                    return lblQuantity;

                case "DESCRIPTION":
                    Panel pnlDescription = new Panel();
                    pnlDescription.CssClass = "StoreProductDescription";
                    Literal litDescription = new Literal();
                    litDescription.Text = System.Web.HttpUtility.HtmlDecode(_product.Description);
                    pnlDescription.Controls.Add(litDescription);
                    return pnlDescription;

                case "LOCALE":
                    return new LiteralControl(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());

                case "TEMPLATESBASEURL":
                    return new LiteralControl(_templatePath);

                case "IMAGESBASEURL":
                    return new LiteralControl(_imagesPath);

                case "PRODUCTDETAILURL":
                    if (_showDetail)
                    {
                        StringDictionary urlLink = new StringDictionary();
                        urlLink.Add("ProductID", _product.ProductID.ToString());
                        if (StoreSettings.SEOFeature)
                            urlLink.Add("Product", _product.SEOName);
                        return new LiteralControl(_catalogNav.GetNavigationUrl(_detailID, urlLink));
                    }

                    return null;

                case "STOCKQUANTITY":
                    if (StoreSettings.InventoryManagement)
                    {
                        Label lblStockQuantity = new Label();
                        if (_product.StockQuantity > 0)
                            lblStockQuantity.Text = string.Format(Localization.GetString("StockQuantity", LocalResourceFile), _product.StockQuantity);
                        else
                        {
                            switch (StoreSettings.OutOfStock)
                            {
                                case (int)StockMessage.Quantity:
                                    lblStockQuantity.Text = string.Format(Localization.GetString("StockQuantity", LocalResourceFile), _product.StockQuantity);
                                    break;
                                case (int)StockMessage.Unavailable:
                                    lblStockQuantity.Text = Localization.GetString("OOStockUnavailable", LocalResourceFile);
                                    break;
                                case (int)StockMessage.Restocking:
                                    lblStockQuantity.Text = Localization.GetString("OOStockRestocking", LocalResourceFile);
                                    break;
                            }
                        }
                        lblStockQuantity.CssClass = "StoreProductStockQuantity";
                        return lblStockQuantity;
                    }

                    return null;

                case "TELLAFRIEND":
                    if (!_inList)
                    {
                        HyperLink btnTellAFriend = new HyperLink();
                        btnTellAFriend.Text = Localization.GetString("TellAFriend", LocalResourceFile);
                        string subject = Localization.GetString("TellAFriendSubject", LocalResourceFile);
                        string body = Localization.GetString("TellAFriendBody", LocalResourceFile);
                        CatalogTokenReplace tkCatalog = new CatalogTokenReplace();
                        _category.StorePageID = TabId;
                        tkCatalog.Category = _category;
                        _product.StorePageID = TabId;
                        tkCatalog.Product = _product;
                        tkCatalog.StoreSettings = StoreSettings;
                        subject = tkCatalog.ReplaceCatalogTokens(subject);
                        body = tkCatalog.ReplaceCatalogTokens(body);
                        btnTellAFriend.NavigateUrl = string.Format(Localization.GetString("TellAFriendHRef", LocalResourceFile), subject, System.Web.HttpUtility.HtmlEncode(System.Web.HttpUtility.UrlPathEncode(body)));
                        btnTellAFriend.CssClass = "CommandButton StoreButtonTellAFriend";
                        return btnTellAFriend;
                    }

                    return null;

                case "TELLAFRIENDIMG":
                    if (!_inList)
                    {
                        HyperLink btnTellAFriendImg = new HyperLink();
                        btnTellAFriendImg.Text = Localization.GetString("TellAFriend", LocalResourceFile);
                        string subject = Localization.GetString("TellAFriendSubject", LocalResourceFile);
                        string body = Localization.GetString("TellAFriendBody", LocalResourceFile);
                        CatalogTokenReplace tkCatalog = new CatalogTokenReplace();
                        _category.StorePageID = TabId;
                        tkCatalog.Category = _category;
                        _product.StorePageID = TabId;
                        tkCatalog.Product = _product;
                        tkCatalog.StoreSettings = StoreSettings;
                        subject = tkCatalog.ReplaceCatalogTokens(subject);
                        body = tkCatalog.ReplaceCatalogTokens(body);
                        btnTellAFriendImg.NavigateUrl = string.Format(Localization.GetString("TellAFriendHRef", LocalResourceFile), subject, System.Web.HttpUtility.HtmlEncode(System.Web.HttpUtility.UrlPathEncode(body)));
                        btnTellAFriendImg.CssClass = "CommandButton StoreButtonTellAFriend";
                        HtmlImage imgButton = new HtmlImage();
                        imgButton.Src = GetImagePath("TellAFriendImg");
                        imgButton.Alt = Localization.GetString("TellAFriend", this.LocalResourceFile);
                        btnTellAFriendImg.Controls.Add(imgButton);
                        return btnTellAFriendImg;
                    }

                    return null;

				default:
					LiteralControl litText = new LiteralControl(tokenName);
					return litText;
			}
		}

        private string SEO(string seoResource, MetaTags metaType)
        {
            string tempResource = seoResource;
            string tempValue = "";
            MatchCollection matchCol = _regTokens.Matches(tempResource);

            if (matchCol.Count > 0)
            {
                foreach (Match match in matchCol)
                {
                    switch (metaType)
                    {
                        case MetaTags.Title:
                            switch (match.Value.ToUpper())
                            {
                                case "[PAGETITLE]":
                                    tempValue = BasePage.Title;
                                    break;
                                case "[STORETITLE]":
                                    tempValue = StoreSettings.Name;
                                    break;
                                case "[MANUFACTURER]":
                                    tempValue = _product.Manufacturer;
                                    break;
                                case "[MODELNUMBER]":
                                    tempValue = _product.ModelNumber;
                                    break;
                                case "[MODELNAME]":
                                    tempValue = _product.ModelName;
                                    break;
                                case "[PRODUCTTITLE]":
                                    tempValue = _product.ProductTitle;
                                    break;
                                case "[PRODUCTSUMMARY]":
                                    tempValue = _product.Summary;
                                    break;
                                default:
                                    tempValue = match.Value;
                                    break;
                            }
                            break;
                        case MetaTags.Keywords:
                            switch (match.Value.ToUpper())
                            {
                                case "[PAGEKEYWORDS]":
                                    tempValue = BasePage.KeyWords;
                                    break;
                                case "[STOREKEYWORDS]":
                                    tempValue = StoreSettings.Keywords;
                                    break;
                                case "[PRODUCTKEYWORDS]":
                                    tempValue = _product.Keywords;
                                    break;
                                default:
                                    tempValue = match.Value;
                                    break;
                            }
                            break;
                        case MetaTags.Description:
                            switch (match.Value.ToUpper())
                            {
                                case "[PAGEDESCRIPTION]":
                                    tempValue = BasePage.Description;
                                    break;
                                case "[STOREDESCRIPTION]":
                                    tempValue = StoreSettings.Description;
                                    break;
                                case "[PRODUCTSUMMARY]":
                                    tempValue = _product.Summary;
                                    break;
                                case "[PRODUCTDESCRIPTION]":
                                    tempValue = _product.Description;
                                    break;
                                default:
                                    tempValue = match.Value;
                                    break;
                            }
                            break;
                    }
                    tempResource = tempResource.Replace(match.Value, tempValue);
                }
            }
            return tempResource.Trim();
        }

		#endregion
	}
}
