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
using System.Collections.Specialized;
using System.Web.UI.WebControls;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;

using DotNetNuke.Modules.Store.Core.Catalog;
using DotNetNuke.Modules.Store.Core.Components;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for CategoryAdmin.
	/// </summary>
	public partial class CategoryAdmin : StoreControlBase, IStoreTabedControl
	{
		#region Private Members

		private AdminNavigation _nav;
        private List<CategoryInfo> _categoryList;
        private string _navigateUrl;

        private SortDirection CategoriesSortDirection
        {
            get
            {
                if (Session["CategoriesSortDirection"] == null)
                    Session["CategoriesSortDirection"] = SortDirection.Ascending;
                return (SortDirection)Session["CategoriesSortDirection"];
            }
            set { Session["CategoriesSortDirection"] = value; }
        }

        private string CategoriesSortExpression
        {
            get
            {
                if (Session["CategoriesSortExpression"] == null)
                    Session["CategoriesSortExpression"] = "CategoryName";
                return (string)Session["CategoriesSortExpression"];
            }
            set { Session["CategoriesSortExpression"] = value; }
        }

        private int CategoriesPageIndex
        {
            get
            {
                if (Session["CategoriesPageIndex"] == null)
                    Session["CategoriesPageIndex"] = 0;
                return (int)Session["CategoriesPageIndex"];
            }
            set { Session["CategoriesPageIndex"] = value; }
        }

		#endregion

        #region Events

		override protected void OnInit(EventArgs e)
		{
			linkAddNew.Click += linkAddNew_Click;

            base.OnInit(e);
		}
		
        protected void Page_Load(object sender, EventArgs e)
		{
			_nav = new AdminNavigation(Request.QueryString);
            if (_nav.CategoryID != Null.NullInteger)
            {
                plhGrid.Visible = false;
                plhForm.Visible = true;
                
                if (_nav.CategoryID == 0)
                {
                    lblEditTitle.Text = Localization.GetString("AddCategory", LocalResourceFile);
                    LoadEditControl(Null.NullInteger);
                }
                else
                {
                    lblEditTitle.Text = Localization.GetString("EditCategory", LocalResourceFile);
                    LoadEditControl(_nav.CategoryID);
                }
            }
            else
            {
                plhGrid.Visible = true;
                plhForm.Visible = false;

                StringDictionary replaceParams = new StringDictionary
                {
                    ["CategoryID"] = "{0}"
                };
                _navigateUrl = _nav.GetNavigationUrl(replaceParams);

                if (IsPostBack == false)
                {
                    Localization.LocalizeGridView(ref gvCategories, LocalResourceFile);
                    SortAndBind();
                }
            }
		}

        protected void linkAddNew_Click(object sender, EventArgs e)
		{
			_nav.CategoryID = 0;
			Response.Redirect(_nav.GetNavigationUrl());
		}

        protected void editControl_EditComplete(object sender, EventArgs e)
		{
			_nav.CategoryID = Null.NullInteger;
			Response.Redirect(_nav.GetNavigationUrl());
		}

        protected void gvCategories_DataBinding(object sender, EventArgs e)
        {
            HyperLinkField hlfEdit = (HyperLinkField)gvCategories.Columns[gvCategories.Columns.Count - 1];
            hlfEdit.DataNavigateUrlFormatString = _navigateUrl;
        }

        protected void gvCategories_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int sortColumnIndex = GetSortColumnIndex();
                if (sortColumnIndex != -1)
                    AddSortImage(sortColumnIndex, e.Row);
            }
        }

        protected void gvCategories_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            // Reset sort if it's not the same column
            if (sortExpression != CategoriesSortExpression)
                CategoriesSortDirection = SortDirection.Ascending;
            else
            {
                // Invert sort order
                if (CategoriesSortDirection == SortDirection.Ascending)
                    CategoriesSortDirection = SortDirection.Descending;
                else
                    CategoriesSortDirection = SortDirection.Ascending;
            }
            CategoriesSortExpression = sortExpression;
            CategoriesPageIndex = 0;
            SortAndBind();
        }

        protected void gvCategories_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CategoriesPageIndex = e.NewPageIndex;
            SortAndBind();
        }

		#endregion

		#region Private Methods

		private void LoadEditControl(int categoryID)
		{
			plhEditControl.Controls.Clear();
            StoreControlBase editControl = (StoreControlBase)LoadControl(ControlPath + "CategoryEdit.ascx");
            editControl.ModuleConfiguration = ModuleConfiguration;
            editControl.StoreSettings = StoreSettings;
			editControl.DataSource = categoryID;
			editControl.EditComplete += editControl_EditComplete;
			plhEditControl.Controls.Add(editControl);
		}

        private void SortAndBind()
        {
            CategoryController categoryController = new CategoryController();
            _categoryList = categoryController.GetCategories(PortalId, false, -1);

            switch (CategoriesSortExpression)
            {
                case "CategoryName":
                    _categoryList.Sort();
                    break;
                case "ParentCategoryName":
                    _categoryList.Sort(CategoryInfo.ParentCategoryNameSorter);
                    break;
                case "OrderID":
                    _categoryList.Sort(CategoryInfo.OrderIDSorter);
                    break;
                case "CreatedDate":
                    _categoryList.Sort(CategoryInfo.CreatedDateSorter);
                    break;
                default:
                    break;
            }

            if (CategoriesSortDirection == SortDirection.Descending)
                _categoryList.Reverse();

            gvCategories.PageIndex = CategoriesPageIndex;
            gvCategories.DataSource = _categoryList;
            gvCategories.DataBind();
        }

        private int GetSortColumnIndex()
        {
            foreach (DataControlField field in gvCategories.Columns)
            {
                if (field.SortExpression == CategoriesSortExpression)
                    return gvCategories.Columns.IndexOf(field);
            }
            return Null.NullInteger;
        }

        private void AddSortImage(int columnIndex, GridViewRow headerRow)
        {
            // Create the sorting image based on the sort direction.
            Image sortImage = new Image();

            if (CategoriesSortDirection == SortDirection.Ascending)
            {
                sortImage.ImageUrl = "~/images/sortascending.gif";
                sortImage.AlternateText = Localization.GetString("SortAscending", LocalResourceFile);
            }
            else
            {
                sortImage.ImageUrl = "~/images/sortdescending.gif";
                sortImage.AlternateText = Localization.GetString("SortDescending", LocalResourceFile);
            }

            // Add the image to the appropriate header cell.
            headerRow.Cells[columnIndex].Controls.Add(sortImage);
        }

		#endregion

        #region IStoreTabedControl Members

        string IStoreTabedControl.Title
        {
            get { return Localization.GetString("lblParentTitle", LocalResourceFile); }
        }

        #endregion
    }
}
