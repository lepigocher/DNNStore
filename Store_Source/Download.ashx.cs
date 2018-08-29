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

using System.Web;

using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.FileSystem;

using DotNetNuke.Modules.Store.Core.Admin;
using DotNetNuke.Modules.Store.Core.Catalog;
using DotNetNuke.Modules.Store.Core.Components;
using DotNetNuke.Modules.Store.Core.Customer;

namespace DotNetNuke.Modules.Store.WebControls
{
    public class Download : IHttpHandler
    {
        #region IHttpHandler Members

        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            PortalSettings portalSettings = PortalController.Instance.GetCurrentPortalSettings();
            UserInfo user = (UserInfo)context.Items["UserInfo"];
            if (user != null && user.UserID != Null.NullInteger)
            {
                // Init params
                int currentUserID = user.UserID;
                int portalID = portalSettings.PortalId;
                string key = context.Request.QueryString["KEY"];
                key = SymmetricHelper.Decrypt(key);
                string[] values = key.Split(',');
                int orderDetailID = int.Parse(values[0]);
                int userID = int.Parse(values[1]);
                ProductInfo product = null;
                // Get the requested order detail row
                OrderController orderControler = new OrderController();
                OrderDetailInfo orderDetail = orderControler.GetOrderDetail(orderDetailID);
                if (orderDetail != null)
                {
                    // Get the corresponding product
                    ProductController productControler = new ProductController();
                    product = productControler.GetProduct(portalID, orderDetail.ProductID);
                }
                // If user authentication and product are valid
                if (currentUserID == userID && product != null)
                {
                    // Is download allowed?
                    if (product.AllowedDownloads == Null.NullInteger || orderDetail.Downloads < product.AllowedDownloads)
                    {
                        // Update download counter then download file
                        int downloads = orderDetail.Downloads;
                        if (downloads == Null.NullInteger)
                            downloads = 1;
                        else
                            downloads += 1;
                        orderControler.UpdateOrderDetail(orderDetail.OrderDetailID, orderDetail.OrderID, orderDetail.ProductID, orderDetail.Quantity, orderDetail.UnitCost, orderDetail.RoleID, downloads);
                        IFileInfo file = FileManager.Instance.GetFile(product.VirtualFileID);
                        FileManager.Instance.WriteFileToResponse(file, ContentDisposition.Attachment);
                    }
                    // The following code is NEVER reached when download succeed!
                }
                // Redirect to the product detail page or the store page
                StoreInfo storeInfo = StoreController.GetStoreInfo(portalSettings.PortalId);
                CatalogNavigation catNav = new CatalogNavigation
                {
                    TabID = storeInfo.StorePageID
                };
                if (product != null)
                {
                    catNav.CategoryID = product.CategoryID;
                    catNav.ProductID = product.ProductID;
                }
                context.Response.Redirect(catNav.GetNavigationUrl());
            }
            else
            {
                // Try to authenticate the user then retry download
                string returnUrl = context.Request.RawUrl;
                int posReturn = returnUrl.IndexOf("?returnurl=");
                if (posReturn != Null.NullInteger)
                    returnUrl = returnUrl.Substring(0, posReturn);
                returnUrl = "returnurl=" + context.Server.UrlEncode(returnUrl);
                if (portalSettings.LoginTabId != Null.NullInteger && context.Request["override"] == null)
                    context.Response.Redirect(Globals.NavigateURL(portalSettings.LoginTabId, "", returnUrl), true);
                else
                {
                    if (portalSettings.HomeTabId != Null.NullInteger)
                        context.Response.Redirect(Globals.NavigateURL(portalSettings.HomeTabId, "Login", returnUrl), true);
                    else
                        context.Response.Redirect(Globals.NavigateURL(portalSettings.ActiveTab.TabID, "Login", returnUrl), true);
                }
            }
        }

        #endregion
    }
}
