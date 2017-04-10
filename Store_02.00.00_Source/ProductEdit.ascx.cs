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
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.Store.Catalog;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Providers;

namespace DotNetNuke.Modules.Store.WebControls
{
	public partial class ProductEdit : StoreControlBase
	{
		#region Private Members

		private CatalogNavigation _nav = null;
        private int _copyFrom = Null.NullInteger;
		
		#endregion

        #region Properties

        public int CopyFrom
        {
            get { return _copyFrom; }
            set { _copyFrom = value; }
        }

        #endregion

        #region Events

        override protected void OnInit(EventArgs e)
		{
            cmdSuggest.Click += cmdSuggest_Click;
            cmdUpdate.Click += cmdUpdate_Click;
            cmdCancel.Click += cmdCancel_Click;
            cmdDelete.Click += cmdDelete_Click;
			base.OnInit(e);
		}
		
		protected void Page_Load(object sender, EventArgs e)
		{
			try 
			{
				// Get the navigation settings
				_nav = new CatalogNavigation(Request.QueryString);
				_nav.ProductID = (int)DataSource;

				// Handle ProductID=0 as Null.NullInteger
				if (_nav.ProductID == 0)
					_nav.ProductID = Null.NullInteger;

                if (Page.IsPostBack == false)
                {
                    // Get category list
                    CategoryController categoryController = new CategoryController();
                    List<CategoryInfo> categories = categoryController.GetCategoriesPath(PortalId, true, Null.NullInteger);

                    // If no category exists, display a warning message
                    if (categories == null || categories.Count == 0)
                    {
                        pnlCategoriesRequired.Visible = true;
                        tblProductForm.Visible = false;
                    }
                    else
                    {
                        pnlCategoriesRequired.Visible = false;
                        // Bind categories and add 'select' choice to the dropdown
                        cmbCategory.DataSource = categories;
                        cmbCategory.DataBind();
                        cmbCategory.Items.Insert(0, new ListItem(Localization.GetString("SelectComboValue", LocalResourceFile), "-1"));

                        // Allowed file extentions
                        imgProduct.FileFilter = "bmp,png,jpg,jpeg,gif";

                        // Are we editing or creating new item?
                        if (_nav.ProductID != Null.NullInteger || _copyFrom != Null.NullInteger)
                        {
                            ProductController controller = new ProductController();
                            ProductInfo product = controller.GetProduct(PortalId, _nav.ProductID == Null.NullInteger ? _copyFrom : _nav.ProductID);

                            if (product != null)
                            {
                                if (_copyFrom == Null.NullInteger)
                                {
                                    // Set delete confirmation
                                    cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");
                                    cmdDelete.Visible = true;
                                }
                                // Define fields values
                                cmbCategory.SelectedValue = product.CategoryID.ToString();
                                txtManufacturer.Text = product.Manufacturer;
                                txtModelNumber.Text = product.ModelNumber;
                                txtModelName.Text = product.ModelName;
                                if (StoreSettings.SEOFeature)
                                {
                                    txtSEOName.Text = product.SEOName;
                                    txtKeywords.Text = product.Keywords;
                                }
                                else
                                {
                                    trSEOName.Visible = false;
                                    trKeywords.Visible = false;
                                }
                                txtSummary.Text = product.Summary;
                                txtRegularPrice.Text = product.RegularPrice.ToString("0.00");
                                txtUnitPrice.Text = product.UnitCost.ToString("0.00");
                                if (StoreSettings.CheckoutMode == CheckoutType.Registred && StoreSettings.AllowVirtualProducts)
                                    chkVirtualProduct.Checked = product.IsVirtual;
                                else
                                    trVirtualProduct.Visible = false;
                                if (chkVirtualProduct.Checked)
                                {
                                    trVirtualProductSection.Visible = true;
                                    trProductDimensions.Visible = false;
                                    if (product.VirtualFileID == Null.NullInteger)
                                    {
                                        urlProductFile.UrlType = "N";
                                    }
                                    else
                                    {
                                        urlProductFile.Url = "FileID=" + product.VirtualFileID;
                                        urlProductFile.UrlType = "F";
                                    }
                                    txtAllowedDownloads.Text = product.AllowedDownloads.ToString();
                                }
                                else
                                {
                                    trVirtualProductSection.Visible = false;
                                    trProductDimensions.Visible = true;
                                    txtUnitWeight.Text = product.ProductWeight == Null.NullDecimal ? string.Empty : product.ProductWeight.ToString("0.00");
                                    txtUnitHeight.Text = product.ProductHeight == Null.NullDecimal ? string.Empty : product.ProductHeight.ToString("0.00");
                                    txtUnitLength.Text = product.ProductLength == Null.NullDecimal ? string.Empty : product.ProductLength.ToString("0.00");
                                    txtUnitWidth.Text = product.ProductWidth == Null.NullDecimal ? string.Empty : product.ProductWidth.ToString("0.00");
                                }
                                if (StoreSettings.InventoryManagement)
                                {
                                    txtStockQuantity.Text = product.StockQuantity.ToString();
                                    txtLowThreshold.Text = product.LowThreshold.ToString();
                                    txtHighThreshold.Text = product.HighThreshold.ToString();
                                    txtDeliveryTime.Text = product.DeliveryTime.ToString();
                                    txtPurchasePrice.Text = product.PurchasePrice.ToString("0.00");
                                }
                                else
                                    trStockManagement.Visible = false;
                                chkArchived.Checked = product.Archived;

                                LoadRole(product.RoleID);

                                bool isFeatured = product.Featured;
                                chkFeatured.Checked = isFeatured;
                                trFeatured.Visible = isFeatured;
                                dshSpecialOffer.IsExpanded = isFeatured;
                                txtSalePrice.Text = product.SalePrice == Null.NullDecimal ? string.Empty : product.SalePrice.ToString("0.00");
                                txtSaleStartDate.Text = product.SaleStartDate != Null.NullDate ? product.SaleStartDate.ToString("d") : "";
                                txtSaleEndDate.Text = product.SaleEndDate != Null.NullDate ? product.SaleEndDate.ToString("d") : "";
                                if (isFeatured)
                                    MakeCalendars();

                                if (string.IsNullOrEmpty(product.ProductImage) == false)
                                {
                                    if (product.ProductImage.StartsWith("http://") || product.ProductImage.StartsWith("https://"))
                                    {
                                        imgProduct.Url = product.ProductImage;
                                        imgProduct.UrlType = "U";
                                    }
                                    else
                                    {
                                        imgProduct.Url = FileSystemHelper.GetUrlFileID(product.ProductImage, PortalSettings);
                                        imgProduct.UrlType = "F";
                                    }
                                }
                                txtDescription.Text = product.Description;
                            }
                            else
                            {
                                // Handle as new item
                                PrepareNew();
                            }
                        }
                        else
                        {
                            // Handle as new item
                            PrepareNew();
                        }
                    }
                }
            } 
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            // Test Trust Level and display warning if needed
            TestTrustLevel();
        }

