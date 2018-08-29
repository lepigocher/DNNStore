<%@ Control Language="c#" AutoEventWireup="True" Codebehind="ReviewEdit.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.ReviewEdit" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm">
    <fieldset>
        <div class="dnnFormItem">
            <dnn:label id="labelUserName" controlname="txtUserName" runat="server"></dnn:label>
            <asp:textbox id="txtUserName" Runat="server" MaxLength="50"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:label id="labelRating" controlname="cmbRating" runat="server"></dnn:label>
            <asp:dropdownlist id="cmbRating" Runat="server" Width="50" AutoPostBack="false">
                <asp:ListItem Value="5" Selected="True">5</asp:ListItem>
                <asp:ListItem Value="4">4</asp:ListItem>
                <asp:ListItem Value="3">3</asp:ListItem>
                <asp:ListItem Value="2">2</asp:ListItem>
                <asp:ListItem Value="1">1</asp:ListItem>
            </asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:label id="labelComments" controlname="txtComments" runat="server"></dnn:label>
            <asp:textbox id="txtComments" Runat="server" MaxLength="500" TextMode="MultiLine" Rows="5"></asp:textbox>
        </div>
        <div class="dnnFormItem" id="divAuthorized" runat="server" Visible="False">
            <dnn:label id="labelAuthorized" controlname="chkAuthorized" runat="server"></dnn:label>
            <asp:checkbox id="chkAuthorized" Runat="server"></asp:checkbox>
        </div>
    </fieldset>
    <ul class="dnnActions dnnClear">
        <li><asp:linkbutton id="cmdUpdate" runat="server" resourcekey="cmdUpdate" CssClass="dnnPrimaryAction">Update</asp:linkbutton></li>
        <li><asp:linkbutton id="cmdCancel" runat="server" resourcekey="cmdCancel" CssClass="dnnSecondaryAction" CausesValidation="False">Cancel</asp:linkbutton></li>
        <li><asp:linkbutton id="cmdDelete" runat="server" resourcekey="cmdDelete" CssClass="dnnSecondaryAction" CausesValidation="False" Visible="False">Delete</asp:linkbutton></li>
    </ul>
    <div class="dnnFormMessage dnnFormInfo" id="divApproval" runat="server"><asp:Label ID="labelApproval" runat="server" resourcekey="labelApproval" CssClass="StoreReviewApproval" Text="Approval Message" /></div>
</div>