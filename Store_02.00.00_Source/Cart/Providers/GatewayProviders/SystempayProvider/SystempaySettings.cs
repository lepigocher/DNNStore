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
	/// Summary description for SystempaySettings.
	/// </summary>
    public sealed class SystempaySettings : GatewaySettings
	{
		#region Private Members

		private string _siteID;
        private string _contracts;
        private bool _useTestCertificate;
		private string _certificate;
        private string _paymentURL = "https://paiement.systempay.fr/vads-payment/";
		private string _currency = "978";
        private string _language = "fr";
        private string _buttonURL;

        #endregion

		#region Constructors

		public SystempaySettings()
		{
		}

        public SystempaySettings(string xml)
		{
			FromString(xml);
		}

		#endregion

		#region Public Properties

        public string SiteID
		{
			get { return _siteID; }
			set { _siteID = value; }
		}

        public string Contracts
		{
            get { return _contracts; }
            set { _contracts = value; }
		}

        public bool UseTestCertificate
        {
            get { return _useTestCertificate; }
            set { _useTestCertificate = value; }
        }

        public string Certificate
        {
            get { return _certificate; }
            set { _certificate = value; }
        }

        public string PaymentURL
        {
            get { return _paymentURL; }
            set { _paymentURL = value; }
        }

		public string Currency
		{
			get { return _currency; }
			set { _currency = value; }
		}

        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }

        public string ButtonURL
        {
            get { return _buttonURL; }
            set { _buttonURL = value; }
        }

        #endregion

		#region GatewaySettings Overrides

		public override bool IsValid()
		{
            return (!string.IsNullOrEmpty(_siteID) && !string.IsNullOrEmpty(_certificate) &&
                !string.IsNullOrEmpty(_paymentURL) && !string.IsNullOrEmpty(_currency) &&
                !string.IsNullOrEmpty(_language));
		}

		#endregion
	}
}
