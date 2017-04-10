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
using System.IO;
using System.Xml.Serialization;
using DotNetNuke.Common.Utilities;

namespace DotNetNuke.Modules.Store.Providers
{
	/// <summary>
	/// Summary description for ProviderController.
	/// </summary>
    public sealed class ProviderController
	{
		#region Private Members

		private readonly string[] _providerPaths = new string[] {"",
									"Providers\\AddressProviders\\",
									"Providers\\CatalogProviders\\",
									"Providers\\PromotionProviders\\",
									"Providers\\ShippingProviders\\",
									"Providers\\TaxProviders\\",
									"Providers\\PaymentProviders\\",
									"Providers\\FulfillmentProviders\\",
									"Providers\\SubscriptionProviders\\"
		};

		private readonly List<ProviderInfo> _providerList;

		#endregion

		#region Constructors

		public ProviderController(StoreProviderType providerType)
		{
            _providerList = (List<ProviderInfo>)DataCache.GetCache("StoreProviderList" + providerType);
            if (_providerList == null)
            {
                string providerPath = Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, "DesktopModules\\Store\\");
                _providerList = new List<ProviderInfo>();
                providerPath = Path.Combine(providerPath, _providerPaths[(int)providerType]);
                string[] folderList = Directory.GetDirectories(providerPath);

                foreach (string folder in folderList)
                {
                    ProviderInfo providerInfo = GetProviderInfo(folder);
                    if (providerInfo != null)
                    {
                        string virtualPath = _providerPaths[(int)providerType].Replace("\\", "/");
                        virtualPath += GetTrailingFolder(folder);

                        providerInfo.Path = folder;
                        providerInfo.VirtualPath = virtualPath;
                        providerInfo.Type = providerType;

                        _providerList.Add(providerInfo);
                    }
                }

                DataCache.SetCache("StoreProviderList" + providerType, _providerList, false);
            }
		}

		#endregion

		#region Public Methods

        public List<ProviderInfo> GetProviders()
		{
			return _providerList;
		}

		public ProviderInfo GetProvider(string providerName)
		{
			ProviderInfo providerInfo = null;

			foreach (ProviderInfo info in _providerList)
			{
				if (info.Name == providerName)
				{
					providerInfo = info;
					break;
				}
			}

			return providerInfo;
		}

		#endregion

		#region Private Methods

		private static ProviderInfo GetProviderInfo(string providerPath)
		{
			ProviderInfo providerInfo = new ProviderInfo();
			string providerName = GetTrailingFolder(providerPath);
            string infoFile = Path.Combine(providerPath, providerName + "Info.xml");

			if (Directory.Exists(providerPath))
			{
                DirectoryInfo dirInfo = new DirectoryInfo(providerPath);
                // if directory is not hidden
                if ((dirInfo.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
				    if (File.Exists(infoFile))
				    {
					    try
					    {
						    Stream infoDoc = File.Open(infoFile, FileMode.Open, FileAccess.Read, FileShare.Read);
						    XmlSerializer serializer = new XmlSerializer(typeof(ProviderInfo));

						    providerInfo = (ProviderInfo)serializer.Deserialize(infoDoc);
    						
						    infoDoc.Close();

						    if (providerInfo.Controls == null || providerInfo.Controls.Length < 1)
							    throw new ApplicationException("No controls are defined in '" + providerPath + "ProviderInfo.xml'.");
					    }
					    catch (Exception ex)
					    {
						    throw new ApplicationException("An error ocurred while reading '" + providerPath + "ProviderInfo.xml'.", ex);
					    }
				    }
				    else
				    {
					    //throw new FileNotFoundException("No ProviderInfo.xml file was found in '" + providerPath + "'.");
                        return null;
				    }
                }
			}
			else
			{
				throw new DirectoryNotFoundException("The provider path '" + providerPath + "' does not exist.");
			}

			return providerInfo;
		}

		private static string GetTrailingFolder(string path)
		{
			string folder = "";

			while (path.EndsWith("\\") && path.Length > 0)
			{
				path = path.Remove(path.Length - 1, 1);
			}

			if (path.Length > 0)
			{
				int s = path.LastIndexOf("\\");

				if (s >= 0)
				{
					folder = path.Substring(s + 1);
				}
				else
				{
					folder = path;
				}
			}

			return folder;
		}

		#endregion
	}
}
