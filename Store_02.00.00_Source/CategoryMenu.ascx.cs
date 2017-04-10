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
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Modules.Store.Catalog;
using DotNetNuke.Modules.Store.Components;

namespace DotNetNuke.Modules.Store.WebControls
{
    public partial class CategoryMenu : StoreControlBase, IActionable
	{
		#region Private Members

		private ModuleSettings _settings;
        private CatalogNavigation _nav;
        private CatalogNavigation _categoryNav;
        private int _selectedCategoryID;
        private int _catalogTabID;
        private CategoryController _categoryController;
        private List<CategoryInfo> _parentCategories;

		#endregion

		#region Events

		override protected void OnInit(EventArgs e)
		{
            if (StoreSettings != null)
            {
                _settings = new ModuleSettings(ModuleId, TabId);
                _catalogTabID = _settings.CategoryMenu.CatalogPage;
                if (_catalogTabID <= 0)
                    _catalogTabID = StoreSettings.StorePageID;
                _categoryNav = new CatalogNavigation();
                _categoryNav.TabID = _catalogTabID;
                _categoryController = new CategoryController();
                _parentCategories = new List<CategoryInfo>();
                if (_settings.CategoryMenu.DisplayMode == "T")
                {
                    MyList.Visible = true;
                    MyList.ItemDataBound += MyList_ItemDataBound;
                    MyList.ItemCommand += MyList_ItemCommand;
                }
                else
                    MyList.Visible = false;
            }
			base.OnInit(e);
		}
		
