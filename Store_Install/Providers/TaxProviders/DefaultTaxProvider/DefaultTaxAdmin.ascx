<%@ Control language="c#" CodeBehind="DefaultTaxAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.Providers.Tax.DefaultTaxProvider.DefaultTaxAdmin" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm">
    <h2 class="dnnFormSectionHead"><a href="#"><asp:Label ID="shTaxProvider" runat="server" ResourceKey="shTaxProvider">Tax Provider</asp:Label> </a></h2>
    <fieldset>
        <div class="dnnFormItem">
	        <dnn:label id="lblEnableTax" runat="server"></dnn:label>
	        <asp:CheckBox ID="cbEnableTax" runat="server" />
        </div>
        <div class="dnnFormItem">
		    <dnn:label id="lblTaxRate" runat="server" controlname="txtTaxRate"></dnn:label>
		    <asp:textbox id="txtTaxRate" runat="server" />
            <asp:CustomValidator ID="valCustTaxRate" runat="server" ControlToValidate="txtTaxRate" OnServerValidate="valCustTaxRate_ServerValidate" Display="Dynamic" SetFocusOnError="True" ResourceKey="valCustTaxRate" CssClass="dnnFormMessage dnnFormError" ValidateEmptyText="True"></asp:CustomValidator>
        </div>
        <ul class="dnnActions dnnClear">
            <li><asp:linkbutton id="btnSaveTaxRates" runat="server" CssClass="dnnPrimaryAction" resourcekey="btnSaveTax">Update Tax Settings</asp:linkbutton></li>
        </ul>
    </fieldset>
</div>
