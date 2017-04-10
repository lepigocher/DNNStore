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
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Customer;

namespace DotNetNuke.Modules.Store.WebControls
{
    public partial class CustomerDownloads : StoreControlBase, IStoreTabedControl
    {
        #region Private Members

        private string _downloadPath;
        private string _refreshScript;
        private string _downloadText;
        private string _unlimitedText;
        private OrderController _orderController;

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (IsLogged)
                {
                    _orderController = new OrderController();
                    List<DownloadInfo> downloads = _orderController.GetDownloads(PortalId, UserId);
                    if (downloads.Count > 0)
                    {
                        _downloadPath = ResolveUrl(ModulePath + "Download.ashx?KEY={0}");
                        _refreshScript = string.Format(@"setTimeout(""window.location = '{0}'"", 7000);return true;", Request.Url);
                        _downloadText = Localization.GetString("lnkDownload", LocalResourceFile);
                        _unlimitedText = Localization.GetString("Unlimited", LocalResourceFile);
                        pnlDownloads.Visible = true;
                        pnlDownloadsError.Visible = false;
                        grdDownloads.DataSource = downloads;
                        grdDownloads.DataBind();
                    }
                    else
                    {
                        pnlDownloads.Visible = false;
                        pnlDownloadsError.Visible = true;
                        lblError.Text = Localization.GetString("NoFile", LocalResourceFile);
                    }
                }
                else
                {
                    pnlDownloads.Visible = false;
                    pnlDownloadsError.Visible = true;
                    lblError.Text = Localization.GetString("UserNotLogged", LocalResourceFile);
                }
            }
        }

        protected void grdDownloads_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DownloadInfo downloadInfo = (DownloadInfo)e.Item.DataItem;

                // Product Title
                Label lblProductTitle = (Label)e.Item.FindControl("lblProductTitle");
                lblProductTitle.Text = downloadInfo.ProductTitle;

                // Allowed Downloads
                int allowed = downloadInfo.AllowedDownloads;
                Label lblAllowed = (Label)e.Item.FindControl("lblAllowed");
                if (allowed == Null.NullInteger)
                    lblAllowed.Text = _unlimitedText;
                else
                {
                    allowed = downloadInfo.AllowedDownloads * downloadInfo.Quantity;
                    lblAllowed.Text = allowed.ToString();
                }

                // Downloaded
                int downloaded = downloadInfo.Downloads;
                if (downloaded == Null.NullInteger)
                    downloaded = 0;
                Label lblDownloaded = (Label)e.Item.FindControl("lblDownloaded");
                lblDownloaded.Text = downloaded.ToString();

                // Download link
                HtmlAnchor lnkDownload = (HtmlAnchor)e.Item.FindControl("lnkDownload");
                if (allowed == Null.NullInteger || downloaded < allowed)
                {
                    lnkDownload.HRef = string.Format(_downloadPath, Server.UrlEncode(SymmetricHelper.Encrypt(downloadInfo.OrderDetailID + "," + UserId)));
                    lnkDownload.Attributes["OnClick"] = _refreshScript;
                    lnkDownload.InnerText = _downloadText;
                }
                else
                    lnkDownload.Visible = false;
            }
        }

        #endregion

        #region IStoreTabedControl Members

        string IStoreTabedControl.Title
        {
            get
            {
                return Localization.GetString("lblParentTitle", LocalResourceFile);
            }
        }

        #endregion
    }
}