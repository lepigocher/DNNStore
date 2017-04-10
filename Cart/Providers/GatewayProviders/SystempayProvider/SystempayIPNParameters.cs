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
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using DotNetNuke.Modules.Store.Components;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for SystempayIPNParameters.
	/// </summary>
    public sealed class SystempayIPNParameters : RequestFormWrapper
	{
		#region Constructors

		public SystempayIPNParameters()
		{
		}

        public SystempayIPNParameters(NameValueCollection requestForm, string certificate) : base(requestForm)
        {
            string[] keys = requestForm.AllKeys;

            foreach (string formKey in keys)
                _form += formKey + "=" + requestForm[formKey] + "|";

            Array.Sort(keys, StringComparer.InvariantCultureIgnoreCase);

            foreach (string key in keys)
                if (key.StartsWith("vads_", StringComparison.InvariantCultureIgnoreCase))
                    _payload += requestForm[key] + "+";

            _certificate = certificate;
        }

		#endregion

		#region Private Members

        private string _form = string.Empty;
        private string _payload = string.Empty;
        private string _signature = string.Empty;
        private string _certificate = string.Empty;

        private string _vads_version = string.Empty;
        private string _vads_contract_used = string.Empty;
        private string _vads_payment_src = string.Empty;
        private string _vads_action_mode = string.Empty;
        private string _vads_hash = string.Empty;
        private string _vads_result = string.Empty;
        private string _vads_extra_result = string.Empty;
        private string _vads_payment_error = string.Empty;
        private string _vads_validation_mode = string.Empty;
        private string _vads_auth_mode = string.Empty;
        private string _vads_auth_number = string.Empty;
        private string _vads_site_id = string.Empty;
        private string _vads_ctx_mode = string.Empty;
        private string _vads_return_mode = string.Empty;
        private int _vads_order_id = -1;
        private string _vads_currency = string.Empty;
        private decimal _vads_amount = -1;
        private string _vads_url_check_src = string.Empty;
        private string _vads_operation_type = string.Empty;
        private string _vads_trans_status = string.Empty;
        private string _vads_trans_id = string.Empty;
        private string _vads_trans_uuid = string.Empty;
        private string _vads_payment_certificate = string.Empty;
        private string _vads_payment_config = string.Empty;
        private int _vads_sequence_number;
        private string _vads_trans_date = string.Empty;
        private string _vads_presentation_date = string.Empty;
        private string _vads_effective_creation_date = string.Empty;
        private int _vads_capture_delay = -1;
        private decimal _vads_change_rate = 1;
        private decimal _vads_effective_amount = -1;
        private string _vads_page_action = string.Empty;
        private string _vads_auth_result = string.Empty;
        private string _vads_threeds_enrolled = string.Empty;
        private string _vads_threeds_status = string.Empty;
        private string _vads_risk_control = string.Empty;
        private string _vads_payment_seq = string.Empty;
        private int _vads_nb_products = -1;
        private decimal _vads_shipping_amount = -1;
        private decimal _vads_tax_amount = -1;
        private string _vads_warranty_result = string.Empty;

		#endregion

		#region Properties

        public string Form
        {
            get { return _form; }
            set { _form = value; }
        }

        public string Payload
        {
            get { return _payload; }
            set { _payload = value; }
        }

        public string signature
        {
            get { return _signature; }
            set { _signature = value; }
        }

        public string Certificate
		{
            get { return _certificate; }
            set { _certificate = value; }
		}

        public string vads_version
		{
            get { return _vads_version; }
            set { _vads_version = value; }
		}

        public string vads_contract_used
		{
            get { return _vads_contract_used; }
            set { _vads_contract_used = value; }
		}

        public string vads_payment_src
		{
            get { return _vads_payment_src; }
            set { _vads_payment_src = value; }
		}

        public string vads_action_mode
		{
            get { return _vads_action_mode; }
            set { _vads_action_mode = value; }
		}

        public string vads_hash
		{
            get { return _vads_hash; }
            set { _vads_hash = value; }
		}

        public string vads_result
		{
            get { return _vads_result; }
            set { _vads_result = value; }
		}

        public string vads_extra_result
		{
            get { return _vads_extra_result; }
            set { _vads_extra_result = value; }
		}

        public string vads_payment_error
		{
            get { return _vads_payment_error; }
            set { _vads_payment_error = value; }
		}

        public string vads_validation_mode
		{
            get { return _vads_validation_mode; }
            set { _vads_validation_mode = value; }
		}

        public string vads_auth_mode
		{
            get { return _vads_auth_mode; }
            set { _vads_auth_mode = value; }
		}

        public string vads_auth_number
		{
            get { return _vads_auth_number; }
            set { _vads_auth_number = value; }
		}

        public string vads_site_id
		{
            get { return _vads_site_id; }
            set { _vads_site_id = value; }
		}

        public string vads_ctx_mode
		{
            get { return _vads_ctx_mode; }
            set { _vads_ctx_mode = value; }
		}

        public string vads_return_mode
		{
            get { return _vads_return_mode; }
            set { _vads_return_mode = value; }
		}

        public int vads_order_id
		{
            get { return _vads_order_id; }
            set { _vads_order_id = value; }
		}

        public string vads_currency
		{
            get { return _vads_currency; }
            set { _vads_currency = value; }
		}

        public decimal vads_amount
		{
            get { return _vads_amount; }
            set { _vads_amount = (value / 100); }
		}

        public string vads_url_check_src
		{
            get { return _vads_url_check_src; }
            set { _vads_url_check_src = value; }
		}

        public string vads_operation_type
		{
            get { return _vads_operation_type; }
            set { _vads_operation_type = value; }
		}

        public string vads_trans_status
		{
            get { return _vads_trans_status; }
            set { _vads_trans_status = value; }
		}

        public string vads_trans_id
		{
            get { return _vads_trans_id; }
            set { _vads_trans_id = value; }
		}

        public string vads_trans_uuid
		{
            get { return _vads_trans_uuid; }
            set { _vads_trans_uuid = value; }
		}

        public string vads_payment_certificate
		{
            get { return _vads_payment_certificate; }
            set { _vads_payment_certificate = value; }
		}

        public string vads_payment_config
		{
            get { return _vads_payment_config; }
            set { _vads_payment_config = value; }
		}

        public int vads_sequence_number
		{
            get { return _vads_sequence_number; }
            set { _vads_sequence_number = value; }
		}

        public string vads_trans_date
		{
            get { return _vads_trans_date; }
            set { _vads_trans_date = value; }
		}

        public string vads_presentation_date
		{
            get { return _vads_presentation_date; }
            set { _vads_presentation_date = value; }
		}

        public string vads_effective_creation_date
		{
            get { return _vads_effective_creation_date; }
            set { _vads_effective_creation_date = value; }
		}

        public int vads_capture_delay
		{
            get { return _vads_capture_delay; }
            set { _vads_capture_delay = value; }
		}

        public decimal vads_change_rate
		{
            get { return _vads_change_rate; }
            set { _vads_change_rate = value; }
		}

        public decimal vads_effective_amount
		{
            get { return _vads_effective_amount; }
            set { _vads_effective_amount = (value / 100); }
		}

        public string vads_page_action
		{
			get { return _vads_page_action; }
			set { _vads_page_action = value; }
		}

        public string vads_auth_result
		{
            get { return _vads_auth_result; }
            set { _vads_auth_result = value; }
		}

        public string vads_threeds_enrolled
		{
            get { return _vads_threeds_enrolled; }
            set { _vads_threeds_enrolled = value; }
		}

        public string vads_threeds_status
		{
            get { return _vads_threeds_status; }
            set { _vads_threeds_status = value; }
		}

        public string vads_risk_control
		{
            get { return _vads_risk_control; }
            set { _vads_risk_control = value; }
		}

        public string vads_payment_seq
		{
            get { return _vads_payment_seq; }
            set { _vads_payment_seq = value; }
		}

        public int vads_nb_products
		{
            get { return _vads_nb_products; }
            set { _vads_nb_products = value; }
		}

        public decimal vads_shipping_amount
		{
            get { return _vads_shipping_amount; }
            set { _vads_shipping_amount = value; }
		}

        public decimal vads_tax_amount
		{
            get { return _vads_tax_amount; }
            set { _vads_tax_amount = value; }
		}

        public string vads_warranty_result
		{
            get { return _vads_warranty_result; }
            set { _vads_warranty_result = value; }
		}

        public bool IsValid
        {
            get
            {
                // If all those value fields are different than default value, and signatures are equals
                // then the IPN transaction is probably valid.
                // Other security checking are done inside SystempayPayment.ascx.cs
                if (!string.IsNullOrEmpty(_payload) && !string.IsNullOrEmpty(_signature) && !string.IsNullOrEmpty(_certificate))
                {
                    // Compute signature
                    string signature = ComputeSHA1(_payload + _certificate, Encoding.UTF8);

                    // Compares signature, if equals the payload is valid
                    return string.Equals(signature, _signature, StringComparison.InvariantCultureIgnoreCase);
                }

                return false;
            }
        }

		#endregion

        #region Private Methods

        private string ComputeSHA1(string text, Encoding encoding)
        {
            byte[] buffer = encoding.GetBytes(text);

            SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider();
            string hash = BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");

            return hash;
        }

        #endregion
    }
}
