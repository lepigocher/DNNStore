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
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework;
using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using System.Net.Mail;

namespace DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider
{
	/// <summary>
	///	StoreAddress is used by DefaultAddressProfile and DefaultAddressCheckout controls.
	/// </summary>
	public partial class StoreAddress : UserControlBase
	{
        #region Private Members

		private const string MyFileName = "StoreAddress.ascx";

		private int _startTabIndex = 1;
		private bool _enabled = true;
		private IAddressInfo _address;
	    private bool _showUserSaved;
	    private bool _showDescription;
	    private AddressSettings _settings;
	    private bool _hasChanged;

        #endregion

        #region Properties

        public string LocalResourceFile
        {
            get { return Localization.GetResourceFile(this, MyFileName); }
        }
		
		public bool Enabled
		{
			get { return _enabled; }
			set 
			{
				_enabled = value;
				foreach (WebControl control in GetInputControls())
                    control.Enabled = _enabled;
            }
		}
		
		public IAddressInfo Address
		{
			get 
			{
                if (_address != null && _hasChanged)
                {
                    // Used to secure fields content
                    PortalSecurity security = new PortalSecurity();
                    _address.FirstName = security.InputFilter(txtFirstName.Text, PortalSecurity.FilterFlag.NoMarkup);
                    _address.LastName = security.InputFilter(txtLastName.Text, PortalSecurity.FilterFlag.NoMarkup);
                    _address.Address1 = security.InputFilter(txtStreet.Text, PortalSecurity.FilterFlag.NoMarkup);
                    _address.Address2 = security.InputFilter(txtUnit.Text, PortalSecurity.FilterFlag.NoMarkup);
                    _address.PostalCode = security.InputFilter(txtPostal.Text, PortalSecurity.FilterFlag.NoMarkup);
                    _address.City = security.InputFilter(txtCity.Text, PortalSecurity.FilterFlag.NoMarkup);
                    _address.CountryCode = security.InputFilter(GetSelectedCountry(), PortalSecurity.FilterFlag.NoMarkup);
                    _address.RegionCode = security.InputFilter(GetSelectedRegion(), PortalSecurity.FilterFlag.NoMarkup);
                    _address.Email = security.InputFilter(txtEmail.Text, PortalSecurity.FilterFlag.NoMarkup);
                    _address.Phone1 = security.InputFilter(txtTelephone.Text, PortalSecurity.FilterFlag.NoMarkup);
                    _address.Phone2 = security.InputFilter(txtCell.Text, PortalSecurity.FilterFlag.NoMarkup);
                    if (_showUserSaved)
                    {
                        _address.AddressID = Null.NullInteger;
                        _address.UserSaved = chkUserSaved.Checked;
                    }
                    if (_showDescription)
                    {
                        _address.PrimaryAddress = chkPrimary.Checked;
                        _address.Description = security.InputFilter(txtDescription.Text, PortalSecurity.FilterFlag.NoMarkup);
                    }
                    _address.Modified = true;
                }
				return _address; 
			}
            set
            {
                _address = value;
                if (IsPostBack)
                    PopulateAddress();
            }
		}

		public int StartTabIndex 
		{
            set { _startTabIndex = value; } 
		} 

		public bool ShowUserSaved
		{
            set
            {
                _showUserSaved = value;
                rowUserSaved.Visible = _showUserSaved;
                if (_showUserSaved)
                {
                    chkUserSaved.Checked = false;
                    chkPrimary.Checked = false;
                    txtDescription.Text = "";
                }
            } 
		}

        public bool ShowDescription
        {
            set
            {
                _showDescription = value;
                rowPrimary.Visible = _showDescription;
                rowDescription.Visible = _showDescription;
            }
        }

	    public AddressSettings Settings
	    {
            set
            {
                _settings = value;
                ShowRequiredFields();
            }
	    }

        #endregion

        #region Event Handlers

        protected override void OnInit(EventArgs e)
		{
            Page.RegisterRequiresControlState(this);
			base.OnInit(e);
		}

