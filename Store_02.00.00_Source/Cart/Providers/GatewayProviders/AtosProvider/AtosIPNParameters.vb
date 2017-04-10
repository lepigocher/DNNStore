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

Imports System.Text

Public Class AtosIPNParameters

#Region "Constructors"

    Public Sub New(ByVal Params As String())
        If Params IsNot Nothing Then
            Me.Code = CType(Params(1), Integer)
            Me.ErrorMessage = Params(2)
            Me.Merchant_ID = Params(3)
            Me.Merchant_Country = Params(4)
            Me.Amount = CType(Params(5), Decimal) / 100
            Me.Transaction_ID = Params(6)
            Me.Payment_Means = Params(7)
            Me.Transmission_Date = Params(8)
            Me.Payment_Time = Params(9)
            Me.Payment_Date = Params(10)
            Me.Response_Code = Params(11)
            Me.Payment_Certificate = Params(12)
            Me.Authorisation_ID = Params(13)
            Me.Currency_Code = CType(Params(14), Integer)
            Me.Card_Number = Params(15)
            Me.CVV_Flag = Params(16)
            Me.CVV_Response_Code = Params(17)
            Me.Bank_Response_Code = Params(18)
            Me.Complementary_Code = Params(19)
            Me.Complementary_Info = Params(20)
            Me.Return_Context = Params(21)
            Me.Caddie = Params(22)
            Me.Receipt_Complement = Params(23)
            Me.Merchant_Language = Params(24)
            Me.Language = Params(25)
            Me.Customer_ID = CType(Params(26), Integer)
            Me.Order_ID = CType(Params(27), Integer)
            Me.Customer_Email = Params(28)
            Me.Customer_IP_Address = Params(29)
            Me.Capture_Day = Params(30)
            Me.Capture_Mode = Params(31)
            Me.Data = Params(32)
        End If
    End Sub

#End Region

#Region "Private Members"

    Private _code As Integer
    Private _error As String
    Private _merchant_id As String
    Private _merchant_country As String
    Private _amount As Decimal
    Private _transaction_id As String
    Private _payment_means As String
    Private _transmission_date As String
    Private _payment_time As String
    Private _payment_date As String
    Private _response_code As Integer
    Private _payment_certificate As String
    Private _authorisation_id As String
    Private _currency_code As Integer
    Private _card_number As String
    Private _cvv_flag As String
    Private _cvv_response_code As String
    Private _bank_response_code As String
    Private _complementary_code As String
    Private _complementary_info As String
    Private _return_context As String
    Private _caddie As String
    Private _receipt_complement As String
    Private _merchant_language As String
    Private _language As String
    Private _customer_id As Integer
    Private _order_id As Integer
    Private _customer_email As String
    Private _customer_ip_address As String
    Private _capture_day As String
    Private _capture_mode As String
    Private _data As String

#End Region

#Region "Properties"

    Public Property Code() As Integer
        Get
            Return _code
        End Get
        Set(ByVal value As Integer)
            _code = value
        End Set
    End Property

    Public Property ErrorMessage() As String
        Get
            Return _error
        End Get
        Set(ByVal value As String)
            _error = value
        End Set
    End Property

    Public Property Merchant_ID() As String
        Get
            Return _merchant_id
        End Get
        Set(ByVal value As String)
            _merchant_id = value
        End Set
    End Property

    Public Property Merchant_Country() As String
        Get
            Return _merchant_country
        End Get
        Set(ByVal value As String)
            _merchant_country = value
        End Set
    End Property

    Public Property Amount() As Decimal
        Get
            Return _amount
        End Get
        Set(ByVal value As Decimal)
            _amount = value
        End Set
    End Property

    Public Property Transaction_ID() As String
        Get
            Return _transaction_id
        End Get
        Set(ByVal value As String)
            _transaction_id = value
        End Set
    End Property

    Public Property Payment_Means() As String
        Get
            Return _payment_means
        End Get
        Set(ByVal value As String)
            _payment_means = value
        End Set
    End Property

    Public Property Transmission_Date() As String
        Get
            Return _transmission_date
        End Get
        Set(ByVal value As String)
            _transmission_date = value
        End Set
    End Property

    Public Property Payment_Time() As String
        Get
            Return _payment_time
        End Get
        Set(ByVal value As String)
            _payment_time = value
        End Set
    End Property

    Public Property Payment_Date() As String
        Get
            Return _payment_date
        End Get
        Set(ByVal value As String)
            _payment_date = value
        End Set
    End Property

    Public Property Response_Code() As Integer
        Get
            Return _response_code
        End Get
        Set(ByVal value As Integer)
            _response_code = value
        End Set
    End Property

    Public Property Payment_Certificate() As String
        Get
            Return _payment_certificate
        End Get
        Set(ByVal value As String)
            _payment_certificate = value
        End Set
    End Property

    Public Property Authorisation_ID() As String
        Get
            Return _authorisation_id
        End Get
        Set(ByVal value As String)
            _authorisation_id = value
        End Set
    End Property

    Public Property Currency_Code() As Integer
        Get
            Return _currency_code
        End Get
        Set(ByVal value As Integer)
            _currency_code = value
        End Set
    End Property

    Public Property Card_Number() As String
        Get
            Return _card_number
        End Get
        Set(ByVal value As String)
            _card_number = value
        End Set
    End Property

    Public Property CVV_Flag() As String
        Get
            Return _cvv_flag
        End Get
        Set(ByVal value As String)
            _cvv_flag = value
        End Set
    End Property

    Public Property CVV_Response_Code() As String
        Get
            Return _cvv_response_code
        End Get
        Set(ByVal value As String)
            _cvv_response_code = value
        End Set
    End Property

    Public Property Bank_Response_Code() As String
        Get
            Return _bank_response_code
        End Get
        Set(ByVal value As String)
            _bank_response_code = value
        End Set
    End Property

    Public Property Complementary_Code() As String
        Get
            Return _complementary_code
        End Get
        Set(ByVal value As String)
            _complementary_code = value
        End Set
    End Property

    Public Property Complementary_Info() As String
        Get
            Return _complementary_info
        End Get
        Set(ByVal value As String)
            _complementary_info = value
        End Set
    End Property

    Public Property Return_Context() As String
        Get
            Return _return_context
        End Get
        Set(ByVal value As String)
            _return_context = value
        End Set
    End Property

    Public Property Caddie() As String
        Get
            Return _caddie
        End Get
        Set(ByVal value As String)
            _caddie = value
        End Set
    End Property

    Public Property Receipt_Complement() As String
        Get
            Return _receipt_complement
        End Get
        Set(ByVal value As String)
            _receipt_complement = value
        End Set
    End Property

    Public Property Merchant_Language() As String
        Get
            Return _merchant_language
        End Get
        Set(ByVal value As String)
            _merchant_language = value
        End Set
    End Property

    Public Property Language() As String
        Get
            Return _language
        End Get
        Set(ByVal value As String)
            _language = value
        End Set
    End Property

    Public Property Customer_ID() As Integer
        Get
            Return _customer_id
        End Get
        Set(ByVal value As Integer)
            _customer_id = value
        End Set
    End Property

    Public Property Order_ID() As Integer
        Get
            Return _order_id
        End Get
        Set(ByVal value As Integer)
            _order_id = value
        End Set
    End Property

    Public Property Customer_Email() As String
        Get
            Return _customer_email
        End Get
        Set(ByVal value As String)
            _customer_email = value
        End Set
    End Property

    Public Property Customer_IP_Address() As String
        Get
            Return _customer_ip_address
        End Get
        Set(ByVal value As String)
            _customer_ip_address = value
        End Set
    End Property

    Public Property Capture_Day() As String
        Get
            Return _capture_day
        End Get
        Set(ByVal value As String)
            _capture_day = value
        End Set
    End Property

    Public Property Capture_Mode() As String
        Get
            Return _capture_mode
        End Get
        Set(ByVal value As String)
            _capture_mode = value
        End Set
    End Property

    Public Property Data() As String
        Get
            Return _data
        End Get
        Set(ByVal value As String)
            _data = value
        End Set
    End Property

