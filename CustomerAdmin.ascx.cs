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
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Customer;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for CustomerAdmin.
	/// </summary>
	public partial  class CustomerAdmin : StoreControlBase, IStoreTabedControl
	{
		#region Private Members

		private AdminNavigation _nav;

		#endregion

		#region Events

		override protected void OnInit(EventArgs e)
		{
            lstCustomers.SelectedIndexChanged += lstCustomers_SelectedIndexChanged;
            lstOrderStatus.SelectedIndexChanged += lstOrderStatus_SelectedIndexChanged;
			base.OnInit(e);
		}
		
		protected void Page_Load(object sender, EventArgs e)
		{
			_nav = new AdminNavigation(Request.QueryString);

            if (!IsPostBack)
            {
                OrderController orderController = new OrderController();
                lstCustomers.DataTextField = "FullName";
                lstCustomers.DataValueField = "UserID";
                lstCustomers.DataSource = orderController.GetCustomers(PortalId);
                lstCustomers.DataBind();
                lstCustomers.Items.Insert(0, new ListItem(Localization.GetString("Select", LocalResourceFile), ""));

                lstOrderStatus.DataTextField = "OrderStatusText";
                lstOrderStatus.DataValueField = "OrderStatusID";
                lstOrderStatus.DataSource = orderController.GetOrderStatuses();
                lstOrderStatus.DataBind();
                lstOrderStatus.Items.Insert(0, new ListItem(Localization.GetString("Select", LocalResourceFile), ""));
            }

            if (_nav.CustomerID != Null.NullInteger || _nav.StatusID != Null.NullInteger || _nav.OrderID != Null.NullInteger)
            {
                CustomerOrders ordersControl = (CustomerOrders)LoadControl(ModulePath + "CustomerOrders.ascx");
                ordersControl.ModuleConfiguration = ModuleConfiguration;
                ordersControl.StoreSettings = StoreSettings;
                ordersControl.ID = "CustomerOrders";

                if (_nav.CustomerID != Null.NullInteger)
                {
                    try
                    {
                        lstCustomers.SelectedValue = _nav.CustomerID.ToString();
                        ordersControl.ShowOrdersInStatus = false;
                        ordersControl.OrderStatusID = Null.NullInteger;
                    }
                    catch
                    {
                        // This occurs when an order has been selected by is number
                        // and the user account was deleted.
                        // Because we can't show the user's orders when the back button is cliqued,
                        // we return to the orders admin start page.
                        _nav = new AdminNavigation();
                        _nav.PageID = "CustomerAdmin";
                        Response.Redirect(_nav.GetNavigationUrl(), true);
                    }
                }

                if (_nav.StatusID != Null.NullInteger)
                {
                    lstOrderStatus.SelectedValue = _nav.StatusID.ToString();
                    ordersControl.ShowOrdersInStatus = true;
                    ordersControl.OrderStatusID = _nav.StatusID;
                }

                if (_nav.OrderID != Null.NullInteger)
                    tbOrderNumber.Text = _nav.OrderID.ToString();

                plhOrders.Controls.Clear();
                plhOrders.Controls.Add(ordersControl);
                plhOrders.Visible = true;
            }
            else
            {
                tbOrderNumber.Text = "";
                lstCustomers.ClearSelection();
                lstOrderStatus.ClearSelection();
                plhOrders.Controls.Clear();
                plhOrders.Visible = false;
            }
		}

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (tbOrderNumber.Text.Length > 0)
            {
                int orderID = _nav.OrderID;
                if (int.TryParse(tbOrderNumber.Text, out orderID))
                {
                    OrderController orderController = new OrderController();
                    OrderInfo orderInfo = orderController.GetOrder(PortalId, orderID);
                    if (orderInfo != null)
                    {
                        _nav.CustomerID = orderInfo.CustomerID;
                        _nav.OrderID = orderID;
                        _nav.StatusID = Null.NullInteger;
                        Response.Redirect(_nav.GetNavigationUrl(), true);
                    }
                }
                lstCustomers.ClearSelection();
                lstOrderStatus.ClearSelection();
                plhOrders.Controls.Clear();
                plhOrders.Visible = false;
                noOrdersFound.Visible = true;
            }
        }

        private void lstCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCustomers.SelectedValue != "")
                _nav.CustomerID = Convert.ToInt32(lstCustomers.SelectedValue);
            else
                _nav.CustomerID = Null.NullInteger;
            lstOrderStatus.ClearSelection();
            _nav.OrderID = Null.NullInteger;
            _nav.StatusID = Null.NullInteger;
            Response.Redirect(_nav.GetNavigationUrl(), true);
        }

        private void lstOrderStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstOrderStatus.SelectedValue != "")
                _nav.StatusID = Convert.ToInt32(lstOrderStatus.SelectedValue);
            else
                _nav.StatusID = Null.NullInteger;
            lstCustomers.ClearSelection();
            _nav.OrderID = Null.NullInteger;
            _nav.CustomerID = Null.NullInteger;
            Response.Redirect(_nav.GetNavigationUrl(), true);
        }

		#endregion

        #region IStoreTabedControl Membres

        string IStoreTabedControl.Title
        {
            get { return Localization.GetString("lblParentTitle", LocalResourceFile); }
        }

        #endregion
    }
}
