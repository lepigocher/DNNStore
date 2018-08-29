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

Imports DotNetNuke.Modules.Store.Core.Components

Namespace DotNetNuke.Modules.Store.Core.Cart

    Public Class AtosNavigation
        Inherits NavigateWrapper

#Region "Private Members"

        Private _gatewayExit As String
        Private _orderID As Integer

#End Region

#Region "Constructors"

        Sub New()
            MyBase.New()
            If _gatewayExit Is Nothing Then _gatewayExit = String.Empty
        End Sub

        Public Sub New(ByVal queryString As NameValueCollection)
            MyBase.New(queryString)
            If _gatewayExit Is Nothing Then _gatewayExit = String.Empty
        End Sub

#End Region

#Region "Properties"

        Public Property GatewayExit() As String
            Get
                Return _gatewayExit
            End Get
            Set(ByVal value As String)
                _gatewayExit = value
            End Set
        End Property

        Public Property OrderID() As Integer
            Get
                Return _orderID
            End Get
            Set(ByVal value As Integer)
                _orderID = value
            End Set
        End Property

#End Region

    End Class

End Namespace