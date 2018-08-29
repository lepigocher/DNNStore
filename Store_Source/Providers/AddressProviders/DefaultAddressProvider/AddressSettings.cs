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
using System.Xml.Serialization;

namespace DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider
{
    [Serializable, XmlType("DefaultAddressProvider")]
    public class AddressSettings
    {
        #region Private Members

        private bool _allowPickup;
        private bool _showFirstName = true;
        private int _firstNameRow;
        private bool _showLastName = true;
        private int _lastNameRow = 1;
        private bool _showStreet = true;
        private int _streetRow = 2;
        private bool _showUnit = true;
        private bool _requireUnit;
        private int _unitRow = 3;
        private bool _showPostal = true;
        private int _postalRow = 4;
        private bool _showCity = true;
        private int _cityRow = 5;
        private bool _showCountry = true;
        private int _countryRow = 6;
        private bool _restrictToCountries;
        private string _authorizedCountries;
        private bool _showRegion = true;
        private int _regionRow = 7;
        private bool _requireRegion = false;
        private bool _showEmail = true;
        private int _emailRow = 8;
        private bool _showTelephone = true;
        private bool _requireTelephone = true;
        private int _telephoneRow = 9;
        private bool _showCell = true;
        private bool _requireCell = true;
        private int _cellRow = 10;
        private string _countryData = "Text";
        private string _regionData = "Text";
        
        #endregion

        #region Properties

        [XmlAttribute]
        public bool AllowPickup
        {
            get { return _allowPickup; }
            set { _allowPickup = value; }
        }

        [XmlAttribute]
        public bool ShowFirstName
        {
            get { return _showFirstName; }
            set { _showFirstName = value; }
        }

        [XmlAttribute]
        public int FirstNameRow
        {
            get { return _firstNameRow; }
            set { _firstNameRow = value; }
        }

        [XmlAttribute]
        public bool ShowLastName
        {
            get { return _showLastName; }
            set { _showLastName = value; }
        }

        [XmlAttribute]
        public int LastNameRow
        {
            get { return _lastNameRow; }
            set { _lastNameRow = value; }
        }

        [XmlAttribute]
        public bool ShowStreet
        {
            get { return _showStreet; }
            set { _showStreet = value; }
        }

        [XmlAttribute]
        public int StreetRow
        {
            get { return _streetRow; }
            set { _streetRow = value; }
        }

        [XmlAttribute]
        public bool ShowUnit
        {
            get { return _showUnit; }
            set { _showUnit = value; }
        }

        [XmlAttribute]
        public int UnitRow
        {
            get { return _unitRow; }
            set { _unitRow = value; }
        }

        [XmlAttribute]
        public bool RequireUnit
        {
            get { return _requireUnit; }
            set { _requireUnit = value; }
        }

        [XmlAttribute]
        public bool ShowPostal
        {
            get { return _showPostal; }
            set { _showPostal = value; }
        }

        [XmlAttribute]
        public int PostalRow
        {
            get { return _postalRow; }
            set { _postalRow = value; }
        }

        [XmlAttribute]
        public bool ShowCity
        {
            get { return _showCity; }
            set { _showCity = value; }
        }

        [XmlAttribute]
        public int CityRow
        {
            get { return _cityRow; }
            set { _cityRow = value; }
        }

        [XmlAttribute]
        public bool ShowCountry
        {
            get { return _showCountry; }
            set { _showCountry = value; }
        }

        [XmlAttribute]
        public int CountryRow
        {
            get { return _countryRow; }
            set { _countryRow = value; }
        }

        [XmlAttribute]
        public bool RestrictToCountries
        {
            get { return _restrictToCountries; }
            set { _restrictToCountries = value; }
        }

        [XmlAttribute]
        public string AuthorizedCountries
        {
            get { return _authorizedCountries; }
            set { _authorizedCountries = value; }
        }

        [XmlAttribute]
        public bool ShowRegion
        {
            get { return _showRegion; }
            set { _showRegion = value; }
        }

        [XmlAttribute]
        public int RegionRow
        {
            get { return _regionRow; }
            set { _regionRow = value; }
        }

        [XmlAttribute]
        public bool RequireRegion
        {
            get { return _requireRegion; }
            set { _requireRegion = value; }
        }

        [XmlAttribute]
        public bool ShowEmail
        {
            get { return _showEmail; }
            set { _showEmail = value; }
        }

        [XmlAttribute]
        public int EmailRow
        {
            get { return _emailRow; }
            set { _emailRow = value; }
        }

        [XmlAttribute]
        public bool ShowTelephone
        {
            get { return _showTelephone; }
            set { _showTelephone = value; }
        }

        [XmlAttribute]
        public bool RequireTelephone
        {
            get { return _requireTelephone; }
            set { _requireTelephone = value; }
        }

        [XmlAttribute]
        public int TelephoneRow
        {
            get { return _telephoneRow; }
            set { _telephoneRow = value; }
        }

        [XmlAttribute]
        public bool ShowCell
        {
            get { return _showCell; }
            set { _showCell = value; }
        }

        [XmlAttribute]
        public bool RequireCell
        {
            get { return _requireCell; }
            set { _requireCell = value; }
        }

        [XmlAttribute]
        public int CellRow
        {
            get { return _cellRow; }
            set { _cellRow = value; }
        }

        [XmlAttribute]
        public string CountryData
        {
            get { return _countryData; }
            set { _countryData = value; }
        }

        [XmlAttribute]
        public string RegionData
        {
            get { return _regionData; }
            set { _regionData = value; }
        }
        
        #endregion
    }
}