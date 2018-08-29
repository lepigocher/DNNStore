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
using System.Web.UI.WebControls;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

using DotNetNuke.Modules.Store.Core.Admin;
using DotNetNuke.Modules.Store.Core.Catalog;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for Settings.
	/// </summary>
    public partial class CatalogSettings : ModuleSettingsBase
	{
        #region Private Members

        private ModuleSettings _moduleSettings;
        private StoreInfo _storeInfo;
        private string _templatePath = "";
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

		#region Events

        override protected void OnInit(EventArgs e)
		{
            _moduleSettings = new ModuleSettings(ModuleId, TabId);
			base.OnInit(e);
		}
		
        protected void chkUseDefaultCategory_CheckedChanged(object sender, EventArgs e)
        {
            trDefaultCategory.Visible = chkUseDefaultCategory.Checked;
            trDisplayAllProducts.Visible = !chkUseDefaultCategory.Checked;
        }

        protected void chkShowCategory_CheckedChanged(object sender, EventArgs e)
        {
            bool visibility = chkShowCategory.Checked;
            fshCategoryProductList.Visible = visibility;
            fsCategoryProductList.Visible = visibility;
            fshSearchProduct.Visible = visibility;
            fsSearchProduct.Visible = visibility;
            fshSortProduct.Visible = visibility;
            fsSortProduct.Visible = visibility;
        }

        protected void chkShowDetail_CheckedChanged(object sender, EventArgs e)
        {
            bool visibility = chkShowDetail.Checked;
            fshProductDetails.Visible = visibility;
            fsProductDetails.Visible = visibility;
            trShowAlsoBoughtProducts.Visible = visibility;
            fshAlsoBoughtProductList.Visible = visibility;
            fsAlsoBoughtProductList.Visible = visibility;
        }

        protected void chkShowNew_CheckedChanged(object sender, EventArgs e)
        {
            bool visibility = chkShowNew.Checked;
            fshNewProductList.Visible = visibility;
            fsNewProductList.Visible = visibility;
        }

        protected void chkShowFeatured_CheckedChanged(object sender, EventArgs e)
        {
            bool visibility = chkShowFeatured.Checked;
            fshFeaturedProductList.Visible = visibility;
            fsFeaturedProductList.Visible = visibility;
        }

        protected void chkShowPopular_CheckedChanged(object sender, EventArgs e)
        {
            bool visibility = chkShowPopular.Checked;
            fshPopularProductList.Visible = visibility;
            fsPopularProductList.Visible = visibility;
        }

        protected void chkShowAlsoBought_CheckedChanged(object sender, EventArgs e)
        {
            bool visibility = chkShowAlsoBought.Checked;
            fshAlsoBoughtProductList.Visible = visibility;
            fsAlsoBoughtProductList.Visible = visibility;
        }

        protected void chkSearchManufacturer_CheckedChanged(object sender, EventArgs e)
        {
            lstSPLSearchColumn.Items[0].Enabled = chkSearchManufacturer.Checked;
            DisplaySearchRows();
        }

        protected void chkSearchModelNumber_CheckedChanged(object sender, EventArgs e)
        {
            lstSPLSearchColumn.Items[1].Enabled = chkSearchModelNumber.Checked;
            DisplaySearchRows();
        }

        protected void chkSearchModelName_CheckedChanged(object sender, EventArgs e)
        {
            lstSPLSearchColumn.Items[2].Enabled = chkSearchModelName.Checked;
            DisplaySearchRows();
        }

        protected void chkSearchSummary_CheckedChanged(object sender, EventArgs e)
        {
            lstSPLSearchColumn.Items[3].Enabled = chkSearchSummary.Checked;
            DisplaySearchRows();
        }

        protected void chkSearchDescription_CheckedChanged(object sender, EventArgs e)
        {
            lstSPLSearchColumn.Items[4].Enabled = chkSearchDescription.Checked;
            DisplaySearchRows();
        }

        protected void chkSortManufacturer_CheckedChanged(object sender, EventArgs e)
        {
            lstCPLSortBy.Items[0].Enabled = chkSortManufacturer.Checked;
            DisplaySortRows();
        }

        protected void chkSortModelNumber_CheckedChanged(object sender, EventArgs e)
        {
            lstCPLSortBy.Items[1].Enabled = chkSortModelNumber.Checked;
            DisplaySortRows();
        }

        protected void chkSortModelName_CheckedChanged(object sender, EventArgs e)
        {
            lstCPLSortBy.Items[2].Enabled = chkSortModelName.Checked;
            DisplaySortRows();
        }

        protected void chkSortUnitPrice_CheckedChanged(object sender, EventArgs e)
        {
            lstCPLSortBy.Items[3].Enabled = chkSortUnitPrice.Checked;
            DisplaySortRows();
        }

        protected void chkSortCreatedDate_CheckedChanged(object sender, EventArgs e)
        {
            lstCPLSortBy.Items[4].Enabled = chkSortCreatedDate.Checked;
            DisplaySortRows();
        }

		#endregion

		#region Base Method Implementations

		public override void LoadSettings()
		{
			try
			{
                _storeInfo = StoreController.GetStoreInfo(PortalId);
                if (_storeInfo.PortalTemplates)
                    _templatePath = MapPath(PortalSettings.HomeDirectory + "Store/Templates/");
                else
                    _templatePath = MapPath(TemplateSourceDirectory + "/Templates/");

                // Fill category list
                CategoryController categoryController = new CategoryController();
                List<CategoryInfo> categoryList = categoryController.GetCategoriesPath(PortalId, false, Null.NullInteger);
                lstDefaultCategory.DataTextField = "CategoryPathName";
                lstDefaultCategory.DataValueField = "CategoryId";
                lstDefaultCategory.DataSource = categoryList;
                lstDefaultCategory.DataBind();
                lstDefaultCategory.Items.Insert(0, new ListItem(Localization.GetString("SelectDefaultCategory", LocalResourceFile), "-1"));

                // Add tab name to combo boxes (Detail Page and Return Page) for each section
				TabController tabController = new TabController();
                //Dictionary<int, TabInfo> tabs = tabController.GetTabsByPortal(PortalId);
                TabCollection tabs = tabController.GetTabsByPortal(PortalId);

                lstNPLDetailPage.Items.Add(new ListItem(Localization.GetString("NPLSamePage", LocalResourceFile), "0"));
                lstFPLDetailPage.Items.Add(new ListItem(Localization.GetString("FPLSamePage", LocalResourceFile), "0"));
                lstPPLDetailPage.Items.Add(new ListItem(Localization.GetString("PPLSamePage", LocalResourceFile), "0"));
                lstCPLDetailPage.Items.Add(new ListItem(Localization.GetString("CPLSamePage", LocalResourceFile), "0"));
                lstABPLDetailPage.Items.Add(new ListItem(Localization.GetString("ABPLSamePage", LocalResourceFile), "0"));
                lstPDSReturnPage.Items.Add(new ListItem(Localization.GetString("PDSSamePage", LocalResourceFile), "0"));

				foreach (TabInfo tab in tabs.Values)
				{
					if (!tab.IsDeleted && !tab.IsSuperTab)
					{
                        lstNPLDetailPage.Items.Add(new ListItem(tab.TabName, tab.TabID.ToString()));
                        lstFPLDetailPage.Items.Add(new ListItem(tab.TabName, tab.TabID.ToString()));
						lstPPLDetailPage.Items.Add(new ListItem(tab.TabName, tab.TabID.ToString()));
						lstCPLDetailPage.Items.Add(new ListItem(tab.TabName, tab.TabID.ToString()));
                        lstABPLDetailPage.Items.Add(new ListItem(tab.TabName, tab.TabID.ToString()));
                        lstPDSReturnPage.Items.Add(new ListItem(tab.TabName, tab.TabID.ToString()));
					}
				}
				
				LoadTemplates();

                // Add directions to repeat combo boxes
                String repeatDirection = Localization.GetString("RepeatDirectionHoriz", LocalResourceFile);
                lstNPLRepeatDirection.Items.Add(new ListItem(repeatDirection, "H"));
                lstFPLRepeatDirection.Items.Add(new ListItem(repeatDirection, "H"));
                lstPPLRepeatDirection.Items.Add(new ListItem(repeatDirection, "H"));
                lstCPLRepeatDirection.Items.Add(new ListItem(repeatDirection, "H"));
                lstABPLRepeatDirection.Items.Add(new ListItem(repeatDirection, "H"));

                repeatDirection = Localization.GetString("RepeatDirectionVert", LocalResourceFile);
                lstNPLRepeatDirection.Items.Add(new ListItem(repeatDirection, "V"));
                lstFPLRepeatDirection.Items.Add(new ListItem(repeatDirection, "V"));
                lstPPLRepeatDirection.Items.Add(new ListItem(repeatDirection, "V"));
                lstCPLRepeatDirection.Items.Add(new ListItem(repeatDirection, "V"));
                lstABPLRepeatDirection.Items.Add(new ListItem(repeatDirection, "V"));

                // Define allowed sort columns
                int sortColumns = _moduleSettings.SortProducts.SortColumns;
                chkSortManufacturer.Checked = ((sortColumns & (int)SortColumn.Manufacturer) != 0);
                chkSortModelNumber.Checked = ((sortColumns & (int)SortColumn.ModelNumber) != 0);
                chkSortModelName.Checked = ((sortColumns & (int)SortColumn.ModelName) != 0);
                chkSortUnitPrice.Checked = ((sortColumns & (int)SortColumn.UnitPrice) != 0);
                chkSortCreatedDate.Checked = ((sortColumns & (int)SortColumn.Date) != 0);

                // Add column names to sort order dropdownlist
                lstCPLSortBy.Items.Add(new ListItem(Localization.GetString("chkSortManufacturer", LocalResourceFile), "0"));
                lstCPLSortBy.Items[0].Enabled = chkSortManufacturer.Checked;
                lstCPLSortBy.Items.Add(new ListItem(Localization.GetString("chkSortModelNumber", LocalResourceFile), "1"));
                lstCPLSortBy.Items[1].Enabled = chkSortModelNumber.Checked;
                lstCPLSortBy.Items.Add(new ListItem(Localization.GetString("chkSortModelName", LocalResourceFile), "2"));
                lstCPLSortBy.Items[2].Enabled = chkSortModelName.Checked;
                lstCPLSortBy.Items.Add(new ListItem(Localization.GetString("chkSortUnitPrice", LocalResourceFile), "3"));
                lstCPLSortBy.Items[3].Enabled = chkSortUnitPrice.Checked;
                lstCPLSortBy.Items.Add(new ListItem(Localization.GetString("chkSortCreatedDate", LocalResourceFile), "4"));
                lstCPLSortBy.Items[4].Enabled = chkSortCreatedDate.Checked;

                // Add sort directions
                String sortDir = "";
                sortDir = Localization.GetString("SortAscending", LocalResourceFile);
                lstCPLSortDir.Items.Add(new ListItem(sortDir, "ASC"));
                sortDir = Localization.GetString("SortDescending", LocalResourceFile);
                lstCPLSortDir.Items.Add(new ListItem(sortDir, "DESC"));

                // Define allowed search columns
                int searchColumns = _moduleSettings.SearchProducts.SearchColumns;
                chkSearchManufacturer.Checked = ((searchColumns & (int)SearchColumn.Manufacturer) != 0);
                chkSearchModelNumber.Checked = ((searchColumns & (int)SearchColumn.ModelNumber) != 0);
                chkSearchModelName.Checked = ((searchColumns & (int)SearchColumn.ModelName) != 0);
                chkSearchSummary.Checked = ((searchColumns & (int)SearchColumn.ProductSummay) != 0);
                chkSearchDescription.Checked = ((searchColumns & (int)SearchColumn.ProductDescription) != 0);

                // Add column names to search column dropdonwlist
                lstSPLSearchColumn.Items.Add(new ListItem(Localization.GetString("chkSearchManufacturer", LocalResourceFile), "0"));
                lstSPLSearchColumn.Items[0].Enabled = chkSearchManufacturer.Checked;
                lstSPLSearchColumn.Items.Add(new ListItem(Localization.GetString("chkSearchModelNumber", LocalResourceFile), "1"));
                lstSPLSearchColumn.Items[1].Enabled = chkSearchModelNumber.Checked;
                lstSPLSearchColumn.Items.Add(new ListItem(Localization.GetString("chkSearchModelName", LocalResourceFile), "2"));
                lstSPLSearchColumn.Items[2].Enabled = chkSearchModelName.Checked;
                lstSPLSearchColumn.Items.Add(new ListItem(Localization.GetString("chkSearchSummary", LocalResourceFile), "3"));
                lstSPLSearchColumn.Items[3].Enabled = chkSearchSummary.Checked;
                lstSPLSearchColumn.Items.Add(new ListItem(Localization.GetString("chkSearchDescription", LocalResourceFile), "4"));
                lstSPLSearchColumn.Items[4].Enabled = chkSearchDescription.Checked;

				// General Player Settings
                ListItem itemTemplate = lstTemplate.Items.FindByText(_moduleSettings.General.Template);
                if (itemTemplate != null)
                    itemTemplate.Selected = true;
				chkUseDefaultCategory.Checked = _moduleSettings.General.UseDefaultCategory;
                if (chkUseDefaultCategory.Checked)
                {
                    trDefaultCategory.Visible = true;
                    trDisplayAllProducts.Visible = false;
                }
                else
                {
                    trDefaultCategory.Visible = false;
                    trDisplayAllProducts.Visible = true;
                }
			    lstDefaultCategory.SelectedValue = _moduleSettings.General.DefaultCategoryID.ToString();
			    chkDisplayAllProducts.Checked = _moduleSettings.General.DisplayAllProducts;
				chkShowMessage.Checked = _moduleSettings.General.ShowMessage;
				chkShowCategory.Checked = _moduleSettings.General.ShowCategoryProducts;
                if (chkShowCategory.Checked == false)
                {
                    fshCategoryProductList.Visible = false;
                    fsCategoryProductList.Visible = false;
                    fshSearchProduct.Visible = false;
                    fsSearchProduct.Visible = false;
                    fshSortProduct.Visible = false;
                    fsSortProduct.Visible = false;
                }
                else
                {
                    DisplaySearchRows();
                    DisplaySortRows();
                }
				chkShowDetail.Checked = _moduleSettings.General.ShowProductDetail;
                if (chkShowDetail.Checked == false)
                {
                    fshProductDetails.Visible = false;
                    fsProductDetails.Visible = false;
                    trShowAlsoBoughtProducts.Visible = false;
                    fshAlsoBoughtProductList.Visible = false;
                    fsAlsoBoughtProductList.Visible = false;
                }
                chkShowAlsoBought.Checked = _moduleSettings.General.ShowAlsoBought;
                if (chkShowAlsoBought.Checked == false)
                {
                    fshAlsoBoughtProductList.Visible = false;
                    fsAlsoBoughtProductList.Visible = false;
                }
                chkShowNew.Checked = _moduleSettings.General.ShowNewProducts;
                if (chkShowNew.Checked == false)
                {
                    fshNewProductList.Visible = false;
                    fsNewProductList.Visible = false;
                }
				chkShowFeatured.Checked = _moduleSettings.General.ShowFeaturedProducts;
                if (chkShowFeatured.Checked == false)
                {
                    fshFeaturedProductList.Visible = false;
                    fsFeaturedProductList.Visible = false;
                }
				chkShowPopular.Checked = _moduleSettings.General.ShowPopularProducts;
                if (chkShowPopular.Checked == false)
                {
                    fshPopularProductList.Visible = false;
                    fsPopularProductList.Visible = false;
                }
                chkAllowPrint.Checked = _moduleSettings.General.AllowPrint;
                chkEnableContentIndexing.Checked = _moduleSettings.General.EnableContentIndexing;
                chkEnableImageCaching.Checked = _moduleSettings.General.EnableImageCaching;
                txtCacheDuration.Text = _moduleSettings.General.CacheImageDuration.ToString();

                // New list settings
                ListItem itemNPLContainerTemplate = lstNPLContainerTemplate.Items.FindByText(_moduleSettings.NewProducts.ContainerTemplate);
                if (itemNPLContainerTemplate != null)
                    itemNPLContainerTemplate.Selected = true;
                ListItem itemNPLTemplate = lstNPLTemplate.Items.FindByText(_moduleSettings.NewProducts.Template);
                if (itemNPLTemplate != null)
                    itemNPLTemplate.Selected = true;
                txtNPLRowCount.Text = _moduleSettings.NewProducts.RowCount.ToString();
                txtNPLColumnCount.Text = _moduleSettings.NewProducts.ColumnCount.ToString();
                txtNPLColumnWidth.Text = _moduleSettings.NewProducts.ColumnWidth.ToString();
                lstNPLRepeatDirection.SelectedValue = _moduleSettings.NewProducts.RepeatDirection;
                chkNPLDisplayByCategory.Checked = _moduleSettings.NewProducts.DisplayByCategory;
                chkNPLShowThumbnail.Checked = _moduleSettings.NewProducts.ShowThumbnail;
                txtNPLThumbnailWidth.Text = _moduleSettings.NewProducts.ThumbnailWidth.ToString();
                txtNPLGIFBgColor.Text = _moduleSettings.NewProducts.GIFBgColor;
                lstNPLDetailPage.SelectedValue = _moduleSettings.NewProducts.DetailPage.ToString();

				// Featured list settings
                ListItem itemFPLContainerTemplate = lstFPLContainerTemplate.Items.FindByText(_moduleSettings.FeaturedProducts.ContainerTemplate);
                if (itemFPLContainerTemplate != null)
                    itemFPLContainerTemplate.Selected = true;
				ListItem itemFPLTemplate = lstFPLTemplate.Items.FindByText(_moduleSettings.FeaturedProducts.Template);
				if (itemFPLTemplate != null)
					itemFPLTemplate.Selected = true;
				txtFPLRowCount.Text = _moduleSettings.FeaturedProducts.RowCount.ToString();
				txtFPLColumnCount.Text = _moduleSettings.FeaturedProducts.ColumnCount.ToString();
				txtFPLColumnWidth.Text = _moduleSettings.FeaturedProducts.ColumnWidth.ToString();
                lstFPLRepeatDirection.SelectedValue = _moduleSettings.FeaturedProducts.RepeatDirection;
                chkFPLDisplayByCategory.Checked = _moduleSettings.FeaturedProducts.DisplayByCategory;
                chkFPLShowThumbnail.Checked = _moduleSettings.FeaturedProducts.ShowThumbnail;
				txtFPLThumbnailWidth.Text = _moduleSettings.FeaturedProducts.ThumbnailWidth.ToString();
                txtFPLGIFBgColor.Text = _moduleSettings.FeaturedProducts.GIFBgColor;
				lstFPLDetailPage.SelectedValue = _moduleSettings.FeaturedProducts.DetailPage.ToString();

				// Popular list settings
                ListItem itemPPLContainerTemplate = lstPPLContainerTemplate.Items.FindByText(_moduleSettings.PopularProducts.ContainerTemplate);
                if (itemPPLContainerTemplate != null)
                    itemPPLContainerTemplate.Selected = true;
				ListItem itemPPLTemplate = lstPPLTemplate.Items.FindByText(_moduleSettings.PopularProducts.Template);
				if (itemPPLTemplate != null)
					itemPPLTemplate.Selected = true;
				txtPPLRowCount.Text = _moduleSettings.PopularProducts.RowCount.ToString();
				txtPPLColumnCount.Text = _moduleSettings.PopularProducts.ColumnCount.ToString();
				txtPPLColumnWidth.Text = _moduleSettings.PopularProducts.ColumnWidth.ToString();
                lstPPLRepeatDirection.SelectedValue = _moduleSettings.PopularProducts.RepeatDirection;
				chkPPLShowThumbnail.Checked = _moduleSettings.PopularProducts.ShowThumbnail;
				txtPPLThumbnailWidth.Text = _moduleSettings.PopularProducts.ThumbnailWidth.ToString();
                txtPPLGIFBgColor.Text = _moduleSettings.PopularProducts.GIFBgColor;
                lstPPLDetailPage.SelectedValue = _moduleSettings.PopularProducts.DetailPage.ToString();

				// Category list settings
				ListItem itemCPLContainerTemplate = lstCPLContainerTemplate.Items.FindByText(_moduleSettings.CategoryProducts.ContainerTemplate);
                if (itemCPLContainerTemplate != null)
                    itemCPLContainerTemplate.Selected = true;
				ListItem itemCPLTemplate = lstCPLTemplate.Items.FindByText(_moduleSettings.CategoryProducts.Template);
				if (itemCPLTemplate != null)
					itemCPLTemplate.Selected = true;
                txtCPLRowCount.Text = _moduleSettings.CategoryProducts.RowCount.ToString();
				txtCPLColumnCount.Text = _moduleSettings.CategoryProducts.ColumnCount.ToString();
				txtCPLColumnWidth.Text = _moduleSettings.CategoryProducts.ColumnWidth.ToString();
                lstCPLRepeatDirection.SelectedValue = _moduleSettings.CategoryProducts.RepeatDirection;
				chkCPLShowThumbnail.Checked = _moduleSettings.CategoryProducts.ShowThumbnail;
				txtCPLThumbnailWidth.Text = _moduleSettings.CategoryProducts.ThumbnailWidth.ToString();
                txtCPLGIFBgColor.Text = _moduleSettings.CategoryProducts.GIFBgColor;
                lstCPLDetailPage.SelectedValue = _moduleSettings.CategoryProducts.DetailPage.ToString();
                chkCPLSubCategories.Checked = _moduleSettings.CategoryProducts.SubCategories;
                chkCPLRepositioning.Checked = _moduleSettings.CategoryProducts.Repositioning;

                // Sort settings
                lstCPLSortBy.SelectedValue = _moduleSettings.SortProducts.SortBy.ToString();
                lstCPLSortDir.SelectedValue = _moduleSettings.SortProducts.SortDir;

                // Search settings
                ListItem itemSPLTemplate = lstSPLTemplate.Items.FindByText(_moduleSettings.SearchProducts.SearchResultsTemplate);
                if (itemSPLTemplate != null)
                    itemSPLTemplate.Selected = true;
                lstSPLSearchColumn.SelectedValue = _moduleSettings.SearchProducts.SearchBy.ToString();

				// Detail settings
				ListItem itemDetailTemplate = lstDetailTemplate.Items.FindByText(_moduleSettings.ProductDetail.Template);
				if (itemDetailTemplate != null)
				{
					itemDetailTemplate.Selected = true;
				}
                chkDetailCartWarning.Checked = _moduleSettings.ProductDetail.CartWarning;
				chkDetailShowThumbnail.Checked = _moduleSettings.ProductDetail.ShowThumbnail;
				txtDetailThumbnailWidth.Text = _moduleSettings.ProductDetail.ThumbnailWidth.ToString();
                txtDetailGIFBgColor.Text = _moduleSettings.ProductDetail.GIFBgColor;
                chkDetailShowReviews.Checked = _moduleSettings.ProductDetail.ShowReviews;
                lstPDSReturnPage.SelectedValue = _moduleSettings.ProductDetail.ReturnPage.ToString();

                // Also Bought list settings
                ListItem itemABPLContainerTemplate = lstABPLContainerTemplate.Items.FindByText(_moduleSettings.AlsoBoughtProducts.ContainerTemplate);
                if (itemABPLContainerTemplate != null)
                    itemABPLContainerTemplate.Selected = true;
                ListItem itemABPLTemplate = lstABPLTemplate.Items.FindByText(_moduleSettings.AlsoBoughtProducts.Template);
                if (itemABPLTemplate != null)
                    itemABPLTemplate.Selected = true;
                txtABPLRowCount.Text = _moduleSettings.AlsoBoughtProducts.RowCount.ToString();
                txtABPLColumnCount.Text = _moduleSettings.AlsoBoughtProducts.ColumnCount.ToString();
                txtABPLColumnWidth.Text = _moduleSettings.AlsoBoughtProducts.ColumnWidth.ToString();
                lstABPLRepeatDirection.SelectedValue = _moduleSettings.AlsoBoughtProducts.RepeatDirection;
                chkABPLShowThumbnail.Checked = _moduleSettings.AlsoBoughtProducts.ShowThumbnail;
                txtABPLThumbnailWidth.Text = _moduleSettings.AlsoBoughtProducts.ThumbnailWidth.ToString();
                txtABPLGIFBgColor.Text = _moduleSettings.AlsoBoughtProducts.GIFBgColor;
                lstABPLDetailPage.SelectedValue = _moduleSettings.AlsoBoughtProducts.DetailPage.ToString();
			}
			catch(Exception ex)
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		public override void UpdateSettings()
		{
			try
			{
				// General Settings
				_moduleSettings.General.Template = lstTemplate.SelectedItem.Text;
				_moduleSettings.General.UseDefaultCategory = chkUseDefaultCategory.Checked;
				if (chkUseDefaultCategory.Checked)
					_moduleSettings.General.DefaultCategoryID = int.Parse(lstDefaultCategory.SelectedItem.Value);
			    _moduleSettings.General.DisplayAllProducts = chkDisplayAllProducts.Checked;
				_moduleSettings.General.ShowMessage = chkShowMessage.Checked;
				_moduleSettings.General.ShowCategoryProducts = chkShowCategory.Checked;
				_moduleSettings.General.ShowProductDetail = chkShowDetail.Checked;
                _moduleSettings.General.ShowAlsoBought = chkShowAlsoBought.Checked;
                _moduleSettings.General.ShowNewProducts = chkShowNew.Checked;
                _moduleSettings.General.ShowFeaturedProducts = chkShowFeatured.Checked;
				_moduleSettings.General.ShowPopularProducts = chkShowPopular.Checked;
                _moduleSettings.General.AllowPrint = chkAllowPrint.Checked;
                if (_moduleSettings.General.AllowPrint)
                {
                    ModuleController controler = new ModuleController();
                    ModuleInfo moduleInfo = controler.GetModule(ModuleId, TabId, false);
                    if (moduleInfo.DisplayPrint == true)
                    {
                        moduleInfo.DisplayPrint = false;
                        controler.UpdateModule(moduleInfo);
                    }
                }
                _moduleSettings.General.EnableContentIndexing = chkEnableContentIndexing.Checked;
                _moduleSettings.General.EnableImageCaching = chkEnableImageCaching.Checked;
                _moduleSettings.General.CacheImageDuration = int.Parse(txtCacheDuration.Text);

				// Category list settings
                if (_moduleSettings.General.ShowCategoryProducts)
                {
                    _moduleSettings.CategoryProducts.ContainerTemplate = lstCPLContainerTemplate.SelectedItem.Text;
                    _moduleSettings.CategoryProducts.Template = lstCPLTemplate.SelectedItem.Text;
                    _moduleSettings.CategoryProducts.RowCount = int.Parse(txtCPLRowCount.Text);
                    _moduleSettings.CategoryProducts.ColumnCount = int.Parse(txtCPLColumnCount.Text);
                    _moduleSettings.CategoryProducts.ColumnWidth = int.Parse(txtCPLColumnWidth.Text);
                    _moduleSettings.CategoryProducts.RepeatDirection = lstCPLRepeatDirection.SelectedItem.Value;
                    _moduleSettings.CategoryProducts.ShowThumbnail = chkCPLShowThumbnail.Checked;
                    _moduleSettings.CategoryProducts.ThumbnailWidth = int.Parse(txtCPLThumbnailWidth.Text);
                    _moduleSettings.CategoryProducts.GIFBgColor = txtCPLGIFBgColor.Text;
                    _moduleSettings.CategoryProducts.DetailPage = int.Parse(lstCPLDetailPage.SelectedItem.Value);
                    _moduleSettings.CategoryProducts.SubCategories = chkCPLSubCategories.Checked;
                    _moduleSettings.CategoryProducts.Repositioning = chkCPLRepositioning.Checked;
                }

                // Search Settings
                _moduleSettings.SearchProducts.SearchColumns = ComputeSearchColumns();
                if (_moduleSettings.SearchProducts.SearchColumns != 0)
                {
                    _moduleSettings.SearchProducts.SearchBy = int.Parse(lstSPLSearchColumn.SelectedItem.Value);
                    _moduleSettings.SearchProducts.SearchResultsTemplate = lstSPLTemplate.SelectedItem.Text;
                }

                // Sort Settings
                _moduleSettings.SortProducts.SortColumns = ComputeSortColumns();
                if (_moduleSettings.SortProducts.SortColumns != 0)
                {
                    _moduleSettings.SortProducts.SortBy = int.Parse(lstCPLSortBy.SelectedItem.Value);
                    _moduleSettings.SortProducts.SortDir = lstCPLSortDir.SelectedItem.Value;
                }

				// Detail settings
                if (_moduleSettings.General.ShowProductDetail)
                {
                    _moduleSettings.ProductDetail.Template = lstDetailTemplate.SelectedItem.Text;
                    _moduleSettings.ProductDetail.CartWarning = chkDetailCartWarning.Checked;
                    _moduleSettings.ProductDetail.ShowThumbnail = chkDetailShowThumbnail.Checked;
                    _moduleSettings.ProductDetail.ThumbnailWidth = int.Parse(txtDetailThumbnailWidth.Text);
                    _moduleSettings.ProductDetail.GIFBgColor = txtDetailGIFBgColor.Text;
                    _moduleSettings.ProductDetail.ShowReviews = chkDetailShowReviews.Checked;
                    _moduleSettings.ProductDetail.ReturnPage = int.Parse(lstPDSReturnPage.SelectedItem.Value);
                }

                // Also Bought list settings
                if (_moduleSettings.General.ShowAlsoBought)
                {
                    _moduleSettings.AlsoBoughtProducts.ContainerTemplate = lstABPLContainerTemplate.SelectedItem.Text;
                    _moduleSettings.AlsoBoughtProducts.Template = lstABPLTemplate.SelectedItem.Text;
                    _moduleSettings.AlsoBoughtProducts.RowCount = int.Parse(txtABPLRowCount.Text);
                    _moduleSettings.AlsoBoughtProducts.ColumnCount = int.Parse(txtABPLColumnCount.Text);
                    _moduleSettings.AlsoBoughtProducts.ColumnWidth = int.Parse(txtABPLColumnWidth.Text);
                    _moduleSettings.AlsoBoughtProducts.RepeatDirection = lstABPLRepeatDirection.SelectedItem.Value;
                    _moduleSettings.AlsoBoughtProducts.ShowThumbnail = chkABPLShowThumbnail.Checked;
                    _moduleSettings.AlsoBoughtProducts.ThumbnailWidth = int.Parse(txtABPLThumbnailWidth.Text);
                    _moduleSettings.AlsoBoughtProducts.GIFBgColor = txtABPLGIFBgColor.Text;
                    _moduleSettings.AlsoBoughtProducts.DetailPage = int.Parse(lstABPLDetailPage.SelectedItem.Value);
                }

                // New list settings
                if (_moduleSettings.General.ShowNewProducts)
                {
                    _moduleSettings.NewProducts.ContainerTemplate = lstNPLContainerTemplate.SelectedItem.Text;
                    _moduleSettings.NewProducts.Template = lstNPLTemplate.SelectedItem.Text;
                    _moduleSettings.NewProducts.RowCount = int.Parse(txtNPLRowCount.Text);
                    _moduleSettings.NewProducts.ColumnCount = int.Parse(txtNPLColumnCount.Text);
                    _moduleSettings.NewProducts.ColumnWidth = int.Parse(txtNPLColumnWidth.Text);
                    _moduleSettings.NewProducts.RepeatDirection = lstNPLRepeatDirection.SelectedItem.Value;
                    _moduleSettings.NewProducts.DisplayByCategory = chkNPLDisplayByCategory.Checked;
                    _moduleSettings.NewProducts.ShowThumbnail = chkNPLShowThumbnail.Checked;
                    _moduleSettings.NewProducts.ThumbnailWidth = int.Parse(txtNPLThumbnailWidth.Text);
                    _moduleSettings.NewProducts.GIFBgColor = txtNPLGIFBgColor.Text;
                    _moduleSettings.NewProducts.DetailPage = int.Parse(lstNPLDetailPage.SelectedItem.Value);
                }

				// Featured list settings
                if (_moduleSettings.General.ShowFeaturedProducts)
                {
                    _moduleSettings.FeaturedProducts.ContainerTemplate = lstFPLContainerTemplate.SelectedItem.Text;
                    _moduleSettings.FeaturedProducts.Template = lstFPLTemplate.SelectedItem.Text;
                    _moduleSettings.FeaturedProducts.RowCount = int.Parse(txtFPLRowCount.Text);
                    _moduleSettings.FeaturedProducts.ColumnCount = int.Parse(txtFPLColumnCount.Text);
                    _moduleSettings.FeaturedProducts.ColumnWidth = int.Parse(txtFPLColumnWidth.Text);
                    _moduleSettings.FeaturedProducts.RepeatDirection = lstFPLRepeatDirection.SelectedItem.Value;
                    _moduleSettings.FeaturedProducts.DisplayByCategory = chkFPLDisplayByCategory.Checked;
                    _moduleSettings.FeaturedProducts.ShowThumbnail = chkFPLShowThumbnail.Checked;
                    _moduleSettings.FeaturedProducts.ThumbnailWidth = int.Parse(txtFPLThumbnailWidth.Text);
                    _moduleSettings.FeaturedProducts.GIFBgColor = txtFPLGIFBgColor.Text;
                    _moduleSettings.FeaturedProducts.DetailPage = int.Parse(lstFPLDetailPage.SelectedItem.Value);
                }

				// Popular list settings
                if (_moduleSettings.General.ShowPopularProducts)
                {
                    _moduleSettings.PopularProducts.ContainerTemplate = lstPPLContainerTemplate.SelectedItem.Text;
                    _moduleSettings.PopularProducts.Template = lstPPLTemplate.SelectedItem.Text;
                    _moduleSettings.PopularProducts.RowCount = int.Parse(txtPPLRowCount.Text);
                    _moduleSettings.PopularProducts.ColumnCount = int.Parse(txtPPLColumnCount.Text);
                    _moduleSettings.PopularProducts.ColumnWidth = int.Parse(txtPPLColumnWidth.Text);
                    _moduleSettings.PopularProducts.RepeatDirection = lstPPLRepeatDirection.SelectedItem.Value;
                    _moduleSettings.PopularProducts.ShowThumbnail = chkPPLShowThumbnail.Checked;
                    _moduleSettings.PopularProducts.ThumbnailWidth = int.Parse(txtPPLThumbnailWidth.Text);
                    _moduleSettings.PopularProducts.GIFBgColor = txtPPLGIFBgColor.Text;
                    _moduleSettings.PopularProducts.DetailPage = int.Parse(lstPPLDetailPage.SelectedItem.Value);
                }

                // Update cached settings
                _moduleSettings.UpdateCache(ModuleId, TabId);
			}
			catch(Exception ex)
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		#endregion

		#region Private Methods

		private void LoadTemplates()
		{
            List<TemplateInfo> templates = TemplateController.GetTemplates(_templatePath);

			foreach (TemplateInfo templateInfo in templates)
			{
                lstTemplate.Items.Add(new ListItem(templateInfo.Name, templateInfo.Path));
                lstNPLContainerTemplate.Items.Add(new ListItem(templateInfo.Name, templateInfo.Path));
                lstNPLTemplate.Items.Add(new ListItem(templateInfo.Name, templateInfo.Path));
                lstFPLContainerTemplate.Items.Add(new ListItem(templateInfo.Name, templateInfo.Path));
                lstFPLTemplate.Items.Add(new ListItem(templateInfo.Name, templateInfo.Path));
                lstPPLContainerTemplate.Items.Add(new ListItem(templateInfo.Name, templateInfo.Path));
                lstPPLTemplate.Items.Add(new ListItem(templateInfo.Name, templateInfo.Path));
                lstCPLContainerTemplate.Items.Add(new ListItem(templateInfo.Name, templateInfo.Path));
                lstCPLTemplate.Items.Add(new ListItem(templateInfo.Name, templateInfo.Path));
                lstSPLTemplate.Items.Add(new ListItem(templateInfo.Name, templateInfo.Path));
                lstDetailTemplate.Items.Add(new ListItem(templateInfo.Name, templateInfo.Path));
                lstABPLContainerTemplate.Items.Add(new ListItem(templateInfo.Name, templateInfo.Path));
                lstABPLTemplate.Items.Add(new ListItem(templateInfo.Name, templateInfo.Path));
			}
		}

        private int ComputeSearchColumns()
        {
            int searchColumns = 0;
            if (chkSearchManufacturer.Checked)
                searchColumns += (int)SearchColumn.Manufacturer;
            if (chkSearchModelNumber.Checked)
                searchColumns += (int)SearchColumn.ModelNumber;
            if (chkSearchModelName.Checked)
                searchColumns += (int)SearchColumn.ModelName;
            if (chkSearchSummary.Checked)
                searchColumns += (int)SearchColumn.ProductSummay;
            if (chkSearchDescription.Checked)
                searchColumns += (int)SearchColumn.ProductDescription;
            return searchColumns;
        }

        private void DisplaySearchRows()
        {
            if (ComputeSearchColumns() == 0)
            {
                trSearchColumn.Visible = false;
                trSearchTemplate.Visible = false;
            }
            else
            {
                trSearchColumn.Visible = true;
                trSearchTemplate.Visible = true;
            }
        }

        private int ComputeSortColumns()
        {
            int sortColumns = 0;
            if (chkSortManufacturer.Checked)
                sortColumns += (int)SortColumn.Manufacturer;
            if (chkSortModelNumber.Checked)
                sortColumns += (int)SortColumn.ModelNumber;
            if (chkSortModelName.Checked)
                sortColumns += (int)SortColumn.ModelName;
            if (chkSortUnitPrice.Checked)
                sortColumns += (int)SortColumn.UnitPrice;
            if (chkSortCreatedDate.Checked)
                sortColumns += (int)SortColumn.Date;
            return sortColumns;
        }

        private void DisplaySortRows()
        {
            if (ComputeSortColumns() == 0)
            {
                trSortBy.Visible = false;
                trSortDir.Visible = false;
            }
            else
            {
                trSortBy.Visible = true;
                trSortDir.Visible = true;
            }
        }

		#endregion
	}
}
