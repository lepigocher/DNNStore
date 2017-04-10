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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Catalog;
using DotNetNuke.Modules.Store.Cart;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Providers.Tax;

namespace DotNetNuke.Modules.Store.WebControls
{
	public partial class CartDetail : StoreControlBase
	{
        #region Private Members

		private decimal _cartTotal;
		private int _itemsCount;
        private readonly NumberFormatInfo _localFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
        private decimal _defaultTaxRate;
        private bool _showTax;
        private ProductController _productControler;
        private bool _showThumbnail;
        private int _thumbnailWidth;
        private string _gifBgColor;
        private string _productColumn;
        private bool _linkToDetail;
        private bool _includeVAT;
        private bool _enableImageCaching;
        private int _cacheImageDuration;
        private string _templatePath;
        private string _linkTitle;
        private string _imageUpdate;
        private string _imageDelete;

        #endregion

        #region Properties

        public bool ShowThumbnail
        {
            get { return _showThumbnail; }
            set { _showThumbnail = value; }
        }

        public int ThumbnailWidth
        {
            get { return _thumbnailWidth; }
            set { _thumbnailWidth = value; }
        }

        public string GIFBgColor
        {
            get { return _gifBgColor; }
            set { _gifBgColor = value; }
        }

        public string ProductColumn
        {
            get { return _productColumn; }
            set { _productColumn = value; }
        }

        public bool LinkToDetail
        {
            get { return _linkToDetail; }
            set { _linkToDetail = value; }
        }

        public bool IncludeVAT
        {
            get { return _includeVAT; }
            set { _includeVAT = value; }
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

        public string TemplatePath
        {
            get { return _templatePath; }
            set { _templatePath = value; }
        }

        public int ItemsCount
        {
            get { return _itemsCount; }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
		{
            if (StoreSettings != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(StoreSettings.CurrencySymbol))
                        _localFormat.CurrencySymbol = StoreSettings.CurrencySymbol;

                    _imageUpdate = _templatePath + "images/" + Localization.GetString("ImageUpdate", LocalResourceFile);
                    _imageDelete = _templatePath + "images/" + Localization.GetString("ImageDelete", LocalResourceFile);

                    ITaxProvider taxProvider = StoreController.GetTaxProvider(StoreSettings.TaxName);
                    ITaxInfo taxInfo = taxProvider.GetDefautTaxRates(PortalId);
                    _defaultTaxRate = taxInfo.DefaultTaxRate;
                    _showTax = taxInfo.ShowTax;

                    if (_linkToDetail)
                    {
                        _linkTitle = Localization.GetString("lnkTitle", LocalResourceFile);
                        if (StoreSettings.SEOFeature)
                            _productControler = new ProductController();
                    }

                    if (IsPostBack == false)
                        UpdateCartGrid();
                }
                catch (Exception ex)
                {
                    Exceptions.ProcessModuleLoadException(this, ex);
                }
            }
		}

		protected void grdItems_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
                ItemInfo itemInfo = (ItemInfo)e.Item.DataItem;

                if (_showThumbnail)
                {
                    Image imgThumbnail = (Image)e.Item.FindControl("imgThumbnail");
                    if (imgThumbnail != null)
                    {
                        imgThumbnail.ImageUrl = GetImageUrl(itemInfo.ProductImage);
                        imgThumbnail.AlternateText = itemInfo.ProductTitle;
                        imgThumbnail.Width = _thumbnailWidth;
                    }
                }

                string productTitle = null;
                switch (_productColumn)
                {
                    case "modelnumber":
                        productTitle = itemInfo.ModelNumber;
                        break;
                    case "modelname":
                        productTitle = itemInfo.ModelName;
                        break;
                    case "producttitle":
                        productTitle = itemInfo.ProductTitle;
                        break;
                }

