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
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for PayPalAdmin.
	/// </summary>
	public partial class PayPalAdmin : StoreControlBase
	{
		#region StoreControlBase Overrides

		public override object DataSource
		{
			get
			{
				PayPalSettings settings = new PayPalSettings();
				settings.PayPalID = txtPayPalID.Text;
                settings.SecureID = txtSecureID.Text;
                settings.VerificationURL = txtPayPalVerificationURL.Text;
                settings.PaymentURL = txtPayPalPaymentURL.Text;
                settings.Lc = txtPayPalLanguage.Text;
                settings.Charset = txtPayPalCharset.Text;
				settings.ButtonURL = txtPayPalButtonURL.Text;
                settings.Currency = txtPayPalCurrency.Text;
                settings.UseSandbox = chkUseSandbox.Checked;
                try
                {
                    settings.SurchargeFixed = Decimal.Parse(txtSurchargeFixed.Text);
                    lblError.Visible = false;                    
                    txtSurchargeFixed.ForeColor = System.Drawing.Color.Empty;
                    txtSurchargeFixed.BorderColor = System.Drawing.Color.Empty;
                }
                catch (Exception)
                {
                    lblError.Visible = true;
                    lblError.Text = Localization.GetString("ErrorFixedSurcharge", LocalResourceFile);
                    txtSurchargeFixed.ForeColor = System.Drawing.Color.Red;
                    txtSurchargeFixed.BorderColor = System.Drawing.Color.Red;
                    txtSurchargeFixed.Text = "0.00";
                }

                try
                {
                    settings.SurchargePercent = Decimal.Parse(txtSurchargePercent.Text);
                    lblError.Visible = false;
                    txtSurchargePercent.ForeColor = System.Drawing.Color.Empty;
                    txtSurchargePercent.BorderColor = System.Drawing.Color.Empty;                    
                }
                catch (Exception)
                {
                    lblError.Visible = true;
                    lblError.Text = Localization.GetString("ErrorPercentageSurcharge", LocalResourceFile);
                    txtSurchargePercent.ForeColor = System.Drawing.Color.Red;
                    txtSurchargePercent.BorderColor = System.Drawing.Color.Red;
                    txtSurchargePercent.Text = "0.00";
                }

                base.DataSource = settings.ToString();
                DataSource = settings.ToString();
				return base.DataSource;
			}
			set
			{
				base.DataSource = value;

				if (base.DataSource != null)
				{
					string gatewaySettings = base.DataSource as string;
					if (gatewaySettings != null)
					{
						PayPalSettings settings = new PayPalSettings(gatewaySettings);
						txtPayPalID.Text = settings.PayPalID;
                        txtSecureID.Text = settings.SecureID;
                        txtPayPalVerificationURL.Text = settings.VerificationURL;
                        txtPayPalPaymentURL.Text = settings.PaymentURL;
                        txtPayPalLanguage.Text = settings.Lc;
                        txtPayPalCharset.Text = settings.Charset;
						txtPayPalButtonURL.Text = settings.ButtonURL;
						txtPayPalCurrency.Text = settings.Currency;
                        txtSurchargePercent.Text = settings.SurchargePercent < 0 ? "" : settings.SurchargePercent.ToString("0.00");
                        txtSurchargeFixed.Text = settings.SurchargeFixed < 0 ? "" : settings.SurchargeFixed.ToString("0.00");
                        chkUseSandbox.Checked = settings.UseSandbox;
					}
				}
			}
		}

		#endregion
	}
}
