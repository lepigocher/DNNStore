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
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;

using DotNetNuke.Common.Lists;

using DotNetNuke.Modules.Store.Core.Customer;
using DotNetNuke.Modules.Store.Core.Providers.Address;


namespace DotNetNuke.Modules.Store.Core.Cart
{
    public sealed class SystempayGatewayProvider
    {
		#region Private Members

		private readonly string _gatewaySettings = string.Empty;

        #endregion

		#region Constructor

        public SystempayGatewayProvider(string gatewaySettings)
		{
			_gatewaySettings = gatewaySettings;
		}

		#endregion

		#region Public Methods

        public void ProcessTransaction(IAddressInfo billing, OrderInfo orderInfo, TransactionDetails transaction)
		{
            if (transaction.IsValid())
            {
                SystempaySettings settings = new SystempaySettings(_gatewaySettings);
                RemoteForm systempay = new RemoteForm("systempayform", settings.PaymentURL);
                // Main fields
                systempay.Fields.Add("vads_version", "V2");
                systempay.Fields.Add("vads_site_id", settings.SiteID);
                systempay.Fields.Add("vads_ctx_mode", settings.UseTestCertificate ? "TEST" : "PRODUCTION");
                if(!string.IsNullOrEmpty( settings.Contracts))
                    systempay.Fields.Add("vads_contracts", settings.Contracts);
                systempay.Fields.Add("vads_page_action", "PAYMENT");
                systempay.Fields.Add("vads_action_mode", "INTERACTIVE");
                systempay.Fields.Add("vads_payment_config", "SINGLE");
                systempay.Fields.Add("vads_capture_delay", "0");
                //systempay.Fields.Add("vads_validation_mode", "0");
                systempay.Fields.Add("vads_trans_id", GetTransactionID());
                systempay.Fields.Add("vads_trans_date", GetTransactionDate());
                systempay.Fields.Add("vads_currency", settings.Currency);
                systempay.Fields.Add("vads_language", settings.Language);
                systempay.Fields.Add("vads_return_mode", "POST");
                systempay.Fields.Add("vads_url_return", transaction.ReturnURL);
                systempay.Fields.Add("vads_url_refused", transaction.RefusedURL);
                systempay.Fields.Add("vads_url_error", transaction.ErrorURL);
                systempay.Fields.Add("vads_url_cancel", transaction.CancelURL);
                systempay.Fields.Add("vads_url_check", transaction.NotifyURL);
                systempay.Fields.Add("vads_shop_name", transaction.ShopName);
                systempay.Fields.Add("vads_theme_config", transaction.Buttons);

                // Customer fields
                systempay.Fields.Add("vads_cust_id", orderInfo.CustomerID.ToString());
                systempay.Fields.Add("vads_cust_first_name", billing.FirstName);
                systempay.Fields.Add("vads_cust_last_name", billing.LastName);
                string address = (billing.Address1 + " " + billing.Address2).Trim();
                if (!string.IsNullOrEmpty(address))
                    systempay.Fields.Add("vads_cust_address", address);
                if (!string.IsNullOrEmpty(billing.PostalCode))
                    systempay.Fields.Add("vads_cust_zip", billing.PostalCode);
                if (!string.IsNullOrEmpty(billing.City))
                    systempay.Fields.Add("vads_cust_city", billing.City);
                // Get ISO country code for specified country name
                string country = GetISOCountryCode(billing.CountryCode);
                if (!string.IsNullOrEmpty(country))
                    systempay.Fields.Add("vads_cust_country", country);
                if (!string.IsNullOrEmpty(billing.Phone1))
                    systempay.Fields.Add("vads_cust_phone", billing.Phone1);
                if (!string.IsNullOrEmpty(billing.Phone2))
                    systempay.Fields.Add("vads_cust_cell_phone", billing.Phone2);
                systempay.Fields.Add("vads_cust_email", transaction.Email);

                // Order fields
                systempay.Fields.Add("vads_order_id", orderInfo.OrderID.ToString());
                // Order details
                OrderController orderController = new OrderController();
                List<OrderDetailInfo> orderDetails = orderController.GetOrderDetails(orderInfo.OrderID);
                int itemNumber = 0;
                foreach (OrderDetailInfo detail in orderDetails)
                {
                    string prodRef = !string.IsNullOrEmpty(detail.ModelNumber) ? detail.ModelNumber : detail.ProductID.ToString();
                    systempay.Fields.Add("vads_product_ref" + itemNumber, prodRef);
                    systempay.Fields.Add("vads_product_label" + itemNumber, detail.ModelName);
                    systempay.Fields.Add("vads_product_qty" + itemNumber, detail.Quantity.ToString());
                    systempay.Fields.Add("vads_product_amount" + itemNumber, FormatAmount(detail.UnitCost));
                    itemNumber++;
                }
                systempay.Fields.Add("vads_nb_products", orderDetails.Count.ToString());
                systempay.Fields.Add("vads_amount", FormatAmount(orderInfo.GrandTotal));

                // Shipping
                if (orderInfo.ShippingCost > 0)
                    systempay.Fields.Add("vads_shipping_amount", FormatAmount(orderInfo.ShippingCost));
                // Tax
                if (orderInfo.TaxTotal > 0)
                    systempay.Fields.Add("vads_tax_amount", FormatAmount(orderInfo.TaxTotal));

                // Add computed signature
                systempay.Fields.Add("signature", GetSignature(systempay.Fields, settings.Certificate));
                // Post the form to the client browser then submit it to Systempay using JavaScript
                systempay.Post(true);
            }
        }

        #endregion

        #region Private Methods

        public static String GetTransactionDate()
        {
            return DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        }

        public static string GetTransactionID()
        {
            double diff = DateTime.UtcNow.TimeOfDay.TotalMilliseconds / 100;

            return String.Format("{0:000000}", diff); 
        } 

        private static string ComputeSHA1(string text, Encoding encoding)
        {
            byte[] buffer = encoding.GetBytes(text);

            SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider();
            string hash = BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");

            return hash;
        }

        public static string GetSignature(NameValueCollection data, string certificate)
        {
            string signature = "";

            if (!string.IsNullOrEmpty(data["vads_contrib"]))
                data.Add("vads_contrib", "DNN_STORE_V3.1.10");

            string[] keys = data.AllKeys;

            Array.Sort(keys, StringComparer.InvariantCultureIgnoreCase);

            foreach (string key in keys)
                signature += data[key] + "+";

            signature += certificate;

            return ComputeSHA1(signature, Encoding.UTF8);
        }

        private static string GetISOCountryCode(string country)
        {
            string isoCode = string.Empty;

            if (country == "United Kingdom")
                isoCode = "GB"; // Correct a bug value with older DNN
            else
            {
                ListController listController = new ListController();
                IEnumerable<ListEntryInfo> listEntries = listController.GetListEntryInfoItems("Country");

                foreach (ListEntryInfo itemInfo in listEntries)
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

        private static string FormatAmount(decimal amount)
        {
            return Convert.ToInt32(amount * 100).ToString();
        }

        #endregion
    }
}
