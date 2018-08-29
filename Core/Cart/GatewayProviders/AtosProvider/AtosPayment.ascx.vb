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

Imports System.IO

Imports DotNetNuke.Entities.Portals

Imports DotNetNuke.Modules.Store.Core.Customer

Namespace DotNetNuke.Modules.Store.Core.Cart

    Public Class AtosPayment
        Inherits PaymentControlBase

#Region "Controls"

        Protected WithEvents lblError As Label
        Protected WithEvents pnlProceedToAtos As System.Web.UI.WebControls.Panel
        Protected WithEvents lblConfirmMessage As Label
        Protected WithEvents imgBankLogo As System.Web.UI.WebControls.Image
        Protected WithEvents btnConfirmOrder As System.Web.UI.WebControls.Button

#End Region

#Region " Code généré par le Concepteur Web Form "

        'Cet appel est requis par le Concepteur Web Form.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        'REMARQUE : la déclaration d'espace réservé suivante est requise par le Concepteur Web Form.
        'Ne pas supprimer ou déplacer.
        Private designerPlaceholderDeclaration As Object

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN : cet appel de méthode est requis par le Concepteur Web Form
            'Ne le modifiez pas en utilisant l'éditeur de code.
            InitializeComponent()
        End Sub

#End Region

#Region "Private Members"

        Private Shared CookieName As String = "DotNetNuke_Store_Portal_"
        Private _settings As AtosSettings = Nothing
        Private _Message As String = String.Empty
        Private _order As OrderInfo

        Private Enum PaymentStatus
            Payed
            Pending
            Rejected
            Canceled
            Unattended
            Invalid
        End Enum

#End Region

#Region "Private Properties"

        Private ReadOnly Property BasePage() As CDefault
            Get
                Return CType(Page, CDefault)
            End Get
        End Property

        Private ReadOnly Property CookieKey() As String
            Get
                Return CookieName + PortalId.ToString()
            End Get
        End Property

#End Region

