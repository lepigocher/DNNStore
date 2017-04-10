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

namespace DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider
{
	/// <summary>
	/// Summary description for SqlDataProvider.
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
            get { return _connectionString; } 
		} 

		public string ProviderPath 
		{
            get { return _providerPath; } 
		} 

		public string ObjectQualifier 
		{
            get { return _objectQualifier; } 
		} 

		public string DatabaseOwner 
		{
            get { return _databaseOwner; } 
		}

		#endregion

		#region Private Methods

		private object GetNull(object field) 
		{ 
			return Null.GetNull(field, DBNull.Value); 
		} 

		#endregion

		#region Public Methods

        public override int AddAddress(int portalID, int userID, string description, string firstName, string lastName, string address1, string address2, string city, string regionCode, string countryCode, string postalCode, string email, string phone1, string phone2, bool primaryAddress, bool userSaved, string createdByUser)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, 
				DatabaseOwner + ObjectQualifier + "Store_Addresses_AddAddress", 
				userID, 
				portalID, 
				description,
                GetNull(firstName),
                lastName,
                GetNull(address1),
                GetNull(address2),
                GetNull(city),
                GetNull(regionCode),
                GetNull(countryCode),
                GetNull(postalCode),
                GetNull(email),
                GetNull(phone1),
                GetNull(phone2),
				primaryAddress,
                userSaved,
                GetNull(createdByUser)));
		}

        public override void UpdateAddress(int addressID, int userID, string description, string firstName, string lastName, string address1, string address2, string city, string regionCode, string countryCode, string postalCode, string email, string phone1, string phone2, bool primaryAddress, bool userSaved)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, 
				DatabaseOwner + ObjectQualifier + "Store_Addresses_UpdateAddress", 
				addressID,
                userID,
                description,
                GetNull(firstName),
                lastName,
                GetNull(address1),
                GetNull(address2),
                GetNull(city),
                GetNull(regionCode),
                GetNull(countryCode),
                GetNull(postalCode),
                GetNull(email),
                GetNull(phone1),
                GetNull(phone2),
				primaryAddress,
                userSaved);
		}

		public override void DeleteAddresses(int portalID, int userID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Addresses_DeleteUserAddresses", userID);
		}

		public override void DeleteAddress(int addressID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Addresses_DeleteAddress", addressID);
		}

		public override IDataReader GetAddresses(int portalID, int userID)
		{
			return  SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Addresses_GetUserAddresses", userID);
		}

		public override IDataReader GetAddress(int addressID)
		{
			return  SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Addresses_GetAddress", addressID);
		}

        public override IDataReader GetAddressSettings(int portalID)
        {
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Administration_GetAddressSettings", portalID);
        }

        public override void UpdateAddressSettings(int portalID, string settings)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Administration_UpdateAddressSettings", portalID, GetNull(settings));
        }

		#endregion
	}
}