                if (_linkToDetail)
                {
                    HtmlAnchor lnkTitle = (HtmlAnchor)e.Item.FindControl("lnkTitle");
                    if (lnkTitle != null)
                    {
                        StringDictionary rplcTitle = new StringDictionary();
                        rplcTitle.Add("ProductID", itemInfo.ProductID.ToString());
                        if (StoreSettings.SEOFeature)
                        {
                            ProductInfo product = _productControler.GetProduct(PortalId, itemInfo.ProductID);
                            rplcTitle.Add("Product", product.SEOName);
                        }

                        CatalogNavigation catalogNav = new CatalogNavigation();
                        lnkTitle.InnerText = productTitle;
                        lnkTitle.Title = _linkTitle;
                        lnkTitle.HRef = catalogNav.GetNavigationUrl(StoreSettings.StorePageID, rplcTitle);
                        lnkTitle.Visible = true;
                    }
                }
                else
                {
                    Label lblTitle = (Label)e.Item.FindControl("lblTitle");
                    if (lblTitle != null)
                    {
                        lblTitle.Text = productTitle;
                        lblTitle.Visible = true;
                    }
                }

				Label lblPrice = (Label)e.Item.FindControl("lblPrice");
				if (lblPrice != null)
				{
                    decimal unitCost = itemInfo.UnitCost;
                    if (_showTax && _includeVAT)
                    {
                        unitCost = (unitCost + (unitCost * (_defaultTaxRate / 100)));
                    }
                    lblPrice.Text = (unitCost).ToString("C", _localFormat);
                }

				Label lblSubtotal = (Label)e.Item.FindControl("lblSubtotal");
				if (lblSubtotal != null)
				{
                    decimal subTotal = itemInfo.SubTotal;
                    if (_showTax && _includeVAT)
                    {
                        subTotal = (subTotal + (subTotal * (_defaultTaxRate / 100)));
                    }
                    lblSubtotal.Text = (subTotal).ToString("C", _localFormat);
                    _cartTotal += subTotal;
					_itemsCount += itemInfo.Quantity;
				}

                TextBox txtQty = (TextBox)e.Item.FindControl("txtQuantity");
                if (txtQty != null)
                    txtQty.Text = itemInfo.Quantity.ToString();

                CustomValidator valCustQuantity = (CustomValidator)e.Item.FindControl("valCustQuantity");
                if (valCustQuantity != null)
                {
                    if (StoreSettings.InventoryManagement && StoreSettings.AvoidNegativeStock)
                    {
                        valCustQuantity.Attributes.Add("ItemID", itemInfo.ItemID.ToString());
                    }
                    else
                        valCustQuantity.Visible = false;
                }

                ImageButton ibUpdate = (ImageButton)e.Item.FindControl("ibUpdate");
                if (ibUpdate != null)
                {
                    ibUpdate.ImageUrl = _imageUpdate;
                    ibUpdate.CommandArgument = e.Item.ItemIndex.ToString();
                    ibUpdate.CommandName = "Update";
                }

                if (txtQty != null && ibUpdate != null)
                    txtQty.Attributes.Add("onkeydown", "javascript:if((event.which && event.which == 13) || (event.keyCode && event.keyCode == 13)){document.getElementById('" + ibUpdate.ClientID + "').click();return false;}else return true;");

