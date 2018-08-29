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
using System.Globalization;

using DotNetNuke.Services.Localization;

using DotNetNuke.Modules.Store.Core.Admin;
using DotNetNuke.Modules.Store.Core.Cart;
using DotNetNuke.Modules.Store.Core.Providers.Tax;

namespace DotNetNuke.Modules.Store.SkinObjects
{
    public partial class MicroCart : UI.Skins.SkinObjectBase
    {
        #region Private Members

        private string _itemsTitleCssClass = "NormalBold";
        private string _itemsCssClass = "Normal";
        private string _totalTitleCssClass = "NormalBold";
        private string _totalCssClass = "Normal";
        private bool _includeVAT;

        #endregion

        #region Properties

        public string ItemsTitleCssClass
        {
            get { return _itemsTitleCssClass; }
            set { _itemsTitleCssClass = value; }
        }

        public string ItemsCssClass
        {
            get { return _itemsCssClass; }
            set { _itemsCssClass = value; }
        }

        public string TotalTitleCssClass
        {
            get { return _totalTitleCssClass; }
            set { _totalTitleCssClass = value; }
        }

        public string TotalCssClass
        {
            get { return _totalCssClass; }
            set { _totalCssClass = value; }
        }

        public bool IncludeVAT
        {
            get { return _includeVAT; }
            set { _includeVAT = value; }
        }

        #endregion

        #region Events

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            int portalId = PortalSettings.PortalId;
            StoreInfo storeInfo = StoreController.GetStoreInfo(portalId);

            if (storeInfo != null)
            {
                NumberFormatInfo localFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
                string text;

                phControls.Visible = true;
                lblStoreMicroCartItemsTitle.CssClass = _itemsTitleCssClass;
                lblStoreMicroCartTotalTitle.CssClass = _totalTitleCssClass;
                lblStoreMicroCartItems.CssClass = _itemsCssClass;
                lblStoreMicroCartTotal.CssClass = _totalCssClass;

                string resource = TemplateSourceDirectory + "/App_LocalResources/MicroCart.ascx.resx";
                lblStoreMicroCartItemsTitle.Text = Localization.GetString("CartItemsTitle.Text", resource);
                lblStoreMicroCartTotalTitle.Text = Localization.GetString("CartTotalTitle.Text", resource);

                try
                {

                    CartInfo cartInfo = CurrentCart.GetInfo(portalId, storeInfo.SecureCookie);

                    if (!string.IsNullOrEmpty(storeInfo.CurrencySymbol))
                        localFormat.CurrencySymbol = storeInfo.CurrencySymbol;

                    ITaxProvider taxProvider = StoreController.GetTaxProvider(storeInfo.TaxName);
                    ITaxInfo taxInfo = taxProvider.GetDefautTaxRates(portalId);
                    bool showTax = taxInfo.ShowTax;
                    decimal defaultTaxRate = taxInfo.DefaultTaxRate;

                    text = Localization.GetString("CartItems.Text", resource);

                    if (cartInfo != null && cartInfo.Items > 0)
                    {
                        lblStoreMicroCartItems.Text = string.Format(text, cartInfo.Items);
                        decimal cartTotal = cartInfo.Total;
                        if (showTax && _includeVAT && cartInfo.Total > 0)
                        {
                            cartTotal = (cartTotal + (cartTotal * (defaultTaxRate / 100)));
                        }
                        lblStoreMicroCartTotal.Text = cartTotal.ToString("C", localFormat);
                    }
                    else
                    {
                        lblStoreMicroCartItems.Text = string.Format(text, 0);
                        lblStoreMicroCartTotal.Text = (0D).ToString("C", localFormat);
                    }
                }
                catch
                {
                    text = Localization.GetString("Error.Text", resource);
                    lblStoreMicroCartItems.Text = text;
                    lblStoreMicroCartTotalTitle.Text = text;
                }
            }
            else
                phControls.Visible = false;
        }

        #endregion
    }
}