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

Imports System
Imports System.Reflection

Namespace DotNetNuke.Modules.Store.Cart

    Public Class AtosSettings
        Inherits GatewaySettings

#Region "Constructors"

        Public Sub New()
        End Sub

        Public Sub New(ByVal xml As String)
            FromString(xml)
        End Sub

#End Region

#Region "Private Members"

        '--- Info sur le paramétrage de ATOS
        Private _Auto_Response_Url As String
        Private _Cancel_Url As String
        'Private _Currency_Code As String
        'Private _Header_Flag As String
        'Private _Language As String
        Private _Merchant_Id As String
        'Private _Merchant_Country As String
        Private _Return_Url As String
        'Private _Payment_Means As String
        Private _PathFileDirectory As String
        Private _BankLogo As String

        '--- Info sur la transaction en cours
        Private _Amount As String
        Private _Transaction_Id As String
        Private _Order_Id As String

        '--- Info sur la transaction en cours Facultatives. Sera implemente dans un deuxième temps
        'Private _Caddie As String
        Private _Customer_Email As String
        Private _Customer_Id As String
        Private _Customer_Ip_Address As String
        'Private _Return_Context As String

        '--- Info sur le paramétrage de ATOS Facultatives. Sera implemente dans un deuxième temps
        'Private _Advert As String
        'Private _Block_Align As String
        'Private _Block_Order As String
        'Private _BackGround_Id As String
        'Private _BgColor As String
        'Private _Caddie As String
        'Private _CaptureDay As String
        'Private _Capture_Mode As String
        'Private _Cancel_Logo As String
        'Private _Data As String
        'Private _Logo_Id As String
        'Private _Logo_Id2 As String
        'Private _Return_Logo As String
        'Private _Order_Id As String
        'Private _Receipt As String
        'Private _Return_Context As String
        'Private _Submit_Logo As String
        'Private _Target As String
        'Private _Template As String
        'Private _TextColor As String

#End Region

#Region "Properties"

        '--- Info sur le paramétrage de ATOS
        <Sips("automatic_response_url", False)> _
        Public Property Auto_Response_Url() As String
            Get
                Return _Auto_Response_Url
            End Get
            Set(ByVal Value As String)
                _Auto_Response_Url = Value
            End Set
        End Property

        <Sips("cancel_return_url", False)> _
        Public Property Cancel_Url() As String
            Get
                Return _Cancel_Url
            End Get
            Set(ByVal Value As String)
                _Cancel_Url = Value
            End Set
        End Property

        '<Sips("currency_code", False)> _
        '        Public Property Currency_Code() As String
        '    Get
        '        Return _Currency_Code
        '    End Get
        '    Set(ByVal Value As String)
        '        _Currency_Code = Value
        '    End Set
        'End Property

        '<Sips("header_flag", False)> _
        '        Public Property Header_Flag() As String
        '    Get
        '        Return _Header_Flag
        '    End Get
        '    Set(ByVal Value As String)
        '        _Header_Flag = Value
        '    End Set
        'End Property

        '<Sips("language", False)> _
        '        Public Property Language() As String
        '    Get
        '        Return _Language
        '    End Get
        '    Set(ByVal Value As String)
        '        _Language = Value
        '    End Set
        'End Property

        <Sips("merchant_id", False)> _
        Public Property Merchant_Id() As String
            Get
                Return _Merchant_Id
            End Get
            Set(ByVal Value As String)
                _Merchant_Id = Value
            End Set
        End Property

        '<Sips("merchant_country", False)> _
        '        Public Property Merchant_Country() As String
        '    Get
        '        Return _Merchant_Country
        '    End Get
        '    Set(ByVal Value As String)
        '        _Merchant_Country = Value
        '    End Set
        'End Property

        <Sips("normal_return_url", True)> _
        Public Property Return_Url() As String
            Get
                Return _Return_Url
            End Get
            Set(ByVal Value As String)
                _Return_Url = Value
            End Set
        End Property

        '<Sips("payment_means", True)> _
        '        Public Property Payment_Means() As String
        '    Get
        '        Return _Payment_Means
        '    End Get
        '    Set(ByVal Value As String)
        '        _Payment_Means = Value
        '    End Set
        'End Property

        <Sips("", False)> _
        Public Property PathFileDirectory() As String
            Get
                Return _PathFileDirectory
            End Get
            Set(ByVal Value As String)
                _PathFileDirectory = Value
            End Set
        End Property

        <Sips("", False)> _
        Public Property BankLogo() As String
            Get
                Return _BankLogo
            End Get
            Set(ByVal Value As String)
                _BankLogo = Value
            End Set
        End Property

        '--- Info sur la transaction en cours
        <Sips("amount", False)> _
        Public Property Amount() As String
            Get
                Return _Amount
            End Get
            Set(ByVal Value As String)
                _Amount = Value
            End Set
        End Property

        <Sips("transaction_id", False)> _
        Public Property Transaction_Id() As String
            Get
                Return _Transaction_Id
            End Get
            Set(ByVal Value As String)
                _Transaction_Id = Value
            End Set
        End Property

        <Sips("order_id", False)> _
        Public Property Order_Id() As String
            Get
                Return _Order_Id
            End Get
            Set(ByVal Value As String)
                _Order_Id = Value
            End Set
        End Property

        '--- Info facultative sur la transaction en cours
        '<Sips("caddie", False)> _
        'Public Property Caddie() As String
        '    Get
        '        Return _Caddie
        '    End Get
        '    Set(ByVal Value As String)
        '        _Caddie = Value
        '    End Set
        'End Property

        <Sips("customer_email", False)> _
        Public Property Customer_Email() As String
            Get
                Return _Customer_Email
            End Get
            Set(ByVal Value As String)
                _Customer_Email = Value
            End Set
        End Property

        <Sips("customer_id", False)> _
        Public Property Customer_Id() As String
            Get
                Return _Customer_Id
            End Get
            Set(ByVal Value As String)
                _Customer_Id = Value
            End Set
        End Property

        <Sips("customer_ip_address", False)> _
        Public Property Customer_Ip_Address() As String
            Get
                Return _Customer_Ip_Address
            End Get
            Set(ByVal Value As String)
                _Customer_Ip_Address = Value
            End Set
        End Property

        '<Sips("return_context", False)> _
        'Public Property Return_Context() As String
        '    Get
        '        Return _Return_Context
        '    End Get
        '    Set(ByVal Value As String)
        '        _Return_Context = Value
        '    End Set
        'End Property

