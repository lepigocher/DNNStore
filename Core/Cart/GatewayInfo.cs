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

using DotNetNuke.Modules.Store.Core.Admin;

namespace DotNetNuke.Modules.Store.Core.Cart
{
    /// <summary>
    /// Gateway class info.
    /// </summary>
    public sealed class GatewayInfo
	{
		#region Private Members

        private string _gatewayName = string.Empty;
        private string _gatewayPath = string.Empty;
        private string _adminControl = "Admin.ascx";
        private string _paymentControl = "Payment.ascx";

		#endregion

		#region Constructors

        /// <summary>
        /// Initialize a new instance of a gateway class info.
        /// </summary>
		public GatewayInfo()
		{
		}

        /// <summary>
        /// Initialize a new instance of a gateway class info from the specified gateway name and installation path.
        /// </summary>
		public GatewayInfo(string gatewayName, string gatewayPath)
		{
			_gatewayName = gatewayName;
			_gatewayPath = gatewayPath;
		}

		#endregion

		#region Public Properties

		public string GatewayName
		{
			get { return _gatewayName; }
			set { _gatewayName = value; }
		}

		public string GatewayPath
		{
			get { return _gatewayPath; }
			set { _gatewayPath = value; }
		}

		public string AdminControl
		{
			get { return _adminControl; }
			set { _adminControl = value; }
		}

		public string PaymentControl
		{
			get { return _paymentControl; }
			set { _paymentControl = value; }		
		}

		#endregion

		#region Public Methods

        /// <summary>
        /// Get gateway settings
        /// </summary>
        /// <param name="portalID">The current portal ID</param>
        /// <returns>Gateway settings</returns>
		public string GetSettings(int portalID)
		{
			string gatewaySettings = string.Empty; 

            StoreInfo storeInfo = StoreController.GetStoreInfo(portalID);
			if (storeInfo != null)
				gatewaySettings = storeInfo.GatewaySettings;

			return gatewaySettings;
		}

        /// <summary>
        /// Save gateway settings to the store.
        /// </summary>
        /// <param name="portalID">The current portal ID</param>
        /// <param name="gatewaySettings">Gateway settings</param>
		public void SetSettings(int portalID, string gatewaySettings)
		{
            StoreInfo storeInfo = StoreController.GetStoreInfo(portalID);
			if (storeInfo != null)
			{
				storeInfo.GatewaySettings = gatewaySettings;
                StoreController controller = new StoreController();
				controller.UpdateStoreInfo(storeInfo);
			}
		}

		#endregion
	}
}
