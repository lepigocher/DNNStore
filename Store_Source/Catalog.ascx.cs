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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Framework;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Security;
using DotNetNuke.UI.Skins;

using DotNetNuke.Modules.Store.Core.Admin;
using DotNetNuke.Modules.Store.Core.Catalog;
using DotNetNuke.Modules.Store.Core.Components;

namespace DotNetNuke.Modules.Store.WebControls
{
    public enum ProductListTypes
    {
        New,
        Featured,
        Popular,
        Category,
        SearchResults,
        AlsoBought
    }

    /// <summary>
    /// Summary description for Catalog
    /// </summary>
    public partial class Catalog : StoreControlBase, IActionable
    {
        #region Private Members

        private ModuleSettings _moduleSettings;
        private CatalogNavigation _catalogNav;
        private string _templatePath;
        private readonly CategoryController _categoryControler = new CategoryController();
        private readonly ProductController _productController = new ProductController();
        private CategoryInfo _currentCategory;
        private ProductInfo _currentProduct;
        private enum MetaTags
        {
            Title = 1,
            Keywords,
            Description,
        }
        private readonly Regex _regTokens = new Regex("\\[\\w+\\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
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

        public Int32 CategoryID
        {
            get { return _catalogNav.CategoryID; }
        }

        public Int32 ProductID
        {
            get { return _catalogNav.ProductID; }
        }

        public CDefault BasePage
        {
            get { return (CDefault)Page; }
        }

        #endregion

        #region Events

        override protected void OnInit(EventArgs e)
        {
            if (StoreSettings != null)
            {
                _moduleSettings = new ModuleSettings(ModuleId, TabId);
                _catalogNav = new CatalogNavigation(Request.QueryString);
                if (_catalogNav.CategoryID == Null.NullInteger && _catalogNav.SearchID == Null.NullInteger && _catalogNav.ProductID == Null.NullInteger)
                {
                    if (_moduleSettings.General.UseDefaultCategory && _moduleSettings.General.DefaultCategoryID != Null.NullInteger)
                    {
                        _catalogNav.CategoryID = _moduleSettings.General.DefaultCategoryID;
                        if (StoreSettings.SEOFeature)
                        {
                            _currentCategory = _categoryControler.GetCategory(PortalId, _catalogNav.CategoryID);
                            if (_currentCategory != null)
                                _catalogNav.Category = _currentCategory.SEOName;
                        }
                        Response.Clear();
                        Response.StatusCode = 301;
                        Response.Status = "301 Moved Permanently";
                        Response.RedirectLocation = _catalogNav.GetNavigationUrl();
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (StoreSettings != null)
            {
                try
                {
                    if (_catalogNav.ProductID != Null.NullInteger)
                    {
                        _currentProduct = _productController.GetProduct(PortalId, _catalogNav.ProductID);
                        _catalogNav.CategoryID = _currentProduct.CategoryID;
                    }

                    if (_catalogNav.CategoryID != Null.NullInteger)
                        _currentCategory = _categoryControler.GetCategory(PortalId, _catalogNav.CategoryID);

                    if (StoreSettings.SEOFeature)
                    {
                        BasePage.Title = SEO(Localization.GetString("ListSEOTitle", LocalResourceFile), MetaTags.Title);
                        BasePage.Description = SEO(Localization.GetString("ListSEODescription", LocalResourceFile), MetaTags.Description);
                        BasePage.KeyWords = SEO(Localization.GetString("ListSEOKeywords", LocalResourceFile), MetaTags.Keywords);
                        if (_catalogNav.ProductID == Null.NullInteger)
                        {
                            CatalogNavigation canonical = new CatalogNavigation();
                            if (_catalogNav.CategoryID != Null.NullInteger)
                                canonical.CategoryID = _catalogNav.CategoryID;
                            if (_catalogNav.SearchID != Null.NullInteger)
                            {
                                canonical.SearchID = _catalogNav.SearchID;
                                canonical.SearchValue = _catalogNav.SearchValue;
                            }
                            if (_catalogNav.PageIndex != Null.NullInteger)
                                canonical.PageIndex = _catalogNav.PageIndex;
                            else
                                canonical.PageIndex = 1;
                            if (_catalogNav.SortID != Null.NullInteger)
                                canonical.SortID = _catalogNav.SortID;
                            else
                                canonical.SortID = _moduleSettings.SortProducts.SortBy;
                            if (_catalogNav.SortDir != Null.NullString)
                                canonical.SortDir = _catalogNav.SortDir;
                            else
                                canonical.SortDir = _moduleSettings.SortProducts.SortDir;
                            string domain = Request.Url.GetLeftPart(UriPartial.Authority);
                            string url = canonical.GetNavigationUrl();
                            if (url.StartsWith(domain, true, System.Globalization.CultureInfo.CurrentCulture) == false)
                                url = domain + url;
                            HeaderHelper.AddCanonicalLink(Page, url);
                        }
                    }

                    _templatePath = CssTools.GetTemplatePath(this, StoreSettings.PortalTemplates);
                    CssTools.AddCss(Page, _templatePath, StoreSettings.StyleSheet);

                    Controls.Add(TemplateController.ParseTemplate(MapPath(_templatePath), _moduleSettings.General.Template, Localization.GetString("TemplateError", LocalSharedResourceFile), UserId != Null.NullInteger, new ProcessTokenDelegate(ProcessToken)));
                }
                catch (Exception ex)
                {
                    Exceptions.ProcessModuleLoadException(this, ex);
                }
            }
            else
            {
                if (UserInfo.IsSuperUser)
                {
                    string errorSettings = Localization.GetString("ErrorSettings", LocalResourceFile);
                    string errorSettingsHeading = Localization.GetString("ErrorSettingsHeading", LocalResourceFile);
                    Skin.AddModuleMessage(this, errorSettingsHeading, errorSettings, UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning);
                }
            }
        }

        void lbSearchButton_Click(object sender, EventArgs e)
        {
            string searchValue = Null.NullString;
            string searchColumn = Null.NullString;

            // Try to find the txtSearchValue control in the DOM
            TextBox txtSearchValue = (TextBox)FindControl("txtSearchValue");
            if (txtSearchValue != null)
            {
                // The control has been found, then get the text to search
                searchValue = txtSearchValue.Text;
                // If the search box is empty redirect to category
                if (string.IsNullOrEmpty(searchValue))
                {
                    if (_currentCategory == null)
                    {
                        if (_moduleSettings.General.UseDefaultCategory && _moduleSettings.General.DefaultCategoryID != Null.NullInteger)
                            _currentCategory = _categoryControler.GetCategory(PortalId, _moduleSettings.General.DefaultCategoryID);
                    }
                    // Create a new CatalogNavigation object
                    CatalogNavigation searchNav = new CatalogNavigation();
                    if (_currentCategory != null)
                    {
                        searchNav.CategoryID = _currentCategory.CategoryID;
                        if (StoreSettings.SEOFeature)
                            searchNav.Category = _currentCategory.SEOName;
                    }
                    // Redirect to the same page with category in the URL
                    Response.Redirect(searchNav.GetNavigationUrl(), true);
                }
            }
            // Try to find the ddlSearchColumn control in the DOM
            DropDownList ddlSearchColumn = (DropDownList)FindControl("ddlSearchColumn");
            if (ddlSearchColumn != null)
            {
                // The control has been found, then get the column number
                searchColumn = ddlSearchColumn.SelectedValue;
            }
            // If both controls has been found and values retreived
            if (searchValue != Null.NullString && searchColumn != Null.NullString)
            {
                // Create a new CatalogNavigation object
                CatalogNavigation searchNav = new CatalogNavigation
                {
                    // Define the search properties
                    SearchID = int.Parse(searchColumn),
                    SearchValue = HttpUtility.UrlEncode(searchValue)
                };
                // Redirect to the same page with search params in the URL
                Response.Redirect(searchNav.GetNavigationUrl(), true);
            }
        }

        #endregion

        #region Private Methods

        private Control ProcessToken(string tokenName)
        {
            switch (tokenName.ToUpper())
            {
                case "SEARCH":
                    if (_moduleSettings.General.ShowCategoryProducts && _moduleSettings.SearchProducts.SearchColumns != 0 && _catalogNav.ProductID == Null.NullInteger)
                    {
                        // Create a panel to contains all other controls
                        Panel pnlSearch = new Panel
                        {
                            CssClass = "StoreSearch"
                        };
                        // Create a literal text with locale resource
                        Literal litSearchValue = new Literal
                        {
                            Text = Localization.GetString("SearchValue", LocalResourceFile)
                        };
                        // Add control to the search container
                        pnlSearch.Controls.Add(litSearchValue);
                        // Create a textbox to receive the text to search
                        TextBox txtSearchValue = new TextBox
                        {
                            ID = "txtSearchValue",
                            CssClass = "StoreSearchTextBox"
                        };
                        // Define the current search value
                        if (_catalogNav.SearchValue != Null.NullString)
                            txtSearchValue.Text = HttpUtility.UrlDecode(_catalogNav.SearchValue);
                        // Add control to the search container
                        pnlSearch.Controls.Add(txtSearchValue);
                        // Create a literal text with locale resource
                        Literal litSearchInside = new Literal
                        {
                            Text = Localization.GetString("SearchInside", LocalResourceFile)
                        };
                        // Add control to the search container
                        pnlSearch.Controls.Add(litSearchInside);
                        // Create a DropDownList with column names
                        DropDownList ddlSearchColumn = new DropDownList
                        {
                            CssClass = "StoreSearchColumns",
                            ID = "ddlSearchColumn",
                            // We don't want to use AutoPostBack feature
                            // because the 'Go' button is used to run the search
                            AutoPostBack = false
                        };
                        // Add column names to the dropdown list
                        int searchColumns = _moduleSettings.SearchProducts.SearchColumns;
                        if ((searchColumns & (int)SearchColumn.Manufacturer) != 0)
                            ddlSearchColumn.Items.Add(new ListItem(Localization.GetString("SearchManufacturer", LocalResourceFile), "0"));
                        if ((searchColumns & (int)SearchColumn.ModelNumber) != 0)
                            ddlSearchColumn.Items.Add(new ListItem(Localization.GetString("SearchModelNumber", LocalResourceFile), "1"));
                        if ((searchColumns & (int)SearchColumn.ModelName) != 0)
                            ddlSearchColumn.Items.Add(new ListItem(Localization.GetString("SearchModelName", LocalResourceFile), "2"));
                        if ((searchColumns & (int)SearchColumn.ProductSummay) != 0)
                            ddlSearchColumn.Items.Add(new ListItem(Localization.GetString("SearchSummary", LocalResourceFile), "3"));
                        if ((searchColumns & (int)SearchColumn.ProductDescription) != 0)
                            ddlSearchColumn.Items.Add(new ListItem(Localization.GetString("SearchDescription", LocalResourceFile), "4"));
                        // Define the default search column
                        string searchColumn;
                        if (_catalogNav.SearchID != Null.NullInteger)
                        {
                            // Currently selected search column
                            searchColumn = _catalogNav.SearchID.ToString();
                        }
                        else
                        {
                            // Default search column
                            searchColumn = _moduleSettings.SearchProducts.SearchBy.ToString();
                        }
                        ListItem itemColumn = ddlSearchColumn.Items.FindByValue(searchColumn);
                        itemColumn.Selected = true;
                        // Add control to the search container
                        pnlSearch.Controls.Add(ddlSearchColumn);
                        // Create a LinkButton with locale resource
                        LinkButton lbSearchButton = new LinkButton
                        {
                            ID = "lbSearchButton",
                            Text = Localization.GetString("SearchButton", LocalResourceFile),
                            CssClass = "StoreSearchLinkButton"
                        };
                        // Workaround for FireFox to make the default button works!
                        Button btnHiddenButton = new Button
                        {
                            ID = "btnHiddenButton",
                            CssClass = "StoreSearchHiddenButton"
                        };
                        // Bind the button click event to your event handler
                        lbSearchButton.Click += lbSearchButton_Click;
                        btnHiddenButton.Click += lbSearchButton_Click;
                        // Add control to the search container
                        pnlSearch.Controls.Add(lbSearchButton);
                        pnlSearch.Controls.Add(btnHiddenButton);
                        // Define the hidden search button as default button
                        pnlSearch.DefaultButton = btnHiddenButton.ID;
                        // Return the container control
                        return pnlSearch;
                    }

                    return null;

                case "SEARCHRESULTS":
                    if (_moduleSettings.General.ShowCategoryProducts && _catalogNav.SearchID != Null.NullInteger && _catalogNav.SearchValue != Null.NullString && _catalogNav.ProductID == Null.NullInteger)
                    {
                        List<ProductInfo> productList = GetSearchedProducts(_catalogNav.SearchID, HttpUtility.UrlDecode(_catalogNav.SearchValue));
                        return LoadProductList(productList, ProductListTypes.SearchResults);
                    }

                    return null;

                case "MESSAGE":
                    if (_moduleSettings.General.ShowMessage && _currentCategory != null && _catalogNav.SearchID == Null.NullInteger && _catalogNav.ProductID == Null.NullInteger)
                    {
                        // Create Message panel
                        Panel pnlMessage = new Panel
                        {
                            CssClass = "StoreMessage"
                        };
                        // Display category message
                        string message = Server.HtmlDecode(_currentCategory.Message);
                        if (message != Null.NullString)
                        {
                            Panel pnlCategoryMessage = new Panel
                            {
                                CssClass = "StoreCategoryMessage"
                            };
                            Literal litCategoryMessage = new Literal
                            {
                                Text = HttpUtility.HtmlDecode(message)
                            };
                            pnlCategoryMessage.Controls.Add(litCategoryMessage);
                            pnlMessage.Controls.Add(pnlCategoryMessage);
                        }

                        // Display sub categories if any
                        CategoryController controller = new CategoryController();
                        List<CategoryInfo> subCategories = controller.GetCategories(PortalId, false, _currentCategory.CategoryID);
                        if (subCategories.Count > 0)
                        {
                            HtmlGenericControl pSubCategories = new HtmlGenericControl("p");
                            pSubCategories.Attributes.Add("class", "StoreSubCategories");
                            LiteralControl ends = new LiteralControl(Localization.GetString("BeforeSubCategories", LocalResourceFile));
                            pSubCategories.Controls.Add(ends);

                            CatalogNavigation categoryNav = new CatalogNavigation();
                            Boolean bIsFirst = true;
                            foreach (CategoryInfo category in subCategories)
                            {
                                if (bIsFirst == false)
                                {
                                    LiteralControl separator = new LiteralControl(Localization.GetString("BetweenSubCategories", LocalResourceFile));
                                    pSubCategories.Controls.Add(separator);
                                }
                                else
                                    bIsFirst = false;

                                HyperLink link = new HyperLink
                                {
                                    Text = category.Name,
                                    CssClass = "StoreSubCategoryItem"
                                };
                                categoryNav.CategoryID = category.CategoryID;
                                if (StoreSettings.SEOFeature)
                                    categoryNav.Category = category.SEOName;
                                link.NavigateUrl = categoryNav.GetNavigationUrl();
                                pSubCategories.Controls.Add(link);
                            }
                            ends = new LiteralControl(Localization.GetString("AfterSubCategories", LocalResourceFile));
                            pSubCategories.Controls.Add(ends);
                            pnlMessage.Controls.Add(pSubCategories);
                        }
                        return pnlMessage;
                    }

                    return null;

                case "NEW":
                    if (_moduleSettings.General.ShowNewProducts)
                    {
                        List<ProductInfo> productList;
                        if (_catalogNav.CategoryID != Null.NullInteger && _moduleSettings.NewProducts.DisplayByCategory)
                            productList = GetNewProducts(_catalogNav.CategoryID);
                        else
                            productList = GetPortalNewProducts(PortalId);
                        productList = TruncateList(productList, _moduleSettings.NewProducts.RowCount * _moduleSettings.NewProducts.ColumnCount);
                        return LoadProductList(productList, ProductListTypes.New);
                    }

                    return null;

                case "FEATURED":
                    if (_moduleSettings.General.ShowFeaturedProducts)
                    {
                        List<ProductInfo> productList;
                        if (IsPostBack == false)
                        {
                            if (_catalogNav.CategoryID != Null.NullInteger && _moduleSettings.FeaturedProducts.DisplayByCategory)
                                productList = GetFeaturedProducts(_catalogNav.CategoryID);
                            else
                                productList = GetPortalFeaturedProducts(PortalId);
                            productList = TruncateList(productList, _moduleSettings.FeaturedProducts.RowCount * _moduleSettings.FeaturedProducts.ColumnCount);
                            ViewState["StoreFeaturedProducts"] = productList;
                        }
                        else
                            productList = (List<ProductInfo>)ViewState["StoreFeaturedProducts"];
                        return LoadProductList(productList, ProductListTypes.Featured);
                    }

                    return null;

                case "POPULAR":
                    if (_moduleSettings.General.ShowPopularProducts)
                    {
                        List<ProductInfo> productList;
                        if (_catalogNav.CategoryID != Null.NullInteger)
                            productList = GetPopularProducts(_catalogNav.CategoryID);
                        else
                            productList = GetPortalPopularProducts(PortalId);
                        productList = TruncateList(productList, _moduleSettings.PopularProducts.RowCount * _moduleSettings.PopularProducts.ColumnCount);
                        return LoadProductList(productList, ProductListTypes.Popular);
                    }

                    return null;

                case "CATEGORY":
                    if (_moduleSettings.General.ShowCategoryProducts)
                    {
                        if (_catalogNav.ProductID == Null.NullInteger && _catalogNav.SearchID == Null.NullInteger)
                        {
                            if (_catalogNav.CategoryID != Null.NullInteger)
                            {
                                List<ProductInfo> productList = GetCategoryProducts(_catalogNav.CategoryID);
                                return LoadProductList(productList, ProductListTypes.Category);
                            }

                            if (_moduleSettings.General.DisplayAllProducts)
                            {
                                List<ProductInfo> productList = GetCategoryProducts(Null.NullInteger);
                                //List<ProductInfo> productList = GetPortalProducts();
                                return LoadProductList(productList, ProductListTypes.Category);
                            }

                            Panel pnlContainer = new Panel
                            {
                                CssClass = "StoreListContainer"
                            };
                            Label lblEmpty = new Label
                            {
                                CssClass = "StoreSelectCategory",
                                Text = Localization.GetString("SelectCategory", LocalResourceFile)
                            };
                            pnlContainer.Controls.Add(lblEmpty);
                            return pnlContainer;
                        }
                    }

                    return null;

                case "ALSOBOUGHT":
                    if (_moduleSettings.General.ShowProductDetail && _catalogNav.ProductID != Null.NullInteger)
                    {
                        List<ProductInfo> productList = GetAlsoBoughtProducts(PortalId, _catalogNav.ProductID);
                        return LoadProductList(productList, ProductListTypes.AlsoBought);
                    }

                    return null;

                case "DETAIL":
                    if (_moduleSettings.General.ShowProductDetail && _catalogNav.ProductID != Null.NullInteger)
                        return LoadProductDetail();

                    return null;

                default:
                    LiteralControl litText = new LiteralControl(tokenName);
                    return litText;
            }
        }

        private List<ProductInfo> GetNewProducts(int categoryID)
        {
            List<ProductInfo> products = new List<ProductInfo>();

            foreach (ProductInfo product in _productController.GetNewProducts(PortalId, categoryID, false))
            {
                if (StoreSettings.ProductsBehavior == (int)Behavior.HideProduct && product.StockQuantity < 1)
                {
                    // Do nothing, in this case the product is hidden
                }
                else
                    products.Add(product);
            }

            // Get child categories (if any) and recurse
            CategoryController categoryController = new CategoryController();

            foreach (CategoryInfo childCategory in categoryController.GetCategories(PortalId, false, categoryID))
            {
                if (childCategory.CategoryID != Null.NullInteger)
                    products.AddRange(GetNewProducts(childCategory.CategoryID));
            }

            return products;
        }

        private List<ProductInfo> GetPortalNewProducts(int portalID)
        {
            List<ProductInfo> products = new List<ProductInfo>();

            foreach (ProductInfo product in _productController.GetPortalNewProducts(portalID, false))
            {
                if (StoreSettings.ProductsBehavior == (int)Behavior.HideProduct && product.StockQuantity < 1)
                {
                    // Do nothing, in this case the product is hidden
                }
                else
                    products.Add(product);
            }

            return products;
        }

        private List<ProductInfo> GetFeaturedProducts(int categoryID)
        {
            List<ProductInfo> products = new List<ProductInfo>();

            foreach (ProductInfo product in _productController.GetFeaturedProducts(PortalId, categoryID, false))
            {
                if (StoreSettings.ProductsBehavior == (int)Behavior.HideProduct && product.StockQuantity < 1)
                {
                    // Do nothing, in this case the product is hidden
                }
                else
                    products.Add(product);
            }

            // Get child categories (if any) and recurse
            CategoryController categoryController = new CategoryController();

            foreach (CategoryInfo childCategory in categoryController.GetCategories(PortalId, false, categoryID))
            {
                if (childCategory.CategoryID != Null.NullInteger)
                    products.AddRange(GetFeaturedProducts(childCategory.CategoryID));
            }

            return products;
        }

        private List<ProductInfo> GetPortalFeaturedProducts(int portalID)
        {
            List<ProductInfo> products = new List<ProductInfo>();

            foreach (ProductInfo product in _productController.GetPortalFeaturedProducts(portalID, false))
            {
                if (StoreSettings.ProductsBehavior == (int)Behavior.HideProduct && product.StockQuantity < 1)
                {
                    // Do nothing, in this case the product is hidden
                }
                else
                    products.Add(product);
            }

            return products;
        }

        private List<ProductInfo> GetPopularProducts(int categoryID)
        {
            List<ProductInfo> products = new List<ProductInfo>();

            foreach (ProductInfo product in _productController.GetPopularProducts(PortalId, categoryID, false))
            {
                if (StoreSettings.ProductsBehavior == (int)Behavior.HideProduct && product.StockQuantity < 1)
                {
                    // Do nothing, in this case the product is hidden
                }
                else
                    products.Add(product);
            }

            // Get child categories (if any) and recurse
            CategoryController categoryController = new CategoryController();

            foreach (CategoryInfo childCategory in categoryController.GetCategories(PortalId, false, categoryID))
            {
                if (childCategory.CategoryID != Null.NullInteger)
                    products.AddRange(GetPopularProducts(childCategory.CategoryID));
            }

            return products;
        }

        private List<ProductInfo> GetPortalPopularProducts(int portalID)
        {
            List<ProductInfo> products = new List<ProductInfo>();

            foreach (ProductInfo product in _productController.GetPortalPopularProducts(portalID, false))
            {
                if (StoreSettings.ProductsBehavior == (int)Behavior.HideProduct && product.StockQuantity < 1)
                {
                    // Do nothing, in this case the product is hidden
                }
                else
                    products.Add(product);
            }

            return products;
        }

        private List<ProductInfo> GetCategoryProducts(int categoryID)
        {
            List<ProductInfo> products = new List<ProductInfo>();

            int sortID;
            if (_catalogNav.SortID != Null.NullInteger)
            {
                // Currently selected sort column
                sortID = _catalogNav.SortID;
            }
            else
            {
                // Default sort column
                sortID = _moduleSettings.SortProducts.SortBy;
            }

            string sortDir;
            if (_catalogNav.SortDir != Null.NullString)
            {
                // Currently selected sort direction
                sortDir = _catalogNav.SortDir;
            }
            else
            {
                // Default sort direction
                sortDir = _moduleSettings.SortProducts.SortDir;
            }

            foreach (ProductInfo product in _productController.GetCategoryProducts(PortalId, categoryID, false, sortID, sortDir))
            {
                if (StoreSettings.ProductsBehavior == (int)Behavior.HideProduct && product.StockQuantity < 1)
                {
                    // Do nothing, in this case the product is hidden
                }
                else
                    products.Add(product);
            }

            if (categoryID != Null.NullInteger && _moduleSettings.CategoryProducts.SubCategories)
            {
                CategoryController categoryController = new CategoryController();

                // Get child categories (if any) and recurse
                foreach (CategoryInfo childCategory in categoryController.GetCategories(PortalId, false, categoryID))
                {
                    if (childCategory.CategoryID != Null.NullInteger)
                        products.AddRange(GetCategoryProducts(childCategory.CategoryID));
                }
            }

            return products;
        }

        private List<ProductInfo> GetAlsoBoughtProducts(int portalID, int productID)
        {
            List<ProductInfo> products = new List<ProductInfo>();

            foreach (ProductInfo product in _productController.GetAlsoBoughtProducts(portalID, productID, false))
            {
                if (StoreSettings.ProductsBehavior == (int)Behavior.HideProduct && product.StockQuantity < 1)
                {
                    // Do nothing, in this case the product is hidden
                }
                else
                    products.Add(product);
            }

            return products;
        }

        private List<ProductInfo> GetSearchedProducts(int searchID, string searchValue)
        {
            List<ProductInfo> products = new List<ProductInfo>();

            int sortID;
            if (_catalogNav.SortID != Null.NullInteger)
            {
                // Currently selected sort column
                sortID = _catalogNav.SortID;
            }
            else
            {
                // Default sort column
                sortID = _moduleSettings.SortProducts.SortBy;
            }

            string sortDir;
            if (_catalogNav.SortDir != Null.NullString)
            {
                // Currently selected sort direction
                sortDir = _catalogNav.SortDir;
            }
            else
            {
                // Default sort direction
                sortDir = _moduleSettings.SortProducts.SortDir;
            }

            foreach (ProductInfo product in _productController.GetSearchedProducts(PortalId, searchID, searchValue, sortID, sortDir))
            {
                if (StoreSettings.ProductsBehavior == (int)Behavior.HideProduct && product.StockQuantity < 1)
                {
                    // Do nothing, in this case the product is hidden
                }
                else
                    products.Add(product);
            }

            return products;
        }

        private Control LoadProductList(List<ProductInfo> products, ProductListTypes listType)
        {
            if (products.Count == 0 && (listType == ProductListTypes.New || listType == ProductListTypes.Featured || listType == ProductListTypes.Popular || listType == ProductListTypes.AlsoBought))
                return null;

            ProductList productList = (ProductList)LoadControl(ControlPath + "ProductList.ascx");
            productList.ModuleConfiguration = ModuleConfiguration;
            productList.StoreSettings = StoreSettings;
            productList.ModuleSettings = _moduleSettings;
            productList.TemplatePath = _templatePath;
            productList.CategoryID = _catalogNav.CategoryID;
            productList.ID = listType.ToString();
            productList.ListType = listType;
            productList.EnableImageCaching = _moduleSettings.General.EnableImageCaching;
            productList.CacheImageDuration = _moduleSettings.General.CacheImageDuration;
            productList.ShowDetail = _moduleSettings.General.ShowProductDetail;

            switch (listType)
            {
                case ProductListTypes.New:
                    productList.Title = Localization.GetString("NPTitle.Text", LocalResourceFile);
                    productList.ContainerTemplate = _moduleSettings.NewProducts.ContainerTemplate;
                    productList.ContainerCssClass = "StoreNewProductList";
                    productList.Template = _moduleSettings.NewProducts.Template;
                    productList.ItemCssClass = "StoreNewProductItem";
                    productList.AlternatingItemCssClass = "StoreNewProductAlternatingItem";
                    productList.RowCount = _moduleSettings.NewProducts.RowCount;
                    productList.ColumnCount = _moduleSettings.NewProducts.ColumnCount;
                    productList.ColumnWidth = _moduleSettings.NewProducts.ColumnWidth;
                    productList.Direction = _moduleSettings.NewProducts.RepeatDirection;
                    productList.ShowThumbnail = _moduleSettings.NewProducts.ShowThumbnail;
                    productList.ThumbnailWidth = _moduleSettings.NewProducts.ThumbnailWidth;
                    productList.GIFBgColor = _moduleSettings.NewProducts.GIFBgColor;
                    productList.DetailPage = _moduleSettings.NewProducts.DetailPage;
                    break;

                case ProductListTypes.Featured:
                    productList.Title = Localization.GetString("FPTitle.Text", LocalResourceFile);
                    productList.ContainerTemplate = _moduleSettings.FeaturedProducts.ContainerTemplate;
                    productList.ContainerCssClass = "StoreFeaturedProductList";
                    productList.Template = _moduleSettings.FeaturedProducts.Template;
                    productList.ItemCssClass = "StoreFeaturedProductItem";
                    productList.AlternatingItemCssClass = "StoreFeaturedProductAlternatingItem";
                    productList.RowCount = _moduleSettings.FeaturedProducts.RowCount;
                    productList.ColumnCount = _moduleSettings.FeaturedProducts.ColumnCount;
                    productList.ColumnWidth = _moduleSettings.FeaturedProducts.ColumnWidth;
                    productList.Direction = _moduleSettings.FeaturedProducts.RepeatDirection;
                    productList.ShowThumbnail = _moduleSettings.FeaturedProducts.ShowThumbnail;
                    productList.ThumbnailWidth = _moduleSettings.FeaturedProducts.ThumbnailWidth;
                    productList.GIFBgColor = _moduleSettings.FeaturedProducts.GIFBgColor;
                    productList.DetailPage = _moduleSettings.FeaturedProducts.DetailPage;
                    break;

                case ProductListTypes.Popular:
                    productList.Title = Localization.GetString("PPTitle.Text", LocalResourceFile);
                    productList.ContainerTemplate = _moduleSettings.PopularProducts.ContainerTemplate;
                    productList.ContainerCssClass = "StorePopularProductList";
                    productList.Template = _moduleSettings.PopularProducts.Template;
                    productList.ItemCssClass = "StorePopularProductItem";
                    productList.AlternatingItemCssClass = "StorePopularProductAlternatingItem";
                    productList.RowCount = _moduleSettings.PopularProducts.RowCount;
                    productList.ColumnCount = _moduleSettings.PopularProducts.ColumnCount;
                    productList.ColumnWidth = _moduleSettings.PopularProducts.ColumnWidth;
                    productList.Direction = _moduleSettings.PopularProducts.RepeatDirection;
                    productList.ShowThumbnail = _moduleSettings.PopularProducts.ShowThumbnail;
                    productList.ThumbnailWidth = _moduleSettings.PopularProducts.ThumbnailWidth;
                    productList.GIFBgColor = _moduleSettings.PopularProducts.GIFBgColor;
                    productList.DetailPage = _moduleSettings.PopularProducts.DetailPage;
                    break;

                case ProductListTypes.Category:
                    if (_catalogNav.CategoryID == Null.NullInteger && _moduleSettings.General.DisplayAllProducts)
                        productList.Title = Localization.GetString("APTitle.Text", LocalResourceFile);
                    else
                        productList.Title = Localization.GetString("CPTitle.Text", LocalResourceFile);
                    productList.ContainerTemplate = _moduleSettings.CategoryProducts.ContainerTemplate;
                    productList.ContainerCssClass = "StoreCategoryProductList";
                    productList.Template = _moduleSettings.CategoryProducts.Template;
                    productList.ItemCssClass = "StoreCategoryProductItem";
                    productList.AlternatingItemCssClass = "StoreCategoryProductAlternatingItem";
                    productList.RowCount = _moduleSettings.CategoryProducts.RowCount;
                    productList.ColumnCount = _moduleSettings.CategoryProducts.ColumnCount;
                    productList.ColumnWidth = _moduleSettings.CategoryProducts.ColumnWidth;
                    productList.Direction = _moduleSettings.CategoryProducts.RepeatDirection;
                    productList.ShowThumbnail = _moduleSettings.CategoryProducts.ShowThumbnail;
                    productList.ThumbnailWidth = _moduleSettings.CategoryProducts.ThumbnailWidth;
                    productList.GIFBgColor = _moduleSettings.CategoryProducts.GIFBgColor;
                    productList.DetailPage = _moduleSettings.CategoryProducts.DetailPage;
                    break;

                case ProductListTypes.SearchResults:
                    productList.Title = Localization.GetString("SPTitle.Text", LocalResourceFile);
                    productList.ContainerTemplate = _moduleSettings.SearchProducts.SearchResultsTemplate;
                    productList.ContainerCssClass = "StoreSearchResultsProductList";
                    productList.Template = _moduleSettings.CategoryProducts.Template;
                    productList.ItemCssClass = "StoreSearchResultsProductItem";
                    productList.AlternatingItemCssClass = "StoreSearchResultsProductAlternatingItem";
                    productList.RowCount = _moduleSettings.CategoryProducts.RowCount;
                    productList.ColumnCount = _moduleSettings.CategoryProducts.ColumnCount;
                    productList.ColumnWidth = _moduleSettings.CategoryProducts.ColumnWidth;
                    productList.Direction = _moduleSettings.CategoryProducts.RepeatDirection;
                    productList.ShowThumbnail = _moduleSettings.CategoryProducts.ShowThumbnail;
                    productList.ThumbnailWidth = _moduleSettings.CategoryProducts.ThumbnailWidth;
                    productList.GIFBgColor = _moduleSettings.CategoryProducts.GIFBgColor;
                    productList.DetailPage = _moduleSettings.CategoryProducts.DetailPage;
                    break;

                case ProductListTypes.AlsoBought:
                    productList.Title = Localization.GetString("ABTitle.Text", LocalResourceFile);
                    productList.ContainerTemplate = _moduleSettings.AlsoBoughtProducts.ContainerTemplate;
                    productList.ContainerCssClass = "StoreAlsoBoughtProductList";
                    productList.Template = _moduleSettings.AlsoBoughtProducts.Template;
                    productList.ItemCssClass = "StoreAlsoBoughtProductItem";
                    productList.AlternatingItemCssClass = "StoreAlsoBoughtProductAlternatingItem";
                    productList.RowCount = _moduleSettings.AlsoBoughtProducts.RowCount;
                    productList.ColumnCount = _moduleSettings.AlsoBoughtProducts.ColumnCount;
                    productList.ColumnWidth = _moduleSettings.AlsoBoughtProducts.ColumnWidth;
                    productList.Direction = _moduleSettings.AlsoBoughtProducts.RepeatDirection;
                    productList.ShowThumbnail = _moduleSettings.AlsoBoughtProducts.ShowThumbnail;
                    productList.ThumbnailWidth = _moduleSettings.AlsoBoughtProducts.ThumbnailWidth;
                    productList.GIFBgColor = _moduleSettings.AlsoBoughtProducts.GIFBgColor;
                    productList.DetailPage = _moduleSettings.AlsoBoughtProducts.DetailPage;
                    break;
            }

            productList.DataSource = products;

            return productList;
        }

        private Control LoadProductDetail()
        {
            ProductDetail productDetail = (ProductDetail)LoadControl(ControlPath + "ProductDetail.ascx");

            productDetail.ModuleConfiguration = ModuleConfiguration;
            productDetail.TemplatePath = _templatePath;
            productDetail.Template = _moduleSettings.ProductDetail.Template;
            productDetail.ReturnPage = _moduleSettings.ProductDetail.ReturnPage;
            productDetail.CategoryID = _currentProduct.CategoryID;
            productDetail.CartWarning = _moduleSettings.ProductDetail.CartWarning;
            productDetail.ShowThumbnail = _moduleSettings.ProductDetail.ShowThumbnail;
            productDetail.ThumbnailWidth = _moduleSettings.ProductDetail.ThumbnailWidth;
            productDetail.GIFBgColor = _moduleSettings.ProductDetail.GIFBgColor;
            productDetail.EnableImageCaching = _moduleSettings.General.EnableImageCaching;
            productDetail.CacheImageDuration = _moduleSettings.General.CacheImageDuration;
            productDetail.ShowReviews = _moduleSettings.ProductDetail.ShowReviews;
            productDetail.DataSource = _currentProduct;

            return productDetail;
        }

        private List<ProductInfo> TruncateList(List<ProductInfo> list, int maxCount)
        {
            if (list.Count > maxCount)
                list.RemoveRange(maxCount, list.Count - maxCount);
            return list;
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
                                case "[CATEGORYNAME]":
                                    if (_catalogNav.CategoryID != Null.NullInteger)
                                        tempValue = _currentCategory.Name;
                                    else
                                        tempValue = "";
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
                                case "[CATEGORYKEYWORDS]":
                                    if (_catalogNav.CategoryID != Null.NullInteger)
                                        tempValue = _currentCategory.Keywords;
                                    else
                                        tempValue = "";
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
                                case "[CATEGORYDESCRIPTION]":
                                    if (_catalogNav.CategoryID != Null.NullInteger)
                                        tempValue = _currentCategory.Description;
                                    else
                                        tempValue = "";
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

        #region IActionable Members

        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection actions = new ModuleActionCollection();
                if (StoreSettings != null)
                {
                    CatalogNavigation editNav = new CatalogNavigation(ModuleId, Request.QueryString)
                    {
                        ProductID = Null.NullInteger,
                        PageIndex = Null.NullInteger,
                        Edit = "Product"
                    };
                    actions.Add(GetNextActionID(), Localization.GetString("AddNewProduct", LocalResourceFile), ModuleActionType.AddContent, "", "", editNav.GetEditUrl(), false, SecurityAccessLevel.Edit, true, false);

                    if (_moduleSettings.General.AllowPrint)
                    {
                        string url = _catalogNav.GetNavigationUrl() + "&mid=" + ModuleId + "&SkinSrc=" + Globals.QueryStringEncode("[G]" + SkinController.RootSkin + "/" + Globals.glbHostSkinFolder + "/" + "No Skin") + "&ContainerSrc=" + Globals.QueryStringEncode("[G]" + SkinController.RootContainer + "/" + Globals.glbHostSkinFolder + "/" + "No Container") + "&dnnprintmode=true";
                        actions.Add(GetNextActionID(), Localization.GetString(ModuleActionType.PrintModule, Localization.GlobalResourceFile), "StorePrint.Action", "", "print.gif", url, false, SecurityAccessLevel.Anonymous, true, true);
                    }
                }
                return actions;
            }
        }

        #endregion
    }
}
