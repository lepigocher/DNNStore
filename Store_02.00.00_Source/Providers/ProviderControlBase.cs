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
using System.IO;
using DotNetNuke.Entities.Modules;

namespace DotNetNuke.Modules.Store.Providers
{
	/// <summary>
    /// Base class used by providers controls.
    /// </summary>
	public class ProviderControlBase : PortalModuleBase
	{
		#region Private Members

	    protected string ErrorMessage;
	    protected bool IsReady
	    {
            get
            {
                object isReady = ViewState["IsReady"];
                return isReady == null ? false : (bool) isReady;
            }
            set { ViewState["IsReady"] = value; }
	    }

		#endregion

		#region Properties

	    public PortalModuleBase ParentControl { get; set; }

	    public virtual object DataSource { get; set; }

	    public virtual bool IsValid
		{
			get { return true; }
		}

        public virtual bool IsLogged
        {
            get { return (UserId != -1); }
        }

		#endregion

		#region Events

		public event EventHandler EditComplete;

		protected void InvokeEditComplete()
		{
			if (EditComplete != null)
				EditComplete(this, null);
		}

        public event ErrorEventHandler ProviderError;

        protected void InvokeProviderError()
        {
            if (ProviderError != null)
                ProviderError(this, new ErrorEventArgs(new ProviderException(ErrorMessage)));
        }

        protected void InvokeSecurityProviderError()
        {
            if (ProviderError != null)
                ProviderError(this, new ErrorEventArgs(new ProviderSecurityException(ErrorMessage)));
        }

		#endregion
		
		#region PortalModuleBase Overrides

		protected override void OnLoad(EventArgs e)
		{
		    Type baseType = GetType().BaseType;
            if (baseType != null)
                LocalResourceFile = Services.Localization.Localization.GetResourceFile(this, baseType.Name + ".ascx");
			base.OnLoad (e);
		}

		#endregion
	}
}
