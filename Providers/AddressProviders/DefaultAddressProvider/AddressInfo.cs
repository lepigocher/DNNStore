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
using System.Globalization;
using System.Text.RegularExpressions;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Tokens;

namespace DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider
{
	/// <summary>
	/// Summary description for AddressInfo.
	/// </summary>
	[Serializable]
    public sealed class AddressInfo : IAddressInfo, IHydratable, IEquatable<AddressInfo>
	{
		#region Private Members

		private int _addressID = -1;
		private int _portalID;
		private int _userID;
		private string _description;
		private string _firstName;
		private string _lastName;
		private string _address1;
		private string _address2;
		private string _postalCode;
		private string _city;
		private string _regionCode;
		private string _countryCode;
        private string _email;
		private string _phone1;
		private string _phone2;
		private bool _primaryAddress;
        private bool _userSaved;
        private bool _modified;
		private string _createdByUser;
		private DateTime _createdDate;

        private static readonly Regex RegFullName = new Regex(@"\[fullname\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex RegFirstName = new Regex(@"\[firstname\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex RegLastName = new Regex(@"\[lastname\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex RegAddress1 = new Regex(@"\[address1\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex RegAddress2 = new Regex(@"\[address2\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex RegPostalCode = new Regex(@"\[postalcode\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex RegCity = new Regex(@"\[city\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex RegRegion = new Regex(@"\[region\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex RegCountry = new Regex(@"\[country\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex RegEmail = new Regex(@"\[email\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex RegPhone1 = new Regex(@"\[phone1\]|\[telephone\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex RegPhone2 = new Regex(@"\[phone2\]|\[cell\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex RegEmptyLines = new Regex(@"^\s*(?:<br />)*$[\r\n]*", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

		#endregion

        #region Properties

        public int AddressID
        {
            get { return _addressID; }
            set { _addressID = value; }
        }

        public int PortalID
        {
            get { return _portalID; }
            set { _portalID = value; }
        }

        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string Address1
        {
            get { return _address1; }
            set { _address1 = value; }
        }

        public string Address2
        {
            get { return _address2; }
            set { _address2 = value; }
        }

        public string PostalCode
        {
            get { return _postalCode; }
            set { _postalCode = value; }
        }

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        public string RegionCode
        {
            get { return _regionCode; }
            set { _regionCode = value; }
        }

        public string CountryCode
        {
            get { return _countryCode; }
            set { _countryCode = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Phone1
        {
            get { return _phone1; }
            set { _phone1 = value; }
        }

        public string Phone2
        {
            get { return _phone2; }
            set { _phone2 = value; }
        }

        public bool PrimaryAddress
        {
            get { return _primaryAddress; }
            set { _primaryAddress = value; }
        }

        public bool UserSaved
        {
            get { return _userSaved; }
            set { _userSaved = value; }
        }

        public bool Modified
        {
            get { return _modified; }
            set { _modified = value; }
        }

        public string CreatedByUser
        {
            get { return _createdByUser; }
            set { _createdByUser = value; }
        }

        public DateTime CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        #endregion

        #region Public Methods

        public string Format(string template)
        {
            string fullName = (_firstName + " " + _lastName).Trim();
            if (!string.IsNullOrEmpty(fullName))
                template = RegFullName.Replace(template, fullName);
            else
                template = RegFullName.Replace(template, string.Empty);
            if (!string.IsNullOrEmpty(_firstName))
                template = RegFirstName.Replace(template, _firstName);
            else
                template = RegFirstName.Replace(template, string.Empty);
            if (!string.IsNullOrEmpty(_lastName))
                template = RegLastName.Replace(template, _lastName);
            else
                template = RegLastName.Replace(template, string.Empty);
            if (!string.IsNullOrEmpty(_address1))
                template = RegAddress1.Replace(template, _address1);
            else
                template = RegAddress1.Replace(template, string.Empty);
            if (!string.IsNullOrEmpty(_address2))
                template = RegAddress2.Replace(template, _address2);
            else
                template = RegAddress2.Replace(template, string.Empty);
            if (!string.IsNullOrEmpty(_postalCode))
                template = RegPostalCode.Replace(template, _postalCode);
            else
                template = RegPostalCode.Replace(template, string.Empty);
            if (!string.IsNullOrEmpty(_city))
                template = RegCity.Replace(template, _city);
            else
                template = RegCity.Replace(template, string.Empty);
            if (!string.IsNullOrEmpty(_regionCode))
                template = RegRegion.Replace(template, _regionCode);
            else
                template = RegRegion.Replace(template, string.Empty);
            if (!string.IsNullOrEmpty(_countryCode))
                template = RegCountry.Replace(template, _countryCode);
            else
                template = RegCountry.Replace(template, string.Empty);
            if (!string.IsNullOrEmpty(_email))
                template = RegEmail.Replace(template, _email);
            else
                template = RegEmail.Replace(template, string.Empty);
            if (!string.IsNullOrEmpty(_phone1))
                template = RegPhone1.Replace(template, _phone1);
            else
                template = RegPhone1.Replace(template, string.Empty);
            if (!string.IsNullOrEmpty(_phone2))
                template = RegPhone2.Replace(template, _phone2);
            else
                template = RegPhone2.Replace(template, string.Empty);
            template = RegEmptyLines.Replace(template, string.Empty);
            return template;
        }

        #endregion

        #region Object Overrides

        public override int GetHashCode()
        {
            return _addressID.GetHashCode();
        }

        #endregion

        #region IPropertyAccess Members

        CacheLevel IPropertyAccess.Cacheability
        {
            get { return CacheLevel.fullyCacheable; }
        }

        string IPropertyAccess.GetProperty(string strPropertyName, string strFormat, CultureInfo formatProvider, UserInfo accessingUser, Scope accessLevel, ref bool propertyNotFound)
        {
            string propertyValue = null;
            switch (strPropertyName.ToLower())
            {
                case "firstname":
                    propertyValue = _firstName;
                    break;
                case "lastname":
                    propertyValue = _lastName;
                    break;
                case "fullname":
                    propertyValue = (_firstName + " " + _lastName).Trim();
                    break;
                case "address1":
                    propertyValue = _address1;
                    break;
                case "address2":
                    propertyValue = _address2;
                    break;
                case "postalcode":
                    propertyValue = _postalCode;
                    break;
                case "city":
                    propertyValue = _city;
                    break;
                case "region":
                    propertyValue = _regionCode;
                    break;
                case "country":
                    propertyValue = _countryCode;
                    break;
                case "email":
                    propertyValue = _email;
                    break;
                case "phone1":
                case "telephone":
                    propertyValue = _phone1;
                    break;
                case "phone2":
                case "cell":
                    propertyValue = _phone2;
                    break;
                default:
                    propertyNotFound = true;
                    break;
            }
            return propertyValue;
        }

        #endregion

        #region IHydratable Members

        public void Fill(System.Data.IDataReader dr)
        {
            _addressID = Convert.ToInt32(dr["AddressID"]);
            _portalID = Convert.ToInt32(dr["PortalID"]);
            _userID = Convert.ToInt32(dr["UserID"]);
            _description = Convert.ToString(Null.SetNull(dr["Description"], _description));
            _firstName = Convert.ToString(Null.SetNull(dr["FirstName"], _firstName));
            _lastName = Convert.ToString(dr["LastName"]);
            _address1 = Convert.ToString(Null.SetNull(dr["Address1"], _address1));
            _address2 = Convert.ToString(Null.SetNull(dr["Address2"], _address2));
            _city = Convert.ToString(Null.SetNull(dr["City"], _city));
            _regionCode = Convert.ToString(Null.SetNull(dr["RegionCode"], _regionCode));
            _countryCode = Convert.ToString(Null.SetNull(dr["CountryCode"], _countryCode));
            _postalCode = Convert.ToString(Null.SetNull(dr["PostalCode"], _postalCode));
            _email = Convert.ToString(Null.SetNull(dr["Email"], _email));
            _phone1 = Convert.ToString(Null.SetNull(dr["Phone1"], _phone1));
            _phone2 = Convert.ToString(Null.SetNull(dr["Phone2"], _phone2));
            _primaryAddress = Convert.ToBoolean(dr["PrimaryAddress"]);
            _userSaved = Convert.ToBoolean(dr["UserSaved"]);
            _createdByUser = Convert.ToString(Null.SetNull(dr["CreatedByUser"], _createdByUser));
            _createdDate = Convert.ToDateTime(Null.SetNull(dr["CreatedDate"], _createdDate));
        }

        public int KeyID
        {
            get { return _addressID; }
            set { _addressID = value; }
        }

        #endregion

        #region IEquatable<AddressInfo> Interface

        public bool Equals(AddressInfo other)
        {
            if (other == null)
                return false;
            return _addressID.Equals(other.AddressID);
        }

        #endregion
    }
}
