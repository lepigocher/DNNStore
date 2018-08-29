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
using System.Collections.Generic;
using System.Web;

using DotNetNuke.Common.Utilities;

using DotNetNuke.Modules.Store.Core.Components;

namespace DotNetNuke.Modules.Store.Core.Cart
{
	/// <summary>
    /// Used to manage the current cart of a user session.
	/// </summary>
    public sealed class CurrentCart
	{
        #region Private Members

        private const string CookieName = "DotNetNuke_Store_Portal_";
		private static bool _isCartVerified;
        private static readonly CartController Controller = new CartController();

        #endregion

        #region Public Methods

        public static ItemInfo GetItem(int itemID)
        {
            return Controller.GetItem(itemID);
        }

		public static void AddItem(int portalID, bool secureCookie, int productID, int quantity)
		{
            string cartID = GetCartID(portalID, secureCookie);

			ItemInfo itemInfo = new ItemInfo();
			itemInfo.CartID = cartID;
			itemInfo.ProductID = productID;
			itemInfo.Quantity = quantity;

			Controller.AddItem(itemInfo);
		}

        public static void UpdateItem(int portalID, bool secureCookie, int itemID, int quantity)
		{
            string cartID = GetCartID(portalID, secureCookie);

			ItemInfo itemInfo = Controller.GetItem(itemID);

			if (itemInfo != null)
			{
				itemInfo.CartID = cartID;
				itemInfo.ItemID = itemID;
				itemInfo.Quantity = quantity;

				Controller.UpdateItem(itemInfo);
			}
		}

		public static void RemoveItem(int itemID)
		{
			Controller.DeleteItem(itemID);
		}

		public static void ClearItems(int portalID, bool secureCookie)
		{
            string cartID = GetCartID(portalID, secureCookie);

			Controller.DeleteItems(cartID);
		}

        public static List<ItemInfo> GetItems(int portalID, bool secureCookie)
		{
            string cartID = GetCartID(portalID, secureCookie);

			return Controller.GetItems(cartID);
		}

        public static bool ProductIsInCart(int portalID, bool secureCookie, int productID)
        {
            bool isInCart = false;
            List<ItemInfo> cartItems = GetItems(portalID, secureCookie);

            foreach (ItemInfo item in cartItems)
            {
                if (item.ProductID == productID)
                {
                    isInCart = true;
                    break;
                }
            }
            return isInCart;
        }

        public static CartInfo GetInfo(int portalID, bool secureCookie)
		{
            string cartID = GetCartID(portalID, secureCookie);

			return Controller.GetCart(cartID, portalID);
		}

        public static void DeleteCart(int portalID, bool secureCookie)
		{
            string cartID = GetCartID(portalID, secureCookie);

			Controller.DeleteItems(cartID);
			Controller.DeleteCart(cartID);

            SetCartID(portalID, null, secureCookie);
		}

		#endregion

		#region Private Methods

        private static string CookieKey(int portalID)
        {
            return CookieName + portalID;
        }

        private static string GetCartID(int portalID, bool secureCookie)
		{
			string cartID = null;

			// Get cart ID from cookie
            HttpCookie cartCookie = HttpContext.Current.Request.Cookies[CookieKey(portalID)];
			if (cartCookie != null)
			{
                cartID = cartCookie["CartID"];
                if (!string.IsNullOrEmpty(cartID) && secureCookie && SymmetricHelper.CanSafelyEncrypt)
                {
                    try
                    {
                        cartID = SymmetricHelper.Decrypt(cartID);
                    }
                    catch (FormatException)
                    {
                        // Do nothing in this case it's probably throwed
                        // because the CartID is not encrypted!
                    }
                }
                else
                {
                    // Verify if it's a valid GUID, otherwise try to decrypt the value
                    if (!CheckGuid(cartID))
                        cartID = SymmetricHelper.Decrypt(cartID);
                }
			}

			// Do we need to verify?
			if (!string.IsNullOrEmpty(cartID) && !_isCartVerified)
			{
				_isCartVerified = (Controller.GetCart(cartID, portalID) != null);
				if (!_isCartVerified)
					cartID = null;
			}

			// Do we need to create a new cart?
            if (string.IsNullOrEmpty(cartID)) 
			{
				cartID = CreateCart(portalID);
                SetCartID(portalID, cartID, secureCookie);
			}

			return cartID;
		}

        private static bool CheckGuid(string cartID)
        {
            bool bResult = true;

            try
            {
                new Guid(cartID);
            }
            catch
            {
                bResult = false;
            }

            return bResult;
        }

        private static void SetCartID(int portalID, string cartID, bool secureCookie)
		{
            if (!string.IsNullOrEmpty(cartID))
			{
                HttpCookie cartCookie = new HttpCookie(CookieKey(portalID));
                if (secureCookie && SymmetricHelper.CanSafelyEncrypt)
                    cartCookie["CartID"] = SymmetricHelper.Encrypt(cartID);
                else
                    cartCookie["CartID"] = cartID;
				HttpContext.Current.Response.Cookies.Add(cartCookie);
			}
			else
			{
                HttpCookie cartCookie = new HttpCookie(CookieKey(portalID));
				cartCookie.Expires = DateTime.Today.AddDays(-100);
				HttpContext.Current.Response.Cookies.Add(cartCookie);
			}
		}

		private static string CreateCart(int portalID)
		{
			string cartID = Guid.NewGuid().ToString();

			Controller.AddCart(cartID, portalID, Null.NullInteger);

			return cartID;
		}

		#endregion
	}
}
