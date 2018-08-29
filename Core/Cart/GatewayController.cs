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
using System.IO;

using DotNetNuke.Common.Utilities;

namespace DotNetNuke.Modules.Store.Core.Cart
{
    /// <summary>
    /// Used to manage gateway providers.
    /// </summary>
    public sealed class GatewayController
	{
		#region Private Members

		private const string ProviderPath = "Providers\\GatewayProviders\\";
		private readonly List<GatewayInfo> _gatewayList;

		#endregion

		#region Constructor

        /// <summary>
        /// Initialize a new instance of the controler with the specified module path.
        /// </summary>
        /// <param name="modulePath">The installation path of the module</param>
		public GatewayController(string modulePath)
		{
            _gatewayList = (List<GatewayInfo>)DataCache.GetCache("StoreGatewayList");
            if (_gatewayList == null)
            {
                string gatewayPath = Path.Combine(modulePath, ProviderPath);
                string[] folderList = Directory.GetDirectories(gatewayPath);
                _gatewayList = new List<GatewayInfo>(folderList.Length);

                foreach (string folder in folderList)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(folder);
                    // if directory is not hidden
                    if ((dirInfo.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    {
                        GatewayInfo gateway = new GatewayInfo(GetGatewayName(folder), folder);

                        // Lookup payment and admin controls
                        string[] adminControls = Directory.GetFiles(folder, "*Admin.ascx");
                        if (adminControls.Length > 0)
                        {
                            gateway.AdminControl = Path.GetFileName(adminControls[0]);
                        }

                        string[] paymentControls = Directory.GetFiles(folder, "*Payment.ascx");
                        if (paymentControls.Length > 0)
                        {
                            gateway.PaymentControl = Path.GetFileName(paymentControls[0]);
                        }

                        _gatewayList.Add(gateway);
                    }
                }

                DataCache.SetCache("StoreGatewayList", _gatewayList);
            }
		}

		#endregion

		#region Public Methods

        /// <summary>
        /// Get a list of installed gateway providers.
        /// </summary>
        /// <returns>List of gateway info classes</returns>
        public List<GatewayInfo> GetGateways()
		{
			return _gatewayList;
		}

        /// <summary>
        /// Get the gateway info class of the specified gateway name.
        /// </summary>
        /// <param name="gatewayName">Gateawy name</param>
        /// <returns>Gateway info class</returns>
		public GatewayInfo GetGateway(string gatewayName)
		{
			GatewayInfo gateway = null;

			foreach(GatewayInfo info in _gatewayList)
			{
				if (string.Compare(info.GatewayName, gatewayName, true) == 0)
				{
					gateway = info;
					break;
				}
			}

			return gateway;
		}

        /// <summary>
        /// Get the gateway name of the specified provider path.
        /// </summary>
        /// <param name="gatewayPath">The gateway provider installation path</param>
        /// <returns>Gateway name</returns>
		public string GetGatewayName(string gatewayPath)
		{
			string gatewayName = gatewayPath;

			gatewayName = gatewayName.TrimEnd(new char[] {'\\','/',' '});
			gatewayName = gatewayName.Substring(gatewayName.LastIndexOf("\\") + 1);

			return gatewayName;
		}

		#endregion
	}
}
