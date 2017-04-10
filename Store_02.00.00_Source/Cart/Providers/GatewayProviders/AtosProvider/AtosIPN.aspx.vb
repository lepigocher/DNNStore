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

Imports System.Diagnostics
Imports System.IO
Imports DotNetNuke.Modules.Store.Admin
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Services.Log.EventLog
Imports DotNetNuke.Services.Mail
Imports DotNetNuke.Modules.Store.Customer

Namespace DotNetNuke.Modules.Store.Cart
    Partial Public Class AtosIPN
        Inherits IPNPageBase

#Region "Private Members"

        Private _settings As AtosSettings = Nothing
        Private portalLanguage As String
        Private userLanguage As String

        Private Enum PaymentStatus
            Payed
            Pending
            Rejected
            Canceled
            Unattended
            Invalid
        End Enum

#End Region

#Region "Event Handlers"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            portalLanguage = PortalSettings.DefaultLanguage
            Dim fields As NameValueCollection = Request.Form
            Dim strMessage As String = Request.Form("DATA")

            If Not String.IsNullOrEmpty(strMessage) Then
                _settings = New AtosSettings(StoreSettings.GatewaySettings)
                Dim objAtosIPN As AtosIPNParameters = GetIPNParameters(strMessage)

                If objAtosIPN IsNot Nothing Then
                    Dim subject As String = ""
                    Dim sendEmail As Boolean = False

                    Select Case VerifyPayment(objAtosIPN)
                        Case PaymentStatus.Payed
                            Dim portalId As Integer = PortalSettings.PortalId
                            ' Set order status to "Paid"...
                            Dim order As OrderInfo = UpdateOrderStatus(objAtosIPN.Order_ID, OrderInfo.OrderStatusList.Paid, userLanguage)
                            ' Add User to Product Roles
                            Dim orderController As New OrderController()
                            orderController.AddUserToRoles(PortalSettings.PortalId, order)
                            ' Add User to Order Role
                            Dim storeSetting As StoreInfo = StoreController.GetStoreInfo(PortalSettings.PortalId)
                            If storeSetting.OnOrderPaidRoleID <> Null.NullInteger Then
                                orderController.AddUserToPaidOrderRole(portalId, order.CustomerID, storeSetting.OnOrderPaidRoleID)
                            End If
                        Case PaymentStatus.Pending
                            ' Inform Store Admin
                            subject = Localization.GetString("StoreAtosGateway", LocalResourceFile) + Localization.GetString("IPNInfo", LocalResourceFile)
                            sendEmail = True
                        Case PaymentStatus.Unattended
                            ' Alert Store Admin
                            subject = Localization.GetString("StoreAtosGateway", LocalResourceFile) + Localization.GetString("IPNAlert", LocalResourceFile)
                            sendEmail = True
                    End Select

                    If sendEmail = True Then
                        Dim emailIPN = Localization.GetString("EmailIPN", LocalResourceFile)
                        Dim body As String = String.Format(emailIPN, objAtosIPN.Order_ID, objAtosIPN.Bank_Response_Code, objAtosIPN.Complementary_Code, objAtosIPN.Complementary_Info)
                        SendEmailToAdmin(subject, body)
                    End If
                End If
            End If

        End Sub

#End Region

#Region "Private Methods"

        Private Function GetIPNParameters(ByVal Message As String) As AtosIPNParameters
            Dim objAtosParams As AtosIPNParameters = Nothing

            If Not String.IsNullOrEmpty(Message) Then
                Dim strMessage = "message=" & Message
                Dim strPathfile As String = "pathfile=" & ReturnPathFile(_settings.PathFileDirectory)
                Dim strResult As String = ExecuteSIPS(Server.MapPath("~\bin\response.exe"), strPathfile & " " & strMessage)
                Dim params As String() = Split(strResult, "!")
                objAtosParams = New AtosIPNParameters(params)
            End If

            Return objAtosParams
        End Function

        Private Function ReturnPathFile(ByVal PathFileDirectory As String) As String
            Dim objPortal As New PortalController
            Dim nfoPortal As PortalInfo = objPortal.GetPortal(PortalSettings.PortalId)

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
                Throw New Exception(Localization.GetString("CannotAccessExe", LocalResourceFile))
            End Try

            Return strReturn
        End Function

        Private Function VerifyPayment(ByVal AtosIPN As AtosIPNParameters) As PaymentStatus
            Dim status As PaymentStatus = PaymentStatus.Invalid
            ' Default Alert Reason
            Dim alertReason As String = Localization.GetString("InvalidIPN", LocalResourceFile)

            If AtosIPN.Code = 0 Then
                Select Case AtosIPN.Response_Code
                    Case 0
                        status = PaymentStatus.Payed
                    Case 2, 5, 17, 34, 75
                        status = PaymentStatus.Pending
                    Case Else
                        status = PaymentStatus.Unattended
                End Select
            End If

            ' If the transaction is invalid
            If status = PaymentStatus.Invalid OrElse status = PaymentStatus.Unattended Then
                ' Add an Admin Alert to the DNN Log
                Dim atosGateway As String = Localization.GetString("StoreAtosGateway", LocalResourceFile)
                Dim adminAlert As String = Localization.GetString("SecurityAlert", LocalResourceFile)
                Dim properties As LogProperties = New LogProperties()
                properties.Add(New LogDetailInfo(atosGateway, adminAlert))
                properties.Add(New LogDetailInfo(Localization.GetString("AlertReason", LocalResourceFile), alertReason))
                properties.Add(New LogDetailInfo(Localization.GetString("FromIP", LocalResourceFile), Request.UserHostAddress))
                properties.Add(New LogDetailInfo(Localization.GetString("IPNPOSTString", LocalResourceFile), AtosIPN.ToString()))
                AddEventLog(EventLogController.EventLogType.ADMIN_ALERT.ToString(), properties, True)
                ' Send an email to the store admin
                SendEmailToAdmin(atosGateway + " " + adminAlert, Localization.GetString("EmailAlert", LocalResourceFile))
            End If

            Return status
        End Function

        Private Sub SendEmailToAdmin(ByVal subject As String, ByVal body As String)
            Dim storeEmail As String = StoreSettings.DefaultEmailAddress
            Dim storeLink As String = vbCrLf & vbCrLf & Request.Url.GetLeftPart(UriPartial.Authority)
            Mail.SendMail(storeEmail, storeEmail, "", subject, body & storeLink, "", "text", "", "", "", "")
        End Sub

#End Region

    End Class
End Namespace
