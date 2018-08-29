<%@ Control Language="c#" AutoEventWireup="True" Codebehind="CategoryEdit.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.CategoryEdit" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<fieldset>
    <div class="dnnFormItem">
        <dnn:label id="labelCategoryName" runat="server" controlname="txtCategoryName"></dnn:label>
        <asp:TextBox id="txtCategoryName" Runat="server" MaxLength="50"></asp:TextBox>
        <asp:RequiredFieldValidator id="valReqCategoryName" runat="server" ControlToValidate="txtCategoryName" Display="Dynamic" ErrorMessage="* Category name is required." resourcekey="valReqCategoryName" SetFocusOnError="true" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
    </div>
    <div class="dnnFormItem" id="trSEOName" runat="server">
        <dnn:label id="labelSEOName" runat="server" controlname="txtSEOName"></dnn:label>
        <asp:TextBox id="txtSEOName" Runat="server" MaxLength="50"></asp:TextBox>
        <asp:linkbutton id="cmdSuggest" CssClass="dnnSecondaryAction" runat="server" CausesValidation="false" resourcekey="cmdSuggest" onclick="cmdSuggest_Click">Suggest</asp:linkbutton>
        <asp:RegularExpressionValidator ID="valRegExSEOName" runat="server" ErrorMessage="Invalid character(s)!" ControlToValidate="txtSEOName" SetFocusOnError="True" ValidationExpression="[_a-zA-Z0-9-]*" Display="Dynamic" CssClass="dnnFormMessage dnnFormError"></asp:RegularExpressionValidator>
    </div>
    <div class="dnnFormItem" id="trKeywords" runat="server">
        <dnn:label id="labelCategoryKeywords" runat="server" controlname="txtCategoryKeywords"></dnn:label>
        <asp:TextBox id="txtCategoryKeywords" Runat="server" Height="50" MaxLength="1000" TextMode="MultiLine"></asp:TextBox>
    </div>
    <div class="dnnFormItem">
        <dnn:label id="labelCategoryDescription" runat="server" controlname="txtDescription"></dnn:label>
        <asp:TextBox id="txtDescription" Runat="server" MaxLength="500"></asp:TextBox>
    </div>
    <div class="dnnFormItem">
        <dnn:label id="labelOrder" runat="server" controlname="txtOrder"></dnn:label>
        <asp:TextBox id="txtOrder" Runat="server" Width="50" MaxLength="5"></asp:TextBox>
        <asp:RequiredFieldValidator id="validatorRequireOrder" runat="server" ControlToValidate="txtOrder" Display="Dynamic" ErrorMessage="* Display Order is required." resourcekey="validatorRequireOrder" SetFocusOnError="true" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
        <asp:CompareValidator id="validatorOrder" runat="server" ErrorMessage="* Enter a valid display order." resourcekey="validatorOrder" Type="integer" ControlToValidate="txtOrder" Operator="DataTypeCheck" Display="Dynamic" SetFocusOnError="true" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
    </div>
    <div class="dnnFormItem">
        <dnn:label id="labelParentCategory" runat="server" controlname="ddlParentCategory"></dnn:label>
        <asp:DropDownList id="ddlParentCategory" runat="server"></asp:DropDownList>
        <asp:CustomValidator ID="valCustomParentCategory" runat="server" ControlToValidate="ddlParentCategory" ErrorMessage="Recurse !" OnServerValidate="valCustomParentCategory_ServerValidate" resourcekey="valCustomParentCategory" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:CustomValidator>
    </div>
    <div class="dnnFormItem">
        <dnn:label id="labelArchived" runat="server" controlname="chkArchived"></dnn:label>
        <asp:CheckBox id="chkArchived" Runat="server"></asp:CheckBox>
    </div>
    <div class="dnnFormItem">
        <dnn:label id="labelMessage" runat="server" controlname="txtMessage"></dnn:label>
        <dnn:TextEditor id="txtMessage" runat="server" height="350" width="100%"></dnn:TextEditor>
    </div>
</fieldset>
<ul class="dnnActions dnnClear">
    <li><asp:linkbutton id="cmdUpdate" CssClass="dnnPrimaryAction" runat="server" resourcekey="cmdUpdate" onclick="cmdUpdate_Click">Update</asp:linkbutton></li>
    <li><asp:linkbutton id="cmdCancel" CssClass="dnnSecondaryAction" runat="server" CausesValidation="False" resourcekey="cmdCancel" onclick="cmdCancel_Click">Cancel</asp:linkbutton></li>
    <li><asp:linkbutton id="cmdDelete" CssClass="dnnSecondaryAction" runat="server" CausesValidation="False" Visible="False" resourcekey="cmdDelete" onclick="cmdDelete_Click">Delete</asp:linkbutton></li>
</ul>
