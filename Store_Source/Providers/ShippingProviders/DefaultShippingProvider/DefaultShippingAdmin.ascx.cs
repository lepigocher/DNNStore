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

using DotNetNuke.Modules.Store.Core.Providers;

namespace DotNetNuke.Modules.Store.Providers.Shipping.DefaultShippingProvider
{
	/// <summary>
    /// Summary description for DefaultShippingProvider.
	/// </summary>
	public partial class DefaultShippingAdmin : ProviderControlBase
	{
        #region Private Members

        private readonly ShippingController _controller = new ShippingController();

        #endregion

        #region Events

        override protected void OnInit(EventArgs e)
		{
            grdShippingRates.ItemCommand += grdShippingRates_ItemCommand;
			btnSaveShippingFee.Click += btnSaveShippingFee_Click;
			base.OnInit(e);
		}

        protected void Page_Load(object sender, EventArgs e)
		{
			if (Page.IsPostBack == false)
                BindShippingRates();
		}

        private void grdShippingRates_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            // Add the new item to the database.
            if (e.CommandName == "Add" && Page.IsValid)
            {
                DataGridItem item = e.Item;

                TextBox txtNewDescription = (TextBox)item.FindControl("txtNewDescription");
                string description = txtNewDescription.Text;

                TextBox txtNewMinWeight = (TextBox)item.FindControl("txtNewMinWeight");
                Decimal newMinWeight = Decimal.Parse(txtNewMinWeight.Text);

                TextBox txtNewMaxWeight = (TextBox)item.FindControl("txtNewMaxWeight");
                Decimal newMaxWeight = Decimal.Parse(txtNewMaxWeight.Text);

                TextBox txtNewCost = (TextBox)item.FindControl("txtNewCost");
                Decimal newCost = Decimal.Parse(txtNewCost.Text);

                ShippingInfo newShippingInfo = new ShippingInfo
                {
                    Description = description,
                    MinWeight = newMinWeight,
                    MaxWeight = newMaxWeight,
                    Cost = newCost,
                    ApplyTaxRate = cbApplyTaxRate.Checked
                };

                _controller.AddShippingRate(PortalId, newShippingInfo);

                BindShippingRates();
            }
        }

		private void btnSaveShippingFee_Click(object sender, EventArgs e)
		{
            if (!Page.IsValid)
                return;

            bool applyTaxRate = cbApplyTaxRate.Checked;

            // Loop through the items in the datagrid.
            foreach (DataGridItem di in grdShippingRates.Items)
            {
                // Make sure this is an item and not the header or footer.
                if (di.ItemType == ListItemType.Item || di.ItemType == ListItemType.AlternatingItem)
                {
                    // Get the current row for update or delete operations later.
                    int ID = (int)grdShippingRates.DataKeys[di.ItemIndex];

                    // Check if this one needs to be deleted.
                    if (((CheckBox)di.FindControl("chkDelete")).Checked)
                    {
                        _controller.DeleteShippingRate(ID);
                    }
                    else
                    {
                        // Verify values
                        TextBox txtDescription = (TextBox)di.FindControl("txtDescription");
                        string description = txtDescription.Text;

                        TextBox txtMinWeight = (TextBox)di.FindControl("txtMinWeight");
                        Decimal minWeight = Decimal.Parse(txtMinWeight.Text);

                        TextBox txtMaxWeight = (TextBox)di.FindControl("txtMaxWeight");
                        Decimal maxWeight = Decimal.Parse(txtMaxWeight.Text);

                        TextBox txtCost = (TextBox)di.FindControl("txtCost");
                        Decimal cost = Decimal.Parse(txtCost.Text);

                        // Update the row
                        ShippingInfo shippingInfo = new ShippingInfo
                        {
                            ID = ID,
                            Description = description,
                            MinWeight = minWeight,
                            MaxWeight = maxWeight,
                            Cost = cost,
                            ApplyTaxRate = applyTaxRate
                        };

                        _controller.UpdateShippingRate(shippingInfo);
                    }
                }
            }

            BindShippingRates();
            InvokeEditComplete();
		}

		#endregion

        #region Private Methods

        private void BindShippingRates()
        {
            List<ShippingInfo> shippingRates = _controller.GetShippingRates(ParentControl.PortalId);
            grdShippingRates.DataSource = shippingRates;
            grdShippingRates.DataBind();

            if (shippingRates.Count > 0)
                cbApplyTaxRate.Checked = shippingRates[0].ApplyTaxRate;
        }

        #endregion
    }
}
