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
using System.Globalization;
using System.Text.RegularExpressions;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Modules.Store.Customer;
using DotNetNuke.Modules.Store.Providers.Address;

namespace DotNetNuke.Modules.Store.Cart
{
    public sealed class PayPalGatewayProvider
    {
		#region Constructor

        public PayPalGatewayProvider(string gatewaySettings)
		{
			_gatewaySettings = gatewaySettings;
		}

		#endregion

		#region Private Members

        private const string SandboxPaymentURL = "https://www.sandbox.paypal.com/cgi-bin/webscr/";
		private readonly string _gatewaySettings = string.Empty;
        private string _paymentURL = string.Empty;

        #endregion

		#region Public Methods

        public void ProcessTransaction(IAddressInfo billing, OrderInfo orderInfo, TransactionDetails transaction)
		{
            PayPalSettings settings = new PayPalSettings(_gatewaySettings);
            if (transaction.IsValid())
            {
                CultureInfo ciEnUs = new CultureInfo("en-US");
                _paymentURL = settings.UseSandbox ? SandboxPaymentURL : settings.PaymentURL;
                RemoteForm paypal = new RemoteForm("paypalform", _paymentURL);
                // Main fields
                paypal.Fields.Add("cmd", "_cart");
                paypal.Fields.Add("upload", "1");
                paypal.Fields.Add("business", settings.PayPalID.ToLower());
                paypal.Fields.Add("charset", settings.Charset);
                paypal.Fields.Add("currency_code", settings.Currency);
                paypal.Fields.Add("invoice", orderInfo.OrderID.ToString());
                paypal.Fields.Add("return", transaction.ReturnURL);
                paypal.Fields.Add("cancel_return", transaction.CancelURL);
                paypal.Fields.Add("notify_url", transaction.NotifyURL);
                paypal.Fields.Add("rm", "2");
                paypal.Fields.Add("lc", settings.Lc);
                paypal.Fields.Add("cbt", transaction.Cbt);
                paypal.Fields.Add("custom", orderInfo.CustomerID.ToString());
                paypal.Fields.Add("email", transaction.Email);
                paypal.Fields.Add("first_name", billing.FirstName);
                paypal.Fields.Add("last_name", billing.LastName);
                if (!string.IsNullOrEmpty(billing.Address1))
                    paypal.Fields.Add("address1", billing.Address1);
                if (!string.IsNullOrEmpty(billing.Address2))
                    paypal.Fields.Add("address2", billing.Address2);
                if (!string.IsNullOrEmpty(billing.City))
                    paypal.Fields.Add("city", billing.City);
                if (!string.IsNullOrEmpty(billing.PostalCode))
                    paypal.Fields.Add("zip", billing.PostalCode);
                // Get ISO country code for specified country name
                string country = GetISOCountryCode(billing.CountryCode);
                if (!string.IsNullOrEmpty(country))
                    paypal.Fields.Add("country", country);
                if (!string.IsNullOrEmpty(billing.Phone1))
                {
                    // Remove all chars but numbers from phone number
                    string phonenumber = Regex.Replace(billing.Phone1, "[^\\d]", "", RegexOptions.Compiled);
                    // If the buyer live in the USA
                    if (country == "US")
                    {
                        // Get US postal code for specified region code and add it to the form
                        paypal.Fields.Add("state", GetUSPostalRegionCode(country, billing.RegionCode));
                        // If the phone number is valid
                        int phoneLength = phonenumber.Length;
                        if (phoneLength > 7)
                        {
                            // Extract area code, three digits prefix and four digits phone number
                            paypal.Fields.Add("night_phone_a", phonenumber.Substring(0, phoneLength - 7));
                            paypal.Fields.Add("night_phone_b", phonenumber.Substring(phoneLength - 7, 3));
                            paypal.Fields.Add("night_phone_c", phonenumber.Substring(phoneLength - 4));
                        }
                    }
                    else
                    {
                        // For International buyers, set country code and phone number
                        //paypal.Fields.Add("night_phone_a", country); HERE PHONE country code is required!
                        paypal.Fields.Add("night_phone_b", phonenumber);
                    }
                }
                // Order details
                OrderController orderController = new OrderController();
                List<OrderDetailInfo> orderDetails = orderController.GetOrderDetails(orderInfo.OrderID);
                int itemNumber = 1;
                foreach (OrderDetailInfo detail in orderDetails)
                {
                    paypal.Fields.Add("item_number_" + itemNumber, detail.ProductID.ToString());
                    paypal.Fields.Add("item_name_" + itemNumber, detail.ProductTitle);
                    paypal.Fields.Add("quantity_" + itemNumber, detail.Quantity.ToString());
                    paypal.Fields.Add("amount_" + itemNumber, detail.UnitCost.ToString("0.00", ciEnUs));
                    itemNumber++;
                }
                // If a valid coupon exists
                if (orderInfo.CouponID != Null.NullInteger)
                {
                    decimal discount = Math.Abs(orderInfo.Discount);
                    paypal.Fields.Add("discount_amount_cart", discount.ToString("0.00", ciEnUs));
                }
                // Shipping
                if (orderInfo.ShippingCost > 0)
                {
                    paypal.Fields.Add("handling_cart", orderInfo.ShippingCost.ToString("0.00", ciEnUs));
                }
                // Tax
                if (orderInfo.TaxTotal > 0)
                {
                    paypal.Fields.Add("tax_cart", orderInfo.TaxTotal.ToString("0.00", ciEnUs));
                }
                // Post the form to the client browser then submit it to PayPal using JavaScript
                paypal.Post();
            }
        }

        #endregion

        #region Private Methods

        private static string GetISOCountryCode(string country)
        {
            string isoCode = string.Empty;

            if (country == "United Kingdom")
                isoCode = "GB";
            else
            {
                ListController ctlEntry = new ListController();
                ListEntryInfoCollection entryCollection = ctlEntry.GetListEntryInfoCollection("Country");

                foreach (ListEntryInfo itemInfo in entryCollection)
                {
                    if (itemInfo.Text == country)
                    {
                        isoCode = itemInfo.Value;
                        break;
                    }
                }
            }

            return isoCode;
        }

        private static string GetUSPostalRegionCode(string ISOCountry, string region)
        {
            string regionCode = string.Empty;
            ListController ctlEntry = new ListController();
            string listKey = "Country." + ISOCountry;
            ListEntryInfoCollection entryCollection = ctlEntry.GetListEntryInfoCollection("Region", listKey);

            foreach (ListEntryInfo itemInfo in entryCollection)
            {
                if (itemInfo.Text == region)
                {
                    regionCode = itemInfo.Value;
                    break;
                }
            }

            return regionCode;
        }

        #endregion
    }
}
