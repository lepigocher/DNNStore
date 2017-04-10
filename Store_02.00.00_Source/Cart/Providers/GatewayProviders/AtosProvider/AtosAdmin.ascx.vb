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

Imports System.IO
Imports System.Collections
Imports System.Web.UI.WebControls

Imports DotNetNuke.Modules.Store.Components
Imports DotNetNuke.Services.FileSystem


Namespace DotNetNuke.Modules.Store.Cart

    Partial Public Class AtosAdmin
        Inherits StoreControlBase

#Region "Controls"

        Protected WithEvents txtMerchantIdValue As System.Web.UI.WebControls.TextBox
        Protected WithEvents cbofolders As System.Web.UI.WebControls.DropDownList
        Protected WithEvents cboFolderFiles As System.Web.UI.WebControls.DropDownList
        Protected WithEvents cmdLoadPathFile As System.Web.UI.WebControls.LinkButton
        Protected WithEvents lblPathFileTitle As System.Web.UI.WebControls.Label
        Protected WithEvents lblPathFileValue As System.Web.UI.WebControls.Label
        Protected WithEvents txtPathFile As System.Web.UI.WebControls.TextBox
        Protected WithEvents cmdPathfile As System.Web.UI.WebControls.LinkButton

#End Region

#Region " Code généré par le Concepteur Web Form "

        'Cet appel est requis par le Concepteur Web Form.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()

        End Sub

        'REMARQUE : la déclaration d'espace réservé suivante est requise par le Concepteur Web Form.
        'Ne pas supprimer ou déplacer.
        Private designerPlaceholderDeclaration As System.Object

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN : cet appel de méthode est requis par le Concepteur Web Form
            'Ne le modifiez pas en utilisant l'éditeur de code.
            InitializeComponent()
        End Sub

#End Region

#Region "Events Handlers"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            BindFolders()
            If MyBase.DataSource IsNot Nothing Then
                Dim gatewaySettings As String = MyBase.DataSource
                If String.IsNullOrEmpty(gatewaySettings) = False Then
                    Dim settings As New AtosSettings(gatewaySettings)

                    txtMerchantIdValue.Text = settings.Merchant_Id
                    If String.IsNullOrEmpty(settings.PathFileDirectory) = False Then
                        cbofolders.SelectedValue = settings.PathFileDirectory
                        LoadPathFile(settings.PathFileDirectory)
                        BindFolderFiles()
                        If String.IsNullOrEmpty(settings.BankLogo) = False Then
                            cboFolderFiles.SelectedValue = settings.BankLogo
                        End If
                    End If
                End If
            End If

        End Sub

        Protected Sub cbofolders_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbofolders.SelectedIndexChanged

            BindFolderFiles()

        End Sub

#End Region

#Region "StoreControlBase Overrides"

        Public Overrides Property DataSource() As Object
            Get

                Dim settings As New AtosSettings
                settings.Merchant_Id = txtMerchantIdValue.Text
                settings.PathFileDirectory = cbofolders.SelectedItem.Value
                settings.BankLogo = cboFolderFiles.SelectedItem.Value
                MyBase.DataSource = settings.ToString
                Return MyBase.DataSource

            End Get
            Set(ByVal Value As Object)

                MyBase.DataSource = Value

            End Set
        End Property

#End Region

#Region "Private Methods"

        Private Sub BindFolders()

            Dim objFolders As New FolderController
            Dim nfoFolder As FolderInfo

            For Each nfoFolder In objFolders.GetFolders(Me.PortalId).Values
                Dim FolderItem As New ListItem
                FolderItem.Text = nfoFolder.FolderPath.Replace("/", "\")
                FolderItem.Value = FolderItem.Text
                cbofolders.Items.Add(FolderItem)
            Next

        End Sub

        Private Sub BindFolderFiles()

            Dim objFiles As New FileController
            Dim objFileInfo As DotNetNuke.Services.FileSystem.FileInfo
            Dim arrFolders As ArrayList = objFiles.GetFilesByFolder(Me.PortalId, cbofolders.SelectedValue.Replace("\", "/"))

            cboFolderFiles.Items.Clear()
            cboFolderFiles.Items.Add(New ListItem(Localization.GetString("None", Me.LocalResourceFile), ""))
            If arrFolders IsNot Nothing Then
                For Each objFileInfo In arrFolders
                    cboFolderFiles.Items.Add(objFileInfo.FileName)
                Next
            End If

        End Sub

        Private Sub LoadPathFile(ByVal PathFileDirectory As String)

            Dim strPathFileDirectory As String = ""
            strPathFileDirectory = Path.Combine(PortalSettings.HomeDirectoryMapPath, PathFileDirectory).Replace("/", "\")

            ' Check directory
            If System.IO.Directory.Exists(strPathFileDirectory) = True Then
                Dim strFilePath As String = Path.Combine(strPathFileDirectory, "pathfile")
                ' read file
                If System.IO.File.Exists(strFilePath) = True Then
                    Dim objStreamReader As StreamReader
                    objStreamReader = File.OpenText(strFilePath)
                    txtPathFile.Text = objStreamReader.ReadToEnd
                    objStreamReader.Close()
                    objStreamReader.Dispose()
                    BindServerMapPath(PathFileDirectory)
                Else
                    txtPathFile.Text = Localization.GetString("MissingFile", Me.LocalResourceFile)
                End If
            Else
                txtPathFile.Text = Localization.GetString("MissingFolder", Me.LocalResourceFile)
            End If

        End Sub

        Private Sub BindServerMapPath(ByVal PathFileDirectory As String)

            lblPathFileValue.Text = "<br>MapPath : " & Path.Combine(PortalSettings.HomeDirectoryMapPath, PathFileDirectory)
            lblPathFileValue.Text += "<br>PortalAlias : " & PortalSettings.PortalAlias.HTTPAlias

        End Sub

        Private Sub cmdPathfile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPathfile.Click

            If cbofolders.Items.Count > 0 AndAlso cbofolders.SelectedIndex <> -1 Then

                Dim strPathFileDirectory As String = ""
                strPathFileDirectory = Path.Combine(PortalSettings.HomeDirectoryMapPath, cbofolders.SelectedItem.Value).Replace("/", "\")

                If File.Exists(strPathFileDirectory & "pathfile") Then
                    File.SetAttributes(strPathFileDirectory & "pathfile", FileAttributes.Normal)
                End If

                ' write file
                Dim objStream As StreamWriter
                objStream = File.CreateText(strPathFileDirectory & "pathfile")
                objStream.WriteLine(txtPathFile.Text)
                objStream.Close()
                objStream.Dispose()
            End If

        End Sub

        Private Sub cmdLoadPathFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLoadPathFile.Click

            If cbofolders.Items.Count > 0 AndAlso cbofolders.SelectedIndex <> -1 Then
                LoadPathFile(cbofolders.SelectedItem.Value)
            End If

        End Sub

#End Region

    End Class

End Namespace