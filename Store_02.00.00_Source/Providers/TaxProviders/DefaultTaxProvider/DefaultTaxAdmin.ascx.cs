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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Store.Providers.Tax.DefaultTaxProvider
{
	/// <summary>
    /// Summary description for DefaultTaxAdmin.
	/// </summary>
	public partial class DefaultTaxAdmin : ProviderControlBase
	{
		#region Events

		override protected void OnInit(EventArgs e)
		{
			btnSaveTaxRates.Click += btnSaveTaxRates_Click;
			base.OnInit(e);
		}

        protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsReady)
                LoadTaxRates();
		}

		private void btnSaveTaxRates_Click(object sender, EventArgs e)
		{
			SaveTaxRates();
		}

		#endregion

		#region Private Methods

		private void SaveTaxRates()
		{
            decimal rate = Null.NullDecimal;

            if (cbEnableTax.Checked && txtTaxRate.Text.Length == 0)
            {
                lblError.Visible = true;
                lblError.Text = Localization.GetString("lblErrorTax", LocalResourceFile);
                txtTaxRate.BorderColor = System.Drawing.Color.Red;
                txtTaxRate.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (txtTaxRate.Text.Length > 0)
            {
                try
                {
                    rate = Decimal.Parse(txtTaxRate.Text);
                    lblError.Visible = false;
                    txtTaxRate.BorderColor = System.Drawing.Color.Empty;
                    txtTaxRate.ForeColor = System.Drawing.Color.Empty;
                    if (rate < 0)
                        throw new Exception();
                }
                catch (Exception)
                {
                    lblError.Visible = true;
                    lblError.Text = Localization.GetString("lblErrorTax", LocalResourceFile);
                    txtTaxRate.BorderColor = System.Drawing.Color.Red;
                    txtTaxRate.ForeColor = System.Drawing.Color.Red;
                    return;
                }
            }
            
            TaxController controller = new TaxController();
            controller.UpdateTaxRates(PortalId, rate, cbEnableTax.Checked);
		}

		private void LoadTaxRates()
		{
            TaxController controller = new TaxController();
            TaxInfo taxInfo = controller.GetTaxRates(PortalId);
            if (taxInfo != null)
            {
                txtTaxRate.Text = taxInfo.DefaultTaxRate < 0 ? "" : taxInfo.DefaultTaxRate.ToString("0.00");
                cbEnableTax.Checked = taxInfo.ShowTax;
            }
		    IsReady = true;
		}

		#endregion
	}
}
