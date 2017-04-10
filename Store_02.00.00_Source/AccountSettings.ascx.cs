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

namespace DotNetNuke.Modules.Store.WebControls
{
    /// <summary>
	///		Summary description for AccountSettings.
	/// </summary>
	public partial class AccountSettings : ModuleSettingsBase
	{
        #region Private Members

        private Cart.ModuleSettings _cartSettings;

        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                _cartSettings = new Cart.ModuleSettings(ModuleId, TabId);
                if (!IsPostBack)
                {
                    // Load product column names
                    lstProductColumn.Items.Add(new ListItem(Localization.GetString("ModelNumber", LocalResourceFile), "modelnumber"));
                    lstProductColumn.Items.Add(new ListItem(Localization.GetString("ModelName", LocalResourceFile), "modelname"));
                    lstProductColumn.Items.Add(new ListItem(Localization.GetString("ProductTitle", LocalResourceFile), "producttitle"));
                    // Load view names
                    lstDefaultView.Items.Add(new ListItem(Localization.GetString("CustomerCart", LocalResourceFile), "CustomerCart"));
                    lstDefaultView.Items.Add(new ListItem(Localization.GetString("CustomerProfile", LocalResourceFile), "CustomerProfile"));
                    lstDefaultView.Items.Add(new ListItem(Localization.GetString("CustomerOrders", LocalResourceFile), "CustomerOrders"));
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

		#region Base Method Implementations

		public override void LoadSettings()
		{
			try
			{
                // Read Main Cart Settings
                ListItem listItem = lstDefaultView.Items.FindByValue(_cartSettings.MainCart.DefaultView);
                if (listItem != null)
                    listItem.Selected = true;
                chkRequireSSL.Checked = _cartSettings.MainCart.RequireSSL;
                chkShowThumbnail.Checked = _cartSettings.MainCart.ShowThumbnail;
                txtThumbnailWidth.Text = _cartSettings.MainCart.ThumbnailWidth.ToString();
                txtGIFBgColor.Text = _cartSettings.MainCart.GIFBgColor;
                chkEnableImageCaching.Checked = _cartSettings.MainCart.EnableImageCaching;
                txtCacheDuration.Text = _cartSettings.MainCart.CacheImageDuration.ToString();
                if (_cartSettings.MainCart.ShowThumbnail)
                {
                    trThumbnailWidth.Visible = true;
                    trGIFBgColor.Visible = true;
                    trEnableImageCaching.Visible = true;
                    if (_cartSettings.MainCart.EnableImageCaching)
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
                listItem = lstProductColumn.Items.FindByValue(_cartSettings.MainCart.ProductColumn);
                if (listItem != null)
                    listItem.Selected = true;
                chkLinkToDetail.Checked = _cartSettings.MainCart.LinkToDetail;
                chkIncludeVAT.Checked = _cartSettings.MainCart.IncludeVAT;
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
                if (Page.IsValid)
				{
                    // Save Main Cart Settings
                    _cartSettings.MainCart.DefaultView = lstDefaultView.SelectedValue;
                    _cartSettings.MainCart.RequireSSL = chkRequireSSL.Checked;
                    _cartSettings.MainCart.ShowThumbnail = chkShowThumbnail.Checked;
                    _cartSettings.MainCart.ThumbnailWidth = int.Parse(txtThumbnailWidth.Text);
                    _cartSettings.MainCart.GIFBgColor = txtGIFBgColor.Text;
                    _cartSettings.MainCart.EnableImageCaching = chkEnableImageCaching.Checked;
                    _cartSettings.MainCart.CacheImageDuration = int.Parse(txtCacheDuration.Text);
                    _cartSettings.MainCart.ProductColumn = lstProductColumn.SelectedValue;
                    _cartSettings.MainCart.LinkToDetail = chkLinkToDetail.Checked;
                    _cartSettings.MainCart.IncludeVAT = chkIncludeVAT.Checked;

                    // Update Cached Settings
                    _cartSettings.UpdateCache(ModuleId, TabId);
                }
			}
			catch(Exception ex)
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		#endregion

	}
}
