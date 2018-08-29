<%@ Control language="c#" CodeBehind="AuthorizeNetAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.Core.Cart.AuthorizeNetAdmin" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnFormItem">
	<dnn:Label id="lblGateway" runat="server" controlname="txtGateway"></dnn:Label>
	<asp:TextBox id="txtGateway" runat="server" MaxLength="255"></asp:TextBox>
</div>
<div class="dnnFormItem">
	<dnn:Label id="lblVersion" runat="server" controlname="txtVersion"></dnn:Label>
	<asp:TextBox id="txtVersion" runat="server" Width="60px" MaxLength="10"></asp:TextBox>
</div>
<div class="dnnFormItem">
	<dnn:Label id="lblUsername" runat="server" controlname="txtUsername"></dnn:Label>
	<asp:TextBox id="txtUsername" runat="server" MaxLength="50"></asp:TextBox>
</div>
<div class="dnnFormItem">
	<dnn:Label id="lblPassword" runat="server" controlname="txtPassword"></dnn:Label>
	<asp:TextBox id="txtPassword" runat="server" MaxLength="50"></asp:TextBox>
</div>
<div class="dnnFormItem">
	<dnn:Label id="lblCaptureType" runat="server" controlname="ddlCapture"></dnn:Label>
	<asp:DropDownList id="ddlCapture" runat="server">
		<asp:ListItem ResourceKey="ddlCaptureAC" Value="AUTH_CAPTURE" Selected="True">Auth and Capture</asp:ListItem>
		<asp:ListItem ResourceKey="ddlCaptureAO" Value="AUTH_ONLY">Auth Only</asp:ListItem>
	</asp:DropDownList>
</div>
<div class="dnnFormItem">
	<dnn:Label id="lblTestMode" runat="server" controlname="cbTest"></dnn:Label>
	<asp:CheckBox id="cbTest" runat="server"></asp:CheckBox>
</div>
