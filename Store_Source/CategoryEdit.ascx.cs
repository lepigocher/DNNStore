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
using System.Collections.Generic;
using System.Web.UI.WebControls;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;

using DotNetNuke.Modules.Store.Core.Catalog;
using DotNetNuke.Modules.Store.Core.Components;

namespace DotNetNuke.Modules.Store.WebControls
{
	public partial  class CategoryEdit : StoreControlBase
	{
		#region Private Members

        private readonly CategoryController _controller = new CategoryController();
		private int _categoryId = Null.NullInteger;
		
		#endregion

		#region Events

		protected void Page_Load(object sender, EventArgs e)
		{
			// Get Category ID
			_categoryId = (int)DataSource;

			if (Page.IsPostBack == false)
			{
				cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");

                //Bind parent category list...
                ddlParentCategory.DataSource = _controller.GetCategoriesPath(PortalId, true, Null.NullInteger);
                ddlParentCategory.DataTextField = "CategoryPathName";
                ddlParentCategory.DataValueField = "CategoryID";
                ddlParentCategory.DataBind();
                ddlParentCategory.Items.Insert(0, new ListItem(Localization.GetString("None", LocalResourceFile), "-1"));

                if (_categoryId != Null.NullInteger)
                {
                    // Search the selected category and disable it as parent
                    ListItem item = ddlParentCategory.Items.FindByValue(_categoryId.ToString());
                    if (item != null)
                        item.Enabled = false;
                    // Get the selected category to edit
                    CategoryInfo category = _controller.GetCategory(PortalId, _categoryId);
                    if (category != null)
                    {
                        txtCategoryName.Text = category.Name;
                        if (StoreSettings.SEOFeature)
                        {
                            txtSEOName.Text = category.SEOName;
                            txtCategoryKeywords.Text = category.Keywords;
                        }
                        else
                        {
                            trSEOName.Visible = false;
                            trKeywords.Visible = false;
                        }
                        txtDescription.Text = category.Description;
                        chkArchived.Checked = category.Archived;
                        txtMessage.Text = category.Message;
                        txtOrder.Text = category.OrderID.ToString();
                        ListItem parent = ddlParentCategory.Items.FindByValue(category.ParentCategoryID.ToString());
                        if (parent != null)
                            parent.Selected = true;
                        cmdDelete.Visible = true;
                    }
                }
                else
                {
                    trSEOName.Visible = false;
                    trKeywords.Visible = false;
                    txtOrder.Text = "0";
                }
			}
		}

        protected void valCustomParentCategory_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int parentCategoryID = int.Parse(args.Value);
            if (parentCategoryID != Null.NullInteger)
                args.IsValid = RecursionCheckPassed(_categoryId, parentCategoryID);
            else
                args.IsValid = true;
        }

        protected void cmdSuggest_Click(object sender, EventArgs e)
        {
            txtSEOName.Text = Normalization.RemoveDiacritics(txtCategoryName.Text, '-').ToLower();
        }

		protected void cmdUpdate_Click(object sender, EventArgs e)
		{
		    if (Page.IsValid) 
		    {
                CategoryInfo category = new CategoryInfo
                {
                    CategoryID = _categoryId,
                    PortalID = PortalId,
                    Name = txtCategoryName.Text,
                    Description = txtDescription.Text,
                    OrderID = Convert.ToInt32(txtOrder.Text),
                    ParentCategoryID = Convert.ToInt32(ddlParentCategory.SelectedItem.Value),
                    Archived = chkArchived.Checked,
                    Message = txtMessage.Text,
                    CreatedByUser = UserId.ToString(),
                    CreatedDate = DateTime.Now
                };

                if (StoreSettings.SEOFeature)
                {
                    category.SEOName = txtSEOName.Text;
                    category.Keywords = txtCategoryKeywords.Text;
                }

			    if (_categoryId == Null.NullInteger)
				    _controller.AddCategory(category);
			    else 
                    _controller.UpdateCategory(category);
                InvokeEditComplete();
		    }
		}

		protected void cmdCancel_Click(object sender, EventArgs e)
		{
			InvokeEditComplete();
		}

		protected void cmdDelete_Click(object sender, EventArgs e)
		{
            try
            {
                if (_categoryId != Null.NullInteger)
                    _controller.DeleteCategory(_categoryId);
                InvokeEditComplete();
            }
            catch
            {
                string errorDelete = Localization.GetString("ErrorDelete", LocalResourceFile);
                Exceptions.ProcessModuleLoadException(errorDelete, this, new Exception(""));
            }
		}

		#endregion

        #region Private Methods

        private bool RecursionCheckPassed(int categoryId, int parentCategoryId)
        {
            bool isSafe = true;

            if (categoryId != Null.NullInteger)
            {
                List<int> categoryTree = new List<int>(10)
                {
                    categoryId,
                    parentCategoryId
                };

                do
                {
                    CategoryInfo parentCategory = _controller.GetCategory(PortalId, parentCategoryId);
                    int parentId = parentCategory.ParentCategoryID;
                    if (categoryTree.Contains(parentId) == false)
                    {
                        categoryTree.Add(parentId);
                        parentCategoryId = parentId;
                    }
                    else
                    {
                        isSafe = false;
                        break;
                    }
                } while (parentCategoryId > 0);
            }

            return isSafe;
        }

        #endregion
    }
}
