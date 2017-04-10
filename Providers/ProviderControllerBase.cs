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

using System.Web;
using DotNetNuke.Entities.Modules;

namespace DotNetNuke.Modules.Store.Providers
{
	/// <summary>
	/// Summary description for ProviderControllerBase.
	/// </summary>
	public class ProviderControllerBase : IProvider
    {
        #region Private Members

        private ProviderInfo _info = new ProviderInfo();

        #endregion

        #region IProvider Members

        public ProviderInfo Info
        {
            get { return _info; }
            set { _info = value; }
        }

        /// <summary>
        /// Builds the proper checkout control according the modulePath specified.
        /// <strong>NOTE: The control must have suffix of "Checkout"</strong>
        /// </summary>
        /// <param name="parentControl">Parent for the checkout control.</param>
        /// <param name="modulePath">Path to the address controllers for this Provider.</param>
        /// <returns>The checkout control for configured provider</returns>
        public ProviderControlBase GetCheckoutControl(PortalModuleBase parentControl, string modulePath)
		{
			ProviderControlBase checkoutControl = LoadControl(parentControl, modulePath, "Checkout");
			return checkoutControl;
		}

        /// <summary>
        /// Builds the proper admin control according the modulePath specified.
        /// <strong>NOTE: The control must have suffix of "Admin"</strong>
        /// </summary>
        /// <param name="parentControl">Parent for the checkout control.</param>
        /// <param name="modulePath">Path to the controllers for this Provider.</param>
        /// <returns>The admin control for configured provider</returns>
        public ProviderControlBase GetAdminControl(PortalModuleBase parentControl, string modulePath)
		{
			ProviderControlBase adminControl = LoadControl(parentControl, modulePath, "Admin");
			return adminControl;
		}

		#endregion

		#region Protected Methods

	    protected ProviderControlBase LoadControl(PortalModuleBase parentControl, string controlPath, string controlName)
		{
		    string controlValue = "";

            controlPath = VirtualPathUtility.AppendTrailingSlash(VirtualPathUtility.Combine(controlPath, _info.VirtualPath));
            for (int i = 0; i < _info.Controls.Length; i++)
            {
                if (_info.Controls[i].Name == controlName)
                {
                    controlValue = _info.Controls[i].Value;
                    break;
                }
            }
            controlPath = VirtualPathUtility.Combine(controlPath, controlValue);

            ProviderControlBase childControl = (ProviderControlBase)parentControl.LoadControl(controlPath);
			childControl.ParentControl = parentControl;

			return childControl;
		}

		#endregion
	}
}
