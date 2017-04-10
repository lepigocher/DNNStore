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
using System.Data;
using DotNetNuke.Framework;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
    /// DataProvider abstract class.
    /// </summary>
	public abstract class DataProvider
	{
		#region Private Members

		private static DataProvider _provider; 

		#endregion

		#region Constructors

        /// <summary>
        /// Static constructor used to initialize a singleton of the concrete data provider.
        /// </summary>
        static DataProvider() 
		{ 
			CreateProvider(); 
		}

        /// <summary>
        /// Create the singleton of the concrete data provider.
        /// </summary>
        private static void CreateProvider() 
		{ 
			_provider = (DataProvider)Reflection.CreateObject("data", "DotNetNuke.Modules.Store.Cart", "DotNetNuke.Modules.Store.Cart"); 
		}

        /// <summary>
        /// Used to access the singleton.
        /// </summary>
        /// <returns>An instance of the concrete data provider</returns>
        public static DataProvider Instance() 
		{ 
			return _provider; 
		}

		#endregion

		#region Abstract Methods

		public abstract void AddCart(string cartID, int portalID, int userID);
		public abstract void UpdateCart(string cartID, int userID);
		public abstract void DeleteCart(string cartID);
		public abstract void PurgeCarts(DateTime purgeDate);
		public abstract IDataReader GetCart(string cartID, int portalID);

		public abstract int AddItem(string cartID, int productID, int quantity);
		public abstract void UpdateItem(int itemID, int quantity);
		public abstract void DeleteItem(int itemID);
		public abstract void DeleteItems(string cartID);
		public abstract IDataReader GetItem(int itemID);
		public abstract IDataReader GetItems(string cartID);

		#endregion
	}
}
