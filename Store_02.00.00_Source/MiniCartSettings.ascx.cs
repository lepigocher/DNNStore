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
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.Store.Cart;

namespace DotNetNuke.Modules.Store.WebControls
{
    public partial class MiniCartSettings : ModuleSettingsBase
    {
        #region Private Members

        private ModuleSettings _cartSettings;

        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                _cartSettings = new ModuleSettings(ModuleId, TabId);
                if (!IsPostBack)
                {
                    lstProductColumn.Items.Add(new ListItem(Localization.GetString("ModelNumber", LocalResourceFile), "modelnumber"));
                    lstProductColumn.Items.Add(new ListItem(Localization.GetString("ModelName", LocalResourceFile), "modelname"));
                    lstProductColumn.Items.Add(new ListItem(Localization.GetString("ProductTitle", LocalResourceFile), "producttitle"));
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
            base.OnInit(e);
        }

        protected void chkShowThumbnail_CheckedChanged(object sender, EventArgs e)
        {
            trThumbnailWidth.Visible = chkShowThumbnail.Checked;
            trGIFBgColor.Visible = chkShowThumbnail.Checked;
            trEnableImageCaching.Visible = chkShowThumbnail.Checked;
            if (chkShowThumbnail.Checked && _cartSettings.MiniCart.EnableImageCaching)
                trCacheDuration.Visible = true;
            else
                trCacheDuration.Visible = false;
        }

        protected void chkEnableImageCaching_CheckedChanged(object sender, EventArgs e)
        {
            trCacheDuration.Visible = chkEnableImageCaching.Checked;
        }

        #endregion

        #region ModuleSettingsBase Implementation

        public override void LoadSettings()
        {
            try
            {
                chkShowThumbnail.Checked = _cartSettings.MiniCart.ShowThumbnail;
                txtThumbnailWidth.Text = _cartSettings.MiniCart.ThumbnailWidth.ToString();
                txtGIFBgColor.Text = _cartSettings.MiniCart.GIFBgColor;
                chkEnableImageCaching.Checked = _cartSettings.MiniCart.EnableImageCaching;
                txtCacheDuration.Text = _cartSettings.MiniCart.CacheImageDuration.ToString();
                if (_cartSettings.MiniCart.ShowThumbnail)
                {
                    trThumbnailWidth.Visible = true;
                    trGIFBgColor.Visible = true;
                    trEnableImageCaching.Visible = true;
                    if (_cartSettings.MiniCart.EnableImageCaching)
                        trCacheDuration.Visible = true;
                    else
                        trCacheDuration.Visible = false;
                }
                else
                {
                    trThumbnailWidth.Visible = false;
                    trGIFBgColor.Visible = false;
                    trEnableImageCaching.Visible = false;
                    trCacheDuration.Visible = false;
                }
                ListItem listItem = lstProductColumn.Items.FindByValue(_cartSettings.MiniCart.ProductColumn);
                if (listItem != null)
                    listItem.Selected = true;
                chkLinkToDetail.Checked = _cartSettings.MiniCart.LinkToDetail;
                chkIncludeVAT.Checked = _cartSettings.MiniCart.IncludeVAT;
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        public override void UpdateSettings()
        {
            try
            {
                // Save Mini Cart Settings
                _cartSettings.MiniCart.ShowThumbnail = chkShowThumbnail.Checked;
                _cartSettings.MiniCart.ThumbnailWidth = int.Parse(txtThumbnailWidth.Text);
                _cartSettings.MiniCart.GIFBgColor = txtGIFBgColor.Text;
                _cartSettings.MiniCart.EnableImageCaching = chkEnableImageCaching.Checked;
                _cartSettings.MiniCart.CacheImageDuration = int.Parse(txtCacheDuration.Text);
                _cartSettings.MiniCart.ProductColumn = lstProductColumn.SelectedValue;
                _cartSettings.MiniCart.LinkToDetail = chkLinkToDetail.Checked;
                _cartSettings.MiniCart.IncludeVAT = chkIncludeVAT.Checked;

                // Update Cached Settings
                _cartSettings.UpdateCache(ModuleId, TabId);
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        #endregion
    }
}