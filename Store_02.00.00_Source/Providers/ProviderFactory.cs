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
using System.Runtime.Remoting;

namespace DotNetNuke.Modules.Store.Providers
{
	/// <summary>
	/// Summary description for ProviderFactory.
	/// </summary>
    public sealed class ProviderFactory
	{
		public static IProvider CreateProvider(ProviderInfo providerInfo)
		{
            IProvider provider = null;
			ObjectHandle handle;
			string name = providerInfo.Name;
			string assemblyName = providerInfo.Assembly;
			string className = providerInfo.Class;

			try
			{
				switch (providerInfo.Type)
				{
					case StoreProviderType.Address:
						handle = Activator.CreateInstance(assemblyName, className);
						provider = (Address.IAddressProvider)handle.Unwrap();
						provider.Info = providerInfo;
						break;

					case StoreProviderType.Catalog:
						throw new NotImplementedException("The 'Catalog' provider type has not been implemented.");
						//break;

					case StoreProviderType.Fulfillment:
						throw new NotImplementedException("The 'Fulfillment' provider type has not been implemented.");
						//break;

					case StoreProviderType.Payment:
						throw new NotImplementedException("The 'Payment' provider type has not been implemented.");
						//break;

					case StoreProviderType.Promotion:
						throw new NotImplementedException("The 'Promotion' provider type has not been implemented.");
						//break;

					case StoreProviderType.Shipping:
						handle = Activator.CreateInstance(assemblyName, className);
						provider = (Shipping.IShippingProvider)handle.Unwrap();
						provider.Info = providerInfo;
						break;

					case StoreProviderType.Subscription:
						throw new NotImplementedException("The 'Subscription' provider type has not been implemented.");
						//break;

					case StoreProviderType.Tax:
						handle = Activator.CreateInstance(assemblyName, className);
						provider = (Tax.ITaxProvider)handle.Unwrap();
						provider.Info = providerInfo;
						break;

					default:
						break;
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException("An error ocurred while creating the '" + name + "' " + Enum.GetName(typeof(StoreProviderType), providerInfo.Type) + " provider.", ex);
			}

			return provider;
		}
	}
}
