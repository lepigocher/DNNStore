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
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;

using DotNetNuke.Modules.Store.Core.Components;

namespace DotNetNuke.Modules.Store.Core.Cart
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
                Form += formKey + "=" + requestForm[formKey] + "|";

            Array.Sort(keys, StringComparer.InvariantCultureIgnoreCase);

            foreach (string key in keys)
                if (key.StartsWith("vads_", StringComparison.InvariantCultureIgnoreCase))
                    Payload += requestForm[key] + "+";

            Certificate = certificate;
        }

        #endregion

        #region Private Members

        private decimal _vads_amount = -1;
        private decimal _vads_effective_amount = -1;

        #endregion

        #region Properties

        public string Form { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public string signature { get; set; } = string.Empty;
        public string Certificate { get; set; } = string.Empty;
        public string vads_version { get; set; } = string.Empty;
        public string vads_contract_used { get; set; } = string.Empty;
        public string vads_payment_src { get; set; } = string.Empty;
        public string vads_action_mode { get; set; } = string.Empty;
        public string vads_hash { get; set; } = string.Empty;
        public string vads_result { get; set; } = string.Empty;
        public string vads_extra_result { get; set; } = string.Empty;
        public string vads_payment_error { get; set; } = string.Empty;
        public string vads_validation_mode { get; set; } = string.Empty;
        public string vads_auth_mode { get; set; } = string.Empty;
        public string vads_auth_number { get; set; } = string.Empty;
        public string vads_site_id { get; set; } = string.Empty;
        public string vads_ctx_mode { get; set; } = string.Empty;
        public string vads_return_mode { get; set; } = string.Empty;
        public int vads_order_id { get; set; } = -1;
        public string vads_currency { get; set; } = string.Empty;
        public decimal vads_amount
		{
            get { return _vads_amount; }
            set { _vads_amount = (value / 100); }
		}
        public string vads_url_check_src { get; set; } = string.Empty;
        public string vads_operation_type { get; set; } = string.Empty;
        public string vads_trans_status { get; set; } = string.Empty;
        public string vads_trans_id { get; set; } = string.Empty;
        public string vads_trans_uuid { get; set; } = string.Empty;
        public string vads_payment_certificate { get; set; } = string.Empty;
        public string vads_payment_config { get; set; } = string.Empty;
        public int vads_sequence_number { get; set; }
        public string vads_trans_date { get; set; } = string.Empty;
        public string vads_presentation_date { get; set; } = string.Empty;
        public string vads_effective_creation_date { get; set; } = string.Empty;
        public int vads_capture_delay { get; set; } = -1;
        public decimal vads_change_rate { get; set; } = 1;
        public decimal vads_effective_amount
		{
            get { return _vads_effective_amount; }
            set { _vads_effective_amount = (value / 100); }
		}
        public string vads_page_action { get; set; } = string.Empty;
        public string vads_auth_result { get; set; } = string.Empty;
        public string vads_threeds_enrolled { get; set; } = string.Empty;
        public string vads_threeds_status { get; set; } = string.Empty;
        public string vads_risk_control { get; set; } = string.Empty;
        public string vads_payment_seq { get; set; } = string.Empty;
        public int vads_nb_products { get; set; } = -1;
        public decimal vads_shipping_amount { get; set; } = -1;
        public decimal vads_tax_amount { get; set; } = -1;
        public string vads_warranty_result { get; set; } = string.Empty;
        public bool IsValid
        {
            get
            {
                // If all those value fields are different than default value, and signatures are equals
                // then the IPN transaction is probably valid.
                // Other security checking are done inside SystempayPayment.ascx.cs
                if (!string.IsNullOrEmpty(Payload) && !string.IsNullOrEmpty(signature) && !string.IsNullOrEmpty(Certificate))
                {
                    // Compute signature
                    string signature = ComputeSHA1(Payload + Certificate, Encoding.UTF8);

                    // Compares signature, if equals the payload is valid
                    return string.Equals(signature, this.signature, StringComparison.InvariantCultureIgnoreCase);
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
