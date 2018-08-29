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

using System;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;

using DotNetNuke.Modules.Store.Core.Admin;

namespace DotNetNuke.Modules.Store.Core.Components
{
	/// <summary>
	/// Base class for store controls.
	/// </summary>
	public class StoreControlBase : PortalModuleBase
	{
		#region Private Members

	    private string _localSharedResourceFile;
        private StoreInfo _storeSettings;
        private bool _isLogged;

		#endregion

        #region Properties

        public string LocalSharedResourceFile
        {
            get { return _localSharedResourceFile; }
        }

        public bool IsLogged
        {
            get { return _isLogged; }
        }

	    public virtual object DataSource { get; set; }

	    public virtual bool IsValid
        {
            get { return true; }
        }

        public StoreInfo StoreSettings
        {
            get
            {
                if (_storeSettings == null)
                    _storeSettings = StoreController.GetStoreInfo(PortalId);
                return _storeSettings;
            }
            set { _storeSettings = value; }
        }

        #endregion

		#region Events

		public event EventHandler EditComplete;

		protected void InvokeEditComplete()
		{
            EditComplete?.Invoke(this, new EventArgs());
        }

		#endregion
		
		#region PortalModuleBase Overrides

        protected override void OnInit(EventArgs e)
        {
            _localSharedResourceFile = Localization.GetResourceFile(this, Localization.LocalSharedResourceFile);
            Type baseType = GetType().BaseType;
            if (baseType != null)
                LocalResourceFile = Localization.GetResourceFile(this, baseType.Name + ".ascx");
            _isLogged = UserId != Null.NullInteger ? true : false;
            base.OnInit(e);
        }

		#endregion
    }
}
