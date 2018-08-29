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
using System.Collections.Generic;
using System.Web.UI.WebControls;

using DotNetNuke.Common.Lists;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;

using DotNetNuke.Modules.Store.Core.Providers;

namespace DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider
{
	/// <summary>
    /// Summary description for DefaultAddressAdmin.
	/// </summary>
	public partial class DefaultAddressAdmin : ProviderControlBase
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            valReqAuthorizedCountries.ErrorMessage = Localization.GetString("valReqAuthorizedCountries.Error", LocalResourceFile);
            if (!IsReady)
            {
                AddressController controller = new AddressController();
                AddressSettings settings = controller.GetAddressSettings(PortalId);
                cbAllowPickup.Checked = settings.AllowPickup;
                ddlFirstNameRowOrder.SelectedIndex = settings.FirstNameRow;
                ddlLastNameRowOrder.SelectedIndex = settings.LastNameRow;
                cbShowStreet.Checked = settings.ShowStreet;
                ddlStreetRowOrder.SelectedIndex = settings.StreetRow;
                cbShowUnit.Checked = settings.ShowUnit;
                ddlUnitRowOrder.SelectedIndex = settings.UnitRow;
                cbRequireUnit.Checked = settings.RequireUnit;
                cbShowPostal.Checked = settings.ShowPostal;
                ddlPostalRowOrder.SelectedIndex = settings.PostalRow;
                cbShowCity.Checked = settings.ShowCity;
                ddlCityRowOrder.SelectedIndex = settings.CityRow;
                cbShowCountry.Checked = settings.ShowCountry;
                ddlCountryRowOrder.SelectedIndex = settings.CountryRow;
                trRestrictToCountries.Visible = cbShowCountry.Checked;
                cbRestrictToCountry.Checked = settings.RestrictToCountries;
                trAuthorizedCountries.Visible = cbRestrictToCountry.Checked;
                FillAuthorizedCountries(settings.AuthorizedCountries);
                cbShowRegion.Checked = settings.ShowRegion;
                ddlRegionRowOrder.SelectedIndex = settings.RegionRow;
                cbRequireRegion.Checked = settings.RequireRegion;
                ddlEmailRowOrder.SelectedIndex = settings.EmailRow;
                cbShowTelephone.Checked = settings.ShowTelephone;
                cbRequireTelephone.Checked = settings.RequireTelephone;
                ddlTelephoneRowOrder.SelectedIndex = settings.TelephoneRow;
                cbShowCell.Checked = settings.ShowCell;
                cbRequireCell.Checked = settings.RequireCell;
                ddlCellRowOrder.SelectedIndex = settings.CellRow;
            }
		}

        protected void btnSaveSettings_Click(object sender, EventArgs e)
        {
            AddressController controller = new AddressController();
            AddressSettings settings = new AddressSettings
            {
                AllowPickup = cbAllowPickup.Checked,
                FirstNameRow = ddlFirstNameRowOrder.SelectedIndex,
                LastNameRow = ddlLastNameRowOrder.SelectedIndex,
                ShowStreet = cbShowStreet.Checked,
                StreetRow = ddlStreetRowOrder.SelectedIndex,
                ShowUnit = cbShowUnit.Checked,
                UnitRow = ddlUnitRowOrder.SelectedIndex,
                RequireUnit = cbRequireUnit.Checked,
                ShowPostal = cbShowPostal.Checked,
                PostalRow = ddlPostalRowOrder.SelectedIndex,
                ShowCity = cbShowCity.Checked,
                CityRow = ddlCityRowOrder.SelectedIndex,
                ShowCountry = cbShowCountry.Checked,
                CountryRow = ddlCountryRowOrder.SelectedIndex,
                RestrictToCountries = cbRestrictToCountry.Checked
            };
            if (settings.RestrictToCountries)
                settings.AuthorizedCountries = GetSelectedCountries();
            settings.ShowRegion = cbShowRegion.Checked;
            settings.RegionRow = ddlRegionRowOrder.SelectedIndex;
            settings.RequireRegion = cbRequireRegion.Checked;
            settings.EmailRow = ddlEmailRowOrder.SelectedIndex;
            settings.ShowTelephone = cbShowTelephone.Checked;
            settings.RequireTelephone = cbRequireTelephone.Checked;
            settings.TelephoneRow = ddlTelephoneRowOrder.SelectedIndex;
            settings.ShowCell = cbShowCell.Checked;
            settings.RequireCell = cbRequireCell.Checked;
            settings.CellRow = ddlCellRowOrder.SelectedIndex;
            controller.UpdateAddressSettings(PortalId, settings);
            string cacheKey = "StoreAddressInputControls_P" + PortalSettings.PortalId;
            DataCache.RemoveCache(cacheKey);
        }

        protected void cbShowCountry_CheckedChanged(object sender, EventArgs e)
        {
            trRestrictToCountries.Visible = cbShowCountry.Checked;
        }

        protected void cbRestrictToCountry_CheckedChanged(object sender, EventArgs e)
        {
            trAuthorizedCountries.Visible = cbRestrictToCountry.Checked;
        }

		#endregion

        #region Private Methods

        private void FillAuthorizedCountries(string authorizedCountries)
        {
            ListController listController = new ListController();
            IEnumerable<ListEntryInfo> listEntries = listController.GetListEntryInfoItems("Country");

            lbAuthorizedCountries.DataSource = listEntries;
            lbAuthorizedCountries.DataBind();

            if (!string.IsNullOrEmpty(authorizedCountries))
            {
                foreach (string authorizedCountry in authorizedCountries.Split(','))
                {
                    ListItem item = lbAuthorizedCountries.Items.FindByValue(authorizedCountry);
                    if (item != null)
                        item.Selected = true;
                }
            }
        }

        private string GetSelectedCountries()
        {
            string selectedCountries = string.Empty;

            foreach(ListItem item in lbAuthorizedCountries.Items)
            {
                if (item.Selected)
                    selectedCountries += item.Value + ",";
            }

            return selectedCountries.TrimEnd(',');
        }

        #endregion
    }
}
