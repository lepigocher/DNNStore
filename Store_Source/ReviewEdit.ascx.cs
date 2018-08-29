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
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Security;
using DotNetNuke.Security.Roles;

using DotNetNuke.Modules.Store.Core.Catalog;
using DotNetNuke.Modules.Store.Core.Components;

namespace DotNetNuke.Modules.Store.WebControls
{
	public partial class ReviewEdit : StoreControlBase
	{
		#region Private Members

		private AdminNavigation _nav;

		#endregion
		
		#region Events

		override protected void OnInit(EventArgs e)
		{
			cmdUpdate.Click += cmdUpdate_Click;
			cmdCancel.Click += cmdCancel_Click;
			cmdDelete.Click += cmdDelete_Click;
			base.OnInit(e);
		}
		
		protected void Page_Load(object sender, EventArgs e)
		{
			_nav = new AdminNavigation(Request.QueryString);

			try 
			{
				// Get the Review ID
				ReviewInfo review = new ReviewInfo();

				if (!Page.IsPostBack) 
				{
					cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");
					if (_nav.ReviewID != Null.NullInteger) 
					{
						ReviewController controller = new ReviewController();
						review = controller.GetReview(PortalId, _nav.ReviewID);
                        if (review != null)
                        {
                            cmdDelete.Visible = true;
                            divApproval.Visible = true;
                            cmbRating.SelectedValue = review.Rating.ToString();
                            txtComments.Text = review.Comments;
                            chkAuthorized.Checked = review.Authorized;
                            txtUserName.Text = review.UserName;
                        }
                        else
                        {
                            if (UserId != -1)
                            {
                                txtUserName.Text = UserInfo.DisplayName;
                            }
                            else
                            {
                                txtUserName.Text = Localization.GetString("Anonymous.Text", LocalResourceFile);
                            }
                        }
					}
				}

				// Which controls do we display?
                if (CanManageReviews())
                {
                    txtUserName.Enabled = false;
                    cmbRating.Enabled = false;
                    divAuthorized.Visible = true;
                    divApproval.Visible = false;
                }
			} 
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

        protected void cmdUpdate_Click(object sender, EventArgs e)
		{
			try 
			{
				if (Page.IsValid) 
				{
					PortalSecurity security = new PortalSecurity();

					ReviewInfo review = new ReviewInfo();
					review = ((ReviewInfo)CBO.InitializeObject(review, typeof(ReviewInfo)));
					review.ReviewID		= _nav.ReviewID;
					review.PortalID		= PortalId;
					review.ProductID	= _nav.ProductID;
					review.Rating		= int.Parse(cmbRating.SelectedValue);
					review.Comments		= security.InputFilter(txtComments.Text, PortalSecurity.FilterFlag.NoMarkup);
					review.Authorized	= chkAuthorized.Checked;
				    string userName = txtUserName.Text;
                    if (!string.IsNullOrEmpty(userName))
                    {
                        review.UserName = security.InputFilter(userName, PortalSecurity.FilterFlag.NoMarkup);
                    }
                    else
                    {
                        review.UserName = Localization.GetString("Anonymous.Text", LocalResourceFile);
                    }
					review.CreatedDate	= DateTime.Now;

					ReviewController controller = new ReviewController();
					if (_nav.ReviewID == 0)
					{
						controller.AddReview(review);
					} 
					else 
					{
						controller.UpdateReview(review);
					}

					InvokeEditComplete();
				}
			} 
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

        protected void cmdCancel_Click(object sender, EventArgs e)
		{
			try 
			{
				InvokeEditComplete();
			} 
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

        protected void cmdDelete_Click(object sender, EventArgs e)
		{
			try 
			{
				if (!Null.IsNull(_nav.ReviewID)) 
				{
					ReviewController controller = new ReviewController();
					controller.DeleteReview(_nav.ReviewID);

					_nav.ReviewID = Null.NullInteger;
				}

				InvokeEditComplete();
			} 
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		#endregion

		#region Private Methods

        private bool CanManageReviews()
        {
            if (UserInfo.IsSuperUser || UserInfo.IsInRole("Administrators"))
                return true;

            if (StoreSettings.CatalogRoleID != Null.NullInteger)
            {
                RoleController roleController = new RoleController();
                RoleInfo catalogRole = roleController.GetRoleById(PortalId, StoreSettings.CatalogRoleID);
                if (UserInfo.IsInRole(catalogRole.RoleName))
                    return true;
            }

            return false;
        }

		#endregion
	}
}
