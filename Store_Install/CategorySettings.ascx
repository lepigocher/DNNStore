<%@ Control Language="c#" AutoEventWireup="True" Codebehind="CategorySettings.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.CategorySettings" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm">
    <fieldset>
        <div class="dnnFormItem">
            <dnn:label id="lblDisplayMode" controlname="cmbDisplayMode" runat="server"></dnn:label>
            <asp:dropdownlist id="cmbDisplayMode" runat="server" autopostback="True"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem" id="trColumnCount" runat="server">
            <dnn:label id="lblColumnCount" controlname="txtColumnCount" runat="server"></dnn:label>
            <asp:textbox id="txtColumnCount" Width="100" Runat="server" MaxLength="3"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:label id="lblCatalogPage" controlname="cmbCatalogPage" runat="server"></dnn:label>
            <asp:dropdownlist id="cmbCatalogPage" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
    </fieldset>
</div>
