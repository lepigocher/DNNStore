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

using DotNetNuke.Common;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

using DotNetNuke.Modules.Store.Core.Cart;
using DotNetNuke.Modules.Store.Core.Components;

namespace DotNetNuke.Modules.Store.WebControls
{
    public partial class MiniCart : StoreControlBase
    {
        #region Private Members

        private int _itemsCount;

        #endregion

		#region Events

		protected void Page_Load(object sender, EventArgs e)
		{
            if (StoreSettings != null)
            {
                try
                {
                    string templatePath = CssTools.GetTemplatePath(this, StoreSettings.PortalTemplates);
                    CssTools.AddCss(Page, templatePath, StoreSettings.StyleSheet);

                    // Read module settings and define cart properties
                    Core.Cart.MiniCartSettings cartSettings = new ModuleSettings(ModuleId, TabId).MiniCart;
                    cartControl.ShowThumbnail = cartSettings.ShowThumbnail;
                    cartControl.ThumbnailWidth = cartSettings.ThumbnailWidth;
                    cartControl.GIFBgColor = cartSettings.GIFBgColor;
                    cartControl.EnableImageCaching = cartSettings.EnableImageCaching;
                    cartControl.CacheImageDuration = cartSettings.CacheImageDuration;
                    cartControl.ProductColumn = cartSettings.ProductColumn.ToLower();
                    cartControl.LinkToDetail = cartSettings.LinkToDetail;
                    cartControl.IncludeVAT = cartSettings.IncludeVAT;
                    cartControl.TemplatePath = templatePath;
                    cartControl.ModuleConfiguration = ModuleConfiguration;
                    cartControl.StoreSettings = StoreSettings;
                    cartControl.EditComplete += cartControl_EditComplete;

                    _itemsCount = CurrentCart.GetItems(PortalId, StoreSettings.SecureCookie).Count;
                    if (_itemsCount == 0)
                        phlViewCart.Visible = false;
                }
                catch(Exception ex)
                {
                    Exceptions.ProcessModuleLoadException(this, ex);
                }
            }
            else
            {
                if (UserInfo.IsSuperUser)
                {
                    string ErrorSettings = Localization.GetString("ErrorSettings", LocalResourceFile);
                    string ErrorSettingsHeading = Localization.GetString("ErrorSettingsHeading", LocalResourceFile);
                    UI.Skins.Skin.AddModuleMessage(this, ErrorSettingsHeading, ErrorSettings, UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning);
                }
                divControls.Visible = false;
            }
        }

        protected void btnViewCart_Click(object sender, EventArgs e)
		{
            Page.Validate();
            if (Page.IsValid)
                Response.Redirect(Globals.NavigateURL(StoreSettings.ShoppingCartPageID));
		}

        protected void cartControl_EditComplete(object sender, EventArgs e)
        {
            if (cartControl.ItemsCount == 0)
                phlViewCart.Visible = false;
        }

		#endregion
	}
}