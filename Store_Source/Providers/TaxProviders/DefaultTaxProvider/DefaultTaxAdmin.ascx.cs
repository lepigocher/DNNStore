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
using System.Web.UI.WebControls;

using DotNetNuke.Common.Utilities;

using DotNetNuke.Modules.Store.Core.Providers;

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

        protected void valCustTaxRate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool valid = true;

            if (cbEnableTax.Checked)
            {
                decimal rate = Null.NullDecimal;

                valid = Decimal.TryParse(args.Value, out rate);

                if (valid)
                    valid = rate > 0;
            }

            args.IsValid = valid;
        }

        #endregion

        #region Private Methods

        private void SaveTaxRates()
        {
            if (!Page.IsValid)
                return;

            bool enableTax = cbEnableTax.Checked;
            decimal rate = Null.NullDecimal;

            if (enableTax)
                rate = Decimal.Parse(txtTaxRate.Text);

            TaxController controller = new TaxController();
            controller.UpdateTaxRates(PortalId, rate, enableTax);
        }

		private void LoadTaxRates()
		{
            TaxController controller = new TaxController();
            TaxInfo taxInfo = controller.GetTaxRates(PortalId);

            if (taxInfo != null)
            {
                cbEnableTax.Checked = taxInfo.ShowTax;
                txtTaxRate.Text = taxInfo.DefaultTaxRate < 0 ? "" : taxInfo.DefaultTaxRate.ToString("0.00");
            }

		    IsReady = true;
		}

        #endregion
    }
}
