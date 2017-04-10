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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using DotNetNuke.Modules.Store.Customer;
using DotNetNuke.Modules.Store.Providers.Address;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for AuthNetGatewayProvider.
	/// </summary>
    public sealed class AuthNetGatewayProvider
	{
		#region Constructor

		public AuthNetGatewayProvider(string gatewaySettings)
		{
			_gatewaySettings = gatewaySettings;
		}

		#endregion

		#region Private Members

		private readonly string _gatewaySettings = string.Empty;

		#endregion

		#region Public Methods

        public TransactionResult ProcessTransaction(IAddressInfo shipping, IAddressInfo billing, OrderInfo orderInfo, TransactionDetails trans)
		{
			TransactionResult result = new TransactionResult();

            CultureInfo ciEnUs = new CultureInfo("en-US");

			// Check data before performing transaction
			AuthNetSettings settings = new AuthNetSettings(_gatewaySettings);
			if (!settings.IsValid())
			{
				result.Succeeded = false;
                result.ResultCode = -3;

				return result;
			}

			if (billing == null)
			{
				result.Succeeded = false;
                result.ResultCode = -4;

				return result;
			}

			if (trans == null || !trans.IsValid())
			{
				result.Succeeded = false;
                result.ResultCode = -5;

				return result;
			}

			// Gather transaction information
			string url = settings.GatewayURL;
			NameValueCollection NVCol = new NameValueCollection();
            // Merchant infos
            NVCol.Add("x_login", settings.Username);//Req
            NVCol.Add("x_tran_key", settings.Password);//Req
            NVCol.Add("x_version", settings.Version);//Req
            NVCol.Add("x_test_request", settings.IsTest.ToString().ToUpper());
            // Init infos
            NVCol.Add("x_delim_data", "TRUE");
            NVCol.Add("x_delim_char", "|");
            NVCol.Add("x_encap_char", "");
            NVCol.Add("x_relay_response", "FALSE");//Req
            //New in Store 3.1.10, added by Authorize in February 2014
            NVCol.Add("x_market_type", "0");// 0=eCommerce, 1 MOTO, 2 Retail
            // Billing infos
            NVCol.Add("x_first_name", billing.FirstName);
            NVCol.Add("x_last_name", billing.LastName);
            NVCol.Add("x_company", "");
            NVCol.Add("x_address", (billing.Address1 + " " + billing.Address2).Trim());
            NVCol.Add("x_city", billing.City);
            NVCol.Add("x_state", billing.RegionCode);
            NVCol.Add("x_zip", billing.PostalCode);
            NVCol.Add("x_country", billing.CountryCode);
            NVCol.Add("x_phone", billing.Phone1);
            // Shipping infos
            NVCol.Add("x_ship_to_first_name", shipping.FirstName);
            NVCol.Add("x_ship_to_last_name", shipping.LastName);
            NVCol.Add("x_ship_to_company", "");
            NVCol.Add("x_ship_to_address", (shipping.Address1 + " " + shipping.Address2).Trim());
            NVCol.Add("x_ship_to_city", shipping.City);
            NVCol.Add("x_ship_to_state", shipping.RegionCode);
            NVCol.Add("x_ship_to_zip", shipping.PostalCode);
            NVCol.Add("x_ship_to_country", shipping.CountryCode);
            // Customer infos
            NVCol.Add("x_cust_id", billing.UserID.ToString());
            NVCol.Add("x_customer_ip", HttpContext.Current.Request.UserHostAddress);
            // Order infos
            NVCol.Add("x_invoice_num", orderInfo.OrderID.ToString());
            NVCol.Add("x_amount", orderInfo.GrandTotal.ToString("0.00", ciEnUs));//Req
            NVCol.Add("x_tax", orderInfo.TaxTotal.ToString("0.00", ciEnUs));
            NVCol.Add("x_freight", orderInfo.ShippingCost.ToString("0.00", ciEnUs));
            // Transaction infos
            NVCol.Add("x_method", "CC");//CC=Credit Card could be also ECHECK
            NVCol.Add("x_type", settings.Capture.ToString());//Req
            NVCol.Add("x_recurring_billing", "NO");
            NVCol.Add("x_card_num", trans.CardNumber);//Req
            NVCol.Add("x_card_code", trans.VerificationCode);
            NVCol.Add("x_exp_date", trans.ExpirationMonth.ToString("00") + "/" + trans.ExpirationYear);//Req
            // Order details
            string fieldSep = "<|>";
            OrderController orderController = new OrderController();
            List<OrderDetailInfo> orderDetails = orderController.GetOrderDetails(orderInfo.OrderID);
            ArrayList items = new ArrayList(orderDetails.Count);
            foreach (OrderDetailInfo detail in orderDetails)
            {
                string modelNumber = detail.ModelNumber;
                if (modelNumber.Length > 31)
                    modelNumber = modelNumber.Substring(0, 31);

                string modelName = detail.ModelName;
                if (modelName.Length > 31)
                    modelName = modelName.Substring(0, 31);

                items.Add(modelNumber + fieldSep + modelName + fieldSep + fieldSep + detail.Quantity +
                          fieldSep + detail.UnitCost.ToString("0.00", ciEnUs) + fieldSep + "Y");
            }
            // Perform transaction
			try
			{
				Encoding enc = Encoding.GetEncoding(1252);
                StreamReader loResponseStream = new StreamReader(PostEx(url, NVCol, items).GetResponseStream(), enc);
                
				string lcHtml = loResponseStream.ReadToEnd();
				loResponseStream.Close();
				
				string[] resultArray = lcHtml.Split('|');

                result.Succeeded = (resultArray[0] == "1");
                result.ResultCode = int.Parse(resultArray[0]);
                result.ReasonCode = int.Parse(resultArray[2]);
				result.Message = resultArray[3];
			}
			catch (Exception ex)
			{
				result.Succeeded = false;
                result.ResultCode = -2;
                result.Message = ex.Message; 
			}

			return result;
		}

		#endregion

		#region Private Methods

        private WebResponse PostEx(string url, NameValueCollection values, ArrayList lineItems)
		{
            Stream stream;
			byte[] bytes;

			WebRequest request = WebRequest.Create(url);
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";

			if (values.Count == 0)
				request.ContentLength = 0;
			else
			{
				StringBuilder builder = new StringBuilder();
				string[] keys = values.AllKeys;
				foreach (string key in keys)
				{
					if (builder.Length > 0)
						builder.Append("&");
					builder.Append(key);
					builder.Append("=");
					builder.Append(HttpUtility.UrlEncode(values[key]));
				}
                foreach (string lineItem in lineItems)
                    builder.Append("&x_line_item=" + HttpUtility.UrlEncode(lineItem));
				bytes = Encoding.UTF8.GetBytes(builder.ToString());
				request.ContentLength = bytes.Length;
				stream = request.GetRequestStream();
				stream.Write(bytes, 0, bytes.Length);
				stream.Close();
			}
			return request.GetResponse();
		}

		#endregion
	}
}
