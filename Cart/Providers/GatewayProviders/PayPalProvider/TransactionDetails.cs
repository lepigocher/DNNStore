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
	/// Summary description for TransactionDetails.
	/// </summary>
    public sealed class TransactionDetails : GatewaySettings
	{
		#region Private Members

		private string _returnURL;
		private string _cancelURL;
		private string _notifyURL;
        private string _cbt;
        private string _email;

		#endregion

		#region Constructors

		public TransactionDetails()
		{
		}

		public TransactionDetails(string xml)
		{
			FromString(xml);
		}

		#endregion

		#region Properties

		public string ReturnURL
		{
			get { return _returnURL; }
			set { _returnURL = value; }
		}

		public string CancelURL
		{
			get { return _cancelURL; }
			set { _cancelURL = value; }
		}

		public string NotifyURL
		{
			get { return _notifyURL; }
			set { _notifyURL = value; }
		}

		public string Cbt
		{
            get { return _cbt; }
            set { _cbt = value; }
		}

		public string Email
		{
            get { return _email; }
            set { _email = value; }
		}

		#endregion

		#region GatewaySettings Overrides

		public override bool IsValid()
		{
            return !string.IsNullOrEmpty(_returnURL) && !string.IsNullOrEmpty(_cancelURL) &&
                !string.IsNullOrEmpty(_notifyURL) && !string.IsNullOrEmpty(_cbt);
		}

		#endregion
	}
}