#End Region

#Region "Object Overrides"

    Public Overrides Function ToString() As String
        Dim sbString As StringBuilder = New StringBuilder()

        sbString.Append("Code=" & Me.Code.ToString() & ", ")
        sbString.Append("ErrorMessage=" & Me.ErrorMessage & ", ")
        sbString.Append("Merchant_ID=" & Me.Merchant_ID & ", ")
        sbString.Append("Merchant_Country=" & Me.Merchant_Country & ", ")
        sbString.Append("Amount=" & Me.Amount.ToString("0.00") & ", ")
        sbString.Append("Transaction_ID=" & Me.Transaction_ID & ", ")
        sbString.Append("Payment_Means=" & Me.Payment_Means & ", ")
        sbString.Append("Transmission_Date=" & Me.Transmission_Date & ", ")
        sbString.Append("Payment_Time=" & Me.Payment_Time & ", ")
        sbString.Append("Payment_Date=" & Me.Payment_Date & ", ")
        sbString.Append("Response_Code=" & Me.Response_Code.ToString() & ", ")
        sbString.Append("Payment_Certificate=" & Me.Payment_Certificate & ", ")
        sbString.Append("Authorisation_ID=" & Me.Authorisation_ID & ", ")
        sbString.Append("Currency_Code=" & Me.Currency_Code.ToString() & ", ")
        sbString.Append("Card_Number=" & Me.Card_Number & ", ")
        sbString.Append("CVV_Flag=" & Me.CVV_Flag & ", ")
        sbString.Append("CVV_Response_Code=" & Me.CVV_Response_Code & ", ")
        sbString.Append("Bank_Response_Code=" & Me.Bank_Response_Code & ", ")
        sbString.Append("Complementary_Code=" & Me.Complementary_Code & ", ")
        sbString.Append("Complementary_Info=" & Me.Complementary_Info & ", ")
        sbString.Append("Return_Context=" & Me.Return_Context & ", ")
        sbString.Append("Caddie=" & Me.Caddie & ", ")
        sbString.Append("Receipt_Complement=" & Me.Receipt_Complement & ", ")
        sbString.Append("Merchant_Language=" & Me.Merchant_Language & ", ")
        sbString.Append("Language=" & Me.Language & ", ")
        sbString.Append("Customer_ID=" & Me.Customer_ID.ToString() & ", ")
        sbString.Append("Order_ID=" & Me.Order_ID.ToString() & ", ")
        sbString.Append("Customer_Email=" & Me.Customer_Email & ", ")
        sbString.Append("Customer_IP_Address=" & Me.Customer_IP_Address & ", ")
        sbString.Append("Capture_Day=" & Me.Capture_Day & ", ")
        sbString.Append("Capture_Mode=" & Me.Capture_Mode & ", ")
        sbString.Append("Data=" & Me.Data)

        Return sbString.ToString()
    End Function

#End Region

End Class
