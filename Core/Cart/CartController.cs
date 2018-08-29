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

using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;

namespace DotNetNuke.Modules.Store.Core.Cart
{
    /// <summary>
    /// Cart business class used to manage the cart and gateway providers.
    /// </summary>
    public sealed class CartController
	{
        #region Public Methods

        public void AddCart(string cartID, int portalID, int userID)
		{
			DataProvider.Instance().ExecuteNonQuery("Store_Cart_AddCart", cartID, portalID, userID);
		}

		public void UpdateCart(CartInfo cartInfo)
		{
			UpdateCart(cartInfo.CartID, cartInfo.UserID);
		}

		public void UpdateCart(string cartID, int userID)
		{
			DataProvider.Instance().ExecuteNonQuery("Store_Cart_UpdateCart", cartID, userID);
		}

		public void DeleteCart(string cartID)
		{
			DataProvider.Instance().ExecuteNonQuery("Store_Cart_DeleteCart", cartID);
		}

		public void PurgeCarts(DateTime purgeDate)
		{
			DataProvider.Instance().ExecuteNonQuery("Store_Cart_PurgeCarts", purgeDate);
		}

		public CartInfo GetCart(string cartID, int portalID)
		{
			return CBO.FillObject<CartInfo>(DataProvider.Instance().ExecuteReader("Store_Cart_GetCart", cartID, portalID)); 
		}

		public void AddItem(ItemInfo itemInfo)
		{
			DataProvider.Instance().ExecuteNonQuery("Store_CartItems_AddItem", itemInfo.CartID, itemInfo.ProductID, itemInfo.Quantity);
		}

		public void UpdateItem(ItemInfo itemInfo)
		{
			DataProvider.Instance().ExecuteNonQuery("Store_CartItems_UpdateItem", itemInfo.ItemID, itemInfo.Quantity);
		}

		public void DeleteItem(int itemID)
		{
			DataProvider.Instance().ExecuteNonQuery("Store_CartItems_DeleteItem", itemID);
		}

		public void DeleteItems(string cartID)
		{
			DataProvider.Instance().ExecuteNonQuery("Store_CartItems_DeleteItems", cartID);
		}

		public ItemInfo GetItem(int itemID)
		{
            return CBO.FillObject<ItemInfo>(DataProvider.Instance().ExecuteReader("Store_CartItems_GetItem", itemID));
		}

		public List<ItemInfo> GetItems(string cartID)
		{
			return CBO.FillCollection<ItemInfo>(DataProvider.Instance().ExecuteReader("Store_CartItems_GetItems", cartID));
		}

		#endregion
	}
}
