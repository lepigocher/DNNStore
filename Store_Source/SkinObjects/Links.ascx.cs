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

using DotNetNuke.Common;
using DotNetNuke.Services.Localization;

using DotNetNuke.Modules.Store.Core.Admin;

namespace DotNetNuke.Modules.Store.SkinObjects
{
    public partial class Links : UI.Skins.SkinObjectBase
    {
        #region Private Members

        private string _imageCssClass = "Normal";
        private string _imageName = "cart.png";
        private string _linkAction = "Cart";
        private string _textCssClass = "Normal";
        private bool _textVisible = true;

        #endregion

        #region Properties

        public string ImageCssClass
        {
            get { return _imageCssClass; }
            set { _imageCssClass = value; }
        }

        public string ImageName
        {
            get { return _imageName; }
            set { _imageName = value; }
        }

        public string LinkAction
        {
            get { return _linkAction; }
            set { _linkAction = value; }
        }

        public string TextCssClass
        {
            get { return _textCssClass; }
            set { _textCssClass = value; }
        }

        public bool TextVisible
        {
            get { return _textVisible; }
            set { _textVisible = value; }
        }

        #endregion

        #region Events

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!IsPostBack)
            {
                string resource = TemplateSourceDirectory + "/App_LocalResources/Links.ascx.resx";
                string text;

                StoreInfo storeInfo = StoreController.GetStoreInfo(PortalSettings.PortalId);

                if (storeInfo != null)
                {
                    try
                    {
                        phControls.Visible = true;
                        int tabID;

                        switch (_linkAction.ToLower())
                        {
                            case "cart":
                                text = Localization.GetString("CartTitle.Text", resource);
                                tabID = storeInfo.ShoppingCartPageID;
                                break;
                            case "store":
                                text = Localization.GetString("StoreTitle.Text", resource);
                                tabID = storeInfo.StorePageID;
                                break;
                            default:
                                text = Localization.GetString("UnknowAction.Text", resource);
                                tabID = PortalSettings.HomeTabId;
                                _textVisible = true;
                                break;
                        }

                        if (_textVisible)
                        {
                            lnkAction.CssClass = _textCssClass;
                            lnkAction.Text = text;
                            lnkAction.PostBackUrl = Globals.NavigateURL(tabID);
                        }
                        lnkAction.Visible = _textVisible;

                        if (!string.IsNullOrEmpty(_imageName))
                        {
                            if (storeInfo.PortalTemplates)
                            {
                                btnImage.ImageUrl = PortalSettings.HomeDirectory + "Store/Templates/Images/" + _imageName;
                            }
                            else
                            {
                                btnImage.ImageUrl = TemplateSourceDirectory + "/../Templates/Images/" + _imageName;
                            }
                            btnImage.CssClass = _imageCssClass;
                            btnImage.ToolTip = text;
                            btnImage.PostBackUrl = Globals.NavigateURL(tabID);
                        }
                        else
                        {
                            btnImage.Visible = false;
                        }
                    }
                    catch
                    {
                        lnkAction.CssClass = _textCssClass;
                        text = Localization.GetString("Error.Text", resource);
                        lnkAction.Text = text;
                        btnImage.Visible = false;
                    }
                }
                else
                    phControls.Visible = false;
            }
        }

        #endregion
    }
}