        protected override void LoadControlState(object savedState)
        {
            if (savedState != null)
            {
                Pair p = savedState as Pair;
                if (p != null)
                {
                    base.LoadControlState(p.First);
                    AddressState state = p.Second as AddressState;
                    if (state != null)
                    {
                        _address = state.Address;
                        _showUserSaved = state.ShowUserSaved;
                        _showDescription = state.ShowDescription;
                        _hasChanged = state.HasChanged;
                        Settings = state.Settings;
                    }
                }
                else
                {
                    if (savedState is AddressState)
                    {
                        AddressState state = (AddressState) savedState;
                        _address = state.Address;
                        _showUserSaved = state.ShowUserSaved;
                        _showDescription = state.ShowDescription;
                        _hasChanged = state.HasChanged;
                        Settings = state.Settings;
                    }
                    else
                        base.LoadControlState(savedState);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                LoadCountryList();
                short tabIndex = (short)_startTabIndex;
                foreach (WebControl control in GetInputControls())
                    control.TabIndex = tabIndex++;
                PopulateAddress();
            }
		}

        protected void address_TextChanged(object sender, EventArgs e)
        {
            _hasChanged = true;
        }

        protected void cboCountry_SelectedIndexChanged(object sender, EventArgs e)
		{
            _hasChanged = true;
            Localize();
		}

        protected void cboRegion_SelectedIndexChanged(object sender, EventArgs e)
		{
            _hasChanged = true;
        }

        protected void chkUserSaved_CheckedChanged(object sender, EventArgs e)
        {
            bool userSaved = chkUserSaved.Checked;
            ShowDescription = userSaved;
            if (userSaved)
            {
                chkPrimary.Checked = false;
                txtDescription.Text = "";
            }
            _hasChanged = true;
        }

        protected void chkPrimary_CheckedChanged(object sender, EventArgs e)
        {
            _hasChanged = true;
        }

        protected void valCustEmail_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // Just in case of this validation was called before the 'required' validation control
            bool hasValue = !string.IsNullOrEmpty(args.Value);

            args.IsValid = hasValue;

            if (hasValue)
            {
                try
                {
                    MailAddress email = new MailAddress(args.Value);
                }
                catch
                {
                    args.IsValid = false;
                }
            }
        }

        protected override object SaveControlState()
        {
            object obj = base.SaveControlState();

            if (_address != null)
            {
                AddressState state = new AddressState {Address = (AddressInfo) _address, ShowUserSaved = _showUserSaved, ShowDescription = _showDescription, HasChanged = _hasChanged, Settings = _settings};

                if (obj != null)
                    return new Pair(obj, state);

                return (state);
            }

            return obj;
        }

        #endregion

        #region Private Methods

        private void PopulateAddress()
		{
            if (_address != null)
            {
                _hasChanged = false;
                chkUserSaved.Checked = _address.UserSaved;
                chkPrimary.Checked = _address.PrimaryAddress;
                txtDescription.Text = _address.Description;
                txtFirstName.Text = _address.FirstName;
                txtLastName.Text = _address.LastName;
                txtStreet.Text = _address.Address1;
                txtUnit.Text = _address.Address2;
                txtPostal.Text = _address.PostalCode;
                txtCity.Text = _address.City;
                txtEmail.Text = _address.Email;
                txtTelephone.Text = _address.Phone1;
                txtCell.Text = _address.Phone2;
                if (UserController.GetCurrentUserInfo().UserID != _address.UserID)
                {
                    chkUserSaved.Checked = false;
                    ShowDescription = false;
                    chkPrimary.Checked = false;
                    txtDescription.Text = "";
                }

                cboCountry.ClearSelection();
                if (string.IsNullOrEmpty(_address.CountryCode))
                    cboCountry.SelectedIndex = 0;
                else
                {
                    ListItem country = null;
                    if (_settings.CountryData.ToLower() == "text")
                        country = cboCountry.Items.FindByText(_address.CountryCode);
                    else if (_settings.CountryData.ToLower() == "value")
                        country = cboCountry.Items.FindByValue(_address.CountryCode);
                    if (country != null)
                        country.Selected = true;
                }

                Localize();

                if (cboRegion.Items.Count > 0)
                {
                    cboRegion.ClearSelection();
                    if (string.IsNullOrEmpty(_address.RegionCode))
                        cboRegion.SelectedIndex = 0;
                    else
                    {
                        ListItem region = null;
                        if (_settings.RegionData.ToLower() == "text")
                            region = cboRegion.Items.FindByText(_address.RegionCode);
                        else if (_settings.RegionData.ToLower() == "value")
                            region = cboRegion.Items.FindByValue(_address.RegionCode);
                        if (region != null)
                            region.Selected = true;
                    }
                }
                else
                    txtRegion.Text = _address.RegionCode;
            }
            else
            {
                cboCountry.ClearSelection();
                Localize();
            }
        }

