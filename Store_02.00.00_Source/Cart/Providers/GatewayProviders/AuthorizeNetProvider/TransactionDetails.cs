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

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for TransactionDetails.
	/// </summary>
    public sealed class TransactionDetails : GatewaySettings
	{
		#region Constructors

		public TransactionDetails()
		{
		}

		public TransactionDetails(string xml)
		{
			FromString(xml);
		}

		#endregion

		#region Private Declarations

		private string _cardNumber = string.Empty;
		private int _expirationMonth = -1;
		private int _expirationYear = -1;
        private string _verificationCode = string.Empty;

		#endregion

		#region Public Properties

		public string CardNumber
		{
			get { return _cardNumber; }
			set { _cardNumber = value; }
		}

		public int ExpirationMonth
		{
			get { return _expirationMonth; }
			set { _expirationMonth = value; }
		}

		public int ExpirationYear
		{
			get { return _expirationYear; }
			set { _expirationYear = value; }
		}

		public string VerificationCode
		{
			get { return _verificationCode; }
			set { _verificationCode = value; }
		}

		#endregion

		#region GatewaySettings Overrides

		public override bool IsValid()
		{
			return (_cardNumber != string.Empty);
		}

		#endregion
	}
}
