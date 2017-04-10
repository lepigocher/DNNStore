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
using System.Collections;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Modules.Store.Catalog;

namespace DotNetNuke.Modules.Store.WebControls
{
	public partial  class CategorySettings : ModuleSettingsBase
	{
		#region Private Members

		private ModuleSettings _settings;
		
		#endregion

		#region Events

		override protected void OnInit(EventArgs e)
		{
			_settings = new ModuleSettings(ModuleId, TabId);
			base.OnInit(e);
		}
		
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
                if (cmbDisplayMode.SelectedValue == "T")
                    trColumnCount.Visible = true;
                else
                    trColumnCount.Visible = false;
			}
			catch(Exception ex)
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		#endregion

		#region ModuleSettingsBase Implementation

		public override void LoadSettings()
		{
			try
			{
				if (!Page.IsPostBack)
				{
                    // Fill display mode combo
                    cmbDisplayMode.Items.Add(new ListItem(Localization.GetString("TableMode", LocalResourceFile), "T"));
                    cmbDisplayMode.Items.Add(new ListItem(Localization.GetString("ListMode", LocalResourceFile), "L"));
                    cmbDisplayMode.SelectedValue = _settings.CategoryMenu.DisplayMode;

					// Fill Catalog Page combo
					TabController tabController = new TabController();
					ArrayList tabs = tabController.GetTabs(PortalId);

					cmbCatalogPage.Items.Add(new ListItem(Localization.GetString("SamePage", LocalResourceFile), "0"));

					foreach (TabInfo tabInfo in tabs)
					{
						if (tabInfo.IsVisible && !tabInfo.IsDeleted && !tabInfo.IsAdminTab && !tabInfo.IsSuperTab)
						{
							cmbCatalogPage.Items.Add(new ListItem(tabInfo.TabName, tabInfo.TabID.ToString()));
						}
					}

					// Get values from settings
					txtColumnCount.Text = _settings.CategoryMenu.ColumnCount.ToString();

					int catalogTabID = _settings.CategoryMenu.CatalogPage;
					if (catalogTabID <= 0)
					{
						cmbCatalogPage.SelectedIndex = 0;
					}
					else
					{
						cmbCatalogPage.SelectedValue = catalogTabID.ToString();
					}
				}
			}
			catch(Exception ex)
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		public override void UpdateSettings()
		{
			try
			{
                _settings.CategoryMenu.DisplayMode = cmbDisplayMode.SelectedValue;
				_settings.CategoryMenu.ColumnCount = int.Parse(txtColumnCount.Text);
				_settings.CategoryMenu.CatalogPage = int.Parse(cmbCatalogPage.SelectedValue);
			}
			catch(Exception ex)
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		#endregion
	}
}