        private List<WebControl> GetInputControls() 
		{
            string cacheKey = "StoreAddressInputControls_P" + PortalSettings.PortalId;
            List<WebControl> inputControls = (List<WebControl>)DataCache.GetCache(cacheKey);

            if (inputControls == null)
            {
                inputControls = new List<WebControl>(15);
                inputControls.Add(chkUserSaved);
                inputControls.Add(chkPrimary);
                inputControls.Add(txtDescription);
                inputControls.Add(txtFirstName);
                inputControls.Add(txtLastName);
                inputControls.Add(txtStreet);
                inputControls.Add(txtUnit);
                inputControls.Add(txtPostal);
                inputControls.Add(txtCity);
                inputControls.Add(cboCountry);
                inputControls.Add(cboRegion);
                inputControls.Add(txtRegion);
                inputControls.Add(txtEmail);
                inputControls.Add(txtTelephone);
                inputControls.Add(txtCell);
                DataCache.SetCache(cacheKey, inputControls);
            }

            return inputControls;
		}

		private string GetSelectedCountry() 
		{
            string retvalue = "";

            if (cboCountry.SelectedIndex > 0)
            {
                if (_settings.CountryData.ToLower() == "text")
                    retvalue = cboCountry.SelectedItem.Text;
                else if (_settings.CountryData.ToLower() == "value")
                    retvalue = cboCountry.SelectedItem.Value;
            }

            return retvalue; 
		}

		private string GetSelectedRegion()
		{ 
			string retvalue = "";

            if (cboRegion.SelectedIndex > 0)
            {
                if (_settings.RegionData.ToLower() == "text")
                    retvalue = cboRegion.SelectedItem.Text;
                else if (_settings.RegionData.ToLower() == "value")
                    retvalue = cboRegion.SelectedItem.Value;
            } 
			else 
				retvalue = txtRegion.Text; 

			return retvalue; 
		}

        private void Localize()
        {
            string countryCode = cboCountry.SelectedItem.Value;

            if (!string.IsNullOrEmpty(countryCode))
            {
                ListController ctlEntry = new ListController();
                string listKey = "Country." + countryCode;
                ListEntryInfoCollection entryCollection = ctlEntry.GetListEntryInfoCollection("Region", listKey);

                if (entryCollection.Count > 0)
                {
                    cboRegion.DataSource = entryCollection;
                    cboRegion.DataBind();
                    cboRegion.Items.Insert(0, new ListItem("< " + Localization.GetString("Not_Specified", Localization.SharedResourceFile) + " >", "-1"));

                    if (countryCode.ToLower() == "us")
                    {
                        plPostal.Text = Localization.GetString("Zip", LocalResourceFile);
                        plPostal.HelpText = Localization.GetString("Zip.Help", LocalResourceFile);
                        plRegion.Text = Localization.GetString("State", LocalResourceFile);
                        plRegion.HelpText = Localization.GetString("State.Help", LocalResourceFile);
                        valRegion1.ErrorMessage = Localization.GetString("StateRequired", Localization.GetResourceFile(this, MyFileName));
                    }
                    else
                    {
                        plPostal.Text = Localization.GetString("plPostal", LocalResourceFile);
                        plPostal.HelpText = Localization.GetString("plPostal.Help", LocalResourceFile);
                        plRegion.Text = Localization.GetString("Province", LocalResourceFile);
                        plRegion.HelpText = Localization.GetString("Province.Help", LocalResourceFile);
                        valRegion1.ErrorMessage = Localization.GetString("ProvinceRequired", LocalResourceFile);
                    }

                    rowRegion.Visible = true;
                    plRegion.ControlName = "cboRegion";
                    cboRegion.Visible = true;
                    valRegion1.Enabled = true;
                    txtRegion.Visible = false;
                    valRegion2.Enabled = false;
                    return;
                }
            }

            cboRegion.Items.Clear();
            cboRegion.Visible = false;
            valRegion1.Enabled = false;

            if (_settings.ShowRegion)
            {
                rowRegion.Visible = true;
                plPostal.Text = Localization.GetString("plPostal", LocalResourceFile);
                plPostal.HelpText = Localization.GetString("plPostal.Help", LocalResourceFile);
                plRegion.ControlName = "txtRegion";
                plRegion.Text = Localization.GetString("plRegion", LocalResourceFile);
                plRegion.HelpText = Localization.GetString("plRegion.Help", LocalResourceFile);
                txtRegion.Visible = true;
                valRegion2.Enabled = _settings.RequireRegion;
                valRegion2.ErrorMessage = Localization.GetString("RegionRequired", LocalResourceFile);
            }
            else
            {
                rowRegion.Visible = false;
                txtRegion.Visible = false;
                valRegion2.Enabled = false;
            }
        }

