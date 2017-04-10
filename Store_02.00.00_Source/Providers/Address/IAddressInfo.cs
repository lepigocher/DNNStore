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
using DotNetNuke.Services.Tokens;

namespace DotNetNuke.Modules.Store.Providers.Address
{
	/// <summary>
	/// Interface implemented by address providers to provide an address to the chekout control.
	/// </summary>
	public interface IAddressInfo : IPropertyAccess
	{
		int AddressID {get; set;}
		int PortalID {get; set;}
		int UserID {get; set;}
		string Description {get; set;}
		string FirstName {get; set;}
		string LastName {get; set;}
		string Address1 {get; set;}
		string Address2 {get; set;}
		string PostalCode {get; set;}
		string City {get; set;}
		string RegionCode {get; set;}
		string CountryCode {get; set;}
		string Email {get; set;}
		string Phone1 {get; set;}
		string Phone2 {get; set;}
		bool PrimaryAddress {get; set;}
		bool UserSaved {get; set;}
        bool Modified { get; set;}
		string CreatedByUser {get; set;}
		DateTime CreatedDate {get; set;}
	    string Format(string template);
	}
}
