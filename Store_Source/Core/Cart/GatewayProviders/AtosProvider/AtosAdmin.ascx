<%@ Control language="vb" CodeBehind="AtosAdmin.ascx.vb" Inherits="DotNetNuke.Modules.Store.Core.Cart.AtosAdmin" AutoEventWireup="false" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnFormItem">
	<dnn:label id="plMerchantId" runat="server" controlname="plMerchantId" suffix=":"></dnn:label>
	<asp:textbox id="txtMerchantIdValue" Runat="server"></asp:textbox>
    <asp:RequiredFieldValidator ID="reqMerchantIdValue" runat="server" ErrorMessage="reqMerchantIdValue" resourcekey="reqMerchantIdValue" ControlToValidate="txtMerchantIdValue" Display="Dynamic" SetFocusOnError="true" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
</div>
<div class="dnnFormItem">
	<dnn:label id="plPathFileDirectory" runat="server" controlname="plPathFileDirectory" suffix=":"></dnn:label>
	<asp:DropDownList ID="cbofolders" Runat="server" AutoPostBack="True"></asp:DropDownList>
    <asp:RequiredFieldValidator ID="reqcbofolders" runat="server" ErrorMessage="reqcbofolders" resourcekey="reqcbofolders" ControlToValidate="cbofolders" Display="Dynamic" SetFocusOnError="true" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
</div>
<div class="dnnFormItem">
	<asp:LinkButton ID="cmdLoadPathFile" Runat="server" CssClass="dnnSecondaryAction" resourcekey="cmdLoadPathFile"></asp:LinkButton>
</div>
<div class="dnnFormItem">
	<asp:Label ID="lblPathFileTitle" Runat="server" resourcekey="lblPathFileTitle"></asp:Label>
	<asp:Label ID="lblPathFileValue" Runat="server"></asp:Label>
</div>
<div class="dnnFormItem">
	<asp:TextBox ID="txtPathFile" Runat="server" TextMode="MultiLine" Rows="10" Columns="70" Wrap="False"></asp:TextBox>
</div>
<div class="dnnFormItem">
	<asp:LinkButton ID="cmdPathfile" Runat="server" CssClass="dnnSecondaryAction" resourcekey="cmdPathfile"></asp:LinkButton>
</div>
<div class="dnnFormItem">
	<dnn:label id="plBankImage" runat="server" controlname="cboFolderFiles" suffix=":"></dnn:label>
	<asp:DropDownList ID="cboFolderFiles" Runat="server"></asp:DropDownList>
</div>
