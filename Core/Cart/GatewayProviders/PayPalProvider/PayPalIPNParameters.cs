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

using System.Collections.Specialized;

using DotNetNuke.Modules.Store.Core.Components;

namespace DotNetNuke.Modules.Store.Core.Cart
{
	/// <summary>
	/// Summary description for PayPalIPNParameters.
	/// </summary>
    public sealed class PayPalIPNParameters : RequestFormWrapper
	{
		#region Constructors

		public PayPalIPNParameters()
		{
		}

        public PayPalIPNParameters(NameValueCollection requestForm, byte[] param) : base(requestForm)
        {
            string strRequest = "cmd=_notify-validate&";
            strRequest += System.Text.Encoding.ASCII.GetString(param);
            PostString = strRequest;
        }

        #endregion

        #region Properties

        public string PostString { get; set; } = string.Empty;
        public string txn_id { get; set; } = string.Empty;
        public string parent_txn_id { get; set; } = string.Empty;
        public string txn_type { get; set; } = string.Empty;
        public string verify_sign { get; set; } = string.Empty;
        public string payment_type { get; set; } = string.Empty;
        public string payment_status { get; set; } = string.Empty;
        public string pending_reason { get; set; } = string.Empty;
        public string case_type { get; set; } = string.Empty;
        public string reason_code { get; set; } = string.Empty;
        public string protection_eligibility { get; set; } = string.Empty;
        public string receiver_id { get; set; } = string.Empty;
        public string receiver_email { get; set; } = string.Empty;
        public string business { get; set; } = string.Empty;
        public string payer_id { get; set; } = string.Empty;
        public string payer_email { get; set; } = string.Empty;
        public string payer_status { get; set; } = string.Empty;
        public string address_status { get; set; } = string.Empty;
        public string mc_currency { get; set; } = string.Empty;
        public decimal exchange_rate { get; set; } = 1;
        public int invoice { get; set; } = -1;
        public string custom { get; set; } = string.Empty;
        public int num_cart_items { get; set; } = -1;
        public decimal mc_shipping { get; set; } = -1;
        public decimal mc_handling { get; set; } = -1;
        public decimal mc_fee { get; set; } = -1;
        public decimal tax { get; set; } = -1;
        public decimal mc_gross { get; set; } = -1;
        public decimal payment_fee { get; set; } = -1;
        public decimal payment_gross { get; set; } = -1;
        public bool resend { get; set; } = false;
        public bool IsValid
		{
			get
			{
                if (txn_id != string.Empty &&
                    verify_sign != string.Empty &&
                    payment_type != string.Empty &&
                    payment_status != string.Empty &&
                    receiver_id != string.Empty &&
                    receiver_email != string.Empty &&
                    business != string.Empty &&
                    payer_id != string.Empty &&
                    payer_email != string.Empty &&
                    payer_status != string.Empty &&
                    mc_currency != string.Empty &&
                    invoice != -1 &&
                    custom != string.Empty &&
                    mc_gross != -1)
                {
                    // If all those value fields are different than default value,
                    // then the IPN transaction is probably valid.
                    // Other security checking are done inside PayPalPayment.ascx.cs
                    return true;
                }
				return false;
			}
		}

		#endregion
	}
}