        protected void cmdSuggest_Click(object sender, EventArgs e)
        {
            txtSEOName.Text = Normalization.RemoveDiacritics(txtModelName.Text, '-').ToLower();
        }

        protected void chkVirtualProduct_CheckedChanged(object sender, EventArgs e)
        {
            trVirtualProductSection.Visible = chkVirtualProduct.Checked;
            trProductDimensions.Visible = !chkVirtualProduct.Checked;
        }

        protected void chkFeatured_CheckedChanged(object sender, EventArgs e)
        {
            trFeatured.Visible = chkFeatured.Checked;
            if (chkFeatured.Checked)
            {
                dshSpecialOffer.IsExpanded = true;
                MakeCalendars();
            }
        }

		protected void cmdUpdate_Click(object sender, EventArgs e)
		{
			try 
			{
                if (chkVirtualProduct.Checked && urlProductFile.UrlType == "N")
                {
                    trErrorProductFile.Visible = true;
                    return;
                }

                trErrorProductFile.Visible = false;

				if (Page.IsValid) 
				{
                    ProductInfo product = new ProductInfo();

                    product.ProductID = _nav.ProductID;
                    product.PortalID = PortalId;
                    product.CategoryID = int.Parse(cmbCategory.SelectedValue);
                    product.Manufacturer = txtManufacturer.Text;
                    product.ModelNumber = txtModelNumber.Text;
                    product.ModelName = txtModelName.Text;
                    if (StoreSettings.SEOFeature)
                    {
                        product.SEOName = txtSEOName.Text;
                        product.Keywords = txtKeywords.Text;
                    }
                    product.Summary = txtSummary.Text;
                    product.RegularPrice = Decimal.Parse(txtRegularPrice.Text);
                    product.UnitCost = Decimal.Parse(txtUnitPrice.Text);
                    product.IsVirtual = chkVirtualProduct.Checked;
                    if (product.IsVirtual)
                    {
                        product.VirtualFileID = GetProductFileID();
                        product.AllowedDownloads = int.Parse(txtAllowedDownloads.Text);
                        product.ProductWeight = 0;
                        product.ProductHeight = 0;
                        product.ProductLength = 0;
                        product.ProductWidth = 0;
                    }
                    else
                    {
                        product.ProductWeight = Decimal.Parse(txtUnitWeight.Text);
                        product.ProductHeight = Decimal.Parse(txtUnitHeight.Text);
                        product.ProductLength = Decimal.Parse(txtUnitLength.Text);
                        product.ProductWidth = Decimal.Parse(txtUnitWidth.Text);
                    }
                    if (StoreSettings.InventoryManagement)
                    {
                        product.StockQuantity = int.Parse(txtStockQuantity.Text);
                        product.LowThreshold = int.Parse(txtLowThreshold.Text);
                        product.HighThreshold = int.Parse(txtHighThreshold.Text);
                        product.DeliveryTime = int.Parse(txtDeliveryTime.Text);
                        product.PurchasePrice = Decimal.Parse(txtPurchasePrice.Text);
                    }
                    else
                    {
                        product.StockQuantity = 0;
                        product.LowThreshold = 0;
                        product.HighThreshold = 0;
                        product.DeliveryTime = 0;
                        product.PurchasePrice = 0;
                    }
                    product.Archived = chkArchived.Checked;
                    if (StoreSettings.CheckoutMode == CheckoutType.Registred)
                        product.RoleID = int.Parse(lstRole.SelectedValue);
                    product.Featured = chkFeatured.Checked;
                    product.SalePrice = txtSalePrice.Text.Length > 0 ? Decimal.Parse(txtSalePrice.Text) : Null.NullDecimal;
                    string saleDate = txtSaleStartDate.Text;
                    product.SaleStartDate = product.Featured && !Null.IsNull(saleDate) ? Convert.ToDateTime(saleDate) : Null.NullDate;
                    saleDate = txtSaleEndDate.Text;
                    product.SaleEndDate = product.Featured && !Null.IsNull(saleDate) ? Convert.ToDateTime(saleDate) : Null.NullDate;
                    product.ProductImage = GetImageUrl();
                    product.Description = txtDescription.Text;
                    product.CreatedByUser = UserId.ToString();
                    product.CreatedDate = DateTime.Now;

					ProductController controller = new ProductController();
					if (product.ProductID == Null.NullInteger)
						controller.AddProduct(product);
					else 
                        controller.UpdateProduct(product);
					InvokeEditComplete();
				}
			} 
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		protected void cmdCancel_Click(object sender, EventArgs e)
		{
			_nav.ProductID = Null.NullInteger;
			InvokeEditComplete();
		}

		protected void cmdDelete_Click(object sender, EventArgs e)
		{
			if (_nav.ProductID != Null.NullInteger) 
			{
				ProductController controller = new ProductController();
				controller.DeleteProduct(_nav.ProductID);
				_nav.ProductID = Null.NullInteger;
			}
			InvokeEditComplete();
		}

		#endregion

		#region Private Methods

		private void PrepareNew()
		{
			// Do we have a category to use as a default?
			if (_nav.CategoryID != Null.NullInteger)
				cmbCategory.SelectedValue = _nav.CategoryID.ToString();

            if (!StoreSettings.SEOFeature)
            {
                trSEOName.Visible = false;
                trKeywords.Visible = false;
            }

            if (StoreSettings.CheckoutMode != CheckoutType.Registred)
                trVirtualProduct.Visible = false;

            trVirtualProductSection.Visible = false;
            urlProductFile.UrlType = "N";
            txtAllowedDownloads.Text = "-1";
            txtRegularPrice.Text = (0D).ToString("0.00");
            txtUnitPrice.Text = (0D).ToString("0.00");
            txtUnitWeight.Text = (0D).ToString("0.00");
            txtUnitHeight.Text = (0D).ToString("0.00");
            txtUnitLength.Text = (0D).ToString("0.00");
            txtUnitWidth.Text = (0D).ToString("0.00");
            if (StoreSettings.InventoryManagement)
            {
                txtStockQuantity.Text = "0";
                txtLowThreshold.Text = "0";
                txtHighThreshold.Text = "0";
                txtDeliveryTime.Text = "0";
                txtPurchasePrice.Text = (0D).ToString("0.00");
            }
            else
                trStockManagement.Visible = false;
            LoadRole(Null.NullInteger);
            trFeatured.Visible = false;
            txtSalePrice.Text = (0D).ToString("0.00");
        }

        private int GetProductFileID()
        {
            if (urlProductFile.UrlType == "N")
                return Null.NullInteger;

            return int.Parse(urlProductFile.Url.Substring(7));
        }

		private string GetImageUrl()
		{
            string imagePath = string.Empty;

            switch (imgProduct.UrlType)
            {
                case "F":
                    imagePath = FileSystemHelper.GetFilePath(imgProduct.Url, PortalSettings);
                    break;
                case "U":
                    imagePath = imgProduct.Url;
                    break;
            }

			return imagePath;
		}

        private void LoadRole(int roleID)
        {
            if (StoreSettings.CheckoutMode == CheckoutType.Registred)
            {
                RoleController roleController = new RoleController();
                ArrayList portalRoles = roleController.GetPortalRoles(PortalId);
                string chooseItem = Localization.GetString("EmptyComboValue", LocalResourceFile);

                lstRole.Items.Insert(0, new ListItem(chooseItem, Null.NullInteger.ToString()));
                lstRole.AppendDataBoundItems = true;
                lstRole.DataValueField = "RoleID";
                lstRole.DataTextField = "RoleName";
                lstRole.DataSource = portalRoles;
                lstRole.DataBind();
                if (roleID != Null.NullInteger)
                {
                    ListItem orderRoleItem = lstRole.Items.FindByValue(roleID.ToString());
                    if (orderRoleItem != null)
                        orderRoleItem.Selected = true;
                }
            }
            else
                trProductRole.Visible = false;
        }

        private void TestTrustLevel()
        {
            if (imgProduct.UrlType == "U" && TrustHelper.IsFullTrust == false)
            {
                trWarningTrustLevel.Visible = true;
                lblWarningTrustLevel.Text = string.Format(Localization.GetString("WarningTrustLevel", LocalResourceFile), TrustHelper.TrustLevel);
            }
            else
                trWarningTrustLevel.Visible = false;
        }

        private void MakeCalendars()
        {
            string localizedCalendarText = Localization.GetString("Calendar");
            string calendarText = "<img src='" + ResolveUrl("~/images/calendar.png") + "' border='0' alt='" + localizedCalendarText + "'>&nbsp;" + localizedCalendarText;

            cmdSaleStartDate.NavigateUrl = Common.Utilities.Calendar.InvokePopupCal(txtSaleStartDate);
            cmdSaleStartDate.Text = calendarText;

            cmdSaleEndDate.NavigateUrl = Common.Utilities.Calendar.InvokePopupCal(txtSaleEndDate);
            cmdSaleEndDate.Text = calendarText;
        }

		#endregion
    }
}