	    private void LoadCountryList() 
		{
			ListController ctlEntry = new ListController(); 
			ListEntryInfoCollection entryCollection = ctlEntry.GetListEntryInfoCollection("Country");
            if (entryCollection.Count > 0)
            {
                if (_settings.RestrictToCountries)
                {
                    string authorizedCountries = _settings.AuthorizedCountries;

                    if (!string.IsNullOrEmpty(authorizedCountries))
                    {
                        ListEntryInfoCollection restricted = new ListEntryInfoCollection();
                        foreach (string authorizedCountry in authorizedCountries.Split(','))
                        {
                            string key = "Country:" + authorizedCountry;
                            restricted.Add(key, entryCollection.Item(key));
                        }
                        cboCountry.DataSource = restricted;
                        cboCountry.DataBind();
                    }
                }
                else
                {
                    cboCountry.DataSource = entryCollection;
                    cboCountry.DataBind();
                }
                cboCountry.Items.Insert(0, new ListItem("< " + Localization.GetString("Not_Specified", Localization.SharedResourceFile) + " >", ""));
                cboCountry.SelectedIndex = 0;
            }
            else
                rowCountry.Visible = false;
        }

        private void ShowRequiredFields()
        {
            if (_settings != null)
            {
                // Reorder address rows
                const int baseRow = 3;
                HtmlTableRow currentRow = rowFirstName;
                tbAddress.Rows.Remove(rowFirstName);
                tbAddress.Rows.Insert(_settings.FirstNameRow + baseRow, currentRow);
                currentRow = rowLastName;
                tbAddress.Rows.Remove(rowLastName);
                tbAddress.Rows.Insert(_settings.LastNameRow + baseRow, currentRow);
                currentRow = rowStreet;
                tbAddress.Rows.Remove(rowStreet);
                tbAddress.Rows.Insert(_settings.StreetRow + baseRow, currentRow);
                currentRow = rowUnit;
                tbAddress.Rows.Remove(rowUnit);
                tbAddress.Rows.Insert(_settings.UnitRow + baseRow, currentRow);
                currentRow = rowPostal;
                tbAddress.Rows.Remove(rowPostal);
                tbAddress.Rows.Insert(_settings.PostalRow + baseRow, currentRow);
                currentRow = rowCity;
                tbAddress.Rows.Remove(rowCity);
                tbAddress.Rows.Insert(_settings.CityRow + baseRow, currentRow);
                currentRow = rowCountry;
                tbAddress.Rows.Remove(rowCountry);
                tbAddress.Rows.Insert(_settings.CountryRow + baseRow, currentRow);
                currentRow = rowRegion;
                tbAddress.Rows.Remove(rowRegion);
                tbAddress.Rows.Insert(_settings.RegionRow + baseRow, currentRow);
                currentRow = rowEmail;
                tbAddress.Rows.Remove(rowEmail);
                tbAddress.Rows.Insert(_settings.EmailRow + baseRow, currentRow);
                currentRow = rowTelephone;
                tbAddress.Rows.Remove(rowTelephone);
                tbAddress.Rows.Insert(_settings.TelephoneRow + baseRow, currentRow);
                currentRow = rowCell;
                tbAddress.Rows.Remove(rowCell);
                tbAddress.Rows.Insert(_settings.CellRow + baseRow, currentRow);
                if (!IsPostBack)
                {
                    // Show/Hide rows and optional validators
                    rowFirstName.Visible = _settings.ShowFirstName;
                    rowLastName.Visible = _settings.ShowLastName;
                    rowStreet.Visible = _settings.ShowStreet;
                    rowUnit.Visible = _settings.ShowUnit;
                    valUnit.Visible = _settings.RequireUnit;
                    rowPostal.Visible = _settings.ShowPostal;
                    rowCity.Visible = _settings.ShowCity;
                    rowCountry.Visible = _settings.ShowCountry;
                    rowRegion.Visible = _settings.ShowRegion;
                    valRegion2.Enabled = _settings.RequireRegion;
                    rowEmail.Visible = _settings.ShowEmail;
                    rowTelephone.Visible = _settings.ShowTelephone;
                    valTelephone.Visible = _settings.RequireTelephone;
                    rowCell.Visible = _settings.ShowCell;
                    valCell.Visible = _settings.RequireCell;
                }
            }
        }

        #endregion
    }
}
