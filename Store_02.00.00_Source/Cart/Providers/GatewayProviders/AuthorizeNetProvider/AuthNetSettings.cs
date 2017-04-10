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
	/// Summary description for AuthNetSettings.
	/// </summary>
    public sealed class AuthNetSettings : GatewaySettings
	{
		#region Constructors

		public AuthNetSettings()
		{
		}

		public AuthNetSettings(string xml)
		{
			FromString(xml);
		}

		#endregion

		#region Private Members

        // For Developer Account Use: https://test.authorize.net/gateway/transact.dll
		private string _gatewayURL = "https://secure.authorize.net/gateway/transact.dll";
		private string _version = "3.1";
		private string _username = string.Empty;
		private string _password = string.Empty;
		private CaptureTypes _capture;
		private bool _isTest;

		#endregion

		#region Properties

		public enum CaptureTypes
		{
            AUTH_CAPTURE,
            AUTH_ONLY
		}

		public string GatewayURL
		{
            get { return _gatewayURL; }
			set { _gatewayURL = value; }
		}

		public string Version
		{
			get { return _version; }
			set { _version = value; }
		}

		public string Username
		{
			get { return _username; }
			set { _username = value; }
		}

		public string Password
		{
			get { return _password; }
			set { _password = value; }
		}

		public CaptureTypes Capture
		{
			get { return _capture; }
			set { _capture = value; }
		}

		public bool IsTest
		{
			get { return _isTest; }
			set { _isTest = value; }
		}

		#endregion

		#region GatewaySettings Overrides

        public override object GetCustomType(string typeName, string stringValue)
		{
            if (typeName == "CaptureTypes")
                return Enum.Parse(typeof(CaptureTypes), stringValue);
                
            return null;
		}

		public override bool IsValid()
		{
			return (_gatewayURL != string.Empty);
		}

		#endregion
	}
}
