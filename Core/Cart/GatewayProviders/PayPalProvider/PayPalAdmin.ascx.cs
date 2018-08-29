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

using DotNetNuke.Modules.Store.Core.Components;

namespace DotNetNuke.Modules.Store.Core.Cart
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
                PayPalSettings settings = new PayPalSettings
                {
                    UseSandbox = chkUseSandbox.Checked,
                    PayPalID = txtPayPalID.Text,
                    SecureID = txtSecureID.Text,
                    VerificationURL = txtPayPalVerificationURL.Text,
                    PaymentURL = txtPayPalPaymentURL.Text,
                    Lc = txtPayPalLanguage.Text,
                    Charset = txtPayPalCharset.Text,
                    ButtonURL = txtPayPalButtonURL.Text,
                    Currency = txtPayPalCurrency.Text,
                    SurchargePercent = Decimal.Parse(txtSurchargePercent.Text),
                    SurchargeFixed = Decimal.Parse(txtSurchargeFixed.Text)
                };

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
                        chkUseSandbox.Checked = settings.UseSandbox;
						txtPayPalID.Text = settings.PayPalID;
                        txtSecureID.Text = settings.SecureID;
                        txtPayPalVerificationURL.Text = settings.VerificationURL;
                        txtPayPalPaymentURL.Text = settings.PaymentURL;
                        txtPayPalLanguage.Text = settings.Lc;
                        txtPayPalCharset.Text = settings.Charset;
						txtPayPalButtonURL.Text = settings.ButtonURL;
						txtPayPalCurrency.Text = settings.Currency;
                        txtSurchargePercent.Text = settings.SurchargePercent < 0 ? "0" : settings.SurchargePercent.ToString("0.00");
                        txtSurchargeFixed.Text = settings.SurchargeFixed < 0 ? "0" : settings.SurchargeFixed.ToString("0.00");
					}
				}
			}
		}

		#endregion
	}
}
