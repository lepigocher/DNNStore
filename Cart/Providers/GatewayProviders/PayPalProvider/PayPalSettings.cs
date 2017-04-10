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

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for PayPalSettings.
	/// </summary>
    public sealed class PayPalSettings : GatewaySettings
	{

		#region Private Members

		private string _payPalID;
		private string _secureID;
		private string _currency = "USD";
        private string _buttonURL = "https://www.paypal.com/en_US/i/bnr/horizontal_solution_PP.gif";
        private decimal _surchargePercent;
        private decimal _surchargeFixed;
        private bool _useSandbox;
        private string _verificationURL = "https://www.paypal.com/cgi-bin/webscr/";
        private string _paymentURL = "https://www.paypal.com/cgi-bin/webscr/";
        private string _lc = "US";
        private string _charset = "UTF-8";

        #endregion

		#region Constructors

		public PayPalSettings()
		{
		}

		public PayPalSettings(string xml)
		{
			FromString(xml);
		}

		#endregion

		#region Public Properties

		public string PayPalID
		{
			get { return _payPalID; }
			set { _payPalID = value; }
		}

        public string SecureID
        {
            get { return _secureID; }
            set { _secureID = value; }
        }

		public string Currency
		{
			get { return _currency; }
			set { _currency = value; }
		}

		public string ButtonURL
		{
			get { return _buttonURL; }
			set { _buttonURL = value; }
		}

        public decimal SurchargePercent
        {
            get { return _surchargePercent; }
            set { _surchargePercent = value; }
        }

        public decimal SurchargeFixed
        {
            get { return _surchargeFixed; }
            set { _surchargeFixed = value; }
        }

        public bool UseSandbox
        {
            get { return _useSandbox; }
            set { _useSandbox = value; }
        }

        public string VerificationURL
        {
            get { return _verificationURL; }
            set { _verificationURL = value; }
        }

        public string PaymentURL
        {
            get { return _paymentURL; }
            set { _paymentURL = value; }
        }

        public string Lc
        {
            get { return _lc; }
            set { _lc = value; }
        }

        public string Charset
        {
            get { return _charset; }
            set { _charset = value; }
        }

        #endregion

		#region GatewaySettings Overrides

		public override bool IsValid()
		{
            return (!string.IsNullOrEmpty(_payPalID) && !string.IsNullOrEmpty(_currency) &&
                !string.IsNullOrEmpty(_buttonURL) && !string.IsNullOrEmpty(_verificationURL) &&
                !string.IsNullOrEmpty(_paymentURL) && !string.IsNullOrEmpty(_lc) && !string.IsNullOrEmpty(_charset));
		}

		#endregion
	}
}
