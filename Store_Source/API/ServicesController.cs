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

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using DotNetNuke.Common;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.Localization;
using DotNetNuke.Web.Api;

using DotNetNuke.Modules.Store.Core.Admin;
using DotNetNuke.Modules.Store.Core.Cart;
using DotNetNuke.Modules.Store.Core.Catalog;

namespace DotNetNuke.Modules.Store.API
{
    public sealed class ServicesController : DnnApiController
    {
        #region Web Service Methods

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage AddToCart(ItemCmdDTO item)
        {
            try
            {
                int portalID = PortalController.Instance.GetCurrentPortalSettings().PortalId;
                StoreInfo storeSettings = StoreController.GetStoreInfo(portalID);

                if (storeSettings.InventoryManagement && storeSettings.AvoidNegativeStock)
                {
                    ProductController controler = new ProductController();
                    ProductInfo currentProduct = controler.GetProduct(portalID, item.ID);

                    if (currentProduct.StockQuantity < item.Quantity)
                        return Request.CreateResponse(HttpStatusCode.Conflict, LocalizeString("NotEnoughProducts"));
                }

                CurrentCart.AddItem(portalID, storeSettings.SecureCookie, item.ID, item.Quantity);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, LocalizeString("Unexpected.Error"));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage UpdateCart(ItemCmdDTO item)
        {
            try
            {
                int portalID = PortalController.Instance.GetCurrentPortalSettings().PortalId;
                StoreInfo storeSettings = StoreController.GetStoreInfo(portalID);

                if (storeSettings.InventoryManagement && storeSettings.AvoidNegativeStock)
                {
                    ProductController controler = new ProductController();
                    ProductInfo currentProduct = controler.GetProduct(portalID, item.ID);

                    if (currentProduct.StockQuantity < item.Quantity)
                        return Request.CreateResponse(HttpStatusCode.Conflict, LocalizeString("NotEnoughProducts"));
                }

                CurrentCart.UpdateItem(portalID, storeSettings.SecureCookie, item.ID, item.Quantity);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, LocalizeString("Unexpected.Error"));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage IsInCart(int productID)
        {
            try
            {
                int portalID = PortalController.Instance.GetCurrentPortalSettings().PortalId;
                StoreInfo storeSettings = StoreController.GetStoreInfo(portalID);
                bool isInCart = CurrentCart.ProductIsInCart(portalID, storeSettings.SecureCookie, productID);

                return Request.CreateResponse(HttpStatusCode.OK, isInCart);
            }
            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, LocalizeString("Unexpected.Error"));
            }
        }


        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetCart()
        {
            try
            {
                int portalID = PortalController.Instance.GetCurrentPortalSettings().PortalId;
                StoreInfo storeSettings = StoreController.GetStoreInfo(portalID);

                CartInfo cart = CurrentCart.GetInfo(portalID, storeSettings.SecureCookie);
                CartDTO cartDTO = new CartDTO {
                    CartID = cart.CartID,
                    Total = cart.Items > 0 ? cart.Total : 0
                };

                List<ItemInfo> cartItems = CurrentCart.GetItems(portalID, storeSettings.SecureCookie);
                List<ItemDTO> itemsDTO = new List<ItemDTO>(cartItems.Count);

                foreach (ItemInfo item in cartItems)
                {
                    ItemDTO itemDTO = new ItemDTO
                    {
                        ItemID = item.ItemID,
                        ProductID = item.ProductID,
                        Manufacturer = item.Manufacturer,
                        ModelNumber = item.ModelNumber,
                        ModelName = item.ModelName,
                        ProductImage = item.ProductImage,
                        UnitCost = item.UnitCost,
                        Quantity = item.Quantity,
                        Discount = item.Discount
                    };

                    itemsDTO.Add(itemDTO);
                }
                cartDTO.Items = itemsDTO;

                return Request.CreateResponse(HttpStatusCode.OK, cartDTO);
            }
            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, LocalizeString("Unexpected.Error"));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetCartURL()
        {
            try
            {
                int portalID = PortalController.Instance.GetCurrentPortalSettings().PortalId;
                StoreInfo storeSettings = StoreController.GetStoreInfo(portalID);

                string url = Globals.NavigateURL(storeSettings.ShoppingCartPageID);

                return Request.CreateResponse(HttpStatusCode.OK, url);
            }
            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, LocalizeString("Unexpected.Error"));
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Localize a string specified by his key.
        /// </summary>
        /// <param name="key">Resource key</param>
        /// <returns>Localized string</returns>
        private string LocalizeString(string key)
        {
            return Localization.GetString(key, Constants.ModuleSharedResources);
        }

        #endregion
    }
}