#End Region

#Region "Public Methods"

        Public Function ReturnCommand() As String

            Dim strReturn As String = ""
            Dim CustAttr As SipsAttribute
            Dim strValue As String

            For Each nfoProperty As PropertyInfo In Me.GetType.GetProperties()
                CustAttr = CType(nfoProperty.GetCustomAttributes(GetType(SipsAttribute), False)(0), SipsAttribute)
                If CustAttr.KeyWord <> "" Then

                    strValue = nfoProperty.GetValue(Me, Nothing)
                    If Not String.IsNullOrEmpty(strValue) Then
                        strReturn += " " & CustAttr.KeyWord.ToLower() & "=" & strValue
                    Else
                        If CustAttr.Required Then
                            strReturn = "The field " & CustAttr.KeyWord & " is required!"
                            Exit For
                        End If
                    End If

                End If
            Next
            Return strReturn

        End Function

        'Public Function CompleteRequete(ByVal nfoReponse As Reponse) As Boolean
        '    Try
        '        Amount = Val(nfoReponse.Amount.Replace(",", ".")) * 100
        '        Caddie = nfoReponse.Caddie
        '        Customer_Email = nfoReponse.Customer_Email
        '        Customer_Id = nfoReponse.Customer_Id
        '        Customer_Ip_Address = nfoReponse.Customer_Ip_Adress
        '        Order_Id = nfoReponse.Operation_Id
        '        Return_Context = nfoReponse.Return_Context
        '        Transaction_Id = nfoReponse.Transaction_Id
        '        Return True
        '    Catch ex As Exception
        '        Return False
        '    End Try
        'End Function

#End Region

#Region "GatewaySettings Overrides"

        <Sips("", False)> _
        Public Shadows ReadOnly Property IsValid() As Boolean
            Get
                Return ((Merchant_Id <> String.Empty) AndAlso (PathFileDirectory <> String.Empty))

                'Return ((Auto_Response_Url <> String.Empty) AndAlso (Cancel_Url <> String.Empty) AndAlso _
                '     (Currency_Code <> String.Empty) AndAlso (Language <> String.Empty) AndAlso _
                '     (Merchant_Id <> String.Empty) AndAlso (Merchant_Country <> String.Empty) AndAlso _
                '     (Payment_Means <> String.Empty) AndAlso (PathFileDirectory <> String.Empty))

            End Get
        End Property

#End Region

    End Class

#Region "Class Attributes"

    <AttributeUsage(AttributeTargets.Property)> Class SipsAttribute
        Inherits Attribute

        Private _KeyWord As String
        Private _Required As Boolean


        Public Property Required() As Boolean
            Get
                Return _Required
            End Get
            Set(ByVal Value As Boolean)
                _Required = Value
            End Set
        End Property

        Public Property KeyWord() As String
            Get
                Return _KeyWord
            End Get
            Set(ByVal Value As String)
                _KeyWord = Value
            End Set
        End Property

        Sub New(ByVal pKeyword As String, ByVal pRequired As Boolean)
            _KeyWord = pKeyword
            _Required = pRequired
        End Sub

    End Class

#End Region

End Namespace