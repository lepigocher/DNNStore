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
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider
{
    /// <summary>
    /// Address Checkout control manage billing and shipping addresses and the shipping mode.
    /// </summary>
    public partial class DefaultAddressCheckout : AddressCheckoutControlBase
    {
        #region Private Members

        private readonly AddressController _controller = new AddressController();
        private string _addressDefaultDescription;

        #endregion

        #region Event Handlers

        override protected void OnInit(EventArgs e)
        {
            Page.RegisterRequiresControlState(this);
            if (!IsPostBack)
            {
                // Initialize data binding fields
                lstBillAddress.DataTextField = "Description";
                lstBillAddress.DataValueField = "AddressID";
                lstShipAddress.DataTextField = "Description";
                lstShipAddress.DataValueField = "AddressID";
                // Get address settings
                AddressSettings addressSettings = _controller.GetAddressSettings(PortalId);
                // Set Billing and Shipping address settings
                addressBilling.Settings = addressSettings;
                addressShipping.Settings = addressSettings;
                // Hide 'Pick-Up' option if not enabled
                if (!addressSettings.AllowPickup)
                {
                    radNone.Visible = false;
                    lblNone.Visible = false;
                }
            }
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
                    if (p.Second is ShippingMode)
                        Shipping = (ShippingMode)p.Second;
                }
                else
                {
                    if (savedState is ShippingMode)
                        Shipping = (ShippingMode)savedState;
                    else
                        base.LoadControlState(savedState);
                }
            }
            else
                Shipping = ShippingMode.SameAsBilling;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Get default address description
            _addressDefaultDescription = Localization.GetString("AddressDefaultDescription", LocalResourceFile);

            if (!IsPostBack)
            {
                // Hide shipping address block if no delivery is defined
                fsShippingAddress.Visible = !NoDelivery;

                bool addressChanged = false;
                IAddressInfo billingAddress = BillingAddress;
                if (billingAddress != null)
                {
                    int billingAddressID = billingAddress.AddressID;
                    IAddressInfo shippingAddress = ShippingAddress;
                    int shippingAddressID = shippingAddress.AddressID;

                    // If the user is logged
                    if (IsLogged)
                    {
                        //Get the user's addresses
                        List<AddressInfo> addresses = _controller.GetAddresses<AddressInfo>(PortalId, UserId, _addressDefaultDescription);

                        // Bind addresses lists
                        BindBillingAddressList(addresses);

                        // If billing address is not set
                        if (billingAddressID == Null.NullInteger)
                        {
                            // Search for the primary address
                            bool primaryAddressFound = false;
                            foreach (AddressInfo address in addresses)
                            {
                                if (address.PrimaryAddress)
                                {
                                    billingAddress = address;
                                    primaryAddressFound = true;
                                    break;
                                }
                            }
                            // Define selected addresses
                            if (primaryAddressFound)
                            {
                                billingAddressID = billingAddress.AddressID;
                                BillingAddress = billingAddress;
                                shippingAddressID = billingAddressID;
                                ShippingAddress = billingAddress;
                                addressChanged = true;
                            }
                        }
                    }
                    else
                        rowListBillAddress.Visible = false;

                    // Update billing interface
                    UpdateBillingInterface();

                    // Define shipping mode then update shipping interface
                    if (shippingAddressID != Null.NullInteger && shippingAddressID != billingAddressID)
                    {
                        Shipping = ShippingMode.Other;
                        radShipping.Checked = true;
                        DisplayShippingAddressSelection(true);
                    }
                    else
                    {
                        Shipping = ShippingMode.SameAsBilling;
                        radBilling.Checked = true;
                        DisplayShippingAddressSelection(false);
                    }
                }
                // Callback Checkout control to save order with new addresses
                if (addressChanged)
                    SendBillingAddressChangedEvent();
            }
        }

        protected void lstBillAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBillAddress(int.Parse(lstBillAddress.SelectedValue));
            if (Shipping == ShippingMode.SameAsBilling)
                ShippingAddress = BillingAddress;
            UpdateBillingInterface();
            SendBillingAddressChangedEvent();
        }

        protected void lstShipAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadShipAddress(int.Parse(lstShipAddress.SelectedValue));
            UpdateShippingInterface();
            SendShippingAddressChangedEvent();
        }

        protected void radNone_CheckedChanged(object sender, EventArgs e)
        {
            if (radNone.Checked)
            {
                Shipping = ShippingMode.None;
                if (lstShipAddress.Items.Count > 0)
                {
                    lstShipAddress.SelectedIndex = 0;
                    LoadShipAddress(int.Parse(lstShipAddress.SelectedValue));
                }
                else
                    LoadShipAddress(Null.NullInteger);
                DisplayShippingAddressSelection(false);
                SendShippingAddressChangedEvent();
            }
        }

        protected void radBilling_CheckedChanged(object sender, EventArgs e)
        {
            if (radBilling.Checked)
            {
                Shipping = ShippingMode.SameAsBilling;
                ShippingAddress = BillingAddress;
                DisplayShippingAddressSelection(false);
                SendShippingAddressChangedEvent();
            }
        }

        protected void radShipping_CheckedChanged(object sender, EventArgs e)
        {
            if (radShipping.Checked)
            {
                Shipping = ShippingMode.Other;
                if (lstShipAddress.Items.Count > 0)
                    lstShipAddress.SelectedIndex = 0;
                LoadShipAddress(Null.NullInteger);
                DisplayShippingAddressSelection(true);
                SendShippingAddressChangedEvent();
            }
        }

        protected override object SaveControlState()
        {
            object obj = base.SaveControlState();

            if (obj != null)
                return new Pair(obj, Shipping);

            return (Shipping);
        }

        #endregion

        #region Properties

        public override IAddressInfo BillingAddress
        {
            get { return addressBilling.Address; }
            set
            {
                addressBilling.Address = value;
                if (IsPostBack && IsLogged)
                {
                    List<AddressInfo> addresses = _controller.GetAddresses<AddressInfo>(PortalId, UserId, _addressDefaultDescription);
                    BindBillingAddressList(addresses);
                    UpdateBillingInterface();
                }
            }
        }

        public override IAddressInfo ShippingAddress
        {
            get { return addressShipping.Address; }
            set
            {
                addressShipping.Address = value;
                if (IsPostBack && IsLogged)
                {
                    List<AddressInfo> addresses = _controller.GetAddresses<AddressInfo>(PortalId, UserId, _addressDefaultDescription);
                    BindShippingAddressList(addresses);
                    UpdateShippingInterface();
                }
            }
        }

        public override ShippingMode Shipping { get; protected set; }

        #endregion

        #region Private Methods

        private void BindBillingAddressList(List<AddressInfo> addresses)
        {
            lstBillAddress.DataSource = addresses;
            lstBillAddress.DataBind();
            lstBillAddress.Items.Insert(0, new ListItem(Localization.GetString("SelectBillingAddress", LocalResourceFile), "-1"));

            BindShippingAddressList(addresses);
        }

        private void BindShippingAddressList(List<AddressInfo> addresses)
        {
            lstShipAddress.DataSource = addresses;
            lstShipAddress.DataBind();
            lstShipAddress.Items.Insert(0, new ListItem(Localization.GetString("SelectShippingAddress", LocalResourceFile), "-1"));
        }

        private void UpdateBillingInterface()
        {
            // Update billing interface
            addressBilling.ShowUserSaved = false;
            addressBilling.ShowDescription = false;
            int billingAddressID = BillingAddress.AddressID;

            if (billingAddressID > 0)
            {
                if (IsLogged)
                {
                    lstBillAddress.ClearSelection();
                    ListItem item = lstBillAddress.Items.FindByValue(billingAddressID.ToString());
                    if (item != null)
                        item.Selected = true;
                    else
                        addressBilling.ShowUserSaved = true;
                }
            }
            else
            {
                if (IsLogged)
                    addressBilling.ShowUserSaved = true;
            }
        }

        private void UpdateShippingInterface()
        {
            // Update shipping interface
            addressShipping.ShowUserSaved = false;
            addressShipping.ShowDescription = false;
            int shippingAddressID = ShippingAddress.AddressID;

            if (shippingAddressID > 0)
            {
                if (IsLogged)
                {
                    lstShipAddress.ClearSelection();
                    ListItem item = lstShipAddress.Items.FindByValue(shippingAddressID.ToString());
                    if (item != null)
                        item.Selected = true;
                    else
                        addressShipping.ShowUserSaved = true;
                }
            }
            else
            {
                if (IsLogged)
                    addressShipping.ShowUserSaved = true;
            }
        }

        /// <summary>
        /// Retrieve an IAddressInfo from the address controller using the address id.
        /// </summary>
        /// <param name="addressId">Address ID to be loaded.</param>
        /// <returns>
        /// A populated IAddressInfo if the address was found;
        /// otherwise the user's registration address.
        /// </returns>
        private IAddressInfo LoadAddress(int addressId)
        {
            if (addressId > 0)
                return _controller.GetAddress(addressId);

            return _controller.GetRegistrationAddress(PortalId, UserId, _addressDefaultDescription);
        }

        private void LoadBillAddress(int addressId)
        {
            IAddressInfo address = null;

            if (addressId != Null.NullInteger)
                address = LoadAddress(addressId);

            if (address != null)
            {
                // Get user account email if user is connected and email address is empty
                if (UserId != Null.NullInteger && string.IsNullOrEmpty(address.Email))
                    address.Email = UserInfo.Email;
                BillingAddress = address;
            }
            else
                BillingAddress = new AddressInfo();
        }

        private void LoadShipAddress(int addressId)
        {
            IAddressInfo address = null;

            if (addressId >= 0)
                address = LoadAddress(addressId);

            if (address != null)
            {
                // Get user account email if user is connected and email address is empty
                if (UserId != Null.NullInteger && string.IsNullOrEmpty(address.Email))
                    address.Email = UserInfo.Email;
                ShippingAddress = address;
            }
            else
                ShippingAddress = new AddressInfo();
        }

        private void DisplayShippingAddressSelection(bool display)
        {
            rowListShipAddress.Visible = display ? IsLogged : false;
            addressShipping.Visible = display;
            UpdateShippingInterface();
        }

        #endregion
    }
}