		protected void Page_Load(object sender, EventArgs e)
		{
            if (StoreSettings != null)
            {
                try
                {
                    // Load utility objects
                    _nav = new CatalogNavigation(Request.QueryString);
                    _nav.ProductID = Null.NullInteger;	//Product should not be displayed!
                    if (_nav.CategoryID == 0)
                        _nav.CategoryID = Null.NullInteger;

                    // Get selected category and parent categories (if any)
                    _selectedCategoryID = _nav.CategoryID;
                    if (_selectedCategoryID != Null.NullInteger && _settings.CategoryMenu.DisplayMode == "T")
                    {
                        CategoryInfo category = _categoryController.GetCategory(PortalId, _selectedCategoryID);
                        _parentCategories.Add(category);
                        if (category.CategoryID != category.ParentCategoryID)
                        {
                            while (category.ParentCategoryID != Null.NullInteger)
                            {
                                category = _categoryController.GetCategory(PortalId, category.ParentCategoryID);
                                _parentCategories.Add(category);
                                foreach (CategoryInfo cat in _parentCategories)
                                {
                                    if (cat.CategoryID == category.CategoryID)
                                    {
                                        //Cyclical categories found
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (_parentCategories.Count > 0)
                        _parentCategories.Reverse();

                    string templatePath = CssTools.GetTemplatePath(this, StoreSettings.PortalTemplates);
                    CssTools.AddCss(Page, templatePath, StoreSettings.StyleSheet);

                    if (_settings.CategoryMenu.DisplayMode == "T")
                    {
                        // Databind to list of categories
                        List<CategoryInfo> categoryList = _categoryController.GetCategories(PortalId, false, -2);
                        MyList.RepeatColumns = _settings.CategoryMenu.ColumnCount;
                        MyList.DataSource = categoryList;
                        MyList.DataBind();
                    }
                    else
                    {
                        Control ulMenu = CreateULMenu(-2, 0);
                        if (ulMenu != null)
                            divStoreMenu.Controls.Add(ulMenu);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                        Exceptions.ProcessModuleLoadException(this, ex.InnerException);
                    else
                        Exceptions.ProcessModuleLoadException(this, ex);
                }
            }
            else
            {
                if (UserInfo.IsSuperUser)
                {
                    string errorSettings = Localization.GetString("ErrorSettings", LocalResourceFile);
                    string errorSettingsHeading = Localization.GetString("ErrorSettingsHeading", LocalResourceFile);
                    UI.Skins.Skin.AddModuleMessage(this, errorSettingsHeading, errorSettings, UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning);
                }
                else
                    MyList.Visible = false;
            }
		}

		private void MyList_ItemDataBound(object sender, DataListItemEventArgs e)
		{
			// Do we have the category info?
			CategoryInfo category = (e.Item.DataItem as CategoryInfo);
			if (category != null)
			{
				// Did we find the hyperlink?
				HyperLink linkCategory = (e.Item.FindControl("linkCategory") as HyperLink);
				if (linkCategory != null)
				{
					// Build the nav URL for this item
                    _categoryNav.CategoryID = category.CategoryID;
                    if (StoreSettings.SEOFeature)
                        _categoryNav.Category = category.SEOName;
                    // Define hyperlink properties
                    linkCategory.NavigateUrl = _categoryNav.GetNavigationUrl();
                    linkCategory.ToolTip = category.Description;
                    //Set styling...
                    string indentClass = " StoreMenuCategoryItemLevel_0";
                    if (_selectedCategoryID == category.CategoryID)
                        linkCategory.CssClass = "StoreMenuCategoryItemSelected" + indentClass;
                    else
                        linkCategory.CssClass = "StoreMenuCategoryItem" + indentClass;

                    if (_parentCategories.Count > 0) 
                    {
                        // If equal, this is a root cat that needs to be expanded...
                        if (category.CategoryID == _parentCategories[0].CategoryID)
                            DisplayChildCategories(_parentCategories[0], e.Item, 1);
                    }
				}
			}
		}

		private void MyList_ItemCommand(object source, DataListCommandEventArgs e)
		{
            if (e.CommandName.ToLower() == "edit")
                EditCategory(e.CommandArgument.ToString());
		}

        void ibEdit_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "edit")
                EditCategory(e.CommandArgument.ToString());
        }

		#endregion

		#region Private Methods

        private void EditCategory(string categoryID)
        {
            CatalogNavigation editNav = new CatalogNavigation();
            editNav.Edit = "Category";
            editNav.CategoryID = int.Parse(categoryID);
            Response.Redirect(editNav.GetEditUrl(ModuleId));
        }

        private void DisplayChildCategories(CategoryInfo category, DataListItem dataListItem, int indentLevel)
        {
            List<CategoryInfo> childCategories = _categoryController.GetCategories(PortalId, false, category.CategoryID);
            foreach (CategoryInfo childCategory in childCategories)
            {
                _categoryNav.CategoryID = childCategory.CategoryID;
                if (StoreSettings.SEOFeature)
                    _categoryNav.Category = childCategory.SEOName;
                dataListItem.Controls.Add(GetBreakRow(indentLevel));
                dataListItem.Controls.Add(CreateHyperLink(childCategory.Name, _categoryNav.GetNavigationUrl(), childCategory.Description, indentLevel, _selectedCategoryID == childCategory.CategoryID));

                //If this category is in the parent array, then recurse...
                foreach (CategoryInfo myCategory in _parentCategories)
                {
                    if (myCategory.CategoryID == childCategory.CategoryID)
                    {
                        int newIndentLevel = indentLevel + 1;
                        DisplayChildCategories(childCategory, dataListItem, newIndentLevel);
                    }
                }
            }
        }

        private HyperLink CreateHyperLink(string text, string navigateURL, string toolTip, int indentLevel, bool selectedCategory)
        {
            HyperLink link = new HyperLink();
            link.Text = text;
            link.NavigateUrl = navigateURL;
            link.ToolTip = toolTip;
            string indentClass = " StoreMenuCategoryItemLevel_" + indentLevel;

            //Set styling...
            if (selectedCategory)
                link.CssClass = "StoreMenuCategoryItemSelected" + indentClass;
            else
                link.CssClass = "StoreMenuCategoryItem" + indentClass;

            return link;
        }

        private LiteralControl GetBreakRow(int indentationLevel)
        {
            LiteralControl literalControl = new LiteralControl();         
            literalControl.Text += "<br />";
            for (int i = 1; i <= indentationLevel; i++)
                literalControl.Text += GetIndentString();
            return literalControl;
        }

        private string GetIndentString()
        {
            return "&nbsp;&nbsp;&nbsp;";
        }

        private Control CreateULMenu(int categoryID, int level)
        {
            List<CategoryInfo> categoryList = _categoryController.GetCategories(PortalId, false, categoryID);

            if (categoryList.Count > 0)
            {
                HtmlGenericControl ulContainer = new HtmlGenericControl("ul");
                ulContainer.Attributes.Add("class", "StoreMenuCategoryContainer StoreMenuCategoryContainerLevel_" + level);
                foreach (CategoryInfo category in categoryList)
                {
                    HtmlGenericControl liCategory = new HtmlGenericControl("li");
                    _categoryNav.CategoryID = category.CategoryID;
                    if (StoreSettings.SEOFeature)
                        _categoryNav.Category = category.SEOName;
                    if (category.CategoryID == _selectedCategoryID)
                        liCategory.Attributes.Add("class", "StoreMenuCategoryListItemSelected StoreMenuCategoryListItemLevel_" + level);
                    else
                        liCategory.Attributes.Add("class", "StoreMenuCategoryListItem StoreMenuCategoryListItemLevel_" + level);
                    if (IsEditable)
                    {
                        ImageButton ibEdit = new ImageButton();
                        ibEdit.ImageUrl = "~/images/edit.gif";
                        ibEdit.CommandName = "edit";
                        ibEdit.CommandArgument = category.CategoryID.ToString();
                        ibEdit.Command += ibEdit_Command;
                        liCategory.Controls.Add(ibEdit);
                    }
                    liCategory.Controls.Add(CreateHyperLink(category.Name, _categoryNav.GetNavigationUrl(), category.Description, level, _selectedCategoryID == category.CategoryID));
                    Control childs = CreateULMenu(category.CategoryID, (level + 1));
                    if (childs != null)
                        liCategory.Controls.Add(childs);
                    ulContainer.Controls.Add(liCategory);
                }
                return ulContainer;
            }

            return null;
        }

		#endregion

		#region IActionable Members

        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection actions = new ModuleActionCollection();
                if (StoreSettings != null)
                {
                    CatalogNavigation editNav = new CatalogNavigation(ModuleId, Request.QueryString);
                    editNav.ProductID = Null.NullInteger;
                    editNav.PageIndex = Null.NullInteger;
                    editNav.CategoryID = Null.NullInteger;
                    editNav.Edit = "Category";

                    actions.Add(GetNextActionID(), Localization.GetString("AddNewCategory", LocalResourceFile), ModuleActionType.AddContent, "", "", editNav.GetEditUrl(), false, SecurityAccessLevel.Edit, true, false);
                }
                return actions;
            }
        }

		#endregion
	}
}
