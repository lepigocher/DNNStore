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

using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;

using DotNetNuke.Modules.Store.API;

namespace DotNetNuke.Modules.Store.WebControls
{
    public partial class ServicesSettings : ModuleSettingsBase
    {
        #region Private Members

        private ModuleSettings _settings;
        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            _settings = new ModuleSettings(ModuleId, TabId);

            base.OnInit(e);
        }

        #endregion

        #region Base Method Implementations

        public override void LoadSettings()
        {
            try
            {
                txtScriptObjectName.Text = _settings.ScriptObjectName;
                txtOptions.Text = _settings.Options;
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        public override void UpdateSettings()
        {
            try
            {
                _settings.ScriptObjectName = txtScriptObjectName.Text;
                _settings.Options = txtOptions.Text;
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        #endregion
    }
}