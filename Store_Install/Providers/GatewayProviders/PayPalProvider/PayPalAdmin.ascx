<%@ Control language="c#" CodeBehind="PayPalAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.Core.Cart.PayPalAdmin" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnFormItem">
    <dnn:label id="lblUseSandbox" runat="server" controlname="chkUseSandbox"></dnn:label>
    <asp:checkbox id="chkUseSandbox" runat="server" Checked="false"></asp:checkbox>
</div>
<div class="dnnFormItem">
	<dnn:label id="lblPayPalID" runat="server" controlname="txtPayPalID"></dnn:label>
	<asp:textbox id="txtPayPalID" runat="server" MaxLength="50"></asp:textbox>
    <asp:RequiredFieldValidator ID="valReqPayPalID" runat="server" ControlToValidate="txtPayPalID" Display="Dynamic" SetFocusOnError="true" resourcekey="valReqPayPalID" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
</div>
<div class="dnnFormItem">
	<dnn:label id="lblSecureID" runat="server" controlname="txtSecureID"></dnn:label>
	<asp:textbox id="txtSecureID" runat="server" MaxLength="50"></asp:textbox>
</div>
<div class="dnnFormItem">
	<dnn:label id="lblPayPalVerificationURL" runat="server" controlname="txtPayPalVerificationURL"></dnn:label>
	<asp:textbox id="txtPayPalVerificationURL" runat="server" MaxLength="255"></asp:textbox>
</div>
<div class="dnnFormItem">
	<dnn:label id="lblPayPalPaymentURL" runat="server" controlname="txtPayPalPaymentURL"></dnn:label>
	<asp:textbox id="txtPayPalPaymentURL" runat="server" MaxLength="255"></asp:textbox>
</div>
<div class="dnnFormItem">
	<dnn:label id="lblPayPalLanguage" runat="server" controlname="txtPayPalLanguage"></dnn:label>
	<asp:textbox id="txtPayPalLanguage" runat="server" MaxLength="2"></asp:textbox>
</div>
<div class="dnnFormItem">
	<dnn:label id="lblPayPalCharset" runat="server" controlname="txtPayPalCharset"></dnn:label>
	<asp:textbox id="txtPayPalCharset" runat="server" MaxLength="25"></asp:textbox>
</div>
<div class="dnnFormItem">
	<dnn:label id="lblPayPalButtonURL" runat="server" controlname="txtPayPalButtonURL"></dnn:label>
	<asp:textbox id="txtPayPalButtonURL" runat="server" MaxLength="255"></asp:textbox>
</div>
<div class="dnnFormItem">
	<dnn:label id="lblCurrency" runat="server" controlname="txtPayPalCurrency"></dnn:label>
	<asp:textbox id="txtPayPalCurrency" runat="server" MaxLength="3">USD</asp:textbox>
</div>
<div class="dnnFormItem">
	<dnn:label id="lblSurchargePercent" runat="server" controlname="txtSurchargePercent"></dnn:label>
	<asp:textbox id="txtSurchargePercent" runat="server">0</asp:textbox>
    <asp:RequiredFieldValidator ID="valReqSurchargePercent" runat="server" ControlToValidate="txtSurchargePercent" Display="Dynamic" SetFocusOnError="true" resourcekey="valReqSurchargePercent" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
    <asp:CompareValidator id="valCompSurchargePercent" runat="server" resourcekey="valCompSurchargePercent" Type="Currency" ControlToValidate="txtSurchargePercent" Operator="DataTypeCheck" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
</div>
<div class="dnnFormItem">
	<dnn:label id="lblSurchargeFixed" runat="server" controlname="txtSurchargeFixed"></dnn:label>
	<asp:textbox id="txtSurchargeFixed" runat="server">0</asp:textbox>
    <asp:RequiredFieldValidator ID="valReqSurchargeFixed" runat="server" ControlToValidate="txtSurchargeFixed" Display="Dynamic" SetFocusOnError="true" resourcekey="valReqSurchargeFixed" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
    <asp:CompareValidator id="valCompSurchargeFixed" runat="server" resourcekey="valCompSurchargeFixed" Type="Currency" ControlToValidate="txtSurchargeFixed" Operator="DataTypeCheck" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
</div>
