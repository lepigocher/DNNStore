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

using System.Collections.Specialized;
using DotNetNuke.Modules.Store.Components;

namespace DotNetNuke.Modules.Store.Cart
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
            _postString = strRequest;
        }

		#endregion

		#region Private Members

		private string _postString = string.Empty;
		private string _txn_id = string.Empty;
        private string _parent_txn_id = string.Empty;
        private string _txn_type = string.Empty;
        private string _verify_sign = string.Empty;
        private string _payment_type = string.Empty;
		private string _payment_status = string.Empty;
        private string _pending_reason = string.Empty;
        private string _case_type = string.Empty;
        private string _reason_code = string.Empty;
        private string _protection_eligibility = string.Empty;
        private string _receiver_id = string.Empty;
		private string _receiver_email = string.Empty;
        private string _business = string.Empty;
        private string _payer_id = string.Empty;
        private string _payer_email = string.Empty;
        private string _payer_status = string.Empty;
        private string _address_status = string.Empty;
        private string _mc_currency = string.Empty;
        private decimal _exchange_rate = 1;
        private int _invoice = -1;
        private string _custom = string.Empty;
        private int _num_cart_items = -1;
        private decimal _mc_shipping = -1;
        private decimal _mc_handling = -1;
        private decimal _mc_fee = -1;
		private decimal _tax = -1;
		private decimal _mc_gross = -1;
        private decimal _payment_fee = -1;
        private decimal _payment_gross = -1;
        private bool _resend = false;

		#endregion

		#region Properties

		public string PostString
		{
			get { return _postString; }
			set { _postString = value; }
		}

		public string txn_id
		{
			get { return _txn_id; }
			set { _txn_id = value; }
		}

        public string parent_txn_id
		{
            get { return _parent_txn_id; }
            set { _parent_txn_id = value; }
		}

        public string txn_type
		{
            get { return _txn_type; }
            set { _txn_type = value; }
		}

        public string verify_sign
		{
            get { return _verify_sign; }
            set { _verify_sign = value; }
		}

        public string payment_type
		{
            get { return _payment_type; }
            set { _payment_type = value; }
		}

		public string payment_status
		{
			get { return _payment_status; }
			set { _payment_status = value; }
		}

        public string pending_reason
		{
            get { return _pending_reason; }
            set { _pending_reason = value; }
		}

        public string case_type
		{
            get { return _case_type; }
            set { _case_type = value; }
		}

        public string reason_code
		{
            get { return _reason_code; }
            set { _reason_code = value; }
		}

        public string protection_eligibility
		{
            get { return _protection_eligibility; }
            set { _protection_eligibility = value; }
		}

        public string receiver_id
		{
            get { return _receiver_id; }
            set { _receiver_id = value; }
		}

		public string receiver_email
		{
			get { return _receiver_email; }
			set { _receiver_email = value; }
		}

        public string business
		{
            get { return _business; }
            set { _business = value; }
		}

        public string payer_id
		{
            get { return _payer_id; }
            set { _payer_id = value; }
		}

        public string payer_email
		{
            get { return _payer_email; }
            set { _payer_email = value; }
		}

        public string payer_status
		{
            get { return _payer_status; }
            set { _payer_status = value; }
		}

        public string address_status
		{
            get { return _address_status; }
            set { _address_status = value; }
		}

        public string mc_currency
		{
            get { return _mc_currency; }
            set { _mc_currency = value; }
		}

        public decimal exchange_rate
		{
            get { return _exchange_rate; }
            set { _exchange_rate = value; }
		}

        public int invoice
		{
            get { return _invoice; }
            set { _invoice = value; }
		}

		public string custom
		{
			get { return _custom; }
			set { _custom = value; }
		}

        public int num_cart_items
		{
            get { return _num_cart_items; }
            set { _num_cart_items = value; }
		}

        public decimal mc_shipping
		{
            get { return _mc_shipping; }
            set { _mc_shipping = value; }
		}

        public decimal mc_handling
		{
            get { return _mc_handling; }
            set { _mc_handling = value; }
		}

        public decimal mc_fee
		{
            get { return _mc_fee; }
            set { _mc_fee = value; }
		}

		public decimal tax
		{
			get { return _tax; }
			set { _tax = value; }
		}

		public decimal mc_gross
		{
			get { return _mc_gross; }
			set { _mc_gross = value; }
		}

        public decimal payment_fee
		{
            get { return _payment_fee; }
            set { _payment_fee = value; }
		}

        public decimal payment_gross
		{
            get { return _payment_gross; }
            set { _payment_gross = value; }
		}

        public bool resend
		{
            get { return _resend; }
            set { _resend = value; }
		}

		public bool IsValid
		{
			get
			{
                if (_txn_id != string.Empty &&
                    _verify_sign != string.Empty &&
                    _payment_type != string.Empty &&
                    _payment_status != string.Empty &&
                    _receiver_id != string.Empty &&
                    _receiver_email != string.Empty &&
                    _business != string.Empty &&
                    _payer_id != string.Empty &&
                    _payer_email != string.Empty &&
                    _payer_status != string.Empty &&
                    _mc_currency != string.Empty &&
                    _invoice != -1 &&
                    _custom != string.Empty &&
                    _mc_gross != -1)
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