#Region "Event Handlers"

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            _settings = New AtosSettings(CheckoutControl.StoreSettings.GatewaySettings)

            ' Do we have any special handling?
            Dim nav As New AtosNavigation(Request.QueryString)

            Select Case nav.GatewayExit.ToUpper()
                Case "CANCEL"
                    InvokePaymentCancelled()
                    CheckoutControl.Hide()
                    pnlProceedToAtos.Visible = False
                    Return
                Case "RETURN"
                    InvokePaymentRequiresConfirmation()
                    CheckoutControl.Hide()
                    pnlProceedToAtos.Visible = False
                    Return
            End Select

            ' If the GatewayExit is anything else with length > 0,
            ' then don't do any processing
            If nav.GatewayExit.Length > 0 Then
                HttpContext.Current.Response.Redirect(Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID), False)
            End If

            ' Continue with display of payment control...
            If _settings Is Nothing OrElse Not _settings.IsValid Then
                lblError.Text = Services.Localization.Localization.GetString("GatewayNotConfigured", Me.LocalResourceFile)
                lblError.Visible = True
                pnlProceedToAtos.Visible = False
            Else
                btnConfirmOrder.Attributes.Add("OnClick", ScriptAvoidDoubleClick(btnConfirmOrder, Localization.GetString("Processing", Me.LocalResourceFile)))
                _Message = Localization.GetString("lblConfirmMessage", Me.LocalResourceFile)
                lblConfirmMessage.Text = String.Format(_Message, PortalSettings.PortalName)

                Dim strBankLogo As String = _settings.BankLogo
                If String.IsNullOrEmpty(strBankLogo) = False Then
                    strBankLogo = Path.Combine(_settings.PathFileDirectory, strBankLogo).Replace("\", "/")
                    _Message = Localization.GetString("imgBankLogo", Me.LocalResourceFile)
                    imgBankLogo.AlternateText = _Message
                    imgBankLogo.ImageUrl = Common.Globals.ResolveUrl(Me.PortalSettings.HomeDirectory & strBankLogo)
                Else
                    imgBankLogo.Visible = False
                End If

                lblError.Text = String.Empty
                lblError.Visible = False
            End If
        End Sub

        Protected Sub btnConfirmOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirmOrder.Click
            ConfirmOrder()
        End Sub

#End Region

#Region "Private Methods"

        Private Sub ClearOrderIdCookie()
            Dim cookie As HttpCookie = HttpContext.Current.Request.Cookies(CookieKey)
            If Not cookie Is Nothing Then
                cookie.Expires = DateTime.Today.AddDays(-100)
                HttpContext.Current.Response.Cookies.Add(cookie)
            End If
        End Sub

        Private Sub ConfirmOrder()
            Page.Validate()
            If Not Page.IsValid Then
                Return
            End If

            Try
                ' Adds order to db...
                Dim objOrder As OrderInfo = CheckoutControl.GetFinalizedOrderInfo()
                GenerateOrderConfirmation()
                ' Set order status to "Awaiting Payment"...
                CheckoutControl.Order = UpdateOrderStatus(objOrder, OrderInfo.OrderStatusList.AwaitingPayment)
                ' Clear basket
                CurrentCart.DeleteCart(PortalId, CheckoutControl.StoreSettings.SecureCookie)
                ' Clear cookies
                ClearOrderIdCookie()
                ' Create transaction and inject generated card controls
                Dim litSIPSForm As New Literal With {
                    .Text = CreateTransaction(objOrder)
                }
                pnlProceedToAtos.Controls.Clear()
                pnlProceedToAtos.Controls.Add(litSIPSForm)
            Catch ex As Exception
                lblError.Text = ex.Message
                Me.lblError.Visible = True
            End Try
        End Sub

        Private Function CreateTransaction(ByVal Order As OrderInfo) As String

            Dim urlAuthority As String = Request.Url.GetLeftPart(UriPartial.Authority)
            Dim nav As AtosNavigation = New AtosNavigation()
            Dim strURL As String

            nav.OrderID = Order.OrderID
            nav.GatewayExit = "return"
            strURL = nav.GetNavigationUrl()
            If strURL.StartsWith(urlAuthority) = False Then
                strURL = urlAuthority & strURL
            End If
            _settings.Return_Url = strURL
            nav.GatewayExit = "cancel"
            strURL = nav.GetNavigationUrl()
            If strURL.StartsWith(urlAuthority) = False Then
                strURL = urlAuthority & strURL
            End If
            _settings.Cancel_Url = strURL

            Dim strLanguage As String = Request.QueryString("language")

            If String.IsNullOrEmpty(strLanguage) Then
                strLanguage = System.Threading.Thread.CurrentThread.CurrentCulture.ToString()
            End If
            _settings.Auto_Response_Url = urlAuthority & TemplateSourceDirectory & "/AtosIPN.aspx?language=" & strLanguage

            Dim Amout As Decimal = Order.GrandTotal * 100

            _settings.Amount = Amout.ToString("0")
            _settings.Order_Id = Order.OrderID.ToString
            _settings.Transaction_Id = Order.OrderID.ToString

            Dim strResult As String = ExecuteSIPS(Server.MapPath("~\bin\request.exe"), _settings.ReturnCommand & " pathfile=" & ReturnPathFile(_settings.PathFileDirectory))
            Dim arResult As Array = Split(strResult, "!")
            Dim code As Integer = CType(arResult(1), Integer)

            If code <> 0 Then
                Return "<p>" & arResult(2) & "</p>"
            Else
                Dim strHTML As String = ""
                ' Match form tag parts
                Dim reCleanForm As New Regex("^<form.*?action=""(.*?)"".*?>(.*?)</form>$", RegexOptions.IgnoreCase)
                Dim matchGroups As Match = reCleanForm.Match(arResult(3))
                ' Get Post URL
                Dim strPostURL As String = matchGroups.Groups(1).Value
                ' Insert Post URL in script
                Dim jsPostToSIPS As String = "<script type=""text/javascript"">function PostToSIPS() {theForm.encoding='application/x-www-form-urlencoded';theForm.action='" & strPostURL & "';return true;}</script>"
                ' Get remaining code between form tag
                Dim strCode As String = matchGroups.Groups(2).Value
                ' Insert JS Call OnClick
                strCode = strCode.Replace("<INPUT TYPE=IMAGE", "<input type=""image"" onclick=""javascript:PostToSIPS();""")
                ' XHTML Compliancy
                strCode = strCode.Replace("BORDER=0", "")
                strCode = Regex.Replace(strCode, "([A-Za-z]+) *= *([A-Za-z0-9]+)", "$1=""$2""")
                strCode = strCode.Replace("<br>", "<br />")
                strCode = strCode.Replace("<INPUT TYPE=""HIDDEN""", "<input type=""hidden""")
                strCode = strCode.Replace("NAME=", "name=")
                strCode = strCode.Replace("<DIV ALIGN=""center"">", "<div>")
                strCode = strCode.Replace("</DIV>", "</div>")
                strCode = strCode.Replace("<IMG", "<img")
                strCode = strCode.Replace("SRC=", "src=")
                strCode = strCode.Replace("VALUE=", "value=")
                strCode = Regex.Replace(strCode, "(<(INPUT|IMG).+?)>", "$1 />", RegexOptions.IgnoreCase)
                ' Return JS and XHTML Code
                strHTML = jsPostToSIPS & vbCrLf & strCode
                Return strHTML
            End If

        End Function

        Private Function ReturnPathFile(ByVal PathFileDirectory As String) As String
            Dim objPortal As New PortalController
            Dim nfoPortal As PortalInfo = objPortal.GetPortal(PortalId)

            Return Path.Combine(nfoPortal.HomeDirectoryMapPath, PathFileDirectory & "Pathfile").Replace("/", "\")
        End Function

        Private Function ExecuteSIPS(ByVal PathToExe As String, ByVal Parameters As String) As String
            Dim strReturn As String = Nothing
            Dim psiSIPSExe As New ProcessStartInfo(PathToExe, Parameters)

            With psiSIPSExe
                .RedirectStandardOutput = True
                .ErrorDialog = False
                .UseShellExecute = False
                .CreateNoWindow = True
                .WindowStyle = ProcessWindowStyle.Hidden
            End With

            Try
                Dim procExe As Process = Process.Start(psiSIPSExe)
                Dim readerOutput As StreamReader = procExe.StandardOutput()
                strReturn = readerOutput.ReadToEnd()
                readerOutput.Close()
            Catch ex As Exception
                Throw New Exception(Localization.GetString("CannotAccessExe", Me.LocalResourceFile))
            End Try

            Return strReturn
        End Function

#End Region

    End Class

End Namespace