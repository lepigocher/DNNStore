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
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Store.Providers.Tax.CountryTaxProvider
{
	/// <summary>
    /// Summary description for CountryTaxAdmin.
	/// </summary>
	public partial class CountryTaxAdmin : ProviderControlBase
    {
        #region Private Members

        private readonly ListController _listController = new ListController();
	    private ListEntryInfoCollection _countries;

        #endregion

		#region Events

        override protected void OnInit(EventArgs e)
		{
            grdCountryTaxRates.ItemCreated += grdCountryTaxRates_ItemCreated;
            grdCountryTaxRates.ItemCommand += grdCountryTaxRates_ItemCommand;
			btnSaveTaxRates.Click += btnSaveTaxRates_Click;
            _countries = _listController.GetListEntryInfoCollection("Country");
			base.OnInit(e);
		}

        protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsReady)
                LoadTaxRates();
		}

		private void btnSaveTaxRates_Click(object sender, EventArgs e)
		{
			SaveTaxRates();
		}

        void grdCountryTaxRates_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlCountries = (DropDownList)e.Item.FindControl("ddlCountries");
                ddlCountries.DataSource = _countries;
                ddlCountries.DataTextField = "Text";
                ddlCountries.DataValueField = "Value";
                ddlCountries.AutoPostBack = true;
                ddlCountries.SelectedIndexChanged += ddlCountries_SelectedIndexChanged;
            }
        }

        void grdCountryTaxRates_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                DropDownList ddlCountries = (DropDownList) e.Item.FindControl("ddlCountries");
                string countryCode = ddlCountries.SelectedValue;

                DropDownList ddlRegions = (DropDownList)e.Item.FindControl("ddlRegions");
                string regionCode = null;
                if (ddlRegions != null && ddlRegions.Visible)
                    regionCode = ddlRegions.SelectedValue;

                TextBox txtTaxRate = (TextBox)e.Item.FindControl("txtTaxRate");
                string value = txtTaxRate.Text;
                if (string.IsNullOrEmpty(value))
                {
                    ShowError(lblErrorTaxRates, Localization.GetString("ErrorTaxRateEmpty", LocalResourceFile), txtTaxRate);
                    return;
                }
                TextBox txtZipCode = (TextBox)e.Item.FindControl("txtZipCode");
                decimal taxRate;
                if (!Decimal.TryParse(value, out taxRate))
                {
                    ShowError(lblErrorTaxRates, Localization.GetString("ErrorNotValidTaxRate", LocalResourceFile), txtTaxRate);
                    return;
                }
                ClearError(lblErrorTaxRates, txtTaxRate);

                CountryTaxInfo countryTaxInfo = new CountryTaxInfo
                                                    {
                                                        CountryCode = countryCode,
                                                        RegionCode = regionCode,
                                                        ZipCode = txtZipCode.Text,
                                                        TaxRate = taxRate
                                                    };

                CountryTaxRates countryTaxRates = (CountryTaxRates)ViewState["Store_CountryTaxRates"];
                countryTaxRates.TaxRates.Add(countryTaxInfo);
                countryTaxRates.TaxRates.Sort();
                ViewState["Store_CountryTaxRates"] = countryTaxRates;

                grdCountryTaxRates.DataSource = countryTaxRates.TaxRates;
                grdCountryTaxRates.DataBind();
            }
        }

        void ddlCountries_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlRegions = (DropDownList) Globals.FindControlRecursiveDown(grdCountryTaxRates, "ddlRegions");
            if (ddlRegions != null)
            {
                DropDownList ddlCountries = (DropDownList) sender;
                ListEntryInfoCollection regions = _listController.GetListEntryInfoCollection("Region", "Country." + ddlCountries.SelectedValue);
                if (regions != null && regions.Count > 0)
                {
                    ddlRegions.Visible = true;
                    ddlRegions.DataSource = regions;
                    ddlRegions.DataTextField = "Text";
                    ddlRegions.DataValueField = "Value";
                    ddlRegions.DataBind();
                    ListItem item = new ListItem(Localization.GetString("AnyRegion", LocalResourceFile), "*");
                    ddlRegions.Items.Insert(0, item);
                }
                else
                    ddlRegions.Visible = false;
            }
        }

		#endregion

        #region Public Methods

	    public string GetCountryName(CountryTaxInfo item)
        {
            string countryName = "";

            foreach (ListEntryInfo entryInfo in _countries)
            {
                if (item.CountryCode.Equals(entryInfo.Value))
                {
                    countryName = entryInfo.Text;
                    break;
                }
            }

            return countryName;
        }

        public string GetRegionName(CountryTaxInfo item)
        {
            if (item.RegionCode == "*")
                return Localization.GetString("AnyRegion", LocalResourceFile);

            string regionName = "";

            ListEntryInfoCollection regions = _listController.GetListEntryInfoCollection("Region", "Country." + item.CountryCode);
            foreach (ListEntryInfo entryInfo in regions)
            {
                if (entryInfo.Value.Equals(item.RegionCode))
                {
                    regionName = entryInfo.Text;
                    break;
                }
            }

            return regionName;
        }

        #endregion

        #region Private Methods

        private void SaveTaxRates()
		{
            if (cbEnableTax.Checked && txtDefaultTaxRate.Text.Length == 0)
            {
                ShowError(lblErrorDefaultTaxRate, Localization.GetString("ErrorTaxRateEmpty", LocalResourceFile), txtDefaultTaxRate);
                return;
            }

            decimal defaultTaxRate = Null.NullDecimal;

            if (cbEnableTax.Checked && !Decimal.TryParse(txtDefaultTaxRate.Text, out defaultTaxRate))
            {
                ShowError(lblErrorDefaultTaxRate, Localization.GetString("ErrorNotValidTaxRate", LocalResourceFile), txtDefaultTaxRate);
                return;
            }

            ClearError(lblErrorDefaultTaxRate, txtDefaultTaxRate);

            CountryTaxRates countryTaxRates = (CountryTaxRates) ViewState["Store_CountryTaxRates"];
            List<int> deletedItems = new List<int>();
            bool hasError = false;

            foreach (DataGridItem gridItem in grdCountryTaxRates.Items)
            {
                CheckBox chkDelete = (CheckBox) gridItem.FindControl("chkDelete");
                if (chkDelete != null && chkDelete.Checked)
                    deletedItems.Add(gridItem.ItemIndex);
                else
                {
                    TextBox txtTaxRate = (TextBox) gridItem.FindControl("txtTaxRate");
                    if (txtTaxRate != null)
                    {
                        string textTaxRate = txtTaxRate.Text;
                        if (!string.IsNullOrEmpty(textTaxRate))
                        {
                            decimal taxRate;
                            if (decimal.TryParse(textTaxRate, out taxRate))
                            {
                                countryTaxRates.TaxRates[gridItem.ItemIndex].TaxRate = taxRate;
                            }
                            else
                            {
                                hasError = true;
                                ShowError(lblErrorTaxRates, Localization.GetString("ErrorNotValidTaxRate", LocalResourceFile), txtTaxRate);
                                break;
                            }
                        }
                        else
                        {
                            hasError = true;
                            ShowError(lblErrorTaxRates, Localization.GetString("ErrorTaxRateEmpty", LocalResourceFile), txtTaxRate);
                            break;
                        }
                    }
                }
            }
            if (hasError)
                return;

            if (deletedItems.Count > 0)
            {
                deletedItems.Reverse();
                foreach (int deletedItem in deletedItems)
                    countryTaxRates.TaxRates.RemoveAt(deletedItem);
            }

            countryTaxRates.TaxRates.Sort();

            ClearError(lblErrorTaxRates, null);

            string taxRates = ProviderSettingsHelper.SerializeSettings(countryTaxRates, typeof(CountryTaxRates));

            TaxController controller = new TaxController();
            controller.UpdateTaxRates(PortalId, defaultTaxRate, cbEnableTax.Checked, taxRates);

            ViewState["Store_CountryTaxRates"] = countryTaxRates;
            grdCountryTaxRates.DataSource = countryTaxRates.TaxRates;
            grdCountryTaxRates.DataBind();
        }

		private void LoadTaxRates()
		{
            TaxController controller = new TaxController();
            TaxInfo taxInfo = controller.GetTaxRates(PortalId);
            if (taxInfo != null)
            {
                txtDefaultTaxRate.Text = taxInfo.DefaultTaxRate < 0 ? "" : taxInfo.DefaultTaxRate.ToString("0.00");
                cbEnableTax.Checked = taxInfo.ShowTax;
                CountryTaxRates countryTaxRates = taxInfo.CountryTaxes;
                ViewState["Store_CountryTaxRates"] = countryTaxRates;
                grdCountryTaxRates.DataSource = countryTaxRates.TaxRates;
                grdCountryTaxRates.DataBind();
            }
		    IsReady = true;
		}

        private void ShowError(Label labelError, string errorMessage, TextBox control)
        {
            labelError.Visible = true;
            labelError.Text = errorMessage;
            control.ForeColor = System.Drawing.Color.Red;
            control.BorderColor = System.Drawing.Color.Red;
            control.Focus();
        }

        private void ClearError(Label labelError, TextBox control)
        {
            labelError.Visible = false;
            if (control != null)
            {
                control.ForeColor = System.Drawing.Color.Empty;
                control.BorderColor = System.Drawing.Color.Empty;
            }
        }

		#endregion
	}
}
