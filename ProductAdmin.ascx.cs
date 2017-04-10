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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.Store.Catalog;
using DotNetNuke.Modules.Store.Components;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for Store.
	/// </summary>
	public partial class ProductAdmin : StoreControlBase, IStoreTabedControl
	{
		#region Private Members

        private readonly NumberFormatInfo _localFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
        private AdminNavigation _nav;

		#endregion

		#region Events

		override protected void OnInit(EventArgs e)
		{
			cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
			linkAddNew.Click += linkAddNew_Click;
			linkAddImage.Click += linkAddNew_Click;
			grdProducts.ItemDataBound += grdProducts_ItemDataBound;
			grdProducts.PageIndexChanged += grdProducts_PageIndexChanged;
			base.OnInit(e);
		}
		
		protected void Page_Load(object sender, EventArgs e)
		{
            if (StoreSettings.CurrencySymbol != string.Empty)
                _localFormat.CurrencySymbol = StoreSettings.CurrencySymbol;
            
			_nav = new AdminNavigation(Request.QueryString);

			// Do we show list or edit view?
			if (_nav.ProductID != Null.NullInteger || _nav.CopyProductID != Null.NullInteger)
				ShowEditControl();
			else
				ShowProductList();
		}

		private void grdProducts_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			ProductInfo productInfo = e.Item.DataItem as ProductInfo;

			if (productInfo != null)
			{
                Label labelArchived = (e.Item.FindControl("labelArchived") as Label);
                if (labelArchived != null)
                    labelArchived.Text = productInfo.Archived ? Localization.GetString("Yes", Localization.SharedResourceFile) : Localization.GetString("No", Localization.SharedResourceFile);

                Label labelFeatured = (e.Item.FindControl("labelFeatured") as Label);
                if (labelFeatured != null)
                    labelFeatured.Text = productInfo.Featured ? Localization.GetString("Yes", Localization.SharedResourceFile) : Localization.GetString("No", Localization.SharedResourceFile);

                HyperLink linkEdit = (e.Item.FindControl("linkEdit") as HyperLink);
                if (linkEdit != null)
                {
	                // Update navURL using this item's ID
	                StringDictionary replaceParams = new StringDictionary();
	                replaceParams["ProductID"] = productInfo.ProductID.ToString();
	                linkEdit.NavigateUrl = _nav.GetNavigationUrl(replaceParams);
                }

                HyperLink linkCopy = (e.Item.FindControl("linkCopy") as HyperLink);
                if (linkCopy != null)
                {
	                // Update navURL using this item's ID
	                StringDictionary replaceParams = new StringDictionary();
                    replaceParams["CopyProductID"] = productInfo.ProductID.ToString();
                    linkCopy.NavigateUrl = _nav.GetNavigationUrl(replaceParams);
                }

                Label labelPrice = (e.Item.FindControl("labelPrice") as Label);
                if (labelPrice != null)
                    labelPrice.Text = productInfo.UnitCost.ToString("C", _localFormat);
			}
		}

		private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
		{
			_nav.PageIndex = Null.NullInteger;
			_nav.CategoryID = int.Parse(cmbCategory.SelectedValue);
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		private void linkAddNew_Click(object sender, EventArgs e)
		{
			_nav.ProductID = 0;
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		private void productEdit_EditComplete(object sender, EventArgs e)
		{
			_nav.ProductID = Null.NullInteger;
            _nav.CopyProductID = Null.NullInteger;
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		private void grdProducts_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			_nav.PageIndex = e.NewPageIndex;
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		#endregion

		#region Private Methods

		private void ShowProductList()
		{
			panelList.Visible = true;
			panelEdit.Visible = false;

			// Load category combo
			CategoryController categoryController = new CategoryController();
            List<CategoryInfo> categoryList = categoryController.GetCategoriesPath(PortalId, true, Null.NullInteger);
            // Bind categories to combobox
			cmbCategory.DataSource = categoryList;
			cmbCategory.DataBind();
            if (StoreSettings.InventoryManagement)
            {
                // Add Low Stock choice
                ListItem categoryLowStock = new ListItem(Localization.GetString("LowStock", LocalResourceFile), "-2");
                cmbCategory.Items.Insert(0, categoryLowStock);
                // Add Out of Stock choice
                ListItem categoryOutOfStock = new ListItem(Localization.GetString("OutOfStock", LocalResourceFile), "-1");
                cmbCategory.Items.Insert(0, categoryOutOfStock);
            }
            else
                grdProducts.Columns[2].Visible = false;
			// Load product grid
			if (cmbCategory.Items.Count > 0)
			{
				// Do we need to select an existing category?
				if (_nav.CategoryID != Null.NullInteger)
					cmbCategory.SelectedValue = _nav.CategoryID.ToString();
				else
					cmbCategory.SelectedIndex = 0;
				UpdateProductList();
			}
		}

		private void UpdateProductList()
		{
            ProductController controller = new ProductController();
            List<ProductInfo> productList;
            
			// Get current category
			int categoryID = int.Parse(cmbCategory.SelectedValue);
            switch (categoryID)
            {
                case -2:
                    // Get Low Stock Product's list
                    productList = controller.GetPortalLowStockProducts(PortalId);
                    break;
                case -1:
                    // Get Out Of Stock Product's list
                    productList = controller.GetPortalOutOfStockProducts(PortalId);
                    break;
                default:
                    productList = controller.GetCategoryProducts(PortalId, categoryID, true, 2, "ASC");
                    break;
            }

            if (productList.Count > 0)
            {
                // Update the grid
                if (productList.Count <= grdProducts.PageSize)
                    grdProducts.AllowPaging = false;
                grdProducts.DataSource = productList;
                grdProducts.CurrentPageIndex = _nav.PageIndex == Null.NullInteger ? 0 : _nav.PageIndex;
                grdProducts.DataBind();
            }
		}

		private void ShowEditControl()
		{
			panelList.Visible = false;
			panelEdit.Visible = true;

			// Inject the edit control
			ProductEdit productEdit = (ProductEdit)LoadControl(ModulePath + "ProductEdit.ascx");
            productEdit.ModuleConfiguration = ModuleConfiguration;
            productEdit.StoreSettings = StoreSettings;
			productEdit.DataSource = _nav.ProductID;
            productEdit.CopyFrom = _nav.CopyProductID;
			productEdit.EditComplete += productEdit_EditComplete;
            productEdit.ID = "ProductEdit";
			editControl.Controls.Add(productEdit);
		}

		#endregion

        #region IStoreTabedControl Members

        string IStoreTabedControl.Title
        {
            get { return Localization.GetString("lblParentTitle", LocalResourceFile); }
        }

        #endregion
    }
}
