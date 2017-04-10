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

namespace DotNetNuke.Modules.Store.Providers.Tax.CountryTaxProvider
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
			Provider provider = (Provider)_providerConfiguration.Providers[_providerConfiguration.DefaultProvider];

            _connectionString = Config.GetConnectionString();
            if (string.IsNullOrEmpty(_connectionString))
                _connectionString = provider.Attributes["connectionString"];
			
			_providerPath = provider.Attributes["providerPath"]; 
			_objectQualifier = provider.Attributes["objectQualifier"]; 
			
			if (!string.IsNullOrEmpty(_objectQualifier) && !_objectQualifier.EndsWith("_")) 
				_objectQualifier += "_"; 
			
			_databaseOwner = provider.Attributes["databaseOwner"]; 
			
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

		public override IDataReader GetTaxRates(int portalID)
		{
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Administration_GetCountryTaxRates", portalID);
		}

		public override void UpdateTaxRates(int portalID, decimal defaultTaxRate, bool showTax, string countryTaxRates)
		{
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Administration_UpdateCountryTaxRates", portalID, GetNull(defaultTaxRate), showTax, GetNull(countryTaxRates));
		}

		#endregion
	}
}