                ImageButton ibDelete = (ImageButton)e.Item.FindControl("ibDelete");
                if (ibDelete != null)
                {
                    ibDelete.ImageUrl = _imageDelete;
                    ibDelete.CommandArgument = e.Item.ItemIndex.ToString();
                    ibDelete.CommandName = "Delete";
                }
			}
			else if (e.Item.ItemType == ListItemType.Footer)
			{
				Label lblCount = (Label)e.Item.FindControl("lblCount");
				if (lblCount != null)
					lblCount.Text = _itemsCount.ToString();

				Label lblTotal = (Label)e.Item.FindControl("lblTotal");
				if (lblTotal != null)
                    lblTotal.Text = _cartTotal.ToString("C", _localFormat);
            }
		}

        protected void valCustQuantity_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (StoreSettings.InventoryManagement && StoreSettings.AvoidNegativeStock)
            {
                CustomValidator valCustQuantity = (CustomValidator)source;
                int quantity;

                if (int.TryParse(args.Value, out quantity))
                {
                    string itemID = valCustQuantity.Attributes["ItemID"];
                    ItemInfo itemInfo = CurrentCart.GetItem(int.Parse(itemID));
                    ProductController controler = new ProductController();
                    ProductInfo currentProduct = controler.GetProduct(PortalId, itemInfo.ProductID);

                    if (currentProduct.StockQuantity < quantity)
                    {
                        if (quantity > 1)
                            valCustQuantity.ErrorMessage = string.Format(Localization.GetString("ErrorQuantityPlural", LocalResourceFile), currentProduct.StockQuantity, currentProduct.ProductTitle);
                        else if (quantity == 1)
                            valCustQuantity.ErrorMessage = string.Format(Localization.GetString("ErrorQuantitySingular", LocalResourceFile), currentProduct.ProductTitle);
                        else
                            valCustQuantity.ErrorMessage = string.Format(Localization.GetString("ErrorQuantityNegative", LocalResourceFile), currentProduct.ProductTitle);
                        args.IsValid = false;
                    }
                }
                else
                {
                    valCustQuantity.ErrorMessage = Localization.GetString("ErrorNotNumeric", LocalResourceFile);
                    args.IsValid = false;
                }
            }
        }

        protected void grdItems_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            int itemID;
            int quantity;
            switch (e.CommandName)
            {
                case "Update":
                    itemID = (int)grdItems.DataKeys[Convert.ToInt32(e.CommandArgument)];
                    quantity = 0;
                    TextBox txtQty = (TextBox)((WebControl)e.CommandSource).Parent.FindControl("txtQuantity");
                    if (txtQty != null)
                    {
                        string qty = txtQty.Text;
                        if (string.IsNullOrEmpty(qty))
                            qty = "0";
                        if (int.TryParse(qty, out quantity))
                        {
                            if (quantity > 0)
                                CurrentCart.UpdateItem(PortalId, StoreSettings.SecureCookie, itemID, quantity);
                            else
                                CurrentCart.RemoveItem(itemID);
                            UpdateCartGrid();
                            Page.Validate("vgCart");
                            InvokeEditComplete();
                        }
                        else
                            Page.Validate("vgCart");
                    }
                    break;
                case "Delete":
                    itemID = (int)grdItems.DataKeys[Convert.ToInt32(e.CommandArgument)];
                    CurrentCart.RemoveItem(itemID);
                    UpdateCartGrid();
                    InvokeEditComplete();
                    break;
                case "UpdateCart":
                    Page.Validate("vgCart");
                    if (Page.IsValid)
                    {
                        foreach (DataGridItem dgItem in grdItems.Items)
                        {
                            itemID = (int)grdItems.DataKeys[dgItem.ItemIndex];
                            string value = ((TextBox)dgItem.FindControl("txtQuantity")).Text;

                            if (string.IsNullOrEmpty(value))
                                quantity = 0;
                            else
                                quantity = int.Parse(value);

                            if (quantity > 0)
                                CurrentCart.UpdateItem(PortalId, StoreSettings.SecureCookie, itemID, quantity);
                            else
                                CurrentCart.RemoveItem(itemID);
                        }
                        UpdateCartGrid();
                        InvokeEditComplete();
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

		#region Private Methods

		private void UpdateCartGrid()
		{
			_cartTotal = 0;
			_itemsCount = 0;
			List<ItemInfo> cartItems = CurrentCart.GetItems(PortalId, StoreSettings.SecureCookie);

            if (cartItems.Count == 0)
            {
                grdItems.Visible = false;
                lblCartEmpty.Visible = true;
            }
            else
            {
                grdItems.Visible = true;
                if (_showThumbnail)
                    grdItems.Columns[0].Visible = true;
                else
                    grdItems.Columns[0].Visible = false;
                grdItems.DataSource = cartItems;
			    grdItems.DataBind();
                lblCartEmpty.Visible = false;
            }
		}

		private string GetImageUrl(string image) 
		{
            if (_enableImageCaching)
                return ModulePath + "Thumbnail.ashx?IP=" + image + "&IW=" + _thumbnailWidth + "&BC=" + GIFBgColor + "&CD=" + _cacheImageDuration; // + "&portalID=" + PortalId.ToString();

            return ModulePath + "Thumbnail.ashx?IP=" + image + "&IW=" + _thumbnailWidth + "&BC=" + GIFBgColor; // +"&portalID=" + PortalId.ToString();
		}

		#endregion
	}
}
