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
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Store.Providers.Shipping.DefaultShippingProvider
{
	/// <summary>
    /// Summary description for DefaultShippingProvider.
	/// </summary>
	public partial class DefaultShippingAdmin : ProviderControlBase
	{
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
            if (e.CommandName == "Add")
            {
                TextBox txtNewDescription = (TextBox)e.Item.FindControl("txtNewDescription");
                string description = txtNewDescription.Text;
                if (string.IsNullOrEmpty(description))
                {
                    ShowError(Localization.GetString("ErrorRateDescription", LocalResourceFile), txtNewDescription);
                    return;
                }
                ClearError(txtNewDescription);

                TextBox txtNewMinWeight = (TextBox)e.Item.FindControl("txtNewMinWeight");
                Decimal newMinWeight;
                try
                {
                    newMinWeight = Decimal.Parse(txtNewMinWeight.Text);
                    ClearError(txtNewMinWeight);
                }
                catch (Exception)
                {
                    ShowError(Localization.GetString("ErrorMinWeight", LocalResourceFile), txtNewMinWeight);
                    return;
                }

                TextBox txtNewMaxWeight = (TextBox)e.Item.FindControl("txtNewMaxWeight");
                Decimal newMaxWeight;
                try
                {
                    newMaxWeight = Decimal.Parse(txtNewMaxWeight.Text);
                    ClearError(txtNewMaxWeight);
                }
                catch (Exception)
                {
                    ShowError(Localization.GetString("ErrorMaxWeight", LocalResourceFile), txtNewMaxWeight);
                    return;
                }

                TextBox txtNewCost = (TextBox)e.Item.FindControl("txtNewCost");
                Decimal newCost;
                try
                {
                    newCost = Decimal.Parse(txtNewCost.Text);
                    ClearError(txtNewCost);
                }
                catch (Exception)
                {
                    ShowError(Localization.GetString("ErrorCost", LocalResourceFile), txtNewCost);
                    return;
                }

                ShippingInfo newShippingInfo = new ShippingInfo();
                newShippingInfo.Description = description;
                newShippingInfo.MinWeight = newMinWeight;
                newShippingInfo.MaxWeight = newMaxWeight;
                newShippingInfo.Cost = newCost;
                newShippingInfo.ApplyTaxRate = cbApplyTaxRate.Checked;

                ShippingController controller = new ShippingController();
                controller.AddShippingRate(PortalId, newShippingInfo);

                BindShippingRates();
                lblError.Visible = false;
            }
        }

		private void btnSaveShippingFee_Click(object sender, EventArgs e)
		{
            ShippingController controller = new ShippingController();
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
                        controller.DeleteShippingRate(ID);
                    }
                    else
                    {
                        // Verify values
                        TextBox lblDescription = (TextBox)di.FindControl("lblDescription");
                        string description = lblDescription.Text;
                        if (string.IsNullOrEmpty(description))
                        {
                            ShowError(Localization.GetString("ErrorRateDescription", LocalResourceFile), lblDescription);
                            return;
                        }
                        ClearError(((TextBox)di.FindControl("lblDescription")));

                        TextBox lblMinWeight = (TextBox)di.FindControl("lblMinWeight");
                        Decimal minWeight;
                        try
                        {
                            minWeight = Decimal.Parse(lblMinWeight.Text);
                            ClearError(lblMinWeight);
                        }
                        catch (Exception)
                        {
                            ShowError(Localization.GetString("ErrorMinWeight", LocalResourceFile), lblMinWeight);
                            return;
                        }

                        TextBox lblMaxWeight = (TextBox)di.FindControl("lblMaxWeight");
                        Decimal maxWeight;
                        try
                        {
                            maxWeight = Decimal.Parse(lblMaxWeight.Text);
                            ClearError(lblMaxWeight);
                        }
                        catch (Exception)
                        {
                            ShowError(Localization.GetString("ErrorMaxWeight", LocalResourceFile), lblMaxWeight);
                            return;
                        }
                        if (maxWeight < minWeight)
                        {
                            ShowError(Localization.GetString("ErrorMaxLTMin", LocalResourceFile), lblMaxWeight);
                            return;
                        }

                        TextBox lblCost = (TextBox)di.FindControl("lblCost");
                        Decimal cost;
                        try
                        {
                            cost = Decimal.Parse(lblCost.Text);
                            ClearError(lblCost);
                        }
                        catch (Exception)
                        {
                            ShowError(Localization.GetString("ErrorCost", LocalResourceFile), lblCost);
                            return;
                        }

                        // Update the row
                        ShippingInfo shippingInfo = new ShippingInfo();
                        shippingInfo.ID = ID;
                        shippingInfo.Description = description;
                        shippingInfo.MinWeight = minWeight;
                        shippingInfo.MaxWeight = maxWeight;
                        shippingInfo.Cost = cost;
                        shippingInfo.ApplyTaxRate = applyTaxRate;
                        controller.UpdateShippingRate(shippingInfo);
                    }
                }
            }

            BindShippingRates();
            lblError.Visible = false;
            InvokeEditComplete();
		}

		#endregion

        #region Private Methods

        private void BindShippingRates()
        {
            ShippingController controller = new ShippingController();
            List<ShippingInfo> shippingRates = controller.GetShippingRates(ParentControl.PortalId);
            grdShippingRates.DataSource = shippingRates;
            grdShippingRates.DataBind();

            if (shippingRates.Count > 0)
                cbApplyTaxRate.Checked = shippingRates[0].ApplyTaxRate;
        }

        private void ShowError(string errorMessage, TextBox control)
        {
            lblError.Visible = true;
            lblError.Text = errorMessage;
            control.ForeColor = System.Drawing.Color.Red;
            control.BorderColor = System.Drawing.Color.Red;
            control.Focus();
        }

        private void ClearError(TextBox control)
        {
            control.ForeColor = System.Drawing.Color.Empty;
            control.BorderColor = System.Drawing.Color.Empty;
        }

        #endregion
    }
}
