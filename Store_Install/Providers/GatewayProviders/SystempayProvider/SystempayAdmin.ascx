<%@ Control language="c#" CodeBehind="SystempayAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.Core.Cart.SystempayAdmin" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnFormItem">
	<dnn:Label id="lblSiteID" runat="server" controlname="txtSiteID"></dnn:Label>
	<asp:textbox id="txtSiteID" runat="server" MaxLength="50"></asp:textbox>
</div>
<div class="dnnFormItem">
	<dnn:Label id="lblContracts" runat="server" controlname="txtContracts"></dnn:Label>
	<asp:textbox id="txtContracts" runat="server"></asp:textbox>
</div>
<div class="dnnFormItem">
	<dnn:Label id="lblUseTestCertificate" runat="server" controlname="chkUseTestCertificate"></dnn:Label>
	<asp:checkbox id="chkUseTestCertificate" runat="server" Checked="false"></asp:checkbox>
</div>
<div class="dnnFormItem">
	<dnn:Label id="lblCertificate" runat="server" controlname="txtCertificate"></dnn:Label>
	<asp:textbox id="txtCertificate" runat="server" MaxLength="50"></asp:textbox>
</div>
<div class="dnnFormItem">
	<dnn:Label id="lblSystempayPaymentURL" runat="server" controlname="txtSystempayPaymentURL"></dnn:Label>
	<asp:textbox id="txtSystempayPaymentURL" runat="server" MaxLength="255"></asp:textbox>
</div>
<div class="dnnFormItem">
	<dnn:Label id="lblSystempayLanguage" runat="server" controlname="txtSystempayLanguage"></dnn:Label>
	<asp:textbox id="txtSystempayLanguage" runat="server" Width="30px" MaxLength="2">fr</asp:textbox>
</div>
<div class="dnnFormItem">
	<dnn:Label id="lblCurrency" runat="server" controlname="txtSystempayCurrency"></dnn:Label>
	<asp:textbox id="txtSystempayCurrency" runat="server" Width="50px" MaxLength="3">978</asp:textbox>
</div>
<div class="dnnFormItem">
	<dnn:Label id="lblButtonURL" runat="server" controlname="txtButtonURL"></dnn:Label>
	<asp:textbox id="txtButtonURL" runat="server"></asp:textbox>
</div>
