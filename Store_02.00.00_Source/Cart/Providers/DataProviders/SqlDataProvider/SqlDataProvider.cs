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
using Microsoft.ApplicationBlocks.Data;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
    /// Concrete cart data provider for SQL Server.
    /// </summary>
    public sealed class SqlDataProvider : DataProvider
	{
		#region Private Members

		private const string ProviderType = "data";
		private readonly ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
		private readonly string _connectionString;
		private readonly string _providerPath;
		private readonly string _objectQualifier;
		private readonly string _databaseOwner;

		#endregion

		#region Constructors

		public SqlDataProvider()
		{
			Provider objProvider = (Provider)_providerConfiguration.Providers[_providerConfiguration.DefaultProvider];

            _connectionString = Config.GetConnectionString();
            if (string.IsNullOrEmpty(_connectionString))
                _connectionString = objProvider.Attributes["connectionString"];
			
			_providerPath = objProvider.Attributes["providerPath"]; 
			_objectQualifier = objProvider.Attributes["objectQualifier"]; 
			
			if (!string.IsNullOrEmpty(_objectQualifier) && !_objectQualifier.EndsWith("_")) 
				_objectQualifier += "_"; 
			
			_databaseOwner = objProvider.Attributes["databaseOwner"]; 
			
			if (!string.IsNullOrEmpty(_databaseOwner) && !_databaseOwner.EndsWith(".")) 
				_databaseOwner += "."; 
		}

		#endregion

		#region Properties

		public string ConnectionString 
		{ 
			get 
			{ 
				return _connectionString; 
			} 
		} 

		public string ProviderPath 
		{ 
			get 
			{ 
				return _providerPath; 
			} 
		} 

		public string ObjectQualifier 
		{ 
			get 
			{ 
				return _objectQualifier; 
			} 
		} 

		public string DatabaseOwner 
		{ 
			get 
			{ 
				return _databaseOwner; 
			} 
		}

		#endregion

		#region Private Methods

		private object GetNull(object Field) 
		{ 
			return Null.GetNull(Field, DBNull.Value); 
		} 

		#endregion

		#region Public Methods

		public override void AddCart(string cartID, int portalID, int userID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Cart_AddCart", cartID, portalID, userID);
		}

		public override void UpdateCart(string cartID, int userID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Cart_UpdateCart", cartID, userID);
		}

		public override void DeleteCart(string cartID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Cart_DeleteCart", cartID);
		}

		public override void PurgeCarts(DateTime purgeDate)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Cart_PurgeCarts", purgeDate);
		}

		public override IDataReader GetCart(string cartID, int portalID)
		{
			return  SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Cart_GetCart", cartID, portalID);
		}

		public override int AddItem(string cartID, int productID, int quantity)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_CartItems_AddItem", cartID, productID, quantity));
		}

		public override void UpdateItem(int itemID, int quantity)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_CartItems_UpdateItem", itemID, quantity);
		}

		public override void DeleteItem(int itemID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_CartItems_DeleteItem", itemID);
		}

		public override void DeleteItems(string cartID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_CartItems_DeleteItems", cartID);
		}

		public override IDataReader GetItem(int itemID)
		{
			return  SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_CartItems_GetItem", itemID);
		}

		public override IDataReader GetItems(string cartID)
		{
			return  SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_CartItems_GetItems", cartID);
		}

		#endregion
	}
}
